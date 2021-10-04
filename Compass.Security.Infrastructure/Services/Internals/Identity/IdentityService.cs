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

        public IdentityService(UserManager<User> userManager)
        {
            _userManager = userManager;
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
    }
}