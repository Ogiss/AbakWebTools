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
        protected readonly IConfiguration Configuration;
        protected readonly ILogger Logger;
        private readonly IServiceProvider serviceProvider;

        public RecurringTaskBase(
            IConfiguration configuration,
            ILogger logger,
            IServiceProvider serviceProvider)
        {
            this.Configuration = configuration;
            this.Logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected abstract Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!Configuration.GetRecurringTask(GetType()).Enabled)
            {
                return;
            }

            cancellationToken.Register(() => Logger.LogDebug($"Cancelling"));

            try
            {
                await Task.Delay(GetRandomStartingDelay(), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            Logger.LogDebug($"Starting");

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
                        await Task.Delay(Configuration.GetRecurringTask(GetType()).Interval, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger.LogDebug($"DoWork cancelled");
                }
                catch (Exception e)
                {
                    Logger.LogError("DoWork exception: " + e);
                }
            }

            Logger.LogDebug($"Stopping");
        }

        private TimeSpan GetRandomStartingDelay()
        {
            var maxDelay = TimeSpan.FromSeconds(30);
            var interval = Configuration.GetRecurringTask(GetType()).Interval;
            var random = new Random();
            var delay = random.Next(0, Math.Min((int)interval.TotalMilliseconds, (int)maxDelay.TotalMilliseconds));

            return TimeSpan.FromMilliseconds(delay);
        }
    }
}
