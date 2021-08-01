using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Product.Projections
{
    public class UnitIdProjection : IProjection<UnitEntity, int>
    {
        private UnitIdProjection() { }

        public static UnitIdProjection Create() => new UnitIdProjection();

        public System.Linq.Expressions.Expression<System.Func<UnitEntity, int>> ToExpression()
        {
            return x => x.Id;
        }
    }
}
