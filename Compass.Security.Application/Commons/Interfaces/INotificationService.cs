using System.Threading.Tasks;
using Compass.Security.Application.Commons.Dtos;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface INotificationService
    {
        Task<string> SendEmail(EmailDto entity);
    }
}