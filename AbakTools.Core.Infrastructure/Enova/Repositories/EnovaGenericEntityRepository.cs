﻿using AbakTools.Core.Domain;
using AbakTools.Core.Framework;
using AbakTools.Core.Infrastructure.Enova.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbakTools.Core.Infrastructure.Enova.Repositories
{
    internal abstract class EnovaGenericEntityRepository<TEntity> : IGenericEntityRepository<TEntity>
        where TEntity : IEntity
    {
        protected IEnovaAPiClient Api { get; private set; }

        protected abstract string Resource { get; }

        protected EnovaGenericEntityRepository(IEnovaAPiClient enovaAPiClient)
        {
            Api = enovaAPiClient;
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public TEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<long> GetDbtsAsync()
        {
            return await Api.GetDbtsAsync();
        }

        public void SaveOrUpdate(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveOrUpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
