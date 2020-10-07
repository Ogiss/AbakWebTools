using AbakTools.Core.Domain.Order;
using AbakTools.Core.Domain.Order.Repositories;
using System.Linq;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class OrderStatusRepository : GenericEntityRepository<OrderStateEntity>, IOrderStatusRepository
    {
        public OrderStatusRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public OrderStateEntity GetByWebId(int webId)
        {
            return GetQueryBase().SingleOrDefault(x => x.WebId == webId);
        }

        public OrderStateEntity GetDefault()
        {
            return GetQueryBase().SingleOrDefault(x => x.NewOrder == true);
        }
    }
}
