// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.FromElementFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class FromElementFactory
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (FromElementFactory));
    private readonly FromClause _fromClause;
    private readonly FromElement _origin;
    private readonly string _path;
    private readonly bool _collection;
    private readonly string _classAlias;
    private readonly string[] _columns;
    private bool _implied;
    private bool _inElementsFunction;
    private IQueryableCollection _queryableCollection;
    private CollectionType _collectionType;

    public FromElementFactory(FromClause fromClause, FromElement origin, string path)
    {
      this._fromClause = fromClause;
      this._origin = origin;
      this._path = path;
      this._collection = false;
    }

    public FromElementFactory(
      FromClause fromClause,
      FromElement origin,
      string path,
      string classAlias,
      string[] columns,
      bool implied)
      : this(fromClause, origin, path)
    {
      this._classAlias = classAlias;
      this._columns = columns;
      this._implied = implied;
      this._collection = true;
    }

    public FromElement AddFromElement()
    {
      FromClause parentFromClause = this._fromClause.ParentFromClause;
      if (parentFromClause != null)
      {
        string alias = PathHelper.GetAlias(this._path);
        FromElement fromElement = parentFromClause.GetFromElement(alias);
        if (fromElement != null)
          return this.CreateFromElementInSubselect(this._path, alias, fromElement, this._classAlias);
      }
      IEntityPersister entityPersister = this._fromClause.SessionFactoryHelper.RequireClassPersister(this._path);
      FromElement andAddFromElement = this.CreateAndAddFromElement(this._path, this._classAlias, entityPersister, (EntityType) ((IPropertyMapping) entityPersister).Type, (string) null);
      this._fromClause.Walker.AddQuerySpaces(entityPersister.QuerySpaces);
      return andAddFromElement;
    }

    public FromElement CreateCollectionElementsJoin(
      IQueryableCollection queryableCollection,
      string collectionName)
    {
      JoinSequence collectionJoinSequence = this._fromClause.SessionFactoryHelper.CreateCollectionJoinSequence(queryableCollection, collectionName);
      this._queryableCollection = queryableCollection;
      return this.CreateCollectionJoin(collectionJoinSequence, (string) null);
    }

    private FromElement CreateFromElementInSubselect(
      string path,
      string pathAlias,
      FromElement parentFromElement,
      string classAlias)
    {
      if (FromElementFactory.Log.IsDebugEnabled)
        FromElementFactory.Log.Debug((object) ("createFromElementInSubselect() : path = " + path));
      FromElement element = this.EvaluateFromElementPath(path, classAlias);
      IEntityPersister entityPersister = element.EntityPersister;
      string tableAlias = pathAlias == parentFromElement.ClassAlias ? element.TableAlias : (string) null;
      if (element.FromClause != this._fromClause)
      {
        if (FromElementFactory.Log.IsDebugEnabled)
          FromElementFactory.Log.Debug((object) "createFromElementInSubselect() : creating a new FROM element...");
        element = this.CreateFromElement(entityPersister);
        this.InitializeAndAddFromElement(element, path, classAlias, entityPersister, (EntityType) ((IPropertyMapping) entityPersister).Type, tableAlias);
      }
      if (FromElementFactory.Log.IsDebugEnabled)
        FromElementFactory.Log.Debug((object) ("createFromElementInSubselect() : " + path + " -> " + (object) element));
      return element;
    }

    public FromElement CreateCollection(
      IQueryableCollection queryableCollection,
      string role,
      JoinType joinType,
      bool fetchFlag,
      bool indexed)
    {
      if (!this._collection)
        throw new InvalidOperationException("FromElementFactory not initialized for collections!");
      this._inElementsFunction = indexed;
      this._queryableCollection = queryableCollection;
      this._collectionType = queryableCollection.CollectionType;
      string name = this._fromClause.AliasGenerator.CreateName(role);
      bool flag = this._fromClause.IsSubQuery && !this._implied;
      if (flag)
      {
        FromElement fromElement = this._fromClause.GetFromElement(StringHelper.Root(this._path));
        if (fromElement == null || fromElement.FromClause != this._fromClause)
          this._implied = true;
      }
      if (flag && DotNode.UseThetaStyleImplicitJoins)
        this._implied = true;
      IType elementType = queryableCollection.ElementType;
      FromElement collection = !elementType.IsEntityType ? (!elementType.IsComponentType ? this.CreateCollectionJoin(this.CreateJoinSequence(name, joinType), name) : this.CreateCollectionJoin(this.CreateJoinSequence(name, joinType), name)) : this.CreateEntityAssociation(role, name, joinType);
      collection.SetRole(role);
      collection.QueryableCollection = queryableCollection;
      if (this._implied)
        collection.IncludeSubclasses = false;
      if (flag)
        collection.InProjectionList = true;
      if (fetchFlag)
        collection.Fetch = true;
      return collection;
    }

    public FromElement CreateElementJoin(IQueryableCollection queryableCollection)
    {
      this._implied = true;
      this._inElementsFunction = true;
      if (!queryableCollection.ElementType.IsEntityType)
        throw new InvalidOperationException("Cannot create element join for a collection of non-entities!");
      this._queryableCollection = queryableCollection;
      SessionFactoryHelperExtensions sessionFactoryHelper = this._fromClause.SessionFactoryHelper;
      IEntityPersister elementPersister = queryableCollection.ElementPersister;
      string name = this._fromClause.AliasGenerator.CreateName(elementPersister.EntityName);
      string entityName = elementPersister.EntityName;
      IEntityPersister entityPersister = sessionFactoryHelper.RequireClassPersister(entityName);
      FromElement andAddFromElement = this.CreateAndAddFromElement(entityName, this._classAlias, entityPersister, (EntityType) queryableCollection.ElementType, name);
      if (this._implied)
        andAddFromElement.IncludeSubclasses = false;
      this._fromClause.AddCollectionJoinFromElementByPath(this._path, andAddFromElement);
      this._fromClause.Walker.AddQuerySpaces(elementPersister.QuerySpaces);
      CollectionType collectionType = queryableCollection.CollectionType;
      string role = collectionType.Role;
      string tableAlias = this._origin.TableAlias;
      string[] collectionElementColumns = sessionFactoryHelper.GetCollectionElementColumns(role, tableAlias);
      IAssociationType elementAssociationType = sessionFactoryHelper.GetElementAssociationType(collectionType);
      JoinSequence joinSequence = sessionFactoryHelper.CreateJoinSequence(this._implied, elementAssociationType, name, JoinType.InnerJoin, collectionElementColumns);
      FromElement elementJoin = this.InitializeJoin(this._path, andAddFromElement, joinSequence, collectionElementColumns, this._origin, false);
      elementJoin.UseFromFragment = true;
      elementJoin.CollectionTableAlias = tableAlias;
      return elementJoin;
    }

    public FromElement CreateEntityJoin(
      string entityClass,
      string tableAlias,
      JoinSequence joinSequence,
      bool fetchFlag,
      bool inFrom,
      EntityType type)
    {
      FromElement join = this.CreateJoin(entityClass, tableAlias, joinSequence, type, false);
      join.Fetch = fetchFlag;
      if (this._implied && !join.UseFromFragment)
      {
        if (FromElementFactory.Log.IsDebugEnabled)
          FromElementFactory.Log.Debug((object) "createEntityJoin() : Implied entity join");
        join.UseFromFragment = true;
      }
      if (this._implied && inFrom)
      {
        joinSequence.SetUseThetaStyle(false);
        join.UseFromFragment = true;
        join.SetImpliedInFromClause(true);
      }
      if (join.Walker.IsSubQuery && (join.FromClause != join.Origin.FromClause || DotNode.UseThetaStyleImplicitJoins))
      {
        join.Type = 135;
        joinSequence.SetUseThetaStyle(true);
        join.UseFromFragment = false;
      }
      return join;
    }

    private FromElement CreateEntityAssociation(string role, string roleAlias, JoinType joinType)
    {
      IQueryable elementPersister = (IQueryable) this._queryableCollection.ElementPersister;
      string entityName = elementPersister.EntityName;
      FromElement entityAssociation;
      if (this._queryableCollection.IsOneToMany)
      {
        if (FromElementFactory.Log.IsDebugEnabled)
          FromElementFactory.Log.Debug((object) ("createEntityAssociation() : One to many - path = " + this._path + " role = " + role + " associatedEntityName = " + entityName));
        JoinSequence joinSequence = this.CreateJoinSequence(roleAlias, joinType);
        entityAssociation = this.CreateJoin(entityName, roleAlias, joinSequence, (EntityType) this._queryableCollection.ElementType, false);
        FromElement fromElement = entityAssociation;
        fromElement.UseFromFragment = ((fromElement.UseFromFragment ? 1 : 0) | (!entityAssociation.IsImplied ? 0 : (entityAssociation.Walker.IsSubQuery ? 1 : 0))) != 0;
      }
      else
      {
        if (FromElementFactory.Log.IsDebugEnabled)
          FromElementFactory.Log.Debug((object) ("createManyToMany() : path = " + this._path + " role = " + role + " associatedEntityName = " + entityName));
        entityAssociation = this.CreateManyToMany(role, entityName, roleAlias, (IEntityPersister) elementPersister, (EntityType) this._queryableCollection.ElementType, joinType);
        this._fromClause.Walker.AddQuerySpaces(this._queryableCollection.CollectionSpaces);
      }
      entityAssociation.CollectionTableAlias = roleAlias;
      return entityAssociation;
    }

    private FromElement CreateCollectionJoin(JoinSequence collectionJoinSequence, string tableAlias)
    {
      FromElement fromElement = (FromElement) this.CreateFromElement(this._queryableCollection.TableName);
      if (this._queryableCollection.ElementType.IsCollectionType)
        throw new SemanticException("Collections of collections are not supported!");
      fromElement.InitializeCollection(this._fromClause, this._classAlias, tableAlias);
      fromElement.Type = 137;
      fromElement.SetIncludeSubclasses(false);
      fromElement.CollectionJoin = true;
      fromElement.JoinSequence = collectionJoinSequence;
      fromElement.SetOrigin(this._origin, false);
      fromElement.CollectionTableAlias = tableAlias;
      this._origin.Text = "";
      this._origin.CollectionJoin = true;
      this._fromClause.AddCollectionJoinFromElementByPath(this._path, fromElement);
      this._fromClause.Walker.AddQuerySpaces(this._queryableCollection.CollectionSpaces);
      return fromElement;
    }

    private FromElement CreateManyToMany(
      string role,
      string associatedEntityName,
      string roleAlias,
      IEntityPersister entityPersister,
      EntityType type,
      JoinType joinType)
    {
      SessionFactoryHelperExtensions sessionFactoryHelper = this._fromClause.SessionFactoryHelper;
      FromElement join;
      if (this._inElementsFunction)
      {
        JoinSequence joinSequence = this.CreateJoinSequence(roleAlias, joinType);
        join = this.CreateJoin(associatedEntityName, roleAlias, joinSequence, type, true);
      }
      else
      {
        string name = this._fromClause.AliasGenerator.CreateName(entityPersister.EntityName);
        string[] collectionElementColumns = sessionFactoryHelper.GetCollectionElementColumns(role, roleAlias);
        JoinSequence joinSequence = this.CreateJoinSequence(roleAlias, joinType);
        joinSequence.AddJoin(sessionFactoryHelper.GetElementAssociationType(this._collectionType), name, joinType, collectionElementColumns);
        join = this.CreateJoin(associatedEntityName, name, joinSequence, type, false);
        join.UseFromFragment = true;
      }
      return join;
    }

    private JoinSequence CreateJoinSequence(string roleAlias, JoinType joinType)
    {
      SessionFactoryHelperExtensions sessionFactoryHelper = this._fromClause.SessionFactoryHelper;
      string[] columns = this.Columns;
      if (this._collectionType == null)
        throw new InvalidOperationException("collectionType is null!");
      return sessionFactoryHelper.CreateJoinSequence(this._implied, (IAssociationType) this._collectionType, roleAlias, joinType, columns);
    }

    private FromElement CreateJoin(
      string entityClass,
      string tableAlias,
      JoinSequence joinSequence,
      EntityType type,
      bool manyToMany)
    {
      IEntityPersister entityPersister = this._fromClause.SessionFactoryHelper.RequireClassPersister(entityClass);
      return this.InitializeJoin(this._path, this.CreateAndAddFromElement(entityClass, this._classAlias, entityPersister, type, tableAlias), joinSequence, this.Columns, this._origin, manyToMany);
    }

    private FromElement InitializeJoin(
      string path,
      FromElement destination,
      JoinSequence joinSequence,
      string[] columns,
      FromElement origin,
      bool manyToMany)
    {
      destination.Type = 137;
      destination.JoinSequence = joinSequence;
      destination.Columns = columns;
      destination.SetOrigin(origin, manyToMany);
      this._fromClause.AddJoinByPathMap(path, destination);
      return destination;
    }

    private FromElement EvaluateFromElementPath(string path, string classAlias)
    {
      FromReferenceNode path1 = (FromReferenceNode) PathHelper.ParsePath(path, this._fromClause.ASTFactory);
      path1.RecursiveResolve(0, false, classAlias, (IASTNode) null);
      return path1.GetImpliedJoin() != null ? path1.GetImpliedJoin() : path1.FromElement;
    }

    private FromElement CreateFromElement(IEntityPersister entityPersister)
    {
      return (FromElement) this.CreateFromElement(((IJoinable) entityPersister).TableName);
    }

    private IASTNode CreateFromElement(string text)
    {
      IASTNode node = this._fromClause.ASTFactory.CreateNode(this._implied ? 136 : 135, text);
      node.Type = 135;
      return node;
    }

    private void InitializeAndAddFromElement(
      FromElement element,
      string className,
      string classAlias,
      IEntityPersister entityPersister,
      EntityType type,
      string tableAlias)
    {
      if (tableAlias == null)
        tableAlias = this._fromClause.AliasGenerator.CreateName(entityPersister.EntityName);
      element.InitializeEntity(this._fromClause, className, entityPersister, type, classAlias, tableAlias);
    }

    private FromElement CreateAndAddFromElement(
      string className,
      string classAlias,
      IEntityPersister entityPersister,
      EntityType type,
      string tableAlias)
    {
      FromElement element = entityPersister is IJoinable ? this.CreateFromElement(entityPersister) : throw new ArgumentException("EntityPersister " + (object) entityPersister + " does not implement Joinable!");
      this.InitializeAndAddFromElement(element, className, classAlias, entityPersister, type, tableAlias);
      return element;
    }

    private string[] Columns
    {
      get
      {
        return this._columns != null ? this._columns : throw new InvalidOperationException("No foriegn key columns were supplied!");
      }
    }

    public FromElement CreateComponentJoin(ComponentType type)
    {
      return (FromElement) new ComponentJoin(this._fromClause, this._origin, this._classAlias, this._path, type);
    }
  }
}
