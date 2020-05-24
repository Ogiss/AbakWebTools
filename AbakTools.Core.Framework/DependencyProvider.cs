using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Framework
{
    public static class DependencyProvider
    {
        public static IServiceProvider ServiceProvider;

        public static T Resolve<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
    }
}
