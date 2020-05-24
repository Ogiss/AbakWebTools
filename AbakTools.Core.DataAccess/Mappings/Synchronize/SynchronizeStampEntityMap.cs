using AbakTools.Core.Domain.Synchronize;

namespace AbakTools.Core.DataAccess.Mappings.Synchronize
{
    class SynchronizeStampEntityMap : EntityMap<SynchronizeStampEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("SynchronizeInfo");

            Map(x => x.Code, "Kod").Not.Nullable();
            Map(x => x.Type, "Type").CustomType<SynchronizeDirectionType>();
            Map(x => x.DateTimeStamp, "Stamp");
            Map(x => x.Stamp, "BigIntStamp");
        }
    }
}
