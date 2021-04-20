using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbakTools.Core.Framework.Helpers.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null && action != null)
            {
                foreach (var item in enumerable)
                {
                    action(item);
                }
            }

            return enumerable;
        }
    }
}
