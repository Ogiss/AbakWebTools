using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.RecurringTasks
{
    abstract class RecurringTaskBase : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public RecurringTaskBase(
            IConfiguration configuration,
            ILogger logger,
            IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected abstract Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!configuration.GetRecurringTask(GetType()).Enabled)
            {
                return;
            }

            cancellationToken.Register(() => logger.LogDebug($"Cancelling"));

            try
            {
                await Task.Delay(GetRandomStartingDelay(), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            logger.LogDebug($"Starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    try
                    {
                        using (var scope = serviceProvider.CreateScope())
                        {
                            await DoWorkAsync(scope, cancellationToken);
                        }
                    }
                    finally
                    {
                        await Task.Delay(configuration.GetRecurringTask(GetType()).Interval, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    logger.LogDebug($"DoWork cancelled");
                }
                catch (Exception e)
                {
                    logger.LogError("DoWork exception: " + e);
                }
            }

            logger.LogDebug($"Stopping");
        }

        protected int GetHourOccurency()
        {
            return configuration.GetRecurringTask(GetType()).TaskHourOccurence;
        }

        private TimeSpan GetRandomStartingDelay()
        {
            var maxDelay = TimeSpan.FromSeconds(30);
            var interval = configuration.GetRecurringTask(GetType()).Interval;
            var random = new Random();
            var delay = random.Next(0, Math.Min((int)interval.TotalMilliseconds, (int)maxDelay.TotalMilliseconds));

            return TimeSpan.FromMilliseconds(delay);
        }
    }
}
