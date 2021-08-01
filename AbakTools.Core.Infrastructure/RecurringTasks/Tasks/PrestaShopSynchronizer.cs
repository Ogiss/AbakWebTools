using AbakTools.Core.Infrastructure.PrestaShop;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    internal class PrestaShopSynchronizer
    {
        private readonly ILogger _logger;

        public PrestaShopSynchronizer(IConfiguration configuration, ILogger<PrestaShopSynchronizer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
        }

        public Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Processing export data to PrestaShop");
            var synchronizer = scope.ServiceProvider.GetRequiredService<IPrestaShopSynchronizeService>();
            synchronizer.DoWork(cancellationToken).Wait();

            return Task.CompletedTask;
        }
    }
}
