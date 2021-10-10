using System.Threading.Tasks;
using Compass.Security.Application.Commons.Dtos;
using Compass.Security.Application.Commons.Interfaces;

namespace Compass.Security.Infrastructure.Services.Externals.Mailgun
{
    public class MailgunService : INotificationService
    {
        public Task<string> SendEmail(EmailDto entity)
        {
            throw new System.NotImplementedException();
        }
    }
}