// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathExtensions
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public static class XPathExtensions
  {
    private const string XmlMetaKey = "XmlMeta";

    public static XmlMetadata GetXmlMeta(this IDictionaryAdapter dictionaryAdapter)
    {
      return dictionaryAdapter.GetXmlMeta((Type) null);
    }

    public static XmlMetadata GetXmlMeta(this IDictionaryAdapter dictionaryAdapter, Type otherType)
    {
      return otherType == null || otherType.IsInterface ? (XmlMetadata) XPathExtensions.GetDictionaryMeta(dictionaryAdapter, otherType).ExtendedProperties[(object) "XmlMeta"] : (XmlMetadata) null;
    }

    internal static XmlMetadata GetXmlMeta(this DictionaryAdapterMeta dictionaryAdapterMeta)
    {
      return (XmlMetadata) dictionaryAdapterMeta.ExtendedProperties[(object) "XmlMeta"];
    }

    internal static void SetXmlMeta(
      this DictionaryAdapterMeta dictionaryAdapterMeta,
      XmlMetadata xmlMeta)
    {
      dictionaryAdapterMeta.ExtendedProperties[(object) "XmlMeta"] = (object) xmlMeta;
    }

    public static Type GetXmlSubclass(
      this IDictionaryAdapter dictionaryAdapter,
      XmlQualifiedName xmlType,
      Type otherType)
    {
      if (xmlType == (XmlQualifiedName) null)
        return (Type) null;
      IEnumerable<Type> xmlIncludes = dictionaryAdapter.GetXmlMeta(otherType).XmlIncludes;
      return xmlIncludes != null ? xmlIncludes.Select(xmlInclude => new
      {
        xmlInclude = xmlInclude,
        xmlIncludeType = dictionaryAdapter.GetXmlMeta(xmlInclude).XmlType
      }).Where(_param1 => _param1.xmlIncludeType.TypeName == xmlType.Name && _param1.xmlIncludeType.Namespace == xmlType.Namespace).Select(_param0 => _param0.xmlInclude).FirstOrDefault<Type>() : (Type) null;
    }

    private static DictionaryAdapterMeta GetDictionaryMeta(
      IDictionaryAdapter dictionaryAdapter,
      Type otherType)
    {
      DictionaryAdapterMeta dictionaryMeta = dictionaryAdapter.Meta;
      if (otherType != null && otherType != dictionaryMeta.Type)
        dictionaryMeta = dictionaryAdapter.This.Factory.GetAdapterMeta(otherType, new DictionaryDescriptor().AddBehavior((IDictionaryBehavior) XPathBehavior.Instance));
      return dictionaryMeta;
    }
  }
}
