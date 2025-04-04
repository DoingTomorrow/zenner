// Decompiled with JetBrains decompiler
// Type: NHibernate.Properties.PropertyAccessorFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Properties
{
  public sealed class PropertyAccessorFactory
  {
    private const string DefaultAccessorName = "property";
    private static readonly IDictionary<string, IPropertyAccessor> accessors;
    [NonSerialized]
    private static readonly IPropertyAccessor MapAccessor = (IPropertyAccessor) new NHibernate.Properties.MapAccessor();

    static PropertyAccessorFactory()
    {
      PropertyAccessorFactory.accessors = (IDictionary<string, IPropertyAccessor>) new Dictionary<string, IPropertyAccessor>(19);
      PropertyAccessorFactory.accessors["property"] = (IPropertyAccessor) new BasicPropertyAccessor();
      PropertyAccessorFactory.accessors["field"] = (IPropertyAccessor) new FieldAccessor();
      PropertyAccessorFactory.accessors["backfield"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new BackFieldStrategy());
      PropertyAccessorFactory.accessors["readonly"] = (IPropertyAccessor) new ReadOnlyAccessor();
      PropertyAccessorFactory.accessors["field.camelcase"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new CamelCaseStrategy());
      PropertyAccessorFactory.accessors["field.camelcase-underscore"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new CamelCaseUnderscoreStrategy());
      PropertyAccessorFactory.accessors["field.camelcase-m-underscore"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new CamelCaseMUnderscoreStrategy());
      PropertyAccessorFactory.accessors["field.lowercase"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new LowerCaseStrategy());
      PropertyAccessorFactory.accessors["field.lowercase-underscore"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new LowerCaseUnderscoreStrategy());
      PropertyAccessorFactory.accessors["field.pascalcase-underscore"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new PascalCaseUnderscoreStrategy());
      PropertyAccessorFactory.accessors["field.pascalcase-m-underscore"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new PascalCaseMUnderscoreStrategy());
      PropertyAccessorFactory.accessors["field.pascalcase-m"] = (IPropertyAccessor) new FieldAccessor((IFieldNamingStrategy) new PascalCaseMStrategy());
      PropertyAccessorFactory.accessors["nosetter.camelcase"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new CamelCaseStrategy());
      PropertyAccessorFactory.accessors["nosetter.camelcase-underscore"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new CamelCaseUnderscoreStrategy());
      PropertyAccessorFactory.accessors["nosetter.camelcase-m-underscore"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new CamelCaseMUnderscoreStrategy());
      PropertyAccessorFactory.accessors["nosetter.lowercase"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new LowerCaseStrategy());
      PropertyAccessorFactory.accessors["nosetter.lowercase-underscore"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new LowerCaseUnderscoreStrategy());
      PropertyAccessorFactory.accessors["nosetter.pascalcase-underscore"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new PascalCaseUnderscoreStrategy());
      PropertyAccessorFactory.accessors["nosetter.pascalcase-m-underscore"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new PascalCaseMUnderscoreStrategy());
      PropertyAccessorFactory.accessors["nosetter.pascalcase-m"] = (IPropertyAccessor) new NoSetterAccessor((IFieldNamingStrategy) new PascalCaseMStrategy());
      PropertyAccessorFactory.accessors["embedded"] = (IPropertyAccessor) new EmbeddedPropertyAccessor();
      PropertyAccessorFactory.accessors["noop"] = (IPropertyAccessor) new NoopAccessor();
      PropertyAccessorFactory.accessors["none"] = (IPropertyAccessor) new NoopAccessor();
    }

    private PropertyAccessorFactory()
    {
    }

    public static IPropertyAccessor GetPropertyAccessor(string type)
    {
      if (string.IsNullOrEmpty(type))
        type = "property";
      IPropertyAccessor propertyAccessor;
      PropertyAccessorFactory.accessors.TryGetValue(type, out propertyAccessor);
      return propertyAccessor ?? PropertyAccessorFactory.ResolveCustomAccessor(type);
    }

    public static IPropertyAccessor GetPropertyAccessor(Property property, EntityMode? mode)
    {
      EntityMode mode1 = mode.HasValue ? mode.Value : EntityMode.Poco;
      switch (mode1)
      {
        case EntityMode.Poco:
          return PropertyAccessorFactory.GetPocoPropertyAccessor(property.PropertyAccessorName);
        case EntityMode.Map:
          return PropertyAccessorFactory.DynamicMapPropertyAccessor;
        case EntityMode.Xml:
          return PropertyAccessorFactory.GetXmlPropertyAccessor(property.GetAccessorPropertyName(mode1), property.Type, (ISessionFactoryImplementor) null);
        default:
          throw new MappingException("Unknown entity mode [" + (object) mode + "]");
      }
    }

    private static IPropertyAccessor GetXmlPropertyAccessor(
      string nodeName,
      IType type,
      ISessionFactoryImplementor factory)
    {
      return (IPropertyAccessor) new XmlAccessor(nodeName, type, factory);
    }

    private static IPropertyAccessor GetPocoPropertyAccessor(string accessorName)
    {
      string str = string.IsNullOrEmpty(accessorName) ? "property" : accessorName;
      IPropertyAccessor propertyAccessor;
      PropertyAccessorFactory.accessors.TryGetValue(str, out propertyAccessor);
      return propertyAccessor ?? PropertyAccessorFactory.ResolveCustomAccessor(str);
    }

    private static IPropertyAccessor ResolveCustomAccessor(string accessorName)
    {
      System.Type type;
      try
      {
        type = ReflectHelper.ClassForName(accessorName);
      }
      catch (TypeLoadException ex)
      {
        throw new MappingException("could not find PropertyAccessor type: " + accessorName, (Exception) ex);
      }
      try
      {
        IPropertyAccessor instance = (IPropertyAccessor) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(type);
        PropertyAccessorFactory.accessors[accessorName] = instance;
        return instance;
      }
      catch (Exception ex)
      {
        throw new MappingException("could not instantiate PropertyAccessor class: " + accessorName, ex);
      }
    }

    public static IPropertyAccessor DynamicMapPropertyAccessor
    {
      get => PropertyAccessorFactory.MapAccessor;
    }
  }
}
