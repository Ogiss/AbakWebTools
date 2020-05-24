using AbakTools.Core.Framework;
using System;

namespace AbakTools.Core.Domain
{
    public class Entity : IEntity
    {
        public virtual int Id { get; protected set; }
    }
}
