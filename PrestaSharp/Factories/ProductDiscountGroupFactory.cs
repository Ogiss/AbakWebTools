using Bukimedia.PrestaSharp.Entities;

namespace Bukimedia.PrestaSharp.Factories
{
    public class ProductDiscountGroupFactory : GenericFactory<product_discount_group>
    {
        protected override string singularEntityName => "product_discount_group";

        protected override string pluralEntityName => "product_discount_groups";

        public ProductDiscountGroupFactory(string BaseUrl, string Account, string Password) : base(BaseUrl, Account, Password)
        {
        }
    }
}
