using AbakTools.Core.Infrastructure.PrestaShop;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    internal class PrestaShopSynchronizer : RecurringTaskBase
    {
        private readonly ILogger _logger;

        public PrestaShopSynchronizer(IConfiguration configuration, ILogger<PrestaShopSynchronizer> logger, IServiceProvider serviceProvider) : base(configuration, logger, serviceProvider)
        {
            _logger = logger;
        }

        protected override async Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Processing synchronize with PrestaShop");
            var synchronizer = scope.ServiceProvider.GetRequiredService<IPrestaShopSynchronizeService>();
            await synchronizer.DoWork(cancellationToken);
        }
    }
}
