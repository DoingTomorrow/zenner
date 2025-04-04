// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlOneToOneWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlOneToOneWriter : NullMappingModelVisitor, IXmlWriter<OneToOneMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(OneToOneMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessOneToOne(OneToOneMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "one-to-one");
      if (mapping.IsSpecified("Access"))
        element.WithAtt("access", mapping.Access);
      if (mapping.IsSpecified("Cascade"))
        element.WithAtt("cascade", mapping.Cascade);
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      if (mapping.IsSpecified("Constrained"))
        element.WithAtt("constrained", mapping.Constrained);
      if (mapping.IsSpecified("Fetch"))
        element.WithAtt("fetch", mapping.Fetch);
      if (mapping.IsSpecified("ForeignKey"))
        element.WithAtt("foreign-key", mapping.ForeignKey);
      if (mapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", mapping.Lazy);
      if (mapping.IsSpecified("Name"))
        element.WithAtt("name", mapping.Name);
      if (mapping.IsSpecified("PropertyRef"))
        element.WithAtt("property-ref", mapping.PropertyRef);
      if (!mapping.IsSpecified("EntityName"))
        return;
      element.WithAtt("entity-name", mapping.EntityName);
    }
  }
}
