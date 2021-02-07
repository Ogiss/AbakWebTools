using AbakTools.Core.Domain.Synchronize;
using System;
using System.Collections.Generic;
using NHibernate.Linq;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class SynchronizeStampRepository : GenericEntityRepository<SynchronizeStampEntity>, ISynchronizeStampRepository
    {
        public SynchronizeStampRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public SynchronizeStampEntity Get(string code, SynchronizeDirectionType type)
        {
            return GetAsync(code, type).Result;
        }

        public async Task<SynchronizeStampEntity> GetAsync(string code, SynchronizeDirectionType type, CancellationToken cancellationToken = default)
        {
            return await GetQueryBase().SingleOrDefaultAsync(x => x.Code == code && x.Type == type);
        }
    }
}
