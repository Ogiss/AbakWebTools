using AbakTools.Core.Framework;

namespace AbakTools.Core.DataAccess.Mappings
{
    class GuidedEntityMap<TEntity> : EntityMap<TEntity>
        where TEntity : IGuidedEntity
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Map(x => x.Guid);
        }
    }
}
