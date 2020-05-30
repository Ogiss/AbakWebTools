using AbakTools.Core.Domain.Customer;
using AbakTools.Core.Framework;

namespace AbakTools.Core.DataAccess.Mappings
{
    class CustomerEntityMap : BusinessEntityMap<CustomerEntity>
    {
        protected override string IsDeletedColumnName => "Usuniety";
        protected override string CreationDateColumnName => "CreationDate";
        protected override string ModificationDateColumnName => "ModificationDate";

        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("Kontrahenci");

            Map(x => x.WebId, "PSID");
            Map(x => x.Code, "Kod");
            Map(x => x.Name, "Nazwa");
            Map(x => x.Nip, "Nip");
            Map(x => x.Email, "Email");
            Map(x => x.Active, "Aktywny");
            Map(x => x.Synchronize, "Synchronizacja").CustomType<SynchronizeType>();
            Map(x => x.WebAccountLogin);
            Map(x => x.WebAccountPassword);

            HasMany(x => x.Addresses)
                .Inverse()
                .KeyColumn("Kontrahent")
                .Cascade.AllDeleteOrphan();
        }
    }
}
