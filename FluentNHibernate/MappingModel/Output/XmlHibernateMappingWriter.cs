// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlHibernateMappingWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output.Sorting;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlHibernateMappingWriter : NullMappingModelVisitor, IXmlWriter<HibernateMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlHibernateMappingWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(HibernateMapping mapping)
    {
      mapping.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessHibernateMapping(HibernateMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "hibernate-mapping");
      element.WithAtt("xmlns", "urn:nhibernate-mapping-2.2");
      if (mapping.IsSpecified("DefaultAccess"))
        element.WithAtt("default-access", mapping.DefaultAccess);
      if (mapping.IsSpecified("AutoImport"))
        element.WithAtt("auto-import", mapping.AutoImport);
      if (mapping.IsSpecified("Schema"))
        element.WithAtt("schema", mapping.Schema);
      if (mapping.IsSpecified("DefaultCascade"))
        element.WithAtt("default-cascade", mapping.DefaultCascade);
      if (mapping.IsSpecified("DefaultLazy"))
        element.WithAtt("default-lazy", mapping.DefaultLazy);
      if (mapping.IsSpecified("Catalog"))
        element.WithAtt("catalog", mapping.Catalog);
      if (mapping.IsSpecified("Namespace"))
        element.WithAtt("namespace", mapping.Namespace);
      if (!mapping.IsSpecified("Assembly"))
        return;
      element.WithAtt("assembly", mapping.Assembly);
    }

    public override void Visit(ImportMapping importMapping)
    {
      XmlNode newChild = this.document.ImportNode((XmlNode) this.serviceLocator.GetWriter<ImportMapping>().Write(importMapping).DocumentElement, true);
      if (this.document.DocumentElement.ChildNodes.Count > 0)
        this.document.DocumentElement.InsertBefore(newChild, this.document.DocumentElement.ChildNodes[0]);
      else
        this.document.DocumentElement.AppendChild(newChild);
    }

    public override void Visit(ClassMapping classMapping)
    {
      XmlNode xmlNode = this.document.ImportNode((XmlNode) this.serviceLocator.GetWriter<ClassMapping>().Write(classMapping).DocumentElement, true);
      XmlNodeSorter.SortClassChildren(xmlNode);
      this.document.DocumentElement.AppendChild(xmlNode);
    }

    public override void Visit(FilterDefinitionMapping filterDefinitionMapping)
    {
      XmlNode xmlNode = this.document.ImportNode((XmlNode) this.serviceLocator.GetWriter<FilterDefinitionMapping>().Write(filterDefinitionMapping).DocumentElement, true);
      XmlNodeSorter.SortClassChildren(xmlNode);
      this.document.DocumentElement.AppendChild(xmlNode);
    }
  }
}
