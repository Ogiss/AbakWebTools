using AbakTools.Core.Infrastructure.PrestaShop;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    //internal class PrestaShopOrderSynchronizer : RecurringTaskBase
    internal class PrestaShopOrderSynchronizer
    {
        private readonly ILogger _logger;

        public PrestaShopOrderSynchronizer(
            IConfiguration configuration,
            ILogger<PrestaShopOrderSynchronizer> logger,
            IServiceProvider serviceProvider) 
            //: base(configuration, logger, serviceProvider)
        {
            _logger = logger;
        }

        //protected override Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        public Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Processing synchronize orders from PrestaShop");
            var synchronizer = scope.ServiceProvider.GetRequiredService<IPrestaShopSynchronizeOrder>();
            synchronizer.DoWork(cancellationToken).Wait();

            return Task.CompletedTask;
        }
    }
}
