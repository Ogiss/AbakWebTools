using AbakTools.Core.Domain.Order;
using AbakTools.Core.Framework;

namespace AbakTools.Core.DataAccess.Mappings.Order
{
    class OrderEntityMap : GuidedEntity<OrderEntity>
    {
        protected override string CreationDateColumnName => "DataDodania";
        protected override string ModificationDateColumnName => "DataAtualizacji";
        protected override string IsDeletedColumnName => "IsDeleted";

        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("Zamowienia");

            Map(x => x.WebId, "PSID");
            Map(x => x.TotalProductsValueNet, "TotalProduktyNetto");
            Map(x => x.TotalProductsValueGross, "TotalProduktyBrutto");
            Map(x => x.TotalValueNet, "TotalNetto");
            Map(x => x.TotalValueGross, "TotalBrutto");
            Map(x => x.Packer, "Pakowacz");
            Map(x => x.Transport).CustomType<TransportType>();
            Map(x => x.DeliveryDate, "NaKiedy");
            Map(x => x.DeliveryTimeOfDay, "NaKiedyTyp");
            Map(x => x.Term, "Termin");
            Map(x => x.InvoiceGuid, "FakturaGuid");
            Map(x => x.InvoiceNumber, "FakturaNumer");
            Map(x => x.Synchronize, "Synchronizacja").CustomType<SynchronizeType>();
            Map(x => x.UrgentOrder, "Pilne");
            Map(x => x.OrderSource, "ZrodloKod");

            References(x => x.Customer, "Kontrahent");
            References(x => x.DeliveryAddress, "AdresWysylki");
            References(x => x.InvoiceAddress, "AdresFaktury");

            References(x => x.State, "OstatniStatusID");
            //Map(x => x.OstatniStatusOpID);
            //Map(x => x.OstatniaHistoriaID);

            HasMany(x => x.History)
                .Inverse()
                .KeyColumn("Zamowienie")
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.Rows)
                .Inverse()
                .KeyColumn("Zamowienie")
                .Cascade.AllDeleteOrphan();
        }
    }
}
