using Bukimedia.PrestaSharp.Factories;
using System.Collections.Generic;
using System.Linq;
using PsCustomer = Bukimedia.PrestaSharp.Entities.customer;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories.Implementations
{
    internal class PSCustomerRepository : PsRepositoryBase<PsCustomer>, IPSCustomerRepository
    {
        protected override GenericFactory<PsCustomer> Factory => PrestaShopClient.CustomerFactory;

        public PSCustomerRepository(IPrestaShopClient prestaShopClient) : base(prestaShopClient)
        {
        }

        public PsCustomer GetByEmail(string email)
        {
            return GetByFilter(new { email }).SingleOrDefault();
        }
    }
}
