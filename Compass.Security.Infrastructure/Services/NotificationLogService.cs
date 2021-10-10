using System;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Entities;
using Compass.Security.Domain.Enums;
using Compass.Security.Domain.Exceptions;

namespace Compass.Security.Infrastructure.Services
{
    public class NotificationLogService : INotificationLogService
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly INotificationLogRepository _notificationLogRepository;
        private readonly INotificationService _notificationService;
        
        public NotificationLogService(IUserNotificationRepository userNotificationRepository, INotificationLogRepository notificationLogRepository, INotificationService notificationService)
        {
            _userNotificationRepository = userNotificationRepository;
            _notificationLogRepository = notificationLogRepository;
            _notificationService = notificationService;
        }
        
        public async Task<bool> SendMailLog(Guid userId, EmailDto email, NotificationTypeEnum type)
        {
            if (!type.Equals(NotificationTypeEnum.None))
            {
                var userNotification = await _userNotificationRepository.GetFilterAsync(x => 
                    x.UserId.Equals(userId) &&
                    x.Type.Equals(type));
            
                userNotification.Counter += 1;

                await _userNotificationRepository.UpdateAsync(userNotification);
            }
            
            var identifier = await _notificationService.SendEmail(email);

            var result = await _notificationLogRepository.InsertAsync(new NotificationLog
            {
                UserId = userId,
                Type = Enum.GetName(typeof(NotificationLogTypeEnum), NotificationLogTypeEnum.Email),
                Identifier = identifier
            });

            return result is not null;
        }
    }
}