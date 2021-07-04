using NHibernate;

namespace AbakTools.Core.DataAccess
{
    public interface ISessionManager
    {
        ISession CurrentSession { get; }
        ISession OpenSession();
        void CloseSession();
    }
}
