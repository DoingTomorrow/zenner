// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.QueryPlanCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine.Query.Sql;
using NHibernate.Linq;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class QueryPlanCache
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (QueryPlanCache));
    private readonly ISessionFactoryImplementor factory;
    private readonly SimpleMRUCache sqlParamMetadataCache = new SimpleMRUCache();
    private readonly SoftLimitMRUCache planCache = new SoftLimitMRUCache(128);

    public QueryPlanCache(ISessionFactoryImplementor factory) => this.factory = factory;

    public ParameterMetadata GetSQLParameterMetadata(string query)
    {
      ParameterMetadata parameterMetadata = (ParameterMetadata) this.sqlParamMetadataCache[(object) query];
      if (parameterMetadata == null)
      {
        parameterMetadata = this.BuildNativeSQLParameterMetadata(query);
        this.sqlParamMetadataCache.Put((object) query, (object) parameterMetadata);
      }
      return parameterMetadata;
    }

    public IQueryPlan GetHQLQueryPlan(
      string queryString,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters)
    {
      QueryPlanCache.HQLQueryPlanKey key = new QueryPlanCache.HQLQueryPlanKey(queryString, shallow, enabledFilters);
      IQueryPlan hqlQueryPlan = (IQueryPlan) this.planCache[(object) key];
      if (hqlQueryPlan == null)
      {
        if (QueryPlanCache.log.IsDebugEnabled)
          QueryPlanCache.log.Debug((object) ("unable to locate HQL query plan in cache; generating (" + queryString + ")"));
        hqlQueryPlan = (IQueryPlan) new HQLStringQueryPlan(queryString, shallow, enabledFilters, this.factory);
        this.planCache.Put((object) key, (object) hqlQueryPlan);
      }
      else if (QueryPlanCache.log.IsDebugEnabled)
        QueryPlanCache.log.Debug((object) ("located HQL query plan in cache (" + queryString + ")"));
      return hqlQueryPlan;
    }

    public IQueryExpressionPlan GetHQLQueryPlan(
      IQueryExpression queryExpression,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters)
    {
      string key1 = queryExpression.Key;
      QueryPlanCache.HQLQueryPlanKey key2 = new QueryPlanCache.HQLQueryPlanKey(queryExpression, shallow, enabledFilters);
      HQLExpressionQueryPlan hqlQueryPlan = (HQLExpressionQueryPlan) this.planCache[(object) key2];
      if (hqlQueryPlan == null)
      {
        if (QueryPlanCache.log.IsDebugEnabled)
          QueryPlanCache.log.Debug((object) ("unable to locate HQL query plan in cache; generating (" + key1 + ")"));
        hqlQueryPlan = new HQLExpressionQueryPlan(key1, queryExpression, shallow, enabledFilters, this.factory);
        this.planCache.Put((object) key2, (object) hqlQueryPlan);
      }
      else
      {
        if (QueryPlanCache.log.IsDebugEnabled)
          QueryPlanCache.log.Debug((object) ("located HQL query plan in cache (" + key1 + ")"));
        NhLinqExpression queryExpression1 = hqlQueryPlan.QueryExpression as NhLinqExpression;
        NhLinqExpression newExpression = queryExpression as NhLinqExpression;
        if (queryExpression1 != null && newExpression != null)
        {
          newExpression.CopyExpressionTranslation(queryExpression1);
          hqlQueryPlan = hqlQueryPlan.Copy((IQueryExpression) newExpression);
        }
      }
      return (IQueryExpressionPlan) hqlQueryPlan;
    }

    public FilterQueryPlan GetFilterQueryPlan(
      string filterString,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters)
    {
      QueryPlanCache.FilterQueryPlanKey key = new QueryPlanCache.FilterQueryPlanKey(filterString, collectionRole, shallow, enabledFilters);
      FilterQueryPlan filterQueryPlan = (FilterQueryPlan) this.planCache[(object) key];
      if (filterQueryPlan == null)
      {
        if (QueryPlanCache.log.IsDebugEnabled)
          QueryPlanCache.log.Debug((object) ("unable to locate collection-filter query plan in cache; generating (" + collectionRole + " : " + filterString + ")"));
        filterQueryPlan = new FilterQueryPlan(filterString, collectionRole, shallow, enabledFilters, this.factory);
        this.planCache.Put((object) key, (object) filterQueryPlan);
      }
      else if (QueryPlanCache.log.IsDebugEnabled)
        QueryPlanCache.log.Debug((object) ("located collection-filter query plan in cache (" + collectionRole + " : " + filterString + ")"));
      return filterQueryPlan;
    }

    public NativeSQLQueryPlan GetNativeSQLQueryPlan(NativeSQLQuerySpecification spec)
    {
      NativeSQLQueryPlan nativeSqlQueryPlan = (NativeSQLQueryPlan) this.planCache[(object) spec];
      if (nativeSqlQueryPlan == null)
      {
        if (QueryPlanCache.log.IsDebugEnabled)
          QueryPlanCache.log.Debug((object) ("unable to locate native-sql query plan in cache; generating (" + spec.QueryString + ")"));
        nativeSqlQueryPlan = new NativeSQLQueryPlan(spec, this.factory);
        this.planCache.Put((object) spec, (object) nativeSqlQueryPlan);
      }
      else if (QueryPlanCache.log.IsDebugEnabled)
        QueryPlanCache.log.Debug((object) ("located native-sql query plan in cache (" + spec.QueryString + ")"));
      return nativeSqlQueryPlan;
    }

    private ParameterMetadata BuildNativeSQLParameterMetadata(string sqlString)
    {
      ParamLocationRecognizer locations = ParamLocationRecognizer.ParseLocations(sqlString);
      OrdinalParameterDescriptor[] ordinalDescriptors = new OrdinalParameterDescriptor[locations.OrdinalParameterLocationList.Count];
      for (int index = 0; index < locations.OrdinalParameterLocationList.Count; ++index)
      {
        int parameterLocation = locations.OrdinalParameterLocationList[index];
        ordinalDescriptors[index] = new OrdinalParameterDescriptor(index, (IType) null);
      }
      IDictionary<string, NamedParameterDescriptor> namedDescriptorMap = (IDictionary<string, NamedParameterDescriptor>) new Dictionary<string, NamedParameterDescriptor>();
      foreach (KeyValuePair<string, ParamLocationRecognizer.NamedParameterDescription> parameterDescription1 in (IEnumerable<KeyValuePair<string, ParamLocationRecognizer.NamedParameterDescription>>) locations.NamedParameterDescriptionMap)
      {
        string key = parameterDescription1.Key;
        ParamLocationRecognizer.NamedParameterDescription parameterDescription2 = parameterDescription1.Value;
        namedDescriptorMap[key] = new NamedParameterDescriptor(key, (IType) null, parameterDescription2.JpaStyle);
      }
      return new ParameterMetadata((IEnumerable<OrdinalParameterDescriptor>) ordinalDescriptors, namedDescriptorMap);
    }

    [Serializable]
    private class HQLQueryPlanKey : IEquatable<QueryPlanCache.HQLQueryPlanKey>
    {
      private readonly string query;
      private readonly bool shallow;
      private readonly ISet<string> filterNames;
      private readonly int hashCode;
      private readonly System.Type queryTypeDiscriminator;

      public HQLQueryPlanKey(
        string query,
        bool shallow,
        IDictionary<string, IFilter> enabledFilters)
        : this(typeof (object), query, shallow, enabledFilters)
      {
      }

      public HQLQueryPlanKey(
        IQueryExpression queryExpression,
        bool shallow,
        IDictionary<string, IFilter> enabledFilters)
        : this(queryExpression.GetType(), queryExpression.Key, shallow, enabledFilters)
      {
      }

      protected HQLQueryPlanKey(
        System.Type queryTypeDiscriminator,
        string query,
        bool shallow,
        IDictionary<string, IFilter> enabledFilters)
      {
        this.queryTypeDiscriminator = queryTypeDiscriminator;
        this.query = query;
        this.shallow = shallow;
        this.filterNames = enabledFilters == null || enabledFilters.Count == 0 ? (ISet<string>) new HashedSet<string>() : (ISet<string>) new HashedSet<string>(enabledFilters.Keys);
        this.hashCode = 29 * (29 * (29 * query.GetHashCode() + (shallow ? 1 : 0)) + CollectionHelper.GetHashCode<string>((IEnumerable<string>) this.filterNames)) + queryTypeDiscriminator.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        return this == obj || this.Equals(obj as QueryPlanCache.HQLQueryPlanKey);
      }

      public bool Equals(QueryPlanCache.HQLQueryPlanKey that)
      {
        return that != null && this.shallow == that.shallow && CollectionHelper.SetEquals<string>(this.filterNames, that.filterNames) && this.query.Equals(that.query) && this.queryTypeDiscriminator == that.queryTypeDiscriminator;
      }

      public override int GetHashCode() => this.hashCode;
    }

    [Serializable]
    private class FilterQueryPlanKey
    {
      private readonly string query;
      private readonly string collectionRole;
      private readonly bool shallow;
      private readonly ISet<string> filterNames;
      private readonly int hashCode;

      public FilterQueryPlanKey(
        string query,
        string collectionRole,
        bool shallow,
        IDictionary<string, IFilter> enabledFilters)
      {
        this.query = query;
        this.collectionRole = collectionRole;
        this.shallow = shallow;
        this.filterNames = enabledFilters == null || enabledFilters.Count == 0 ? (ISet<string>) new HashedSet<string>() : (ISet<string>) new HashedSet<string>(enabledFilters.Keys);
        this.hashCode = 29 * (29 * (29 * query.GetHashCode() + collectionRole.GetHashCode()) + (shallow ? 1 : 0)) + CollectionHelper.GetHashCode<string>((IEnumerable<string>) this.filterNames);
      }

      public override bool Equals(object obj)
      {
        return this == obj || this.Equals(obj as QueryPlanCache.FilterQueryPlanKey);
      }

      public bool Equals(QueryPlanCache.FilterQueryPlanKey that)
      {
        return that != null && this.shallow == that.shallow && CollectionHelper.SetEquals<string>(this.filterNames, that.filterNames) && this.query.Equals(that.query) && this.collectionRole.Equals(that.collectionRole);
      }

      public override int GetHashCode() => this.hashCode;
    }
  }
}
