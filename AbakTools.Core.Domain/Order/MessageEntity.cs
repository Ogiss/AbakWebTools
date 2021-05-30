using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Order
{
    public class MessageEntity : BusinessEntity
    {
        public virtual int? WebId { get; protected set; }
        public virtual OrderEntity Order { get; }
        public virtual string Text { get; protected set; }
        public virtual bool IsPrivate { get; protected set; }
        public virtual SynchronizeType Synchronize { get; protected set; }


        protected MessageEntity() { }

        public MessageEntity(OrderEntity order, string text, int? webId = null)
        {
            Guard.NotNull(order, nameof(order));
            Order = order;
            Text = text;
            WebId = webId;

            if (WebId.HasValue)
            {
                Synchronize = SynchronizeType.Synchronized;
            }
            else
            {
                Synchronize = SynchronizeType.New;
            }
        }

        public virtual void SetWebId(int webId)
        {
            WebId = webId;
        }

        public virtual void SetText(string text)
        {
            Text = text;
        }
    }
}
