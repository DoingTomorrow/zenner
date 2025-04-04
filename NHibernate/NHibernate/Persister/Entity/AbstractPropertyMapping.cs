// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.AbstractPropertyMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public abstract class AbstractPropertyMapping : IPropertyMapping
  {
    private readonly Dictionary<string, IType> typesByPropertyPath = new Dictionary<string, IType>();
    private readonly Dictionary<string, string[]> columnsByPropertyPath = new Dictionary<string, string[]>();
    private readonly Dictionary<string, string[]> formulaTemplatesByPropertyPath = new Dictionary<string, string[]>();

    public virtual string[] IdentifierColumnNames
    {
      get => throw new InvalidOperationException("one-to-one is not supported here");
    }

    protected abstract string EntityName { get; }

    protected QueryException PropertyException(string propertyName)
    {
      return new QueryException(string.Format("could not resolve property: {0} of: {1}", (object) propertyName, (object) this.EntityName));
    }

    public IType ToType(string propertyName)
    {
      IType type;
      if (!this.typesByPropertyPath.TryGetValue(propertyName, out type))
        throw this.PropertyException(propertyName);
      return type;
    }

    public bool TryToType(string propertyName, out IType type)
    {
      return this.typesByPropertyPath.TryGetValue(propertyName, out type);
    }

    public virtual string[] ToColumns(string alias, string propertyName)
    {
      string[] columns1 = this.GetColumns(propertyName);
      string[] strArray;
      this.formulaTemplatesByPropertyPath.TryGetValue(propertyName, out strArray);
      string[] columns2 = new string[columns1.Length];
      for (int index = 0; index < columns1.Length; ++index)
        columns2[index] = columns1[index] != null ? StringHelper.Qualify(alias, columns1[index]) : StringHelper.Replace(strArray[index], Template.Placeholder, alias);
      return columns2;
    }

    private string[] GetColumns(string propertyName)
    {
      string[] columns;
      if (!this.columnsByPropertyPath.TryGetValue(propertyName, out columns))
        throw this.PropertyException(propertyName);
      return columns;
    }

    public virtual string[] ToColumns(string propertyName)
    {
      string[] columns1 = this.GetColumns(propertyName);
      string[] strArray;
      this.formulaTemplatesByPropertyPath.TryGetValue(propertyName, out strArray);
      string[] columns2 = new string[columns1.Length];
      for (int index = 0; index < columns1.Length; ++index)
        columns2[index] = columns1[index] != null ? columns1[index] : StringHelper.Replace(strArray[index], Template.Placeholder, string.Empty);
      return columns2;
    }

    public abstract IType Type { get; }

    protected void AddPropertyPath(
      string path,
      IType type,
      string[] columns,
      string[] formulaTemplates)
    {
      this.typesByPropertyPath[path] = type;
      this.columnsByPropertyPath[path] = columns;
      if (formulaTemplates == null)
        return;
      this.formulaTemplatesByPropertyPath[path] = formulaTemplates;
    }

    protected internal void InitPropertyPaths(
      string path,
      IType type,
      string[] columns,
      string[] formulaTemplates,
      IMapping factory)
    {
      if (columns.Length != type.GetColumnSpan(factory))
        throw new MappingException(string.Format("broken column mapping for: {0} of: {1}, type {2} expects {3} columns, but {4} were mapped", (object) path, (object) this.EntityName, (object) type.Name, (object) type.GetColumnSpan(factory), (object) columns.Length));
      if (type.IsAssociationType)
      {
        IAssociationType associationType = (IAssociationType) type;
        if (associationType.UseLHSPrimaryKey)
        {
          columns = this.IdentifierColumnNames;
        }
        else
        {
          string lhsPropertyName = associationType.LHSPropertyName;
          if (lhsPropertyName != null && !this.columnsByPropertyPath.TryGetValue(lhsPropertyName, out columns))
            return;
        }
      }
      if (path != null)
        this.AddPropertyPath(path, type, columns, formulaTemplates);
      if (type.IsComponentType)
      {
        IAbstractComponentType type1 = (IAbstractComponentType) type;
        this.InitComponentPropertyPaths(path, type1, columns, formulaTemplates, factory);
        if (!type1.IsEmbedded)
          return;
        this.InitComponentPropertyPaths(path == null ? (string) null : StringHelper.Qualifier(path), type1, columns, formulaTemplates, factory);
      }
      else
      {
        if (!type.IsEntityType)
          return;
        this.InitIdentifierPropertyPaths(path, (EntityType) type, columns, factory);
      }
    }

    protected void InitIdentifierPropertyPaths(
      string path,
      EntityType etype,
      string[] columns,
      IMapping factory)
    {
      IType identifierOrUniqueKeyType = etype.GetIdentifierOrUniqueKeyType(factory);
      string uniqueKeyPropertyName = etype.GetIdentifierOrUniqueKeyPropertyName(factory);
      bool flag = this.HasNonIdentifierPropertyNamedId(etype, factory);
      if (etype.IsReferenceToPrimaryKey && !flag)
      {
        string path1 = AbstractPropertyMapping.ExtendPath(path, EntityPersister.EntityID);
        this.AddPropertyPath(path1, identifierOrUniqueKeyType, columns, (string[]) null);
        this.InitPropertyPaths(path1, identifierOrUniqueKeyType, columns, (string[]) null, factory);
      }
      if (uniqueKeyPropertyName == null)
        return;
      string path2 = AbstractPropertyMapping.ExtendPath(path, uniqueKeyPropertyName);
      this.AddPropertyPath(path2, identifierOrUniqueKeyType, columns, (string[]) null);
      this.InitPropertyPaths(path2, identifierOrUniqueKeyType, columns, (string[]) null, factory);
    }

    private bool HasNonIdentifierPropertyNamedId(EntityType entityType, IMapping factory)
    {
      return factory.HasNonIdentifierPropertyNamedId(entityType.GetAssociatedEntityName());
    }

    protected void InitComponentPropertyPaths(
      string path,
      IAbstractComponentType type,
      string[] columns,
      string[] formulaTemplates,
      IMapping factory)
    {
      IType[] subtypes = type.Subtypes;
      string[] propertyNames = type.PropertyNames;
      int begin = 0;
      for (int index = 0; index < propertyNames.Length; ++index)
      {
        string path1 = AbstractPropertyMapping.ExtendPath(path, propertyNames[index]);
        try
        {
          int columnSpan = subtypes[index].GetColumnSpan(factory);
          string[] columns1 = ArrayHelper.Slice(columns, begin, columnSpan);
          string[] formulaTemplates1 = formulaTemplates == null ? (string[]) null : ArrayHelper.Slice(formulaTemplates, begin, columnSpan);
          this.InitPropertyPaths(path1, subtypes[index], columns1, formulaTemplates1, factory);
          begin += columnSpan;
        }
        catch (Exception ex)
        {
          throw new MappingException("bug in InitComponentPropertyPaths", ex);
        }
      }
    }

    private static string ExtendPath(string path, string property)
    {
      return string.IsNullOrEmpty(path) ? property : StringHelper.Qualify(path, property);
    }

    public string[] GetColumnNames(string propertyName)
    {
      string[] columnNames;
      if (!this.columnsByPropertyPath.TryGetValue(propertyName, out columnNames))
        throw new MappingException(string.Format("unknown property: {0} of: {1}", (object) propertyName, (object) this.EntityName));
      return columnNames;
    }
  }
}
