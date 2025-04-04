// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlKeyWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlKeyWriter : NullMappingModelVisitor, IXmlWriter<KeyMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlKeyWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(KeyMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessKey(KeyMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "key");
      if (mapping.IsSpecified("ForeignKey"))
        element.WithAtt("foreign-key", mapping.ForeignKey);
      if (mapping.IsSpecified("OnDelete"))
        element.WithAtt("on-delete", mapping.OnDelete);
      if (mapping.IsSpecified("PropertyRef"))
        element.WithAtt("property-ref", mapping.PropertyRef);
      if (mapping.IsSpecified("NotNull"))
        element.WithAtt("not-null", mapping.NotNull);
      if (mapping.IsSpecified("Update"))
        element.WithAtt("update", mapping.Update);
      if (!mapping.IsSpecified("Unique"))
        return;
      element.WithAtt("unique", mapping.Unique);
    }

    public override void Visit(ColumnMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(mapping));
    }
  }
}
