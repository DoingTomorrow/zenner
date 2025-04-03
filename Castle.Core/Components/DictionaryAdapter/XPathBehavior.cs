// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathBehavior
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class XPathBehavior : 
    DictionaryBehaviorAttribute,
    IDictionaryMetaInitializer,
    IDictionaryBehavior
  {
    public static readonly XPathBehavior Instance = new XPathBehavior();

    void IDictionaryMetaInitializer.Initialize(
      IDictionaryAdapterFactory factory,
      DictionaryAdapterMeta dictionaryMeta)
    {
      Type type = dictionaryMeta.Type;
      string defaultNamespace = (string) null;
      XmlTypeAttribute xmlType = (XmlTypeAttribute) null;
      XmlRootAttribute xmlRoot = (XmlRootAttribute) null;
      List<Type> xmlIncludes = (List<Type>) null;
      new BehaviorVisitor().OfType<XmlTypeAttribute>((Action<XmlTypeAttribute>) (attrib => xmlType = attrib)).OfType<XmlRootAttribute>((Action<XmlRootAttribute>) (attrib => xmlRoot = attrib)).OfType<XmlNamespaceAttribute>((Action<XmlNamespaceAttribute>) (attrib =>
      {
        if (!attrib.Default)
          return;
        defaultNamespace = attrib.NamespaceUri;
      })).OfType<XmlIncludeAttribute>((Action<XmlIncludeAttribute>) (attrib =>
      {
        xmlIncludes = xmlIncludes ?? new List<Type>();
        if (type == attrib.Type || !type.IsAssignableFrom(attrib.Type))
          return;
        xmlIncludes.Add(attrib.Type);
      })).Apply((IEnumerable) dictionaryMeta.Behaviors);
      if (xmlType == null)
      {
        xmlType = new XmlTypeAttribute()
        {
          TypeName = type.Name,
          Namespace = defaultNamespace
        };
        if (xmlType.TypeName.StartsWith("I"))
          xmlType.TypeName = xmlType.TypeName.Substring(1);
      }
      else if (xmlType.Namespace == null)
        xmlType.Namespace = defaultNamespace;
      dictionaryMeta.SetXmlMeta(new XmlMetadata(type, xmlType, xmlRoot, (IEnumerable<Type>) xmlIncludes));
    }
  }
}
