using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Framework
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
        bool IsSatisfiedBy(T entity);
    }
}
