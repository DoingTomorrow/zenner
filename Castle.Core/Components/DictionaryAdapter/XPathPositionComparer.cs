// Decompiled with JetBrains decompiler
// Type: Castle.Components.DictionaryAdapter.XPathPositionComparer
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace Castle.Components.DictionaryAdapter
{
  internal class XPathPositionComparer : IComparer<XPathNavigator>
  {
    public static readonly XPathPositionComparer Instance = new XPathPositionComparer();

    private XPathPositionComparer()
    {
    }

    public int Compare(XPathNavigator x, XPathNavigator y)
    {
      switch (x.ComparePosition(y))
      {
        case XmlNodeOrder.Before:
          return -1;
        case XmlNodeOrder.After:
          return 1;
        default:
          return 0;
      }
    }
  }
}
