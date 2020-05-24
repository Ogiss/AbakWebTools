using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Product.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
