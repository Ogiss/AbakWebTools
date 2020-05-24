using AbakTools.Core.Domain.Tax;

namespace AbakTools.Core.DataAccess.Mappings.Tax
{
    class TaxEntityMap : GuidedEntityMap<TaxEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("tax");

            Map(x => x.WebId, "id_tax");
            Map(x => x.Rate, "rate");
            Map(x => x.Name, "name");
        }
    }
}
