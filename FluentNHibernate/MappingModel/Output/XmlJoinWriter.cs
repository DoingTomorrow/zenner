// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlJoinWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlJoinWriter : NullMappingModelVisitor, IXmlWriter<JoinMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlJoinWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(JoinMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessJoin(JoinMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "join");
      if (mapping.IsSpecified("TableName"))
        element.WithAtt("table", mapping.TableName);
      if (mapping.IsSpecified("Schema"))
        element.WithAtt("schema", mapping.Schema);
      if (mapping.IsSpecified("Fetch"))
        element.WithAtt("fetch", mapping.Fetch);
      if (mapping.IsSpecified("Catalog"))
        element.WithAtt("catalog", mapping.Catalog);
      if (mapping.IsSpecified("Subselect"))
        element.WithAtt("subselect", mapping.Subselect);
      if (mapping.IsSpecified("Inverse"))
        element.WithAtt("inverse", mapping.Inverse);
      if (!mapping.IsSpecified("Optional"))
        return;
      element.WithAtt("optional", mapping.Optional);
    }

    public override void Visit(PropertyMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<PropertyMapping>().Write(mapping));
    }

    public override void Visit(KeyMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<KeyMapping>().Write(mapping));
    }

    public override void Visit(ManyToOneMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ManyToOneMapping>().Write(mapping));
    }

    public override void Visit(IComponentMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<IComponentMapping>().Write(mapping));
    }

    public override void Visit(AnyMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<AnyMapping>().Write(mapping));
    }

    public override void Visit(CollectionMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<CollectionMapping>().Write(mapping));
    }
  }
}
