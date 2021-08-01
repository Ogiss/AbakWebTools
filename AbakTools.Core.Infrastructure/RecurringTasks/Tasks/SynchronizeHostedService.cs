using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    class SynchronizeHostedService : RecurringTaskBase
    {
        public SynchronizeHostedService(
            IConfiguration configuration,
            ILogger<SynchronizeHostedService> logger,
            IServiceProvider serviceProvider) : base(configuration, logger, serviceProvider)
        {
        }

        protected override Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            Logger.LogDebug("Starting synchronization");

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<EnovaSynchronizer>().DoWorkAsync(scope, cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<PrestaShopSynchronizer>().DoWorkAsync(scope, cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<PrestaShopOrderSynchronizer>().DoWorkAsync(scope, cancellationToken).Wait();

            return Task.CompletedTask;
        }
    }
}
