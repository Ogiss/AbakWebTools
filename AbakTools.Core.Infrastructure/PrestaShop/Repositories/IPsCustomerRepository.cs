using PsCustomer = Bukimedia.PrestaSharp.Entities.customer;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    public interface IPSCustomerRepository : IPsRepositoryBase<PsCustomer>
    {
        PsCustomer GetByEmail(string email);
    }
}
