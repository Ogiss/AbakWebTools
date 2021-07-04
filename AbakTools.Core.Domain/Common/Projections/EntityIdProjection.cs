using AbakTools.Core.Framework;
using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Common.Projections
{
    public class EntityIdProjection<TEntity> : IProjection<TEntity, int>
        where TEntity: IEntity
    {
        private EntityIdProjection() { }

        public static EntityIdProjection<TEntity> Create() => new EntityIdProjection<TEntity>();

        public Expression<Func<TEntity, int>> ToExpression()
        {
            return x => x.Id;
        }
    }
}
