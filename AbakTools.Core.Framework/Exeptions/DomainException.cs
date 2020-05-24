using System;

namespace AbakTools.Core.Framework.Exeptions
{
    public class DomainException : Exception
    {
        public DomainExeptionCode Code { get; private set; }


        public DomainException(DomainExeptionCode code)
        {
            Code = code;
        }

        public DomainException(DomainExeptionCode code, string message) : base(message)
        {
            Code = code;
        }

        public DomainException(DomainExeptionCode code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        public static void ThrowIf(bool condition, DomainExeptionCode code, string message = null, Exception innerException = null)
        {
            if (condition)
            {
                throw new DomainException(code, message, innerException);
            }
        }
    }
}
