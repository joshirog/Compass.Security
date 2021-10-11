using Compass.Security.Application.Commons.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class OAuthExtension
    {
        public static void AddOAuthExtension(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = ConfigurationConstant.GoogleAuthKey;
                    options.ClientSecret = ConfigurationConstant.GoogleAuthSecret;
                });
        }
    }
}