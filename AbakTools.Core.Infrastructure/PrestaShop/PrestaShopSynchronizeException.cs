using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
