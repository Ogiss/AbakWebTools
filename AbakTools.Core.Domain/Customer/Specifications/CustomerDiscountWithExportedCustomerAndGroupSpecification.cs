using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Customer.Specifications
{
    public class CustomerDiscountWithExportedCustomerAndGroupSpecification : Specification<CustomerDiscountEntity>
    {
        public override Expression<Func<CustomerDiscountEntity, bool>> ToExpression()
        {
            return x => x.Customer.WebId != null && x.DiscountGroup.WebId != null;
        }
    }
}
