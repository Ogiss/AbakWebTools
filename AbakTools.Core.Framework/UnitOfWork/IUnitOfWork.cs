using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbakTools.Core.Framework.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Open();
        void Close();
        void Flush();
        void Commit();
        Task CommitAsync();
    }
}
