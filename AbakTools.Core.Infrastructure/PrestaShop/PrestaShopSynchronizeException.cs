using System;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public class PrestaShopSynchronizeException : Exception
    {
        public PrestaShopSynchronizeException()
        {
        }

        public PrestaShopSynchronizeException(string message)
            : base(message)
        {
        }

        public static void TrowIf(bool condition, string message)
        {
            if (condition)
            {
                throw new PrestaShopSynchronizeException(message);
            }
        }

        public static void TrowIfNull(object obj, string message)
        {
            TrowIf(obj == null, message);
        }
    }
}
