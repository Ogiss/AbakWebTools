using AbakTools.Core.Domain.Product;
using PsProductDiscountGroup = Bukimedia.PrestaSharp.Entities.product_discount_group;
using PsProduct = Bukimedia.PrestaSharp.Entities.product;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services
{
    public interface IProductDiscountGroupSynchronizeService
    {
        void Synchronize(ProductEntity product, PsProduct psProduct);
        PsProductDiscountGroup Synchronize(int psProductId, ProductDiscountGroupEntity productDiscountGroup);
    }
}
