using AbakTools.Core.Domain.Category;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class CategoryRepository : GenericBusinessEntityRepository<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public IReadOnlyList<CategoryEntity> GetAll(int? parentId)
        {
            var query = GetQueryBase();

            if (parentId.HasValue)
            {
                query = query.Where(x => x.Parent.Id == parentId);
            }
            else
            {
                query = query.Where(x => x.Parent == null);
            }

            return query.ToList();
        }

        public IReadOnlyCollection<CategoryEntity> GetAllModified(DateTime from, DateTime? to)
        {
            var qry = GetQueryBase().Where(x => x.EnovaFeature == false && x.DateTimeStamp > from ); // TODO: change x.DateTimeStamp to x.ModyficationDate

            if (to.HasValue)
            {
                qry = qry.Where(x => x.ModificationDate <= to);
            }

            return qry.OrderBy(x => x.LevelDepth).ThenBy(x => x.Name).ToList();
        }
    }
}
