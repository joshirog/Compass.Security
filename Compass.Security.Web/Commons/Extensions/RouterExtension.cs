using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Web.Commons.Extensions
{
    public static class RouterExtension
    {
        public static void AddRouterExtension(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
        }
    }
}