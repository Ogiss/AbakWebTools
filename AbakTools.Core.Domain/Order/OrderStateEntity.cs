using AbakTools.Core.Framework;
using System;

namespace AbakTools.Core.Domain.Order
{
    public class OrderStateEntity : Entity
    {
        public virtual int? WebId { get; set; }
        public virtual string Name { get; set; }

        [Obsolete("To remove")]
        public virtual bool NewOrder { get; set; }
        //public virtual SynchronizeType Synchronize { get; set; }
    }
}
