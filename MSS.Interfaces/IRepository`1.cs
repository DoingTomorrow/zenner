// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.IRepository`1
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using Common.Library.NHibernate.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Interfaces
{
  public interface IRepository<T>
  {
    void Insert(T entity);

    void Delete(T entity);

    void Update(T entity);

    void TransactionalInsert(T entity);

    void TransactionalDelete(T entity);

    void TransactionalUpdate(T entity);

    void InsertMany(IEnumerable<T> entity);

    bool Exists(Expression<Func<T, bool>> predicate);

    [Obsolete("SearchFor(condition) is deprecated, please use Where(condition).ToList() instead. Returns IQueriable instead of IList, allowing selection before bringing the whole object in memory.")]
    IList<T> SearchFor(Expression<Func<T, bool>> predicate);

    IQueryable<T> Where(Expression<Func<T, bool>> predicate);

    bool ExistsInMemoryOrDb(Expression<Func<T, bool>> expression, Func<T, bool> whereCondition);

    IList<T> GetAll();

    int GetAll_PageCount(int pageSize);

    int GetAll_RecordsCount();

    IList<T> GetAll_ByPage(int startIndex, int pageSize);

    int SearchFor_PageCount(Expression<Func<T, bool>> predicate, int pageSize);

    int SearchFor_RecordsCount(Expression<Func<T, bool>> predicate);

    IList<T> SearchFor_ByPage(Expression<Func<T, bool>> predicate, int startIndex, int pageSize);

    void DeleteAll();

    void TransactionalDeleteAll(string whereCondition = "");

    T GetById(object id);

    T FirstOrDefault(Expression<Func<T, bool>> predicate);

    void Refresh(object id);

    Type GetUnderlyingType();

    IList<T> SearchFor_ByPage(
      Expression<Func<T, bool>> predicate,
      IEnumerable<OrderClauseInfo> orderClauses,
      string whereClauses,
      int startIndex,
      int pageSize,
      out int totalCount);

    IEnumerable<T> SearchForInMemoryOrDb(
      Expression<Func<T, bool>> expression,
      Func<T, bool> whereCondition);

    IList<T> SearchWithFetch<TRelated>(
      Expression<Func<T, bool>> predicate,
      Expression<Func<T, TRelated>> fetchExpression);

    T TransactionalInsertOrUpdate(
      T entity,
      Expression<Func<T, bool>> expression,
      Func<T, bool> function);

    T FirstOrDefaultCacheSearch(
      Expression<Func<T, bool>> whereExpression,
      Func<T, bool> whereCondition);

    List<T> SearchForInMemoryOrDbToList(
      Expression<Func<T, bool>> expression,
      Func<T, bool> whereCondition);

    IEnumerable<T> TransactionalInsertOrUpdateList(
      Dictionary<Guid, T> entityList,
      Expression<Func<T, bool>> expression,
      Func<T, bool> function);

    void TransactionalUpdateMany(IEnumerable<T> entities);

    void TransactionalInsertMany(IEnumerable<T> entities);
  }
}
