using Compass.Security.Web.Commons.Filters;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Web.Commons.Extensions
{
    public static class ControllerExtension
    {
        public static void AddControllerExtension(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
                    options.Filters.Add<ValidationFilter>())
                .AddFluentValidation()
                .AddRazorRuntimeCompilation();
        }
    }
}