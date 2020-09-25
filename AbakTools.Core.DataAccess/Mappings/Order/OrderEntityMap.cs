using AbakTools.Core.Domain.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.DataAccess.Mappings.Order
{
    class OrderEntityMap : BusinessEntityMap<OrderEntity>
    {
        protected override string CreationDateColumnName => "DataDodania";
        protected override string ModificationDateColumnName => "DataAtualizacji";
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
            Map(x => x.Synchronize, "Synchronizacja");
            Map(x => x.UrgentOrder, "Pilne");

            References(x => x.Customer, "Kontrahent");
            References(x => x.DeliveryAddress, "AdresWysylki");
            References(x => x.InvoiceAddress, "AdresFaktury");

            References(x => x.Status, "OstatniStatusID");
            //Map(x => x.OstatniStatusOpID);
            //Map(x => x.OstatniaHistoriaID);

            HasMany(x => x.History)
                .Inverse()
                .KeyColumn("Zamowienie")
                .Cascade.AllDeleteOrphan();

            HasMany(x => x.Items)
                .Inverse()
                .KeyColumn("Zamowienie")
                .Cascade.AllDeleteOrphan();
        }
    }
}
