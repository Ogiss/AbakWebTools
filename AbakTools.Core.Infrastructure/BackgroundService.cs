using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure
{
    abstract class BackgroundService : IHostedService, IDisposable
    {
        private Task executingTask;
        private readonly CancellationTokenSource stoppingCTS = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            executingTask = ExecuteAsync(stoppingCTS.Token);

            if (executingTask.IsCompleted)
            {
                return executingTask;
            }

            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (executingTask == null)
            {
                return;
            }

            try
            {
                stoppingCTS.Cancel();
            }
            finally
            {
                await Task.WhenAny(
                    executingTask,
                    Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public virtual void Dispose()
        {
            stoppingCTS.Cancel();
            stoppingCTS.Dispose();
        }
    }
}
