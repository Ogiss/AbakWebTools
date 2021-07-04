using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Threading.Tasks;
using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class ProductRepository : GenericBusinessEntityRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public IReadOnlyCollection<ProductEntity> GetAllByEnovaGuid(Guid guid)
        {
            return GetQueryBase().Where(x => x.EnovaGuid == guid).ToArray();
        }

        public IReadOnlyCollection<ProductEntity> GetAllReady()
        {
            return GetQueryBase().Where(x => x.IsReady == true).ToList();
        }

        public ProductEntity GetByWebId(int webId)
        {
            return GetQueryBase().SingleOrDefault(x => x.WebId == webId);
        }

        public async Task<ProductEntity> GetEnovaProductAsync(Guid enovaGuid)
        {
            return await GetQueryBase().SingleOrDefaultAsync(x => x.EnovaGuid == enovaGuid && x.IsEnovaProduct);
        }

        public async Task<IList<ProductEntity>> GetEnovaProductsAsync(Guid enovaGuid)
        {
            return await GetQueryBase().Where(x => x.EnovaGuid == enovaGuid && x.IsEnovaProduct).ToListAsync();
        }

        public async Task<IList<ProductEntity>> GetEnovaProductsWithoutInDeletingProcessAsync(Guid enovaGuid)
        {
            return await GetQueryBase().Where(x => x.Synchronize != SynchronizeType.Deleted && x.EnovaGuid == enovaGuid && x.IsEnovaProduct).ToListAsync();
        }

        public IEnumerable<int> GetAllWebIdsByEnovaGuid(Guid enovaGuid)
        {
            return GetQueryBase()
                .Where(x => x.Synchronize != SynchronizeType.Deleted && x.WebId > 0 && x.EnovaGuid == enovaGuid)
                .Select(x => x.WebId.Value)
                .ToList();
        }
    }
}
