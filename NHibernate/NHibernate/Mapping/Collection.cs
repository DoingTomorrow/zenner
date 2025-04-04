// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Collection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public abstract class Collection : IFetchable, IValue, IFilterable
  {
    public const string DefaultElementColumnName = "elt";
    public const string DefaultKeyColumnName = "id";
    private static readonly IEnumerable<ISelectable> EmptyColumns = (IEnumerable<ISelectable>) new ISelectable[0];
    private IKeyValue key;
    private IValue element;
    private string elementNodeName;
    private Table collectionTable;
    private string role;
    private bool lazy;
    private bool extraLazy;
    private bool inverse;
    private bool mutable = true;
    private string cacheConcurrencyStrategy;
    private string cacheRegionName;
    private string orderBy;
    private string where;
    private PersistentClass owner;
    private bool sorted;
    private object comparer;
    private bool orphanDelete;
    private int batchSize = -1;
    private FetchMode fetchMode;
    private System.Type collectionPersisterClass;
    private string referencedPropertyName;
    private string typeName;
    private bool embedded = true;
    private string nodeName;
    private string loaderName;
    private SqlString customSQLInsert;
    private bool customInsertCallable;
    private ExecuteUpdateResultCheckStyle insertCheckStyle;
    private SqlString customSQLDelete;
    private bool customDeleteCallable;
    private ExecuteUpdateResultCheckStyle deleteCheckStyle;
    private SqlString customSQLUpdate;
    private bool customUpdateCallable;
    private ExecuteUpdateResultCheckStyle updateCheckStyle;
    private SqlString customSQLDeleteAll;
    private bool customDeleteAllCallable;
    private ExecuteUpdateResultCheckStyle deleteAllCheckStyle;
    private bool isGeneric;
    private System.Type[] genericArguments;
    private readonly Dictionary<string, string> filters = new Dictionary<string, string>();
    private readonly Dictionary<string, string> manyToManyFilters = new Dictionary<string, string>();
    private bool subselectLoadable;
    private string manyToManyWhere;
    private string manyToManyOrderBy;
    private bool optimisticLocked;
    private readonly HashedSet<string> synchronizedTables = new HashedSet<string>();
    private IDictionary<string, string> typeParameters;

    protected Collection(PersistentClass owner) => this.owner = owner;

    public int ColumnSpan => 0;

    public virtual bool IsSet => false;

    public IKeyValue Key
    {
      get => this.key;
      set => this.key = value;
    }

    public IValue Element
    {
      get => this.element;
      set => this.element = value;
    }

    public virtual bool IsIndexed => false;

    public Table CollectionTable
    {
      get => this.collectionTable;
      set => this.collectionTable = value;
    }

    public Table Table => this.Owner.Table;

    public bool IsSorted
    {
      get => this.sorted;
      set => this.sorted = value;
    }

    public bool HasOrder => this.orderBy != null || this.manyToManyOrderBy != null;

    public PersistentClass Owner
    {
      get => this.owner;
      set => this.owner = value;
    }

    public System.Type CollectionPersisterClass
    {
      get => this.collectionPersisterClass;
      set => this.collectionPersisterClass = value;
    }

    public object Comparer
    {
      get
      {
        if (this.comparer == null)
        {
          if (!string.IsNullOrEmpty(this.ComparerClassName))
          {
            try
            {
              this.comparer = NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(this.ComparerClassName));
            }
            catch
            {
              throw new MappingException("Could not instantiate comparator class [" + this.ComparerClassName + "] for collection " + this.Role);
            }
          }
        }
        return this.comparer;
      }
      set => this.comparer = value;
    }

    public string ComparerClassName { get; set; }

    public bool IsLazy
    {
      get => this.lazy;
      set => this.lazy = value;
    }

    public string Role
    {
      get => this.role;
      set => this.role = StringHelper.InternedIfPossible(value);
    }

    public IEnumerable<ISelectable> ColumnIterator => Collection.EmptyColumns;

    public Formula Formula => (Formula) null;

    public bool IsNullable => true;

    public bool IsUnique => false;

    public virtual CollectionType CollectionType
    {
      get
      {
        return this.typeName == null ? this.DefaultCollectionType : TypeFactory.CustomCollection(this.typeName, this.typeParameters, this.role, this.referencedPropertyName, this.Embedded);
      }
    }

    public abstract CollectionType DefaultCollectionType { get; }

    public IType Type => (IType) this.CollectionType;

    public virtual bool IsPrimitiveArray => false;

    public virtual bool IsArray => false;

    public virtual bool HasFormula => false;

    public virtual bool IsIdentified => false;

    public bool IsOneToMany => this.element is OneToMany;

    public string CacheConcurrencyStrategy
    {
      get => this.cacheConcurrencyStrategy;
      set => this.cacheConcurrencyStrategy = value;
    }

    public string CacheRegionName
    {
      get => this.cacheRegionName ?? this.Role;
      set => this.cacheRegionName = value;
    }

    public bool IsInverse
    {
      get => this.inverse;
      set => this.inverse = value;
    }

    public string OwnerEntityName => this.owner.EntityName;

    public string OrderBy
    {
      get => this.orderBy;
      set => this.orderBy = value;
    }

    public string Where
    {
      get => this.where;
      set => this.where = value;
    }

    public bool HasOrphanDelete
    {
      get => this.orphanDelete;
      set => this.orphanDelete = value;
    }

    public int BatchSize
    {
      get => this.batchSize;
      set => this.batchSize = value;
    }

    public FetchMode FetchMode
    {
      get => this.fetchMode;
      set => this.fetchMode = value;
    }

    public bool IsGeneric
    {
      get => this.isGeneric;
      set => this.isGeneric = value;
    }

    public System.Type[] GenericArguments
    {
      get => this.genericArguments;
      set => this.genericArguments = value;
    }

    protected void CheckGenericArgumentsLength(int expectedLength)
    {
      if (this.genericArguments.Length != expectedLength)
        throw new MappingException(string.Format("Error mapping generic collection {0}: expected {1} generic parameters, but the property type has {2}", (object) this.Role, (object) expectedLength, (object) this.genericArguments.Length));
    }

    public void CreateForeignKey()
    {
    }

    private void CreateForeignKeys()
    {
      if (!string.IsNullOrEmpty(this.referencedPropertyName))
        return;
      this.Element.CreateForeignKey();
      this.key.CreateForeignKeyOfEntity(this.Owner.EntityName);
    }

    public abstract void CreatePrimaryKey();

    public virtual void CreateAllKeys()
    {
      this.CreateForeignKeys();
      if (this.IsInverse)
        return;
      this.CreatePrimaryKey();
    }

    public bool IsSimpleValue => false;

    public bool IsValid(IMapping mapping) => true;

    public string ReferencedPropertyName
    {
      get => this.referencedPropertyName;
      set => this.referencedPropertyName = StringHelper.InternedIfPossible(value);
    }

    public virtual void Validate(IMapping mapping)
    {
      if (this.Key.IsCascadeDeleteEnabled && (!this.IsInverse || !this.IsOneToMany))
        throw new MappingException(string.Format("only inverse one-to-many associations may use on-delete=\"cascade\": {0}", (object) this.Role));
      if (!this.Key.IsValid(mapping))
        throw new MappingException(string.Format("collection foreign key mapping has wrong number of columns: {0} type: {1}", (object) this.Role, (object) this.Key.Type.Name));
      if (!this.Element.IsValid(mapping))
        throw new MappingException(string.Format("collection element mapping has wrong number of columns: {0} type: {1}", (object) this.Role, (object) this.Element.Type.Name));
      this.CheckColumnDuplication();
      if (this.elementNodeName != null && this.elementNodeName.StartsWith("@"))
        throw new MappingException(string.Format("element node must not be an attribute: {0}", (object) this.elementNodeName));
      if (this.elementNodeName != null && this.elementNodeName.Equals("."))
        throw new MappingException(string.Format("element node must not be the parent: {0}", (object) this.elementNodeName));
      if (this.nodeName != null && this.nodeName.IndexOf('@') > -1)
        throw new MappingException(string.Format("collection node must not be an attribute: {0}", (object) this.elementNodeName));
    }

    public bool[] ColumnInsertability => ArrayHelper.EmptyBoolArray;

    public bool[] ColumnUpdateability => ArrayHelper.EmptyBoolArray;

    public string TypeName
    {
      get => this.typeName;
      set => this.typeName = value;
    }

    public SqlString CustomSQLInsert => this.customSQLInsert;

    public SqlString CustomSQLDelete => this.customSQLDelete;

    public SqlString CustomSQLUpdate => this.customSQLUpdate;

    public SqlString CustomSQLDeleteAll => this.customSQLDeleteAll;

    public bool IsCustomInsertCallable => this.customInsertCallable;

    public bool IsCustomDeleteCallable => this.customDeleteCallable;

    public bool IsCustomUpdateCallable => this.customUpdateCallable;

    public bool IsCustomDeleteAllCallable => this.customDeleteAllCallable;

    public ExecuteUpdateResultCheckStyle CustomSQLInsertCheckStyle => this.insertCheckStyle;

    public ExecuteUpdateResultCheckStyle CustomSQLDeleteCheckStyle => this.deleteCheckStyle;

    public ExecuteUpdateResultCheckStyle CustomSQLUpdateCheckStyle => this.updateCheckStyle;

    public ExecuteUpdateResultCheckStyle CustomSQLDeleteAllCheckStyle => this.deleteAllCheckStyle;

    public void SetCustomSQLInsert(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLInsert = SqlString.Parse(sql);
      this.customInsertCallable = callable;
      this.insertCheckStyle = checkStyle;
    }

    public void SetCustomSQLDelete(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLDelete = SqlString.Parse(sql);
      this.customDeleteCallable = callable;
      this.deleteCheckStyle = checkStyle;
    }

    public void SetCustomSQLDeleteAll(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLDeleteAll = SqlString.Parse(sql);
      this.customDeleteAllCallable = callable;
      this.deleteAllCheckStyle = checkStyle;
    }

    public void SetCustomSQLUpdate(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLUpdate = SqlString.Parse(sql);
      this.customUpdateCallable = callable;
      this.updateCheckStyle = checkStyle;
    }

    public void AddFilter(string name, string condition) => this.filters[name] = condition;

    public IDictionary<string, string> FilterMap => (IDictionary<string, string>) this.filters;

    public void AddManyToManyFilter(string name, string condition)
    {
      this.manyToManyFilters[name] = condition;
    }

    public IDictionary<string, string> ManyToManyFilterMap
    {
      get => (IDictionary<string, string>) this.manyToManyFilters;
    }

    public string LoaderName
    {
      get => this.loaderName;
      set => this.loaderName = StringHelper.InternedIfPossible(value);
    }

    public bool IsSubselectLoadable
    {
      get => this.subselectLoadable;
      set => this.subselectLoadable = value;
    }

    public string ManyToManyWhere
    {
      get => this.manyToManyWhere;
      set => this.manyToManyWhere = value;
    }

    public string ManyToManyOrdering
    {
      get => this.manyToManyOrderBy;
      set => this.manyToManyOrderBy = value;
    }

    public bool IsOptimisticLocked
    {
      get => this.optimisticLocked;
      set => this.optimisticLocked = value;
    }

    public string ElementNodeName
    {
      get => this.elementNodeName;
      set => this.elementNodeName = value;
    }

    public bool Embedded
    {
      get => this.embedded;
      set => this.embedded = value;
    }

    public bool ExtraLazy
    {
      get => this.extraLazy;
      set => this.extraLazy = value;
    }

    private void CheckColumnDuplication()
    {
      HashedSet<string> distinctColumns = new HashedSet<string>();
      this.CheckColumnDuplication((ISet<string>) distinctColumns, this.Key.ColumnIterator);
      if (this.IsIndexed)
        this.CheckColumnDuplication((ISet<string>) distinctColumns, ((IndexedCollection) this).Index.ColumnIterator);
      if (this.IsIdentified)
        this.CheckColumnDuplication((ISet<string>) distinctColumns, ((IdentifierCollection) this).Identifier.ColumnIterator);
      if (this.IsOneToMany)
        return;
      this.CheckColumnDuplication((ISet<string>) distinctColumns, this.Element.ColumnIterator);
    }

    private void CheckColumnDuplication(
      ISet<string> distinctColumns,
      IEnumerable<ISelectable> columns)
    {
      foreach (ISelectable column1 in columns)
      {
        if (!column1.IsFormula)
        {
          Column column2 = (Column) column1;
          if (!distinctColumns.Add(column2.Name))
            throw new MappingException(string.Format("Repeated column in mapping for collection: {0} column: {1}", (object) this.Role, (object) column2.Name));
        }
      }
    }

    public virtual bool IsAlternateUniqueKey => false;

    public void SetTypeUsingReflection(string className, string propertyName, string access)
    {
    }

    public object Accept(IValueVisitor visitor) => visitor.Accept((IValue) this);

    public virtual bool IsMap => false;

    public bool IsMutable
    {
      get => this.mutable;
      set => this.mutable = value;
    }

    public string NodeName
    {
      get => this.nodeName;
      set => this.nodeName = value;
    }

    public ISet<string> SynchronizedTables => (ISet<string>) this.synchronizedTables;

    public IDictionary<string, string> TypeParameters
    {
      get => this.typeParameters;
      set => this.typeParameters = value;
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.Role + (object) ')';
    }
  }
}
