// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.Sorting.XmlClasslikeNodeSorter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output.Sorting
{
  public class XmlClasslikeNodeSorter : BaseXmlNodeSorter
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
          "subselect",
          new SortValue() { Position = 0, Level = 2 }
        },
        {
          "cache",
          new SortValue() { Position = 0, Level = 3 }
        },
        {
          "synchronize",
          new SortValue() { Position = 0, Level = 4 }
        },
        {
          "comment",
          new SortValue() { Position = 0, Level = 5 }
        },
        {
          "tuplizer",
          new SortValue() { Position = 0, Level = 6 }
        },
        {
          "key",
          new SortValue() { Position = 0, Level = 7 }
        },
        {
          "parent",
          new SortValue() { Position = 0, Level = 7 }
        },
        {
          "id",
          new SortValue() { Position = 0, Level = 7 }
        },
        {
          "composite-id",
          new SortValue() { Position = 0, Level = 7 }
        },
        {
          "discriminator",
          new SortValue() { Position = 0, Level = 8 }
        },
        {
          "natural-id",
          new SortValue() { Position = 0, Level = 9 }
        },
        {
          "version",
          new SortValue() { Position = 0, Level = 10 }
        },
        {
          "timestamp",
          new SortValue() { Position = 0, Level = 10 }
        },
        {
          "property",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "many-to-one",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "one-to-one",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "component",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "dynamic-component",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "properties",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "any",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "map",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "set",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "list",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "bag",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "idbag",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "array",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "primitive-array",
          new SortValue() { Position = 1, Level = 1 }
        },
        {
          "join",
          new SortValue() { Position = 2, Level = 1 }
        },
        {
          "subclass",
          new SortValue() { Position = 2, Level = 2 }
        },
        {
          "joined-subclass",
          new SortValue() { Position = 2, Level = 3 }
        },
        {
          "union-subclass",
          new SortValue() { Position = 2, Level = 4 }
        },
        {
          "loader",
          new SortValue() { Position = 2, Level = 5 }
        },
        {
          "sql-insert",
          new SortValue() { Position = 2, Level = 6 }
        },
        {
          "sql-update",
          new SortValue() { Position = 2, Level = 7 }
        },
        {
          "sql-delete",
          new SortValue() { Position = 2, Level = 8 }
        },
        {
          "filter",
          new SortValue() { Position = 2, Level = 9 }
        },
        {
          "query",
          new SortValue() { Position = 2, Level = 10 }
        },
        {
          "sql-query",
          new SortValue() { Position = 2, Level = 11 }
        }
      };
    }

    protected override void SortChildren(XmlNode node)
    {
      if (node.Name == "subclass" || node.Name == "joined-subclass" || node.Name == "union-subclass" || node.Name == "component")
        this.Sort(node);
      else if (node.Name == "id")
      {
        new XmlIdNodeSorter().Sort(node);
      }
      else
      {
        if (!this.IsCollection(node.Name))
          return;
        new XmlCollectionNodeSorter().Sort(node);
      }
    }

    private bool IsCollection(string name)
    {
      return name == "bag" || name == "set" || name == "list" || name == "map" || name == "array" || name == "primitive-array";
    }
  }
}
