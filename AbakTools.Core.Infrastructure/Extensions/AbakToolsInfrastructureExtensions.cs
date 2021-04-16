using AbakTools.Core.Domain.Enova;
using AbakTools.Core.Domain.Enova.Product;
using AbakTools.Core.Domain.Policies;
using AbakTools.Core.Framework.Cryptography;
using AbakTools.Core.Infrastructure.Cryptography;
using AbakTools.Core.Infrastructure.Enova.Api;
using AbakTools.Core.Infrastructure.Enova.Importers;
using AbakTools.Core.Infrastructure.Enova.Repositories;
using AbakTools.Core.Infrastructure.Enova.Services;
using AbakTools.Core.Infrastructure.Enova.Services.Implementations;
using AbakTools.Core.Infrastructure.Policies;
using AbakTools.Core.Infrastructure.PrestaShop;
using AbakTools.Core.Infrastructure.PrestaShop.Exporters;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbakToolsInfrastructureExtensions
    {
        public static void AddInfrastructureComponent(this IServiceCollection service)
        {
            service.AddTransient<IPrestaShopClient, PrestaShopClient>();
            service.AddTransient<IStringEncryptor, TripleDESStringEncryptor>();
            RegisterPolicies(service);
            RegisterPrestaShopComponent(service);
            RegisterEnovaApiComponent(service);
        }

        private static void RegisterEnovaApiComponent(IServiceCollection service)
        {
            service.AddSingleton<IEnovaAPiClient, EnovaApiClient>();
            service.AddSingleton<IEnovaProductRepository, EnovaProductRepository>();
            service.AddSingleton<IEnovaCustomerRepository, EnovaCustomerRepository>();
            service.AddSingleton<IEnovaDiscountGroupRepository, EnovaDiscountGroupRepository>();
            service.AddSingleton<IEnovaCustomerDiscountRepository, EnovaCustomerDiscountRepository>();
            service.AddSingleton<IEnovaDictionaryItemRepository, EnovaDictionaryItemRepository>();

            service.AddSingleton<IEnovaSynchronizeService, EnovaSynchronizeService>();
            service.AddSingleton<EnovaPricesImporter>();
            service.AddSingleton<EnovaCustomersImporter>();
            service.AddSingleton<EnovaDiscountGroupsImporter>();
            service.AddSingleton<EnovaDeletedDiscountGroupsImporter>();
            service.AddSingleton<EnovaCustomerDiscountsImporter>();
            service.AddSingleton<EnovaProductImporter>();
        }

        private static void RegisterPrestaShopComponent(IServiceCollection service)
        {
            service.AddTransient<IPrestaShopSynchronizeCustomer, PrestaShopSynchronizeCustomer>();
            service.AddTransient<IPrestaShopSynchronizeOrder, PrestaShopSynchronizeOrder>();

            service.AddTransient<IPSOrderRepository, PSOrderRepository>();
            service.AddTransient<IPSCustomerRepository, PSCustomerRepository>();
            service.AddTransient<IPSDiscountGroupRepository, PSDiscountGroupRepository>();

            service.AddTransient<IPrestaShopExporter, DiscoutGroupExporter>();
        }

        private static void RegisterPolicies(IServiceCollection services)
        {
            services.AddScoped<IProductPricePolicy, ProductPricePolicy>();
        }
    }
}
