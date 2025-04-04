// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlAnyWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlAnyWriter : NullMappingModelVisitor, IXmlWriter<AnyMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlAnyWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(AnyMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessAny(AnyMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "any");
      if (mapping.IsSpecified("Access"))
        element.WithAtt("access", mapping.Access);
      if (mapping.IsSpecified("Cascade"))
        element.WithAtt("cascade", mapping.Cascade);
      if (mapping.IsSpecified("IdType"))
        element.WithAtt("id-type", mapping.IdType);
      if (mapping.IsSpecified("Insert"))
        element.WithAtt("insert", mapping.Insert);
      if (mapping.IsSpecified("MetaType"))
        element.WithAtt("meta-type", mapping.MetaType);
      if (mapping.IsSpecified("Name"))
        element.WithAtt("name", mapping.Name);
      if (mapping.IsSpecified("Update"))
        element.WithAtt("update", mapping.Update);
      if (mapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", mapping.Lazy);
      if (!mapping.IsSpecified("OptimisticLock"))
        return;
      element.WithAtt("optimistic-lock", mapping.OptimisticLock);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(columnMapping));
    }

    public override void Visit(MetaValueMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<MetaValueMapping>().Write(mapping));
    }
  }
}
