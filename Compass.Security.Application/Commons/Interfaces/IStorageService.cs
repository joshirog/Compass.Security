using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface IStorageService
    {
        Task<string> Upload(IFormFile file);
    }
}