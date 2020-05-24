using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.DataAccess.Repository
{
    class BaseRepository
    {
        private ISessionManager _sessionManager;

        public ISession CurrentSession => _sessionManager?.CurrentSession;

        protected BaseRepository(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public long GetDbts()
        {
            return CurrentSession
                .CreateSQLQuery("SELECT CONVERT(BIGINT, @@DBTS) DBTS")
                .UniqueResult<long>();
        }

        public void Flush()
        {
            CurrentSession?.Flush();
        }
    }
}
