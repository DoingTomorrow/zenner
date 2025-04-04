// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Loader.QueryLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Impl;
using NHibernate.Loader;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Loader
{
  [CLSCompliant(false)]
  public class QueryLoader : BasicLoader
  {
    private readonly QueryTranslatorImpl _queryTranslator;
    private bool _hasScalars;
    private string[][] _scalarColumnNames;
    private IType[] _queryReturnTypes;
    private IResultTransformer _selectNewTransformer;
    private string[] _queryReturnAliases;
    private IQueryableCollection[] _collectionPersisters;
    private int[] _collectionOwners;
    private string[] _collectionSuffixes;
    private IQueryable[] _entityPersisters;
    private bool[] _entityEagerPropertyFetches;
    private string[] _entityAliases;
    private string[] _sqlAliases;
    private string[] _sqlAliasSuffixes;
    private bool[] _includeInSelect;
    private int[] _owners;
    private EntityType[] _ownerAssociationTypes;
    private readonly NullableDictionary<string, string> _sqlAliasByEntityAlias = new NullableDictionary<string, string>();
    private int _selectLength;
    private LockMode[] _defaultLockModes;

    public QueryLoader(
      QueryTranslatorImpl queryTranslator,
      ISessionFactoryImplementor factory,
      SelectClause selectClause)
      : base(factory)
    {
      this._queryTranslator = queryTranslator;
      this.Initialize(selectClause);
      this.PostInstantiate();
    }

    public override bool IsSubselectLoadingEnabled => this.HasSubselectLoadableCollections();

    protected override SqlString ApplyLocks(
      SqlString sql,
      IDictionary<string, LockMode> lockModes,
      NHibernate.Dialect.Dialect dialect)
    {
      if (lockModes == null || lockModes.Count == 0)
        return sql;
      Dictionary<string, LockMode> aliasedLockModes = new Dictionary<string, LockMode>();
      Dictionary<string, string[]> keyColumnNames = dialect.ForUpdateOfColumns ? new Dictionary<string, string[]>() : (Dictionary<string, string[]>) null;
      foreach (KeyValuePair<string, LockMode> lockMode in (IEnumerable<KeyValuePair<string, LockMode>>) lockModes)
      {
        string key = lockMode.Key;
        string aliasByEntityAlia = this._sqlAliasByEntityAlias[key];
        if (aliasByEntityAlia == null)
          throw new InvalidOperationException("could not locate alias to apply lock mode : " + key);
        ILockable queryable = (ILockable) ((AbstractRestrictableStatement) this._queryTranslator.SqlAST).FromClause.GetFromElement(key).Queryable;
        string rootTableAlias = queryable.GetRootTableAlias(aliasByEntityAlia);
        aliasedLockModes.Add(rootTableAlias, lockMode.Value);
        keyColumnNames?.Add(rootTableAlias, queryable.RootTableIdentifierColumnNames);
      }
      return dialect.ApplyLocksToSql(sql, (IDictionary<string, LockMode>) aliasedLockModes, (IDictionary<string, string[]>) keyColumnNames);
    }

    protected override string[] Aliases => this._sqlAliases;

    protected override int[] CollectionOwners => this._collectionOwners;

    protected override bool[] EntityEagerPropertyFetches => this._entityEagerPropertyFetches;

    protected override EntityType[] OwnerAssociationTypes => this._ownerAssociationTypes;

    protected override int[] Owners => this._owners;

    public override string QueryIdentifier => this._queryTranslator.QueryIdentifier;

    protected override bool UpgradeLocks() => true;

    public override LockMode[] GetLockModes(IDictionary<string, LockMode> lockModes)
    {
      if (lockModes == null || lockModes.Count == 0)
        return this._defaultLockModes;
      LockMode[] lockModes1 = new LockMode[this._entityAliases.Length];
      for (int index = 0; index < this._entityAliases.Length; ++index)
      {
        LockMode none;
        if (!lockModes.TryGetValue(this._entityAliases[index], out none))
          none = LockMode.None;
        lockModes1[index] = none;
      }
      return lockModes1;
    }

    public override SqlString SqlString => this._queryTranslator.SqlString;

    public override ILoadable[] EntityPersisters => (ILoadable[]) this._entityPersisters;

    protected override string[] Suffixes => this._sqlAliasSuffixes;

    protected override string[] CollectionSuffixes => this._collectionSuffixes;

    protected override ICollectionPersister[] CollectionPersisters
    {
      get => (ICollectionPersister[]) this._collectionPersisters;
    }

    private void Initialize(SelectClause selectClause)
    {
      IList<FromElement> fromElementsForLoad = selectClause.FromElementsForLoad;
      this._hasScalars = selectClause.IsScalarSelect;
      this._scalarColumnNames = selectClause.ColumnNames;
      this._queryReturnTypes = selectClause.QueryReturnTypes;
      this._selectNewTransformer = HolderInstantiator.CreateSelectNewTransformer(selectClause.Constructor, selectClause.IsMap, selectClause.IsList);
      this._queryReturnAliases = selectClause.QueryReturnAliases;
      IList<FromElement> collectionFromElements = selectClause.CollectionFromElements;
      if (collectionFromElements != null && collectionFromElements.Count != 0)
      {
        int count = collectionFromElements.Count;
        this._collectionPersisters = new IQueryableCollection[count];
        this._collectionOwners = new int[count];
        this._collectionSuffixes = new string[count];
        for (int index = 0; index < count; ++index)
        {
          FromElement fromElement = collectionFromElements[index];
          this._collectionPersisters[index] = fromElement.QueryableCollection;
          this._collectionOwners[index] = fromElementsForLoad.IndexOf(fromElement.Origin);
          this._collectionSuffixes[index] = fromElement.CollectionSuffix;
        }
      }
      int count1 = fromElementsForLoad.Count;
      this._entityPersisters = new IQueryable[count1];
      this._entityEagerPropertyFetches = new bool[count1];
      this._entityAliases = new string[count1];
      this._sqlAliases = new string[count1];
      this._sqlAliasSuffixes = new string[count1];
      this._includeInSelect = new bool[count1];
      this._owners = new int[count1];
      this._ownerAssociationTypes = new EntityType[count1];
      for (int index = 0; index < count1; ++index)
      {
        FromElement fromElement = fromElementsForLoad[index];
        this._entityPersisters[index] = (IQueryable) fromElement.EntityPersister;
        if (this._entityPersisters[index] == null)
          throw new InvalidOperationException("No entity persister for " + (object) fromElement);
        this._entityEagerPropertyFetches[index] = fromElement.IsAllPropertyFetch;
        this._sqlAliases[index] = fromElement.TableAlias;
        this._entityAliases[index] = fromElement.ClassAlias;
        this._sqlAliasByEntityAlias.Add(this._entityAliases[index], this._sqlAliases[index]);
        this._sqlAliasSuffixes[index] = count1 == 1 ? "" : index.ToString() + "_";
        this._includeInSelect[index] = !fromElement.IsFetch;
        if (this._includeInSelect[index])
          ++this._selectLength;
        this._owners[index] = -1;
        if (fromElement.IsFetch && !fromElement.IsCollectionJoin && fromElement.QueryableCollection == null && fromElement.DataType.IsEntityType)
        {
          EntityType dataType = (EntityType) fromElement.DataType;
          if (dataType.IsOneToOne)
            this._owners[index] = fromElementsForLoad.IndexOf(fromElement.Origin);
          this._ownerAssociationTypes[index] = dataType;
        }
      }
      this._defaultLockModes = ArrayHelper.FillArray(LockMode.None, count1);
    }

    public IList List(ISessionImplementor session, QueryParameters queryParameters)
    {
      this.CheckQuery(queryParameters);
      return this.List(session, queryParameters, this._queryTranslator.QuerySpaces, this._queryReturnTypes);
    }

    public override IList GetResultList(IList results, IResultTransformer resultTransformer)
    {
      HolderInstantiator holderInstantiator = HolderInstantiator.GetHolderInstantiator(this._selectNewTransformer, resultTransformer, this._queryReturnAliases);
      if (!holderInstantiator.IsRequired)
        return results;
      for (int index = 0; index < results.Count; ++index)
      {
        object[] result = (object[]) results[index];
        object obj = holderInstantiator.Instantiate(result);
        results[index] = obj;
      }
      return !this.HasSelectNew && resultTransformer != null ? resultTransformer.TransformList(results) : results;
    }

    protected override object GetResultColumnOrRow(
      object[] row,
      IResultTransformer resultTransformer,
      IDataReader rs,
      ISessionImplementor session)
    {
      row = this.ToResultRow(row);
      bool flag = this.HasSelectNew || resultTransformer != null;
      if (this._hasScalars)
      {
        string[][] scalarColumnNames = this._scalarColumnNames;
        int length = this._queryReturnTypes.Length;
        if (!flag && length == 1)
          return this._queryReturnTypes[0].NullSafeGet(rs, scalarColumnNames[0], session, (object) null);
        row = new object[length];
        for (int index = 0; index < length; ++index)
          row[index] = this._queryReturnTypes[index].NullSafeGet(rs, scalarColumnNames[index], session, (object) null);
        return (object) row;
      }
      if (flag)
        return (object) row;
      return row.Length != 1 ? (object) row : row[0];
    }

    private object[] ToResultRow(object[] row)
    {
      if (this._selectLength == row.Length)
        return row;
      object[] resultRow = new object[this._selectLength];
      int num = 0;
      for (int index = 0; index < row.Length; ++index)
      {
        if (this._includeInSelect[index])
          resultRow[num++] = row[index];
      }
      return resultRow;
    }

    private void CheckQuery(QueryParameters queryParameters)
    {
      if (this.HasSelectNew && queryParameters.ResultTransformer != null)
        throw new QueryException("ResultTransformer is not allowed for 'select new' queries.");
    }

    private bool HasSelectNew => this._selectNewTransformer != null;

    public IType[] ReturnTypes => this._queryReturnTypes;

    internal IEnumerable GetEnumerable(QueryParameters queryParameters, IEventSource session)
    {
      this.CheckQuery(queryParameters);
      bool statisticsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      IDbCommand dbCommand = this.PrepareQueryCommand(queryParameters, false, (ISessionImplementor) session);
      IDataReader resultSet = this.GetResultSet(dbCommand, queryParameters.HasAutoDiscoverScalarTypes, false, queryParameters.RowSelection, (ISessionImplementor) session);
      HolderInstantiator holderInstantiator = HolderInstantiator.GetHolderInstantiator(this._selectNewTransformer, queryParameters.ResultTransformer, this._queryReturnAliases);
      IEnumerable enumerable = (IEnumerable) new EnumerableImpl(resultSet, dbCommand, session, queryParameters.IsReadOnly((ISessionImplementor) session), this._queryTranslator.ReturnTypes, this._queryTranslator.GetColumnNames(), queryParameters.RowSelection, holderInstantiator);
      if (statisticsEnabled)
      {
        stopwatch.Stop();
        session.Factory.StatisticsImplementor.QueryExecuted("HQL: " + this._queryTranslator.QueryString, 0, stopwatch.Elapsed);
        session.Factory.StatisticsImplementor.QueryExecuted(this.QueryIdentifier, 0, stopwatch.Elapsed);
      }
      return enumerable;
    }

    protected override void ResetEffectiveExpectedType(
      IEnumerable<IParameterSpecification> parameterSpecs,
      QueryParameters queryParameters)
    {
      parameterSpecs.ResetEffectiveExpectedType(queryParameters);
    }

    protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
    {
      return (IEnumerable<IParameterSpecification>) this._queryTranslator.CollectedParameterSpecifications;
    }
  }
}
