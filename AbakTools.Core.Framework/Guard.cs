using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Framework
{
    public static class Guard
    {
        public static void NotNull(object value, string name)
        {
            if (value == null)
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

        public static void NotEmpty(Guid value, string name)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"Argumet {name} cann't be empty");
            }
        }

        public static void GreaterThanZero(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentException($"Argument {name} must be greater than zero");
            }
        }
    }
}
