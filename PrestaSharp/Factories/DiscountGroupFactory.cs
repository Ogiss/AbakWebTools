using Bukimedia.PrestaSharp.Entities;

namespace Bukimedia.PrestaSharp.Factories
{
    public class DiscountGroupFactory : GenericFactory<discount_group>
    {
        protected override string singularEntityName => "discount_group";
        protected override string pluralEntityName => "discount_groups";

        public DiscountGroupFactory(string BaseUrl, string Account, string Password) : base(BaseUrl, Account, Password)
        {
        }
    }
}
