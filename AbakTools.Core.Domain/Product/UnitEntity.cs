namespace AbakTools.Core.Domain.Product
{
    public class UnitEntity : GuidedEntity
    {
        public virtual int? WebId { get; set; }
        public virtual bool Default { get; set; }
        public virtual int Multiplier { get; set; }
        public virtual string Name { get; set; }
        public virtual long Stamp { get; set; }
    }
}
