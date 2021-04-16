using AbakTools.Core.Domain.Address;
using AbakTools.Core.Framework;

namespace AbakTools.Core.DataAccess.Mappings
{
    class AddressEntityMap : GuidedEntity<AddressEntity>
    {
        protected override string IsDeletedColumnName => "Usuniety";
        protected override string CreationDateColumnName => "CreationDate";
        protected override string ModificationDateColumnName => "ModificationDate";

        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("Adresy");

            Map(x => x.WebId, "PSID");
            Map(x => x.Active, "Aktywny");
            Map(x => x.AddressLine1, "Adres1");
            Map(x => x.AddressLine2, "Adres2");
            Map(x => x.PostalCode, "KodPocztowy");
            Map(x => x.City, "Miasto");
            Map(x => x.Name, "Firma");
            Map(x => x.FirstName, "Imie");
            Map(x => x.LastName, "Nazwisko");
            Map(x => x.Phone, "Telefon");
            Map(x => x.MobilePhone, "TelefonKomorkowy");
            Map(x => x.Synchronize, "Synchronizacja").CustomType<SynchronizeType>();
            Map(x => x.IsDefaultInvoiceAddress, "DomyslnyAdresFaktury");
            Map(x => x.IsDefaultDeliveryAddress, "DomyslnyAdresWysylki");

            References(x => x.Customer, "Kontrahent");
        }
}
}
