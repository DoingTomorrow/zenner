// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.IXPathSerializer
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  public interface IXPathSerializer
  {
    bool WriteObject(XPathResult result, XPathNavigator node, object value);

    bool ReadObject(XPathResult result, XPathNavigator node, out object value);
  }
}
