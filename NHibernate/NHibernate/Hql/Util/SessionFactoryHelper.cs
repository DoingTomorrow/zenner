// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Util.SessionFactoryHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Util
{
  public class SessionFactoryHelper
  {
    private readonly ISessionFactoryImplementor sfi;
    private readonly IDictionary<string, CollectionPropertyMapping> collectionPropertyMappingByRole = (IDictionary<string, CollectionPropertyMapping>) new Dictionary<string, CollectionPropertyMapping>();

    public SessionFactoryHelper(ISessionFactoryImplementor sfi) => this.sfi = sfi;

    public IQueryable FindQueryableUsingImports(string className)
    {
      return this.FindEntityPersisterUsingImports(className) as IQueryable;
    }

    public IEntityPersister FindEntityPersisterUsingImports(string className)
    {
      if (string.IsNullOrEmpty(className))
        return (IEntityPersister) null;
      if (!char.IsLetter(className[0]) && !className[0].Equals('_'))
        return (IEntityPersister) null;
      IEntityPersister entityPersister = this.sfi.TryGetEntityPersister(SessionFactoryHelper.GetEntityName(className));
      if (entityPersister != null)
        return entityPersister;
      string importedClassName = this.sfi.GetImportedClassName(className);
      return importedClassName == null ? (IEntityPersister) null : this.sfi.TryGetEntityPersister(SessionFactoryHelper.GetEntityName(importedClassName));
    }

    private static string GetEntityName(string assemblyQualifiedName)
    {
      return TypeNameParser.Parse(assemblyQualifiedName).Type;
    }

    public IQueryableCollection GetCollectionPersister(string role)
    {
      try
      {
        return (IQueryableCollection) this.sfi.GetCollectionPersister(role);
      }
      catch (InvalidCastException ex)
      {
        throw new QueryException("collection is not queryable: " + role);
      }
      catch (Exception ex)
      {
        throw new QueryException("collection not found: " + role);
      }
    }

    public IEntityPersister RequireClassPersister(string name)
    {
      return this.FindEntityPersisterByName(name) ?? throw new QuerySyntaxException(name + " is not mapped");
    }

    private IEntityPersister FindEntityPersisterByName(string name)
    {
      try
      {
        return this.sfi.GetEntityPersister(name);
      }
      catch (MappingException ex)
      {
      }
      string importedClassName = this.sfi.GetImportedClassName(name);
      return importedClassName == null ? (IEntityPersister) null : this.sfi.GetEntityPersister(importedClassName);
    }

    public Type GetImportedClass(string className)
    {
      string importedClassName = this.sfi.GetImportedClassName(className);
      return importedClassName == null ? (Type) null : Type.GetType(importedClassName, false);
    }

    public IPropertyMapping GetCollectionPropertyMapping(string role)
    {
      return (IPropertyMapping) this.collectionPropertyMappingByRole[role];
    }

    public IQueryableCollection RequireQueryableCollection(string role)
    {
      try
      {
        IQueryableCollection collectionPersister = (IQueryableCollection) this.sfi.GetCollectionPersister(role);
        if (collectionPersister != null)
          this.collectionPropertyMappingByRole.Add(role, new CollectionPropertyMapping(collectionPersister));
        return collectionPersister;
      }
      catch (InvalidCastException ex)
      {
        throw new QueryException("collection role is not queryable: " + role);
      }
      catch (Exception ex)
      {
        throw new QueryException("collection role not found: " + role);
      }
    }
  }
}
