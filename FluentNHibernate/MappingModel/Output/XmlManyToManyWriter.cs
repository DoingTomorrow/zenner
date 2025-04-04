// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlManyToManyWriter
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
  public class XmlManyToManyWriter : NullMappingModelVisitor, IXmlWriter<ManyToManyMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlManyToManyWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(ManyToManyMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessManyToMany(ManyToManyMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "many-to-many");
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      if (mapping.IsSpecified("Fetch"))
        element.WithAtt("fetch", mapping.Fetch);
      if (mapping.IsSpecified("ForeignKey"))
        element.WithAtt("foreign-key", mapping.ForeignKey);
      if (mapping.IsSpecified("ChildPropertyRef"))
        element.WithAtt("property-ref", mapping.ChildPropertyRef);
      if (mapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", mapping.Lazy);
      if (mapping.IsSpecified("NotFound"))
        element.WithAtt("not-found", mapping.NotFound);
      if (mapping.IsSpecified("Where"))
        element.WithAtt("where", mapping.Where);
      if (mapping.IsSpecified("EntityName"))
        element.WithAtt("entity-name", mapping.EntityName);
      if (!mapping.IsSpecified("OrderBy"))
        return;
      element.WithAtt("order-by", mapping.OrderBy);
    }

    public override void Visit(ColumnMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(mapping));
    }

    public override void Visit(FilterMapping filterMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<FilterMapping>().Write(filterMapping));
    }
  }
}
