using AbakTools.Core.Domain.Product;
using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.DataAccess.Mappings.Product
{
    class ImageEntityMap : GuidedEntityMap<ImageEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("image");

            Map(x => x.WebId, "id_image");
            Map(x => x.Legend, "legend");
            Map(x => x.Position, "position");
            Map(x => x.Cover, "cover");
            Map(x => x.Synchronize, "synchronize").CustomType<SynchronizeType>();
            Map(x => x.ImageBytes, "image_bytes").Length(int.MaxValue);

            References(x => x.Product, "id_local_product");
        }
    }
}
