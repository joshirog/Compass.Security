using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Compass.Security.Infrastructure.Services.Internals.Identity
{
    public class IdentityEmailToken<T> : DataProtectorTokenProvider<T> where T : class
    {
        public IdentityEmailToken(IDataProtectionProvider provider, IOptions<IdentityEmailTokenOptions> options, ILogger<DataProtectorTokenProvider<T>> logger)
            : base(provider, options, logger)
        {

        }
    }

    public class IdentityEmailTokenOptions : DataProtectionTokenProviderOptions
    {

    }
}