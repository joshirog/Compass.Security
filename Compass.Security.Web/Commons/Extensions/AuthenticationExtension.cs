using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Web.Commons.Extensions
{
    public static class AuthenticationExtension
    {
        public static void AddAuthenticationExtension(this IServiceCollection services)
        {
            services
                .ConfigureApplicationCookie(
                    options =>
                    {
                        options.LoginPath = new PathString("/account/signin");
                        options.LogoutPath = new PathString("/account/signout");
                        options.AccessDeniedPath = new PathString("/denied");
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                        options.SlidingExpiration = true;
                    });
        }
    }
}