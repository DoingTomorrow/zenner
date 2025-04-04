// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlManyToOneWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlManyToOneWriter : NullMappingModelVisitor, IXmlWriter<ManyToOneMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlManyToOneWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(ManyToOneMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessManyToOne(ManyToOneMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "many-to-one");
      if (mapping.IsSpecified("Access"))
        element.WithAtt("access", mapping.Access);
      if (mapping.IsSpecified("Cascade"))
        element.WithAtt("cascade", mapping.Cascade);
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      if (mapping.IsSpecified("Fetch"))
        element.WithAtt("fetch", mapping.Fetch);
      if (mapping.IsSpecified("ForeignKey"))
        element.WithAtt("foreign-key", mapping.ForeignKey);
      if (mapping.IsSpecified("Insert"))
        element.WithAtt("insert", mapping.Insert);
      if (mapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", mapping.Lazy);
      if (mapping.IsSpecified("Name"))
        element.WithAtt("name", mapping.Name);
      if (mapping.IsSpecified("NotFound"))
        element.WithAtt("not-found", mapping.NotFound);
      if (mapping.IsSpecified("Formula"))
        element.WithAtt("formula", mapping.Formula);
      if (mapping.IsSpecified("PropertyRef"))
        element.WithAtt("property-ref", mapping.PropertyRef);
      if (mapping.IsSpecified("Update"))
        element.WithAtt("update", mapping.Update);
      if (mapping.IsSpecified("EntityName"))
        element.WithAtt("entity-name", mapping.EntityName);
      if (!mapping.IsSpecified("OptimisticLock"))
        return;
      element.WithAtt("optimistic-lock", mapping.OptimisticLock);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(columnMapping));
    }
  }
}
