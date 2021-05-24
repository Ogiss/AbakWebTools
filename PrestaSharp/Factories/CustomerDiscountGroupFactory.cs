using Bukimedia.PrestaSharp.Entities;

namespace Bukimedia.PrestaSharp.Factories
{
    public class CustomerDiscountGroupFactory : GenericFactory<customer_discount_group>
    {
        public CustomerDiscountGroupFactory(string BaseUrl, string Account, string Password) : base(BaseUrl, Account, Password)
        {
        }

        protected override string singularEntityName => "customer_discount_group";

        protected override string pluralEntityName => "customer_discount_groups";
    }
}
