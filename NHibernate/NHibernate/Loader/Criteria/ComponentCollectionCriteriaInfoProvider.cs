// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Criteria.ComponentCollectionCriteriaInfoProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Criteria
{
  public class ComponentCollectionCriteriaInfoProvider : ICriteriaInfoProvider
  {
    private readonly IQueryableCollection persister;
    private readonly IDictionary<string, IType> subTypes = (IDictionary<string, IType>) new Dictionary<string, IType>();

    public ComponentCollectionCriteriaInfoProvider(IQueryableCollection persister)
    {
      this.persister = persister;
      if (!persister.ElementType.IsComponentType)
        throw new ArgumentException("persister for role " + persister.Role + " is not a collection-of-component");
      IAbstractComponentType elementType = (IAbstractComponentType) persister.ElementType;
      string[] propertyNames = elementType.PropertyNames;
      IType[] subtypes = elementType.Subtypes;
      for (int index = 0; index < propertyNames.Length; ++index)
        this.subTypes.Add(propertyNames[index], subtypes[index]);
    }

    public string Name => this.persister.Role;

    public string[] Spaces => this.persister.CollectionSpaces;

    public IPropertyMapping PropertyMapping => (IPropertyMapping) this.persister;

    public IType GetType(string relativePath)
    {
      if (relativePath.IndexOf('.') >= 0)
        throw new ArgumentException("dotted paths not handled (yet?!) for collection-of-component");
      IType type;
      if (!this.subTypes.TryGetValue(relativePath, out type))
        throw new ArgumentException("property " + relativePath + " not found in component of collection " + this.Name);
      return type;
    }
  }
}
