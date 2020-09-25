using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    class PrestaShopClient : IPrestaShopClient
    {
        private readonly IConfiguration configuration;
        private string baseUrl;
        private string key;
        private string password;
        private long RootCategoryId = 2;
        private string defaultCountryIsoCode = "PL";
        private language defaultLanguage;
        private country defaultCountry;
        private CountryFactory countryFactory;
        private SupplierFactory supplierFactory;
        private CategoryFactory categoryFactory;
        private LanguageFactory languageFactory;
        private ProductFactory productFactory;
        private ImageFactory imageFactory;
        private TaxRuleGroupFactory taxRuleGroupFactory;
        private TaxRuleFactory taxRuleFactory;
        private TaxFactory taxFactory;
        private CustomerFactory customerFactory;
        private AddressFactory addressFactory;
        

        public Bukimedia.PrestaSharp.Entities.language DefaultLanguage
        {
            get
            {
                if (defaultLanguage == null)
                {
                    defaultLanguage = GetDefaultLanguange();
                }

                return defaultLanguage;
            }
        }

        public country DefaultCountry
        {
            get
            {
                if (defaultCountry == null)
                {
                    defaultCountry = CountryFactory.GetByFilter(new Dictionary<string, string> { { "iso_code", defaultCountryIsoCode } }, null, null).SingleOrDefault();
                }

                return defaultCountry;
            }
        }

        public CountryFactory CountryFactory
        {
            get
            {
                if(countryFactory == null)
                {
                    countryFactory = new CountryFactory(baseUrl, key, password);
                }

                return countryFactory;
            }
        }

        public LanguageFactory LanguageFactory
        {
            get
            {
                if (languageFactory == null)
                {
                    languageFactory = new LanguageFactory(baseUrl, key, password);
                }

                return languageFactory;
            }
        }

        public SupplierFactory SupplierFactory
        {
            get
            {
                if (supplierFactory == null)
                {
                    supplierFactory = new SupplierFactory(baseUrl, key, password);
                }

                return supplierFactory;
            }
        }

        public CategoryFactory CategoryFactory
        {
            get
            {
                if (categoryFactory == null)
                {
                    categoryFactory = new CategoryFactory(baseUrl, key, password);
                }

                return categoryFactory;
            }
        }

        public ProductFactory ProductFactory
        {
            get
            {
                if (productFactory == null)
                {
                    productFactory = new ProductFactory(baseUrl, key, password);
                }

                return productFactory;
            }
        }

        public ImageFactory ImageFactory
        {
            get
            {
                if (imageFactory == null)
                {
                    imageFactory = new ImageFactory(baseUrl, key, password);
                }

                return imageFactory;
            }
        }

        public TaxRuleGroupFactory TaxRuleGroupFactory
        {
            get
            {
                if(taxRuleGroupFactory == null)
                {
                    taxRuleGroupFactory = new TaxRuleGroupFactory(baseUrl, key, password);
                }

                return taxRuleGroupFactory;
            }
        }

        public TaxRuleFactory TaxRuleFactory
        {
            get
            {
                if (taxRuleFactory == null)
                {
                    taxRuleFactory = new TaxRuleFactory(baseUrl, key, password);
                }

                return taxRuleFactory;
            }
        }

        public TaxFactory TaxFactory
        {
            get
            {
                if (taxFactory == null)
                {
                    taxFactory = new TaxFactory(baseUrl, key, password);
                }

                return taxFactory;
            }
        }

        public CustomerFactory CustomerFactory
        {
            get
            {
                if(customerFactory == null)
                {
                    customerFactory = new CustomerFactory(baseUrl, key, password);
                }

                return customerFactory;
            }
        }

        public AddressFactory AddressFactory
        {
            get
            {
                if(addressFactory == null)
                {
                    addressFactory = new AddressFactory(baseUrl, key, password);
                }

                return addressFactory;
            }
        }

        public PrestaShopClient(IConfiguration _configuration)
        {
            configuration = _configuration;
            baseUrl = configuration["PrestaShop:BaseUrl"];
            key = configuration["PrestaShop:Key"];
            password = configuration["PrestaShop:Password"];
        }

        public void SetLangValue<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression, string value)
            where TEntity : PrestaShopEntity
            where TProperty : List<Bukimedia.PrestaSharp.Entities.AuxEntities.language>
        {
            var propertyInfo = (PropertyInfo)((MemberExpression)expression.Body).Member;
            var valueList = (TProperty)propertyInfo.GetValue(entity);

            var langValue = valueList.SingleOrDefault(x => x.id == DefaultLanguage.id);
            if (langValue == null)
            {
                langValue = new Bukimedia.PrestaSharp.Entities.AuxEntities.language((long)DefaultLanguage.id, value);
                valueList.Add(langValue);
            }
            else
            {
                langValue.Value = value;
            }
        }

        public category GetRootCategory(int? shopId = null)
        {
            return CategoryFactory.Get(RootCategoryId);
        }

        public tax_rule_group GetTaxRuleGroupByRate(decimal rate)
        {
            var taxRules = TaxRuleFactory.GetByFilter(PsFilter.Create("id_country", DefaultCountry.id), null, null);

            foreach(var taxRule in taxRules)
            {
                if (taxRule.id_tax.HasValue && taxRule.id_tax_rules_group.HasValue)
                {
                    var tax = TaxFactory.Get(taxRule.id_tax.Value);

                    if (tax.rate == rate)
                    {
                        return TaxRuleGroupFactory.Get(taxRule.id_tax_rules_group.Value);
                    }
                }
            }

            return null;
        }

        private Bukimedia.PrestaSharp.Entities.language GetDefaultLanguange()
        {
            Dictionary<string, string> dtn = new Dictionary<string, string>();
            dtn.Add("iso_code", "pl");

            return LanguageFactory.GetByFilter(dtn, null, null).Single();
        }

    }
}
