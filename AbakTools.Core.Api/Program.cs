using AbakTools.Core.Api.HostedServices;
using AbakTools.Core.Infrastructure.PrestaShop;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AbakTools.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services => {
                    services.AddHostedService<ConsumeScopedServiceHostedService>();
                    services.AddScoped<IPrestaShopSynchronizeService, PrestaShopSynchronizeService>();
                });
    }
}
