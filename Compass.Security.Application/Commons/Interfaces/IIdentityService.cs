using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Compass.Security.Domain.Entities;
using Microsoft.AspNetCore.Authentication;

namespace Compass.Security.Application.Commons.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool, bool, User)> SignIn(string username, string password, bool isPersistent, bool isLockOnFailed);
        
        Task<List<AuthenticationScheme>> Schemes();
        
        AuthenticationProperties Properties(string provider, string redirectUrl);
        
        Task<bool> CallBack();
        
        Task<(bool, User)> SignUp(User user, string password, IEnumerable<Claim> claims);
        
        Task SignOut();
        
        Task<bool> ConfirmEmail(string userId, string token);

        Task<bool> TwoFactor(string code);

        Task<bool> ResetPassword(string userId, string token, string password);

        Task<(User, List<Claim>)> GetUserAndClaims(string username);
        
        Task<List<Claim>> GetClaims(User user);

        Task<string> TokenConfirm(User user);
        
        Task<string> TokenPassword(User user);

        Task<string> TokenTwoFactor(User user, string provider);
    }
}