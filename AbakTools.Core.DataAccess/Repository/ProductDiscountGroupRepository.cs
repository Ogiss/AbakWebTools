using AbakTools.Core.Domain.Product.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using AbakTools.Core.Domain.Product;
using System.Linq;
using NHibernate.Linq;
using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.DataAccess.Repository
{
    class ProductDiscountGroupRepository : BaseRepository, IProductDiscountGroupRepository
    {
        public ProductDiscountGroupRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public IEnumerable<ProductDiscountGroupEntity> GetAllGroupsForProductWithReferences(int productId)
        {
            return CurrentSession.Query<ProductDiscountGroupEntity>()
                .Where(x => x.Product.Id == productId)
                .Fetch(x => x.Product)
                .Fetch(x => x.DiscountGroup)
                .ToList();
        }

        public IEnumerable<int> GetEnovaProductIdsWithModifiedDiscountGroups(long stampFrom, long stampTo)
        {
            var sql =
                "SELECT DISTINCT pdg.Towar " +
                "FROM TowarGrupaRabatowa pdg " +
                "INNER JOIN product t ON t.id = pdg.Towar " +
                $"WHERE CONVERT(BIGINT, pdg.Stamp) > {stampFrom} AND CONVERT(BIGINT, pdg.Stamp) <= {stampTo} " +
                $"AND pdg.Synchronize <> {(int)SynchronizeType.Synchronized} AND t.TowarEnova = 1";

            return CurrentSession.CreateSQLQuery(sql)
                .List<int>();
        }

        public void SaveOrUpdate(ProductDiscountGroupEntity group)
        {
            CurrentSession.SaveOrUpdate(group);
        }
    }
}
