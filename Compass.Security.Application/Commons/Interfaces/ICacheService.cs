using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface ICacheService
    {
        string Template(string key);

        Task<List<AuthenticationScheme>> ExternalLogin();

        void Remove(string key);
    }
}