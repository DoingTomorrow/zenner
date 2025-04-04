// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlIdWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlIdWriter : NullMappingModelVisitor, IXmlWriter<IdMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlIdWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(IdMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessId(IdMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "id");
      if (mapping.IsSpecified("Access"))
        element.WithAtt("access", mapping.Access);
      if (mapping.IsSpecified("Name"))
        element.WithAtt("name", mapping.Name);
      if (mapping.IsSpecified("Type"))
        element.WithAtt("type", mapping.Type);
      if (!mapping.IsSpecified("UnsavedValue"))
        return;
      element.WithAtt("unsaved-value", mapping.UnsavedValue);
    }

    public override void Visit(GeneratorMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<GeneratorMapping>().Write(mapping));
    }

    public override void Visit(ColumnMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(mapping));
    }
  }
}
