using AbakTools.Core.Domain.Common.Specifications;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Product.Specifications
{
    public class ProductToAutomaticExportSpecification : ProductSpecificationBase
    {
        private readonly long _stampFrom;
        private readonly long _stampTo;

        private ProductToAutomaticExportSpecification(long stampFrom, long stampTo)
        {
            _stampFrom = stampFrom;
            _stampTo = stampTo;
        }

        public override Expression<Func<ProductEntity, bool>> ToExpression()
        {
            var specification =
                //SynchronizedSpecification<ProductEntity>.Create().Not() &
                WithWebIdentitySpecification<ProductEntity>.Create() &
                NewOrModifiedSpecification<ProductEntity>.Of(_stampFrom, _stampTo);

            return specification.ToExpression();
        }

        public static ProductToAutomaticExportSpecification Of(long stampFrom, long stampTo)
            => new ProductToAutomaticExportSpecification(stampFrom, stampTo);
    }
}
