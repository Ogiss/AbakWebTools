using System;
using System.Globalization;

namespace AbakTools.Core.Framework.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime? ParseInvariant(string str)
        {
            return DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) ? date : (DateTime?)null;
        }
    }
}
