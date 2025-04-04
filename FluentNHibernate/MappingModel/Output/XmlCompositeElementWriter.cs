// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlCompositeElementWriter
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
  public class XmlCompositeElementWriter : 
    NullMappingModelVisitor,
    IXmlWriter<CompositeElementMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    protected XmlDocument document;

    public XmlCompositeElementWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(CompositeElementMapping compositeElement)
    {
      this.document = (XmlDocument) null;
      compositeElement.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessCompositeElement(CompositeElementMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, mapping is NestedCompositeElementMapping ? "nested-composite-element" : "composite-element");
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      if (!(mapping is NestedCompositeElementMapping))
        return;
      element.WithAtt("name", ((NestedCompositeElementMapping) mapping).Name);
    }

    public override void Visit(CompositeElementMapping compositeElementMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<CompositeElementMapping>().Write(compositeElementMapping));
    }

    public override void Visit(PropertyMapping propertyMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<PropertyMapping>().Write(propertyMapping));
    }

    public override void Visit(ManyToOneMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ManyToOneMapping>().Write(mapping));
    }

    public override void Visit(ParentMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ParentMapping>().Write(mapping));
    }
  }
}
