using System;

namespace AbakTools.Core.Domain.Synchronize
{
    public class SynchronizeStampEntity : Entity
    {
        public virtual string Code { get; protected set; }
        public virtual SynchronizeDirectionType Type { get; protected set; }
        public virtual DateTime? DateTimeStamp { get; set; }
        public virtual long? Stamp { get; set; }

        protected SynchronizeStampEntity() { }

        internal SynchronizeStampEntity(string code, SynchronizeDirectionType type)
        {
            Code = code;
            Type = type;
        }
    }
}
