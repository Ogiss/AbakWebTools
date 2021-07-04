using AbakTools.Core.Domain.Enova;
using AbakTools.Core.Domain.Enova.Product;
using AbakTools.Core.Domain.Policies;
using AbakTools.Core.Framework.Cryptography;
using AbakTools.Core.Infrastructure.Cryptography;
using AbakTools.Core.Infrastructure.Enova.Api;
using AbakTools.Core.Infrastructure.Enova.Importers;
using AbakTools.Core.Infrastructure.Enova.Repositories;
using AbakTools.Core.Infrastructure.Policies;
using AbakTools.Core.Infrastructure.PrestaShop;
using AbakTools.Core.Infrastructure.PrestaShop.Exporters;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories;
using AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations;
using AbakTools.Core.Infrastructure.PrestaShop.Services;
using AbakTools.Core.Infrastructure.PrestaShop.Services.Implementations;

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

            //service.AddScoped<IEnovaSynchronizeService, EnovaSynchronizeService>();
            service.AddScoped<EnovaPricesImporter>();
            service.AddScoped<EnovaCustomersImporter>();
            service.AddScoped<EnovaDiscountGroupsImporter>();
            service.AddScoped<EnovaDeletedDiscountGroupsImporter>();
            service.AddScoped<EnovaCustomerDiscountsImporter>();
            service.AddScoped<EnovaProductImporter>();
        }

        private static void RegisterPrestaShopComponent(IServiceCollection service)
        {
            service.AddScoped<IPrestaShopSynchronizeService, PrestaShopSynchronizeService>();
            service.AddScoped<IPrestaShopSynchronizeCustomer, PrestaShopSynchronizeCustomer>();
            service.AddScoped<IPrestaShopSynchronizeOrder, PrestaShopSynchronizeOrder>();

            service.AddScoped<IPSOrderRepository, PSOrderRepository>();
            service.AddScoped<IPSCustomerRepository, PSCustomerRepository>();
            service.AddScoped<IPsDiscountGroupRepository, PsDiscountGroupRepository>();
            service.AddScoped<IPsProductDiscountGroupRepository, PsProductDiscountGroupRepository>();
            service.AddScoped<IPsCustomerDiscountGroupRepository, PsCustomerDiscountGroupRepository>();
            service.AddScoped<IPsMessageRepository, PsMessageRepository>();
            service.AddScoped<IPsProductRepository, PsProductRepository>();

            service.AddScoped<IDiscountGroupSynchronizeService, DiscountGroupSynchronizeService>();
            service.AddScoped<IProductDiscountGroupSynchronizeService, ProductDiscountGroupSynchronizeService>();
            service.AddScoped<IProductSynchronizeService, ProductSynchronizeService>();
            service.AddScoped<IProductImageSynchronizeService, ProductImageSynchronizeService>();

            service.AddScoped<IPrestaShopExporter, DiscoutGroupExporter>();
            service.AddScoped<IPrestaShopExporter, ProductExporter>();
            service.AddScoped<IPrestaShopExporter, ProductDiscountGroupExporter>();
            service.AddScoped<IPrestaShopExporter, CustomerDiscountGroupExporter>();
        }

        private static void RegisterPolicies(IServiceCollection services)
        {
            services.AddScoped<IProductPricePolicy, ProductPricePolicy>();
        }
    }
}
