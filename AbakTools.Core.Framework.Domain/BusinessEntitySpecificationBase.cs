namespace AbakTools.Core.Framework.Domain
{
    public abstract class BusinessEntitySpecificationBase<TEntity> : Specification<TEntity>
        where TEntity : IBusinessEntity
    {
        public bool WithDeleted { get; private set; }

        public BusinessEntitySpecificationBase<TEntity> GetWithDeleted()
        {
            WithDeleted = true;
            return this;
        }
    }
}
