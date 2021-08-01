using Microsoft.Extensions.DependencyInjection;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRecurringTasks(this IServiceCollection services)
        {
            services.AddHostedService<SynchronizeHostedService>();
            services.AddScoped<PrestaShopOrderSynchronizer>();
            services.AddScoped<EnovaSynchronizer>();
            services.AddScoped<PrestaShopSynchronizer>();

            //services.AddHostedService<PrestaShopOrderSynchronizer>();
            //services.AddHostedService<EnovaSynchronizer>();
            //services.AddHostedService<PrestaShopSynchronizer>();

            return services;
        }
    }
}
