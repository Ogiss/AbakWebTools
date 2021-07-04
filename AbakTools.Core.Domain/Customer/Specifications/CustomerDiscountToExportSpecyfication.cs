using AbakTools.Core.Domain.Common.Specifications;
using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Customer.Specifications
{
    public class CustomerDiscountToExportSpecyfication : Specification<CustomerDiscountEntity>
    {
        private readonly long _stampFrom;
        private readonly long _stampTo;

        private CustomerDiscountToExportSpecyfication(long stampFrom, long stampTo)
        {
            _stampFrom = stampFrom;
            _stampTo = stampTo;
        }

        public override Expression<Func<CustomerDiscountEntity, bool>> ToExpression()
        {
            var specification = SynchronizedSpecyfication<CustomerDiscountEntity>.Create().Not()
                .And(NewOrModifiedSpecification<CustomerDiscountEntity>.Of(_stampFrom, _stampTo))
                .And(new CustomerDiscountWithExportedCustomerAndGroupSpecification());

            return specification.ToExpression();
        }

        public static CustomerDiscountToExportSpecyfication Of(long stampFrom, long stampTo)
        {
            return new CustomerDiscountToExportSpecyfication(stampFrom, stampTo);
        }
    }
}
