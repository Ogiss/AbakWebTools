using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public interface IPrestaShopSynchronizeCustomer
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
