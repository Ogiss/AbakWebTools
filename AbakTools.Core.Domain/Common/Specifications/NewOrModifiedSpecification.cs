using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Common.Specifications
{
    public class NewOrModifiedSpecification<TEntity> : Specification<TEntity>
        where TEntity : IStampedEntity
    {
        private readonly long _stampFrom;
        private readonly long _stampTo;

        private NewOrModifiedSpecification(long stampFrom, long stampTo)
        {
            _stampFrom = stampFrom;
            _stampTo = stampTo;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
                return x => x.Stamp > _stampFrom && x.Stamp <= _stampTo;
        }

        public static NewOrModifiedSpecification<TEntity> Of(long stampFrom, long stampTo)
        {
            return new NewOrModifiedSpecification<TEntity>(stampFrom, stampTo);
        }
    }
}
