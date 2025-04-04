// Decompiled with JetBrains decompiler
// Type: MSS.Data.Repository.Repository`1
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using AutoMapper;
using Common.Library.NHibernate.Data;
using Common.Library.NHibernate.Data.Extensions;
using Microsoft.CSharp.RuntimeBinder;
using MSS.Data.Utils;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace MSS.Data.Repository
{
  public class Repository<T> : IRepository<T> where T : class
  {
    protected ISession _session;

    [Inject]
    public Repository(ISession session) => this._session = session;

    public void Insert(T entity)
    {
      HibernateMultipleDatabasesManager.Insert((object) entity, this._session);
    }

    public void Update(T entity)
    {
      HibernateMultipleDatabasesManager.Update((object) entity, this._session);
    }

    public void TransactionalInsert(T entity)
    {
      HibernateMultipleDatabasesManager.TransactionalInsert((object) entity, this._session);
    }

    public void TransactionalDelete(T entity)
    {
      HibernateMultipleDatabasesManager.TransactionalDelete((object) entity, this._session);
    }

    public void TransactionalUpdate(T entity)
    {
      HibernateMultipleDatabasesManager.TransactionalUpdate((object) entity, this._session);
    }

    public void TransactionalDeleteAll(string whereCondition = "")
    {
      string queryString = string.Format("DELETE FROM {0}", (object) typeof (T).FullName);
      if (whereCondition != string.Empty)
        queryString = queryString + " " + whereCondition;
      this._session.CreateQuery(queryString).ExecuteUpdate();
    }

    public T TransactionalInsertOrUpdate(
      T entity,
      Expression<Func<T, bool>> expression,
      Func<T, bool> function)
    {
      T obj = this.FirstOrDefaultCacheSearch(expression, function);
      if ((object) obj == null)
      {
        HibernateMultipleDatabasesManager.TransactionalInsert((object) entity, this._session);
        return entity;
      }
      Mapper.Reset();
      Mapper.CreateMap<T, T>();
      Mapper.Map<T, T>(entity, obj);
      HibernateMultipleDatabasesManager.TransactionalUpdate((object) obj, this._session);
      return entity;
    }

    public T FirstOrDefaultCacheSearch(
      Expression<Func<T, bool>> whereExpression,
      Func<T, bool> whereCondition)
    {
      return this._session.Query<T>().Where<T>(whereExpression).AsEnumerable<T>().Concat<T>(Enumerable.OfType<T>(this._session.GetSessionImplementation().PersistenceContext.EntitiesByKey.Values).Where<T>(whereCondition)).Distinct<T>().FirstOrDefault<T>();
    }

    public IEnumerable<T> TransactionalInsertOrUpdateList(
      Dictionary<Guid, T> entityList,
      Expression<Func<T, bool>> expression,
      Func<T, bool> function)
    {
      List<T> source = new List<T>();
      List<T> insertList = new List<T>();
      List<T> updateList = new List<T>();
      Dictionary<Guid, T> databaseList = this._session.Query<T>().Where<T>(expression).ToDictionary<T, Guid, T>((Func<T, Guid>) (x =>
      {
        // ISSUE: reference to a compiler-generated field
        if (MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, Guid>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Guid), typeof (MSS.Data.Repository.Repository<T>)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Guid> target = MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Guid>> p1 = MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Id", typeof (MSS.Data.Repository.Repository<T>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__0, (object) x);
        return target((CallSite) p1, obj);
      }), (Func<T, T>) (x => x));
      Dictionary<Guid, T> contextList = Enumerable.OfType<T>(this._session.GetSessionImplementation().PersistenceContext.EntitiesByKey.Values).Where<T>(function).ToDictionary<T, Guid, T>((Func<T, Guid>) (x =>
      {
        // ISSUE: reference to a compiler-generated field
        if (MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Guid>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (Guid), typeof (MSS.Data.Repository.Repository<T>)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Guid> target = MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Guid>> p3 = MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Id", typeof (MSS.Data.Repository.Repository<T>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__2.Target((CallSite) MSS.Data.Repository.Repository<T>.\u003C\u003Eo__10.\u003C\u003Ep__2, (object) x);
        return target((CallSite) p3, obj);
      }), (Func<T, T>) (x => x));
      Mapper.Reset();
      Mapper.CreateMap<T, T>();
      entityList.ForEach<KeyValuePair<Guid, T>>((Action<KeyValuePair<Guid, T>>) (x =>
      {
        if (!databaseList.ContainsKey(x.Key) && !contextList.ContainsKey(x.Key))
        {
          insertList.Add(x.Value);
        }
        else
        {
          T destination = databaseList.ContainsKey(x.Key) ? databaseList[x.Key] : contextList[x.Key];
          Mapper.Map<T, T>(x.Value, destination);
          updateList.Add(destination);
        }
      }));
      if (insertList.Any<T>())
      {
        this.TransactionalInsertMany((IEnumerable<T>) insertList);
        source.AddRange((IEnumerable<T>) insertList);
      }
      if (updateList.Any<T>())
      {
        this.TransactionalUpdateMany((IEnumerable<T>) updateList);
        source.AddRange((IEnumerable<T>) updateList);
      }
      return source.Distinct<T>();
    }

    public void InsertMany(IEnumerable<T> entities)
    {
      using (ITransaction transaction = this._session.BeginTransaction())
      {
        this._session.FlushMode = FlushMode.Commit;
        foreach (T entity in entities)
          this.TransactionalInsert(entity);
        transaction.Commit();
      }
    }

    public void TransactionalUpdateMany(IEnumerable<T> entities)
    {
      foreach (T entity in entities)
        this.TransactionalUpdate(entity);
    }

    public void TransactionalInsertMany(IEnumerable<T> entities)
    {
      foreach (T entity in entities)
        this.TransactionalInsert(entity);
    }

    public void Delete(T entity)
    {
      HibernateMultipleDatabasesManager.Delete((object) entity, this._session);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
      return this._session.Query<T>().Any<T>(predicate);
    }

    public bool ExistsInMemoryOrDb(
      Expression<Func<T, bool>> expression,
      Func<T, bool> whereCondition)
    {
      return this._session.Query<T>().Any<T>(expression) | Enumerable.OfType<T>(this._session.GetSessionImplementation().PersistenceContext.EntitiesByKey.Values).Any<T>(whereCondition);
    }

    public IEnumerable<T> SearchForInMemoryOrDb(
      Expression<Func<T, bool>> expression,
      Func<T, bool> whereCondition)
    {
      return this._session.Query<T>().Where<T>(expression).AsEnumerable<T>().Concat<T>(Enumerable.OfType<T>(this._session.GetSessionImplementation().PersistenceContext.EntitiesByKey.Values).Where<T>(whereCondition)).Distinct<T>();
    }

    public List<T> SearchForInMemoryOrDbToList(
      Expression<Func<T, bool>> expression,
      Func<T, bool> whereCondition)
    {
      return this._session.Query<T>().Where<T>(expression).ToList<T>().Concat<T>((IEnumerable<T>) Enumerable.OfType<T>(this._session.GetSessionImplementation().PersistenceContext.EntitiesByKey.Values).Where<T>(whereCondition).ToList<T>()).Distinct<T>().ToList<T>();
    }

    public IList<T> SearchFor(Expression<Func<T, bool>> predicate)
    {
      return (IList<T>) this._session.Query<T>().Where<T>(predicate).ToList<T>();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
    {
      return this._session.Query<T>().Where<T>(predicate);
    }

    public IEnumerable<T> SearchToFuture(Expression<Func<T, bool>> predicate)
    {
      return this._session.Query<T>().Where<T>(predicate).ToFuture<T>();
    }

    public IList<T> GetAll() => (IList<T>) this._session.Query<T>().ToList<T>();

    public void DeleteAll()
    {
      ITransaction transaction = this._session.BeginTransaction();
      this._session.CreateQuery(string.Format("DELETE FROM \"{0}\"", (object) typeof (T).Name)).ExecuteUpdate();
      transaction.Commit();
    }

    public T GetById(object id) => HibernateMultipleDatabasesManager.Load<T>(id, this._session);

    public T FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
      return this._session.Query<T>().FirstOrDefault<T>(predicate);
    }

    public void Refresh(object id)
    {
      T byId = this.GetById(id);
      if ((object) byId == null)
        return;
      this._session.Refresh((object) byId);
    }

    public INhFetchRequest<TOriginating, TRelated> Fetch<TOriginating, TRelated>(
      IQueryable<TOriginating> query,
      Expression<Func<TOriginating, TRelated>> relatedObjectSelector)
    {
      throw new NotImplementedException();
    }

    public IList<T> SearchWithFetch<TRelated>(
      Expression<Func<T, bool>> predicate,
      Expression<Func<T, TRelated>> fetchExpression)
    {
      return (IList<T>) this._session.Query<T>().Fetch<T, TRelated>(fetchExpression).Where<T>(predicate).ToList<T>();
    }

    public Type GetUnderlyingType() => typeof (T);

    public int GetAll_PageCount(int pageSize)
    {
      return (int) Math.Ceiling((Decimal) this._session.QueryOver<T>().RowCount() / (Decimal) pageSize);
    }

    public int GetAll_RecordsCount() => this._session.Query<T>().Count<T>();

    public int SearchFor_PageCount(Expression<Func<T, bool>> predicate, int pageSize)
    {
      return (int) Math.Ceiling((Decimal) this._session.QueryOver<T>().Where(predicate).RowCount() / (Decimal) pageSize);
    }

    public int SearchFor_RecordsCount(Expression<Func<T, bool>> predicate)
    {
      return this._session.Query<T>().Count<T>(predicate);
    }

    public IList<T> GetAll_ByPage(int startIndex, int pageSize)
    {
      return (IList<T>) this._session.Query<T>().Take<T>(pageSize).Skip<T>(startIndex).ToList<T>();
    }

    public IList<T> SearchFor_ByPage(
      Expression<Func<T, bool>> predicate,
      int startIndex,
      int pageSize)
    {
      return (IList<T>) this._session.Query<T>().Where<T>(predicate).Take<T>(pageSize).Skip<T>(startIndex).ToList<T>();
    }

    public IList<T> SearchFor_ByPage(
      Expression<Func<T, bool>> initialCondition,
      IEnumerable<OrderClauseInfo> orderClauses,
      string whereClauses,
      int startIndex,
      int pageSize,
      out int totalCount)
    {
      Expression<Func<T, bool>> lambda = MSS.Data.Utils.DynamicExpression.ParseLambda<T, bool>(whereClauses);
      IQueryable<T> seed = this._session.Query<T>().Where<T>(initialCondition).Where<T>(lambda);
      IQueryable<T> source = orderClauses.Aggregate<OrderClauseInfo, IQueryable<T>>(seed, (Func<IQueryable<T>, OrderClauseInfo, IQueryable<T>>) ((current, clause) => current.OrderBy<T>(clause.PropertyName + " " + (object) clause.Direction)));
      totalCount = source.Count<T>();
      return (IList<T>) source.Take<T>(pageSize).Skip<T>(startIndex).ToList<T>();
    }
  }
}
