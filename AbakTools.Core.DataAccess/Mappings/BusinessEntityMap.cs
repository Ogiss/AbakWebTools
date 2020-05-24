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
        public override void CreateMapping()
        {
            base.CreateMapping();

            Map(x => x.IsDeleted, IsDeletedColumnName);
            Map(x => x.CreationDate, "date_add");
            Map(x => x.ModificationDate, "date_upd");
        }
    }
}
