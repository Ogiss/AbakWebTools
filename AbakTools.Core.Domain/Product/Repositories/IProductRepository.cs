using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Product.Repositories
{
    public interface IProductRepository : IGenericGuidedEntityRepository<ProductEntity>
    {
        IReadOnlyCollection<ProductEntity> GetAllReady();
        ProductEntity GetByWebId(int webId);
        IReadOnlyCollection<ProductEntity> GetAllByEnovaGuid(Guid guid);
        Task<ProductEntity> GetEnovaProductAsync(Guid guid);
        Task<IList<ProductEntity>> GetEnovaProductsAsync(Guid enovaGuid);
        Task<IList<ProductEntity>> GetEnovaProductsWithoutInDeletingProcessAsync(Guid enovaGuid);
    }
}
