using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Product.Specifications
{
    public class ProductToExportSpecification : ProductSpecificationBase
    {
        public static ProductToExportSpecification Instance = new ProductToExportSpecification();

        private ProductToExportSpecification() { }

        public override Expression<Func<ProductEntity, bool>> ToExpression()
        {
            return x => x.IsReady == true;
        }
    }
}
