using AbakTools.Core.Framework;
using NHibernate;
using NHibernate.Type;
using System;

namespace AbakTools.Core.DataAccess.Interceptors
{
    public class UpdateModificationDateInterceptor : EmptyInterceptor
    {
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            if (entity is IBusinessEntity entityWithModificationDate)
            {
                var now = DateTime.Now;

                var idx = Array.IndexOf(propertyNames, nameof(IBusinessEntity.ModificationDate));
                currentState[idx] = now;

                entityWithModificationDate.ModificationDate = now;
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }
    }
}
