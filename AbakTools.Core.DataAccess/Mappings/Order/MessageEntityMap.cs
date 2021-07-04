using AbakTools.Core.Domain.Order;
using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.DataAccess.Mappings.Order
{
    internal class MessageEntityMap : GuidedEntity<MessageEntity>
    {
        protected override string CreationDateColumnName => "Stamp";
        protected override string ModificationDateColumnName => null;
        protected override string IsDeletedColumnName => null;

        public override void CreateMapping()
        {
            base.CreateMapping();
            Table("Wiadomosci");

            Map(x => x.WebId, "PSID");
            Map(x => x.Text, "Tekst").Not.Nullable();
            Map(x => x.IsPrivate, "Prywatna");
            Map(x => x.Synchronize, "Synchronizacja").CustomType<SynchronizeType>();

            References(x => x.Order, "Zamowienie");
        }
    }
}
