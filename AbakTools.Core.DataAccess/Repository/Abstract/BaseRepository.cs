using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public void Flush()
        {
            CurrentSession?.Flush();
        }

        public long GetDbts()
        {
            return CurrentSession
                .CreateSQLQuery("SELECT CONVERT(BIGINT, @@DBTS) DBTS")
                .UniqueResult<long>();
        }

        public async Task<long> GetDbtsAsync()
        {
            return await CurrentSession
                .CreateSQLQuery("SELECT CONVERT(BIGINT, @@DBTS) DBTS")
                .UniqueResultAsync<long>();
        }
    }
}
