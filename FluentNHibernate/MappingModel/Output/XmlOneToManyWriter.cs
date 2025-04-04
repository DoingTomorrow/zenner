// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlOneToManyWriter
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
  public class XmlOneToManyWriter : NullMappingModelVisitor, IXmlWriter<OneToManyMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(OneToManyMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessOneToMany(OneToManyMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "one-to-many");
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      if (mapping.IsSpecified("NotFound"))
        element.WithAtt("not-found", mapping.NotFound);
      if (!mapping.IsSpecified("EntityName"))
        return;
      element.WithAtt("entity-name", mapping.EntityName);
    }
  }
}
