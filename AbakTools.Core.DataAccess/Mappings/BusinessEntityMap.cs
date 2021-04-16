using AbakTools.Core.Domain;

namespace AbakTools.Core.DataAccess.Mappings
{
    class GuidedEntity<TEntity> : GuidedEntityMap<TEntity>
        where TEntity : BusinessEntity
    {
        protected virtual string IsDeletedColumnName => "deleted";
        protected virtual string CreationDateColumnName => "date_add";
        protected virtual string ModificationDateColumnName => "date_upd";

        public override void CreateMapping()
        {
            base.CreateMapping();

            if (!string.IsNullOrEmpty(IsDeletedColumnName))
            {
                Map(x => x.IsDeleted, IsDeletedColumnName);
            }

            if (!string.IsNullOrEmpty(CreationDateColumnName))
            {
                Map(x => x.CreationDate, CreationDateColumnName);
            }

            if (!string.IsNullOrEmpty(ModificationDateColumnName))
            {
                Map(x => x.ModificationDate, ModificationDateColumnName);
            }
        }
    }
}
