using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public interface IPrestaShopClient
    {
        language DefaultLanguage { get; }
        country DefaultCountry { get; }
        SupplierFactory SupplierFactory { get; }
        CategoryFactory CategoryFactory { get; }
        ProductFactory ProductFactory { get; }
        ImageFactory ImageFactory { get; }
        CustomerFactory CustomerFactory { get; }
        AddressFactory AddressFactory { get; }
        OrderFactory OrderFactory { get; }

        category GetRootCategory(int? shopId = null);
        void SetLangValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression, string value)
            where TEntity : PrestaShopEntity
            where TProperty : List<Bukimedia.PrestaSharp.Entities.AuxEntities.language>;

        tax_rule_group GetTaxRuleGroupByRate(decimal rate);
    }
}
