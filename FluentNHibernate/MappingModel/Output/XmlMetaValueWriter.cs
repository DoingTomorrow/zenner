// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlMetaValueWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlMetaValueWriter : NullMappingModelVisitor, IXmlWriter<MetaValueMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(MetaValueMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessMetaValue(MetaValueMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "meta-value");
      if (mapping.IsSpecified("Value"))
        element.WithAtt("value", mapping.Value);
      if (!mapping.IsSpecified("Class"))
        return;
      element.WithAtt("class", mapping.Class);
    }
  }
}
