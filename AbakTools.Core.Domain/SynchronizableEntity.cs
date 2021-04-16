using AbakTools.Core.Framework;
using System;

namespace AbakTools.Core.Domain
{
    public abstract class SynchronizableEntity : GuidedEntity
    {
        public virtual int? WebId { get; protected set; }
        public virtual SynchronizeType Synchronize { get; protected set; } = SynchronizeType.New;
        public virtual bool IsDeleted { get; protected set; }
        public virtual long Stamp { get; protected set; }

        public virtual bool IsArchived => IsDeleted || Synchronize == SynchronizeType.Deleted;

        public virtual void SetWebId(int webId)
        {
            if (!WebId.HasValue)
            {
                Guard.GreaterThanZero(webId, nameof(webId));
                WebId = webId;
            }
            else if (webId != WebId.Value)
            {
                throw new InvalidOperationException("Can`t change setted value of WebId");
            }
        }

        public virtual void MakeSynchronized()
        {
            if (Synchronize == SynchronizeType.Deleted)
            {
                IsDeleted = true;
            }

            Synchronize = SynchronizeType.Synchronized;
        }

        public virtual void MakeEdited()
        {
            if (Synchronize == SynchronizeType.Synchronized)
            {
                Synchronize = SynchronizeType.Edited;
            }
        }

        public virtual void Delete()
        {
            if (!IsDeleted)
            {
                Synchronize = SynchronizeType.Deleted;
            }
        }
    }
}
