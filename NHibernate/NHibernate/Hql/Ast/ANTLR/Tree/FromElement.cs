// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.FromElement
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class FromElement : HqlSqlWalkerNode, IDisplayableNode, IParameterContainer
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (FromElement));
    private bool _isAllPropertyFetch;
    private FromElementType _elementType;
    private string _tableAlias;
    private string _classAlias;
    private string _className;
    private string _collectionTableAlias;
    private FromClause _fromClause;
    private string[] _columns;
    private FromElement _origin;
    private bool _useFromFragment;
    private bool _useWhereFragment = true;
    private bool _includeSubclasses = true;
    private readonly List<FromElement> _destinations = new List<FromElement>();
    private bool _dereferencedBySubclassProperty;
    private bool _dereferencedBySuperclassProperty;
    private bool _collectionJoin;
    private string _role;
    private bool _initialized;
    private SqlString _withClauseFragment;
    private string _withClauseJoinAlias;
    private bool _filter;
    private IToken _token;
    private bool _fetch;
    private List<IParameterSpecification> _embeddedParameters;

    public FromElement(IToken token)
      : base(token)
    {
      this._token = token;
    }

    protected FromElement(FromClause fromClause, FromElement origin, string alias)
      : this(origin._token)
    {
      this._fromClause = fromClause;
      this._origin = origin;
      this._classAlias = alias;
      this._tableAlias = origin.TableAlias;
      this.Initialize((object) fromClause.Walker);
    }

    protected void InitializeComponentJoin(FromElementType elementType)
    {
      this._elementType = elementType;
      this._fromClause.RegisterFromElement(this);
      this._initialized = true;
    }

    public void SetAllPropertyFetch(bool fetch) => this._isAllPropertyFetch = fetch;

    public void SetWithClauseFragment(string withClauseJoinAlias, SqlString withClauseFragment)
    {
      this._withClauseJoinAlias = withClauseJoinAlias;
      this._withClauseFragment = withClauseFragment;
    }

    public JoinSequence JoinSequence
    {
      get => this._elementType.JoinSequence;
      set => this._elementType.JoinSequence = value;
    }

    public string[] Columns
    {
      get => this._columns;
      set => this._columns = value;
    }

    public bool IsEntity => this._elementType.IsEntity;

    public bool IsFromOrJoinFragment => this.Type == 135 || this.Type == 137;

    public bool IsAllPropertyFetch
    {
      get => this._isAllPropertyFetch;
      set => this._isAllPropertyFetch = value;
    }

    public virtual bool IsImpliedInFromClause => false;

    public bool IsFetch => this._fetch;

    public bool Filter
    {
      set => this._filter = value;
    }

    public bool IsFilter => this._filter;

    public IParameterSpecification[] GetEmbeddedParameters() => this._embeddedParameters.ToArray();

    public bool HasEmbeddedParameters
    {
      get => this._embeddedParameters != null && this._embeddedParameters.Count > 0;
    }

    public IParameterSpecification IndexCollectionSelectorParamSpec
    {
      get => this._elementType.IndexCollectionSelectorParamSpec;
      set
      {
        if (value == null)
        {
          if (this._elementType.IndexCollectionSelectorParamSpec == null)
            return;
          this._embeddedParameters.Remove(this._elementType.IndexCollectionSelectorParamSpec);
          this._elementType.IndexCollectionSelectorParamSpec = (IParameterSpecification) null;
        }
        else
        {
          this._elementType.IndexCollectionSelectorParamSpec = value;
          this.AddEmbeddedParameter(value);
        }
      }
    }

    public virtual bool IsImplied => false;

    public bool IsDereferencedBySuperclassOrSubclassProperty
    {
      get => this._dereferencedBySubclassProperty || this._dereferencedBySuperclassProperty;
    }

    public bool IsDereferencedBySubclassProperty => this._dereferencedBySubclassProperty;

    public IEntityPersister EntityPersister => this._elementType.EntityPersister;

    public override IType DataType
    {
      get => this._elementType.DataType;
      set => base.DataType = value;
    }

    public string TableAlias => this._tableAlias;

    private string TableName
    {
      get
      {
        NHibernate.Persister.Entity.IQueryable queryable = this.Queryable;
        return queryable == null ? "{none}" : queryable.TableName;
      }
    }

    public string ClassAlias => this._classAlias;

    public string ClassName => this._className;

    public FromClause FromClause => this._fromClause;

    public NHibernate.Persister.Entity.IQueryable Queryable => this._elementType.Queryable;

    public IQueryableCollection QueryableCollection
    {
      get => this._elementType.QueryableCollection;
      set => this._elementType.QueryableCollection = value;
    }

    public string CollectionTableAlias
    {
      get => this._collectionTableAlias;
      set => this._collectionTableAlias = value;
    }

    public bool CollectionJoin
    {
      get => this._collectionJoin;
      set => this._collectionJoin = value;
    }

    public string CollectionSuffix
    {
      get => this._elementType.CollectionSuffix;
      set => this._elementType.CollectionSuffix = value;
    }

    public IType SelectType => this._elementType.SelectType;

    public bool IsCollectionOfValuesOrComponents
    {
      get => this._elementType.IsCollectionOfValuesOrComponents;
    }

    public bool IsCollectionJoin => this._collectionJoin;

    public void SetRole(string role) => this._role = role;

    public FromElement Origin => this._origin;

    public FromElement RealOrigin
    {
      get
      {
        if (this._origin == null)
          return (FromElement) null;
        return string.IsNullOrEmpty(this._origin.Text) ? this._origin.RealOrigin : this._origin;
      }
    }

    public SqlString WithClauseFragment => this._withClauseFragment;

    public string WithClauseJoinAlias => this._withClauseJoinAlias;

    public string RenderIdentifierSelect(int size, int k)
    {
      return this._elementType.RenderIdentifierSelect(size, k);
    }

    public string RenderPropertySelect(int size, int k)
    {
      return this._elementType.RenderPropertySelect(size, k, this.IsAllPropertyFetch);
    }

    public override SqlString RenderText(ISessionFactoryImplementor sessionFactory)
    {
      SqlString sqlString = SqlString.Parse(this.Text);
      if (this.HasEmbeddedParameters)
      {
        Parameter[] array = sqlString.GetParameters().ToArray<Parameter>();
        int num = 0;
        foreach (string str in this._embeddedParameters.SelectMany<IParameterSpecification, string>((Func<IParameterSpecification, IEnumerable<string>>) (specification => specification.GetIdsForBackTrack((IMapping) sessionFactory))))
          array[num++].BackTrack = (object) str;
      }
      return sqlString;
    }

    public string RenderCollectionSelectFragment(int size, int k)
    {
      return this._elementType.RenderCollectionSelectFragment(size, k);
    }

    public string RenderValueCollectionSelectFragment(int size, int k)
    {
      return this._elementType.RenderValueCollectionSelectFragment(size, k);
    }

    public void SetIndexCollectionSelectorParamSpec(
      IParameterSpecification indexCollectionSelectorParamSpec)
    {
      if (indexCollectionSelectorParamSpec == null)
      {
        if (this._elementType.IndexCollectionSelectorParamSpec == null)
          return;
        this._embeddedParameters.Remove(this._elementType.IndexCollectionSelectorParamSpec);
        this._elementType.IndexCollectionSelectorParamSpec = (IParameterSpecification) null;
      }
      else
      {
        this._elementType.IndexCollectionSelectorParamSpec = indexCollectionSelectorParamSpec;
        this.AddEmbeddedParameter(indexCollectionSelectorParamSpec);
      }
    }

    public virtual void SetImpliedInFromClause(bool flag)
    {
      throw new InvalidOperationException("Explicit FROM elements can't be implied in the FROM clause!");
    }

    public virtual bool IncludeSubclasses
    {
      get => this._includeSubclasses;
      set
      {
        if (this.IsDereferencedBySuperclassOrSubclassProperty && !this._includeSubclasses && FromElement.Log.IsInfoEnabled)
          FromElement.Log.Info((object) "attempt to disable subclass-inclusions", new Exception("stack-trace source"));
        this._includeSubclasses = value;
      }
    }

    public virtual bool InProjectionList
    {
      get => !this.IsImplied && this.IsFromOrJoinFragment;
      set
      {
      }
    }

    public bool Fetch
    {
      get => this._fetch;
      set
      {
        this._fetch = value;
        if (this._fetch && this.Walker.IsShallowQuery)
          throw new QueryException("fetch may not be used with scroll() or iterate()");
      }
    }

    public string RenderScalarIdentifierSelect(int i)
    {
      return this._elementType.RenderScalarIdentifierSelect(i);
    }

    public bool UseFromFragment
    {
      get
      {
        this.CheckInitialized();
        return !this.IsImplied || this._useFromFragment;
      }
      set => this._useFromFragment = value;
    }

    public bool UseWhereFragment
    {
      get => this._useWhereFragment;
      set => this._useWhereFragment = value;
    }

    public string[] ToColumns(string tableAlias, string path, bool inSelect)
    {
      return this._elementType.ToColumns(tableAlias, path, inSelect);
    }

    public string[] ToColumns(string tableAlias, string path, bool inSelect, bool forceAlias)
    {
      return this._elementType.ToColumns(tableAlias, path, inSelect, forceAlias);
    }

    public IPropertyMapping GetPropertyMapping(string propertyName)
    {
      return this._elementType.GetPropertyMapping(propertyName);
    }

    public IType GetPropertyType(string propertyName, string propertyPath)
    {
      return this._elementType.GetPropertyType(propertyName, propertyPath);
    }

    public virtual string GetIdentityColumn()
    {
      this.CheckInitialized();
      string tableAlias = this.TableAlias;
      if (tableAlias == null)
        throw new InvalidOperationException("No table alias for node " + (object) this);
      string propertyName = this.EntityPersister == null || this.EntityPersister.EntityMetamodel == null || !this.EntityPersister.EntityMetamodel.HasNonIdentifierPropertyNamedId ? NHibernate.Persister.Entity.EntityPersister.EntityID : this.EntityPersister.IdentifierPropertyName;
      return StringHelper.Join(", ", this.Walker.StatementType != 45 ? (IEnumerable) this.GetPropertyMapping(propertyName).ToColumns(propertyName) : (IEnumerable) this.GetPropertyMapping(propertyName).ToColumns(tableAlias, propertyName));
    }

    public void HandlePropertyBeingDereferenced(IType propertySource, string propertyName)
    {
      if (this.QueryableCollection != null && CollectionProperties.IsCollectionProperty(propertyName) || propertySource.IsComponentType)
        return;
      NHibernate.Persister.Entity.IQueryable queryable = this.Queryable;
      if (queryable == null)
        return;
      try
      {
        Declarer propertyDeclarer = queryable.GetSubclassPropertyDeclarer(propertyName);
        if (FromElement.Log.IsInfoEnabled)
          FromElement.Log.Info((object) ("handling property dereference [" + queryable.EntityName + " (" + this.ClassAlias + ") -> " + propertyName + " (" + (object) propertyDeclarer + ")]"));
        if (propertyDeclarer == Declarer.SubClass)
        {
          this._dereferencedBySubclassProperty = true;
          this._includeSubclasses = true;
        }
        else
        {
          if (propertyDeclarer != Declarer.SuperClass)
            return;
          this._dereferencedBySuperclassProperty = true;
        }
      }
      catch (QueryException ex)
      {
      }
    }

    public void SetOrigin(FromElement origin, bool manyToMany)
    {
      this._origin = origin;
      origin.AddDestination(this);
      if (origin.FromClause == this.FromClause)
      {
        if (manyToMany)
          origin.AddSibling((IASTNode) this);
        else if (!this.Walker.IsInFrom && !this.Walker.IsInSelect)
          this.FromClause.AddChild((IASTNode) this);
        else
          origin.AddChild((IASTNode) this);
      }
      else
      {
        if (this.Walker.IsInFrom)
          return;
        this.FromClause.AddChild((IASTNode) this);
      }
    }

    public void SetIncludeSubclasses(bool includeSubclasses)
    {
      if (this.IsDereferencedBySuperclassOrSubclassProperty && !includeSubclasses && FromElement.Log.IsInfoEnabled)
        FromElement.Log.Info((object) "attempt to disable subclass-inclusions", new Exception("stack-trace source"));
      this._includeSubclasses = includeSubclasses;
    }

    public virtual string GetDisplayText()
    {
      StringBuilder buf = new StringBuilder();
      buf.Append("FromElement{");
      this.AppendDisplayText(buf);
      buf.Append("}");
      return buf.ToString();
    }

    public void InitializeCollection(FromClause fromClause, string classAlias, string tableAlias)
    {
      this.DoInitialize(fromClause, tableAlias, (string) null, classAlias, (IEntityPersister) null, (EntityType) null);
      this._initialized = true;
    }

    public void InitializeEntity(
      FromClause fromClause,
      string className,
      IEntityPersister persister,
      EntityType type,
      string classAlias,
      string tableAlias)
    {
      this.DoInitialize(fromClause, tableAlias, className, classAlias, persister, type);
      this._initialized = true;
    }

    public void CheckInitialized()
    {
      if (!this._initialized)
        throw new InvalidOperationException("FromElement has not been initialized!");
    }

    protected void AppendDisplayText(StringBuilder buf)
    {
      buf.Append(this.IsImplied ? (this.IsImpliedInFromClause ? "implied in FROM clause" : "implied") : "explicit");
      buf.Append(",").Append(this.IsCollectionJoin ? "collection join" : "not a collection join");
      buf.Append(",").Append(this._fetch ? "fetch join" : "not a fetch join");
      buf.Append(",").Append(this.IsAllPropertyFetch ? "fetch all properties" : "fetch non-lazy properties");
      buf.Append(",classAlias=").Append(this.ClassAlias);
      buf.Append(",role=").Append(this._role);
      buf.Append(",tableName=").Append(this.TableName);
      buf.Append(",tableAlias=").Append(this.TableAlias);
      FromElement realOrigin = this.RealOrigin;
      buf.Append(",origin=").Append(realOrigin == null ? "null" : realOrigin.Text);
      buf.Append(",colums={");
      if (this._columns != null)
      {
        for (int index = 0; index < this._columns.Length; ++index)
        {
          buf.Append(this._columns[index]);
          if (index < this._columns.Length)
            buf.Append(" ");
        }
      }
      buf.Append(",className=").Append(this._className);
      buf.Append("}");
    }

    private void AddDestination(FromElement fromElement) => this._destinations.Add(fromElement);

    private void DoInitialize(
      FromClause fromClause,
      string tableAlias,
      string className,
      string classAlias,
      IEntityPersister persister,
      EntityType type)
    {
      if (this._initialized)
        throw new InvalidOperationException("Already initialized!!");
      this._fromClause = fromClause;
      this._tableAlias = tableAlias;
      this._className = className;
      this._classAlias = classAlias;
      this._elementType = new FromElementType(this, persister, type);
      fromClause.RegisterFromElement(this);
      if (!FromElement.Log.IsDebugEnabled)
        return;
      FromElement.Log.Debug((object) (fromClause.ToString() + " :  " + className + " (" + (classAlias ?? "no alias") + ") -> " + tableAlias));
    }

    public void AddEmbeddedParameter(IParameterSpecification specification)
    {
      if (this._embeddedParameters == null)
        this._embeddedParameters = new List<IParameterSpecification>();
      this._embeddedParameters.Add(specification);
    }
  }
}
