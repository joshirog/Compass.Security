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

namespace Compass.Security.Application.Services.Accounts.Commands.Reset
{
    public class ResetNotification : INotification
    {
        public string UserId { get; set; }
    }
    
    public class ResetNotificationHandler : INotificationHandler<ResetNotification>
    {
        private readonly ICacheService _cacheService;
        private readonly INotificationLogService _notificationLogService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;

        public ResetNotificationHandler(ICacheService cacheService, INotificationLogService notificationLogService, IHttpContextAccessor accessor, IUserRepository userRepository, IIdentityService identityService)
        {
            _cacheService = cacheService;
            _notificationLogService = notificationLogService;
            _accessor = accessor;
            _userRepository = userRepository;
            _identityService = identityService;
        }

        public async Task Handle(ResetNotification notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(new Guid(notification.UserId));

            var claims = await _identityService.GetClaims(user);
            var firstname = claims.Where(x => x.Type.Equals(ClaimTypeConstant.Firstname)).Select(x => x.Value).FirstOrDefault();
            
            var link = $"{_accessor.HttpContext?.Request.Scheme}://{_accessor.HttpContext?.Request.Host}";
            
            var html = _cacheService.Template(TemplateConstant.TemplatePassword);
            html = html.Replace("{0}", link);
            
            await _notificationLogService.SendMailLog(user.Id ,new EmailDto
            {
                Sender = new SenderDto { Id = TemplateConstant.SenderId },
                To = new List<ToDto> { new() { Name = firstname, Email = user.Email } },
                Subject = TemplateConstant.SubjectReset,
                HtmlContent = html,
                TextContent = TemplateConstant.SubjectReset,
            }, NotificationTypeEnum.Reset);
        }
    }
}