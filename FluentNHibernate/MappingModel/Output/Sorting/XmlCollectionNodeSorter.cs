// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.Sorting.XmlCollectionNodeSorter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output.Sorting
{
  public class XmlCollectionNodeSorter : BaseXmlNodeSorter
  {
    protected override IDictionary<string, SortValue> GetSorting()
    {
      return (IDictionary<string, SortValue>) new Dictionary<string, SortValue>()
      {
        {
          "meta",
          new SortValue() { Position = 0, Level = 1 }
        },
        {
          "jcs-cache",
          new SortValue() { Position = 0, Level = 1 }
        },
        {
          "cache",
          new SortValue() { Position = 0, Level = 1 }
        },
        {
          "key",
          new SortValue() { Position = 0, Level = 3 }
        },
        {
          "index",
          new SortValue() { Position = 0, Level = 4 }
        },
        {
          "list-index",
          new SortValue() { Position = 0, Level = 4 }
        },
        {
          "index-many-to-many",
          new SortValue() { Position = 0, Level = 4 }
        },
        {
          "element",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "one-to-many",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "many-to-many",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "composite-element",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "many-to-any",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "filter",
          new SortValue() { Position = 2, Level = 1 }
        }
      };
    }

    protected override void SortChildren(XmlNode node)
    {
    }
  }
}
