using AbakTools.Core.Domain.Product;
using Bukimedia.PrestaSharp.Entities;
using PsProductEntity = Bukimedia.PrestaSharp.Entities.product;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services
{
    public interface IProductImageSynchronizeService
    {
        product Synchronize(ProductEntity product, PsProductEntity psProduct);
    }
}
