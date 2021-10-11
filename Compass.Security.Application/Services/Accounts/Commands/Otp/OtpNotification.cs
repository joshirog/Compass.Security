using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Compass.Security.Application.Services.Accounts.Commands.Otp
{
    public class OtpNotification : INotification
    {
        public Guid UserId { get; set; }

        public string ReturnUrl { get; set; }
    }
    
    public class OtpNotificationHandler : INotificationHandler<OtpNotification>
    {
        private readonly ICacheService _cacheService;
        private readonly INotificationLogService _notificationLogService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IIdentityService _identityService;
        
        public OtpNotificationHandler(ICacheService cacheService, INotificationLogService notificationLogService, IUserRepository userRepository, IHttpContextAccessor accessor, IIdentityService identityService)
        {
            _cacheService = cacheService;
            _notificationLogService = notificationLogService;
            _userRepository = userRepository;
            _accessor = accessor;
            _identityService = identityService;
        }
        
        public async Task Handle(OtpNotification notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(notification.UserId);
            var claims = await _identityService.GetClaims(user);
            var firstname = claims.Where(x => x.Type.Equals(ClaimTypeConstant.Firstname)).Select(x => x.Value).FirstOrDefault();

            var code = await _identityService.TokenTwoFactor(user, "Email");

            var link = $"{_accessor.HttpContext?.Request.Scheme}://{_accessor.HttpContext?.Request.Host}/account/otp?returnUrl={notification.ReturnUrl}";
            
            Console.WriteLine(link);

            var html = _cacheService.Template(TemplateConstant.TemplateOtp);
            html = html.Replace("{0}", code);
            html = html.Replace("{1}", link);
            
            await _notificationLogService.SendMailLog(user.Id, new EmailDto
            {
                Sender = new SenderDto { Id = TemplateConstant.SenderId },
                To = new List<ToDto> { new() { Name = firstname, Email = user.Email } },
                Subject = TemplateConstant.SubjectTwoFactor,
                HtmlContent = html,
                TextContent = TemplateConstant.SubjectTwoFactor
            }, NotificationTypeEnum.TwoFactor);
        }
    }
}