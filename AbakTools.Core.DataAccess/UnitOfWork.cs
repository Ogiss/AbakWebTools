using AbakTools.Core.Framework.UnitOfWork;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace AbakTools.Core.DataAccess
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly bool readOnly;
        private readonly IsolationLevel isolationLevel;
        private readonly ISessionManager sessionManager;

        private ITransaction transaction;

        public bool IsOpen { get; private set; }

        public UnitOfWork(ISessionManager sessionManager, IsolationLevel isolationLevel, bool readOnly)
        {
            this.isolationLevel = isolationLevel;
            this.readOnly = readOnly;
            this.sessionManager = sessionManager;
        }

        public void Open()
        {
            var session = sessionManager.OpenSession();

            if (readOnly)
            {
                session.FlushMode = FlushMode.Manual;
            }
            else
            {
                session.FlushMode = FlushMode.Commit;
            }

            transaction = session.BeginTransaction(isolationLevel);

            IsOpen = true;
        }

        public void Close()
        {
            if (transaction != null)
            {
                if (!transaction.WasCommitted && !transaction.WasRolledBack)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (NHibernate.TransactionException)
                    {
                    }
                }
            }

            sessionManager.CloseSession();

            IsOpen = false;
            transaction = null;
        }

        public void Flush()
        {
            sessionManager.CurrentSession?.Flush();
        }

        public void Commit()
        {
            if (readOnly)
            {
                throw new InvalidOperationException("Cannot commit a read-only unit of work.");
            }

            if (transaction.IsActive)
            {
                transaction.Commit();
            }
        }

        public async Task CommitAsync()
        {
            if (readOnly)
            {
                throw new InvalidOperationException("Cannot commit a read-only unit of work.");
            }

            if (transaction.IsActive)
            {
                await transaction.CommitAsync();
            }
        }

        public void Dispose()
        {
            if (IsOpen)
            {
                Close();
            }
        }
    }
}
