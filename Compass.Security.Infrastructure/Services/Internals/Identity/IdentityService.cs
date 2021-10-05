using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Commons.Enums;
using Compass.Security.Domain.Entities;
using Compass.Security.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
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
        
        public async Task<List<AuthenticationScheme>> Schemes()
        {
            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        
        public AuthenticationProperties Properties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }
        
        public async Task<bool> CallBack()
        {
             var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                throw new ErrorInvalidException(new []{ "Oops, an error occurred in the communication with the external provider, please try it in a few minutes." });
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);

            if (signInResult.Succeeded)
            {
                return signInResult.Succeeded;
            }
            
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (email is null && identifier is null)
            {
                throw new ErrorInvalidException(new []{ "Oops, an error occurred in the communication with the external provider, please try it in a few minutes." });
            }

            User user;
            var isCreated = false;
            
            if (email is not null)
            {
                user = await _userManager.FindByEmailAsync(email);
                
                if (user is null)
                {
                    isCreated = true;
                    user = new User
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };
                }
            }
            else
            {
                user = await _userManager.FindByNameAsync(identifier);
            
                if (user is null)
                {
                    isCreated = true;
                    user = new User
                    {
                        UserName = identifier,
                        EmailConfirmed = true
                    };
                }
            }

            if (isCreated)
            {
                await _userManager.CreateAsync(user);
                    
                await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleEnum), RoleEnum.Guest));
            
                await _userManager.AddClaimsAsync(user, new List<Claim>
                {
                    new("FullName", "", ClaimValueTypes.String),
                    new("Avatar", ConfigurationConstant.Avatar, ClaimValueTypes.String)
                });
            }

            var identity = await _userManager.AddLoginAsync(user, info);
            
            await _signInManager.SignInAsync(user, false);
            
            return identity.Succeeded;
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