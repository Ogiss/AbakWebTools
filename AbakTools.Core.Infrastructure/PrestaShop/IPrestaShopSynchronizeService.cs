using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public interface IPrestaShopSynchronizeService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
