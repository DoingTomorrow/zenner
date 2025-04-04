// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlDiscriminatorWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlDiscriminatorWriter : NullMappingModelVisitor, IXmlWriter<DiscriminatorMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlDiscriminatorWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(DiscriminatorMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "discriminator");
      if (discriminatorMapping.IsSpecified("Type"))
        element.WithAtt("type", TypeMapping.GetTypeString(discriminatorMapping.Type.GetUnderlyingSystemType()));
      if (discriminatorMapping.IsSpecified("Force"))
        element.WithAtt("force", discriminatorMapping.Force);
      if (discriminatorMapping.IsSpecified("Formula"))
        element.WithAtt("formula", discriminatorMapping.Formula);
      if (!discriminatorMapping.IsSpecified("Insert"))
        return;
      element.WithAtt("insert", discriminatorMapping.Insert);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(columnMapping));
    }
  }
}
