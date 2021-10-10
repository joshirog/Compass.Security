using System;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Domain.Enums;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface INotificationLogService
    {
        Task<bool> SendMailLog(Guid userId, EmailDto email, NotificationTypeEnum type);
    }
}