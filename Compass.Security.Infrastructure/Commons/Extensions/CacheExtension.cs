using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class CacheExtension
    {
        public static void AddCacheExtension(this IServiceCollection services)
        {
            services.AddLazyCache();
        }
    }
}