using AbakTools.Core.Domain.Supplier;

namespace AbakTools.Core.DataAccess.Mappings.Supplier
{
    class SupplierEntityMap : GuidedEntityMap<SupplierEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("Dostawcy");

            Map(x => x.Name, "Nazwa").Not.Nullable();
            Map(x=>x.Stamp).Formula("CONVERT(BIGINT, Stamp)").Generated.Always();
        }
    }
}
