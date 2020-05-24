using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Infrastructure.PrestaShop
{
    public class PsFilter
    {
        private Dictionary<string, string> filter = new Dictionary<string, string>();

        public static PsFilter Create(string key, object value)
        {
            var ps = new PsFilter();
            ps.filter.Add(key, value.ToString());

            return ps;
        }


        public PsFilter Add(string key, object value)
        {
            filter.Add(key, value.ToString());
            return this;
        }

        public static implicit operator Dictionary<string,string>(PsFilter psFilter)
        {
            return psFilter.filter;
        }
    }
}
