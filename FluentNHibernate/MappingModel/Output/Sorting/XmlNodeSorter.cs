// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.Sorting.XmlNodeSorter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output.Sorting
{
  public class XmlNodeSorter
  {
    public static XmlNode SortClassChildren(XmlNode node)
    {
      return new XmlClasslikeNodeSorter().Sort(node);
    }
  }
}
