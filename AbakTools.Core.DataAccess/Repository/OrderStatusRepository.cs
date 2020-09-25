using AbakTools.Core.Domain.Order;
using AbakTools.Core.Domain.Order.Repositories;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class OrderStatusRepository : GenericBusinessEntityRepository<OrderStatusEntity>, IOrderStatusRepository
    {
        public OrderStatusRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }
    }
}
