using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Infrastructure.Commons.Extensions;
using Compass.Security.Infrastructure.Persistences.Repositories;
using Compass.Security.Infrastructure.Services;
using Compass.Security.Infrastructure.Services.Internals.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
            
            services.AddScoped<IBlacklistRepository, BlacklistRepository>();
            
            services.AddScoped<IIdentityService, IdentityService>();
            
            services.AddConnectionExtension();
            
            services.AddIdentityExtension();

            services.AddTaskExtension();
            
            return services;
        }
    }
}