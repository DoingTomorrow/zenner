// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.SessionFactoryHelperExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Util;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR
{
  [CLSCompliant(false)]
  public class SessionFactoryHelperExtensions
  {
    private readonly ISessionFactoryImplementor _sfi;
    private readonly NullableDictionary<string, IPropertyMapping> _collectionPropertyMappingByRole;
    private readonly SessionFactoryHelper helper;

    public SessionFactoryHelperExtensions(ISessionFactoryImplementor sfi)
    {
      this._sfi = sfi;
      this.helper = new SessionFactoryHelper(this._sfi);
      this._collectionPropertyMappingByRole = new NullableDictionary<string, IPropertyMapping>();
    }

    public ISessionFactoryImplementor Factory => this._sfi;

    public ISQLFunction FindSQLFunction(string functionName)
    {
      return this._sfi.SQLFunctionRegistry.FindSQLFunction(functionName.ToLowerInvariant());
    }

    private ISQLFunction RequireSQLFunction(string functionName)
    {
      return this.FindSQLFunction(functionName) ?? throw new QueryException("Unable to find SQL function: " + functionName);
    }

    public IType FindFunctionReturnType(string functionName, IASTNode first)
    {
      ISQLFunction sqlFunction = this.RequireSQLFunction(functionName);
      IType columnType = (IType) null;
      if (first != null)
      {
        if (functionName == "cast")
          columnType = TypeFactory.HeuristicType(first.NextSibling.Text);
        else if (first is SqlNode)
          columnType = ((SqlNode) first).DataType;
      }
      return sqlFunction.ReturnType(columnType, (IMapping) this._sfi);
    }

    public string GetImportedClassName(string className)
    {
      return this._sfi.GetImportedClassName(className);
    }

    public bool HasPhysicalDiscriminatorColumn(IQueryable persister)
    {
      if (persister.DiscriminatorType != null)
      {
        string discriminatorColumnName = persister.DiscriminatorColumnName;
        if (discriminatorColumnName != null && "clazz_" != discriminatorColumnName)
          return true;
      }
      return false;
    }

    public IQueryableCollection GetCollectionPersister(string collectionFilterRole)
    {
      try
      {
        return (IQueryableCollection) this._sfi.GetCollectionPersister(collectionFilterRole);
      }
      catch (InvalidCastException ex)
      {
        throw new QueryException("collection is not queryable: " + collectionFilterRole, (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new QueryException("collection not found: " + collectionFilterRole, ex);
      }
    }

    public string GetIdentifierOrUniqueKeyPropertyName(EntityType entityType)
    {
      try
      {
        return entityType.GetIdentifierOrUniqueKeyPropertyName((IMapping) this._sfi);
      }
      catch (MappingException ex)
      {
        throw new QueryException((Exception) ex);
      }
    }

    public string[] GetCollectionElementColumns(string role, string roleAlias)
    {
      return this.GetCollectionPropertyMapping(role).ToColumns(roleAlias, "elements");
    }

    public IAssociationType GetElementAssociationType(CollectionType collectionType)
    {
      return (IAssociationType) this.GetElementType(collectionType);
    }

    public IQueryableCollection RequireQueryableCollection(string role)
    {
      try
      {
        IQueryableCollection collectionPersister = (IQueryableCollection) this._sfi.GetCollectionPersister(role);
        if (collectionPersister != null)
          this._collectionPropertyMappingByRole.Add(role, (IPropertyMapping) new CollectionPropertyMapping(collectionPersister));
        return collectionPersister;
      }
      catch (InvalidCastException ex)
      {
        throw new QueryException("collection role is not queryable: " + role, (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new QueryException("collection role not found: " + role, ex);
      }
    }

    public IEntityPersister RequireClassPersister(string name)
    {
      IEntityPersister entityPersisterByName;
      try
      {
        entityPersisterByName = this.FindEntityPersisterByName(name);
        if (entityPersisterByName == null)
          throw new QuerySyntaxException(name + " is not mapped");
      }
      catch (MappingException ex)
      {
        throw new QueryException(ex.Message, (Exception) ex);
      }
      return entityPersisterByName;
    }

    public IQueryable FindQueryableUsingImports(string className)
    {
      return SessionFactoryHelperExtensions.FindQueryableUsingImports(this._sfi, className);
    }

    private static IQueryable FindQueryableUsingImports(
      ISessionFactoryImplementor sfi,
      string className)
    {
      return new SessionFactoryHelper(sfi).FindQueryableUsingImports(className);
    }

    private IEntityPersister FindEntityPersisterByName(string name)
    {
      return this.helper.FindEntityPersisterUsingImports(name);
    }

    public JoinSequence CreateCollectionJoinSequence(
      IQueryableCollection collPersister,
      string collectionName)
    {
      JoinSequence joinSequence = this.CreateJoinSequence();
      joinSequence.SetRoot((IJoinable) collPersister, collectionName);
      joinSequence.SetUseThetaStyle(true);
      return joinSequence;
    }

    public JoinSequence CreateJoinSequence() => new JoinSequence(this._sfi);

    public JoinSequence CreateJoinSequence(
      bool implicitJoin,
      IAssociationType associationType,
      string tableAlias,
      JoinType joinType,
      string[] columns)
    {
      JoinSequence joinSequence = this.CreateJoinSequence();
      joinSequence.SetUseThetaStyle(implicitJoin);
      joinSequence.AddJoin(associationType, tableAlias, joinType, columns);
      return joinSequence;
    }

    public string[][] GenerateColumnNames(IType[] sqlResultTypes)
    {
      return NameGenerator.GenerateColumnNames(sqlResultTypes, this._sfi);
    }

    public bool IsStrictJPAQLComplianceEnabled => false;

    private IPropertyMapping GetCollectionPropertyMapping(string role)
    {
      return this._collectionPropertyMappingByRole[role];
    }

    private IType GetElementType(CollectionType collectionType)
    {
      return collectionType.GetElementType(this._sfi);
    }
  }
}
