using System.Collections.Generic;

namespace AbakTools.Core.Domain.Product.Repositories
{
    public interface IProductRepository : IGenericGuidedEntityRepository<ProductEntity>
    {
        IReadOnlyCollection<ProductEntity> GetAllReady();
        ProductEntity GetByWebId(int webId);
    }
}
