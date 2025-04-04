// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.Binder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public abstract class Binder
  {
    protected static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Binder));
    protected static readonly IDictionary<string, MetaAttribute> EmptyMeta = (IDictionary<string, MetaAttribute>) new CollectionHelper.EmptyMapClass<string, MetaAttribute>();
    protected readonly Mappings mappings;

    protected Binder(Mappings mappings)
    {
      this.mappings = mappings != null ? mappings : throw new ArgumentNullException(nameof (mappings));
    }

    public Mappings Mappings => this.mappings;

    protected static string FullQualifiedClassName(string className, Mappings mappings)
    {
      return className == null ? (string) null : TypeNameParser.Parse(className, mappings.DefaultNamespace, mappings.DefaultAssembly).ToString();
    }

    protected static string FullClassName(string className, Mappings mappings)
    {
      return className == null ? (string) null : TypeNameParser.Parse(className, mappings.DefaultNamespace, mappings.DefaultAssembly).Type;
    }

    protected static bool NeedQualifiedClassName(string className)
    {
      return !string.IsNullOrEmpty(className) && className.IndexOf('.') <= 0 && TypeFactory.Basic(className) == null;
    }

    protected static System.Type ClassForFullNameChecked(string fullName, string errorMessage)
    {
      try
      {
        return ReflectHelper.ClassForName(fullName);
      }
      catch (Exception ex)
      {
        throw new MappingException(string.Format(errorMessage, (object) fullName), ex);
      }
    }

    protected static System.Type ClassForNameChecked(
      string name,
      Mappings mappings,
      string errorMessage)
    {
      return Binder.ClassForFullNameChecked(Binder.FullQualifiedClassName(name, mappings), errorMessage);
    }

    protected static string GetClassName(string unqualifiedName, Mappings mappings)
    {
      return Binder.ClassForNameChecked(unqualifiedName, mappings, "unknown class {0}").FullName;
    }

    protected static string GetQualifiedClassName(string unqualifiedName, Mappings mappings)
    {
      return Binder.ClassForNameChecked(unqualifiedName, mappings, "unknown class {0}").AssemblyQualifiedName;
    }

    public static IDictionary<string, MetaAttribute> GetMetas(
      IDecoratable decoratable,
      IDictionary<string, MetaAttribute> inheritedMeta)
    {
      return Binder.GetMetas(decoratable, inheritedMeta, false);
    }

    public static IDictionary<string, MetaAttribute> GetMetas(
      IDecoratable decoratable,
      IDictionary<string, MetaAttribute> inheritedMeta,
      bool onlyInheritable)
    {
      if (decoratable == null)
        return Binder.EmptyMeta;
      Dictionary<string, MetaAttribute> metas = new Dictionary<string, MetaAttribute>(inheritedMeta);
      foreach (KeyValuePair<string, MetaAttribute> keyValuePair in onlyInheritable ? (IEnumerable<KeyValuePair<string, MetaAttribute>>) decoratable.InheritableMetaData : (IEnumerable<KeyValuePair<string, MetaAttribute>>) decoratable.MappedMetaData)
      {
        string key = keyValuePair.Key;
        MetaAttribute metaAttribute1;
        metas.TryGetValue(key, out metaAttribute1);
        MetaAttribute metaAttribute2;
        inheritedMeta.TryGetValue(key, out metaAttribute2);
        if (metaAttribute1 == null)
        {
          metaAttribute1 = new MetaAttribute(key);
          metas[key] = metaAttribute1;
        }
        else if (metaAttribute1 == metaAttribute2)
        {
          metaAttribute1 = new MetaAttribute(key);
          metas[key] = metaAttribute1;
        }
        metaAttribute1.AddValues((IEnumerable<string>) keyValuePair.Value.Values);
      }
      return (IDictionary<string, MetaAttribute>) metas;
    }
  }
}
