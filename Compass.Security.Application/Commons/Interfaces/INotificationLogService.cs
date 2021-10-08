using System;
using System.Threading.Tasks;
using Compass.Security.Application.Commons.Dtos;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface INotificationLogService
    {
        Task<bool> SendMailLog(Guid userId, EmailDto email);
    }
}