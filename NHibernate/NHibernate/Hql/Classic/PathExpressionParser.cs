// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.PathExpressionParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class PathExpressionParser : IParser
  {
    public const string EntityID = "id";
    public const string EntityClass = "class";
    private int dotcount;
    private string currentName;
    private string currentProperty;
    private string oneToOneOwnerName;
    private IAssociationType ownerAssociationType;
    private string[] columns;
    private string collectionName;
    private string collectionOwnerName;
    private string collectionRole;
    private readonly StringBuilder componentPath = new StringBuilder();
    private IType type;
    private readonly StringBuilder path = new StringBuilder();
    private bool ignoreInitialJoin;
    private bool continuation;
    private JoinType joinType;
    private bool useThetaStyleJoin = true;
    private IPropertyMapping currentPropertyMapping;
    private JoinSequence joinSequence;
    private bool expectingCollectionIndex;
    private readonly List<PathExpressionParser.CollectionElement> collectionElements = new List<PathExpressionParser.CollectionElement>();

    public JoinType JoinType
    {
      get => this.joinType;
      set => this.joinType = value;
    }

    public bool UseThetaStyleJoin
    {
      get => this.useThetaStyleJoin;
      set => this.useThetaStyleJoin = value;
    }

    private IPropertyMapping PropertyMapping => this.currentPropertyMapping;

    private void AddJoin(string name, IAssociationType joinableType)
    {
      this.AddJoin(name, joinableType, this.CurrentColumns());
    }

    private void AddJoin(string name, IAssociationType joinableType, string[] foreignKeyColumns)
    {
      try
      {
        this.joinSequence.AddJoin(joinableType, name, this.joinType, foreignKeyColumns);
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
    }

    public string ContinueFromManyToMany(string clazz, string[] joinColumns, QueryTranslator q)
    {
      this.Start(q);
      this.continuation = true;
      this.currentName = q.CreateNameFor(clazz);
      q.AddType(this.currentName, clazz);
      IQueryable persister = q.GetPersister(clazz);
      this.AddJoin(this.currentName, (IAssociationType) TypeFactory.ManyToOne(clazz), joinColumns);
      this.currentPropertyMapping = (IPropertyMapping) persister;
      return this.currentName;
    }

    public void IgnoreInitialJoin() => this.ignoreInitialJoin = true;

    public void Token(string token, QueryTranslator q)
    {
      if (token != null)
        this.path.Append(token);
      string pathAlias = q.GetPathAlias(this.path.ToString());
      if (pathAlias != null)
      {
        this.Reset(q);
        this.currentName = pathAlias;
        this.currentPropertyMapping = q.GetPropertyMapping(this.currentName);
        if (this.ignoreInitialJoin)
          return;
        JoinSequence pathJoin = q.GetPathJoin(this.path.ToString());
        try
        {
          this.joinSequence.AddCondition(pathJoin.ToJoinFragment(q.EnabledFilters, true).ToWhereFragmentString);
        }
        catch (MappingException ex)
        {
          throw new QueryException((Exception) ex);
        }
      }
      else if (".".Equals(token))
        ++this.dotcount;
      else if (this.dotcount == 0)
      {
        if (this.continuation)
          return;
        this.currentName = q.IsName(token) ? token : throw new QueryException("undefined alias or unknown mapping: " + token);
        this.currentPropertyMapping = q.GetPropertyMapping(this.currentName);
      }
      else if (this.dotcount == 1)
      {
        if (this.currentName != null)
        {
          this.currentProperty = token;
        }
        else
        {
          if (this.collectionName == null)
            throw new QueryException("unexpected");
          this.continuation = false;
        }
      }
      else
      {
        IType propertyType = this.PropertyType;
        if (propertyType == null)
          throw new QueryException("unresolved property: " + this.currentProperty);
        if (propertyType.IsComponentType)
          this.DereferenceComponent(token);
        else if (propertyType.IsEntityType)
          this.DereferenceEntity(token, (EntityType) propertyType, q);
        else if (propertyType.IsCollectionType)
          this.DereferenceCollection(token, ((CollectionType) propertyType).Role, q);
        else if (token != null)
          throw new QueryException("dereferenced: " + this.currentProperty);
      }
    }

    private void DereferenceEntity(string propertyName, EntityType propertyType, QueryTranslator q)
    {
      bool flag1 = "id".Equals(propertyName) && !propertyType.IsUniqueKeyReference;
      string uniqueKeyPropertyName;
      try
      {
        uniqueKeyPropertyName = propertyType.GetIdentifierOrUniqueKeyPropertyName((IMapping) q.Factory);
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
      bool flag2 = uniqueKeyPropertyName != null && uniqueKeyPropertyName.Equals(propertyName);
      if (flag1 || flag2)
      {
        this.DereferenceProperty(propertyName);
      }
      else
      {
        string associatedEntityName = propertyType.GetAssociatedEntityName();
        string nameFor = q.CreateNameFor(associatedEntityName);
        q.AddType(nameFor, associatedEntityName);
        this.AddJoin(nameFor, (IAssociationType) propertyType);
        this.oneToOneOwnerName = !propertyType.IsOneToOne ? (string) null : this.currentName;
        this.ownerAssociationType = (IAssociationType) propertyType;
        this.currentName = nameFor;
        this.currentProperty = propertyName;
        q.AddPathAliasAndJoin(this.path.ToString(0, this.path.ToString().LastIndexOf('.')), nameFor, this.joinSequence.Copy());
        this.componentPath.Length = 0;
        this.currentPropertyMapping = (IPropertyMapping) q.GetPersister(associatedEntityName);
      }
    }

    private void DereferenceProperty(string propertyName)
    {
      if (propertyName == null)
        return;
      if (this.componentPath.Length > 0)
        this.componentPath.Append('.');
      this.componentPath.Append(propertyName);
    }

    private void DereferenceComponent(string propertyName)
    {
      this.DereferenceProperty(propertyName);
    }

    private void DereferenceCollection(string propertyName, string role, QueryTranslator q)
    {
      this.collectionRole = role;
      IQueryableCollection collectionPersister = q.GetCollectionPersister(role);
      string nameForCollection = q.CreateNameForCollection(role);
      this.AddJoin(nameForCollection, (IAssociationType) collectionPersister.CollectionType);
      this.collectionName = nameForCollection;
      this.collectionOwnerName = this.currentName;
      this.currentName = nameForCollection;
      this.currentProperty = propertyName;
      this.componentPath.Length = 0;
      this.currentPropertyMapping = (IPropertyMapping) new CollectionPropertyMapping(collectionPersister);
    }

    private string PropertyPath
    {
      get
      {
        if (this.currentProperty == null)
          return "id";
        return this.componentPath != null && this.componentPath.Length > 0 ? this.currentProperty + (object) '.' + (object) this.componentPath : this.currentProperty;
      }
    }

    private void SetType(QueryTranslator q)
    {
      if (this.currentProperty == null)
        this.type = this.PropertyMapping.Type;
      else
        this.type = this.PropertyType;
    }

    protected IType PropertyType
    {
      get
      {
        return this.PropertyMapping.ToType(this.PropertyPath) ?? throw new QueryException("could not resolve property type: " + this.PropertyPath);
      }
    }

    protected string[] CurrentColumns()
    {
      string propertyPath = this.PropertyPath;
      return this.PropertyMapping.ToColumns(this.currentName, propertyPath) ?? throw new QueryException("could not resolve property columns: " + propertyPath);
    }

    private void Reset(QueryTranslator q)
    {
      this.dotcount = 0;
      this.currentName = (string) null;
      this.currentProperty = (string) null;
      this.collectionName = (string) null;
      this.collectionRole = (string) null;
      this.componentPath.Length = 0;
      this.type = (IType) null;
      this.collectionName = (string) null;
      this.columns = (string[]) null;
      this.expectingCollectionIndex = false;
      this.continuation = false;
      this.currentPropertyMapping = (IPropertyMapping) null;
    }

    public void Start(QueryTranslator q)
    {
      if (this.continuation)
        return;
      this.Reset(q);
      this.path.Length = 0;
      this.joinSequence = new JoinSequence(q.Factory).SetUseThetaStyle(this.useThetaStyleJoin);
    }

    public virtual void End(QueryTranslator q)
    {
      this.ignoreInitialJoin = false;
      IType propertyType = this.PropertyType;
      if (propertyType != null && propertyType.IsCollectionType)
      {
        this.collectionRole = ((CollectionType) propertyType).Role;
        this.collectionName = q.CreateNameForCollection(this.collectionRole);
        this.PrepareForIndex(q);
      }
      else
      {
        this.columns = this.CurrentColumns();
        this.SetType(q);
      }
      this.continuation = false;
    }

    private void PrepareForIndex(QueryTranslator q)
    {
      IQueryableCollection collectionPersister = q.GetCollectionPersister(this.collectionRole);
      string[] strArray = collectionPersister.HasIndex ? collectionPersister.IndexColumnNames : throw new QueryException("unindexed collection before []");
      if (strArray.Length != 1)
        throw new QueryException("composite-index appears in []: " + (object) this.path);
      JoinSequence joinSequence = new JoinSequence(q.Factory).SetUseThetaStyle(this.useThetaStyleJoin).SetRoot((IJoinable) collectionPersister, this.collectionName).SetNext(this.joinSequence.Copy());
      if (!this.continuation)
        this.AddJoin(this.collectionName, (IAssociationType) collectionPersister.CollectionType);
      this.joinSequence.AddCondition(new SqlString(this.collectionName + (object) '.' + strArray[0] + " = "));
      this.collectionElements.Add(new PathExpressionParser.CollectionElement()
      {
        ElementColumns = collectionPersister.GetElementColumnNames(this.collectionName),
        Type = collectionPersister.ElementType,
        IsOneToMany = collectionPersister.IsOneToMany,
        Alias = this.collectionName,
        JoinSequence = this.joinSequence
      });
      this.SetExpectingCollectionIndex();
      q.AddCollection(this.collectionName, this.collectionRole);
      q.AddJoin(this.collectionName, joinSequence);
    }

    public PathExpressionParser.CollectionElement LastCollectionElement()
    {
      PathExpressionParser.CollectionElement collectionElement = this.collectionElements[this.collectionElements.Count - 1];
      this.collectionElements.RemoveAt(this.collectionElements.Count - 1);
      return collectionElement;
    }

    public void SetLastCollectionElementIndexValue(SqlString value)
    {
      this.collectionElements[this.collectionElements.Count - 1].IndexValue.Add(value);
    }

    public bool IsExpectingCollectionIndex
    {
      get => this.expectingCollectionIndex;
      set => this.expectingCollectionIndex = value;
    }

    protected virtual void SetExpectingCollectionIndex() => this.expectingCollectionIndex = true;

    public JoinSequence WhereJoin => this.joinSequence;

    public string WhereColumn
    {
      get
      {
        return this.columns.Length == 1 ? this.columns[0] : throw new QueryException("path expression ends in a composite value");
      }
    }

    public string[] WhereColumns => this.columns;

    public IType WhereColumnType => this.type;

    public string Name => this.currentName != null ? this.currentName : this.collectionName;

    public string GetCollectionSubquery(IDictionary<string, IFilter> enabledFilters)
    {
      return CollectionSubqueryFactory.CreateCollectionSubquery(this.joinSequence, enabledFilters, this.CurrentColumns());
    }

    public bool IsCollectionValued
    {
      get => this.collectionName != null && !this.PropertyType.IsCollectionType;
    }

    public void AddAssociation(QueryTranslator q) => q.AddJoin(this.Name, this.joinSequence);

    public string AddFromAssociation(QueryTranslator q)
    {
      if (this.IsCollectionValued)
        return this.AddFromCollection(q);
      q.AddFrom(this.currentName, this.joinSequence);
      return this.currentName;
    }

    public string AddFromCollection(QueryTranslator q)
    {
      IType propertyType = this.PropertyType;
      if (propertyType == null)
        throw new QueryException(string.Format("must specify 'elements' for collection valued property in from clause: {0}", (object) this.path));
      if (propertyType.IsEntityType)
      {
        IQueryableCollection collectionPersister = q.GetCollectionPersister(this.collectionRole);
        string entityName = collectionPersister.ElementPersister.EntityName;
        string name;
        if (collectionPersister.IsOneToMany)
        {
          name = this.collectionName;
          q.DecoratePropertyMapping(name, (IPropertyMapping) collectionPersister);
        }
        else
        {
          q.AddCollection(this.collectionName, this.collectionRole);
          name = q.CreateNameFor(entityName);
          this.AddJoin(name, (IAssociationType) propertyType);
        }
        q.AddFrom(name, entityName, this.joinSequence);
        this.currentPropertyMapping = (IPropertyMapping) new CollectionPropertyMapping(collectionPersister);
        return name;
      }
      q.AddFromCollection(this.collectionName, this.collectionRole, this.joinSequence);
      return this.collectionName;
    }

    public string CollectionName => this.collectionName;

    public string CollectionRole => this.collectionRole;

    public string CollectionOwnerName => this.collectionOwnerName;

    public string CurrentName => this.currentName;

    public string CurrentProperty => this.currentProperty;

    public void Fetch(QueryTranslator q, string entityName)
    {
      if (this.IsCollectionValued)
        q.AddCollectionToFetch(this.CollectionRole, this.CollectionName, this.CollectionOwnerName, entityName);
      else
        q.AddEntityToFetch(entityName, this.oneToOneOwnerName, this.ownerAssociationType);
    }

    public string ProcessedPath => this.path.ToString();

    public sealed class CollectionElement
    {
      public IType Type;
      public bool IsOneToMany;
      public string Alias;
      public string[] ElementColumns;
      public JoinSequence JoinSequence;
      public SqlStringBuilder IndexValue = new SqlStringBuilder();
    }
  }
}
