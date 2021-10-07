using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Infrastructure.Commons.Extensions;
using Compass.Security.Infrastructure.Persistences.Repositories;
using Compass.Security.Infrastructure.Services;
using Compass.Security.Infrastructure.Services.Externals.Google;
using Compass.Security.Infrastructure.Services.Externals.SendInBlue;
using Compass.Security.Infrastructure.Services.Internals.Identity;
using Compass.Security.Infrastructure.Services.Internals.LazyCache;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddAccessorExtension();
            services.AddConnectionExtension();
            services.AddIdentityExtension();
            services.AddOpenIddictExtension();
            services.AddCacheExtension();
            services.AddTaskExtension();
            services.AddHttpClientExtension();
            services.AddOAuthExtension();
            
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<ICacheService, LazyCacheService>();
            services.AddTransient<INotificationService, SendInBlueService>();
            services.AddTransient<IStorageService, FirebaseStorageService>();
            services.AddTransient<ICaptchaService, GoogleCaptchaService>();
            services.AddTransient<IIdentityService, IdentityService>();
            
            services.AddScoped<IBlacklistRepository, BlacklistRepository>();

            return services;
        }
    }
}