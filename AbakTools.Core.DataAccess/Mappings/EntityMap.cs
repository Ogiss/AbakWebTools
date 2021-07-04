using AbakTools.Core.Framework.Domain;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.DataAccess.Mappings
{
    class EntityMap<TEntity> : ClassMap<TEntity>, IEntityMap
        where TEntity : IEntity
    {
        public EntityMap()
        {
            CreateMapping();
        }

        public virtual void CreateMapping()
        {
            Id(x => x.Id);
        }
    }
}
