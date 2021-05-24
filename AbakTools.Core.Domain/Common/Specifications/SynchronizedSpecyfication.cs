using AbakTools.Core.Framework;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Common.Specifications
{
    public class SynchronizedSpecyfication<TEntity> : Specification<TEntity>
        where TEntity : ISynchronizableEntity
    {
        private SynchronizedSpecyfication() { }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => x.Synchronize == SynchronizeType.Synchronized;
        }

        public static SynchronizedSpecyfication<TEntity> Create()
        {
            return new SynchronizedSpecyfication<TEntity>();
        }
    }
}
