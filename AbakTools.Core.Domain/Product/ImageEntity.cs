using AbakTools.Core.Domain.Synchronize;
using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Product
{
    public class ImageEntity : BusinessEntity
    {
        public virtual int? WebId { get; set; }
        public virtual ProductEntity Product { get; set; }
        public virtual string Legend { get; set; }
        public virtual byte? Position { get; set; }
        public virtual bool? Cover { get; set; }
        public virtual SynchronizeType Synchronize { get; set; }
        public virtual byte[] ImageBytes { get; set; }

        public virtual bool IsArchived => Synchronize == SynchronizeType.Deleted || IsDeleted;
        public virtual bool IsEmpty => ImageBytes == null || ImageBytes.Length == 0;

        public virtual bool IsSynchronizable => !IsArchived && !IsEmpty;
    }
}
