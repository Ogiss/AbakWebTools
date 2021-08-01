using AbakTools.Core.Framework.Domain;
using System;
using System.Linq.Expressions;

namespace AbakTools.Core.Domain.Product.Specifications
{
    public class UnitsToExportSpecification : Specification<UnitEntity>
    {
        private readonly long _stampFrom;
        private readonly long _stampTo;

        private UnitsToExportSpecification(long stampFrom, long stampTo)
        {
            _stampFrom = stampFrom;
            _stampTo = stampTo;
        }

        public static UnitsToExportSpecification Of(long stampFrom, long stampTo) => new UnitsToExportSpecification(stampFrom, stampTo);

        public override Expression<Func<UnitEntity, bool>> ToExpression()
        {
            return x => x.Stamp > _stampFrom && x.Stamp <= _stampTo;
        }
    }
}
