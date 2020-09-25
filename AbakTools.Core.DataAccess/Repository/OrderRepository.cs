using AbakTools.Core.Domain.Order;
using AbakTools.Core.Domain.Order.Repositories;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class OrderRepository : GenericBusinessEntityRepository<OrderEntity>, IOrderRepository
    {
        public OrderRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
