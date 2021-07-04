using AbakTools.Core.Domain;
using AbakTools.Core.Framework;
using AbakTools.Core.Framework.Domain;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class GenericBusinessEntityRepository<TEntity> : GenericGuidedEntityRepository<TEntity>
        where TEntity : BusinessEntity
    {
        protected GenericBusinessEntityRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public override IQueryable<TEntity> GetQueryBase()
        {
            return base.GetQueryBase().Where(x => x.IsDeleted == false);
        }

        protected override IQueryable<TEntity> GetQuery(ISpecification<TEntity> specification)
        {
            var withDeleted = false;

            if (specification is BusinessEntitySpecificationBase<TEntity> businessSpecification)
            {
                withDeleted = businessSpecification.WithDeleted;
            }

            return withDeleted ? base.GetQueryBase().Where(specification.ToExpression()) : GetQueryBase().Where(specification.ToExpression());
        }

        public override void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            SaveOrUpdate(entity);
        }
    }
}
