using AbakTools.Core.Domain.Order;

namespace AbakTools.Core.DataAccess.Mappings.Order
{
    class OrderHistoryEntityMap : EntityMap<OrderHistoryEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("HistoriaZamowien");

            Map(x => x.Date, "DataDodania");

            References(x => x.Order, "Zamowienie").Not.Nullable(); ;
            References(x => x.Status, "Status").Not.Nullable();
        }
    }
}
