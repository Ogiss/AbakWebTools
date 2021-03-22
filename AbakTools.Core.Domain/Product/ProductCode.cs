using AbakTools.Core.Framework;

namespace AbakTools.Core.Domain.Product
{
    public record ProductCode
    {
        public string Code { get; }

        public ProductCode(string code)
        {
            Guard.NotEmpty(code, nameof(code));

            Code = code;
        }

        public static implicit operator ProductCode(string code)
        {
            return new ProductCode(code);
        }
    }
}
