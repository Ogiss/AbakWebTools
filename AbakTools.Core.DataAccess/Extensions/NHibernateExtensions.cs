﻿using AbakTools.Core.DataAccess;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services)
        {
            services.AddSingleton<ISessionManager>(new SessionManager());
            return services;
        }

        public static IServiceCollection InitializeNHibernate(this IServiceCollection services, string connectionString)
        {
            var sessionManager = services.BuildServiceProvider().GetService<ISessionManager>() as SessionManager;
            sessionManager?.Initialize(connectionString);
            return services;
        }
    }
}
