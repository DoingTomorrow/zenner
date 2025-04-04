// Decompiled with JetBrains decompiler
// Type: NHibernate.IStatelessSession
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;
using System.Data;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate
{
  public interface IStatelessSession : IDisposable
  {
    IDbConnection Connection { get; }

    ITransaction Transaction { get; }

    bool IsOpen { get; }

    bool IsConnected { get; }

    ISessionImplementor GetSessionImplementation();

    void Close();

    object Insert(object entity);

    object Insert(string entityName, object entity);

    void Update(object entity);

    void Update(string entityName, object entity);

    void Delete(object entity);

    void Delete(string entityName, object entity);

    object Get(string entityName, object id);

    T Get<T>(object id);

    object Get(string entityName, object id, LockMode lockMode);

    T Get<T>(object id, LockMode lockMode);

    void Refresh(object entity);

    void Refresh(string entityName, object entity);

    void Refresh(object entity, LockMode lockMode);

    void Refresh(string entityName, object entity, LockMode lockMode);

    IQuery CreateQuery(string queryString);

    IQuery GetNamedQuery(string queryName);

    ICriteria CreateCriteria<T>() where T : class;

    ICriteria CreateCriteria<T>(string alias) where T : class;

    ICriteria CreateCriteria(Type entityType);

    ICriteria CreateCriteria(Type entityType, string alias);

    ICriteria CreateCriteria(string entityName);

    ICriteria CreateCriteria(string entityName, string alias);

    IQueryOver<T, T> QueryOver<T>() where T : class;

    IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class;

    ISQLQuery CreateSQLQuery(string queryString);

    ITransaction BeginTransaction();

    ITransaction BeginTransaction(IsolationLevel isolationLevel);

    IStatelessSession SetBatchSize(int batchSize);
  }
}
