using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Enova
{
    public interface IEnovaDiscountGroupRepository
    {
        Task<IEnumerable<EnovaApi.Models.DiscountGroup.DiscountGroup>> GetModifiedDiscountGroupAsync(int priceDefId, long stampFrom, long stampTo);
    }
}
