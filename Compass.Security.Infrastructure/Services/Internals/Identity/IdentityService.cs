using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Commons.Enums;
using Compass.Security.Domain.Entities;
using Compass.Security.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Compass.Security.Infrastructure.Services.Internals.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        public async Task<(bool, User)> SignIn(string username, string password, bool isPersistent, bool isLockOnFailed)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent, isLockOnFailed);
            
            if (result.IsNotAllowed)
            {
                throw new ErrorInvalidException(new []{ "We sent a verification email to activate your account, please check your email." });
            }

            if (result.IsLockedOut)
            {
                throw new ErrorInvalidException(new []{ "It seems that you have exceeded the maximum number of attempts, please try again later." });
            }
            
            var user = await _userManager.FindByNameAsync(username);

            if (result.RequiresTwoFactor)
            {
                return (result.RequiresTwoFactor, user);
            }

            if (!result.Succeeded)
                throw new ErrorInvalidException(new[] { "Incorrect email or password, please check and try again." });
            
            await _userManager.ResetAccessFailedCountAsync(user);
                
            return (result.Succeeded, user);
        }

        public async Task<(bool, User)> SignUp(User user, string password, IEnumerable<Claim> claims)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var identityResult = await _userManager.CreateAsync(user, password);
            
            if (!identityResult.Succeeded)
                throw new ErrorInvalidException(identityResult.Errors?.Select(x => x.Description));

            identityResult = await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleEnum), RoleEnum.Guest));

            if (!identityResult.Succeeded)
                throw new ErrorInvalidException(identityResult.Errors?.Select(x => x.Description));

            identityResult = await _userManager.AddClaimsAsync(user, claims);

            if (!identityResult.Succeeded)
                throw new ErrorInvalidException(identityResult.Errors?.Select(x => x.Description));
                
            transaction.Complete();
                
            return (identityResult.Succeeded, user);
        }
        
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}