﻿using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.ClientOutbox;
using SFA.DAS.NServiceBus.SqlServer.ClientOutbox;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.UnitOfWork;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString));
            For<IDocumentClient>().Use(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IPermissionsRepository>().Use<PermissionsRepository>();
            For<ProviderRelationshipsDbContext>().Use(c => GetDbContext(c));
        }

        private ProviderRelationshipsDbContext GetDbContext(IContext context)
        {
            var unitOfWorkContext = context.GetInstance<IUnitOfWorkContext>();
            var clientSession = unitOfWorkContext.Find<IClientOutboxTransaction>();
            var serverSession = unitOfWorkContext.Find<SynchronizedStorageSession>();
            var sqlSession = clientSession?.GetSqlSession() ?? serverSession?.GetSqlSession() ?? throw new Exception("Cannot find the SQL session");
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseSqlServer(sqlSession.Connection);
                // add package reference to Microsoft.EntityFrameworkCore.Proxies, if we want to enable this...
                //.UseLazyLoadingProxies();
            var db = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            
            db.Database.UseTransaction(sqlSession.Transaction);
            
            return db;
        }
    }
}