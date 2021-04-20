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
            service.AddScoped<IPrestaShopClient, PrestaShopClient>();
            service.AddScoped<IStringEncryptor, TripleDESStringEncryptor>();
            RegisterPolicies(service);
            RegisterPrestaShopComponent(service);
            RegisterEnovaApiComponent(service);
        }

        private static void RegisterEnovaApiComponent(IServiceCollection service)
        {
            service.AddScoped<IEnovaAPiClient, EnovaApiClient>();
            service.AddScoped<IEnovaProductRepository, EnovaProductRepository>();
            service.AddScoped<IEnovaCustomerRepository, EnovaCustomerRepository>();
            service.AddScoped<IEnovaDiscountGroupRepository, EnovaDiscountGroupRepository>();
            service.AddScoped<IEnovaCustomerDiscountRepository, EnovaCustomerDiscountRepository>();
            service.AddScoped<IEnovaDictionaryItemRepository, EnovaDictionaryItemRepository>();

            service.AddScoped<IEnovaSynchronizeService, EnovaSynchronizeService>();
            service.AddScoped<EnovaPricesImporter>();
            service.AddScoped<EnovaCustomersImporter>();
            service.AddScoped<EnovaDiscountGroupsImporter>();
            service.AddScoped<EnovaDeletedDiscountGroupsImporter>();
            service.AddScoped<EnovaCustomerDiscountsImporter>();
            service.AddScoped<EnovaProductImporter>();
        }

        private static void RegisterPrestaShopComponent(IServiceCollection service)
        {
            service.AddScoped<IPrestaShopSynchronizeCustomer, PrestaShopSynchronizeCustomer>();
            service.AddScoped<IPrestaShopSynchronizeOrder, PrestaShopSynchronizeOrder>();

            service.AddScoped<IPSOrderRepository, PSOrderRepository>();
            service.AddScoped<IPSCustomerRepository, PSCustomerRepository>();
            service.AddScoped<IPSDiscountGroupRepository, PSDiscountGroupRepository>();
            service.AddScoped<IPSProductDiscountGroupRepository, PSProductDiscountGroupRepository>();

            service.AddScoped<IPrestaShopExporter, DiscoutGroupExporter>();
            service.AddScoped<IPrestaShopExporter, ProductDiscountGroupExporter>();

            //service.AddSingleton<DiscoutGroupExporter>();
            //service.AddSingleton<ProductDiscountGroupExporter>();

        }

        private static void RegisterPolicies(IServiceCollection services)
        {
            services.AddScoped<IProductPricePolicy, ProductPricePolicy>();
        }
    }
}
