using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public interface IPrestaShopSynchronizeOrder
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
