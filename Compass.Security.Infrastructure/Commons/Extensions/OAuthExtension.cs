using Compass.Security.Application.Commons.Constants;
using Compass.Security.Infrastructure.Commons.Constants;
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
                })
                .AddFacebook(options =>
                {
                    options.AppId = ConfigurationConstant.FacebookAuthKey;
                    options.AppSecret = ConfigurationConstant.FacebookAuthSecret;
                    options.AccessDeniedPath = SettingConstant.AccessDeniedPathInfo;
                })
                .AddTwitter(options =>
                {
                    options.ConsumerKey = ConfigurationConstant.TwitterAuthKey;
                    options.ConsumerSecret = ConfigurationConstant.TwitterAuthSecret;
                    options.RetrieveUserDetails = SettingConstant.RetrieveUserDetails;
                })
                .AddLinkedIn(options =>
                {
                    options.ClientId = ConfigurationConstant.LinkedinAuthKey;
                    options.ClientSecret = ConfigurationConstant.LinkedinAuthSecret;
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = ConfigurationConstant.MicrosoftAuthKey;
                    options.ClientSecret = ConfigurationConstant.MicrosoftAuthSecret;
                });
        }
    }
}