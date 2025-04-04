// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.Sql.SQLQueryReturnProcessor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query.Sql;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Custom.Sql
{
  public class SQLQueryReturnProcessor
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SQLQueryReturnProcessor));
    private readonly INativeSQLQueryReturn[] queryReturns;
    private readonly Dictionary<string, INativeSQLQueryReturn> alias2Return = new Dictionary<string, INativeSQLQueryReturn>();
    private readonly Dictionary<string, string> alias2OwnerAlias = new Dictionary<string, string>();
    private readonly Dictionary<string, ISqlLoadable> alias2Persister = new Dictionary<string, ISqlLoadable>();
    private readonly Dictionary<string, string> alias2Suffix = new Dictionary<string, string>();
    private readonly Dictionary<string, ISqlLoadableCollection> alias2CollectionPersister = new Dictionary<string, ISqlLoadableCollection>();
    private readonly Dictionary<string, string> alias2CollectionSuffix = new Dictionary<string, string>();
    private readonly Dictionary<string, IDictionary<string, string[]>> entityPropertyResultMaps = new Dictionary<string, IDictionary<string, string[]>>();
    private readonly Dictionary<string, IDictionary<string, string[]>> collectionPropertyResultMaps = new Dictionary<string, IDictionary<string, string[]>>();
    private readonly ISessionFactoryImplementor factory;
    private int entitySuffixSeed;
    private int collectionSuffixSeed;

    private ISessionFactoryImplementor Factory => this.factory;

    public SQLQueryReturnProcessor(
      INativeSQLQueryReturn[] queryReturns,
      ISessionFactoryImplementor factory)
    {
      this.queryReturns = queryReturns;
      this.factory = factory;
    }

    private IDictionary<string, string[]> InternalGetPropertyResultsMap(string alias)
    {
      return this.alias2Return[alias] is NativeSQLQueryNonScalarReturn queryNonScalarReturn ? queryNonScalarReturn.PropertyResultsMap : (IDictionary<string, string[]>) null;
    }

    private bool HasPropertyResultMap(string alias)
    {
      IDictionary<string, string[]> propertyResultsMap = this.InternalGetPropertyResultsMap(alias);
      return propertyResultsMap != null && propertyResultsMap.Count != 0;
    }

    public SQLQueryReturnProcessor.ResultAliasContext Process()
    {
      for (int index = 0; index < this.queryReturns.Length; ++index)
      {
        if (this.queryReturns[index] is NativeSQLQueryNonScalarReturn)
        {
          NativeSQLQueryNonScalarReturn queryReturn1 = (NativeSQLQueryNonScalarReturn) this.queryReturns[index];
          this.alias2Return[queryReturn1.Alias] = (INativeSQLQueryReturn) queryReturn1;
          if (queryReturn1 is NativeSQLQueryJoinReturn)
          {
            NativeSQLQueryJoinReturn queryReturn2 = (NativeSQLQueryJoinReturn) this.queryReturns[index];
            this.alias2OwnerAlias[queryReturn2.Alias] = queryReturn2.OwnerAlias;
          }
        }
      }
      for (int index = 0; index < this.queryReturns.Length; ++index)
        this.ProcessReturn(this.queryReturns[index]);
      return new SQLQueryReturnProcessor.ResultAliasContext(this);
    }

    private ISqlLoadable GetSQLLoadable(string entityName)
    {
      if (!(this.factory.GetEntityPersister(entityName) is ISqlLoadable entityPersister))
        throw new MappingException("class persister is not ISqlLoadable: " + entityName);
      return entityPersister;
    }

    private string GenerateEntitySuffix()
    {
      return BasicLoader.GenerateSuffixes(this.entitySuffixSeed++, 1)[0];
    }

    private string GenerateCollectionSuffix() => this.collectionSuffixSeed++.ToString() + "__";

    private void ProcessReturn(INativeSQLQueryReturn rtn)
    {
      switch (rtn)
      {
        case NativeSQLQueryScalarReturn _:
          this.ProcessScalarReturn((NativeSQLQueryScalarReturn) rtn);
          break;
        case NativeSQLQueryRootReturn _:
          this.ProcessRootReturn((NativeSQLQueryRootReturn) rtn);
          break;
        case NativeSQLQueryCollectionReturn _:
          this.ProcessCollectionReturn((NativeSQLQueryCollectionReturn) rtn);
          break;
        default:
          this.ProcessJoinReturn((NativeSQLQueryJoinReturn) rtn);
          break;
      }
    }

    private void ProcessScalarReturn(NativeSQLQueryScalarReturn typeReturn)
    {
    }

    private void ProcessRootReturn(NativeSQLQueryRootReturn rootReturn)
    {
      if (this.alias2Persister.ContainsKey(rootReturn.Alias))
        return;
      ISqlLoadable sqlLoadable = this.GetSQLLoadable(rootReturn.ReturnEntityName);
      this.AddPersister(rootReturn.Alias, rootReturn.PropertyResultsMap, sqlLoadable);
    }

    private void AddPersister(
      string alias,
      IDictionary<string, string[]> propertyResult,
      ISqlLoadable persister)
    {
      this.alias2Persister[alias] = persister;
      string entitySuffix = this.GenerateEntitySuffix();
      SQLQueryReturnProcessor.log.Debug((object) ("mapping alias [" + alias + "] to entity-suffix [" + entitySuffix + "]"));
      this.alias2Suffix[alias] = entitySuffix;
      this.entityPropertyResultMaps[alias] = propertyResult;
    }

    private void AddCollection(
      string role,
      string alias,
      IDictionary<string, string[]> propertyResults)
    {
      ISqlLoadableCollection collectionPersister = (ISqlLoadableCollection) this.Factory.GetCollectionPersister(role);
      this.alias2CollectionPersister[alias] = collectionPersister;
      string collectionSuffix = this.GenerateCollectionSuffix();
      SQLQueryReturnProcessor.log.Debug((object) ("mapping alias [" + alias + "] to collection-suffix [" + collectionSuffix + "]"));
      this.alias2CollectionSuffix[alias] = collectionSuffix;
      this.collectionPropertyResultMaps[alias] = propertyResults;
      if (!collectionPersister.IsOneToMany)
        return;
      ISqlLoadable elementPersister = (ISqlLoadable) collectionPersister.ElementPersister;
      this.AddPersister(alias, this.Filter(propertyResults), elementPersister);
    }

    private IDictionary<string, string[]> Filter(IDictionary<string, string[]> propertyResults)
    {
      Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>(propertyResults.Count);
      string str = "element.";
      foreach (KeyValuePair<string, string[]> propertyResult in (IEnumerable<KeyValuePair<string, string[]>>) propertyResults)
      {
        string key = propertyResult.Key;
        if (key.StartsWith(str))
          dictionary[key.Substring(str.Length)] = propertyResult.Value;
      }
      return (IDictionary<string, string[]>) dictionary;
    }

    private void ProcessCollectionReturn(NativeSQLQueryCollectionReturn collectionReturn)
    {
      this.AddCollection(collectionReturn.OwnerEntityName + (object) '.' + collectionReturn.OwnerProperty, collectionReturn.Alias, collectionReturn.PropertyResultsMap);
    }

    private void ProcessJoinReturn(NativeSQLQueryJoinReturn fetchReturn)
    {
      string alias = fetchReturn.Alias;
      if (this.alias2Persister.ContainsKey(alias) || this.alias2CollectionPersister.ContainsKey(alias))
        return;
      string ownerAlias = fetchReturn.OwnerAlias;
      if (!this.alias2Return.ContainsKey(ownerAlias))
        throw new HibernateException("Owner alias [" + ownerAlias + "] is unknown for alias [" + alias + "]");
      if (!this.alias2Persister.ContainsKey(ownerAlias))
        this.ProcessReturn(this.alias2Return[ownerAlias]);
      ISqlLoadable sqlLoadable1 = this.alias2Persister[ownerAlias];
      IType propertyType = sqlLoadable1.GetPropertyType(fetchReturn.OwnerProperty);
      if (propertyType.IsCollectionType)
      {
        this.AddCollection(sqlLoadable1.EntityName + (object) '.' + fetchReturn.OwnerProperty, alias, fetchReturn.PropertyResultsMap);
      }
      else
      {
        if (!propertyType.IsEntityType)
          return;
        ISqlLoadable sqlLoadable2 = this.GetSQLLoadable(((EntityType) propertyType).GetAssociatedEntityName());
        this.AddPersister(alias, fetchReturn.PropertyResultsMap, sqlLoadable2);
      }
    }

    public IList GenerateCustomReturns(bool queryHadAliases)
    {
      IList customReturns = (IList) new ArrayList();
      IDictionary<string, object> dictionary = (IDictionary<string, object>) new Dictionary<string, object>();
      for (int index = 0; index < this.queryReturns.Length; ++index)
      {
        if (this.queryReturns[index] is NativeSQLQueryScalarReturn)
        {
          NativeSQLQueryScalarReturn queryReturn = (NativeSQLQueryScalarReturn) this.queryReturns[index];
          customReturns.Add((object) new ScalarReturn(queryReturn.Type, queryReturn.ColumnAlias));
        }
        else if (this.queryReturns[index] is NativeSQLQueryRootReturn)
        {
          NativeSQLQueryRootReturn queryReturn = (NativeSQLQueryRootReturn) this.queryReturns[index];
          string alias = queryReturn.Alias;
          IEntityAliases entityAliases = queryHadAliases || this.HasPropertyResultMap(alias) ? (IEntityAliases) new DefaultEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]) : (IEntityAliases) new ColumnEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]);
          RootReturn rootReturn = new RootReturn(alias, queryReturn.ReturnEntityName, entityAliases, queryReturn.LockMode);
          customReturns.Add((object) rootReturn);
          dictionary[queryReturn.Alias] = (object) rootReturn;
        }
        else if (this.queryReturns[index] is NativeSQLQueryCollectionReturn)
        {
          NativeSQLQueryCollectionReturn queryReturn = (NativeSQLQueryCollectionReturn) this.queryReturns[index];
          string alias = queryReturn.Alias;
          bool isEntityType = this.alias2CollectionPersister[alias].ElementType.IsEntityType;
          IEntityAliases elementEntityAliases = (IEntityAliases) null;
          ICollectionAliases collectionAliases;
          if (queryHadAliases || this.HasPropertyResultMap(alias))
          {
            collectionAliases = (ICollectionAliases) new GeneratedCollectionAliases(this.collectionPropertyResultMaps[alias], (ICollectionPersister) this.alias2CollectionPersister[alias], this.alias2CollectionSuffix[alias]);
            if (isEntityType)
              elementEntityAliases = (IEntityAliases) new DefaultEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]);
          }
          else
          {
            collectionAliases = (ICollectionAliases) new ColumnCollectionAliases(this.collectionPropertyResultMaps[alias], this.alias2CollectionPersister[alias]);
            if (isEntityType)
              elementEntityAliases = (IEntityAliases) new ColumnEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]);
          }
          CollectionReturn collectionReturn = new CollectionReturn(alias, queryReturn.OwnerEntityName, queryReturn.OwnerProperty, collectionAliases, elementEntityAliases, queryReturn.LockMode);
          customReturns.Add((object) collectionReturn);
          dictionary[queryReturn.Alias] = (object) collectionReturn;
        }
        else if (this.queryReturns[index] is NativeSQLQueryJoinReturn)
        {
          NativeSQLQueryJoinReturn queryReturn = (NativeSQLQueryJoinReturn) this.queryReturns[index];
          string alias = queryReturn.Alias;
          NonScalarReturn owner = (NonScalarReturn) dictionary[queryReturn.OwnerAlias];
          FetchReturn fetchReturn;
          if (this.alias2CollectionPersister.ContainsKey(alias))
          {
            ISqlLoadableCollection persister = this.alias2CollectionPersister[alias];
            bool isEntityType = persister.ElementType.IsEntityType;
            IEntityAliases elementEntityAliases = (IEntityAliases) null;
            ICollectionAliases collectionAliases;
            if (queryHadAliases || this.HasPropertyResultMap(alias))
            {
              collectionAliases = (ICollectionAliases) new GeneratedCollectionAliases(this.collectionPropertyResultMaps[alias], (ICollectionPersister) persister, this.alias2CollectionSuffix[alias]);
              if (isEntityType)
                elementEntityAliases = (IEntityAliases) new DefaultEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]);
            }
            else
            {
              collectionAliases = (ICollectionAliases) new ColumnCollectionAliases(this.collectionPropertyResultMaps[alias], persister);
              if (isEntityType)
                elementEntityAliases = (IEntityAliases) new ColumnEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]);
            }
            fetchReturn = (FetchReturn) new CollectionFetchReturn(alias, owner, queryReturn.OwnerProperty, collectionAliases, elementEntityAliases, queryReturn.LockMode);
          }
          else
          {
            IEntityAliases entityAliases = queryHadAliases || this.HasPropertyResultMap(alias) ? (IEntityAliases) new DefaultEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]) : (IEntityAliases) new ColumnEntityAliases(this.entityPropertyResultMaps[alias], (ILoadable) this.alias2Persister[alias], this.alias2Suffix[alias]);
            fetchReturn = (FetchReturn) new EntityFetchReturn(alias, entityAliases, owner, queryReturn.OwnerProperty, queryReturn.LockMode);
          }
          customReturns.Add((object) fetchReturn);
          dictionary[alias] = (object) fetchReturn;
        }
      }
      return customReturns;
    }

    public class ResultAliasContext
    {
      private readonly SQLQueryReturnProcessor parent;

      public ResultAliasContext(SQLQueryReturnProcessor parent) => this.parent = parent;

      public ISqlLoadable GetEntityPersister(string alias)
      {
        ISqlLoadable entityPersister;
        this.parent.alias2Persister.TryGetValue(alias, out entityPersister);
        return entityPersister;
      }

      public ISqlLoadableCollection GetCollectionPersister(string alias)
      {
        ISqlLoadableCollection collectionPersister;
        this.parent.alias2CollectionPersister.TryGetValue(alias, out collectionPersister);
        return collectionPersister;
      }

      public string GetEntitySuffix(string alias)
      {
        string entitySuffix;
        this.parent.alias2Suffix.TryGetValue(alias, out entitySuffix);
        return entitySuffix;
      }

      public string GetCollectionSuffix(string alias)
      {
        string collectionSuffix;
        this.parent.alias2CollectionSuffix.TryGetValue(alias, out collectionSuffix);
        return collectionSuffix;
      }

      public string GetOwnerAlias(string alias)
      {
        string ownerAlias;
        this.parent.alias2OwnerAlias.TryGetValue(alias, out ownerAlias);
        return ownerAlias;
      }

      public IDictionary<string, string[]> GetPropertyResultsMap(string alias)
      {
        return this.parent.InternalGetPropertyResultsMap(alias);
      }
    }
  }
}
