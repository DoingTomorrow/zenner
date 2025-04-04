// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlSubclassWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlSubclassWriter : XmlClassWriterBase, IXmlWriter<SubclassMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;

    public XmlSubclassWriter(IXmlWriterServiceLocator serviceLocator)
      : base(serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(SubclassMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessSubclass(SubclassMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, mapping.SubclassType.GetElementName());
      if (mapping.IsSpecified("Name"))
        element.WithAtt("name", mapping.Name);
      if (mapping.IsSpecified("Proxy"))
        element.WithAtt("proxy", mapping.Proxy);
      if (mapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", mapping.Lazy);
      if (mapping.IsSpecified("DynamicUpdate"))
        element.WithAtt("dynamic-update", mapping.DynamicUpdate);
      if (mapping.IsSpecified("DynamicInsert"))
        element.WithAtt("dynamic-insert", mapping.DynamicInsert);
      if (mapping.IsSpecified("SelectBeforeUpdate"))
        element.WithAtt("select-before-update", mapping.SelectBeforeUpdate);
      if (mapping.IsSpecified("Abstract"))
        element.WithAtt("abstract", mapping.Abstract);
      if (mapping.IsSpecified("EntityName"))
        element.WithAtt("entity-name", mapping.EntityName);
      if (mapping.SubclassType == SubclassType.Subclass)
      {
        if (!mapping.IsSpecified("DiscriminatorValue"))
          return;
        element.WithAtt("discriminator-value", mapping.DiscriminatorValue.ToString());
      }
      else
      {
        if (mapping.IsSpecified("TableName"))
          element.WithAtt("table", mapping.TableName);
        if (mapping.IsSpecified("Schema"))
          element.WithAtt("schema", mapping.Schema);
        if (mapping.IsSpecified("Check"))
          element.WithAtt("check", mapping.Check);
        if (mapping.IsSpecified("Subselect"))
          element.WithAtt("subselect", mapping.Subselect);
        if (mapping.IsSpecified("Persister"))
          element.WithAtt("persister", mapping.Persister);
        if (!mapping.IsSpecified("BatchSize"))
          return;
        element.WithAtt("batch-size", mapping.BatchSize);
      }
    }

    public override void Visit(KeyMapping keyMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<KeyMapping>().Write(keyMapping));
    }

    public override void Visit(SubclassMapping subclassMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<SubclassMapping>().Write(subclassMapping));
    }

    public override void Visit(IComponentMapping componentMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<IComponentMapping>().Write(componentMapping));
    }

    public override void Visit(JoinMapping joinMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<JoinMapping>().Write(joinMapping));
    }
  }
}
