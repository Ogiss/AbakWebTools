namespace AbakTools.Core.Domain
{
    public interface IGenericSynchronizableEntityRepository<TEntity> : IGenericGuidedEntityRepository<TEntity>
        where TEntity : SynchronizableEntity
    {
        TEntity GetWithArchived(int id);
    }
}
