using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class InjectionExtension
    {
        public static void AddInjectionExtension(this IServiceCollection services)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
        }
    }
}