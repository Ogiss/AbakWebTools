using AbakTools.Core.DataAccess.Interceptors;
using AbakTools.Core.DataAccess.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Context;
using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Text;
using NHibernateConfig = NHibernate.Cfg.Configuration;

namespace AbakTools.Core.DataAccess
{
    class SessionManager : ISessionManager
    {
        //private readonly ILogger _logger;

        private ISessionFactory _sessionFactory;

        public NHibernateConfig Configuration { get; private set; }
        public bool Initialized => _sessionFactory != null;
        public ISession CurrentSession => _sessionFactory?.GetCurrentSession();

        public SessionManager()
        {
        }

        public void Initialize(string connectionString)
        {
            if (Initialized)
            {
                throw new InvalidOperationException("database already initialized.");
            }

            Configuration = CreateConfiguration(connectionString);
            _sessionFactory = Configuration.BuildSessionFactory();

            //_logger.LogInformation("NHibernate initialized");
        }

        public ISession OpenSession()
        {
            if (!CurrentSessionContext.HasBind(_sessionFactory))
            {
                var sessionBuilder = _sessionFactory.WithOptions()
                    .Interceptor(new UpdateModificationDateInterceptor());

                CurrentSessionContext.Bind(sessionBuilder.OpenSession());
            }

            return CurrentSession;
        }

        public void CloseSession()
        {
            var currentSession = CurrentSessionContext.Unbind(_sessionFactory);

            if (currentSession != null)
            {
                currentSession.Close();
                currentSession.Dispose();
            }
        }

        private NHibernateConfig CreateConfiguration(string connectionString)
        {
            var fluentConfig = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<IEntityMap>())
                .CurrentSessionContext<ThreadStaticSessionContext>();
                

            try
            {
                return fluentConfig.BuildConfiguration();
            }
            catch (FluentConfigurationException e)
            {
                //_logger.LogError(e.InnerException.ToString());
                throw;
            }
        }
    }
}
