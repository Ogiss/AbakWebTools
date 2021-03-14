using EnovaApi.Models.DiscountGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Enova
{
    public interface IEnovaCustomerDiscountRepository
    {
        Task<IEnumerable<CustomerDiscountGroup>> GetModifiedCustomerDiscountsAsync(int priceDefId, long stampFrom, long stampTo);
    }
}
