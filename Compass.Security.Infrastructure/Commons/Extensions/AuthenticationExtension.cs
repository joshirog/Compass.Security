using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class AuthenticationExtension
    {
        public static void AddAuthenticationExtension(this IServiceCollection services)
        {
            /*
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = SettingConstant.GoogleAuthKey;
                    options.ClientSecret = SettingConstant.GoogleAuthSecret;
                })
                .AddFacebook(options =>
                {
                    options.AppId = SettingConstant.FacebookAuthKey;
                    options.AppSecret = SettingConstant.FacebookAuthSecret;
                    options.AccessDeniedPath = "/AccessDeniedPathInfo";
                })
                .AddTwitter(options =>
                {
                    options.ConsumerKey = SettingConstant.TwitterAuthKey;
                    options.ConsumerSecret = SettingConstant.TwitterAuthSecret;
                    options.RetrieveUserDetails = true;
                })
                .AddLinkedIn(options =>
                {
                    options.ClientId = SettingConstant.LinkedinAuthKey;
                    options.ClientSecret = SettingConstant.LinkedinAuthSecret;
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = "4ab012b8-40ae-4336-9ba6-baae530dbe73";
                    options.ClientSecret = ".smC7~2A_Do_15U7F9Dtm8I~Z4P3S4TN56";
                });
            */
        }
    }
}