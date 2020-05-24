using System.Data;

namespace AbakTools.Core.Framework.UnitOfWork
{
    public interface IUnitOfWorkProvider
    {
        IUnitOfWork Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        IUnitOfWork CreateReadOnly(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
