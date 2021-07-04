using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Framework
{
    public interface IProjection<TSource, TResult>
    {
        Expression<Func<TSource, TResult>> ToExpression();
    }
}
