using AbakTools.Core.Domain.DiscountGroup;
using PsDiscountGroup = Bukimedia.PrestaSharp.Entities.discount_group;

namespace AbakTools.Core.Infrastructure.PrestaShop.Services
{
    public interface IDiscountGroupSynchronizeService
    {
        PsDiscountGroup Synchronize(DiscountGroupEntity discountGroup);
    }
}
