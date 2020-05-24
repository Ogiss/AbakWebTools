using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Framework
{
    public static class Guard
    {
        public static void NotNull(object value, string name)
        {
            if(value == null)
            {
                throw new ArgumentNullException($"Argument {name} cann't be null");
            }
        }

        public static void NotEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Argumet {name} cann't be null or empty");
            }
        }
    }
}
