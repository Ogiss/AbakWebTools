using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Common.Specifications
{
    public class SynchronizedSpecification<TEntity> : Specification<TEntity>
        where TEntity : ISynchronizableEntity
    {
        private SynchronizedSpecification() { }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => x.Synchronize == SynchronizeType.Synchronized;
        }

        public static SynchronizedSpecification<TEntity> Create()
        {
            return new SynchronizedSpecification<TEntity>();
        }
    }
}
