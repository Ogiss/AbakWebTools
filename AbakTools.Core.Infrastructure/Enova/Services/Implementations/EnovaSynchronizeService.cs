using AbakTools.Core.Infrastructure.Enova.Importers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Services.Implementations
{
    class EnovaSynchronizeService : IEnovaSynchronizeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public EnovaSynchronizeService(IConfiguration configuration, ILogger<EnovaSynchronizeService> logger)
            => (_configuration, _logger) = (configuration, logger);

        public async Task DoWork(IServiceScope serviceScope, CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                bool.TryParse(_configuration.GetSection("EnovaSynchronization:Disabled").Value, out bool disabled);

                if (disabled)
                {
                    _logger.LogWarning("Enova synchronization is disabled");
                }

                while (!disabled && !stoppingToken.IsCancellationRequested)
                {
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaPricesImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaCustomersImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaDeletedDiscountGroupsImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaDiscountGroupsImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaCustomerDiscountsImporter>().RunImport(serviceScope);
                    await serviceScope.ServiceProvider.GetRequiredService<EnovaProductImporter>().RunImport(serviceScope);

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Enova synchronization was cancel");
                }
            });
        }
    }
}
