// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlIndexWriter
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
  public class XmlIndexWriter : NullMappingModelVisitor, IXmlWriter<IndexMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlIndexWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(IndexMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessIndex(IndexMapping mapping)
    {
      this.document = new XmlDocument();
      if (mapping.IsSpecified("Offset"))
        this.WriteListIndex(mapping);
      else
        this.WriteIndex(mapping);
    }

    private void WriteIndex(IndexMapping mapping)
    {
      XmlElement element = XmlExtensions.AddElement(this.document, "index");
      if (!mapping.IsSpecified("Type"))
        return;
      element.WithAtt("type", mapping.Type);
    }

    private void WriteListIndex(IndexMapping mapping)
    {
      XmlExtensions.AddElement(this.document, "list-index").WithAtt("base", mapping.Offset);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(columnMapping));
    }
  }
}
