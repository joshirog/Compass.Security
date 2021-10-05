using Compass.Security.Web.Commons.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllerExtension();
            services.AddRouterExtension();
            services.AddAuthenticationExtension();
            
            return services;
        }
    }
}