using AbakTools.Core.Api.HostedServices;
using AbakTools.Core.Infrastructure.PrestaShop;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                .UseWindowsService()
                .ConfigureLogging(logging => {
                    logging.AddFile("Logs/application-{Date}.txt");
                })
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
