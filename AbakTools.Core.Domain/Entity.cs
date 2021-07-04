using AbakTools.Core.Framework.Domain;

namespace AbakTools.Core.Domain
{
    public class Entity : IEntity
    {
        public virtual int Id { get; protected set; }
    }
}
