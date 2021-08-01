using Bukimedia.PrestaSharp.Entities;

namespace Bukimedia.PrestaSharp.Factories
{
    public class UnitFactory : GenericFactory<unit>
    {
        public UnitFactory(string BaseUrl, string Account, string Password) : base(BaseUrl, Account, Password)
        {
        }

        protected override string singularEntityName => "unit";
        protected override string pluralEntityName => "units";
    }
}
