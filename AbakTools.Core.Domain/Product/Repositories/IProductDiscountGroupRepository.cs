using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Product.Repositories
{
    public interface IProductDiscountGroupRepository
    {
        IEnumerable<int> GetEnovaProductIdsWithModifiedDiscountGroups(long stampfrom, long stampTo);
        IEnumerable<ProductDiscountGroupEntity> GetAllGroupsForProductWithReferences(int productId);
        void SaveOrUpdate(ProductDiscountGroupEntity group);
    }
}
