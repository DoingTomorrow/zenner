// Decompiled with JetBrains decompiler
// Type: NHibernate.ISession
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;
using System;
using System.Data;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate
{
  public interface ISession : IDisposable
  {
    EntityMode ActiveEntityMode { get; }

    void Flush();

    FlushMode FlushMode { get; set; }

    CacheMode CacheMode { get; set; }

    ISessionFactory SessionFactory { get; }

    IDbConnection Connection { get; }

    IDbConnection Disconnect();

    void Reconnect();

    void Reconnect(IDbConnection connection);

    IDbConnection Close();

    void CancelQuery();

    bool IsOpen { get; }

    bool IsConnected { get; }

    bool IsDirty();

    bool IsReadOnly(object entityOrProxy);

    void SetReadOnly(object entityOrProxy, bool readOnly);

    bool DefaultReadOnly { get; set; }

    object GetIdentifier(object obj);

    bool Contains(object obj);

    void Evict(object obj);

    object Load(System.Type theType, object id, LockMode lockMode);

    object Load(string entityName, object id, LockMode lockMode);

    object Load(System.Type theType, object id);

    T Load<T>(object id, LockMode lockMode);

    T Load<T>(object id);

    object Load(string entityName, object id);

    void Load(object obj, object id);

    void Replicate(object obj, ReplicationMode replicationMode);

    void Replicate(string entityName, object obj, ReplicationMode replicationMode);

    object Save(object obj);

    void Save(object obj, object id);

    object Save(string entityName, object obj);

    void SaveOrUpdate(object obj);

    void SaveOrUpdate(string entityName, object obj);

    void Update(object obj);

    void Update(object obj, object id);

    void Update(string entityName, object obj);

    object Merge(object obj);

    object Merge(string entityName, object obj);

    T Merge<T>(T entity) where T : class;

    T Merge<T>(string entityName, T entity) where T : class;

    void Persist(object obj);

    void Persist(string entityName, object obj);

    [Obsolete("Use Merge(object) instead")]
    object SaveOrUpdateCopy(object obj);

    [Obsolete("No direct replacement. Use Merge instead.")]
    object SaveOrUpdateCopy(object obj, object id);

    void Delete(object obj);

    void Delete(string entityName, object obj);

    int Delete(string query);

    int Delete(string query, object value, IType type);

    int Delete(string query, object[] values, IType[] types);

    void Lock(object obj, LockMode lockMode);

    void Lock(string entityName, object obj, LockMode lockMode);

    void Refresh(object obj);

    void Refresh(object obj, LockMode lockMode);

    LockMode GetCurrentLockMode(object obj);

    ITransaction BeginTransaction();

    ITransaction BeginTransaction(IsolationLevel isolationLevel);

    ITransaction Transaction { get; }

    ICriteria CreateCriteria<T>() where T : class;

    ICriteria CreateCriteria<T>(string alias) where T : class;

    ICriteria CreateCriteria(System.Type persistentClass);

    ICriteria CreateCriteria(System.Type persistentClass, string alias);

    ICriteria CreateCriteria(string entityName);

    ICriteria CreateCriteria(string entityName, string alias);

    IQueryOver<T, T> QueryOver<T>() where T : class;

    IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class;

    IQueryOver<T, T> QueryOver<T>(string entityName) where T : class;

    IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class;

    IQuery CreateQuery(string queryString);

    IQuery CreateFilter(object collection, string queryString);

    IQuery GetNamedQuery(string queryName);

    ISQLQuery CreateSQLQuery(string queryString);

    void Clear();

    object Get(System.Type clazz, object id);

    object Get(System.Type clazz, object id, LockMode lockMode);

    object Get(string entityName, object id);

    T Get<T>(object id);

    T Get<T>(object id, LockMode lockMode);

    string GetEntityName(object obj);

    IFilter EnableFilter(string filterName);

    IFilter GetEnabledFilter(string filterName);

    void DisableFilter(string filterName);

    IMultiQuery CreateMultiQuery();

    ISession SetBatchSize(int batchSize);

    ISessionImplementor GetSessionImplementation();

    IMultiCriteria CreateMultiCriteria();

    ISessionStatistics Statistics { get; }

    ISession GetSession(EntityMode entityMode);
  }
}
