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
            if (entity is IBusinessEntity businessEntity && !businessEntity.DisableUpdateModificationDate)
            {
                businessEntity.ModificationDate = DateTime.Now;
                UpdateState(nameof(businessEntity.ModificationDate), businessEntity.ModificationDate, ref currentState, propertyNames);
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (entity is IBusinessEntity businessEntity)
            {
                if (businessEntity.Guid == Guid.Empty)
                {
                    businessEntity.Guid = Guid.NewGuid();
                    UpdateState(nameof(businessEntity.Guid), businessEntity.Guid, ref state, propertyNames);
                }

                if (businessEntity.CreationDate == DateTime.MinValue)
                {
                    businessEntity.CreationDate = DateTime.Now;
                    UpdateState(nameof(businessEntity.CreationDate), businessEntity.CreationDate, ref state, propertyNames);
                }

                if (!businessEntity.DisableUpdateModificationDate)
                {
                    businessEntity.ModificationDate = DateTime.Now;
                    UpdateState(nameof(businessEntity.ModificationDate), businessEntity.ModificationDate, ref state, propertyNames);
                }
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        private void UpdateState(string propertyName, object value, ref object[] state, string[] propertyNames)
        {
            var idx = Array.IndexOf(propertyNames, propertyName);

            if (idx >= 0 && idx < state.Length)
            {
                state[idx] = value;
            }
        }
    }
}
