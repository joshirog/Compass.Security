using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Infrastructure.Commons.Extensions;
using Compass.Security.Infrastructure.Persistences.Repositories;
using Compass.Security.Infrastructure.Services;
using Compass.Security.Infrastructure.Services.Externals.Google;
using Compass.Security.Infrastructure.Services.Internals.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddConnectionExtension();
            services.AddIdentityExtension();
            services.AddOpenIddictExtension();
            services.AddCacheExtension();
            services.AddTaskExtension();
            services.AddHttpClientExtension();
            services.AddAccessorExtension();
            services.AddOAuthExtension();
            
            services.AddTransient<IDateTimeService, DateTimeService>();
            
            services.AddScoped<IBlacklistRepository, BlacklistRepository>();
            
            services.AddScoped<IIdentityService, IdentityService>();
            
            services.AddScoped<ICaptchaService, GoogleCaptchaService>();
            
            return services;
        }
    }
}