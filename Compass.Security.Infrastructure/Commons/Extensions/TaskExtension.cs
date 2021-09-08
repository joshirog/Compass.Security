using Compass.Security.Infrastructure.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class TaskExtension
    {
        public static void AddTaskExtension(this IServiceCollection services)
        {
            services.AddHostedService<SeederTask>();
        }
    }
}