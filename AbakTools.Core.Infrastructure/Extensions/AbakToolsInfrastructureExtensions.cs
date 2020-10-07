using AbakTools.Core.Domain.Enova.Product;
using AbakTools.Core.Domain.Policies;
using AbakTools.Core.Framework.Cryptography;
using AbakTools.Core.Infrastructure.Cryptography;
using AbakTools.Core.Infrastructure.Enova.Api;
using AbakTools.Core.Infrastructure.Policies;
using AbakTools.Core.Infrastructure.PrestaShop;
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
            service.AddTransient<IEnovaProductRepository, EnovaProductRepository>();
        }

        private static void RegisterPrestaShopComponent(IServiceCollection service)
        {
            service.AddTransient<IPrestaShopSynchronizeCustomer, PrestaShopSynchronizeCustomer>();
            service.AddTransient<IPrestaShopSynchronizeOrder, PrestaShopSynchronizeOrder>();

            service.AddTransient<IPsOrderRepository, PsOrderRepository>();
            service.AddTransient<IPsCustomerRepository, PsCustomerRepository>();
        }

        private static void RegisterPolicies(IServiceCollection services)
        {
            services.AddScoped<IProductPricePolicy, ProductPricePolicy>();
        }
    }
}
