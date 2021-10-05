using System.Threading.Tasks;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface ICaptchaService
    {
        Task<bool> SiteVerify(string captcha);
    }
}