using System;
using Compass.Security.Domain.Entities;
using Compass.Security.Infrastructure.Persistences.Contexts;
using Compass.Security.Infrastructure.Services.Internals.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class IdentityExtension
    {
        public static void AddIdentityExtension(this IServiceCollection services)
        {
            services
                .AddIdentity<User, Role>(
                    options =>
                    {
                        options.User.RequireUniqueEmail = false;
                        options.Lockout.AllowedForNewUsers = true;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        options.Lockout.MaxFailedAccessAttempts = 3;
                        options.Password.RequiredLength = 8;
                        options.Password.RequiredUniqueChars = 1;
                        options.Password.RequireDigit = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.Password.RequireUppercase = true;
                        options.SignIn.RequireConfirmedEmail = true;
                        options.Tokens.EmailConfirmationTokenProvider = "1EpnLoHHgYt7kutpAklvAMDeQPXJjei2";
                    })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityCustomError>()
                .AddTokenProvider<IdentityEmailToken<User>>("1EpnLoHHgYt7kutpAklvAMDeQPXJjei2")
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(5));
        }
    }
}