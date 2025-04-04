// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlClassWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlClassWriter : XmlClassWriterBase, IXmlWriter<ClassMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;

    public XmlClassWriter(IXmlWriterServiceLocator serviceLocator)
      : base(serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(ClassMapping mapping)
    {
      this.document = (XmlDocument) null;
      mapping.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessClass(ClassMapping classMapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "class").WithAtt("xmlns", "urn:nhibernate-mapping-2.2");
      if (classMapping.IsSpecified("BatchSize"))
        element.WithAtt("batch-size", classMapping.BatchSize);
      if (classMapping.IsSpecified("DiscriminatorValue"))
        element.WithAtt("discriminator-value", classMapping.DiscriminatorValue.ToString());
      if (classMapping.IsSpecified("DynamicInsert"))
        element.WithAtt("dynamic-insert", classMapping.DynamicInsert);
      if (classMapping.IsSpecified("DynamicUpdate"))
        element.WithAtt("dynamic-update", classMapping.DynamicUpdate);
      if (classMapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", classMapping.Lazy);
      if (classMapping.IsSpecified("Schema"))
        element.WithAtt("schema", classMapping.Schema);
      if (classMapping.IsSpecified("Mutable"))
        element.WithAtt("mutable", classMapping.Mutable);
      if (classMapping.IsSpecified("Polymorphism"))
        element.WithAtt("polymorphism", classMapping.Polymorphism);
      if (classMapping.IsSpecified("Persister"))
        element.WithAtt("persister", classMapping.Persister);
      if (classMapping.IsSpecified("Where"))
        element.WithAtt("where", classMapping.Where);
      if (classMapping.IsSpecified("OptimisticLock"))
        element.WithAtt("optimistic-lock", classMapping.OptimisticLock);
      if (classMapping.IsSpecified("Check"))
        element.WithAtt("check", classMapping.Check);
      if (classMapping.IsSpecified("Name"))
        element.WithAtt("name", classMapping.Name);
      if (classMapping.IsSpecified("TableName"))
        element.WithAtt("table", classMapping.TableName);
      if (classMapping.IsSpecified("Proxy"))
        element.WithAtt("proxy", classMapping.Proxy);
      if (classMapping.IsSpecified("SelectBeforeUpdate"))
        element.WithAtt("select-before-update", classMapping.SelectBeforeUpdate);
      if (classMapping.IsSpecified("Abstract"))
        element.WithAtt("abstract", classMapping.Abstract);
      if (classMapping.IsSpecified("Subselect"))
        element.WithAtt("subselect", classMapping.Subselect);
      if (classMapping.IsSpecified("SchemaAction"))
        element.WithAtt("schema-action", classMapping.SchemaAction);
      if (!classMapping.IsSpecified("EntityName"))
        return;
      element.WithAtt("entity-name", classMapping.EntityName);
    }

    public override void Visit(DiscriminatorMapping discriminatorMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<DiscriminatorMapping>().Write(discriminatorMapping));
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

    public override void Visit(IIdentityMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<IIdentityMapping>().Write(mapping));
    }

    public override void Visit(NaturalIdMapping naturalIdMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<NaturalIdMapping>().Write(naturalIdMapping));
    }

    public override void Visit(CacheMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<CacheMapping>().Write(mapping));
    }

    public override void Visit(ManyToOneMapping manyToOneMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ManyToOneMapping>().Write(manyToOneMapping));
    }

    public override void Visit(FilterMapping filterMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<FilterMapping>().Write(filterMapping));
    }

    public override void Visit(TuplizerMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<TuplizerMapping>().Write(mapping));
    }
  }
}
