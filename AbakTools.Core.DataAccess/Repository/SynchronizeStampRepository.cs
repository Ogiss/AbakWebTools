using AbakTools.Core.Domain.Synchronize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbakTools.Core.DataAccess.Repository
{
    internal class SynchronizeStampRepository : GenericEntityRepository<SynchronizeStampEntity>, ISynchronizeStampRepository
    {
        public SynchronizeStampRepository(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        public SynchronizeStampEntity Get(string code, SynchronizeDirectionType type)
        {
            return GetQueryBase().SingleOrDefault(x => x.Code == code && x.Type == type);
        }
    }
}
