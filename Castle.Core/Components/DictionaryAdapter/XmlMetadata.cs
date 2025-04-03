// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XmlMetadata
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public class XmlMetadata
  {
    public XmlMetadata(
      Type type,
      XmlTypeAttribute xmlType,
      XmlRootAttribute xmlRoot,
      IEnumerable<Type> xmlIncludes)
    {
      this.Type = type;
      this.XmlType = xmlType;
      this.XmlRoot = xmlRoot;
      this.XmlIncludes = xmlIncludes;
    }

    public Type Type { get; private set; }

    public XmlTypeAttribute XmlType { get; private set; }

    public XmlRootAttribute XmlRoot { get; private set; }

    public IEnumerable<Type> XmlIncludes { get; private set; }
  }
}
