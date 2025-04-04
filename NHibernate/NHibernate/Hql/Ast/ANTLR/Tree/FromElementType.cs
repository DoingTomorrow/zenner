// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.FromElementType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class FromElementType
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (FromElementType));
    private readonly FromElement _fromElement;
    private readonly IEntityPersister _persister;
    private readonly EntityType _entityType;
    private IQueryableCollection _queryableCollection;
    private CollectionPropertyMapping _collectionPropertyMapping;
    private JoinSequence _joinSequence;
    private IParameterSpecification _indexCollectionSelectorParamSpec;
    private string _collectionSuffix;

    public FromElementType(
      FromElement fromElement,
      IEntityPersister persister,
      EntityType entityType)
    {
      this._fromElement = fromElement;
      this._persister = persister;
      this._entityType = entityType;
      if (persister == null)
        return;
      fromElement.Text = ((IJoinable) persister).TableName + " " + this.TableAlias;
    }

    protected FromElementType(FromElement fromElement) => this._fromElement = fromElement;

    public IEntityPersister EntityPersister => this._persister;

    private string TableAlias => this._fromElement.TableAlias;

    private string CollectionTableAlias => this._fromElement.CollectionTableAlias;

    public virtual IType DataType
    {
      get
      {
        if (this._persister != null)
          return (IType) this._entityType;
        return this._queryableCollection == null ? (IType) null : this._queryableCollection.Type;
      }
    }

    public IType SelectType
    {
      get
      {
        if (this._entityType == null)
          return (IType) null;
        bool isShallowQuery = this._fromElement.FromClause.Walker.IsShallowQuery;
        return (IType) TypeFactory.ManyToOne(this._entityType.GetAssociatedEntityName(), isShallowQuery);
      }
    }

    public string CollectionSuffix
    {
      get => this._collectionSuffix;
      set => this._collectionSuffix = value;
    }

    public IParameterSpecification IndexCollectionSelectorParamSpec
    {
      get => this._indexCollectionSelectorParamSpec;
      set => this._indexCollectionSelectorParamSpec = value;
    }

    public JoinSequence JoinSequence
    {
      get
      {
        if (this._joinSequence != null)
          return this._joinSequence;
        if (!(this._persister is IJoinable))
          return (JoinSequence) null;
        IJoinable persister = (IJoinable) this._persister;
        return this._fromElement.SessionFactoryHelper.CreateJoinSequence().SetRoot(persister, this.TableAlias);
      }
      set => this._joinSequence = value;
    }

    public string RenderIdentifierSelect(int size, int k)
    {
      this.CheckInitialized();
      if (this._fromElement.FromClause.IsSubQuery)
      {
        string[] strArray = this._persister != null ? ((ILoadable) this._persister).IdentifierColumnNames : new string[0];
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < strArray.Length; ++index)
        {
          stringBuilder.Append(this._fromElement.TableAlias).Append('.').Append(strArray[index]);
          if (index != strArray.Length - 1)
            stringBuilder.Append(", ");
        }
        return stringBuilder.ToString();
      }
      if (this._persister == null)
        throw new QueryException("not an entity");
      return FromElementType.TrimLeadingCommaAndSpaces(((IQueryable) this._persister).IdentifierSelectFragment(this.TableAlias, FromElementType.GetSuffix(size, k)));
    }

    public virtual string RenderScalarIdentifierSelect(int i)
    {
      this.CheckInitialized();
      string[] columns = this.GetPropertyMapping(NHibernate.Persister.Entity.EntityPersister.EntityID).ToColumns(this.TableAlias, NHibernate.Persister.Entity.EntityPersister.EntityID);
      StringBuilder stringBuilder = new StringBuilder();
      for (int y = 0; y < columns.Length; ++y)
      {
        string str = columns[y];
        if (y > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append(str).Append(" as ").Append(NameGenerator.ScalarName(i, y));
      }
      return stringBuilder.ToString();
    }

    public string RenderPropertySelect(int size, int k, bool allProperties)
    {
      this.CheckInitialized();
      return this._persister == null ? "" : FromElementType.TrimLeadingCommaAndSpaces(((IQueryable) this._persister).PropertySelectFragment(this.TableAlias, FromElementType.GetSuffix(size, k), allProperties));
    }

    public string RenderCollectionSelectFragment(int size, int k)
    {
      if (this._queryableCollection == null)
        return "";
      if (this._collectionSuffix == null)
        this._collectionSuffix = FromElementType.GenerateSuffix(size, k);
      return FromElementType.TrimLeadingCommaAndSpaces(this._queryableCollection.SelectFragment(this.CollectionTableAlias, this._collectionSuffix));
    }

    public string RenderValueCollectionSelectFragment(int size, int k)
    {
      if (this._queryableCollection == null)
        return "";
      if (this._collectionSuffix == null)
        this._collectionSuffix = FromElementType.GenerateSuffix(size, k);
      return FromElementType.TrimLeadingCommaAndSpaces(this._queryableCollection.SelectFragment(this.TableAlias, this._collectionSuffix));
    }

    public bool IsEntity => this._persister != null;

    public bool IsCollectionOfValuesOrComponents
    {
      get
      {
        return this._persister == null && this._queryableCollection != null && !this._queryableCollection.ElementType.IsEntityType;
      }
    }

    public virtual IPropertyMapping GetPropertyMapping(string propertyName)
    {
      this.CheckInitialized();
      if (this._queryableCollection == null)
        return (IPropertyMapping) this._persister;
      if (CollectionProperties.IsCollectionProperty(propertyName))
      {
        if (this._collectionPropertyMapping == null)
          this._collectionPropertyMapping = new CollectionPropertyMapping(this._queryableCollection);
        return (IPropertyMapping) this._collectionPropertyMapping;
      }
      if (this._queryableCollection.ElementType.IsAnyType)
        return (IPropertyMapping) this._queryableCollection;
      return this._queryableCollection.ElementType.IsComponentType && propertyName == NHibernate.Persister.Entity.EntityPersister.EntityID ? (IPropertyMapping) this._queryableCollection.OwnerEntityPersister : (IPropertyMapping) this._queryableCollection;
    }

    public virtual IType GetPropertyType(string propertyName, string propertyPath)
    {
      this.CheckInitialized();
      return (this._persister == null || !(propertyName == propertyPath) || !(propertyName == this._persister.IdentifierPropertyName) ? this.GetPropertyMapping(propertyName).ToType(propertyPath) : this._persister.IdentifierType) ?? throw new MappingException("Property " + propertyName + " does not exist in " + (this._queryableCollection == null ? "class" : "collection") + " " + (this._queryableCollection == null ? this._fromElement.ClassName : this._queryableCollection.Role));
    }

    public string[] ToColumns(string tableAlias, string path, bool inSelect)
    {
      return this.ToColumns(tableAlias, path, inSelect, false);
    }

    public IQueryable Queryable
    {
      get => !(this._persister is IQueryable) ? (IQueryable) null : (IQueryable) this._persister;
    }

    public virtual IQueryableCollection QueryableCollection
    {
      get => this._queryableCollection;
      set
      {
        this._queryableCollection = this._queryableCollection == null ? value : throw new InvalidOperationException("QueryableCollection is already defined for " + (object) this + "!");
        if (this._queryableCollection.IsOneToMany)
          return;
        this._fromElement.Text = this._queryableCollection.TableName + " " + this.TableAlias;
      }
    }

    public string[] ToColumns(string tableAlias, string path, bool inSelect, bool forceAlias)
    {
      this.CheckInitialized();
      IPropertyMapping propertyMapping = this.GetPropertyMapping(path);
      if (!inSelect && this._queryableCollection != null && CollectionProperties.IsCollectionProperty(path))
      {
        string collectionSubquery = CollectionSubqueryFactory.CreateCollectionSubquery(this._joinSequence, this._fromElement.Walker.EnabledFilters, propertyMapping.ToColumns(tableAlias, path));
        if (FromElementType.Log.IsDebugEnabled)
          FromElementType.Log.Debug((object) ("toColumns(" + tableAlias + "," + path + ") : subquery = " + collectionSubquery));
        return new string[1]
        {
          "(" + collectionSubquery + ")"
        };
      }
      if (forceAlias || this._fromElement.Walker.StatementType == 45 || this._fromElement.Walker.CurrentClauseType == 45)
        return propertyMapping.ToColumns(tableAlias, path);
      if (this._fromElement.Walker.IsSubQuery)
        return this.IsCorrelation && !this.IsMultiTable ? propertyMapping.ToColumns(this.ExtractTableName(), path) : propertyMapping.ToColumns(tableAlias, path);
      string[] columns = propertyMapping.ToColumns(path);
      FromElementType.Log.Info((object) ("Using non-qualified column reference [" + path + " -> (" + ArrayHelper.ToString((object[]) columns) + ")]"));
      return columns;
    }

    private static string GetSuffix(int size, int sequence)
    {
      return FromElementType.GenerateSuffix(size, sequence);
    }

    private static string GenerateSuffix(int size, int k)
    {
      return size == 1 ? "" : k.ToString() + (object) '_';
    }

    private void CheckInitialized() => this._fromElement.CheckInitialized();

    private bool IsCorrelation
    {
      get
      {
        FromClause finalFromClause = this._fromElement.Walker.GetFinalFromClause();
        return this._fromElement.FromClause != this._fromElement.Walker.CurrentFromClause && this._fromElement.FromClause == finalFromClause;
      }
    }

    private bool IsMultiTable
    {
      get => this._fromElement.Queryable != null && this._fromElement.Queryable.IsMultiTable;
    }

    private string ExtractTableName() => this._fromElement.Queryable.TableName;

    private static string TrimLeadingCommaAndSpaces(string fragment)
    {
      if (fragment.Length > 0 && fragment[0] == ',')
        fragment = fragment.Substring(1);
      fragment = fragment.Trim();
      return fragment.Trim();
    }
  }
}
