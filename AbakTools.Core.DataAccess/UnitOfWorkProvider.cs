using AbakTools.Core.Framework.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace AbakTools.Core.DataAccess
{
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private ISessionManager _sessionManager;

        public UnitOfWorkProvider(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public IUnitOfWork Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var uow = new UnitOfWork(_sessionManager, isolationLevel, readOnly: false);
            uow.Open();

            return uow;
        }

        public IUnitOfWork CreateReadOnly(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var uow = new UnitOfWork(_sessionManager, isolationLevel, readOnly: true);
            uow.Open();

            return uow;
        }
    }
}
