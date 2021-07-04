using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.DataAccess.Mappings
{
    class CustomerDiscountEntityMap : GuidedEntityMap<CustomerDiscountEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("KontrahentRabatyGrupowe");

            References(x => x.DiscountGroup, "GrupaRabatowa").Not.Nullable();
            References(x => x.Customer, "Kontrahent").Not.Nullable();
            Component(x => x.Discount, m => { m.Map(p => p.Value, "Rabat"); });
            Map(x => x.DiscountActive, "RabatZdefiniowany");
            Map(x => x.Stamp).Formula("CONVERT(BIGINT, Stamp)");
            Map(x => x.Synchronize).CustomType<SynchronizeType>();
            Map(x => x.EnovaStamp);
        }
    }
}
