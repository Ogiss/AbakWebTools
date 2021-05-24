namespace AbakTools.Core.Domain.Common
{
    public record Discount
    {
        public decimal Value { get; }

        protected Discount() { }

        private Discount(decimal value) => Value = value;

        public static Discount Of(decimal value)
        {
            return new Discount(value);
        }

        public static implicit operator decimal(Discount discount)
        {
            return discount.Value;
        }

        public static Discount operator +(Discount d1, Discount d2)
        {
            return new Discount(value: d1.Value + d2.Value);
        }

        public static Discount operator -(Discount d1, Discount d2)
        {
            return new Discount(value: d1.Value - d2.Value);
        }

        public override string ToString()
        {
            return $"{Value:P}";
        }
    }
}
