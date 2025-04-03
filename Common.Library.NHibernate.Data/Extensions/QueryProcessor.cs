// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.QueryProcessor
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public static class QueryProcessor
  {
    public const int ALL_RECORDS = -1;

    public static IQueryable<T> GetQueryPagedData<T>(string queryName) where T : class
    {
      return QueryProcessor.GetQueryPagedData<T>(queryName, 1, -1, (IEnumerable<OrderClauseInfo>) null, (IEnumerable<IWhereClauseInfo>) null, out int _);
    }

    public static IQueryable<T> GetQueryPagedData<T>(string queryName, int page, int pageSize) where T : class
    {
      return QueryProcessor.GetQueryPagedData<T>(queryName, page, pageSize, (IEnumerable<OrderClauseInfo>) null, (IEnumerable<IWhereClauseInfo>) null, out int _);
    }

    public static IQueryable<T> GetQueryPagedData<T>(
      string queryName,
      int page,
      int pageSize,
      IEnumerable<OrderClauseInfo> sortClauses)
      where T : class
    {
      return QueryProcessor.GetQueryPagedData<T>(queryName, page, pageSize, sortClauses, (IEnumerable<IWhereClauseInfo>) null, out int _);
    }

    public static IQueryable<T> GetQueryPagedData<T>(
      string queryName,
      int page,
      int pageSize,
      IEnumerable<OrderClauseInfo> sortClauses,
      IEnumerable<IWhereClauseInfo> filters)
      where T : class
    {
      return QueryProcessor.GetQueryPagedData<T>(queryName, page, pageSize, sortClauses, filters, out int _);
    }

    public static IQueryable<T> GetQueryPagedData<T>(
      string queryName,
      int page,
      int pageSize,
      IEnumerable<OrderClauseInfo> sortClauses,
      IEnumerable<IWhereClauseInfo> filters,
      out int totalCount)
      where T : class
    {
      return QueryProcessor.GetQueryPagedData<T>(new ExtendedQuery(HibernateManager.DataSessionFactory.GetCurrentSession().GetNamedQuery(queryName)), page, pageSize, sortClauses, filters, out totalCount);
    }

    public static IQueryable<T> GetQueryPagedData<T>(
      ExtendedQuery extendedQuery,
      int page,
      int pageSize,
      IEnumerable<OrderClauseInfo> sortClauses,
      IEnumerable<IWhereClauseInfo> filters,
      out int totalCount)
      where T : class
    {
      IQuery query = extendedQuery.Query;
      if (filters != null)
      {
        foreach (IWhereClauseInfo filter in filters)
          query = query.AddWhere(filter);
      }
      extendedQuery.Query = query;
      totalCount = QueryProcessor.GetCountOfQuery<T>(extendedQuery);
      if (sortClauses != null)
      {
        foreach (OrderClauseInfo sortClause in sortClauses)
          query = query.AddOrder(sortClause);
      }
      extendedQuery.Query = query;
      return pageSize == -1 ? query.SetResultTransformer(Transformers.AliasToBean(typeof (T))).Future<T>().AsQueryable<T>() : (!query.HasColumnsSelect() ? query.SetFirstResult((page - 1) * pageSize).SetMaxResults(pageSize).Future<T>().AsQueryable<T>() : query.SetResultTransformer(Transformers.AliasToBean(typeof (T))).SetFirstResult((page - 1) * pageSize).SetMaxResults(pageSize).Future<T>().AsQueryable<T>());
    }

    private static int GetCountOfQuery<T>(ExtendedQuery extendedQuery) where T : class
    {
      string queryString = extendedQuery.Query.QueryString;
      int num1 = queryString.IndexOf("select", StringComparison.CurrentCultureIgnoreCase);
      int num2 = StringsProcessor.SearchWordPositionOuterLevel("from", queryString);
      if (num1 < 0 || num2 <= 0)
        return 0;
      queryString.Substring(num1 + 7, num2 - 1 - (num1 + 7));
      if (!queryString.Contains(":"))
        return (int) HibernateManager.DataSessionFactory.GetCurrentSession().CreateQuery(queryString.Substring(0, num1 + 7) + "countAll()" + queryString.Substring(num2 - 1, queryString.Length - num2 + 1)).UniqueResult();
      IQuery query = HibernateManager.DataSessionFactory.GetCurrentSession().CreateQuery(queryString.Substring(0, num1 + 7) + "countAll()" + queryString.Substring(num2 - 1, queryString.Length - num2 + 1));
      foreach (string namedParameter in extendedQuery.GetNamedParameters())
        query.SetParameter(namedParameter, extendedQuery.GetParameterValue(namedParameter));
      foreach (string namedParameterList in extendedQuery.GetNamedParameterLists())
        query.SetParameterList(namedParameterList, extendedQuery.GetParameterListValue(namedParameterList));
      return (int) query.UniqueResult();
    }

    public static IList<T> GetCriteriaPagedData<T>(
      ICriteria criteria,
      int page,
      int pageSize,
      IEnumerable<OrderClauseInfo> sortClauses,
      IEnumerable<IWhereClauseInfo> filters,
      ProjectionList projections,
      out int totalCount)
      where T : class
    {
      return QueryProcessor.GetCriteriaPagedData<T>(criteria, page, pageSize, sortClauses, filters, (QueryPropertyAliasesList) null, projections, out totalCount);
    }

    public static IList<T> GetCriteriaPagedData<T>(
      ICriteria criteria,
      int page,
      int pageSize,
      IEnumerable<OrderClauseInfo> sortClauses,
      IEnumerable<IWhereClauseInfo> filters,
      QueryPropertyAliasesList aliasesList,
      ProjectionList projections,
      out int totalCount)
      where T : class
    {
      HibernateManager.DataSessionFactory.GetCurrentSession();
      ExtendedCriteria extendedCriteria = new ExtendedCriteria(criteria).AddAliases(aliasesList);
      if (projections != null)
        extendedCriteria.SetProjections(projections);
      if (filters != null)
      {
        foreach (IWhereClauseInfo filter in filters)
          extendedCriteria.AddWhere(filter);
      }
      totalCount = (int) (criteria.Clone() as ICriteria).SetProjection(Projections.RowCount()).UniqueResult();
      if (sortClauses != null)
      {
        foreach (OrderClauseInfo sortClause in sortClauses)
          extendedCriteria.AddOrder(sortClause);
      }
      criteria = extendedCriteria.ProcessCriteria();
      return pageSize.Equals(-1) ? (aliasesList != null ? criteria.SetResultTransformer(Transformers.AliasToBean(typeof (T))).List<T>() : criteria.List<T>()) : (aliasesList != null ? criteria.SetFirstResult((page - 1) * pageSize).SetMaxResults(pageSize).SetResultTransformer(Transformers.AliasToBean(typeof (T))).List<T>() : criteria.SetFirstResult((page - 1) * pageSize).SetMaxResults(pageSize).List<T>());
    }
  }
}
