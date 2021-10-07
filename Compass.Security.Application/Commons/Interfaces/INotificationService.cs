using System.Threading.Tasks;
using Compass.Security.Application.Commons.Dtos;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface INotificationService
    {
        Task SendEmail(EmailDto entity);
    }
}