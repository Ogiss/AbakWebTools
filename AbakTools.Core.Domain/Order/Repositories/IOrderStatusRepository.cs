namespace AbakTools.Core.Domain.Order.Repositories
{
    public interface IOrderStatusRepository : IGenericEntityRepository<OrderStateEntity>
    {
        OrderStateEntity GetByWebId(int webId);

        OrderStateEntity GetDefault();
    }
}
