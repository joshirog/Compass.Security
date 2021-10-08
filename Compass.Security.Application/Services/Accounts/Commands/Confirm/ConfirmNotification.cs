using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Compass.Security.Application.Services.Accounts.Commands.Confirm
{
    public class ConfirmNotification : INotification
    {
        public string UserId { get; set; }
    }
    
    public class ConfirmNotificationHandler : INotificationHandler<ConfirmNotification>
    {
        private readonly ICacheService _cacheService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;
        private readonly INotificationLogService _notificationLogService;

        public ConfirmNotificationHandler(ICacheService cacheService, IHttpContextAccessor accessor, IUserRepository userRepository, IIdentityService identityService, INotificationLogService notificationLogService)
        {
            _cacheService = cacheService;
            _accessor = accessor;
            _userRepository = userRepository;
            _identityService = identityService;
            _notificationLogService = notificationLogService;
        }

        public async Task Handle(ConfirmNotification notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(new Guid(notification.UserId));

            var claims = await _identityService.GetClaims(user);
            var fullName = claims.Where(x => x.Type.Equals("FullName")).Select(x => x.Value).FirstOrDefault();
            
            var link = $"{_accessor.HttpContext?.Request.Scheme}://{_accessor.HttpContext?.Request.Host}";
            
            var html = _cacheService.Template(TemplateConstant.TemplateWelcome);
            html = html.Replace("{0}", fullName);
            html = html.Replace("{1}", link);
            
            await _notificationLogService.SendMailLog(user.Id, new EmailDto
            {
                Sender = new SenderDto { Id = TemplateConstant.SenderId },
                To = new List<ToDto> { new() { Name = fullName, Email = user.Email } },
                Subject = TemplateConstant.SubjectWelcome,
                HtmlContent = html,
                TextContent =  TemplateConstant.SubjectWelcome,
            });
        }
    }
}