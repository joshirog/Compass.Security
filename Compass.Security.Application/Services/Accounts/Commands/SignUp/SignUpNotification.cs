using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Compass.Security.Application.Services.Accounts.Commands.SignUp
{
    public class SignUpNotification : INotification
    {
        public string UserName { get; set; }

        public string ReturnUrl { get; set; }  
    }
    
     public class SignUpNotificationHandler : INotificationHandler<SignUpNotification>
    {
        private readonly ICacheService _cacheService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IIdentityService _identityService;
        private readonly INotificationLogService _notificationLogService;
        
        public SignUpNotificationHandler(ICacheService cacheService, IHttpContextAccessor accessor, IIdentityService identityService, INotificationLogService notificationLogService)
        {
            _cacheService = cacheService;
            _accessor = accessor;
            _identityService = identityService;
            _notificationLogService = notificationLogService;
        }
        
        public async Task Handle(SignUpNotification notification, CancellationToken cancellationToken)
        {
            var (user, claims) = await _identityService.GetUserAndClaims(notification.UserName);

            if (!string.IsNullOrEmpty(user.Email))
            {
                var fullName = claims.Where(x => x.Type.Equals("First")).Select(x => x.Value).FirstOrDefault();

                var token = await _identityService.TokenConfirm(user);
                var tokenEncodedBytes = Encoding.UTF8.GetBytes(token);
                var tokenEncoded = WebEncoders.Base64UrlEncode(tokenEncodedBytes);
            
                var link = $"{_accessor.HttpContext?.Request.Scheme}://{_accessor.HttpContext?.Request.Host}/account/confirm/{user.Id}?token={tokenEncoded}&returnUrl={notification.ReturnUrl}";
            
                Console.WriteLine(link);

                var html = _cacheService.Template(TemplateConstant.TemplateActivation);
                html = html.Replace("{0}", fullName);
                html = html.Replace("{1}", link);

                await _notificationLogService.SendMailLog(user.Id, new EmailDto
                {
                    Sender = new SenderDto { Id = TemplateConstant.SenderId },
                    To = new List<ToDto> { new() { Name = fullName, Email = user.Email } },
                    Subject = TemplateConstant.SubjectActivateAccount,
                    HtmlContent = html,
                    TextContent = TemplateConstant.SubjectActivateAccount
                });
            }
        }
    }
}