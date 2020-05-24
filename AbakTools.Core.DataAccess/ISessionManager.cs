using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.DataAccess
{
    public interface ISessionManager
    {
        ISession CurrentSession { get; }
        ISession OpenSession();
        void CloseSession();
    }
}
