using AbakTools.Core.Domain.Product;
using AbakTools.Core.Framework.Domain;
using System;
using System.Security.Cryptography.X509Certificates;

namespace AbakTools.Core.DataAccess.Mappings.Product
{
    class ProductEntityMap : GuidedEntity<ProductEntity>
    {
        [Obsolete("To remove")]
        protected override string IsDeletedColumnName => "Usuniety";
        
        public override void CreateMapping()
        {
            base.CreateMapping();

            Table("product");

            Map(x => x.WebId, "id_product");
            Map(x => x.Price, "price");
            Map(x => x.Code, "reference");
            Map(x => x.SupplierCode, "KodDostawcy");
            Map(x => x.Active, "active");
            Map(x => x.Quantity, "Ilosc");
            Map(x => x.Description, "description");
            Map(x => x.DescriptionShort, "description_short");
            Map(x => x.LinkRewrite, "link_rewrite");
            Map(x => x.MetaDescription, "meta_description");
            Map(x => x.MetaTitle, "meta_title");
            Map(x => x.MetaWords, "MetaSlowa");
            Map(x => x.Name, "name");
            Map(x => x.EnovaGuid, "enova_guid");
            Map(x => x.Synchronize, "synchronize").CustomType<SynchronizeType>();
            Map(x => x.IsReady, "ready");
            Map(x => x.IsEnovaProduct, "TowarEnova");
            Map(x => x.IsAvailable, "is_available");
            Map(x => x.Order, "Kolejnosc");
            Map(x => x.FormOrder, "KolejnoscNaForm");
            Map(x => x.OrderIndex);
            Map(x => x.SearchIndex);
            Map(x => x.NotWebAvailable);
            Map(x => x.MinimumOrderQuantity);

            References(x => x.Tax, "id_tax");
            References(x => x.Unit, "IDJednostki");
            References(x => x.Supplier, "Dostawca");

            HasManyToMany(x => x.Categories)
                .Table("CategoryProductView")
                .ParentKeyColumn("ProductId")
                .ChildKeyColumn("CategoryId");

            HasMany(x => x.Images)
                .Inverse()
                .KeyColumn("id_local_product")
                .Cascade.SaveUpdate();

            HasMany(x => x.ProductDiscountGroups)
                .Inverse()
                .Table("TowarGrupaRabatowa")
                .KeyColumn("Towar")
                .Cascade.AllDeleteOrphan();
        }
    }
}
