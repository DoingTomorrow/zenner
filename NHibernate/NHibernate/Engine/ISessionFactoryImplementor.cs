// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.ISessionFactoryImplementor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect.Function;
using NHibernate.Engine.Query;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Stat;
using NHibernate.Transaction;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Engine
{
  public interface ISessionFactoryImplementor : IMapping, ISessionFactory, IDisposable
  {
    NHibernate.Dialect.Dialect Dialect { get; }

    IInterceptor Interceptor { get; }

    QueryPlanCache QueryPlanCache { get; }

    IConnectionProvider ConnectionProvider { get; }

    ITransactionFactory TransactionFactory { get; }

    UpdateTimestampsCache UpdateTimestampsCache { get; }

    IStatisticsImplementor StatisticsImplementor { get; }

    ISQLExceptionConverter SQLExceptionConverter { get; }

    Settings Settings { get; }

    IEntityNotFoundDelegate EntityNotFoundDelegate { get; }

    SQLFunctionRegistry SQLFunctionRegistry { get; }

    IDictionary<string, ICache> GetAllSecondLevelCacheRegions();

    IEntityPersister GetEntityPersister(string entityName);

    ICollectionPersister GetCollectionPersister(string role);

    IType[] GetReturnTypes(string queryString);

    string[] GetReturnAliases(string queryString);

    string[] GetImplementors(string entityOrClassName);

    string GetImportedClassName(string name);

    IQueryCache QueryCache { get; }

    IQueryCache GetQueryCache(string regionName);

    NamedQueryDefinition GetNamedQuery(string queryName);

    NamedSQLQueryDefinition GetNamedSQLQuery(string queryName);

    ResultSetMappingDefinition GetResultSetMapping(string resultSetRef);

    IIdentifierGenerator GetIdentifierGenerator(string rootEntityName);

    ICache GetSecondLevelCacheRegion(string regionName);

    ISession OpenSession(
      IDbConnection connection,
      bool flushBeforeCompletionEnabled,
      bool autoCloseSessionEnabled,
      ConnectionReleaseMode connectionReleaseMode);

    ISet<string> GetCollectionRolesByEntityParticipant(string entityName);

    ICurrentSessionContext CurrentSessionContext { get; }

    IEntityPersister TryGetEntityPersister(string entityName);

    string TryGetGuessEntityName(System.Type implementor);
  }
}
