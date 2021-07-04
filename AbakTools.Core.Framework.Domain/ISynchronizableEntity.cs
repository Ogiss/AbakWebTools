namespace AbakTools.Core.Framework.Domain
{
    public interface ISynchronizableEntity
    {
        SynchronizeType Synchronize { get; }
    }
}
