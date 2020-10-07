namespace AbakTools.Core.Domain.Order.Repositories
{
    public interface IOrderRepository : IGenericGuidedEntityRepository<OrderEntity>
    {
        OrderEntity GetByWebId(int webId);
    }
}
