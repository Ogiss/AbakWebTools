using AbakTools.Core.Framework.Cryptography;
using AbakTools.Core.Infrastructure.Cryptography;
using AbakTools.Core.Infrastructure.PrestaShop;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbakToolsInfrastructureExtensions
    {
        public static void AddInfrastructureComponent(this IServiceCollection service)
        {
            service.AddTransient<IPrestaShopClient, PrestaShopClient>();
            service.AddTransient<IStringEncryptor, TripleDESStringEncryptor>();
            service.AddTransient<IPrestaShopSynchronizeCustomer, PrestaShopSynchronizeCustomer>();
        }
    }
}
