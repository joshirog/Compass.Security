using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Enums;
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
        private readonly IUserRepository _userRepository;

        public IdentityService(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }
        
        public async Task<(bool, bool, User)> SignIn(string username, string password, bool isPersistent, bool isLockOnFailed)
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
                return (result.RequiresTwoFactor, result.RequiresTwoFactor, user);
            }

            if (!result.Succeeded)
                throw new ErrorInvalidException(new[] { "Incorrect email or password, please check and try again." });
            
            await _userManager.ResetAccessFailedCountAsync(user);
                
            return (result.Succeeded, result.RequiresTwoFactor, user);
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
                        EmailConfirmed = true,
                        Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active)
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
                        EmailConfirmed = true,
                        Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active)
                    };
                }
            }

            if (isCreated)
            {
                await _userManager.CreateAsync(user);
                    
                await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleEnum), RoleEnum.Guest));
            
                await _userManager.AddClaimsAsync(user, new List<Claim>
                {
                    new("firstname", "firstname", ClaimValueTypes.String),
                    new("lastname", "lastname", ClaimValueTypes.String),
                    new("avatar", ConfigurationConstant.Avatar, ClaimValueTypes.String)
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
        
        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(new Guid(userId));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return result.Succeeded;
            
            throw new ErrorInvalidException(result.Errors?.Select(x => x.Description));
        }
        
        public async Task<bool> TwoFactor(string code)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            
            if(user is null)
                throw new ErrorInvalidException(new []{ "The code is incorrect, please check, or generate a new authentication code." });
            
            var result = await _signInManager.TwoFactorSignInAsync("Email", code, true, false);

            if (result.IsNotAllowed)
            {
                throw new ErrorInvalidException(new []{ "We sent a verification email to activate your account, please check your email." });
            }

            if (result.IsLockedOut)
            {
                throw new ErrorInvalidException(new []{ "It seems that you have exceeded the maximum number of attempts, please try again later." });
            }

            if (result.RequiresTwoFactor)
            {
                throw new ErrorInvalidException(new []{ "Wrong token, please try again." });
            }

            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                
                await _userManager.UpdateSecurityStampAsync(user);
                
                return result.Succeeded;
            }

            throw new ErrorInvalidException(new []{ "The code is incorrect, please check, or generate a new authentication code." });
        }
        
        public async Task<bool> ResetPassword(string userId, string token, string password)
        {
            var user = await _userRepository.GetByIdAsync(new Guid(userId));

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (result.Succeeded)
                return result.Succeeded;
            
            throw new ErrorInvalidException(result.Errors?.Select(x => x.Description));
        }
        
        public async Task<(User, List<Claim>)> GetUserAndClaims(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            
            return (user, (await _userManager.GetClaimsAsync(user)).ToList());
        }

        public async Task<List<Claim>> GetClaims(User user)
        {
            return (await _userManager.GetClaimsAsync(user)).ToList();
        }
        
        public async Task<string> TokenConfirm(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> TokenTwoFactor(User user, string provider)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            
            return await _userManager.GenerateTwoFactorTokenAsync(user, provider);
        }
        
        public async Task<string> TokenPassword(User user)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
    }
}