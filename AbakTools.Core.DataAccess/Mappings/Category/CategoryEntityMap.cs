using AbakTools.Core.Domain.Category;
using AbakTools.Core.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.DataAccess.Mappings.Category
{
    class CategoryEntityMap : GuidedEntity<CategoryEntity>
    {
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("category");

            Map(x => x.LevelDepth, "level_depth");
            Map(x => x.Active, "active");
            Map(x => x.Name, "name");
            Map(x => x.Description, "description");
            Map(x => x.LinkRewrite, "link_rewrite");
            Map(x => x.MetaTitle, "meta_title");
            Map(x => x.MetaDescription, "meta_description");
            Map(x => x.MetaKeywords, "meta_keywords");
            Map(x => x.Synchronize).CustomType(typeof(SynchronizeType));
            Map(x => x.DisplayOrder, "display_order");
            Map(x => x.EnovaFeature, "enova_feature");
            Map(x => x.WebId, "id_category");
            Map(x => x.DateTimeStamp, "stamp");

            References(x => x.Parent, "id_local_parent")
                .Cascade.None()
                .LazyLoad();
        }
    }
}
