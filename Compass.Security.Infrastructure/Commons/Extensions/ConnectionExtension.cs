using System;
using Compass.Security.Application.Commons.Constants;
using Compass.Security.Domain.Entities;
using Compass.Security.Infrastructure.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class ConnectionExtension
    {
        public static void AddConnectionExtension(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationDbContext).Assembly.FullName;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(SettingConstant.ConnectionString,
                    b => b.MigrationsAssembly(assembly)
                ).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                options.UseOpenIddict<Domain.Entities.Application, ApplicationAuthorization, ApplicationScope, ApplicationToken, Guid>();
            });
        }
    }
}