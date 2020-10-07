using PsCustomer = Bukimedia.PrestaSharp.Entities.customer;

namespace AbakTools.Core.Infrastructure.PrestaShop.Repositories
{
    public interface IPsCustomerRepository : IPsRepositoryBase<PsCustomer>
    {
        PsCustomer GetByEmail(string email);
    }
}
