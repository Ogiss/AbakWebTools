using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Common.Specifications
{
    public class WithWebIdentitySpecification<TEntity> : Specification<TEntity>
        where TEntity : IWebIdHolder
    {
        private WithWebIdentitySpecification() { }

        public static WithWebIdentitySpecification<TEntity> Create() => new WithWebIdentitySpecification<TEntity>();

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return x => x.WebId != null && x.WebId > 0;
        }
    }
}
