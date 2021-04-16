using AbakTools.Core.Domain.DiscountGroup;

namespace AbakTools.Core.DataAccess.Mappings
{
    class DiscountGroupMap : SynchronizableEntityMap<DiscountGroupEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("GrupyRabatowe");

            Map(x => x.Category, "Kategoria")
                .CustomSqlType("varchar")
                .Not.Nullable()
                .Length(16);

            Map(x => x.Name, "Wartosc")
                .CustomSqlType("varchar")
                .Not.Nullable()
                .Length(80);

            Map(x => x.EnovaStamp).Not.Nullable();
        }
    }
}
