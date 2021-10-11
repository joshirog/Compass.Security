using System;
using System.Threading;
using System.Threading.Tasks;
using Compass.Security.Infrastructure.Persistences.Seeders;
using Microsoft.Extensions.Hosting;

namespace Compass.Security.Infrastructure.Tasks
{
    public class SeederTask : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeederTask(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            IdentitySeeder.Seed(_serviceProvider);
            OpenIdSeeder.Seed(_serviceProvider);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}