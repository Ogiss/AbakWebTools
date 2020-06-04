using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.DataAccess.Mappings
{
    class BusinessEntityMap<TEntity> : GuidedEntityMap<TEntity>
        where TEntity : IBusinessEntity
    {
        protected virtual string IsDeletedColumnName => "deleted";
        protected virtual string CreationDateColumnName => "date_add";
        protected virtual string ModificationDateColumnName => "date_upd";

        public override void CreateMapping()
        {
            base.CreateMapping();

            Map(x => x.IsDeleted, IsDeletedColumnName);

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
