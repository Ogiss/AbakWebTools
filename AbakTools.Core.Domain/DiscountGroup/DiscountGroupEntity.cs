using AbakTools.Core.Framework;
using System;

namespace AbakTools.Core.Domain.DiscountGroup
{
    public class DiscountGroupEntity : GuidedEntity
    {
        public virtual string Category { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual byte[] EnovaStamp { get; protected set; }

        protected DiscountGroupEntity() { }

        public DiscountGroupEntity(Guid guid, string category, string name)
        {
            Guard.NotEmpty(guid, nameof(guid));
            Guard.NotEmpty(category, nameof(category));
            Guard.NotEmpty(name, nameof(name));

            Guid = guid;
            Category = category;
            Name = name;
            EnovaStamp = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        public virtual void SetName(string name)
        {
            Guard.NotEmpty(name, nameof(name));
            Name = name;
        }
    }
}
