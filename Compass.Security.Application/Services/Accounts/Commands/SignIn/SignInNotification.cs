using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;

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

        public SignInNotificationHandler(ICacheService cacheService, IIdentityService identityService, INotificationLogService notificationLogService)
        {
            _cacheService = cacheService;
            _identityService = identityService;
            _notificationLogService = notificationLogService;
        }
        
        public async Task Handle(SignInNotification notification, CancellationToken cancellationToken)
        {
            var (user, claims) = await _identityService.GetUserAndClaims(notification.Username);

            if (!string.IsNullOrEmpty(user.Email))
            {
                var firstname = claims.Where(x => x.Type.Equals("firstname")).Select(x => x.Value).FirstOrDefault();
                
                /* TODO: lock user account notification
                var html = _cacheService.Template(TemplateConstant.TemplateActivation);
                html = html.Replace("{0}", fullName);
                html = html.Replace("{1}", link);
                */

                await _notificationLogService.SendMailLog(user.Id, new EmailDto
                {
                    Sender = new SenderDto { Id = TemplateConstant.SenderId },
                    To = new List<ToDto> { new() { Name = firstname, Email = user.Email } },
                    Subject = TemplateConstant.SubjectLockedAccount,
                    HtmlContent = "<h1>SubjectLockedAccount</h1>",
                    TextContent = TemplateConstant.SubjectLockedAccount
                });
            }
        }
    }
}