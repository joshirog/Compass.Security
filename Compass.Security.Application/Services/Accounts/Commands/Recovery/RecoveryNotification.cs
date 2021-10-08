using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using MediatR;

namespace Compass.Security.Application.Services.Accounts.Commands.Recovery
{
    public class RecoveryNotification : INotification
    {
        public Guid UserId { get; set; }
        
        public string Name { get; set; }

        public string Email { get; set; }

        public string Callback { get; set; }
    }
    
    public class RecoveryNotificationHandler : INotificationHandler<RecoveryNotification>
    {
        private readonly ICacheService _cacheService;
        private readonly INotificationLogService _notificationLogService;

        public RecoveryNotificationHandler(ICacheService cacheService, INotificationLogService notificationLogService)
        {
            _cacheService = cacheService;
            _notificationLogService = notificationLogService;
        }
        
        public async Task Handle(RecoveryNotification notification, CancellationToken cancellationToken)
        {
            var html = _cacheService.Template(TemplateConstant.TemplateReset);
            html = html.Replace("{0}", notification.Callback);
            
            await _notificationLogService.SendMailLog(notification.UserId, new EmailDto
            {
                Sender = new SenderDto { Id = TemplateConstant.SenderId },
                To = new List<ToDto> { new() { Name = notification.Name, Email = notification.Email } },
                Subject = TemplateConstant.SubjectRecovery,
                HtmlContent = html,
                TextContent = TemplateConstant.SubjectRecovery
            });
        }
    }
}