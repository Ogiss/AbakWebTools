using AbakTools.Core.Domain.Order;

namespace AbakTools.Core.DataAccess.Mappings.Order
{
    class OrderStateEntityMap : EntityMap<OrderStateEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("StatusyZamowien");

            Map(x => x.WebId, "PSID");
            Map(x => x.Name, "Nazwa").Not.Nullable();
            Map(x => x.NewOrder, "NoweZamowienie");
            //Map(x => x.Synchronize, "Synchronizacja");
        }
    }
}
