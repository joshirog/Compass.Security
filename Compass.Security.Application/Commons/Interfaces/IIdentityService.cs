using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Compass.Security.Domain.Entities;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool, User)> SignIn(string username, string password, bool isPersistent, bool isLockOnFailed);
        
        Task<(bool, User)> SignUp(User user, string password, IEnumerable<Claim> claims);
        
        Task SignOut();
    }
}