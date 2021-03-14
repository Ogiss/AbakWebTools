using AbakTools.Core.Infrastructure.Enova.Importers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Services.Implementations
{
    class EnovaSynchronizeService : IEnovaSynchronizeService
    {
        private readonly ILogger _logger;

        public EnovaSynchronizeService(ILogger<EnovaSynchronizeService> logger) => _logger = logger;

        public async Task DoWork(IServiceScope serviceScope, CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogDebug("Staring synctonize Enova");

                    await serviceScope.ServiceProvider.GetRequiredService<EnovaPricesImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaCustomersImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaDiscountGroupsImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaCustomerDiscountsImporter>().RunImport(serviceScope);

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            });
        }
    }
}
