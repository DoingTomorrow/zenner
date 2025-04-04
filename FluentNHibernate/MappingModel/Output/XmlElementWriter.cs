// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlElementWriter
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
  public class XmlElementWriter : NullMappingModelVisitor, IXmlWriter<ElementMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlElementWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(ElementMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessElement(ElementMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "element");
      if (mapping.IsSpecified("Type"))
        element.WithAtt("type", mapping.Type);
      if (!mapping.IsSpecified("Formula"))
        return;
      element.WithAtt("formula", mapping.Formula);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(columnMapping));
    }
  }
}
