using AbakTools.Core.Domain.Order;
using AbakTools.Core.Framework;

namespace AbakTools.Core.DataAccess.Mappings.Order
{
    class OrderRowEntityMap : EntityMap<OrderRowEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("PozycjeZamowien");

            Map(x => x.WebId, "PSID");
            Map(x => x.ItemNumber, "Ident");
            Map(x => x.Quantity, "Ilosc");
            Map(x => x.OrginalQuantity, "IloscOrg");
            Map(x => x.Price, "Cena");
            Map(x => x.TaxName, "StawkaVatSymbol");
            Map(x => x.TaxValue, "StawkaVatValue");
            Map(x => x.Description, "Opis");
            Map(x => x.Synchronize, "Synchronizacja").CustomType<SynchronizeType>();
            Map(x => x.Rebate, "Rabat");
            Map(x => x.RebateModified, "ZmienionoRabat");

            References(x => x.Order, "Zamowienie");
            References(x => x.Product, "Produkt");
        }
    }
}
