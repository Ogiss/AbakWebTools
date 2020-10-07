using AbakTools.Core.Domain.Order;
using AbakTools.Core.Domain.Order.Repositories;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class OrderRepository : GenericBusinessEntityRepository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public OrderEntity GetByWebId(int webId)
        {
            return GetQueryBase().SingleOrDefault(x => x.WebId == webId);
        }
    }
}
