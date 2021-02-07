using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Services
{
    public interface IEnovaSynchronizeService
    {
        Task DoWork(IServiceScope serviceScope, CancellationToken stoppingToken);
    }
}
