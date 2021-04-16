using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbakTools.Core.Domain.Enova
{
    public interface IEnovaDictionaryItemRepository
    {
        Task<IEnumerable<Guid>> GetDeletedItemsGuidsAsync(DateTime stampFrom, DateTime stampTo);
    }
}
