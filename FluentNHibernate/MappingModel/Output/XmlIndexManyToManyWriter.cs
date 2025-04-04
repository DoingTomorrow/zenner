// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlIndexManyToManyWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlIndexManyToManyWriter : NullMappingModelVisitor, IXmlWriter<IndexManyToManyMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlIndexManyToManyWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(IndexManyToManyMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessIndex(IndexManyToManyMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "index-many-to-many");
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      if (mapping.IsSpecified("ForeignKey"))
        element.WithAtt("foreign-key", mapping.ForeignKey);
      if (!mapping.IsSpecified("EntityName"))
        return;
      element.WithAtt("entity-name", mapping.EntityName);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(columnMapping));
    }
  }
}
