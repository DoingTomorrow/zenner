// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.Sorting.BaseXmlNodeSorter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output.Sorting
{
  public abstract class BaseXmlNodeSorter
  {
    protected const int First = 0;
    protected const int Anywhere = 1;
    protected const int Last = 2;

    public XmlNode Sort(XmlNode node)
    {
      List<XmlNode> xmlNodeList = new List<XmlNode>();
      IDictionary<string, SortValue> sorting = this.GetSorting();
      foreach (XmlNode childNode in node.ChildNodes)
      {
        xmlNodeList.Add(childNode);
        this.SortChildren(childNode);
      }
      XmlNode[] originalSortOrder = xmlNodeList.ToArray();
      xmlNodeList.Sort((Comparison<XmlNode>) ((x, y) =>
      {
        if (!sorting.ContainsKey(x.Name) || !sorting.ContainsKey(y.Name))
          return 0;
        SortValue sortValue1 = sorting[x.Name];
        SortValue sortValue2 = sorting[y.Name];
        if (sortValue1.Position != sortValue2.Position)
          return sortValue1.Position.CompareTo(sortValue2.Position);
        return sortValue1.Level != sortValue2.Level ? sortValue1.Level.CompareTo(sortValue2.Level) : Array.IndexOf<XmlNode>(originalSortOrder, x).CompareTo(Array.IndexOf<XmlNode>(originalSortOrder, y));
      }));
      while (node.ChildNodes.Count > 0)
        node.RemoveChild(node.ChildNodes[0]);
      foreach (XmlNode newChild in xmlNodeList)
        node.AppendChild(newChild);
      return node;
    }

    protected abstract IDictionary<string, SortValue> GetSorting();

    protected abstract void SortChildren(XmlNode node);
  }
}
