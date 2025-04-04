// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.AbstractEntityPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Dialect.Lock;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Id.Insert;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Loader.Entity;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.Properties;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Tuple.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public abstract class AbstractEntityPersister : 
    IOuterJoinLoadable,
    IQueryable,
    IPropertyMapping,
    IJoinable,
    IClassMetadata,
    IUniqueKeyLoadable,
    ISqlLoadable,
    ILoadable,
    ILazyPropertyInitializer,
    IPostInsertIdentityPersister,
    ILockable,
    IEntityPersister,
    IOptimisticCacheSource
  {
    public const string EntityClass = "class";
    protected const string Discriminator_Alias = "clazz_";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractEntityPersister));
    private readonly ISessionFactoryImplementor factory;
    private readonly ICacheConcurrencyStrategy cache;
    private readonly bool isLazyPropertiesCacheable;
    private readonly ICacheEntryStructure cacheEntryStructure;
    private readonly EntityMetamodel entityMetamodel;
    private readonly Dictionary<System.Type, string> entityNameBySubclass = new Dictionary<System.Type, string>();
    private readonly string[] rootTableKeyColumnNames;
    private readonly string[] identifierAliases;
    private readonly int identifierColumnSpan;
    private readonly string versionColumnName;
    private readonly bool hasFormulaProperties;
    private readonly int batchSize;
    private readonly bool hasSubselectLoadableCollections;
    protected internal string rowIdName;
    private readonly ISet<string> lazyProperties;
    private readonly string sqlWhereString;
    private readonly string sqlWhereStringTemplate;
    private readonly int[] propertyColumnSpans;
    private readonly string[] propertySubclassNames;
    private readonly string[][] propertyColumnAliases;
    private readonly string[][] propertyColumnNames;
    private readonly string[][] propertyColumnFormulaTemplates;
    private readonly bool[][] propertyColumnUpdateable;
    private readonly bool[][] propertyColumnInsertable;
    private readonly bool[] propertyUniqueness;
    private readonly bool[] propertySelectable;
    private readonly string[] lazyPropertyNames;
    private readonly int[] lazyPropertyNumbers;
    private readonly IType[] lazyPropertyTypes;
    private readonly string[][] lazyPropertyColumnAliases;
    private readonly string[] subclassPropertyNameClosure;
    private readonly string[] subclassPropertySubclassNameClosure;
    private readonly IType[] subclassPropertyTypeClosure;
    private readonly string[][] subclassPropertyFormulaTemplateClosure;
    private readonly string[][] subclassPropertyColumnNameClosure;
    private readonly FetchMode[] subclassPropertyFetchModeClosure;
    private readonly bool[] subclassPropertyNullabilityClosure;
    protected bool[] propertyDefinedOnSubclass;
    private readonly int[][] subclassPropertyColumnNumberClosure;
    private readonly int[][] subclassPropertyFormulaNumberClosure;
    private readonly CascadeStyle[] subclassPropertyCascadeStyleClosure;
    private readonly string[] subclassColumnClosure;
    private readonly bool[] subclassColumnLazyClosure;
    private readonly string[] subclassColumnAliasClosure;
    private readonly bool[] subclassColumnSelectableClosure;
    private readonly string[] subclassFormulaClosure;
    private readonly string[] subclassFormulaTemplateClosure;
    private readonly string[] subclassFormulaAliasClosure;
    private readonly bool[] subclassFormulaLazyClosure;
    private readonly FilterHelper filterHelper;
    private readonly Dictionary<string, EntityLoader> uniqueKeyLoaders = new Dictionary<string, EntityLoader>();
    private readonly Dictionary<LockMode, ILockingStrategy> lockers = new Dictionary<LockMode, ILockingStrategy>();
    private readonly Dictionary<string, IUniqueEntityLoader> loaders = new Dictionary<string, IUniqueEntityLoader>();
    private SqlString sqlVersionSelectString;
    private SqlString sqlSnapshotSelectString;
    private SqlString sqlLazySelectString;
    private SqlCommandInfo sqlIdentityInsertString;
    private SqlCommandInfo sqlUpdateByRowIdString;
    private SqlCommandInfo sqlLazyUpdateByRowIdString;
    private SqlCommandInfo[] sqlDeleteStrings;
    private SqlCommandInfo[] sqlInsertStrings;
    private SqlCommandInfo[] sqlUpdateStrings;
    private SqlCommandInfo[] sqlLazyUpdateStrings;
    private SqlString sqlInsertGeneratedValuesSelectString;
    private SqlString sqlUpdateGeneratedValuesSelectString;
    private string identitySelectString;
    protected internal bool[] insertCallable;
    protected internal bool[] updateCallable;
    protected internal bool[] deleteCallable;
    protected internal SqlString[] customSQLInsert;
    protected internal SqlString[] customSQLUpdate;
    protected internal SqlString[] customSQLDelete;
    protected internal ExecuteUpdateResultCheckStyle[] insertResultCheckStyles;
    protected internal ExecuteUpdateResultCheckStyle[] updateResultCheckStyles;
    protected internal ExecuteUpdateResultCheckStyle[] deleteResultCheckStyles;
    private IInsertGeneratedIdentifierDelegate identityDelegate;
    private bool[] tableHasColumns;
    private readonly string loaderName;
    private IUniqueEntityLoader queryLoader;
    private readonly string temporaryIdTableName;
    private readonly string temporaryIdTableDDL;
    private readonly Dictionary<string, string[]> subclassPropertyAliases = new Dictionary<string, string[]>();
    private readonly Dictionary<string, string[]> subclassPropertyColumnNames = new Dictionary<string, string[]>();
    protected readonly BasicEntityPropertyMapping propertyMapping;
    private SqlType[] idAndVersionSqlTypes;

    protected AbstractEntityPersister(
      PersistentClass persistentClass,
      ICacheConcurrencyStrategy cache,
      ISessionFactoryImplementor factory)
    {
      this.factory = factory;
      this.cache = cache;
      this.isLazyPropertiesCacheable = persistentClass.IsLazyPropertiesCacheable;
      this.cacheEntryStructure = factory.Settings.IsStructuredCacheEntriesEnabled ? (ICacheEntryStructure) new StructuredCacheEntry((IEntityPersister) this) : (ICacheEntryStructure) new UnstructuredCacheEntry();
      this.entityMetamodel = new EntityMetamodel(persistentClass, factory);
      if (persistentClass.HasPocoRepresentation)
      {
        foreach (Subclass subclass in persistentClass.SubclassIterator)
          this.entityNameBySubclass[subclass.MappedClass] = subclass.EntityName;
      }
      this.batchSize = persistentClass.BatchSize.HasValue ? persistentClass.BatchSize.Value : factory.Settings.DefaultBatchFetchSize;
      this.hasSubselectLoadableCollections = persistentClass.HasSubselectLoadableCollections;
      this.propertyMapping = new BasicEntityPropertyMapping(this);
      this.identifierColumnSpan = persistentClass.Identifier.ColumnSpan;
      this.rootTableKeyColumnNames = new string[this.identifierColumnSpan];
      this.identifierAliases = new string[this.identifierColumnSpan];
      this.rowIdName = persistentClass.RootTable.RowId;
      this.loaderName = persistentClass.LoaderName;
      int index1 = 0;
      foreach (Column column in persistentClass.Identifier.ColumnIterator)
      {
        this.rootTableKeyColumnNames[index1] = column.GetQuotedName(factory.Dialect);
        this.identifierAliases[index1] = column.GetAlias(factory.Dialect, persistentClass.RootTable);
        ++index1;
      }
      if (persistentClass.IsVersioned)
      {
        using (IEnumerator<ISelectable> enumerator = persistentClass.Version.ColumnIterator.GetEnumerator())
        {
          if (enumerator.MoveNext())
            this.versionColumnName = ((Column) enumerator.Current).GetQuotedName(factory.Dialect);
        }
      }
      else
        this.versionColumnName = (string) null;
      this.sqlWhereString = !string.IsNullOrEmpty(persistentClass.Where) ? "( " + persistentClass.Where + ") " : (string) null;
      this.sqlWhereStringTemplate = this.sqlWhereString == null ? (string) null : Template.RenderWhereStringTemplate(this.sqlWhereString, factory.Dialect, factory.SQLFunctionRegistry);
      bool flag1 = this.IsInstrumented(EntityMode.Poco) && this.entityMetamodel.IsLazy;
      int propertySpan = this.entityMetamodel.PropertySpan;
      this.propertyColumnSpans = new int[propertySpan];
      this.propertySubclassNames = new string[propertySpan];
      this.propertyColumnAliases = new string[propertySpan][];
      this.propertyColumnNames = new string[propertySpan][];
      this.propertyColumnFormulaTemplates = new string[propertySpan][];
      this.propertyUniqueness = new bool[propertySpan];
      this.propertySelectable = new bool[propertySpan];
      this.propertyColumnUpdateable = new bool[propertySpan][];
      this.propertyColumnInsertable = new bool[propertySpan][];
      ISet set = (ISet) new HashedSet();
      this.lazyProperties = (ISet<string>) new HashedSet<string>();
      List<string> stringList1 = new List<string>();
      List<int> intList = new List<int>();
      List<IType> typeList1 = new List<IType>();
      List<string[]> strArrayList1 = new List<string[]>();
      int index2 = 0;
      bool flag2 = false;
      foreach (Property o in persistentClass.PropertyClosureIterator)
      {
        set.Add((object) o);
        int columnSpan = o.ColumnSpan;
        this.propertyColumnSpans[index2] = columnSpan;
        this.propertySubclassNames[index2] = o.PersistentClass.EntityName;
        string[] strArray1 = new string[columnSpan];
        string[] strArray2 = new string[columnSpan];
        string[] strArray3 = new string[columnSpan];
        int index3 = 0;
        foreach (ISelectable selectable in o.ColumnIterator)
        {
          strArray2[index3] = selectable.GetAlias(factory.Dialect, o.Value.Table);
          if (selectable.IsFormula)
          {
            flag2 = true;
            strArray3[index3] = selectable.GetTemplate(factory.Dialect, factory.SQLFunctionRegistry);
          }
          else
            strArray1[index3] = selectable.GetTemplate(factory.Dialect, factory.SQLFunctionRegistry);
          ++index3;
        }
        this.propertyColumnNames[index2] = strArray1;
        this.propertyColumnFormulaTemplates[index2] = strArray3;
        this.propertyColumnAliases[index2] = strArray2;
        if (flag1 && o.IsLazy)
        {
          this.lazyProperties.Add(o.Name);
          stringList1.Add(o.Name);
          intList.Add(index2);
          typeList1.Add(o.Value.Type);
          strArrayList1.Add(strArray2);
        }
        this.propertyColumnUpdateable[index2] = o.Value.ColumnUpdateability;
        this.propertyColumnInsertable[index2] = o.Value.ColumnInsertability;
        this.propertySelectable[index2] = o.IsSelectable;
        this.propertyUniqueness[index2] = o.Value.IsAlternateUniqueKey;
        ++index2;
      }
      this.hasFormulaProperties = flag2;
      this.lazyPropertyColumnAliases = strArrayList1.ToArray();
      this.lazyPropertyNames = stringList1.ToArray();
      this.lazyPropertyNumbers = intList.ToArray();
      this.lazyPropertyTypes = typeList1.ToArray();
      List<string> stringList2 = new List<string>();
      List<bool> boolList1 = new List<bool>();
      List<string> stringList3 = new List<string>();
      List<string> stringList4 = new List<string>();
      List<string> stringList5 = new List<string>();
      List<string> stringList6 = new List<string>();
      List<bool> boolList2 = new List<bool>();
      List<IType> typeList2 = new List<IType>();
      List<string> stringList7 = new List<string>();
      List<string> stringList8 = new List<string>();
      List<string[]> strArrayList2 = new List<string[]>();
      List<string[]> strArrayList3 = new List<string[]>();
      List<FetchMode> fetchModeList = new List<FetchMode>();
      List<CascadeStyle> cascadeStyleList = new List<CascadeStyle>();
      List<bool> boolList3 = new List<bool>();
      List<int[]> numArrayList1 = new List<int[]>();
      List<int[]> numArrayList2 = new List<int[]>();
      List<bool> boolList4 = new List<bool>();
      List<bool> boolList5 = new List<bool>();
      foreach (Property o in persistentClass.SubclassPropertyClosureIterator)
      {
        stringList7.Add(o.Name);
        stringList8.Add(o.PersistentClass.EntityName);
        bool flag3 = !set.Contains((object) o);
        boolList3.Add(flag3);
        boolList5.Add(o.IsOptional || flag3);
        typeList2.Add(o.Type);
        string[] strArray4 = new string[o.ColumnSpan];
        string[] strArray5 = new string[o.ColumnSpan];
        int[] numArray1 = new int[o.ColumnSpan];
        int[] numArray2 = new int[o.ColumnSpan];
        int index4 = 0;
        bool flag4 = o.IsLazy && flag1;
        foreach (ISelectable selectable in o.ColumnIterator)
        {
          if (selectable.IsFormula)
          {
            string template = selectable.GetTemplate(factory.Dialect, factory.SQLFunctionRegistry);
            numArray2[index4] = stringList6.Count;
            numArray1[index4] = -1;
            stringList6.Add(template);
            strArray5[index4] = template;
            stringList4.Add(selectable.GetText(factory.Dialect));
            stringList5.Add(selectable.GetAlias(factory.Dialect));
            boolList2.Add(flag4);
          }
          else
          {
            string template = selectable.GetTemplate(factory.Dialect, factory.SQLFunctionRegistry);
            numArray1[index4] = stringList2.Count;
            numArray2[index4] = -1;
            stringList2.Add(template);
            strArray4[index4] = template;
            stringList3.Add(selectable.GetAlias(factory.Dialect, o.Value.Table));
            boolList1.Add(flag4);
            boolList4.Add(o.IsSelectable);
          }
          ++index4;
        }
        strArrayList3.Add(strArray4);
        strArrayList2.Add(strArray5);
        numArrayList1.Add(numArray1);
        numArrayList2.Add(numArray2);
        fetchModeList.Add(o.Value.FetchMode);
        cascadeStyleList.Add(o.CascadeStyle);
      }
      this.subclassColumnClosure = stringList2.ToArray();
      this.subclassColumnAliasClosure = stringList3.ToArray();
      this.subclassColumnLazyClosure = boolList1.ToArray();
      this.subclassColumnSelectableClosure = boolList4.ToArray();
      this.subclassFormulaClosure = stringList4.ToArray();
      this.subclassFormulaTemplateClosure = stringList6.ToArray();
      this.subclassFormulaAliasClosure = stringList5.ToArray();
      this.subclassFormulaLazyClosure = boolList2.ToArray();
      this.subclassPropertyNameClosure = stringList7.ToArray();
      this.subclassPropertySubclassNameClosure = stringList8.ToArray();
      this.subclassPropertyTypeClosure = typeList2.ToArray();
      this.subclassPropertyNullabilityClosure = boolList5.ToArray();
      this.subclassPropertyFormulaTemplateClosure = strArrayList2.ToArray();
      this.subclassPropertyColumnNameClosure = strArrayList3.ToArray();
      this.subclassPropertyColumnNumberClosure = numArrayList1.ToArray();
      this.subclassPropertyFormulaNumberClosure = numArrayList2.ToArray();
      this.subclassPropertyCascadeStyleClosure = cascadeStyleList.ToArray();
      this.subclassPropertyFetchModeClosure = fetchModeList.ToArray();
      this.propertyDefinedOnSubclass = boolList3.ToArray();
      this.filterHelper = new FilterHelper(persistentClass.FilterMap, factory.Dialect, factory.SQLFunctionRegistry);
      this.temporaryIdTableName = persistentClass.TemporaryIdTableName;
      this.temporaryIdTableDDL = persistentClass.TemporaryIdTableDDL;
    }

    protected abstract int[] SubclassColumnTableNumberClosure { get; }

    protected abstract int[] SubclassFormulaTableNumberClosure { get; }

    protected internal abstract int[] PropertyTableNumbersInSelect { get; }

    protected internal abstract int[] PropertyTableNumbers { get; }

    public virtual string DiscriminatorColumnName => "clazz_";

    protected virtual string DiscriminatorFormulaTemplate => (string) null;

    public string[] RootTableKeyColumnNames => this.rootTableKeyColumnNames;

    protected internal SqlCommandInfo[] SQLUpdateByRowIdStrings
    {
      get
      {
        if (this.sqlUpdateByRowIdString == null)
          throw new AssertionFailure("no update by row id");
        SqlCommandInfo[] destinationArray = new SqlCommandInfo[this.TableSpan + 1];
        destinationArray[0] = this.sqlUpdateByRowIdString;
        System.Array.Copy((System.Array) this.sqlUpdateStrings, 0, (System.Array) destinationArray, 1, this.TableSpan);
        return destinationArray;
      }
    }

    protected internal SqlCommandInfo[] SQLLazyUpdateByRowIdStrings
    {
      get
      {
        if (this.sqlLazyUpdateByRowIdString == null)
          throw new AssertionFailure("no update by row id");
        SqlCommandInfo[] updateByRowIdStrings = new SqlCommandInfo[this.TableSpan];
        updateByRowIdStrings[0] = this.sqlLazyUpdateByRowIdString;
        for (int index = 1; index < this.TableSpan; ++index)
          updateByRowIdStrings[index] = this.sqlLazyUpdateStrings[index];
        return updateByRowIdStrings;
      }
    }

    protected SqlString SQLSnapshotSelectString => this.sqlSnapshotSelectString;

    protected SqlString SQLLazySelectString => this.sqlLazySelectString;

    protected SqlCommandInfo[] SqlDeleteStrings => this.sqlDeleteStrings;

    protected SqlCommandInfo[] SqlInsertStrings => this.sqlInsertStrings;

    protected SqlCommandInfo[] SqlUpdateStrings => this.sqlUpdateStrings;

    protected internal SqlCommandInfo[] SQLLazyUpdateStrings => this.sqlLazyUpdateStrings;

    protected internal SqlCommandInfo SQLIdentityInsertString => this.sqlIdentityInsertString;

    protected SqlString VersionSelectString => this.sqlVersionSelectString;

    public bool IsBatchable
    {
      get
      {
        if (this.OptimisticLockMode == Versioning.OptimisticLock.None)
          return true;
        return !this.IsVersioned && this.OptimisticLockMode == Versioning.OptimisticLock.Version;
      }
    }

    public virtual string[] QuerySpaces => this.PropertySpaces;

    protected internal ISet<string> LazyProperties => this.lazyProperties;

    public bool IsBatchLoadable => this.batchSize > 1;

    public virtual string[] IdentifierColumnNames => this.rootTableKeyColumnNames;

    protected int IdentifierColumnSpan => this.identifierColumnSpan;

    public virtual string VersionColumnName => this.versionColumnName;

    protected internal string VersionedTableName => this.GetTableName(0);

    protected internal bool[] SubclassColumnLaziness => this.subclassColumnLazyClosure;

    protected internal bool[] SubclassFormulaLaziness => this.subclassFormulaLazyClosure;

    public bool IsCacheInvalidationRequired
    {
      get
      {
        if (this.HasFormulaProperties)
          return true;
        if (this.IsVersioned)
          return false;
        return this.entityMetamodel.IsDynamicUpdate || this.TableSpan > 1;
      }
    }

    public bool IsLazyPropertiesCacheable => this.isLazyPropertiesCacheable;

    public virtual string RootTableName => this.GetSubclassTableName(0);

    public virtual string[] RootTableIdentifierColumnNames => this.RootTableKeyColumnNames;

    protected internal string[] PropertySubclassNames => this.propertySubclassNames;

    protected string[][] SubclassPropertyFormulaTemplateClosure
    {
      get => this.subclassPropertyFormulaTemplateClosure;
    }

    protected IType[] SubclassPropertyTypeClosure => this.subclassPropertyTypeClosure;

    protected string[][] SubclassPropertyColumnNameClosure
    {
      get => this.subclassPropertyColumnNameClosure;
    }

    protected string[] SubclassPropertyNameClosure => this.subclassPropertyNameClosure;

    protected string[] SubclassPropertySubclassNameClosure
    {
      get => this.subclassPropertySubclassNameClosure;
    }

    protected string[] SubclassColumnClosure => this.subclassColumnClosure;

    protected string[] SubclassColumnAliasClosure => this.subclassColumnAliasClosure;

    protected string[] SubclassFormulaClosure => this.subclassFormulaClosure;

    protected string[] SubclassFormulaTemplateClosure => this.subclassFormulaTemplateClosure;

    protected string[] SubclassFormulaAliasClosure => this.subclassFormulaAliasClosure;

    public string IdentitySelectString
    {
      get
      {
        if (this.identitySelectString == null)
          this.identitySelectString = this.Factory.Dialect.GetIdentitySelectString(this.GetTableName(0), this.GetKeyColumns(0)[0], this.IdentifierType.SqlTypes((IMapping) this.Factory)[0].DbType);
        return this.identitySelectString;
      }
    }

    private string RootAlias => StringHelper.GenerateAlias(this.EntityName);

    public ISessionFactoryImplementor Factory => this.factory;

    public EntityMetamodel EntityMetamodel => this.entityMetamodel;

    public ICacheConcurrencyStrategy Cache => this.cache;

    public ICacheEntryStructure CacheEntryStructure => this.cacheEntryStructure;

    public IComparer VersionComparator
    {
      get => !this.IsVersioned ? (IComparer) null : this.VersionType.Comparator;
    }

    public string EntityName => this.entityMetamodel.Name;

    public EntityType EntityType => this.entityMetamodel.EntityType;

    public virtual bool IsPolymorphic => this.entityMetamodel.IsPolymorphic;

    public virtual bool IsInherited => this.entityMetamodel.IsInherited;

    public virtual IVersionType VersionType => this.LocateVersionType();

    public virtual int VersionProperty => this.entityMetamodel.VersionPropertyIndex;

    public virtual bool IsVersioned => this.entityMetamodel.IsVersioned;

    public virtual bool IsIdentifierAssignedByInsert
    {
      get => this.entityMetamodel.IdentifierProperty.IsIdentifierAssignedByInsert;
    }

    public virtual bool IsMutable => this.entityMetamodel.IsMutable;

    public virtual bool IsAbstract => this.entityMetamodel.IsAbstract;

    public virtual IIdentifierGenerator IdentifierGenerator
    {
      get => this.entityMetamodel.IdentifierProperty.IdentifierGenerator;
    }

    public virtual string RootEntityName => this.entityMetamodel.RootName;

    public virtual IClassMetadata ClassMetadata => (IClassMetadata) this;

    public virtual string MappedSuperclass => this.entityMetamodel.Superclass;

    public virtual bool IsExplicitPolymorphism => this.entityMetamodel.IsExplicitPolymorphism;

    public string[] KeyColumnNames => this.IdentifierColumnNames;

    public string Name => this.EntityName;

    public bool IsCollection => false;

    public IType Type => (IType) this.entityMetamodel.EntityType;

    public bool IsSelectBeforeUpdateRequired => this.entityMetamodel.IsSelectBeforeUpdate;

    public bool IsVersionPropertyGenerated
    {
      get
      {
        return this.IsVersioned && this.PropertyUpdateGenerationInclusions[this.VersionProperty] != ValueInclusion.None;
      }
    }

    public bool VersionPropertyInsertable
    {
      get => this.IsVersioned && this.PropertyInsertability[this.VersionProperty];
    }

    public virtual string[] PropertyNames => this.entityMetamodel.PropertyNames;

    public virtual IType[] PropertyTypes => this.entityMetamodel.PropertyTypes;

    public bool[] PropertyLaziness => this.entityMetamodel.PropertyLaziness;

    public virtual bool[] PropertyCheckability => this.entityMetamodel.PropertyCheckability;

    public bool[] NonLazyPropertyUpdateability => this.entityMetamodel.NonlazyPropertyUpdateability;

    public virtual bool[] PropertyInsertability => this.entityMetamodel.PropertyInsertability;

    public ValueInclusion[] PropertyInsertGenerationInclusions
    {
      get => this.entityMetamodel.PropertyInsertGenerationInclusions;
    }

    public ValueInclusion[] PropertyUpdateGenerationInclusions
    {
      get => this.entityMetamodel.PropertyUpdateGenerationInclusions;
    }

    public virtual bool[] PropertyNullability => this.entityMetamodel.PropertyNullability;

    public virtual bool[] PropertyVersionability => this.entityMetamodel.PropertyVersionability;

    public virtual CascadeStyle[] PropertyCascadeStyles => this.entityMetamodel.CascadeStyles;

    public virtual bool IsMultiTable => false;

    public string TemporaryIdTableName => this.temporaryIdTableName;

    public string TemporaryIdTableDDL => this.temporaryIdTableDDL;

    protected int PropertySpan => this.entityMetamodel.PropertySpan;

    public virtual string IdentifierPropertyName => this.entityMetamodel.IdentifierProperty.Name;

    public virtual IType IdentifierType => this.entityMetamodel.IdentifierProperty.Type;

    public int[] NaturalIdentifierProperties => this.entityMetamodel.NaturalIdentifierProperties;

    public abstract string[][] ContraintOrderedTableKeyColumnClosure { get; }

    public abstract IType DiscriminatorType { get; }

    public abstract string[] ConstraintOrderedTableNameClosure { get; }

    public abstract string DiscriminatorSQLValue { get; }

    public abstract object DiscriminatorValue { get; }

    public abstract string[] PropertySpaces { get; }

    protected virtual void AddDiscriminatorToInsert(SqlInsertBuilder insert)
    {
    }

    protected virtual void AddDiscriminatorToSelect(
      NHibernate.SqlCommand.SelectFragment select,
      string name,
      string suffix)
    {
    }

    public abstract string GetSubclassTableName(int j);

    protected abstract string[] GetSubclassTableKeyColumns(int j);

    protected abstract bool IsClassOrSuperclassTable(int j);

    protected abstract int SubclassTableSpan { get; }

    protected abstract int TableSpan { get; }

    protected abstract bool IsTableCascadeDeleteEnabled(int j);

    protected abstract string GetTableName(int table);

    protected abstract string[] GetKeyColumns(int table);

    protected abstract bool IsPropertyOfTable(int property, int table);

    protected abstract int GetSubclassPropertyTableNumber(int i);

    public abstract string FilterFragment(string alias);

    protected internal virtual string DiscriminatorAlias => "clazz_";

    protected virtual bool IsInverseTable(int j) => false;

    protected virtual bool IsNullableTable(int j) => false;

    protected virtual bool IsNullableSubclassTable(int j) => false;

    protected virtual bool IsInverseSubclassTable(int j) => false;

    public virtual bool IsSubclassEntityName(string entityName)
    {
      return this.entityMetamodel.SubclassEntityNames.Contains(entityName);
    }

    protected bool[] TableHasColumns => this.tableHasColumns;

    protected bool IsInsertCallable(int j) => this.insertCallable[j];

    protected bool IsUpdateCallable(int j) => this.updateCallable[j];

    protected bool IsDeleteCallable(int j) => this.deleteCallable[j];

    protected virtual bool IsSubclassPropertyDeferred(string propertyName, string entityName)
    {
      return false;
    }

    protected virtual bool IsSubclassTableSequentialSelect(int table) => false;

    public virtual bool HasSequentialSelect => false;

    private bool[] GetTableUpdateNeeded(int[] dirtyProperties, bool hasDirtyCollection)
    {
      if (dirtyProperties == null)
        return this.TableHasColumns;
      bool[] propertyUpdateability = this.PropertyUpdateability;
      int[] propertyTableNumbers = this.PropertyTableNumbers;
      bool[] tableUpdateNeeded = new bool[this.TableSpan];
      for (int index1 = 0; index1 < dirtyProperties.Length; ++index1)
      {
        int dirtyProperty = dirtyProperties[index1];
        int index2 = propertyTableNumbers[dirtyProperty];
        tableUpdateNeeded[index2] = tableUpdateNeeded[index2] || this.GetPropertyColumnSpan(dirtyProperty) > 0 && propertyUpdateability[dirtyProperty];
      }
      if (this.IsVersioned && !this.entityMetamodel.VersionProperty.IsUpdateGenerated)
        tableUpdateNeeded[0] = tableUpdateNeeded[0] || Versioning.IsVersionIncrementRequired(dirtyProperties, hasDirtyCollection, this.PropertyVersionability);
      return tableUpdateNeeded;
    }

    public virtual bool HasRowId => this.rowIdName != null;

    protected internal virtual SqlString GenerateLazySelectString()
    {
      if (!this.entityMetamodel.HasLazyProperties)
        return (SqlString) null;
      HashedSet<int> coll = new HashedSet<int>();
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      for (int index1 = 0; index1 < this.lazyPropertyNames.Length; ++index1)
      {
        int subclassPropertyIndex = this.GetSubclassPropertyIndex(this.lazyPropertyNames[index1]);
        int propertyTableNumber = this.GetSubclassPropertyTableNumber(subclassPropertyIndex);
        coll.Add(propertyTableNumber);
        int[] numArray1 = this.subclassPropertyColumnNumberClosure[subclassPropertyIndex];
        for (int index2 = 0; index2 < numArray1.Length; ++index2)
        {
          if (numArray1[index2] != -1)
            intList1.Add(numArray1[index2]);
        }
        int[] numArray2 = this.subclassPropertyFormulaNumberClosure[subclassPropertyIndex];
        for (int index3 = 0; index3 < numArray2.Length; ++index3)
        {
          if (numArray2[index3] != -1)
            intList2.Add(numArray2[index3]);
        }
      }
      return intList1.Count == 0 && intList2.Count == 0 ? (SqlString) null : this.RenderSelect(ArrayHelper.ToIntArray((ICollection) coll), intList1.ToArray(), intList2.ToArray());
    }

    public virtual object InitializeLazyProperty(
      string fieldName,
      object entity,
      ISessionImplementor session)
    {
      object entityIdentifier = session.GetContextEntityIdentifier(entity);
      EntityEntry entry = session.PersistenceContext.GetEntry(entity);
      if (entry == null)
        throw new HibernateException("entity is not associated with the session: " + entityIdentifier);
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) string.Format("initializing lazy properties of: {0}, field access: {1}", (object) MessageHelper.InfoString((IEntityPersister) this, entityIdentifier, this.Factory), (object) fieldName));
      if (this.HasCache)
      {
        object map = this.Cache.Get(new CacheKey(entityIdentifier, this.IdentifierType, this.EntityName, session.EntityMode, this.Factory), session.Timestamp);
        if (map != null)
        {
          CacheEntry cacheEntry = (CacheEntry) this.CacheEntryStructure.Destructure(map, this.factory);
          if (!cacheEntry.AreLazyPropertiesUnfetched)
            return this.InitializeLazyPropertiesFromCache(fieldName, entity, session, entry, cacheEntry);
        }
      }
      return this.InitializeLazyPropertiesFromDatastore(fieldName, entity, session, entityIdentifier, entry);
    }

    private object InitializeLazyPropertiesFromDatastore(
      string fieldName,
      object entity,
      ISessionImplementor session,
      object id,
      EntityEntry entry)
    {
      if (!this.HasLazyProperties)
        throw new AssertionFailure("no lazy properties");
      AbstractEntityPersister.log.Debug((object) "initializing lazy properties from datastore");
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          object obj = (object) null;
          IDbCommand dbCommand = (IDbCommand) null;
          IDataReader dataReader = (IDataReader) null;
          try
          {
            SqlString lazySelectString = this.SQLLazySelectString;
            if (lazySelectString != null)
            {
              dbCommand = session.Batcher.PrepareCommand(CommandType.Text, lazySelectString, this.IdentifierType.SqlTypes((IMapping) this.Factory));
              this.IdentifierType.NullSafeSet(dbCommand, id, 0, session);
              dataReader = session.Batcher.ExecuteReader(dbCommand);
              dataReader.Read();
            }
            object[] loadedState = entry.LoadedState;
            for (int j = 0; j < this.lazyPropertyNames.Length; ++j)
            {
              object propValue = this.lazyPropertyTypes[j].NullSafeGet(dataReader, this.lazyPropertyColumnAliases[j], session, entity);
              if (this.InitializeLazyProperty(fieldName, entity, session, loadedState, j, propValue))
                obj = propValue;
            }
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, dataReader);
          }
          AbstractEntityPersister.log.Debug((object) "done initializing lazy properties");
          return obj;
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
          {
            SqlException = (Exception) ex,
            Message = "could not initialize lazy properties: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory),
            Sql = this.SQLLazySelectString.ToString(),
            EntityName = this.EntityName,
            EntityId = id
          });
        }
      }
    }

    private object InitializeLazyPropertiesFromCache(
      string fieldName,
      object entity,
      ISessionImplementor session,
      EntityEntry entry,
      CacheEntry cacheEntry)
    {
      AbstractEntityPersister.log.Debug((object) "initializing lazy properties from second-level cache");
      object obj = (object) null;
      object[] disassembledState = cacheEntry.DisassembledState;
      object[] loadedState = entry.LoadedState;
      for (int j = 0; j < this.lazyPropertyNames.Length; ++j)
      {
        object propValue = this.lazyPropertyTypes[j].Assemble(disassembledState[this.lazyPropertyNumbers[j]], session, entity);
        if (this.InitializeLazyProperty(fieldName, entity, session, loadedState, j, propValue))
          obj = propValue;
      }
      AbstractEntityPersister.log.Debug((object) "done initializing lazy properties");
      return obj;
    }

    private bool InitializeLazyProperty(
      string fieldName,
      object entity,
      ISessionImplementor session,
      object[] snapshot,
      int j,
      object propValue)
    {
      this.SetPropertyValue(entity, this.lazyPropertyNumbers[j], propValue, session.EntityMode);
      if (snapshot != null)
        snapshot[this.lazyPropertyNumbers[j]] = this.lazyPropertyTypes[j].DeepCopy(propValue, session.EntityMode, this.factory);
      return fieldName.Equals(this.lazyPropertyNames[j]);
    }

    public string[] IdentifierAliases => this.identifierAliases;

    public string SelectFragment(string alias, string suffix)
    {
      return this.IdentifierSelectFragment(alias, suffix) + this.PropertySelectFragment(alias, suffix, false);
    }

    public string[] GetIdentifierAliases(string suffix)
    {
      return new Alias(suffix).ToAliasStrings(this.IdentifierAliases, this.factory.Dialect);
    }

    public string[] GetPropertyAliases(string suffix, int i)
    {
      return new Alias(suffix).ToUnquotedAliasStrings(this.propertyColumnAliases[i], this.factory.Dialect);
    }

    public string GetDiscriminatorAlias(string suffix)
    {
      return !this.entityMetamodel.HasSubclasses ? (string) null : new Alias(suffix).ToAliasString(this.DiscriminatorAlias, this.factory.Dialect);
    }

    public virtual string IdentifierSelectFragment(string name, string suffix)
    {
      return new NHibernate.SqlCommand.SelectFragment(this.factory.Dialect).SetSuffix(suffix).AddColumns(name, this.IdentifierColumnNames, this.IdentifierAliases).ToSqlStringFragment(false);
    }

    public string PropertySelectFragment(string name, string suffix, bool allProperties)
    {
      NHibernate.SqlCommand.SelectFragment select = new NHibernate.SqlCommand.SelectFragment(this.Factory.Dialect).SetSuffix(suffix).SetUsedAliases(this.IdentifierAliases);
      int[] tableNumberClosure1 = this.SubclassColumnTableNumberClosure;
      string[] columnAliasClosure = this.SubclassColumnAliasClosure;
      string[] subclassColumnClosure = this.SubclassColumnClosure;
      for (int index = 0; index < subclassColumnClosure.Length; ++index)
      {
        if ((allProperties || !this.subclassColumnLazyClosure[index]) && !this.IsSubclassTableSequentialSelect(tableNumberClosure1[index]) && this.subclassColumnSelectableClosure[index])
        {
          string tableAlias = this.GenerateTableAlias(name, tableNumberClosure1[index]);
          select.AddColumn(tableAlias, subclassColumnClosure[index], columnAliasClosure[index]);
        }
      }
      int[] tableNumberClosure2 = this.SubclassFormulaTableNumberClosure;
      string[] formulaTemplateClosure = this.SubclassFormulaTemplateClosure;
      string[] formulaAliasClosure = this.SubclassFormulaAliasClosure;
      for (int index = 0; index < formulaTemplateClosure.Length; ++index)
      {
        if ((allProperties || !this.subclassFormulaLazyClosure[index]) && !this.IsSubclassTableSequentialSelect(tableNumberClosure2[index]))
        {
          string tableAlias = this.GenerateTableAlias(name, tableNumberClosure2[index]);
          select.AddFormula(tableAlias, formulaTemplateClosure[index], formulaAliasClosure[index]);
        }
      }
      if (this.entityMetamodel.HasSubclasses)
        this.AddDiscriminatorToSelect(select, name, suffix);
      if (this.HasRowId)
        select.AddColumn(name, this.rowIdName, Loadable.RowIdAlias);
      return select.ToSqlStringFragment();
    }

    public object[] GetDatabaseSnapshot(object id, ISessionImplementor session)
    {
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) ("Getting current persistent state for: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, this.SQLSnapshotSelectString, this.IdentifierType.SqlTypes((IMapping) this.factory));
          IDataReader dataReader = (IDataReader) null;
          try
          {
            this.IdentifierType.NullSafeSet(dbCommand, id, 0, session);
            dataReader = session.Batcher.ExecuteReader(dbCommand);
            if (!dataReader.Read())
              return (object[]) null;
            IType[] propertyTypes = this.PropertyTypes;
            object[] databaseSnapshot = new object[propertyTypes.Length];
            bool[] propertyUpdateability = this.PropertyUpdateability;
            for (int i = 0; i < propertyTypes.Length; ++i)
            {
              if (propertyUpdateability[i])
                databaseSnapshot[i] = propertyTypes[i].Hydrate(dataReader, this.GetPropertyAliases(string.Empty, i), session, (object) null);
            }
            return databaseSnapshot;
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, dataReader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
          {
            SqlException = (Exception) ex,
            Message = "could not retrieve snapshot: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory),
            Sql = this.SQLSnapshotSelectString.ToString(),
            EntityName = this.EntityName,
            EntityId = id
          });
        }
      }
    }

    protected SqlString GenerateSelectVersionString()
    {
      SqlSimpleSelectBuilder simpleSelectBuilder = new SqlSimpleSelectBuilder(this.Factory.Dialect, (IMapping) this.factory).SetTableName(this.VersionedTableName);
      if (this.IsVersioned)
        simpleSelectBuilder.AddColumn(this.versionColumnName);
      else
        simpleSelectBuilder.AddColumns(this.rootTableKeyColumnNames);
      if (this.Factory.Settings.IsCommentsEnabled)
        simpleSelectBuilder.SetComment("get version " + this.EntityName);
      return simpleSelectBuilder.AddWhereFragment(this.rootTableKeyColumnNames, this.IdentifierType, " = ").ToSqlString();
    }

    protected SqlString GenerateInsertGeneratedValuesSelectString()
    {
      return this.GenerateGeneratedValuesSelectString(this.PropertyInsertGenerationInclusions);
    }

    protected SqlString GenerateUpdateGeneratedValuesSelectString()
    {
      return this.GenerateGeneratedValuesSelectString(this.PropertyUpdateGenerationInclusions);
    }

    private SqlString GenerateGeneratedValuesSelectString(ValueInclusion[] inclusions)
    {
      SqlSelectBuilder sqlSelectBuilder = new SqlSelectBuilder(this.Factory);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlSelectBuilder.SetComment("get generated state " + this.EntityName);
      string[] objects = StringHelper.Qualify(this.RootAlias, this.IdentifierColumnNames);
      string selectClause = this.ConcretePropertySelectFragment(this.RootAlias, inclusions).Substring(2);
      string fromClause = this.FromTableFragment(this.RootAlias) + (object) this.FromJoinFragment(this.RootAlias, true, false);
      SqlString sqlString = new SqlStringBuilder().Add(StringHelper.Join(new SqlString(new object[3]
      {
        (object) "=",
        (object) Parameter.Placeholder,
        (object) " and "
      }), (IEnumerable) objects)).Add("=").AddParameter().Add(this.WhereJoinFragment(this.RootAlias, true, false)).ToSqlString();
      return sqlSelectBuilder.SetSelectClause(selectClause).SetFromClause(fromClause).SetOuterJoins(SqlString.Empty, SqlString.Empty).SetWhereClause(sqlString).ToSqlString();
    }

    protected string ConcretePropertySelectFragment(string alias, ValueInclusion[] inclusions)
    {
      return this.ConcretePropertySelectFragment(alias, (AbstractEntityPersister.IInclusionChecker) new AbstractEntityPersister.NoneInclusionChecker(inclusions));
    }

    protected string ConcretePropertySelectFragment(string alias, bool[] includeProperty)
    {
      return this.ConcretePropertySelectFragment(alias, (AbstractEntityPersister.IInclusionChecker) new AbstractEntityPersister.FullInclusionChecker(includeProperty));
    }

    protected string ConcretePropertySelectFragment(
      string alias,
      AbstractEntityPersister.IInclusionChecker inclusionChecker)
    {
      int length = this.PropertyNames.Length;
      int[] tableNumbersInSelect = this.PropertyTableNumbersInSelect;
      NHibernate.SqlCommand.SelectFragment selectFragment = new NHibernate.SqlCommand.SelectFragment(this.Factory.Dialect);
      for (int propertyNumber = 0; propertyNumber < length; ++propertyNumber)
      {
        if (inclusionChecker.IncludeProperty(propertyNumber))
        {
          selectFragment.AddColumns(this.GenerateTableAlias(alias, tableNumbersInSelect[propertyNumber]), this.propertyColumnNames[propertyNumber], this.propertyColumnAliases[propertyNumber]);
          selectFragment.AddFormulas(this.GenerateTableAlias(alias, tableNumbersInSelect[propertyNumber]), this.propertyColumnFormulaTemplates[propertyNumber], this.propertyColumnAliases[propertyNumber]);
        }
      }
      return selectFragment.ToFragmentString();
    }

    protected virtual SqlString GenerateSnapshotSelectString()
    {
      SqlSelectBuilder sqlSelectBuilder = new SqlSelectBuilder(this.Factory);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlSelectBuilder.SetComment("get current state " + this.EntityName);
      string[] objects = StringHelper.Qualify(this.RootAlias, this.IdentifierColumnNames);
      string selectClause = StringHelper.Join(", ", (IEnumerable) objects) + this.ConcretePropertySelectFragment(this.RootAlias, this.PropertyUpdateability);
      SqlString fromClause = new SqlString(this.FromTableFragment(this.RootAlias)) + this.FromJoinFragment(this.RootAlias, true, false);
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder().Add(StringHelper.Join(new SqlString(new object[3]
      {
        (object) "=",
        (object) Parameter.Placeholder,
        (object) " and "
      }), (IEnumerable) objects)).Add("=").AddParameter().Add(this.WhereJoinFragment(this.RootAlias, true, false));
      return sqlSelectBuilder.SetSelectClause(selectClause).SetFromClause(fromClause).SetOuterJoins(SqlString.Empty, SqlString.Empty).SetWhereClause(sqlStringBuilder.ToSqlString()).ToSqlString();
    }

    public object ForceVersionIncrement(
      object id,
      object currentVersion,
      ISessionImplementor session)
    {
      if (!this.IsVersioned)
        throw new AssertionFailure("cannot force version increment on non-versioned entity");
      if (this.IsVersionPropertyGenerated)
        throw new HibernateException("LockMode.Force is currently not supported for generated version properties");
      object obj = this.VersionType.Next(currentVersion, session);
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) ("Forcing version increment [" + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory) + "; " + this.VersionType.ToLoggableString(currentVersion, this.Factory) + " -> " + this.VersionType.ToLoggableString(obj, this.Factory) + "]"));
      IExpectation expectation = Expectations.AppropriateExpectation(this.updateResultCheckStyles[0]);
      SqlCommandInfo incrementUpdateString = this.GenerateVersionIncrementUpdateString();
      try
      {
        IDbCommand dbCommand = session.Batcher.PrepareCommand(incrementUpdateString.CommandType, incrementUpdateString.Text, incrementUpdateString.ParameterTypes);
        try
        {
          this.VersionType.NullSafeSet(dbCommand, obj, 0, session);
          this.IdentifierType.NullSafeSet(dbCommand, id, 1, session);
          this.VersionType.NullSafeSet(dbCommand, currentVersion, 1 + this.IdentifierColumnSpan, session);
          this.Check(session.Batcher.ExecuteNonQuery(dbCommand), id, 0, expectation, dbCommand);
        }
        finally
        {
          session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
        {
          SqlException = (Exception) ex,
          Message = "could not retrieve version: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory),
          Sql = this.VersionSelectString.ToString(),
          EntityName = this.EntityName,
          EntityId = id
        });
      }
      return obj;
    }

    private SqlCommandInfo GenerateVersionIncrementUpdateString()
    {
      SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(this.Factory.Dialect, (IMapping) this.Factory);
      sqlUpdateBuilder.SetTableName(this.GetTableName(0));
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlUpdateBuilder.SetComment("forced version increment");
      sqlUpdateBuilder.AddColumn(this.VersionColumnName, (IType) this.VersionType);
      sqlUpdateBuilder.SetIdentityColumn(this.IdentifierColumnNames, this.IdentifierType);
      sqlUpdateBuilder.SetVersionColumn(new string[1]
      {
        this.VersionColumnName
      }, this.VersionType);
      return sqlUpdateBuilder.ToSqlCommandInfo();
    }

    public object GetCurrentVersion(object id, ISessionImplementor session)
    {
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) ("Getting version: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          IDbCommand dbCommand = session.Batcher.PrepareQueryCommand(CommandType.Text, this.VersionSelectString, this.IdentifierType.SqlTypes((IMapping) this.Factory));
          IDataReader dataReader = (IDataReader) null;
          try
          {
            this.IdentifierType.NullSafeSet(dbCommand, id, 0, session);
            dataReader = session.Batcher.ExecuteReader(dbCommand);
            if (!dataReader.Read())
              return (object) null;
            return !this.IsVersioned ? (object) this : this.VersionType.NullSafeGet(dataReader, this.VersionColumnName, session, (object) null);
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, dataReader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
          {
            SqlException = (Exception) ex,
            Message = "could not retrieve version: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory),
            Sql = this.VersionSelectString.ToString(),
            EntityName = this.EntityName,
            EntityId = id
          });
        }
      }
    }

    protected internal virtual void InitLockers()
    {
      this.lockers[LockMode.Read] = this.GenerateLocker(LockMode.Read);
      this.lockers[LockMode.Upgrade] = this.GenerateLocker(LockMode.Upgrade);
      this.lockers[LockMode.UpgradeNoWait] = this.GenerateLocker(LockMode.UpgradeNoWait);
      this.lockers[LockMode.Force] = this.GenerateLocker(LockMode.Force);
    }

    protected internal virtual ILockingStrategy GenerateLocker(LockMode lockMode)
    {
      return this.factory.Dialect.GetLockingStrategy((ILockable) this, lockMode);
    }

    private ILockingStrategy GetLocker(LockMode lockMode)
    {
      try
      {
        return this.lockers[lockMode];
      }
      catch (KeyNotFoundException ex)
      {
        throw new HibernateException(string.Format("LockMode {0} not supported by {1}", (object) lockMode, (object) this.GetType().FullName));
      }
    }

    public virtual void Lock(
      object id,
      object version,
      object obj,
      LockMode lockMode,
      ISessionImplementor session)
    {
      this.GetLocker(lockMode).Lock(id, version, obj, session);
    }

    public virtual string GetRootTableAlias(string drivingAlias) => drivingAlias;

    public virtual string[] ToColumns(string alias, string propertyName)
    {
      return this.propertyMapping.ToColumns(alias, propertyName);
    }

    public string[] ToColumns(string propertyName)
    {
      return this.propertyMapping.GetColumnNames(propertyName);
    }

    public IType ToType(string propertyName) => this.propertyMapping.ToType(propertyName);

    public bool TryToType(string propertyName, out IType type)
    {
      return this.propertyMapping.TryToType(propertyName, out type);
    }

    public string[] GetPropertyColumnNames(string propertyName)
    {
      return this.propertyMapping.GetColumnNames(propertyName);
    }

    public virtual int GetSubclassPropertyTableNumber(string propertyPath)
    {
      string propertyName = StringHelper.Root(propertyPath);
      IType type = this.propertyMapping.ToType(propertyName);
      if (type.IsAssociationType)
      {
        IAssociationType associationType = (IAssociationType) type;
        if (associationType.UseLHSPrimaryKey)
          return 0;
        if (type.IsCollectionType)
          propertyName = associationType.LHSPropertyName;
      }
      int i = System.Array.IndexOf<string>(this.SubclassPropertyNameClosure, propertyName);
      return i != -1 ? this.GetSubclassPropertyTableNumber(i) : 0;
    }

    public virtual Declarer GetSubclassPropertyDeclarer(string propertyPath)
    {
      int propertyTableNumber = this.GetSubclassPropertyTableNumber(propertyPath);
      if (propertyTableNumber == 0)
        return Declarer.Class;
      return this.IsClassOrSuperclassTable(propertyTableNumber) ? Declarer.SuperClass : Declarer.SubClass;
    }

    public virtual string GenerateTableAliasForColumn(string rootAlias, string column)
    {
      int index = System.Array.IndexOf<string>(this.SubclassColumnClosure, column);
      return index < 0 || System.Array.IndexOf<string>(this.KeyColumnNames, column) >= 0 ? rootAlias : this.GenerateTableAlias(rootAlias, this.SubclassColumnTableNumberClosure[index]);
    }

    public string GenerateTableAlias(string rootAlias, int tableNumber)
    {
      if (tableNumber == 0)
        return rootAlias;
      StringBuilder stringBuilder = new StringBuilder().Append(rootAlias);
      if (!rootAlias.EndsWith("_"))
        stringBuilder.Append('_');
      return stringBuilder.Append(tableNumber).Append('_').ToString();
    }

    public string[] ToColumns(string name, int i)
    {
      string tableAlias = this.GenerateTableAlias(name, this.GetSubclassPropertyTableNumber(i));
      string[] propertyColumnNames = this.GetSubclassPropertyColumnNames(i);
      string[] strArray = this.SubclassPropertyFormulaTemplateClosure[i];
      string[] columns = new string[propertyColumnNames.Length];
      for (int index = 0; index < propertyColumnNames.Length; ++index)
        columns[index] = propertyColumnNames[index] != null ? StringHelper.Qualify(tableAlias, propertyColumnNames[index]) : StringHelper.Replace(strArray[index], Template.Placeholder, tableAlias);
      return columns;
    }

    public string[] ToIdentifierColumns(string name)
    {
      string tableAlias = this.GenerateTableAlias(name, 0);
      string[] identifierColumnNames = this.IdentifierColumnNames;
      string[] identifierColumns = new string[identifierColumnNames.Length];
      for (int index = 0; index < identifierColumnNames.Length; ++index)
        identifierColumns[index] = StringHelper.Qualify(tableAlias, identifierColumnNames[index]);
      return identifierColumns;
    }

    private int GetSubclassPropertyIndex(string propertyName)
    {
      return System.Array.IndexOf<string>(this.subclassPropertyNameClosure, propertyName);
    }

    public string[] GetPropertyColumnNames(int i) => this.propertyColumnNames[i];

    protected int GetPropertyColumnSpan(int i) => this.propertyColumnSpans[i];

    protected bool HasFormulaProperties => this.hasFormulaProperties;

    public FetchMode GetFetchMode(int i) => this.subclassPropertyFetchModeClosure[i];

    public CascadeStyle GetCascadeStyle(int i) => this.subclassPropertyCascadeStyleClosure[i];

    public IType GetSubclassPropertyType(int i) => this.subclassPropertyTypeClosure[i];

    public string GetSubclassPropertyName(int i) => this.subclassPropertyNameClosure[i];

    public int CountSubclassProperties() => this.subclassPropertyTypeClosure.Length;

    public string[] GetSubclassPropertyColumnNames(int i)
    {
      return this.subclassPropertyColumnNameClosure[i];
    }

    public bool IsDefinedOnSubclass(int i) => this.propertyDefinedOnSubclass[i];

    public string[] GetSubclassPropertyColumnAliases(string propertyName, string suffix)
    {
      string[] strArray;
      if (!this.subclassPropertyAliases.TryGetValue(propertyName, out strArray))
        return (string[]) null;
      string[] propertyColumnAliases = new string[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        propertyColumnAliases[index] = new Alias(suffix).ToUnquotedAliasString(strArray[index], this.Factory.Dialect);
      return propertyColumnAliases;
    }

    public string[] GetSubclassPropertyColumnNames(string propertyName)
    {
      string[] propertyColumnNames;
      this.subclassPropertyColumnNames.TryGetValue(propertyName, out propertyColumnNames);
      return propertyColumnNames;
    }

    protected void InitSubclassPropertyAliasesMap(PersistentClass model)
    {
      this.InternalInitSubclassPropertyAliasesMap((string) null, model.SubclassPropertyClosureIterator);
      if (!this.entityMetamodel.HasNonIdentifierPropertyNamedId)
      {
        this.subclassPropertyAliases[EntityPersister.EntityID] = this.identifierAliases;
        this.subclassPropertyColumnNames[EntityPersister.EntityID] = this.IdentifierColumnNames;
      }
      if (this.HasIdentifierProperty)
      {
        this.subclassPropertyAliases[this.IdentifierPropertyName] = this.IdentifierAliases;
        this.subclassPropertyColumnNames[this.IdentifierPropertyName] = this.IdentifierColumnNames;
      }
      if (this.IdentifierType.IsComponentType)
      {
        string[] propertyNames = ((IAbstractComponentType) this.IdentifierType).PropertyNames;
        string[] identifierAliases = this.IdentifierAliases;
        string[] identifierColumnNames = this.IdentifierColumnNames;
        for (int index = 0; index < propertyNames.Length; ++index)
        {
          if (this.entityMetamodel.HasNonIdentifierPropertyNamedId)
          {
            this.subclassPropertyAliases[EntityPersister.EntityID + "." + propertyNames[index]] = new string[1]
            {
              identifierAliases[index]
            };
            this.subclassPropertyColumnNames[EntityPersister.EntityID + "." + this.IdentifierPropertyName + "." + propertyNames[index]] = new string[1]
            {
              identifierColumnNames[index]
            };
          }
          if (this.HasIdentifierProperty)
          {
            this.subclassPropertyAliases[this.IdentifierPropertyName + "." + propertyNames[index]] = new string[1]
            {
              identifierAliases[index]
            };
            this.subclassPropertyColumnNames[this.IdentifierPropertyName + "." + propertyNames[index]] = new string[1]
            {
              identifierColumnNames[index]
            };
          }
          else
          {
            this.subclassPropertyAliases[propertyNames[index]] = new string[1]
            {
              identifierAliases[index]
            };
            this.subclassPropertyColumnNames[propertyNames[index]] = new string[1]
            {
              identifierColumnNames[index]
            };
          }
        }
      }
      if (!this.entityMetamodel.IsPolymorphic)
        return;
      this.subclassPropertyAliases["class"] = new string[1]
      {
        this.DiscriminatorAlias
      };
      this.subclassPropertyColumnNames["class"] = new string[1]
      {
        this.DiscriminatorColumnName
      };
    }

    private void InternalInitSubclassPropertyAliasesMap(string path, IEnumerable<Property> col)
    {
      foreach (Property property in col)
      {
        string str = path == null ? property.Name : path + "." + property.Name;
        if (property.IsComposite)
        {
          Component component = (Component) property.Value;
          this.InternalInitSubclassPropertyAliasesMap(str, component.PropertyIterator);
        }
        else
        {
          string[] strArray1 = new string[property.ColumnSpan];
          string[] strArray2 = new string[property.ColumnSpan];
          int index = 0;
          foreach (ISelectable selectable in property.ColumnIterator)
          {
            strArray1[index] = selectable.GetAlias(this.Factory.Dialect, property.Value.Table);
            strArray2[index] = selectable.GetText(this.Factory.Dialect);
            ++index;
          }
          this.subclassPropertyAliases[str] = strArray1;
          this.subclassPropertyColumnNames[str] = strArray2;
        }
      }
    }

    public object LoadByUniqueKey(
      string propertyName,
      object uniqueKey,
      ISessionImplementor session)
    {
      return this.GetAppropriateUniqueKeyLoader(propertyName, session.EnabledFilters).LoadByUniqueKey(session, uniqueKey);
    }

    private EntityLoader GetAppropriateUniqueKeyLoader(
      string propertyName,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      return (enabledFilters == null || enabledFilters.Count == 0) && propertyName.IndexOf('.') < 0 ? this.uniqueKeyLoaders[propertyName] : this.CreateUniqueKeyLoader(this.propertyMapping.ToType(propertyName), this.propertyMapping.ToColumns(propertyName), enabledFilters);
    }

    public int GetPropertyIndex(string propertyName)
    {
      return this.entityMetamodel.GetPropertyIndex(propertyName);
    }

    protected void CreateUniqueKeyLoaders()
    {
      IType[] propertyTypes = this.PropertyTypes;
      string[] propertyNames = this.PropertyNames;
      for (int i = 0; i < this.entityMetamodel.PropertySpan; ++i)
      {
        if (this.propertyUniqueness[i])
          this.uniqueKeyLoaders[propertyNames[i]] = this.CreateUniqueKeyLoader(propertyTypes[i], this.GetPropertyColumnNames(i), (IDictionary<string, NHibernate.IFilter>) new CollectionHelper.EmptyMapClass<string, NHibernate.IFilter>());
      }
    }

    private EntityLoader CreateUniqueKeyLoader(
      IType uniqueKeyType,
      string[] columns,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      if (uniqueKeyType.IsEntityType)
        uniqueKeyType = this.Factory.GetEntityPersister(((EntityType) uniqueKeyType).GetAssociatedEntityName()).IdentifierType;
      return new EntityLoader((IOuterJoinLoadable) this, columns, uniqueKeyType, 1, LockMode.None, this.Factory, enabledFilters);
    }

    protected string GetSQLWhereString(string alias)
    {
      return StringHelper.Replace(this.sqlWhereStringTemplate, Template.Placeholder, alias);
    }

    protected bool HasWhere => !string.IsNullOrEmpty(this.sqlWhereString);

    private void InitOrdinaryPropertyPaths(IMapping mapping)
    {
      for (int index = 0; index < this.SubclassPropertyNameClosure.Length; ++index)
        this.propertyMapping.InitPropertyPaths(this.SubclassPropertyNameClosure[index], this.SubclassPropertyTypeClosure[index], this.SubclassPropertyColumnNameClosure[index], this.SubclassPropertyFormulaTemplateClosure[index], mapping);
    }

    private void InitIdentifierPropertyPaths(IMapping mapping)
    {
      string identifierPropertyName = this.IdentifierPropertyName;
      if (identifierPropertyName != null)
        this.propertyMapping.InitPropertyPaths(identifierPropertyName, this.IdentifierType, this.IdentifierColumnNames, (string[]) null, mapping);
      if (this.entityMetamodel.IdentifierProperty.IsEmbedded)
        this.propertyMapping.InitPropertyPaths((string) null, this.IdentifierType, this.IdentifierColumnNames, (string[]) null, mapping);
      if (this.entityMetamodel.HasNonIdentifierPropertyNamedId)
        return;
      this.propertyMapping.InitPropertyPaths(EntityPersister.EntityID, this.IdentifierType, this.IdentifierColumnNames, (string[]) null, mapping);
    }

    private void InitDiscriminatorPropertyPath()
    {
      this.propertyMapping.InitPropertyPaths("class", this.DiscriminatorType, new string[1]
      {
        this.DiscriminatorColumnName
      }, new string[1]{ this.DiscriminatorFormulaTemplate }, (IMapping) this.Factory);
    }

    private void InitPropertyPaths(IMapping mapping)
    {
      this.InitOrdinaryPropertyPaths(mapping);
      this.InitOrdinaryPropertyPaths(mapping);
      this.InitIdentifierPropertyPaths(mapping);
      if (!this.entityMetamodel.IsPolymorphic)
        return;
      this.InitDiscriminatorPropertyPath();
    }

    protected IUniqueEntityLoader CreateEntityLoader(
      LockMode lockMode,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      return BatchingEntityLoader.CreateBatchingEntityLoader((IOuterJoinLoadable) this, this.batchSize, lockMode, this.Factory, enabledFilters);
    }

    protected IUniqueEntityLoader CreateEntityLoader(LockMode lockMode)
    {
      return this.CreateEntityLoader(lockMode, (IDictionary<string, NHibernate.IFilter>) new CollectionHelper.EmptyMapClass<string, NHibernate.IFilter>());
    }

    protected bool Check(
      int rows,
      object id,
      int tableNumber,
      IExpectation expectation,
      IDbCommand statement)
    {
      try
      {
        expectation.VerifyOutcomeNonBatched(rows, statement);
      }
      catch (StaleStateException ex)
      {
        if (!this.IsNullableTable(tableNumber))
        {
          if (this.Factory.Statistics.IsStatisticsEnabled)
            this.Factory.StatisticsImplementor.OptimisticFailure(this.EntityName);
          throw new StaleObjectStateException(this.EntityName, id);
        }
      }
      catch (TooManyRowsAffectedException ex)
      {
        throw new HibernateException("Duplicate identifier in table for: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory), (Exception) ex);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    protected virtual SqlCommandInfo GenerateUpdateString(
      bool[] includeProperty,
      int j,
      bool useRowId)
    {
      return this.GenerateUpdateString(includeProperty, j, (object[]) null, useRowId);
    }

    protected internal SqlCommandInfo GenerateUpdateString(
      bool[] includeProperty,
      int j,
      object[] oldFields,
      bool useRowId)
    {
      SqlUpdateBuilder sqlUpdateBuilder = new SqlUpdateBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(this.GetTableName(j));
      if (useRowId)
        sqlUpdateBuilder.SetIdentityColumn(new string[1]
        {
          this.rowIdName
        }, (IType) NHibernateUtil.Int32);
      else
        sqlUpdateBuilder.SetIdentityColumn(this.GetKeyColumns(j), this.IdentifierType);
      bool flag = false;
      for (int index = 0; index < this.entityMetamodel.PropertySpan; ++index)
      {
        if (includeProperty[index] && this.IsPropertyOfTable(index, j))
        {
          sqlUpdateBuilder.AddColumns(this.GetPropertyColumnNames(index), this.propertyColumnUpdateable[index], this.PropertyTypes[index]);
          flag = flag || this.GetPropertyColumnSpan(index) > 0;
        }
      }
      if (j == 0 && this.IsVersioned && this.entityMetamodel.OptimisticLockMode == Versioning.OptimisticLock.Version)
      {
        if (this.CheckVersion(includeProperty))
        {
          sqlUpdateBuilder.SetVersionColumn(new string[1]
          {
            this.VersionColumnName
          }, this.VersionType);
          flag = true;
        }
      }
      else if (this.entityMetamodel.OptimisticLockMode > Versioning.OptimisticLock.Version && oldFields != null)
      {
        bool[] flagArray = this.OptimisticLockMode == Versioning.OptimisticLock.All ? this.PropertyUpdateability : includeProperty;
        bool[] propertyVersionability = this.PropertyVersionability;
        IType[] propertyTypes = this.PropertyTypes;
        for (int index1 = 0; index1 < this.entityMetamodel.PropertySpan; ++index1)
        {
          if (flagArray[index1] && this.IsPropertyOfTable(index1, j) && propertyVersionability[index1])
          {
            string[] propertyColumnNames = this.GetPropertyColumnNames(index1);
            bool[] columnNullness = propertyTypes[index1].ToColumnNullness(oldFields[index1], (IMapping) this.Factory);
            SqlType[] sqlTypeArray = propertyTypes[index1].SqlTypes((IMapping) this.Factory);
            for (int index2 = 0; index2 < columnNullness.Length; ++index2)
            {
              if (columnNullness[index2])
                sqlUpdateBuilder.AddWhereFragment(propertyColumnNames[index2], sqlTypeArray[index2], " = ");
              else
                sqlUpdateBuilder.AddWhereFragment(propertyColumnNames[index2] + " is null");
            }
          }
        }
      }
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlUpdateBuilder.SetComment("update " + this.EntityName);
      return !flag ? (SqlCommandInfo) null : sqlUpdateBuilder.ToSqlCommandInfo();
    }

    private bool CheckVersion(bool[] includeProperty)
    {
      return includeProperty[this.VersionProperty] || this.entityMetamodel.PropertyUpdateGenerationInclusions[this.VersionProperty] != ValueInclusion.None;
    }

    protected SqlCommandInfo GenerateInsertString(bool[] includeProperty, int j)
    {
      return this.GenerateInsertString(false, includeProperty, j);
    }

    protected SqlCommandInfo GenerateInsertString(bool identityInsert, bool[] includeProperty)
    {
      return this.GenerateInsertString(identityInsert, includeProperty, 0);
    }

    protected virtual SqlCommandInfo GenerateInsertString(
      bool identityInsert,
      bool[] includeProperty,
      int j)
    {
      SqlInsertBuilder insert = new SqlInsertBuilder(this.Factory).SetTableName(this.GetTableName(j));
      for (int index = 0; index < this.entityMetamodel.PropertySpan; ++index)
      {
        if (includeProperty[index] && this.IsPropertyOfTable(index, j))
          insert.AddColumns(this.GetPropertyColumnNames(index), this.propertyColumnInsertable[index], this.PropertyTypes[index]);
      }
      if (j == 0)
        this.AddDiscriminatorToInsert(insert);
      if (j == 0 && identityInsert)
        insert.AddIdentityColumn(this.GetKeyColumns(0)[0]);
      else
        insert.AddColumns(this.GetKeyColumns(j), (bool[]) null, this.IdentifierType);
      if (this.Factory.Settings.IsCommentsEnabled)
        insert.SetComment("insert " + this.EntityName);
      return j == 0 && identityInsert && this.UseInsertSelectIdentity() ? new SqlCommandInfo(this.Factory.Dialect.AppendIdentitySelectToInsert(insert.ToSqlString()), insert.GetParametersTypeArray()) : insert.ToSqlCommandInfo();
    }

    protected virtual SqlCommandInfo GenerateIdentityInsertString(bool[] includeProperty)
    {
      SqlInsertBuilder insert = (SqlInsertBuilder) this.identityDelegate.PrepareIdentifierGeneratingInsert();
      insert.SetTableName(this.GetTableName(0));
      for (int index = 0; index < this.entityMetamodel.PropertySpan; ++index)
      {
        if (includeProperty[index] && this.IsPropertyOfTable(index, 0))
          insert.AddColumns(this.GetPropertyColumnNames(index), this.propertyColumnInsertable[index], this.PropertyTypes[index]);
      }
      this.AddDiscriminatorToInsert(insert);
      if (this.Factory.Settings.IsCommentsEnabled)
        insert.SetComment("insert " + this.EntityName);
      return insert.ToSqlCommandInfo();
    }

    protected virtual SqlCommandInfo GenerateDeleteString(int j)
    {
      SqlDeleteBuilder sqlDeleteBuilder = new SqlDeleteBuilder(this.Factory.Dialect, (IMapping) this.Factory);
      sqlDeleteBuilder.SetTableName(this.GetTableName(j)).SetIdentityColumn(this.GetKeyColumns(j), this.IdentifierType);
      if (j == 0 && this.IsVersioned && this.entityMetamodel.OptimisticLockMode == Versioning.OptimisticLock.Version)
        sqlDeleteBuilder.SetVersionColumn(new string[1]
        {
          this.VersionColumnName
        }, this.VersionType);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlDeleteBuilder.SetComment("delete " + this.EntityName);
      return sqlDeleteBuilder.ToSqlCommandInfo();
    }

    protected int Dehydrate(
      object id,
      object[] fields,
      bool[] includeProperty,
      bool[][] includeColumns,
      int j,
      IDbCommand st,
      ISessionImplementor session)
    {
      return this.Dehydrate(id, fields, (object) null, includeProperty, includeColumns, j, st, session, 0);
    }

    protected int Dehydrate(
      object id,
      object[] fields,
      object rowId,
      bool[] includeProperty,
      bool[][] includeColumns,
      int table,
      IDbCommand statement,
      ISessionImplementor session,
      int index)
    {
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) ("Dehydrating entity: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
      for (int property = 0; property < this.entityMetamodel.PropertySpan; ++property)
      {
        if (includeProperty[property])
        {
          if (this.IsPropertyOfTable(property, table))
          {
            try
            {
              this.PropertyTypes[property].NullSafeSet(statement, fields[property], index, includeColumns[property], session);
              index += ArrayHelper.CountTrue(includeColumns[property]);
            }
            catch (Exception ex)
            {
              throw new PropertyValueException("Error dehydrating property value for", this.EntityName, this.entityMetamodel.PropertyNames[property], ex);
            }
          }
        }
      }
      if (rowId != null)
        throw new NotImplementedException("support to set the rowId");
      if (id != null)
      {
        this.IdentifierType.NullSafeSet(statement, id, index, session);
        index += this.IdentifierColumnSpan;
      }
      return index;
    }

    public object[] Hydrate(
      IDataReader rs,
      object id,
      object obj,
      ILoadable rootLoadable,
      string[][] suffixedPropertyColumns,
      bool allProperties,
      ISessionImplementor session)
    {
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) ("Hydrating entity: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
      AbstractEntityPersister abstractEntityPersister = (AbstractEntityPersister) rootLoadable;
      bool sequentialSelect1 = abstractEntityPersister.HasSequentialSelect;
      IDbCommand dbCommand = (IDbCommand) null;
      IDataReader reader = (IDataReader) null;
      bool flag1 = false;
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          if (sequentialSelect1)
          {
            SqlString sequentialSelect2 = abstractEntityPersister.GetSequentialSelect(this.EntityName);
            if (sequentialSelect2 != null)
            {
              dbCommand = session.Batcher.PrepareCommand(CommandType.Text, sequentialSelect2, this.IdentifierType.SqlTypes((IMapping) this.factory));
              abstractEntityPersister.IdentifierType.NullSafeSet(dbCommand, id, 0, session);
              reader = session.Batcher.ExecuteReader(dbCommand);
              if (!reader.Read())
                flag1 = true;
            }
          }
          string[] propertyNames = this.PropertyNames;
          IType[] propertyTypes = this.PropertyTypes;
          object[] objArray = new object[propertyTypes.Length];
          bool[] propertyLaziness = this.PropertyLaziness;
          string[] subclassNameClosure = this.SubclassPropertySubclassNameClosure;
          for (int index = 0; index < propertyTypes.Length; ++index)
          {
            if (!this.propertySelectable[index])
              objArray[index] = BackrefPropertyAccessor.Unknown;
            else if (allProperties || !propertyLaziness[index])
            {
              bool flag2 = sequentialSelect1 && abstractEntityPersister.IsSubclassPropertyDeferred(propertyNames[index], subclassNameClosure[index]);
              if (flag2 && flag1)
              {
                objArray[index] = (object) null;
              }
              else
              {
                IDataReader rs1 = flag2 ? reader : rs;
                string[] names = flag2 ? this.propertyColumnAliases[index] : suffixedPropertyColumns[index];
                objArray[index] = propertyTypes[index].Hydrate(rs1, names, session, obj);
              }
            }
            else
              objArray[index] = LazyPropertyInitializer.UnfetchedProperty;
          }
          reader?.Close();
          return objArray;
        }
        finally
        {
          if (dbCommand != null)
            session.Batcher.CloseCommand(dbCommand, reader);
        }
      }
    }

    protected bool UseInsertSelectIdentity()
    {
      return !this.UseGetGeneratedKeys() && this.Factory.Dialect.SupportsInsertSelectIdentity;
    }

    protected bool UseGetGeneratedKeys() => this.Factory.Settings.IsGetGeneratedKeysEnabled;

    protected virtual SqlString GetSequentialSelect(string entityName)
    {
      throw new NotSupportedException("no sequential selects");
    }

    protected object Insert(
      object[] fields,
      bool[] notNull,
      SqlCommandInfo sql,
      object obj,
      ISessionImplementor session)
    {
      if (AbstractEntityPersister.log.IsDebugEnabled)
      {
        AbstractEntityPersister.log.Debug((object) ("Inserting entity: " + this.EntityName + " (native id)"));
        if (this.IsVersioned)
          AbstractEntityPersister.log.Debug((object) ("Version: " + Versioning.GetVersion(fields, (IEntityPersister) this)));
      }
      IBinder binder = (IBinder) new AbstractEntityPersister.GeneratedIdentifierBinder(fields, notNull, session, obj, this);
      return this.identityDelegate.PerformInsert(sql, session, binder);
    }

    public virtual SqlString GetSelectByUniqueKeyString(string propertyName)
    {
      return new SqlSimpleSelectBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(this.GetTableName(0)).AddColumns(this.GetKeyColumns(0)).AddWhereFragment(this.GetPropertyColumnNames(propertyName), this.GetPropertyType(propertyName), " = ").ToSqlString();
    }

    protected void Insert(
      object id,
      object[] fields,
      bool[] notNull,
      int j,
      SqlCommandInfo sql,
      object obj,
      ISessionImplementor session)
    {
      if (this.IsInverseTable(j) || this.IsNullableTable(j) && this.IsAllNull(fields, j))
        return;
      if (AbstractEntityPersister.log.IsDebugEnabled)
      {
        AbstractEntityPersister.log.Debug((object) ("Inserting entity: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
        if (j == 0 && this.IsVersioned)
          AbstractEntityPersister.log.Debug((object) ("Version: " + Versioning.GetVersion(fields, (IEntityPersister) this)));
      }
      IExpectation expectation = Expectations.AppropriateExpectation(this.insertResultCheckStyles[j]);
      bool flag = j == 0 && expectation.CanBeBatched;
      try
      {
        IDbCommand dbCommand = flag ? session.Batcher.PrepareBatchCommand(sql.CommandType, sql.Text, sql.ParameterTypes) : session.Batcher.PrepareCommand(sql.CommandType, sql.Text, sql.ParameterTypes);
        try
        {
          int index = 0;
          this.Dehydrate(id, fields, (object) null, notNull, this.propertyColumnInsertable, j, dbCommand, session, index);
          if (flag)
            session.Batcher.AddToBatch(expectation);
          else
            expectation.VerifyOutcomeNonBatched(session.Batcher.ExecuteNonQuery(dbCommand), dbCommand);
        }
        catch (Exception ex)
        {
          if (flag)
            session.Batcher.AbortBatch(ex);
          throw;
        }
        finally
        {
          if (!flag)
            session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
        {
          SqlException = (Exception) ex,
          Message = "could not insert: " + MessageHelper.InfoString((IEntityPersister) this, id),
          Sql = sql.ToString(),
          EntityName = this.EntityName,
          EntityId = id
        });
      }
    }

    protected internal virtual void UpdateOrInsert(
      object id,
      object[] fields,
      object[] oldFields,
      object rowId,
      bool[] includeProperty,
      int j,
      object oldVersion,
      object obj,
      SqlCommandInfo sql,
      ISessionImplementor session)
    {
      if (this.IsInverseTable(j))
        return;
      bool flag;
      if (this.IsNullableTable(j) && oldFields != null && this.IsAllNull(oldFields, j))
        flag = false;
      else if (this.IsNullableTable(j) && this.IsAllNull(fields, j))
      {
        flag = true;
        this.Delete(id, oldVersion, j, obj, this.SqlDeleteStrings[j], session, (object[]) null);
      }
      else
        flag = this.Update(id, fields, oldFields, rowId, includeProperty, j, oldVersion, obj, sql, session);
      if (flag || this.IsAllNull(fields, j))
        return;
      this.Insert(id, fields, this.PropertyInsertability, j, this.SqlInsertStrings[j], obj, session);
    }

    protected bool Update(
      object id,
      object[] fields,
      object[] oldFields,
      object rowId,
      bool[] includeProperty,
      int j,
      object oldVersion,
      object obj,
      SqlCommandInfo sql,
      ISessionImplementor session)
    {
      bool flag1 = j == 0 && this.IsVersioned;
      IExpectation expectation = Expectations.AppropriateExpectation(this.updateResultCheckStyles[j]);
      bool flag2 = j == 0 && expectation.CanBeBatched && this.IsBatchable;
      if (AbstractEntityPersister.log.IsDebugEnabled)
      {
        AbstractEntityPersister.log.Debug((object) ("Updating entity: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
        if (flag1)
          AbstractEntityPersister.log.Debug((object) ("Existing version: " + oldVersion + " -> New Version: " + fields[this.VersionProperty]));
      }
      try
      {
        int index1 = 0;
        IDbCommand dbCommand = flag2 ? session.Batcher.PrepareBatchCommand(sql.CommandType, sql.Text, sql.ParameterTypes) : session.Batcher.PrepareCommand(sql.CommandType, sql.Text, sql.ParameterTypes);
        try
        {
          int index2 = this.Dehydrate(id, fields, rowId, includeProperty, this.propertyColumnUpdateable, j, dbCommand, session, index1);
          if (flag1 && this.entityMetamodel.OptimisticLockMode == Versioning.OptimisticLock.Version)
          {
            if (this.CheckVersion(includeProperty))
              this.VersionType.NullSafeSet(dbCommand, oldVersion, index2, session);
          }
          else if (this.entityMetamodel.OptimisticLockMode > Versioning.OptimisticLock.Version && oldFields != null)
          {
            bool[] propertyVersionability = this.PropertyVersionability;
            bool[] flagArray = this.OptimisticLockMode == Versioning.OptimisticLock.All ? this.PropertyUpdateability : includeProperty;
            IType[] propertyTypes = this.PropertyTypes;
            for (int property = 0; property < this.entityMetamodel.PropertySpan; ++property)
            {
              if (flagArray[property] && this.IsPropertyOfTable(property, j) && propertyVersionability[property])
              {
                bool[] columnNullness = propertyTypes[property].ToColumnNullness(oldFields[property], (IMapping) this.Factory);
                propertyTypes[property].NullSafeSet(dbCommand, oldFields[property], index2, columnNullness, session);
                index2 += ArrayHelper.CountTrue(columnNullness);
              }
            }
          }
          if (!flag2)
            return this.Check(session.Batcher.ExecuteNonQuery(dbCommand), id, j, expectation, dbCommand);
          session.Batcher.AddToBatch(expectation);
          return true;
        }
        catch (StaleStateException ex)
        {
          if (flag2)
            session.Batcher.AbortBatch((Exception) ex);
          throw new StaleObjectStateException(this.EntityName, id);
        }
        catch (Exception ex)
        {
          if (flag2)
            session.Batcher.AbortBatch(ex);
          throw;
        }
        finally
        {
          if (!flag2)
            session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        }
      }
      catch (DbException ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
        {
          SqlException = (Exception) ex,
          Message = "could not update: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory),
          Sql = sql.Text.ToString(),
          EntityName = this.EntityName,
          EntityId = id
        });
      }
    }

    public void Delete(
      object id,
      object version,
      int j,
      object obj,
      SqlCommandInfo sql,
      ISessionImplementor session,
      object[] loadedState)
    {
      if (this.IsInverseTable(j))
        return;
      bool flag1 = j == 0 && this.IsVersioned && Versioning.OptimisticLock.Version == this.entityMetamodel.OptimisticLockMode;
      IExpectation expectation = Expectations.AppropriateExpectation(this.deleteResultCheckStyles[j]);
      bool flag2 = j == 0 && expectation.CanBeBatched && this.IsBatchable;
      if (AbstractEntityPersister.log.IsDebugEnabled)
      {
        AbstractEntityPersister.log.Debug((object) ("Deleting entity: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
        if (flag1)
          AbstractEntityPersister.log.Debug((object) ("Version: " + version));
      }
      if (this.IsTableCascadeDeleteEnabled(j))
      {
        if (!AbstractEntityPersister.log.IsDebugEnabled)
          return;
        AbstractEntityPersister.log.Debug((object) ("delete handled by foreign key constraint: " + this.GetTableName(j)));
      }
      else
      {
        try
        {
          int index1 = 0;
          IDbCommand dbCommand = !flag2 ? session.Batcher.PrepareCommand(sql.CommandType, sql.Text, sql.ParameterTypes) : session.Batcher.PrepareBatchCommand(sql.CommandType, sql.Text, sql.ParameterTypes);
          try
          {
            this.IdentifierType.NullSafeSet(dbCommand, id, index1, session);
            int index2 = index1 + this.IdentifierColumnSpan;
            if (flag1)
              this.VersionType.NullSafeSet(dbCommand, version, index2, session);
            else if (this.entityMetamodel.OptimisticLockMode > Versioning.OptimisticLock.Version && loadedState != null)
            {
              bool[] propertyVersionability = this.PropertyVersionability;
              IType[] propertyTypes = this.PropertyTypes;
              for (int property = 0; property < this.entityMetamodel.PropertySpan; ++property)
              {
                if (this.IsPropertyOfTable(property, j) && propertyVersionability[property])
                {
                  bool[] columnNullness = propertyTypes[property].ToColumnNullness(loadedState[property], (IMapping) this.Factory);
                  propertyTypes[property].NullSafeSet(dbCommand, loadedState[property], index2, columnNullness, session);
                  index2 += ArrayHelper.CountTrue(columnNullness);
                }
              }
            }
            if (flag2)
              session.Batcher.AddToBatch(expectation);
            else
              this.Check(session.Batcher.ExecuteNonQuery(dbCommand), id, j, expectation, dbCommand);
          }
          catch (Exception ex)
          {
            if (flag2)
              session.Batcher.AbortBatch(ex);
            throw;
          }
          finally
          {
            if (!flag2)
              session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
          {
            SqlException = (Exception) ex,
            Message = "could not delete: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory),
            Sql = sql.Text.ToString(),
            EntityName = this.EntityName,
            EntityId = id
          });
        }
      }
    }

    private SqlCommandInfo[] GetUpdateStrings(bool byRowId, bool lazy)
    {
      return byRowId ? (!lazy ? this.SQLUpdateByRowIdStrings : this.SQLLazyUpdateByRowIdStrings) : (!lazy ? this.SqlUpdateStrings : this.SQLLazyUpdateStrings);
    }

    public void Update(
      object id,
      object[] fields,
      int[] dirtyFields,
      bool hasDirtyCollection,
      object[] oldFields,
      object oldVersion,
      object obj,
      object rowId,
      ISessionImplementor session)
    {
      bool[] tableUpdateNeeded = this.GetTableUpdateNeeded(dirtyFields, hasDirtyCollection);
      int tableSpan = this.TableSpan;
      EntityEntry entry = session.PersistenceContext.GetEntry(obj);
      if (entry == null && !this.IsMutable)
        throw new InvalidOperationException("Updating immutable entity that is not in session yet!");
      bool[] includeProperty;
      SqlCommandInfo[] sqlCommandInfoArray;
      if (this.entityMetamodel.IsDynamicUpdate && dirtyFields != null)
      {
        includeProperty = this.GetPropertiesToUpdate(dirtyFields, hasDirtyCollection);
        sqlCommandInfoArray = new SqlCommandInfo[tableSpan];
        for (int j = 0; j < tableSpan; ++j)
          sqlCommandInfoArray[j] = tableUpdateNeeded[j] ? this.GenerateUpdateString(includeProperty, j, oldFields, j == 0 && rowId != null) : (SqlCommandInfo) null;
      }
      else if (!this.IsModifiableEntity(entry))
      {
        includeProperty = this.GetPropertiesToUpdate(dirtyFields == null ? ArrayHelper.EmptyIntArray : dirtyFields, hasDirtyCollection);
        sqlCommandInfoArray = new SqlCommandInfo[tableSpan];
        for (int j = 0; j < tableSpan; ++j)
          sqlCommandInfoArray[j] = tableUpdateNeeded[j] ? this.GenerateUpdateString(includeProperty, j, oldFields, j == 0 && rowId != null) : (SqlCommandInfo) null;
      }
      else
      {
        sqlCommandInfoArray = this.GetUpdateStrings(rowId != null, this.HasUninitializedLazyProperties(obj, session.EntityMode));
        includeProperty = this.GetPropertyUpdateability(obj, session.EntityMode);
      }
      for (int j = 0; j < tableSpan; ++j)
      {
        if (tableUpdateNeeded[j])
          this.UpdateOrInsert(id, fields, oldFields, j == 0 ? rowId : (object) null, includeProperty, j, oldVersion, obj, sqlCommandInfoArray[j], session);
      }
    }

    public object Insert(object[] fields, object obj, ISessionImplementor session)
    {
      int tableSpan = this.TableSpan;
      object id;
      if (this.entityMetamodel.IsDynamicInsert)
      {
        bool[] propertiesToInsert = this.GetPropertiesToInsert(fields);
        id = this.Insert(fields, propertiesToInsert, this.GenerateInsertString(true, propertiesToInsert), obj, session);
        for (int j = 1; j < tableSpan; ++j)
          this.Insert(id, fields, propertiesToInsert, j, this.GenerateInsertString(propertiesToInsert, j), obj, session);
      }
      else
      {
        id = this.Insert(fields, this.PropertyInsertability, this.SQLIdentityInsertString, obj, session);
        for (int j = 1; j < tableSpan; ++j)
          this.Insert(id, fields, this.PropertyInsertability, j, this.SqlInsertStrings[j], obj, session);
      }
      return id;
    }

    public void Insert(object id, object[] fields, object obj, ISessionImplementor session)
    {
      int tableSpan = this.TableSpan;
      if (this.entityMetamodel.IsDynamicInsert)
      {
        bool[] propertiesToInsert = this.GetPropertiesToInsert(fields);
        for (int j = 0; j < tableSpan; ++j)
          this.Insert(id, fields, propertiesToInsert, j, this.GenerateInsertString(propertiesToInsert, j), obj, session);
      }
      else
      {
        for (int j = 0; j < tableSpan; ++j)
          this.Insert(id, fields, this.PropertyInsertability, j, this.SqlInsertStrings[j], obj, session);
      }
    }

    public void Delete(object id, object version, object obj, ISessionImplementor session)
    {
      int tableSpan = this.TableSpan;
      bool flag = !this.entityMetamodel.IsVersioned && this.entityMetamodel.OptimisticLockMode > Versioning.OptimisticLock.Version;
      object[] loadedState = (object[]) null;
      if (flag)
      {
        EntityKey key = new EntityKey(id, (IEntityPersister) this, session.EntityMode);
        object entity = session.PersistenceContext.GetEntity(key);
        if (entity != null)
          loadedState = session.PersistenceContext.GetEntry(entity).LoadedState;
      }
      SqlCommandInfo[] sqlCommandInfoArray = !flag || loadedState == null ? this.SqlDeleteStrings : this.GenerateSQLDeleteStrings(loadedState);
      for (int j = tableSpan - 1; j >= 0; --j)
        this.Delete(id, version, j, obj, sqlCommandInfoArray[j], session, loadedState);
    }

    protected SqlCommandInfo[] GenerateSQLDeleteStrings(object[] loadedState)
    {
      int tableSpan = this.TableSpan;
      SqlCommandInfo[] sqlDeleteStrings = new SqlCommandInfo[tableSpan];
      for (int table = tableSpan - 1; table >= 0; --table)
      {
        SqlDeleteBuilder sqlDeleteBuilder = new SqlDeleteBuilder(this.Factory.Dialect, (IMapping) this.Factory).SetTableName(this.GetTableName(table)).SetIdentityColumn(this.GetKeyColumns(table), this.IdentifierType);
        if (this.Factory.Settings.IsCommentsEnabled)
          sqlDeleteBuilder.SetComment("delete " + this.EntityName + " [" + (object) table + "]");
        bool[] propertyVersionability = this.PropertyVersionability;
        IType[] propertyTypes = this.PropertyTypes;
        for (int index1 = 0; index1 < this.entityMetamodel.PropertySpan; ++index1)
        {
          if (propertyVersionability[index1] && this.IsPropertyOfTable(index1, table))
          {
            string[] propertyColumnNames = this.GetPropertyColumnNames(index1);
            bool[] columnNullness = propertyTypes[index1].ToColumnNullness(loadedState[index1], (IMapping) this.Factory);
            SqlType[] sqlTypeArray = propertyTypes[index1].SqlTypes((IMapping) this.Factory);
            for (int index2 = 0; index2 < columnNullness.Length; ++index2)
            {
              if (columnNullness[index2])
                sqlDeleteBuilder.AddWhereFragment(propertyColumnNames[index2], sqlTypeArray[index2], " = ");
              else
                sqlDeleteBuilder.AddWhereFragment(propertyColumnNames[index2] + " is null");
            }
          }
        }
        sqlDeleteStrings[table] = sqlDeleteBuilder.ToSqlCommandInfo();
      }
      return sqlDeleteStrings;
    }

    protected void LogStaticSQL()
    {
      if (!AbstractEntityPersister.log.IsDebugEnabled)
        return;
      AbstractEntityPersister.log.Debug((object) ("Static SQL for entity: " + this.EntityName));
      if (this.sqlLazySelectString != null)
        AbstractEntityPersister.log.Debug((object) (" Lazy select: " + (object) this.sqlLazySelectString));
      if (this.sqlVersionSelectString != null)
        AbstractEntityPersister.log.Debug((object) (" Version select: " + (object) this.sqlVersionSelectString));
      if (this.sqlSnapshotSelectString != null)
        AbstractEntityPersister.log.Debug((object) (" Snapshot select: " + (object) this.sqlSnapshotSelectString));
      for (int index = 0; index < this.TableSpan; ++index)
      {
        AbstractEntityPersister.log.Debug((object) (" Insert " + (object) index + ": " + (object) this.SqlInsertStrings[index]));
        AbstractEntityPersister.log.Debug((object) (" Update " + (object) index + ": " + (object) this.SqlUpdateStrings[index]));
        AbstractEntityPersister.log.Debug((object) (" Delete " + (object) index + ": " + (object) this.SqlDeleteStrings[index]));
      }
      if (this.sqlIdentityInsertString != null)
        AbstractEntityPersister.log.Debug((object) (" Identity insert: " + (object) this.sqlIdentityInsertString));
      if (this.sqlUpdateByRowIdString != null)
        AbstractEntityPersister.log.Debug((object) (" Update by row id (all fields): " + (object) this.sqlUpdateByRowIdString));
      if (this.sqlLazyUpdateByRowIdString != null)
        AbstractEntityPersister.log.Debug((object) (" Update by row id (non-lazy fields): " + (object) this.sqlLazyUpdateByRowIdString));
      if (this.sqlInsertGeneratedValuesSelectString != null)
        AbstractEntityPersister.log.Debug((object) ("Insert-generated property select: " + (object) this.sqlInsertGeneratedValuesSelectString));
      if (this.sqlUpdateGeneratedValuesSelectString == null)
        return;
      AbstractEntityPersister.log.Debug((object) ("Update-generated property select: " + (object) this.sqlUpdateGeneratedValuesSelectString));
    }

    public virtual string FilterFragment(string alias, IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      StringBuilder buffer = new StringBuilder();
      this.filterHelper.Render(buffer, this.GenerateFilterConditionAlias(alias), enabledFilters);
      return buffer.Append(this.FilterFragment(alias)).ToString();
    }

    public virtual string GenerateFilterConditionAlias(string rootAlias) => rootAlias;

    public virtual string OneToManyFilterFragment(string alias) => string.Empty;

    public virtual SqlString FromJoinFragment(string alias, bool innerJoin, bool includeSubclasses)
    {
      return this.SubclassTableSpan != 1 ? this.CreateJoin(alias, innerJoin, includeSubclasses).ToFromFragmentString : new SqlString(string.Empty);
    }

    public virtual SqlString WhereJoinFragment(
      string alias,
      bool innerJoin,
      bool includeSubclasses)
    {
      return this.SubclassTableSpan != 1 ? this.CreateJoin(alias, innerJoin, includeSubclasses).ToWhereFragmentString : SqlString.Empty;
    }

    protected internal virtual bool IsSubclassTableLazy(int j) => false;

    private JoinFragment CreateJoin(string name, bool innerjoin, bool includeSubclasses)
    {
      string[] fkColumns = StringHelper.Qualify(name, this.IdentifierColumnNames);
      JoinFragment outerJoinFragment = this.Factory.Dialect.CreateOuterJoinFragment();
      int subclassTableSpan = this.SubclassTableSpan;
      for (int index = 1; index < subclassTableSpan; ++index)
      {
        if (this.IsClassOrSuperclassTable(index) || includeSubclasses && !this.IsSubclassTableSequentialSelect(index) && !this.IsSubclassTableLazy(index))
          outerJoinFragment.AddJoin(this.GetSubclassTableName(index), this.GenerateTableAlias(name, index), fkColumns, this.GetSubclassTableKeyColumns(index), !innerjoin || !this.IsClassOrSuperclassTable(index) || this.IsInverseTable(index) || this.IsNullableTable(index) ? JoinType.LeftOuterJoin : JoinType.InnerJoin);
      }
      return outerJoinFragment;
    }

    private JoinFragment CreateJoin(int[] tableNumbers, string drivingAlias)
    {
      string[] fkColumns = StringHelper.Qualify(drivingAlias, this.GetSubclassTableKeyColumns(tableNumbers[0]));
      JoinFragment outerJoinFragment = this.Factory.Dialect.CreateOuterJoinFragment();
      for (int index = 1; index < tableNumbers.Length; ++index)
      {
        int tableNumber = tableNumbers[index];
        outerJoinFragment.AddJoin(this.GetSubclassTableName(tableNumber), this.GenerateTableAlias(this.RootAlias, tableNumber), fkColumns, this.GetSubclassTableKeyColumns(tableNumber), this.IsInverseSubclassTable(tableNumber) || this.IsNullableSubclassTable(tableNumber) ? JoinType.LeftOuterJoin : JoinType.InnerJoin);
      }
      return outerJoinFragment;
    }

    protected NHibernate.SqlCommand.SelectFragment CreateSelect(
      int[] subclassColumnNumbers,
      int[] subclassFormulaNumbers)
    {
      NHibernate.SqlCommand.SelectFragment select = new NHibernate.SqlCommand.SelectFragment(this.Factory.Dialect);
      int[] tableNumberClosure1 = this.SubclassColumnTableNumberClosure;
      string[] columnAliasClosure = this.SubclassColumnAliasClosure;
      string[] subclassColumnClosure = this.SubclassColumnClosure;
      for (int index = 0; index < subclassColumnNumbers.Length; ++index)
      {
        if (this.subclassColumnSelectableClosure[index])
        {
          int subclassColumnNumber = subclassColumnNumbers[index];
          string tableAlias = this.GenerateTableAlias(this.RootAlias, tableNumberClosure1[subclassColumnNumber]);
          select.AddColumn(tableAlias, subclassColumnClosure[subclassColumnNumber], columnAliasClosure[subclassColumnNumber]);
        }
      }
      int[] tableNumberClosure2 = this.SubclassFormulaTableNumberClosure;
      string[] formulaTemplateClosure = this.SubclassFormulaTemplateClosure;
      string[] formulaAliasClosure = this.SubclassFormulaAliasClosure;
      for (int index = 0; index < subclassFormulaNumbers.Length; ++index)
      {
        int subclassFormulaNumber = subclassFormulaNumbers[index];
        string tableAlias = this.GenerateTableAlias(this.RootAlias, tableNumberClosure2[subclassFormulaNumber]);
        select.AddFormula(tableAlias, formulaTemplateClosure[subclassFormulaNumber], formulaAliasClosure[subclassFormulaNumber]);
      }
      return select;
    }

    protected string CreateFrom(int tableNumber, string alias)
    {
      return this.GetSubclassTableName(tableNumber) + (object) ' ' + alias;
    }

    protected SqlString CreateWhereByKey(int tableNumber, string alias)
    {
      string[] strArray = StringHelper.Qualify(alias, this.GetSubclassTableKeyColumns(tableNumber));
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(strArray.Length * 4);
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str = strArray[index];
        if (index > 0)
          sqlStringBuilder.Add(" and ");
        sqlStringBuilder.Add(str + "=").AddParameter();
      }
      return sqlStringBuilder.ToSqlString();
    }

    protected SqlString RenderSelect(int[] tableNumbers, int[] columnNumbers, int[] formulaNumbers)
    {
      System.Array.Sort<int>(tableNumbers);
      int tableNumber = tableNumbers[0];
      string tableAlias = this.GenerateTableAlias(this.RootAlias, tableNumber);
      SqlString whereByKey = this.CreateWhereByKey(tableNumber, tableAlias);
      string from = this.CreateFrom(tableNumber, tableAlias);
      JoinFragment join = this.CreateJoin(tableNumbers, tableAlias);
      NHibernate.SqlCommand.SelectFragment select = this.CreateSelect(columnNumbers, formulaNumbers);
      SqlSelectBuilder sqlSelectBuilder = new SqlSelectBuilder(this.Factory);
      sqlSelectBuilder.SetSelectClause(select.ToFragmentString().Substring(2));
      sqlSelectBuilder.SetFromClause(from);
      sqlSelectBuilder.SetWhereClause(whereByKey);
      sqlSelectBuilder.SetOuterJoins(join.ToFromFragmentString, join.ToWhereFragmentString);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlSelectBuilder.SetComment("sequential select " + this.EntityName);
      return sqlSelectBuilder.ToSqlString();
    }

    protected void PostConstruct(IMapping mapping) => this.InitPropertyPaths(mapping);

    public virtual void PostInstantiate()
    {
      int tableSpan = this.TableSpan;
      this.sqlDeleteStrings = new SqlCommandInfo[tableSpan];
      this.sqlInsertStrings = new SqlCommandInfo[tableSpan];
      this.sqlUpdateStrings = new SqlCommandInfo[tableSpan];
      this.sqlLazyUpdateStrings = new SqlCommandInfo[tableSpan];
      this.sqlUpdateByRowIdString = this.rowIdName == null ? (SqlCommandInfo) null : this.GenerateUpdateString(this.PropertyUpdateability, 0, true);
      this.sqlLazyUpdateByRowIdString = this.rowIdName == null ? (SqlCommandInfo) null : this.GenerateUpdateString(this.NonLazyPropertyUpdateability, 0, true);
      for (int j = 0; j < tableSpan; ++j)
      {
        SqlCommandInfo insertString = this.GenerateInsertString(this.PropertyInsertability, j);
        SqlCommandInfo updateString = this.GenerateUpdateString(this.PropertyUpdateability, j, false);
        SqlCommandInfo deleteString = this.GenerateDeleteString(j);
        this.sqlInsertStrings[j] = this.customSQLInsert[j] != null ? new SqlCommandInfo(this.customSQLInsert[j], insertString.ParameterTypes) : insertString;
        this.sqlUpdateStrings[j] = this.customSQLUpdate[j] != null ? new SqlCommandInfo(this.customSQLUpdate[j], updateString.ParameterTypes) : updateString;
        this.sqlLazyUpdateStrings[j] = this.customSQLUpdate[j] != null ? new SqlCommandInfo(this.customSQLUpdate[j], updateString.ParameterTypes) : this.GenerateUpdateString(this.NonLazyPropertyUpdateability, j, false);
        this.sqlDeleteStrings[j] = this.customSQLDelete[j] != null ? new SqlCommandInfo(this.customSQLDelete[j], deleteString.ParameterTypes) : deleteString;
      }
      this.tableHasColumns = new bool[tableSpan];
      for (int index = 0; index < tableSpan; ++index)
        this.tableHasColumns[index] = this.sqlUpdateStrings[index] != null;
      this.sqlSnapshotSelectString = this.GenerateSnapshotSelectString();
      this.sqlLazySelectString = this.GenerateLazySelectString();
      this.sqlVersionSelectString = this.GenerateSelectVersionString();
      if (this.HasInsertGeneratedProperties)
        this.sqlInsertGeneratedValuesSelectString = this.GenerateInsertGeneratedValuesSelectString();
      if (this.HasUpdateGeneratedProperties)
        this.sqlUpdateGeneratedValuesSelectString = this.GenerateUpdateGeneratedValuesSelectString();
      if (this.IsIdentifierAssignedByInsert)
      {
        this.identityDelegate = ((IPostInsertIdentifierGenerator) this.IdentifierGenerator).GetInsertGeneratedIdentifierDelegate((IPostInsertIdentityPersister) this, this.Factory, this.UseGetGeneratedKeys());
        SqlCommandInfo identityInsertString = this.GenerateIdentityInsertString(this.PropertyInsertability);
        this.sqlIdentityInsertString = this.customSQLInsert[0] != null ? new SqlCommandInfo(this.customSQLInsert[0], identityInsertString.ParameterTypes) : identityInsertString;
      }
      else
        this.sqlIdentityInsertString = (SqlCommandInfo) null;
      this.LogStaticSQL();
      this.CreateLoaders();
      this.CreateUniqueKeyLoaders();
      this.CreateQueryLoader();
    }

    private void CreateLoaders()
    {
      this.loaders[LockMode.None.ToString()] = this.CreateEntityLoader(LockMode.None);
      IUniqueEntityLoader entityLoader = this.CreateEntityLoader(LockMode.Read);
      this.loaders[LockMode.Read.ToString()] = entityLoader;
      bool flag = this.SubclassTableSpan > 1 && this.HasSubclasses && !this.Factory.Dialect.SupportsOuterJoinForUpdate;
      this.loaders[LockMode.Upgrade.ToString()] = flag ? entityLoader : this.CreateEntityLoader(LockMode.Upgrade);
      this.loaders[LockMode.UpgradeNoWait.ToString()] = flag ? entityLoader : this.CreateEntityLoader(LockMode.UpgradeNoWait);
      this.loaders[LockMode.Force.ToString()] = flag ? entityLoader : this.CreateEntityLoader(LockMode.Force);
      this.loaders["merge"] = (IUniqueEntityLoader) new CascadeEntityLoader((IOuterJoinLoadable) this, CascadingAction.Merge, this.Factory);
      this.loaders["refresh"] = (IUniqueEntityLoader) new CascadeEntityLoader((IOuterJoinLoadable) this, CascadingAction.Refresh, this.Factory);
    }

    protected void CreateQueryLoader()
    {
      if (this.loaderName == null)
        return;
      this.queryLoader = (IUniqueEntityLoader) new NamedQueryLoader(this.loaderName, (IEntityPersister) this);
    }

    public object Load(
      object id,
      object optionalObject,
      LockMode lockMode,
      ISessionImplementor session)
    {
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) ("Fetching entity: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
      return this.GetAppropriateLoader(lockMode, session).Load(id, optionalObject, session);
    }

    private IUniqueEntityLoader GetAppropriateLoader(LockMode lockMode, ISessionImplementor session)
    {
      IDictionary<string, NHibernate.IFilter> enabledFilters = session.EnabledFilters;
      if (this.queryLoader != null)
        return this.queryLoader;
      if (enabledFilters != null && enabledFilters.Count != 0)
        return this.CreateEntityLoader(lockMode, enabledFilters);
      return !string.IsNullOrEmpty(session.FetchProfile) && LockMode.Upgrade.GreaterThan(lockMode) ? this.loaders[session.FetchProfile] : this.loaders[lockMode.ToString()];
    }

    private bool IsAllNull(object[] array, int tableNumber)
    {
      for (int property = 0; property < array.Length; ++property)
      {
        if (this.IsPropertyOfTable(property, tableNumber) && array[property] != null)
          return false;
      }
      return true;
    }

    public bool IsSubclassPropertyNullable(int i) => this.subclassPropertyNullabilityClosure[i];

    protected bool[] GetPropertiesToUpdate(int[] dirtyProperties, bool hasDirtyCollection)
    {
      bool[] propertiesToUpdate = new bool[this.entityMetamodel.PropertySpan];
      bool[] propertyUpdateability = this.PropertyUpdateability;
      for (int index = 0; index < dirtyProperties.Length; ++index)
      {
        int dirtyProperty = dirtyProperties[index];
        if (propertyUpdateability[dirtyProperty])
          propertiesToUpdate[dirtyProperty] = true;
      }
      if (this.IsVersioned)
        propertiesToUpdate[this.VersionProperty] = this.PropertyUpdateability[this.VersionProperty] && Versioning.IsVersionIncrementRequired(dirtyProperties, hasDirtyCollection, this.PropertyVersionability);
      return propertiesToUpdate;
    }

    protected bool[] GetPropertiesToInsert(object[] fields)
    {
      bool[] propertiesToInsert = new bool[fields.Length];
      bool[] propertyInsertability = this.PropertyInsertability;
      for (int index = 0; index < fields.Length; ++index)
        propertiesToInsert[index] = propertyInsertability[index] && fields[index] != null;
      return propertiesToInsert;
    }

    public virtual int[] FindDirty(
      object[] currentState,
      object[] previousState,
      object entity,
      ISessionImplementor session)
    {
      int[] dirty = TypeHelper.FindDirty(this.entityMetamodel.Properties, currentState, previousState, this.propertyColumnUpdateable, this.HasUninitializedLazyProperties(entity, session.EntityMode), session);
      if (dirty == null)
        return (int[]) null;
      this.LogDirtyProperties(dirty);
      return dirty;
    }

    public virtual int[] FindModified(
      object[] old,
      object[] current,
      object entity,
      ISessionImplementor session)
    {
      int[] modified = TypeHelper.FindModified(this.entityMetamodel.Properties, current, old, this.propertyColumnUpdateable, this.HasUninitializedLazyProperties(entity, session.EntityMode), session);
      if (modified == null)
        return (int[]) null;
      this.LogDirtyProperties(modified);
      return modified;
    }

    protected bool[] GetPropertyUpdateability(object entity, EntityMode entityMode)
    {
      return !this.HasUninitializedLazyProperties(entity, entityMode) ? this.PropertyUpdateability : this.NonLazyPropertyUpdateability;
    }

    private void LogDirtyProperties(int[] props)
    {
      if (!AbstractEntityPersister.log.IsDebugEnabled)
        return;
      for (int index = 0; index < props.Length; ++index)
      {
        string name = this.entityMetamodel.Properties[props[index]].Name;
        AbstractEntityPersister.log.Debug((object) (StringHelper.Qualify(this.EntityName, name) + " is dirty"));
      }
    }

    protected internal IEntityTuplizer GetTuplizer(ISessionImplementor session)
    {
      return this.GetTuplizer(session.EntityMode);
    }

    protected internal IEntityTuplizer GetTuplizer(EntityMode entityMode)
    {
      return this.entityMetamodel.GetTuplizer(entityMode);
    }

    public virtual bool HasCache => this.cache != null;

    private string GetSubclassEntityName(System.Type clazz)
    {
      string subclassEntityName;
      this.entityNameBySubclass.TryGetValue(clazz, out subclassEntityName);
      return subclassEntityName;
    }

    public virtual bool HasCascades => this.entityMetamodel.HasCascades;

    public virtual bool HasIdentifierProperty => !this.entityMetamodel.IdentifierProperty.IsVirtual;

    private IVersionType LocateVersionType()
    {
      return this.entityMetamodel.VersionProperty != null ? (IVersionType) this.entityMetamodel.VersionProperty.Type : (IVersionType) null;
    }

    public virtual bool HasLazyProperties => this.entityMetamodel.HasLazyProperties;

    public virtual void AfterReassociate(object entity, ISessionImplementor session)
    {
      if (!FieldInterceptionHelper.IsInstrumented(entity))
        return;
      IFieldInterceptor fieldInterceptor = FieldInterceptionHelper.ExtractFieldInterceptor(entity);
      if (fieldInterceptor != null)
        fieldInterceptor.Session = session;
      else
        FieldInterceptionHelper.InjectFieldInterceptor(entity, this.EntityName, this.GetMappedClass(session.EntityMode), (ISet<string>) null, (ISet<string>) null, session).MarkDirty();
    }

    public virtual bool? IsTransient(object entity, ISessionImplementor session)
    {
      object identifier = !this.CanExtractIdOutOfEntity ? (object) null : this.GetIdentifier(entity, session.EntityMode);
      if (identifier == null)
        return new bool?(true);
      if (this.IsVersioned)
      {
        bool? nullable = this.entityMetamodel.VersionProperty.UnsavedValue.IsUnsaved(this.GetVersion(entity, session.EntityMode));
        if (nullable.HasValue)
          return nullable;
      }
      bool? nullable1 = this.entityMetamodel.IdentifierProperty.UnsavedValue.IsUnsaved(identifier);
      if (nullable1.HasValue)
      {
        if (!(this.IdentifierGenerator is Assigned))
          return nullable1;
        if (nullable1.Value)
          return new bool?(true);
      }
      return this.HasCache && this.Cache.Get(new CacheKey(identifier, this.IdentifierType, this.RootEntityName, session.EntityMode, session.Factory), session.Timestamp) != null ? new bool?(false) : new bool?();
    }

    public virtual bool IsModifiableEntity(EntityEntry entry)
    {
      return entry != null ? entry.IsModifiableEntity() : this.IsMutable;
    }

    public virtual bool HasCollections => this.entityMetamodel.HasCollections;

    public virtual bool HasMutableProperties => this.entityMetamodel.HasMutableProperties;

    public virtual bool HasSubclasses => this.entityMetamodel.HasSubclasses;

    public virtual bool HasProxy => this.entityMetamodel.IsLazy;

    protected virtual bool UseDynamicUpdate => this.entityMetamodel.IsDynamicUpdate;

    protected virtual bool UseDynamicInsert => this.entityMetamodel.IsDynamicInsert;

    protected virtual bool HasEmbeddedCompositeIdentifier
    {
      get => this.entityMetamodel.IdentifierProperty.IsEmbedded;
    }

    public virtual bool CanExtractIdOutOfEntity
    {
      get
      {
        return this.HasIdentifierProperty || this.HasEmbeddedCompositeIdentifier || this.HasIdentifierMapper();
      }
    }

    private bool HasIdentifierMapper()
    {
      return this.entityMetamodel.IdentifierProperty.HasIdentifierMapper;
    }

    public bool ConsumesEntityAlias() => true;

    public bool ConsumesCollectionAlias() => false;

    public virtual IType GetPropertyType(string path) => this.propertyMapping.ToType(path);

    protected Versioning.OptimisticLock OptimisticLockMode
    {
      get => this.entityMetamodel.OptimisticLockMode;
    }

    public object CreateProxy(object id, ISessionImplementor session)
    {
      return this.entityMetamodel.GetTuplizer(session.EntityMode).CreateProxy(id, session);
    }

    public override string ToString()
    {
      return StringHelper.Unqualify(this.GetType().FullName) + (object) '(' + this.entityMetamodel.Name + (object) ')';
    }

    public string SelectFragment(
      IJoinable rhs,
      string rhsAlias,
      string lhsAlias,
      string entitySuffix,
      string collectionSuffix,
      bool includeCollectionColumns)
    {
      return this.SelectFragment(lhsAlias, entitySuffix);
    }

    public bool IsInstrumented(EntityMode entityMode)
    {
      IEntityTuplizer tuplizerOrNull = this.entityMetamodel.GetTuplizerOrNull(entityMode);
      return tuplizerOrNull != null && tuplizerOrNull.IsInstrumented;
    }

    public bool HasInsertGeneratedProperties => this.entityMetamodel.HasInsertGeneratedValues;

    public bool HasUpdateGeneratedProperties => this.entityMetamodel.HasUpdateGeneratedValues;

    public void AfterInitialize(
      object entity,
      bool lazyPropertiesAreUnfetched,
      ISessionImplementor session)
    {
      this.GetTuplizer(session).AfterInitialize(entity, lazyPropertiesAreUnfetched, session);
    }

    public virtual bool[] PropertyUpdateability => this.entityMetamodel.PropertyUpdateability;

    public System.Type GetMappedClass(EntityMode entityMode)
    {
      return this.entityMetamodel.GetTuplizerOrNull(entityMode)?.MappedClass;
    }

    public bool ImplementsLifecycle(EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).IsLifecycleImplementor;
    }

    public bool ImplementsValidatable(EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).IsValidatableImplementor;
    }

    public System.Type GetConcreteProxyClass(EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).ConcreteProxyClass;
    }

    public void SetPropertyValues(object obj, object[] values, EntityMode entityMode)
    {
      this.GetTuplizer(entityMode).SetPropertyValues(obj, values);
    }

    public void SetPropertyValue(object obj, int i, object value, EntityMode entityMode)
    {
      this.GetTuplizer(entityMode).SetPropertyValue(obj, i, value);
    }

    public object[] GetPropertyValues(object obj, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).GetPropertyValues(obj);
    }

    public object GetPropertyValue(object obj, int i, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).GetPropertyValue(obj, i);
    }

    public object GetPropertyValue(object obj, string propertyName, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).GetPropertyValue(obj, propertyName);
    }

    public virtual object GetIdentifier(object obj, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).GetIdentifier(obj);
    }

    public virtual void SetIdentifier(object obj, object id, EntityMode entityMode)
    {
      this.GetTuplizer(entityMode).SetIdentifier(obj, id);
    }

    public virtual object GetVersion(object obj, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).GetVersion(obj);
    }

    public virtual object Instantiate(object id, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).Instantiate(id);
    }

    public bool IsInstance(object entity, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).IsInstance(entity);
    }

    public virtual bool HasUninitializedLazyProperties(object obj, EntityMode entityMode)
    {
      return this.GetTuplizer(entityMode).HasUninitializedLazyProperties(obj);
    }

    public virtual void ResetIdentifier(
      object entity,
      object currentId,
      object currentVersion,
      EntityMode entityMode)
    {
      this.GetTuplizer(entityMode).ResetIdentifier(entity, currentId, currentVersion);
    }

    public IEntityPersister GetSubclassEntityPersister(
      object instance,
      ISessionFactoryImplementor factory,
      EntityMode entityMode)
    {
      if (!this.HasSubclasses)
        return (IEntityPersister) this;
      System.Type type = instance.GetType();
      if (type == this.GetMappedClass(entityMode))
        return (IEntityPersister) this;
      string subclassEntityName = this.GetSubclassEntityName(type);
      return subclassEntityName == null || this.EntityName.Equals(subclassEntityName) ? (IEntityPersister) this : factory.GetEntityPersister(subclassEntityName);
    }

    public virtual EntityMode? GuessEntityMode(object obj)
    {
      return this.entityMetamodel.GuessEntityMode(obj);
    }

    public virtual object[] GetPropertyValuesToInsert(
      object obj,
      IDictionary mergeMap,
      ISessionImplementor session)
    {
      return this.GetTuplizer(session.EntityMode).GetPropertyValuesToInsert(obj, mergeMap, session);
    }

    public void ProcessInsertGeneratedProperties(
      object id,
      object entity,
      object[] state,
      ISessionImplementor session)
    {
      if (!this.HasInsertGeneratedProperties)
        throw new AssertionFailure("no insert-generated properties");
      this.ProcessGeneratedProperties(id, entity, state, session, this.sqlInsertGeneratedValuesSelectString, this.PropertyInsertGenerationInclusions);
    }

    public void ProcessUpdateGeneratedProperties(
      object id,
      object entity,
      object[] state,
      ISessionImplementor session)
    {
      if (!this.HasUpdateGeneratedProperties)
        throw new AssertionFailure("no update-generated properties");
      this.ProcessGeneratedProperties(id, entity, state, session, this.sqlUpdateGeneratedValuesSelectString, this.PropertyUpdateGenerationInclusions);
    }

    private void ProcessGeneratedProperties(
      object id,
      object entity,
      object[] state,
      ISessionImplementor session,
      SqlString selectionSQL,
      ValueInclusion[] includeds)
    {
      session.Batcher.ExecuteBatch();
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          IDbCommand dbCommand = session.Batcher.PrepareQueryCommand(CommandType.Text, selectionSQL, this.IdentifierType.SqlTypes((IMapping) this.Factory));
          IDataReader dataReader = (IDataReader) null;
          try
          {
            this.IdentifierType.NullSafeSet(dbCommand, id, 0, session);
            dataReader = session.Batcher.ExecuteReader(dbCommand);
            if (!dataReader.Read())
              throw new HibernateException("Unable to locate row for retrieval of generated properties: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory));
            for (int i = 0; i < this.PropertySpan; ++i)
            {
              if (includeds[i] != ValueInclusion.None)
              {
                object obj = this.PropertyTypes[i].Hydrate(dataReader, this.GetPropertyAliases(string.Empty, i), session, entity);
                state[i] = this.PropertyTypes[i].ResolveIdentifier(obj, session, entity);
                this.SetPropertyValue(entity, i, state[i], session.EntityMode);
              }
            }
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, dataReader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
          {
            SqlException = (Exception) ex,
            Message = "unable to select generated column values",
            Sql = selectionSQL.ToString(),
            EntityName = this.EntityName,
            EntityId = id
          });
        }
      }
    }

    public bool HasSubselectLoadableCollections => this.hasSubselectLoadableCollections;

    public virtual object[] GetNaturalIdentifierSnapshot(object id, ISessionImplementor session)
    {
      if (!this.HasNaturalIdentifier)
        throw new MappingException("persistent class did not define a natural-id : " + MessageHelper.InfoString((IEntityPersister) this));
      if (AbstractEntityPersister.log.IsDebugEnabled)
        AbstractEntityPersister.log.Debug((object) ("Getting current natural-id snapshot state for: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory)));
      int[] identifierProperties = this.NaturalIdentifierProperties;
      int length = identifierProperties.Length;
      bool[] include = new bool[this.PropertySpan];
      IType[] typeArray = new IType[length];
      for (int index = 0; index < length; ++index)
      {
        typeArray[index] = this.PropertyTypes[identifierProperties[index]];
        include[identifierProperties[index]] = true;
      }
      SqlSelectBuilder sqlSelectBuilder = new SqlSelectBuilder(this.Factory);
      if (this.Factory.Settings.IsCommentsEnabled)
        sqlSelectBuilder.SetComment("get current natural-id state " + this.EntityName);
      sqlSelectBuilder.SetSelectClause(this.ConcretePropertySelectFragmentSansLeadingComma(this.RootAlias, include));
      sqlSelectBuilder.SetFromClause(this.FromTableFragment(this.RootAlias) + (object) this.FromJoinFragment(this.RootAlias, true, false));
      SqlString sqlString = new SqlStringBuilder().Add(StringHelper.Join(new SqlString(new object[3]
      {
        (object) "=",
        (object) Parameter.Placeholder,
        (object) " and "
      }), (IEnumerable) StringHelper.Qualify(this.RootAlias, this.IdentifierColumnNames))).Add("=").AddParameter().Add(this.WhereJoinFragment(this.RootAlias, true, false)).ToSqlString();
      SqlString statementString = sqlSelectBuilder.SetOuterJoins(SqlString.Empty, SqlString.Empty).SetWhereClause(sqlString).ToStatementString();
      object[] identifierSnapshot = new object[length];
      using (new SessionIdLoggingContext(session.SessionId))
      {
        try
        {
          IDbCommand dbCommand = session.Batcher.PrepareCommand(CommandType.Text, statementString, this.IdentifierType.SqlTypes((IMapping) this.factory));
          IDataReader dataReader = (IDataReader) null;
          try
          {
            this.IdentifierType.NullSafeSet(dbCommand, id, 0, session);
            dataReader = session.Batcher.ExecuteReader(dbCommand);
            if (!dataReader.Read())
              return (object[]) null;
            for (int index = 0; index < length; ++index)
            {
              identifierSnapshot[index] = typeArray[index].Hydrate(dataReader, this.GetPropertyAliases(string.Empty, identifierProperties[index]), session, (object) null);
              if (typeArray[index].IsEntityType)
                identifierSnapshot[index] = typeArray[index].ResolveIdentifier(identifierSnapshot[index], session, (object) null);
            }
            return identifierSnapshot;
          }
          finally
          {
            session.Batcher.CloseCommand(dbCommand, dataReader);
          }
        }
        catch (DbException ex)
        {
          throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, new AdoExceptionContextInfo()
          {
            SqlException = (Exception) ex,
            Message = "could not retrieve snapshot: " + MessageHelper.InfoString((IEntityPersister) this, id, this.Factory),
            Sql = statementString.ToString(),
            EntityName = this.EntityName,
            EntityId = id
          });
        }
      }
    }

    protected string ConcretePropertySelectFragmentSansLeadingComma(string alias, bool[] include)
    {
      string str = this.ConcretePropertySelectFragment(alias, include);
      if (str.IndexOf(", ") == 0)
        str = str.Substring(2);
      return str;
    }

    public virtual bool HasNaturalIdentifier => this.entityMetamodel.HasNaturalIdentifier;

    public virtual void SetPropertyValue(
      object obj,
      string propertyName,
      object value,
      EntityMode entityMode)
    {
      this.GetTuplizer(entityMode).SetPropertyValue(obj, propertyName, value);
    }

    public abstract string GetPropertyTableName(string propertyName);

    public abstract string FromTableFragment(string alias);

    public abstract string GetSubclassForDiscriminatorValue(object value);

    public abstract string GetSubclassPropertyTableName(int i);

    public abstract string TableName { get; }

    public bool? IsUnsavedVersion(object version)
    {
      return !this.IsVersioned ? new bool?(false) : this.entityMetamodel.VersionProperty.UnsavedValue.IsUnsaved(version);
    }

    public virtual SqlType[] IdAndVersionSqlTypes
    {
      get
      {
        if (this.idAndVersionSqlTypes == null)
          this.idAndVersionSqlTypes = this.IsVersioned ? ArrayHelper.Join(this.IdentifierType.SqlTypes((IMapping) this.factory), this.VersionType.SqlTypes((IMapping) this.factory)) : this.IdentifierType.SqlTypes((IMapping) this.factory);
        return this.idAndVersionSqlTypes;
      }
    }

    public string GetInfoString() => MessageHelper.InfoString((IEntityPersister) this);

    protected internal interface IInclusionChecker
    {
      bool IncludeProperty(int propertyNumber);
    }

    private class NoneInclusionChecker : AbstractEntityPersister.IInclusionChecker
    {
      private readonly ValueInclusion[] inclusions;

      public NoneInclusionChecker(ValueInclusion[] inclusions) => this.inclusions = inclusions;

      public bool IncludeProperty(int propertyNumber)
      {
        return this.inclusions[propertyNumber] != ValueInclusion.None;
      }
    }

    private class FullInclusionChecker : AbstractEntityPersister.IInclusionChecker
    {
      private readonly bool[] includeProperty;

      public FullInclusionChecker(bool[] includeProperty) => this.includeProperty = includeProperty;

      public bool IncludeProperty(int propertyNumber) => this.includeProperty[propertyNumber];
    }

    private class GeneratedIdentifierBinder : IBinder
    {
      private readonly object[] fields;
      private readonly bool[] notNull;
      private readonly ISessionImplementor session;
      private readonly object entity;
      private readonly AbstractEntityPersister entityPersister;

      public GeneratedIdentifierBinder(
        object[] fields,
        bool[] notNull,
        ISessionImplementor session,
        object entity,
        AbstractEntityPersister entityPersister)
      {
        this.fields = fields;
        this.notNull = notNull;
        this.session = session;
        this.entity = entity;
        this.entityPersister = entityPersister;
      }

      public object Entity => this.entity;

      public virtual void BindValues(IDbCommand ps)
      {
        this.entityPersister.Dehydrate((object) null, this.fields, this.notNull, this.entityPersister.propertyColumnInsertable, 0, ps, this.session);
      }
    }
  }
}
