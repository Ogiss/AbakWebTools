using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class ProductRepository : GenericBusinessEntityRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public IReadOnlyCollection<ProductEntity> GetAllReady()
        {
            return GetQueryBase().Where(x => x.IsReady == true).ToList();
        }

        public ProductEntity GetByWebId(int webId)
        {
            return GetQueryBase().SingleOrDefault(x => x.WebId == webId);
        }
    }
}
