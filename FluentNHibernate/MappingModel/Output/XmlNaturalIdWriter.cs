// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlNaturalIdWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlNaturalIdWriter : NullMappingModelVisitor, IXmlWriter<NaturalIdMapping>
  {
    private IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlNaturalIdWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(NaturalIdMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessNaturalId(NaturalIdMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "natural-id");
      if (!mapping.IsSpecified("Mutable"))
        return;
      element.WithAtt("mutable", mapping.Mutable);
    }

    public override void Visit(PropertyMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<PropertyMapping>().Write(mapping));
    }

    public override void Visit(ManyToOneMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ManyToOneMapping>().Write(mapping));
    }
  }
}
