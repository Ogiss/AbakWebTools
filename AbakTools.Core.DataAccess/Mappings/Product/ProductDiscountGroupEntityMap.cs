using AbakTools.Core.Domain.Product;
using AbakTools.Core.Framework.Domain;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbakTools.Core.DataAccess.Mappings.Product
{
    class ProductDiscountGroupEntityMap : ClassMap<ProductDiscountGroupEntity>
    {
        public ProductDiscountGroupEntityMap()
        {
            Table("TowarGrupaRabatowa");

            CompositeId()
                .KeyReference(x => x.Product, "Towar")
                .KeyReference(x => x.DiscountGroup, "GrupaRabatowa");

            Map(x => x.IsDeleted, "Deleted");
            Map(x => x.Synchronize).CustomType<SynchronizeType>();
        }
    }
}
