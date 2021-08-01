using AbakTools.Core.Domain.Product;

namespace AbakTools.Core.DataAccess.Mappings.Product
{
    class UnitEntityMap : GuidedEntityMap<UnitEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("JednostkiMiary");

            Map(x => x.Default, "Domyslna");
            Map(x => x.Multiplier, "Mnoznik");
            Map(x => x.Name, "Nazwa");
            Map(x => x.WebId, "PSID");
            Map(x => x.Stamp).Formula("CONVERT(BIGINT, Stamp)").Generated.Always();
        }
    }
}
