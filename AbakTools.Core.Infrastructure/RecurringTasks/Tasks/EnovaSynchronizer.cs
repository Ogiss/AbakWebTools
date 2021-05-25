using AbakTools.Core.Infrastructure.Enova.Importers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    internal class EnovaSynchronizer : RecurringTaskBase
    {
        private readonly ILogger _logger;

        public EnovaSynchronizer(IConfiguration configuration, ILogger<EnovaSynchronizer> logger, IServiceProvider serviceProvider) : base(configuration, logger, serviceProvider)
        {
            _logger = logger;
        }

        protected override async Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Processing Enova synchronization");

            if (!cancellationToken.IsCancellationRequested)
                await scope.ServiceProvider.GetRequiredService<EnovaPricesImporter>().RunImport(scope, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await scope.ServiceProvider.GetRequiredService<EnovaCustomersImporter>().RunImport(scope, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await scope.ServiceProvider.GetRequiredService<EnovaDeletedDiscountGroupsImporter>().RunImport(scope, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await scope.ServiceProvider.GetRequiredService<EnovaDiscountGroupsImporter>().RunImport(scope, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await scope.ServiceProvider.GetRequiredService<EnovaCustomerDiscountsImporter>().RunImport(scope, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await scope.ServiceProvider.GetRequiredService<EnovaProductImporter>().RunImport(scope, cancellationToken);
        }
    }
}
