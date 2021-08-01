using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.PrestaShop.Exporters
{
    public interface IPrestaShopExporter
    {
        void StartExport(CancellationToken cancelerationToken);
    }
}
