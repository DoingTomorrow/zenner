// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.PropertyFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Type;
using NHibernate.Util;
using System.Reflection;

#nullable disable
namespace NHibernate.Tuple
{
  public class PropertyFactory
  {
    public static IdentifierProperty BuildIdentifierProperty(
      PersistentClass mappedEntity,
      IIdentifierGenerator generator)
    {
      string nullValue = mappedEntity.Identifier.NullValue;
      IType type = mappedEntity.Identifier.Type;
      NHibernate.Mapping.Property identifierProperty = mappedEntity.IdentifierProperty;
      IdentifierValue unsavedIdentifierValue = UnsavedValueFactory.GetUnsavedIdentifierValue(nullValue, PropertyFactory.GetGetter(identifierProperty), type, PropertyFactory.GetConstructor(mappedEntity));
      return identifierProperty == null ? new IdentifierProperty(type, mappedEntity.HasEmbeddedIdentifier, mappedEntity.HasIdentifierMapper, unsavedIdentifierValue, generator) : new IdentifierProperty(identifierProperty.Name, identifierProperty.NodeName, type, mappedEntity.HasEmbeddedIdentifier, unsavedIdentifierValue, generator);
    }

    public static VersionProperty BuildVersionProperty(NHibernate.Mapping.Property property, bool lazyAvailable)
    {
      VersionValue unsavedVersionValue = UnsavedValueFactory.GetUnsavedVersionValue(((IKeyValue) property.Value).NullValue, PropertyFactory.GetGetter(property), (IVersionType) property.Type, PropertyFactory.GetConstructor(property.PersistentClass));
      bool lazy = lazyAvailable && property.IsLazy;
      return new VersionProperty(property.Name, property.NodeName, property.Value.Type, lazy, property.IsInsertable, property.IsUpdateable, property.Generation == PropertyGeneration.Insert || property.Generation == PropertyGeneration.Always, property.Generation == PropertyGeneration.Always, property.IsOptional, property.IsUpdateable && !lazy, property.IsOptimisticLocked, property.CascadeStyle, unsavedVersionValue);
    }

    public static StandardProperty BuildStandardProperty(NHibernate.Mapping.Property property, bool lazyAvailable)
    {
      IType type = property.Value.Type;
      bool flag = type.IsAssociationType && ((IAssociationType) type).IsAlwaysDirtyChecked;
      return new StandardProperty(property.Name, property.NodeName, type, lazyAvailable && property.IsLazy, property.IsInsertable, property.IsUpdateable, property.Generation == PropertyGeneration.Insert || property.Generation == PropertyGeneration.Always, property.Generation == PropertyGeneration.Always, property.IsOptional, flag || property.IsUpdateable, property.IsOptimisticLocked, property.CascadeStyle, new FetchMode?(property.Value.FetchMode));
    }

    private static ConstructorInfo GetConstructor(PersistentClass persistentClass)
    {
      if (persistentClass != null)
      {
        if (persistentClass.HasPocoRepresentation)
        {
          try
          {
            return ReflectHelper.GetDefaultConstructor(persistentClass.MappedClass);
          }
          catch
          {
            return (ConstructorInfo) null;
          }
        }
      }
      return (ConstructorInfo) null;
    }

    private static IGetter GetGetter(NHibernate.Mapping.Property mappingProperty)
    {
      return mappingProperty == null || !mappingProperty.PersistentClass.HasPocoRepresentation ? (IGetter) null : PropertyAccessorFactory.GetPropertyAccessor(mappingProperty, new EntityMode?(EntityMode.Poco)).GetGetter(mappingProperty.PersistentClass.MappedClass, mappingProperty.Name);
    }
  }
}
