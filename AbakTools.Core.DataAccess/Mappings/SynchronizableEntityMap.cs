using AbakTools.Core.Domain;
using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.DataAccess.Mappings
{
    internal class SynchronizableEntityMap<TEntity> : GuidedEntityMap<TEntity>
        where TEntity : SynchronizableEntity
    {
        protected virtual string IsDeleteColumn => "IsDeleted";
        protected virtual string StampColumn => "Stamp";

        public override void CreateMapping()
        {
            base.CreateMapping();

            Map(x => x.WebId);
            Map(x => x.Synchronize).CustomType<SynchronizeType>();
            Map(x => x.IsDeleted, IsDeleteColumn).Default("0");
            Map(x => x.Stamp).Formula($"CONVERT(BIGINT, {StampColumn})");
        }
    }
}
