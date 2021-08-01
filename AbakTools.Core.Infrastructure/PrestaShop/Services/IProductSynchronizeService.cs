using AbakTools.Core.Domain.Product;
using PsProduct = Bukimedia.PrestaSharp.Entities.product;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services
{
    public interface IProductSynchronizeService
    {
        PsProduct Synchronize(ProductEntity product);
        void AutomaticUpdate(ProductEntity product);
    }
}
