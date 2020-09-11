using System;
using System.Collections.Generic;
using System.Text;

namespace AbakTools.Core.Domain
{
    public class DisableUpdateModyficationDate : IDisposable
    {
        private bool disposed;
        private BusinessEntity businessEntity;

        public DisableUpdateModyficationDate(BusinessEntity businessEntity)
        {
            this.businessEntity = businessEntity;
            businessEntity.DisableUpdateModificationDate = true;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                businessEntity.DisableUpdateModificationDate = false;
            }
        }
    }
}
