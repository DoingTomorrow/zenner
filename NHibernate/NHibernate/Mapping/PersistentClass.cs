// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.PersistentClass
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public abstract class PersistentClass : IFilterable, IMetaAttributable, ISqlCustomizable
  {
    public const string NullDiscriminatorMapping = "null";
    public const string NotNullDiscriminatorMapping = "not null";
    private static readonly Alias PKAlias = new Alias(15, "PK");
    private string entityName;
    private string className;
    private string proxyInterfaceName;
    private string nodeName;
    private string discriminatorValue;
    private bool lazy;
    private readonly List<Property> properties = new List<Property>();
    private readonly List<Subclass> subclasses = new List<Subclass>();
    private readonly List<Property> subclassProperties = new List<Property>();
    private readonly List<Table> subclassTables = new List<Table>();
    private bool dynamicInsert;
    private bool dynamicUpdate;
    private int? batchSize;
    private bool selectBeforeUpdate;
    private IDictionary<string, MetaAttribute> metaAttributes;
    private readonly List<Join> joins = new List<Join>();
    private readonly List<Join> subclassJoins = new List<Join>();
    private readonly IDictionary<string, string> filters = (IDictionary<string, string>) new Dictionary<string, string>();
    private readonly ISet<string> synchronizedTables = (ISet<string>) new HashedSet<string>();
    private string loaderName;
    private bool? isAbstract;
    private bool hasSubselectLoadableCollections;
    private Component identifierMapper;
    private SqlString customSQLInsert;
    private bool customInsertCallable;
    private ExecuteUpdateResultCheckStyle insertCheckStyle;
    private SqlString customSQLUpdate;
    private bool customUpdateCallable;
    private ExecuteUpdateResultCheckStyle updateCheckStyle;
    private SqlString customSQLDelete;
    private bool customDeleteCallable;
    private ExecuteUpdateResultCheckStyle deleteCheckStyle;
    private string temporaryIdTableName;
    private string temporaryIdTableDDL;
    private IDictionary<EntityMode, string> tuplizerImpls;
    private Versioning.OptimisticLock optimisticLockMode;
    private Type mappedClass;
    private Type proxyInterface;

    public string ClassName
    {
      get => this.className;
      set
      {
        this.className = value == null ? (string) null : string.Intern(value);
        this.mappedClass = (Type) null;
      }
    }

    public string ProxyInterfaceName
    {
      get => this.proxyInterfaceName;
      set
      {
        this.proxyInterfaceName = value;
        this.proxyInterface = (Type) null;
      }
    }

    public virtual Type MappedClass
    {
      get
      {
        if (this.mappedClass == null)
        {
          if (this.className == null)
            return (Type) null;
          try
          {
            this.mappedClass = ReflectHelper.ClassForName(this.className);
          }
          catch (Exception ex)
          {
            throw new MappingException("entity class not found: " + this.className, ex);
          }
        }
        return this.mappedClass;
      }
    }

    public virtual Type ProxyInterface
    {
      get
      {
        if (this.proxyInterface == null)
        {
          if (this.proxyInterfaceName == null)
            return (Type) null;
          try
          {
            this.proxyInterface = ReflectHelper.ClassForName(this.proxyInterfaceName);
          }
          catch (Exception ex)
          {
            throw new MappingException("proxy class not found: " + this.proxyInterfaceName, ex);
          }
        }
        return this.proxyInterface;
      }
    }

    public abstract int SubclassId { get; }

    public virtual bool DynamicInsert
    {
      get => this.dynamicInsert;
      set => this.dynamicInsert = value;
    }

    public virtual bool DynamicUpdate
    {
      get => this.dynamicUpdate;
      set => this.dynamicUpdate = value;
    }

    public virtual string DiscriminatorValue
    {
      get => this.discriminatorValue;
      set => this.discriminatorValue = value;
    }

    public virtual int SubclassSpan
    {
      get
      {
        int count = this.subclasses.Count;
        foreach (Subclass subclass in this.subclasses)
          count += subclass.SubclassSpan;
        return count;
      }
    }

    public virtual IEnumerable<Subclass> SubclassIterator
    {
      get
      {
        IEnumerable<Subclass>[] enumerables = new IEnumerable<Subclass>[this.subclasses.Count + 1];
        int index = 0;
        foreach (Subclass subclass in this.subclasses)
          enumerables[index++] = subclass.SubclassIterator;
        enumerables[index] = (IEnumerable<Subclass>) this.subclasses;
        return (IEnumerable<Subclass>) new JoinedEnumerable<Subclass>(enumerables);
      }
    }

    public virtual IEnumerable<PersistentClass> SubclassClosureIterator
    {
      get
      {
        List<IEnumerable<PersistentClass>> enumerables = new List<IEnumerable<PersistentClass>>();
        enumerables.Add((IEnumerable<PersistentClass>) new SingletonEnumerable<PersistentClass>(this));
        foreach (Subclass subclass in this.SubclassIterator)
          enumerables.Add(subclass.SubclassClosureIterator);
        return (IEnumerable<PersistentClass>) new JoinedEnumerable<PersistentClass>(enumerables);
      }
    }

    public virtual Table IdentityTable => this.RootTable;

    public virtual IEnumerable<Subclass> DirectSubclasses
    {
      get => (IEnumerable<Subclass>) this.subclasses;
    }

    public virtual string EntityName
    {
      get => this.entityName;
      set => this.entityName = value == null ? (string) null : string.Intern(value);
    }

    public abstract bool IsInherited { get; }

    public abstract bool IsVersioned { get; }

    public abstract IEnumerable<Property> PropertyClosureIterator { get; }

    public abstract IEnumerable<Table> TableClosureIterator { get; }

    public abstract IEnumerable<IKeyValue> KeyClosureIterator { get; }

    public virtual IEnumerable<Property> SubclassPropertyClosureIterator
    {
      get
      {
        List<IEnumerable<Property>> enumerables = new List<IEnumerable<Property>>();
        enumerables.Add(this.PropertyClosureIterator);
        enumerables.Add((IEnumerable<Property>) this.subclassProperties);
        foreach (Join subclassJoin in this.subclassJoins)
          enumerables.Add(subclassJoin.PropertyIterator);
        return (IEnumerable<Property>) new JoinedEnumerable<Property>(enumerables);
      }
    }

    public virtual IEnumerable<Join> SubclassJoinClosureIterator
    {
      get
      {
        return (IEnumerable<Join>) new JoinedEnumerable<Join>(this.JoinClosureIterator, (IEnumerable<Join>) this.subclassJoins);
      }
    }

    public virtual IEnumerable<Table> SubclassTableClosureIterator
    {
      get
      {
        return (IEnumerable<Table>) new JoinedEnumerable<Table>(this.TableClosureIterator, (IEnumerable<Table>) this.subclassTables);
      }
    }

    public bool IsLazy
    {
      get => this.lazy;
      set => this.lazy = value;
    }

    public abstract Type EntityPersisterClass { get; set; }

    public abstract Table RootTable { get; }

    public int? BatchSize
    {
      get => this.batchSize;
      set => this.batchSize = value;
    }

    public bool SelectBeforeUpdate
    {
      get => this.selectBeforeUpdate;
      set => this.selectBeforeUpdate = value;
    }

    public virtual IEnumerable<Property> ReferenceablePropertyIterator
    {
      get => this.PropertyClosureIterator;
    }

    public bool IsDiscriminatorValueNotNull => "not null".Equals(this.DiscriminatorValue);

    public bool IsDiscriminatorValueNull => "null".Equals(this.DiscriminatorValue);

    public IDictionary<string, MetaAttribute> MetaAttributes
    {
      get => this.metaAttributes;
      set => this.metaAttributes = value;
    }

    public virtual IEnumerable<Join> JoinIterator => (IEnumerable<Join>) this.joins;

    public virtual IEnumerable<Join> JoinClosureIterator => (IEnumerable<Join>) this.joins;

    public virtual int JoinClosureSpan => this.joins.Count;

    public virtual int PropertyClosureSpan
    {
      get
      {
        int count = this.properties.Count;
        foreach (Join join in this.joins)
          count += join.PropertySpan;
        return count;
      }
    }

    public virtual IEnumerable<Property> PropertyIterator
    {
      get
      {
        List<IEnumerable<Property>> enumerables = new List<IEnumerable<Property>>();
        enumerables.Add((IEnumerable<Property>) this.properties);
        foreach (Join join in this.joins)
          enumerables.Add(join.PropertyIterator);
        return (IEnumerable<Property>) new JoinedEnumerable<Property>(enumerables);
      }
    }

    public virtual IEnumerable<Property> UnjoinedPropertyIterator
    {
      get => (IEnumerable<Property>) this.properties;
    }

    public bool IsCustomInsertCallable => this.customInsertCallable;

    public ExecuteUpdateResultCheckStyle CustomSQLInsertCheckStyle => this.insertCheckStyle;

    public bool IsCustomUpdateCallable => this.customUpdateCallable;

    public ExecuteUpdateResultCheckStyle CustomSQLUpdateCheckStyle => this.updateCheckStyle;

    public bool IsCustomDeleteCallable => this.customDeleteCallable;

    public ExecuteUpdateResultCheckStyle CustomSQLDeleteCheckStyle => this.deleteCheckStyle;

    public virtual IDictionary<string, string> FilterMap => this.filters;

    public abstract bool IsJoinedSubclass { get; }

    public string LoaderName
    {
      get => this.loaderName;
      set => this.loaderName = value == null ? (string) null : string.Intern(value);
    }

    public virtual ISet<string> SynchronizedTables => this.synchronizedTables;

    protected internal virtual IEnumerable<Property> NonDuplicatedPropertyIterator
    {
      get => this.UnjoinedPropertyIterator;
    }

    protected internal virtual IEnumerable<ISelectable> DiscriminatorColumnIterator
    {
      get => (IEnumerable<ISelectable>) new CollectionHelper.EmptyEnumerableClass<ISelectable>();
    }

    public string NodeName
    {
      get => this.nodeName;
      set => this.nodeName = value;
    }

    public virtual bool HasSubselectLoadableCollections
    {
      get => this.hasSubselectLoadableCollections;
      set => this.hasSubselectLoadableCollections = value;
    }

    public string TemporaryIdTableName => this.temporaryIdTableName;

    public string TemporaryIdTableDDL => this.temporaryIdTableDDL;

    public virtual IDictionary<EntityMode, string> TuplizerMap
    {
      get
      {
        return this.tuplizerImpls != null ? (IDictionary<EntityMode, string>) new UnmodifiableDictionary<EntityMode, string>(this.tuplizerImpls) : (IDictionary<EntityMode, string>) null;
      }
    }

    internal abstract int NextSubclassId();

    public virtual void AddSubclass(Subclass subclass)
    {
      for (PersistentClass superclass = this.Superclass; superclass != null; superclass = superclass.Superclass)
      {
        if (subclass.EntityName.Equals(superclass.EntityName))
          throw new MappingException(string.Format("Circular inheritance mapping detected: {0} will have itself as superclass when extending {1}", (object) subclass.EntityName, (object) this.EntityName));
      }
      this.subclasses.Add(subclass);
    }

    public virtual bool HasSubclasses => this.subclasses.Count > 0;

    public virtual void AddProperty(Property p)
    {
      this.properties.Add(p);
      p.PersistentClass = this;
    }

    public abstract Table Table { get; }

    public abstract bool IsMutable { get; set; }

    public abstract bool HasIdentifierProperty { get; }

    public abstract Property IdentifierProperty { get; set; }

    public abstract IKeyValue Identifier { get; set; }

    public abstract Property Version { get; set; }

    public abstract IValue Discriminator { get; set; }

    public abstract bool IsPolymorphic { get; set; }

    public abstract string CacheConcurrencyStrategy { get; set; }

    public abstract PersistentClass Superclass { get; set; }

    public abstract bool IsExplicitPolymorphism { get; set; }

    public abstract bool IsDiscriminatorInsertable { get; set; }

    public virtual void AddSubclassProperty(Property p) => this.subclassProperties.Add(p);

    public virtual void AddSubclassJoin(Join join) => this.subclassJoins.Add(join);

    public virtual void AddSubclassTable(Table table) => this.subclassTables.Add(table);

    public virtual bool IsClassOrSuperclassJoin(Join join) => this.joins.Contains(join);

    public virtual bool IsClassOrSuperclassTable(Table closureTable) => this.Table == closureTable;

    public abstract bool HasEmbeddedIdentifier { get; set; }

    public abstract RootClass RootClazz { get; }

    public abstract IKeyValue Key { get; set; }

    public virtual void CreatePrimaryKey(NHibernate.Dialect.Dialect dialect)
    {
      PrimaryKey primaryKey = new PrimaryKey();
      Table table = this.Table;
      primaryKey.Table = table;
      primaryKey.Name = PersistentClass.PKAlias.ToAliasString(table.Name, dialect);
      table.PrimaryKey = primaryKey;
      primaryKey.AddColumns((IEnumerable<Column>) new SafetyEnumerable<Column>((IEnumerable) this.Key.ColumnIterator));
    }

    public abstract string Where { get; set; }

    public Property GetReferencedProperty(string propertyPath)
    {
      try
      {
        return this.GetRecursiveProperty(propertyPath, this.ReferenceablePropertyIterator);
      }
      catch (MappingException ex)
      {
        throw new MappingException("property-ref [" + propertyPath + "] not found on entity [" + this.EntityName + "]", (Exception) ex);
      }
    }

    public Property GetRecursiveProperty(string propertyPath)
    {
      try
      {
        return this.GetRecursiveProperty(propertyPath, this.PropertyIterator);
      }
      catch (MappingException ex)
      {
        throw new MappingException("property [" + propertyPath + "] not found on entity [" + this.EntityName + "]", (Exception) ex);
      }
    }

    private Property GetRecursiveProperty(string propertyPath, IEnumerable<Property> iter)
    {
      Property recursiveProperty = (Property) null;
      StringTokenizer stringTokenizer = new StringTokenizer(propertyPath, ".", false);
      try
      {
        foreach (string propertyName in stringTokenizer)
        {
          if (recursiveProperty == null)
          {
            Property identifierProperty = this.IdentifierProperty;
            if (identifierProperty != null && identifierProperty.Name.Equals(propertyName))
              recursiveProperty = identifierProperty;
            else if (identifierProperty == null && this.Identifier != null)
            {
              if (typeof (Component).IsInstanceOfType((object) this.Identifier))
              {
                try
                {
                  Property property = this.GetProperty(propertyName, ((Component) this.Identifier).PropertyIterator);
                  if (property != null)
                    recursiveProperty = property;
                }
                catch (MappingException ex)
                {
                }
              }
            }
            if (recursiveProperty == null)
              recursiveProperty = this.GetProperty(propertyName, iter);
          }
          else
            recursiveProperty = ((Component) recursiveProperty.Value).GetProperty(propertyName);
        }
      }
      catch (MappingException ex)
      {
        throw new MappingException("property [" + propertyPath + "] not found on entity [" + this.EntityName + "]");
      }
      return recursiveProperty;
    }

    private Property GetProperty(string propertyName, IEnumerable<Property> iter)
    {
      string str = StringHelper.Root(propertyName);
      foreach (Property property in iter)
      {
        if (property.Name.Equals(str))
          return property;
      }
      throw new MappingException(string.Format("property not found: {0} on entity {1}", (object) propertyName, (object) this.EntityName));
    }

    public Property GetProperty(string propertyName)
    {
      IEnumerable<Property> propertyClosureIterator = this.PropertyClosureIterator;
      Property identifierProperty = this.IdentifierProperty;
      return identifierProperty != null && identifierProperty.Name.Equals(StringHelper.Root(propertyName)) ? identifierProperty : this.GetProperty(propertyName, propertyClosureIterator);
    }

    public virtual Versioning.OptimisticLock OptimisticLockMode
    {
      get => this.optimisticLockMode;
      set => this.optimisticLockMode = value;
    }

    public virtual void Validate(IMapping mapping)
    {
      foreach (Property property in this.PropertyIterator)
      {
        if (!property.IsValid(mapping))
          throw new MappingException(string.Format("property mapping has wrong number of columns: {0} type: {1}", (object) StringHelper.Qualify(this.EntityName, property.Name), (object) property.Type.Name));
      }
      this.CheckPropertyDuplication();
      this.CheckColumnDuplication();
    }

    private void CheckPropertyDuplication()
    {
      HashedSet<string> hashedSet = new HashedSet<string>();
      foreach (Property property in this.PropertyIterator)
      {
        if (!hashedSet.Add(property.Name))
          throw new MappingException("Duplicate property mapping of " + property.Name + " found in " + this.EntityName);
      }
    }

    public MetaAttribute GetMetaAttribute(string attributeName)
    {
      if (this.metaAttributes == null)
        return (MetaAttribute) null;
      MetaAttribute metaAttribute;
      this.metaAttributes.TryGetValue(attributeName, out metaAttribute);
      return metaAttribute;
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.EntityName + (object) ')';
    }

    public virtual void AddJoin(Join join)
    {
      this.joins.Add(join);
      join.PersistentClass = this;
    }

    public virtual int GetJoinNumber(Property prop)
    {
      int joinNumber = 1;
      foreach (Join join in this.SubclassJoinClosureIterator)
      {
        if (join.ContainsProperty(prop))
          return joinNumber;
        ++joinNumber;
      }
      return 0;
    }

    public void SetCustomSQLInsert(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLInsert = SqlString.Parse(sql);
      this.customInsertCallable = callable;
      this.insertCheckStyle = checkStyle;
    }

    public SqlString CustomSQLInsert => this.customSQLInsert;

    public void SetCustomSQLUpdate(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLUpdate = SqlString.Parse(sql);
      this.customUpdateCallable = callable;
      this.updateCheckStyle = checkStyle;
    }

    public SqlString CustomSQLUpdate => this.customSQLUpdate;

    public void SetCustomSQLDelete(
      string sql,
      bool callable,
      ExecuteUpdateResultCheckStyle checkStyle)
    {
      this.customSQLDelete = SqlString.Parse(sql);
      this.customDeleteCallable = callable;
      this.deleteCheckStyle = checkStyle;
    }

    public SqlString CustomSQLDelete => this.customSQLDelete;

    public void AddFilter(string name, string condition) => this.filters.Add(name, condition);

    public virtual bool IsForceDiscriminator
    {
      get => false;
      set => throw new NotImplementedException("subclasses need to override this method");
    }

    public void AddSynchronizedTable(string table) => this.synchronizedTables.Add(table);

    public bool? IsAbstract
    {
      get => this.isAbstract;
      set => this.isAbstract = value;
    }

    protected internal void CheckColumnDuplication(
      ISet<string> distinctColumns,
      IEnumerable<ISelectable> columns)
    {
      foreach (ISelectable column1 in columns)
      {
        if (!column1.IsFormula)
        {
          Column column2 = (Column) column1;
          distinctColumns.Add(column2.Name);
        }
      }
    }

    protected internal void CheckPropertyColumnDuplication(
      ISet<string> distinctColumns,
      IEnumerable<Property> properties)
    {
      foreach (Property property in properties)
      {
        if (property.Value is Component component)
          this.CheckPropertyColumnDuplication(distinctColumns, component.PropertyIterator);
        else if (property.IsUpdateable || property.IsInsertable)
          this.CheckColumnDuplication(distinctColumns, property.ColumnIterator);
      }
    }

    protected internal virtual void CheckColumnDuplication()
    {
      HashedSet<string> distinctColumns = new HashedSet<string>();
      if (this.IdentifierMapper == null)
        this.CheckColumnDuplication((ISet<string>) distinctColumns, this.Key.ColumnIterator);
      this.CheckColumnDuplication((ISet<string>) distinctColumns, this.DiscriminatorColumnIterator);
      this.CheckPropertyColumnDuplication((ISet<string>) distinctColumns, this.NonDuplicatedPropertyIterator);
      foreach (Join join in this.JoinIterator)
      {
        distinctColumns.Clear();
        this.CheckColumnDuplication((ISet<string>) distinctColumns, join.Key.ColumnIterator);
        this.CheckPropertyColumnDuplication((ISet<string>) distinctColumns, join.PropertyIterator);
      }
    }

    public abstract object Accept(IPersistentClassVisitor mv);

    public bool HasPocoRepresentation => this.ClassName != null;

    public void PrepareTemporaryTables(IMapping mapping, NHibernate.Dialect.Dialect dialect)
    {
      if (!dialect.SupportsTemporaryTables)
        return;
      this.temporaryIdTableName = dialect.GenerateTemporaryTableName(this.Table.Name);
      Table table = new Table();
      table.Name = this.temporaryIdTableName;
      foreach (Column column in this.Table.PrimaryKey.ColumnIterator)
        table.AddColumn((Column) column.Clone());
      this.temporaryIdTableDDL = table.SqlTemporaryTableCreateString(dialect, mapping);
    }

    public virtual Component IdentifierMapper
    {
      get => this.identifierMapper;
      set => this.identifierMapper = value;
    }

    public bool HasIdentifierMapper => this.identifierMapper != null;

    public void AddTuplizer(EntityMode entityMode, string implClass)
    {
      if (this.tuplizerImpls == null)
        this.tuplizerImpls = (IDictionary<EntityMode, string>) new Dictionary<EntityMode, string>();
      this.tuplizerImpls[entityMode] = implClass;
    }

    public virtual string GetTuplizerImplClassName(EntityMode mode)
    {
      if (this.tuplizerImpls == null)
        return (string) null;
      string tuplizerImplClassName;
      this.tuplizerImpls.TryGetValue(mode, out tuplizerImplClassName);
      return tuplizerImplClassName;
    }

    public bool HasNaturalId()
    {
      foreach (Property property in this.RootClazz.PropertyIterator)
      {
        if (property.IsNaturalIdentifier)
          return true;
      }
      return false;
    }

    public abstract bool IsLazyPropertiesCacheable { get; }
  }
}
