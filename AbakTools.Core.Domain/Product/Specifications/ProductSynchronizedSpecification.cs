using AbakTools.Core.Domain.Common.Specifications;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Product.Specifications
{
    public class ProductSynchronizedSpecification : ProductSpecificationBase
    {
        private ProductSynchronizedSpecification() { }

        public static ProductSynchronizedSpecification Create() => new ProductSynchronizedSpecification();

        public override Expression<Func<ProductEntity, bool>> ToExpression()
        {
            return (SynchronizedSpecification<ProductEntity>.Create() & WithWebIdentitySpecification<ProductEntity>.Create()).ToExpression();
        }
    }
}
