// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.AbstractCollectionPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Cfg;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Id.Insert;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public abstract class AbstractCollectionPersister : 
    ICollectionMetadata,
    ISqlLoadableCollection,
    IQueryableCollection,
    IPropertyMapping,
    IJoinable,
    ICollectionPersister,
    IPostInsertIdentityPersister
  {
    protected static readonly object NotFoundPlaceHolder = new object();
    private readonly string role;
    private readonly SqlCommandInfo sqlDeleteString;
    private readonly SqlCommandInfo sqlInsertRowString;
    private readonly SqlCommandInfo sqlUpdateRowString;
    private readonly SqlCommandInfo sqlDeleteRowString;
    private readonly SqlString sqlSelectRowByIndexString;
    private readonly SqlString sqlDetectRowByIndexString;
    private readonly SqlString sqlDetectRowByElementString;
    private readonly string sqlOrderByString;
    protected readonly string sqlWhereString;
    private readonly string sqlOrderByStringTemplate;
    private readonly string sqlWhereStringTemplate;
    private readonly bool hasOrder;
    private readonly bool hasWhere;
    private readonly int baseIndex;
    private readonly string nodeName;
    private readonly string elementNodeName;
    private readonly string indexNodeName;
    protected internal bool indexContainsFormula;
    protected internal bool elementIsPureFormula;
    private readonly IType keyType;
    private readonly IType indexType;
    private readonly IType elementType;
    private readonly IType identifierType;
    private readonly string[] keyColumnNames;
    private readonly string[] indexColumnNames;
    protected readonly string[] indexFormulaTemplates;
    private readonly string[] indexFormulas;
    protected readonly bool[] indexColumnIsSettable;
    private readonly string[] elementColumnNames;
    protected readonly string[] elementFormulaTemplates;
    protected readonly string[] elementFormulas;
    protected readonly bool[] elementColumnIsSettable;
    protected readonly bool[] elementColumnIsInPrimaryKey;
    private readonly string[] indexColumnAliases;
    protected readonly string[] elementColumnAliases;
    private readonly string[] keyColumnAliases;
    private readonly string identifierColumnName;
    private readonly string identifierColumnAlias;
    protected readonly string qualifiedTableName;
    private readonly string queryLoaderName;
    private readonly bool isPrimitiveArray;
    private readonly bool isArray;
    private readonly bool hasIndex;
    protected readonly bool hasIdentifier;
    protected readonly IInsertGeneratedIdentifierDelegate identityDelegate;
    private readonly bool isLazy;
    private readonly bool isExtraLazy;
    private readonly bool isInverse;
    private readonly bool isMutable;
    private readonly bool isVersioned;
    protected readonly int batchSize;
    private readonly FetchMode fetchMode;
    private readonly bool hasOrphanDelete;
    private readonly bool subselectLoadable;
    private readonly System.Type elementClass;
    private readonly string entityName;
    private readonly NHibernate.Dialect.Dialect dialect;
    private readonly ISQLExceptionConverter sqlExceptionConverter;
    private readonly ISessionFactoryImplementor factory;
    private readonly IEntityPersister ownerPersister;
    private readonly IIdentifierGenerator identifierGenerator;
    private readonly IPropertyMapping elementPropertyMapping;
    private readonly IEntityPersister elementPersister;
    private readonly ICacheConcurrencyStrategy cache;
    private readonly CollectionType collectionType;
    private ICollectionInitializer initializer;
    private readonly ICacheEntryStructure cacheEntryStructure;
    private readonly FilterHelper filterHelper;
    private readonly FilterHelper manyToManyFilterHelper;
    private readonly string manyToManyWhereString;
    private readonly string manyToManyWhereTemplate;
    private readonly string manyToManyOrderByString;
    private readonly string manyToManyOrderByTemplate;
    private readonly bool insertCallable;
    private readonly bool updateCallable;
    private readonly bool deleteCallable;
    private readonly bool deleteAllCallable;
    private readonly ExecuteUpdateResultCheckStyle insertCheckStyle;
    private readonly ExecuteUpdateResultCheckStyle updateCheckStyle;
    private readonly ExecuteUpdateResultCheckStyle deleteCheckStyle;
    private readonly ExecuteUpdateResultCheckStyle deleteAllCheckStyle;
    private readonly string[] spaces;
    private readonly IDictionary<string, object> collectionPropertyColumnAliases = (IDictionary<string, object>) new Dictionary<string, object>();
    private readonly IDictionary<string, object> collectionPropertyColumnNames = (IDictionary<string, object>) new Dictionary<string, object>();
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ICollectionPersister));
    private string identitySelectString;
    private bool isCollectionIntegerIndex;

    public AbstractCollectionPersister(
      NHibernate.Mapping.Collection collection,
      ICacheConcurrencyStrategy cache,
      Configuration cfg,
      ISessionFactoryImplementor factory)
    {
      this.factory = factory;
      this.cache = cache;
      this.cacheEntryStructure = !factory.Settings.IsStructuredCacheEntriesEnabled ? (ICacheEntryStructure) new UnstructuredCacheEntry() : (collection.IsMap ? (ICacheEntryStructure) new StructuredMapCacheEntry() : (ICacheEntryStructure) new StructuredCollectionCacheEntry());
      this.dialect = factory.Dialect;
      this.sqlExceptionConverter = factory.SQLExceptionConverter;
      this.collectionType = collection.CollectionType;
      this.role = collection.Role;
      this.entityName = collection.OwnerEntityName;
      this.ownerPersister = factory.GetEntityPersister(this.entityName);
      this.queryLoaderName = collection.LoaderName;
      this.nodeName = collection.NodeName;
      this.isMutable = collection.IsMutable;
      Table collectionTable = collection.CollectionTable;
      this.fetchMode = collection.Element.FetchMode;
      this.elementType = collection.Element.Type;
      this.isPrimitiveArray = collection.IsPrimitiveArray;
      this.isArray = collection.IsArray;
      this.subselectLoadable = collection.IsSubselectLoadable;
      this.qualifiedTableName = collectionTable.GetQualifiedName(this.dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName);
      this.spaces = new string[1 + collection.SynchronizedTables.Count];
      int num1 = 0;
      string[] spaces = this.spaces;
      int index1 = num1;
      int num2 = index1 + 1;
      string qualifiedTableName = this.qualifiedTableName;
      spaces[index1] = qualifiedTableName;
      foreach (string synchronizedTable in (IEnumerable<string>) collection.SynchronizedTables)
        this.spaces[num2++] = synchronizedTable;
      this.sqlOrderByString = collection.OrderBy;
      this.hasOrder = this.sqlOrderByString != null;
      this.sqlOrderByStringTemplate = this.hasOrder ? Template.RenderOrderByStringTemplate(this.sqlOrderByString, this.dialect, factory.SQLFunctionRegistry) : (string) null;
      this.sqlWhereString = !string.IsNullOrEmpty(collection.Where) ? '('.ToString() + collection.Where + (object) ')' : (string) null;
      this.hasWhere = this.sqlWhereString != null;
      this.sqlWhereStringTemplate = this.hasWhere ? Template.RenderWhereStringTemplate(this.sqlWhereString, this.dialect, factory.SQLFunctionRegistry) : (string) null;
      this.hasOrphanDelete = collection.HasOrphanDelete;
      int num3 = collection.BatchSize;
      if (num3 == -1)
        num3 = factory.Settings.DefaultBatchFetchSize;
      this.batchSize = num3;
      this.isVersioned = collection.IsOptimisticLocked;
      this.keyType = collection.Key.Type;
      int columnSpan1 = collection.Key.ColumnSpan;
      this.keyColumnNames = new string[columnSpan1];
      this.keyColumnAliases = new string[columnSpan1];
      int index2 = 0;
      foreach (Column column in collection.Key.ColumnIterator)
      {
        this.keyColumnNames[index2] = column.GetQuotedName(this.dialect);
        this.keyColumnAliases[index2] = column.GetAlias(this.dialect);
        ++index2;
      }
      ISet distinctColumns = (ISet) new HashedSet();
      this.CheckColumnDuplication(distinctColumns, collection.Key.ColumnIterator);
      IValue element = collection.Element;
      if (!collection.IsOneToMany)
        this.CheckColumnDuplication(distinctColumns, element.ColumnIterator);
      string str = collection.ElementNodeName;
      if (this.elementType.IsEntityType)
      {
        string associatedEntityName = ((EntityType) this.elementType).GetAssociatedEntityName();
        this.elementPersister = factory.GetEntityPersister(associatedEntityName);
        if (str == null)
          str = cfg.GetClassMapping(associatedEntityName).NodeName;
      }
      else
        this.elementPersister = (IEntityPersister) null;
      this.elementNodeName = str;
      int columnSpan2 = element.ColumnSpan;
      this.elementColumnAliases = new string[columnSpan2];
      this.elementColumnNames = new string[columnSpan2];
      this.elementFormulaTemplates = new string[columnSpan2];
      this.elementFormulas = new string[columnSpan2];
      this.elementColumnIsSettable = new bool[columnSpan2];
      this.elementColumnIsInPrimaryKey = new bool[columnSpan2];
      bool flag1 = true;
      bool flag2 = false;
      int index3 = 0;
      foreach (ISelectable selectable in element.ColumnIterator)
      {
        this.elementColumnAliases[index3] = selectable.GetAlias(this.dialect);
        if (selectable.IsFormula)
        {
          Formula formula = (Formula) selectable;
          this.elementFormulaTemplates[index3] = formula.GetTemplate(this.dialect, factory.SQLFunctionRegistry);
          this.elementFormulas[index3] = formula.FormulaString;
        }
        else
        {
          Column column = (Column) selectable;
          this.elementColumnNames[index3] = column.GetQuotedName(this.dialect);
          this.elementColumnIsSettable[index3] = true;
          this.elementColumnIsInPrimaryKey[index3] = !column.IsNullable;
          if (!column.IsNullable)
            flag2 = true;
          flag1 = false;
        }
        ++index3;
      }
      this.elementIsPureFormula = flag1;
      if (!flag2)
        ArrayHelper.Fill<bool>(this.elementColumnIsInPrimaryKey, true);
      this.hasIndex = collection.IsIndexed;
      if (this.hasIndex)
      {
        IndexedCollection indexedCollection = (IndexedCollection) collection;
        this.indexType = indexedCollection.Index.Type;
        int columnSpan3 = indexedCollection.Index.ColumnSpan;
        this.indexColumnNames = new string[columnSpan3];
        this.indexFormulaTemplates = new string[columnSpan3];
        this.indexFormulas = new string[columnSpan3];
        this.indexColumnIsSettable = new bool[columnSpan3];
        this.indexColumnAliases = new string[columnSpan3];
        bool flag3 = false;
        int index4 = 0;
        foreach (ISelectable selectable in indexedCollection.Index.ColumnIterator)
        {
          this.indexColumnAliases[index4] = selectable.GetAlias(this.dialect);
          if (selectable.IsFormula)
          {
            Formula formula = (Formula) selectable;
            this.indexFormulaTemplates[index4] = formula.GetTemplate(this.dialect, factory.SQLFunctionRegistry);
            this.indexFormulas[index4] = formula.FormulaString;
            flag3 = true;
          }
          else
          {
            Column column = (Column) selectable;
            this.indexColumnNames[index4] = column.GetQuotedName(this.dialect);
            this.indexColumnIsSettable[index4] = true;
          }
          ++index4;
        }
        this.indexContainsFormula = flag3;
        this.baseIndex = indexedCollection.IsList ? ((List) indexedCollection).BaseIndex : 0;
        this.indexNodeName = indexedCollection.IndexNodeName;
        this.CheckColumnDuplication(distinctColumns, indexedCollection.Index.ColumnIterator);
      }
      else
      {
        this.indexContainsFormula = false;
        this.indexColumnIsSettable = (bool[]) null;
        this.indexFormulaTemplates = (string[]) null;
        this.indexFormulas = (string[]) null;
        this.indexType = (IType) null;
        this.indexColumnNames = (string[]) null;
        this.indexColumnAliases = (string[]) null;
        this.baseIndex = 0;
        this.indexNodeName = (string) null;
      }
      this.hasIdentifier = collection.IsIdentified;
      if (this.hasIdentifier)
      {
        IdentifierCollection identifierCollection = !collection.IsOneToMany ? (IdentifierCollection) collection : throw new MappingException("one-to-many collections with identifiers are not supported.");
        this.identifierType = identifierCollection.Identifier.Type;
        Column column = (Column) null;
        using (IEnumerator<ISelectable> enumerator = identifierCollection.Identifier.ColumnIterator.GetEnumerator())
        {
          if (enumerator.MoveNext())
            column = (Column) enumerator.Current;
        }
        this.identifierColumnName = column.GetQuotedName(this.dialect);
        this.identifierColumnAlias = column.GetAlias(this.dialect);
        this.identifierGenerator = identifierCollection.Identifier.CreateIdentifierGenerator(factory.Dialect, factory.Settings.DefaultCatalogName, factory.Settings.DefaultSchemaName, (RootClass) null);
        this.identityDelegate = !(this.identifierGenerator is IPostInsertIdentifierGenerator identifierGenerator) ? (IInsertGeneratedIdentifierDelegate) null : identifierGenerator.GetInsertGeneratedIdentifierDelegate((IPostInsertIdentityPersister) this, this.Factory, this.UseGetGeneratedKeys());
        this.CheckColumnDuplication(distinctColumns, identifierCollection.Identifier.ColumnIterator);
      }
      else
      {
        this.identifierType = (IType) null;
        this.identifierColumnName = (string) null;
        this.identifierColumnAlias = (string) null;
        this.identifierGenerator = (IIdentifierGenerator) null;
        this.identityDelegate = (IInsertGeneratedIdentifierDelegate) null;
      }
      if (collection.CustomSQLInsert == null)
      {
        this.sqlInsertRowString = this.IsIdentifierAssignedByInsert ? this.GenerateIdentityInsertRowString() : this.GenerateInsertRowString();
        this.insertCallable = false;
        this.insertCheckStyle = ExecuteUpdateResultCheckStyle.Count;
      }
      else
      {
        SqlType[] parameterTypes = this.GenerateInsertRowString().ParameterTypes;
        this.sqlInsertRowString = new SqlCommandInfo(collection.CustomSQLInsert, parameterTypes);
        this.insertCallable = collection.IsCustomInsertCallable;
        this.insertCheckStyle = collection.CustomSQLInsertCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(collection.CustomSQLInsert, this.insertCallable);
      }
      this.sqlUpdateRowString = this.GenerateUpdateRowString();
      if (collection.CustomSQLUpdate == null)
      {
        this.updateCallable = false;
        this.updateCheckStyle = ExecuteUpdateResultCheckStyle.Count;
      }
      else
      {
        this.sqlUpdateRowString = new SqlCommandInfo(collection.CustomSQLUpdate, this.sqlUpdateRowString.ParameterTypes);
        this.updateCallable = collection.IsCustomUpdateCallable;
        this.updateCheckStyle = collection.CustomSQLUpdateCheckStyle ?? ExecuteUpdateResultCheckStyle.DetermineDefault(collection.CustomSQLUpdate, this.updateCallable);
      }
      this.sqlDeleteRowString = this.GenerateDeleteRowString();
      if (collection.CustomSQLDelete == null)
      {
        this.deleteCallable = false;
        this.deleteCheckStyle = ExecuteUpdateResultCheckStyle.None;
      }
      else
      {
        this.sqlDeleteRowString = new SqlCommandInfo(collection.CustomSQLDelete, this.sqlDeleteRowString.ParameterTypes);
        this.deleteCallable = collection.IsCustomDeleteCallable;
        this.deleteCheckStyle = ExecuteUpdateResultCheckStyle.None;
      }
      this.sqlDeleteString = this.GenerateDeleteString();
      if (collection.CustomSQLDeleteAll == null)
      {
        this.deleteAllCallable = false;
        this.deleteAllCheckStyle = ExecuteUpdateResultCheckStyle.None;
      }
      else
      {
        this.sqlDeleteString = new SqlCommandInfo(collection.CustomSQLDeleteAll, this.sqlDeleteString.ParameterTypes);
        this.deleteAllCallable = collection.IsCustomDeleteAllCallable;
        this.deleteAllCheckStyle = ExecuteUpdateResultCheckStyle.None;
      }
      this.isCollectionIntegerIndex = collection.IsIndexed && !collection.IsMap;
      this.sqlDetectRowByIndexString = this.GenerateDetectRowByIndexString();
      this.sqlDetectRowByElementString = this.GenerateDetectRowByElementString();
      this.sqlSelectRowByIndexString = this.GenerateSelectRowByIndexString();
      this.LogStaticSQL();
      this.isLazy = collection.IsLazy;
      this.isExtraLazy = collection.ExtraLazy;
      this.isInverse = collection.IsInverse;
      this.elementClass = !collection.IsArray ? (System.Type) null : ((NHibernate.Mapping.Array) collection).ElementClass;
      if (this.elementType.IsComponentType)
        this.elementPropertyMapping = (IPropertyMapping) new CompositeElementPropertyMapping(this.elementColumnNames, this.elementFormulaTemplates, (IAbstractComponentType) this.elementType, (IMapping) factory);
      else if (!this.elementType.IsEntityType)
      {
        this.elementPropertyMapping = (IPropertyMapping) new ElementPropertyMapping(this.elementColumnNames, this.elementType);
      }
      else
      {
        this.elementPropertyMapping = this.elementPersister as IPropertyMapping;
        if (this.elementPropertyMapping == null)
          this.elementPropertyMapping = (IPropertyMapping) new ElementPropertyMapping(this.elementColumnNames, this.elementType);
      }
      this.filterHelper = new FilterHelper(collection.FilterMap, this.dialect, factory.SQLFunctionRegistry);
      this.manyToManyFilterHelper = new FilterHelper(collection.ManyToManyFilterMap, this.dialect, factory.SQLFunctionRegistry);
      this.manyToManyWhereString = !string.IsNullOrEmpty(collection.ManyToManyWhere) ? "( " + collection.ManyToManyWhere + " )" : (string) null;
      this.manyToManyWhereTemplate = this.manyToManyWhereString == null ? (string) null : Template.RenderWhereStringTemplate(this.manyToManyWhereString, factory.Dialect, factory.SQLFunctionRegistry);
      this.manyToManyOrderByString = collection.ManyToManyOrdering;
      this.manyToManyOrderByTemplate = this.manyToManyOrderByString == null ? (string) null : Template.RenderOrderByStringTemplate(this.manyToManyOrderByString, factory.Dialect, factory.SQLFunctionRegistry);
      this.InitCollectionPropertyMap();
    }

    public void PostInstantiate()
    {
      this.initializer = this.queryLoaderName == null ? this.CreateCollectionInitializer((IDictionary<string, NHibernate.IFilter>) new CollectionHelper.EmptyMapClass<string, NHibernate.IFilter>()) : (ICollectionInitializer) new NamedQueryCollectionInitializer(this.queryLoaderName, (ICollectionPersister) this);
    }

    protected void LogStaticSQL()
    {
      if (!AbstractCollectionPersister.log.IsDebugEnabled)
        return;
      AbstractCollectionPersister.log.Debug((object) ("Static SQL for collection: " + this.Role));
      if (this.SqlInsertRowString != null)
        AbstractCollectionPersister.log.Debug((object) (" Row insert: " + (object) this.SqlInsertRowString.Text));
      if (this.SqlUpdateRowString != null)
        AbstractCollectionPersister.log.Debug((object) (" Row update: " + (object) this.SqlUpdateRowString.Text));
      if (this.SqlDeleteRowString != null)
        AbstractCollectionPersister.log.Debug((object) (" Row delete: " + (object) this.SqlDeleteRowString.Text));
      if (this.SqlDeleteString == null)
        return;
      AbstractCollectionPersister.log.Debug((object) (" One-shot delete: " + (object) this.SqlDeleteString.Text));
    }

    public void Initialize(object key, ISessionImplementor session)
    {
      this.GetAppropriateInitializer(key, session).Initialize(key, session);
    }

    protected ICollectionInitializer GetAppropriateInitializer(
      object key,
      ISessionImplementor session)
    {
      if (this.queryLoaderName != null)
        return this.initializer;
      ICollectionInitializer subselectInitializer = this.GetSubselectInitializer(key, session);
      if (subselectInitializer != null)
        return subselectInitializer;
      return session.EnabledFilters.Count == 0 ? this.initializer : this.CreateCollectionInitializer(session.EnabledFilters);
    }

    private ICollectionInitializer GetSubselectInitializer(object key, ISessionImplementor session)
    {
      if (!this.IsSubselectLoadable)
        return (ICollectionInitializer) null;
      IPersistenceContext persistenceContext = session.PersistenceContext;
      SubselectFetch subselect = persistenceContext.BatchFetchQueue.GetSubselect(new EntityKey(key, this.OwnerEntityPersister, session.EntityMode));
      if (subselect == null)
        return (ICollectionInitializer) null;
      List<EntityKey> c = new List<EntityKey>(subselect.Result.Count);
      foreach (EntityKey key1 in (IEnumerable<EntityKey>) subselect.Result)
      {
        if (!persistenceContext.ContainsEntity(key1))
          c.Add(key1);
      }
      subselect.Result.RemoveAll((ICollection<EntityKey>) c);
      return this.CreateSubselectInitializer(subselect, session);
    }

    protected abstract ICollectionInitializer CreateSubselectInitializer(
      SubselectFetch subselect,
      ISessionImplementor session);

    protected abstract ICollectionInitializer CreateCollectionInitializer(
      IDictionary<string, NHibernate.IFilter> enabledFilters);

    public bool HasCache => this.cache != null;

    public string GetSQLWhereString(string alias)
    {
      return StringHelper.Replace(this.sqlWhereStringTemplate, Template.Placeholder, alias);
    }

    public string GetSQLOrderByString(string alias)
    {
      return !this.HasOrdering ? string.Empty : StringHelper.Replace(this.sqlOrderByStringTemplate, Template.Placeholder, alias);
    }

    public string GetManyToManyOrderByString(string alias)
    {
      return this.IsManyToMany && this.manyToManyOrderByString != null ? StringHelper.Replace(this.manyToManyOrderByTemplate, Template.Placeholder, alias) : string.Empty;
    }

    public bool HasOrdering => this.hasOrder;

    public bool HasManyToManyOrdering
    {
      get => this.IsManyToMany && this.manyToManyOrderByTemplate != null;
    }

    public bool HasWhere => this.hasWhere;

    public object ReadElement(
      IDataReader rs,
      object owner,
      string[] aliases,
      ISessionImplementor session)
    {
      return this.ElementType.NullSafeGet(rs, aliases, session, owner);
    }

    public object ReadIndex(IDataReader rs, string[] aliases, ISessionImplementor session)
    {
      return this.DecrementIndexByBase(this.IndexType.NullSafeGet(rs, aliases, session, (object) null) ?? throw new HibernateException("null index column for collection: " + this.role));
    }

    public object DecrementIndexByBase(object index)
    {
      if (this.baseIndex != 0)
        index = (object) ((int) index - this.baseIndex);
      return index;
    }

    public object ReadIdentifier(IDataReader rs, string alias, ISessionImplementor session)
    {
      return this.IdentifierType.NullSafeGet(rs, alias, session, (object) null) ?? throw new HibernateException("null identifier column for collection: " + this.role);
    }

    public object ReadKey(IDataReader dr, string[] aliases, ISessionImplementor session)
    {
      return this.KeyType.NullSafeGet(dr, aliases, session, (object) null);
    }

    protected int WriteKey(IDbCommand st, object id, int i, ISessionImplementor session)
    {
      if (id == null)
        throw new ArgumentNullException(nameof (id), "Null key for collection: " + this.role);
      this.KeyType.NullSafeSet(st, id, i, session);
      return i + this.keyColumnAliases.Length;
    }

    protected int WriteElement(IDbCommand st, object elt, int i, ISessionImplementor session)
    {
      this.ElementType.NullSafeSet(st, elt, i, this.elementColumnIsSettable, session);
      return i + ArrayHelper.CountTrue(this.elementColumnIsSettable);
    }

    protected int WriteIndex(IDbCommand st, object idx, int i, ISessionImplementor session)
    {
      this.IndexType.NullSafeSet(st, this.IncrementIndexByBase(idx), i, this.indexColumnIsSettable, session);
      return i + ArrayHelper.CountTrue(this.indexColumnIsSettable);
    }

    protected object IncrementIndexByBase(object index)
    {
      if (this.baseIndex != 0)
        index = (object) ((int) index + this.baseIndex);
      return index;
    }

    protected int WriteElementToWhere(
      IDbCommand st,
      object elt,
      int i,
      ISessionImplementor session)
    {
      if (this.elementIsPureFormula)
        throw new AssertionFailure("cannot use a formula-based element in the where condition");
      this.ElementType.NullSafeSet(st, elt, i, this.elementColumnIsInPrimaryKey, session);
      return i + this.elementColumnAliases.Length;
    }

    protected int WriteIndexToWhere(
      IDbCommand st,
      object index,
      int i,
      ISessionImplementor session)
    {
      if (this.indexContainsFormula)
        throw new AssertionFailure("cannot use a formula-based index in the where condition");
      this.IndexType.NullSafeSet(st, this.IncrementIndexByBase(index), i, session);
      return i + this.indexColumnAliases.Length;
    }

    protected int WriteIdentifier(IDbCommand st, object idx, int i, ISessionImplementor session)
    {
      this.IdentifierType.NullSafeSet(st, idx, i, session);
      return i + 1;
    }

    public string[] GetKeyColumnAliases(string suffix)
    {
      return new Alias(suffix).ToAliasStrings(this.keyColumnAliases, this.dialect);
    }

    public string[] GetElementColumnAliases(string suffix)
    {
      return new Alias(suffix).ToAliasStrings(this.elementColumnAliases, this.dialect);
    }

    public string[] GetIndexColumnAliases(string suffix)
    {
      return this.hasIndex ? new Alias(suffix).ToAliasStrings(this.indexColumnAliases, this.dialect) : (string[]) null;
    }

    public string GetIdentifierColumnAlias(string suffix)
    {
      return this.hasIdentifier ? new Alias(suffix).ToAliasString(this.identifierColumnAlias, this.dialect) : (string) null;
    }

    public string SelectFragment(string alias, string columnSuffix)
    {
      NHibernate.SqlCommand.SelectFragment selectFragment = this.GenerateSelectFragment(alias, columnSuffix);
      this.AppendElementColumns(selectFragment, alias);
      this.AppendIndexColumns(selectFragment, alias);
      this.AppendIdentifierColumns(selectFragment, alias);
      return selectFragment.ToSqlStringFragment(false);
    }

    private SqlString AddWhereFragment(SqlString sql)
    {
      return !this.hasWhere ? sql : sql.Append(" and ").Append(this.sqlWhereString);
    }

    private SqlString GenerateSelectSizeString(ISessionImplementor sessionImplementor)
    {
      string countSqlSelectClause = this.GetCountSqlSelectClause();
      return new SqlSimpleSelectBuilder(this.dialect, (IMapping) this.factory).SetTableName(this.TableName).AddWhereFragment(this.KeyColumnNames, this.KeyType, "=").AddColumn(countSqlSelectClause).ToSqlString().Append(this.FilterFragment(this.TableName, sessionImplementor.EnabledFilters));
    }

    protected virtual string GetCountSqlSelectClause()
    {
      if (this.isCollectionIntegerIndex)
        return string.Format("max({0}) + 1", (object) this.IndexColumnNames[0]);
      return !this.HasIndex ? string.Format("count({0})", (object) this.ElementColumnNames[0]) : string.Format("count({0})", (object) this.GetIndexCountExpression());
    }

    private string GetIndexCountExpression() => this.IndexColumnNames[0] ?? this.IndexFormulas[0];

    private SqlString GenerateDetectRowByIndexString()
    {
      return !this.hasIndex ? (SqlString) null : this.AddWhereFragment(new SqlSimpleSelectBuilder(this.dialect, (IMapping) this.factory).SetTableName(this.TableName).AddWhereFragment(this.KeyColumnNames, this.KeyType, "=").AddWhereFragment(this.IndexColumnNames, this.IndexType, "=").AddWhereFragment(this.indexFormulas, this.IndexType, "=").AddColumn("1").ToSqlString());
    }

    private SqlString GenerateSelectRowByIndexString()
    {
      return !this.hasIndex ? (SqlString) null : this.AddWhereFragment(new SqlSimpleSelectBuilder(this.dialect, (IMapping) this.factory).SetTableName(this.TableName).AddWhereFragment(this.KeyColumnNames, this.KeyType, "=").AddWhereFragment(this.IndexColumnNames, this.IndexType, "=").AddWhereFragment(this.indexFormulas, this.IndexType, "=").AddColumns(this.ElementColumnNames, this.elementColumnAliases).AddColumns(this.indexFormulas, this.indexColumnAliases).ToSqlString());
    }

    private SqlString GenerateDetectRowByElementString()
    {
      return this.AddWhereFragment(new SqlSimpleSelectBuilder(this.dialect, (IMapping) this.factory).SetTableName(this.TableName).AddWhereFragment(this.KeyColumnNames, this.KeyType, "=").AddWhereFragment(this.ElementColumnNames, this.ElementType, "=").AddWhereFragment(this.elementFormulas, this.ElementType, "=").AddColumn("1").ToSqlString());
    }

    protected virtual NHibernate.SqlCommand.SelectFragment GenerateSelectFragment(
      string alias,
      string columnSuffix)
    {
      return new NHibernate.SqlCommand.SelectFragment(this.dialect).SetSuffix(columnSuffix).AddColumns(alias, this.keyColumnNames, this.keyColumnAliases);
    }

    protected virtual void AppendElementColumns(NHibernate.SqlCommand.SelectFragment frag, string elemAlias)
    {
      for (int index = 0; index < this.elementColumnIsSettable.Length; ++index)
      {
        if (this.elementColumnIsSettable[index])
          frag.AddColumn(elemAlias, this.elementColumnNames[index], this.elementColumnAliases[index]);
        else
          frag.AddFormula(elemAlias, this.elementFormulaTemplates[index], this.elementColumnAliases[index]);
      }
    }

    protected virtual void AppendIndexColumns(NHibernate.SqlCommand.SelectFragment frag, string alias)
    {
      if (!this.hasIndex)
        return;
      for (int index = 0; index < this.indexColumnIsSettable.Length; ++index)
      {
        if (this.indexColumnIsSettable[index])
          frag.AddColumn(alias, this.indexColumnNames[index], this.indexColumnAliases[index]);
        else
          frag.AddFormula(alias, this.indexFormulaTemplates[index], this.indexColumnAliases[index]);
      }
    }

    protected virtual void AppendIdentifierColumns(NHibernate.SqlCommand.SelectFragment frag, string alias)
    {
      if (!this.hasIdentifier)
        return;
      frag.AddColumn(alias, this.identifierColumnName, this.identifierColumnAlias);
    }

    public string[] IndexColumnNames => this.indexColumnNames;

    public string[] GetIndexColumnNames(string alias)
    {
      return AbstractCollectionPersister.Qualify(alias, this.indexColumnNames, this.indexFormulaTemplates);
    }

    public string[] GetElementColumnNames(string alias)
    {
      return AbstractCollectionPersister.Qualify(alias, this.elementColumnNames, this.elementFormulaTemplates);
    }

    private static string[] Qualify(string alias, string[] columnNames, string[] formulaTemplates)
    {
      int length = columnNames.Length;
      string[] strArray = new string[length];
      for (int index = 0; index < length; ++index)
        strArray[index] = columnNames[index] != null ? StringHelper.Qualify(alias, columnNames[index]) : StringHelper.Replace(formulaTemplates[index], Template.Placeholder, alias);
      return strArray;
    }

    public string[] ElementColumnNames => this.elementColumnNames;

    public bool HasIndex => this.hasIndex;

    public virtual string TableName => this.qualifiedTableName;

    public void Remove(object id, ISessionImplementor session)
    {
      if (this.isInverse || !this.RowDeleteEnabled)
        return;
      if (AbstractCollectionPersister.log.IsDebugEnabled)
        AbstractCollectionPersister.log.Debug((object) ("Deleting collection: " + MessageHelper.InfoString((ICollectionPersister) this, id, this.Factory)));
      try
      {
        int i = 0;
        IExpectation expectation = Expectations.AppropriateExpectation(this.DeleteAllCheckStyle);
        bool canBeBatched = expectation.CanBeBatched;
        IDbCommand dbCommand = canBeBatched ? session.Batcher.PrepareBatchCommand(this.SqlDeleteString.CommandType, this.SqlDeleteString.Text, this.SqlDeleteString.ParameterTypes) : session.Batcher.PrepareCommand(this.SqlDeleteString.CommandType, this.SqlDeleteString.Text, this.SqlDeleteString.ParameterTypes);
        try
        {
          this.WriteKey(dbCommand, id, i, session);
          if (canBeBatched)
            session.Batcher.AddToBatch(expectation);
          else
            expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(dbCommand), dbCommand);
        }
        catch (Exception ex)
        {
          if (canBeBatched)
            session.Batcher.AbortBatch(ex);
          throw;
        }
        finally
        {
          if (!canBeBatched)
            session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        }
        if (!AbstractCollectionPersister.log.IsDebugEnabled)
          return;
        AbstractCollectionPersister.log.Debug((object) "done deleting collection");
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.sqlExceptionConverter, (Exception) ex, "could not delete collection: " + MessageHelper.InfoString((ICollectionPersister) this, id));
      }
    }

    public void Recreate(IPersistentCollection collection, object id, ISessionImplementor session)
    {
      if (this.isInverse || !this.RowInsertEnabled)
        return;
      if (AbstractCollectionPersister.log.IsDebugEnabled)
        AbstractCollectionPersister.log.Debug((object) ("Inserting collection: " + MessageHelper.InfoString((ICollectionPersister) this, id)));
      try
      {
        IEnumerator enumerator = collection.Entries((ICollectionPersister) this).GetEnumerator();
        if (enumerator.MoveNext())
        {
          enumerator.Reset();
          IExpectation expectation = Expectations.AppropriateExpectation(this.insertCheckStyle);
          collection.PreInsert((ICollectionPersister) this);
          bool canBeBatched = expectation.CanBeBatched;
          int num1 = 0;
          int num2 = 0;
          while (enumerator.MoveNext())
          {
            object current = enumerator.Current;
            if (collection.EntryExists(current, num1))
            {
              object id1 = this.IsIdentifierAssignedByInsert ? this.PerformInsert(id, collection, current, num1, session) : this.PerformInsert(id, collection, expectation, current, num1, canBeBatched, false, session);
              collection.AfterRowInsert((ICollectionPersister) this, current, num1, id1);
              ++num2;
            }
            ++num1;
          }
          if (!AbstractCollectionPersister.log.IsDebugEnabled)
            return;
          AbstractCollectionPersister.log.Debug((object) string.Format("done inserting collection: {0} rows inserted", (object) num2));
        }
        else
        {
          if (!AbstractCollectionPersister.log.IsDebugEnabled)
            return;
          AbstractCollectionPersister.log.Debug((object) "collection was empty");
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.sqlExceptionConverter, (Exception) ex, "could not insert collection: " + MessageHelper.InfoString((ICollectionPersister) this, id));
      }
    }

    public void DeleteRows(
      IPersistentCollection collection,
      object id,
      ISessionImplementor session)
    {
      if (this.isInverse || !this.RowDeleteEnabled)
        return;
      if (AbstractCollectionPersister.log.IsDebugEnabled)
        AbstractCollectionPersister.log.Debug((object) ("Deleting rows of collection: " + MessageHelper.InfoString((ICollectionPersister) this, id)));
      bool flag = !this.IsOneToMany && this.hasIndex && !this.indexContainsFormula;
      try
      {
        IEnumerator enumerator = collection.GetDeletes((ICollectionPersister) this, !flag).GetEnumerator();
        if (enumerator.MoveNext())
        {
          enumerator.Reset();
          int num1 = 0;
          int num2 = 0;
          while (enumerator.MoveNext())
          {
            IExpectation expectation = Expectations.AppropriateExpectation(this.deleteCheckStyle);
            bool canBeBatched = expectation.CanBeBatched;
            IDbCommand dbCommand = !canBeBatched ? session.Batcher.PrepareCommand(this.SqlDeleteRowString.CommandType, this.SqlDeleteRowString.Text, this.SqlDeleteRowString.ParameterTypes) : session.Batcher.PrepareBatchCommand(this.SqlDeleteRowString.CommandType, this.SqlDeleteRowString.Text, this.SqlDeleteRowString.ParameterTypes);
            try
            {
              object current = enumerator.Current;
              int i1 = num1;
              if (this.hasIdentifier)
              {
                this.WriteIdentifier(dbCommand, current, i1, session);
              }
              else
              {
                int i2 = this.WriteKey(dbCommand, id, i1, session);
                if (flag)
                  this.WriteIndexToWhere(dbCommand, current, i2, session);
                else
                  this.WriteElementToWhere(dbCommand, current, i2, session);
              }
              if (canBeBatched)
                session.Batcher.AddToBatch(expectation);
              else
                expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(dbCommand), dbCommand);
              ++num2;
            }
            catch (Exception ex)
            {
              if (canBeBatched)
                session.Batcher.AbortBatch(ex);
              throw;
            }
            finally
            {
              if (!canBeBatched)
                session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
            }
          }
          if (!AbstractCollectionPersister.log.IsDebugEnabled)
            return;
          AbstractCollectionPersister.log.Debug((object) ("done deleting collection rows: " + (object) num2 + " deleted"));
        }
        else
        {
          if (!AbstractCollectionPersister.log.IsDebugEnabled)
            return;
          AbstractCollectionPersister.log.Debug((object) "no rows to delete");
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.sqlExceptionConverter, (Exception) ex, "could not delete collection rows: " + MessageHelper.InfoString((ICollectionPersister) this, id));
      }
    }

    public void InsertRows(
      IPersistentCollection collection,
      object id,
      ISessionImplementor session)
    {
      if (this.isInverse || !this.RowInsertEnabled)
        return;
      if (AbstractCollectionPersister.log.IsDebugEnabled)
        AbstractCollectionPersister.log.Debug((object) ("Inserting rows of collection: " + MessageHelper.InfoString((ICollectionPersister) this, id, this.Factory)));
      try
      {
        collection.PreInsert((ICollectionPersister) this);
        IExpectation expectation = Expectations.AppropriateExpectation(this.insertCheckStyle);
        bool canBeBatched = expectation.CanBeBatched;
        int num1 = 0;
        int num2 = 0;
        foreach (object entry in collection.Entries((ICollectionPersister) this))
        {
          if (collection.NeedsInserting(entry, num1, this.elementType))
          {
            object id1 = this.IsIdentifierAssignedByInsert ? this.PerformInsert(id, collection, entry, num1, session) : this.PerformInsert(id, collection, expectation, entry, num1, canBeBatched, false, session);
            collection.AfterRowInsert((ICollectionPersister) this, entry, num1, id1);
            ++num2;
          }
          ++num1;
        }
        if (!AbstractCollectionPersister.log.IsDebugEnabled)
          return;
        AbstractCollectionPersister.log.Debug((object) string.Format("done inserting rows: {0} inserted", (object) num2));
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.sqlExceptionConverter, (Exception) ex, "could not insert collection rows: " + MessageHelper.InfoString((ICollectionPersister) this, id));
      }
    }

    public bool HasOrphanDelete => this.hasOrphanDelete;

    public IType ToType(string propertyName)
    {
      return "index".Equals(propertyName) ? this.indexType : this.elementPropertyMapping.ToType(propertyName);
    }

    public bool TryToType(string propertyName, out IType type)
    {
      if (!"index".Equals(propertyName))
        return this.elementPropertyMapping.TryToType(propertyName, out type);
      type = this.indexType;
      return true;
    }

    public string GetManyToManyFilterFragment(
      string alias,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      StringBuilder buffer = new StringBuilder();
      this.manyToManyFilterHelper.Render(buffer, alias, enabledFilters);
      if (this.manyToManyWhereString != null)
        buffer.Append(" and ").Append(StringHelper.Replace(this.manyToManyWhereTemplate, Template.Placeholder, alias));
      return buffer.ToString();
    }

    public string[] ToColumns(string alias, string propertyName)
    {
      if (!"index".Equals(propertyName))
        return this.elementPropertyMapping.ToColumns(alias, propertyName);
      if (this.IsManyToMany)
        throw new QueryException("index() function not supported for many-to-many association");
      return StringHelper.Qualify(alias, this.indexColumnNames);
    }

    public string[] ToColumns(string propertyName)
    {
      if (!"index".Equals(propertyName))
        return this.elementPropertyMapping.ToColumns(propertyName);
      if (this.IsManyToMany)
        throw new QueryException("index() function not supported for many-to-many association");
      return this.indexColumnNames;
    }

    protected abstract SqlCommandInfo GenerateDeleteString();

    protected abstract SqlCommandInfo GenerateDeleteRowString();

    protected abstract SqlCommandInfo GenerateUpdateRowString();

    protected abstract SqlCommandInfo GenerateInsertRowString();

    protected abstract SqlCommandInfo GenerateIdentityInsertRowString();

    public void UpdateRows(
      IPersistentCollection collection,
      object id,
      ISessionImplementor session)
    {
      if (this.isInverse || !collection.RowUpdatePossible)
        return;
      if (AbstractCollectionPersister.log.IsDebugEnabled)
        AbstractCollectionPersister.log.Debug((object) string.Format("Updating rows of collection: {0}#{1}", (object) this.role, id));
      int num = this.DoUpdateRows(id, collection, session);
      if (!AbstractCollectionPersister.log.IsDebugEnabled)
        return;
      AbstractCollectionPersister.log.Debug((object) string.Format("done updating rows: {0} updated", (object) num));
    }

    protected abstract int DoUpdateRows(
      object key,
      IPersistentCollection collection,
      ISessionImplementor session);

    protected virtual string FilterFragment(string alias)
    {
      return !this.HasWhere ? "" : " and " + this.GetSQLWhereString(alias);
    }

    public virtual string FilterFragment(string alias, IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      StringBuilder buffer = new StringBuilder();
      this.filterHelper.Render(buffer, alias, enabledFilters);
      return buffer.Append(this.FilterFragment(alias)).ToString();
    }

    public string OneToManyFilterFragment(string alias) => string.Empty;

    public override string ToString()
    {
      return string.Format("{0}({1})", (object) this.GetType().Name, (object) this.role);
    }

    public bool IsAffectedByEnabledFilters(ISessionImplementor session)
    {
      if (this.filterHelper.IsAffectedBy(session.EnabledFilters))
        return true;
      return this.IsManyToMany && this.manyToManyFilterHelper.IsAffectedBy(session.EnabledFilters);
    }

    public string[] GetCollectionPropertyColumnAliases(string propertyName, string suffix)
    {
      object obj;
      if (!this.collectionPropertyColumnAliases.TryGetValue(propertyName, out obj))
        return (string[]) null;
      string[] strArray = (string[]) obj;
      string[] propertyColumnAliases = new string[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        propertyColumnAliases[index] = new Alias(suffix).ToUnquotedAliasString(strArray[index], this.dialect);
      return propertyColumnAliases;
    }

    public void InitCollectionPropertyMap()
    {
      this.InitCollectionPropertyMap("key", this.keyType, this.keyColumnAliases, this.keyColumnNames);
      this.InitCollectionPropertyMap("element", this.elementType, this.elementColumnAliases, this.elementColumnNames);
      if (this.hasIndex)
        this.InitCollectionPropertyMap("index", this.indexType, this.indexColumnAliases, this.indexColumnNames);
      if (!this.hasIdentifier)
        return;
      this.InitCollectionPropertyMap("id", this.identifierType, new string[1]
      {
        this.identifierColumnAlias
      }, new string[1]{ this.identifierColumnName });
    }

    private void InitCollectionPropertyMap(
      string aliasName,
      IType type,
      string[] columnAliases,
      string[] columnNames)
    {
      this.collectionPropertyColumnAliases[aliasName] = (object) columnAliases;
      this.collectionPropertyColumnNames[aliasName] = (object) columnNames;
      if (!type.IsComponentType)
        return;
      int sourceIndex = 0;
      IAbstractComponentType abstractComponentType = (IAbstractComponentType) type;
      string[] propertyNames = abstractComponentType.PropertyNames;
      for (int index = 0; index < propertyNames.Length; ++index)
      {
        string str = propertyNames[index];
        IType subtype = abstractComponentType.Subtypes[index];
        int count = 0;
        AbstractCollectionPersister.CalcPropertyColumnSpan(subtype, ref count);
        string[] strArray1 = new string[count];
        string[] strArray2 = new string[count];
        System.Array.Copy((System.Array) columnAliases, sourceIndex, (System.Array) strArray1, 0, count);
        System.Array.Copy((System.Array) columnNames, sourceIndex, (System.Array) strArray2, 0, count);
        this.InitCollectionPropertyMap(aliasName + "." + str, subtype, strArray1, strArray2);
        sourceIndex += count;
      }
    }

    private static void CalcPropertyColumnSpan(IType propertyType, ref int count)
    {
      if (!propertyType.IsComponentType)
      {
        ++count;
      }
      else
      {
        foreach (IType subtype in ((IAbstractComponentType) propertyType).Subtypes)
          AbstractCollectionPersister.CalcPropertyColumnSpan(subtype, ref count);
      }
    }

    public int GetSize(object key, ISessionImplementor session)
    {
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          int count = session.EnabledFilters.Count;
          IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.GenerateSelectSizeString(session), this.KeyType.SqlTypes((IMapping) this.factory));
          IDataReader reader = (IDataReader) null;
          try
          {
            this.KeyType.NullSafeSet(dbCommand, key, 0, session);
            reader = session.Batcher.ExecuteReader(dbCommand);
            return reader.Read() ? Convert.ToInt32(reader.GetValue(0)) - this.baseIndex : 0;
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, reader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "could not retrieve collection size: " + MessageHelper.InfoString((ICollectionPersister) this, key, this.Factory), this.GenerateSelectSizeString(session));
        }
      }
    }

    public bool IndexExists(object key, object index, ISessionImplementor session)
    {
      return this.Exists(key, this.IncrementIndexByBase(index), this.IndexType, this.sqlDetectRowByIndexString, session);
    }

    public bool ElementExists(object key, object element, ISessionImplementor session)
    {
      return this.Exists(key, element, this.ElementType, this.sqlDetectRowByElementString, session);
    }

    private bool Exists(
      object key,
      object indexOrElement,
      IType indexOrElementType,
      SqlString sql,
      ISessionImplementor session)
    {
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          List<SqlType> sqlTypeList = new List<SqlType>((IEnumerable<SqlType>) this.KeyType.SqlTypes((IMapping) this.factory));
          sqlTypeList.AddRange((IEnumerable<SqlType>) indexOrElementType.SqlTypes((IMapping) this.factory));
          IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, sql, sqlTypeList.ToArray());
          IDataReader reader = (IDataReader) null;
          try
          {
            this.KeyType.NullSafeSet(dbCommand, key, 0, session);
            indexOrElementType.NullSafeSet(dbCommand, indexOrElement, this.keyColumnNames.Length, session);
            reader = session.Batcher.ExecuteReader(dbCommand);
            try
            {
              return reader.Read();
            }
            finally
            {
              reader.Close();
            }
          }
          catch (TransientObjectException ex)
          {
            return false;
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, reader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "could not check row existence: " + MessageHelper.InfoString((ICollectionPersister) this, key, this.Factory), this.GenerateSelectSizeString(session));
        }
      }
    }

    public virtual object GetElementByIndex(
      object key,
      object index,
      ISessionImplementor session,
      object owner)
    {
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          List<SqlType> sqlTypeList = new List<SqlType>((IEnumerable<SqlType>) this.KeyType.SqlTypes((IMapping) this.factory));
          sqlTypeList.AddRange((IEnumerable<SqlType>) this.IndexType.SqlTypes((IMapping) this.factory));
          IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.sqlSelectRowByIndexString, sqlTypeList.ToArray());
          IDataReader dataReader = (IDataReader) null;
          try
          {
            this.KeyType.NullSafeSet(dbCommand, key, 0, session);
            this.IndexType.NullSafeSet(dbCommand, this.IncrementIndexByBase(index), this.keyColumnNames.Length, session);
            dataReader = session.Batcher.ExecuteReader(dbCommand);
            try
            {
              return dataReader.Read() ? this.ElementType.NullSafeGet(dataReader, this.elementColumnAliases, session, owner) : this.NotFoundObject;
            }
            finally
            {
              dataReader.Close();
            }
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, dataReader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, (Exception) ex, "could not read row: " + MessageHelper.InfoString((ICollectionPersister) this, key, this.Factory), this.GenerateSelectSizeString(session));
        }
      }
    }

    public object NotFoundObject => AbstractCollectionPersister.NotFoundPlaceHolder;

    public abstract bool ConsumesEntityAlias();

    public abstract SqlString FromJoinFragment(
      string alias,
      bool innerJoin,
      bool includeSubclasses);

    public abstract SqlString WhereJoinFragment(
      string alias,
      bool innerJoin,
      bool includeSubclasses);

    public abstract string SelectFragment(
      IJoinable rhs,
      string rhsAlias,
      string lhsAlias,
      string currentEntitySuffix,
      string currentCollectionSuffix,
      bool includeCollectionColumns);

    public abstract bool ConsumesCollectionAlias();

    private void CheckColumnDuplication(ISet distinctColumns, IEnumerable<ISelectable> columns)
    {
      foreach (ISelectable column1 in columns)
      {
        if (column1 is Column column2)
        {
          if (distinctColumns.Contains((object) column2.Name))
            throw new MappingException("Repeated column in mapping for collection: " + this.role + " column: " + column2.Name);
          distinctColumns.Add((object) column2.Name);
        }
      }
    }

    public ICacheConcurrencyStrategy Cache => this.cache;

    public CollectionType CollectionType => this.collectionType;

    public FetchMode FetchMode => this.fetchMode;

    protected SqlCommandInfo SqlDeleteString => this.sqlDeleteString;

    protected SqlCommandInfo SqlInsertRowString => this.sqlInsertRowString;

    protected SqlCommandInfo SqlUpdateRowString => this.sqlUpdateRowString;

    protected SqlCommandInfo SqlDeleteRowString => this.sqlDeleteRowString;

    public IType KeyType => this.keyType;

    public IType IndexType => this.indexType;

    public IType ElementType => this.elementType;

    public System.Type ElementClass => this.elementClass;

    public bool IsPrimitiveArray => this.isPrimitiveArray;

    public bool IsArray => this.isArray;

    public string IdentifierColumnName
    {
      get => this.hasIdentifier ? this.identifierColumnName : (string) null;
    }

    public string[] IndexFormulas => this.indexFormulas;

    public string[] KeyColumnNames => this.keyColumnNames;

    protected string[] KeyColumnAliases => this.keyColumnAliases;

    public bool IsLazy => this.isLazy;

    public bool IsInverse => this.isInverse;

    protected virtual bool RowDeleteEnabled => true;

    protected virtual bool RowInsertEnabled => true;

    public string Role => this.role;

    public virtual string OwnerEntityName => this.entityName;

    public IEntityPersister OwnerEntityPersister => this.ownerPersister;

    public IIdentifierGenerator IdentifierGenerator => this.identifierGenerator;

    public IType IdentifierType => this.identifierType;

    public abstract bool IsManyToMany { get; }

    public IType Type => this.elementPropertyMapping.Type;

    public string Name => this.Role;

    public IEntityPersister ElementPersister
    {
      get
      {
        return this.elementPersister != null ? this.elementPersister : throw new AssertionFailure("Not an association");
      }
    }

    public bool IsCollection => true;

    public string[] CollectionSpaces => this.spaces;

    public ICollectionMetadata CollectionMetadata => (ICollectionMetadata) this;

    public ISessionFactoryImplementor Factory => this.factory;

    protected virtual bool InsertCallable => this.insertCallable;

    protected ExecuteUpdateResultCheckStyle InsertCheckStyle => this.insertCheckStyle;

    protected virtual bool UpdateCallable => this.updateCallable;

    protected ExecuteUpdateResultCheckStyle UpdateCheckStyle => this.updateCheckStyle;

    protected virtual bool DeleteCallable => this.deleteCallable;

    protected ExecuteUpdateResultCheckStyle DeleteCheckStyle => this.deleteCheckStyle;

    protected virtual bool DeleteAllCallable => this.deleteAllCallable;

    protected ExecuteUpdateResultCheckStyle DeleteAllCheckStyle => this.deleteAllCheckStyle;

    public bool IsVersioned => this.isVersioned && this.OwnerEntityPersister.IsVersioned;

    public string NodeName => this.nodeName;

    public string ElementNodeName => this.elementNodeName;

    public string IndexNodeName => this.indexNodeName;

    protected virtual ISQLExceptionConverter SQLExceptionConverter => this.sqlExceptionConverter;

    public ICacheEntryStructure CacheEntryStructure => this.cacheEntryStructure;

    public bool IsSubselectLoadable => this.subselectLoadable;

    public bool IsMutable => this.isMutable;

    public bool IsExtraLazy => this.isExtraLazy;

    protected NHibernate.Dialect.Dialect Dialect => this.dialect;

    public abstract bool CascadeDeleteEnabled { get; }

    public abstract bool IsOneToMany { get; }

    protected object PerformInsert(
      object ownerId,
      IPersistentCollection collection,
      IExpectation expectation,
      object entry,
      int index,
      bool useBatch,
      bool callable,
      ISessionImplementor session)
    {
      object idx = (object) null;
      int i1 = 0;
      IDbCommand dbCommand = useBatch ? session.Batcher.PrepareBatchCommand(this.SqlInsertRowString.CommandType, this.SqlInsertRowString.Text, this.SqlInsertRowString.ParameterTypes) : session.Batcher.PrepareCommand(this.SqlInsertRowString.CommandType, this.SqlInsertRowString.Text, this.SqlInsertRowString.ParameterTypes);
      try
      {
        int i2 = this.WriteKey(dbCommand, ownerId, i1, session);
        if (this.hasIdentifier)
        {
          idx = collection.GetIdentifier(entry, index);
          i2 = this.WriteIdentifier(dbCommand, idx, i2, session);
        }
        if (this.hasIndex)
          i2 = this.WriteIndex(dbCommand, collection.GetIndex(entry, index, (ICollectionPersister) this), i2, session);
        this.WriteElement(dbCommand, collection.GetElement(entry), i2, session);
        if (useBatch)
          session.Batcher.AddToBatch(expectation);
        else
          expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(dbCommand), dbCommand);
      }
      catch (Exception ex)
      {
        if (useBatch)
          session.Batcher.AbortBatch(ex);
        throw;
      }
      finally
      {
        if (!useBatch)
          session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
      }
      return idx;
    }

    public bool IsIdentifierAssignedByInsert => this.identityDelegate != null;

    protected bool UseInsertSelectIdentity()
    {
      return !this.UseGetGeneratedKeys() && this.Factory.Dialect.SupportsInsertSelectIdentity;
    }

    protected bool UseGetGeneratedKeys() => this.Factory.Settings.IsGetGeneratedKeysEnabled;

    public string IdentitySelectString
    {
      get
      {
        if (this.identitySelectString == null)
          this.identitySelectString = this.Factory.Dialect.GetIdentitySelectString(this.IdentifierColumnName, this.qualifiedTableName, this.IdentifierType.SqlTypes((IMapping) this.Factory)[0].DbType);
        return this.identitySelectString;
      }
    }

    public string[] RootTableKeyColumnNames
    {
      get => new string[1]{ this.IdentifierColumnName };
    }

    public SqlString GetSelectByUniqueKeyString(string propertyName)
    {
      return new SqlSimpleSelectBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(this.qualifiedTableName).AddColumns(this.KeyColumnNames).AddWhereFragment(this.KeyColumnNames, this.KeyType, " = ").ToSqlString();
    }

    public string GetInfoString()
    {
      return MessageHelper.InfoString((ICollectionPersister) this, (object) null);
    }

    protected object PerformInsert(
      object ownerId,
      IPersistentCollection collection,
      object entry,
      int index,
      ISessionImplementor session)
    {
      IBinder binder = (IBinder) new AbstractCollectionPersister.GeneratedIdentifierBinder(ownerId, collection, entry, index, session, this);
      return this.identityDelegate.PerformInsert(this.SqlInsertRowString, session, binder);
    }

    protected class GeneratedIdentifierBinder : IBinder
    {
      private readonly object ownerId;
      private readonly IPersistentCollection collection;
      private readonly object entry;
      private readonly int index;
      private readonly ISessionImplementor session;
      private readonly AbstractCollectionPersister persister;

      public GeneratedIdentifierBinder(
        object ownerId,
        IPersistentCollection collection,
        object entry,
        int index,
        ISessionImplementor session,
        AbstractCollectionPersister persister)
      {
        this.ownerId = ownerId;
        this.collection = collection;
        this.entry = entry;
        this.index = index;
        this.session = session;
        this.persister = persister;
      }

      public object Entity => this.entry;

      public void BindValues(IDbCommand cm)
      {
        int i1 = 0;
        int i2 = this.persister.WriteKey(cm, this.ownerId, i1, this.session);
        if (this.persister.HasIndex)
          i2 = this.persister.WriteIndex(cm, this.collection.GetIndex(this.entry, this.index, (ICollectionPersister) this.persister), i2, this.session);
        this.persister.WriteElement(cm, this.collection.GetElement(this.entry), i2, this.session);
      }
    }
  }
}
