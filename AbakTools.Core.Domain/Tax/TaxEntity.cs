namespace AbakTools.Core.Domain.Tax
{
    public class TaxEntity : GuidedEntity
    {
        public virtual int? WebId { get; set; }
        public virtual decimal Rate { get; set; }
        public virtual string Name { get; set; }
    }
}
