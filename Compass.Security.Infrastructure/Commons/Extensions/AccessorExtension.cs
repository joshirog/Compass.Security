using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class AccessorExtension
    {
        public static void AddAccessorExtension(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
    }
}