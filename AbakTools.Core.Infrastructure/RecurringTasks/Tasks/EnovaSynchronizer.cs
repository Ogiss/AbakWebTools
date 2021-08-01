using AbakTools.Core.Infrastructure.Enova.Importers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    //internal class EnovaSynchronizer : RecurringTaskBase
    internal class EnovaSynchronizer
    {
        private readonly ILogger _logger;

        //public EnovaSynchronizer(IConfiguration configuration, ILogger<EnovaSynchronizer> logger, IServiceProvider serviceProvider) : base(configuration, logger, serviceProvider)
        public EnovaSynchronizer(IConfiguration configuration, ILogger<EnovaSynchronizer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
        }

        //protected override Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        public Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Processing Enova synchronization");

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<EnovaPricesImporter>().RunImport(scope, cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<EnovaCustomersImporter>().RunImport(scope, cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<EnovaDeletedDiscountGroupsImporter>().RunImport(scope, cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<EnovaDiscountGroupsImporter>().RunImport(scope, cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<EnovaCustomerDiscountsImporter>().RunImport(scope, cancellationToken).Wait();

            if (!cancellationToken.IsCancellationRequested)
                scope.ServiceProvider.GetRequiredService<EnovaProductImporter>().RunImport(scope, cancellationToken).Wait();

            return Task.CompletedTask;
        }
    }
}
