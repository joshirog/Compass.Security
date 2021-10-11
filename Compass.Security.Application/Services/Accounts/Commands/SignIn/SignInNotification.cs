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

namespace Compass.Security.Application.Services.Accounts.Commands.SignIn
{
    public class SignInNotification : INotification
    {
        public string Username { get; set; }
    }
    
    public class SignInNotificationHandler : INotificationHandler<SignInNotification>
    {
        private readonly ICacheService _cacheService;
        private readonly IIdentityService _identityService;
        private readonly INotificationLogService _notificationLogService;
        private readonly IHttpContextAccessor _accessor;
        
        public SignInNotificationHandler(ICacheService cacheService, IIdentityService identityService, INotificationLogService notificationLogService, IHttpContextAccessor accessor)
        {
            _cacheService = cacheService;
            _identityService = identityService;
            _notificationLogService = notificationLogService;
            _accessor = accessor;
        }
        
        public async Task Handle(SignInNotification notification, CancellationToken cancellationToken)
        {
            var (user, claims) = await _identityService.GetUserAndClaims(notification.Username);

            if (!string.IsNullOrEmpty(user.Email))
            {
                var link = $"{_accessor.HttpContext?.Request.Scheme}://{_accessor.HttpContext?.Request.Host}";
                var firstname = claims.Where(x => x.Type.Equals(ClaimTypeConstant.Firstname)).Select(x => x.Value).FirstOrDefault();
                
                var html = _cacheService.Template(TemplateConstant.TemplateLocked);
                html = html.Replace("{0}", link);

                await _notificationLogService.SendMailLog(user.Id, new EmailDto
                {
                    Sender = new SenderDto { Id = TemplateConstant.SenderId },
                    To = new List<ToDto> { new() { Name = firstname, Email = user.Email } },
                    Subject = TemplateConstant.SubjectLockedAccount,
                    HtmlContent = html,
                    TextContent = TemplateConstant.SubjectLockedAccount
                }, NotificationTypeEnum.Locked);
            }
        }
    }
}