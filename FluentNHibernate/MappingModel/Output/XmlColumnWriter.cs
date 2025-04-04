// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlColumnWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlColumnWriter : NullMappingModelVisitor, IXmlWriter<ColumnMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(ColumnMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessColumn(ColumnMapping columnMapping)
    {
      this.document = new XmlDocument();
      XmlElement element = this.document.CreateElement("column");
      if (columnMapping.IsSpecified("Name"))
        element.WithAtt("name", columnMapping.Name);
      if (columnMapping.IsSpecified("Check"))
        element.WithAtt("check", columnMapping.Check);
      if (columnMapping.IsSpecified("Length"))
        element.WithAtt("length", columnMapping.Length);
      if (columnMapping.IsSpecified("Index"))
        element.WithAtt("index", columnMapping.Index);
      if (columnMapping.IsSpecified("NotNull"))
        element.WithAtt("not-null", columnMapping.NotNull);
      if (columnMapping.IsSpecified("SqlType"))
        element.WithAtt("sql-type", columnMapping.SqlType);
      if (columnMapping.IsSpecified("Unique"))
        element.WithAtt("unique", columnMapping.Unique);
      if (columnMapping.IsSpecified("UniqueKey"))
        element.WithAtt("unique-key", columnMapping.UniqueKey);
      if (columnMapping.IsSpecified("Precision"))
        element.WithAtt("precision", columnMapping.Precision);
      if (columnMapping.IsSpecified("Scale"))
        element.WithAtt("scale", columnMapping.Scale);
      if (columnMapping.IsSpecified("Default"))
        element.WithAtt("default", columnMapping.Default);
      this.document.AppendChild((XmlNode) element);
    }
  }
}
