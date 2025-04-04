// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlPropertyWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlPropertyWriter : NullMappingModelVisitor, IXmlWriter<PropertyMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlPropertyWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(PropertyMapping property)
    {
      this.document = (XmlDocument) null;
      property.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessProperty(PropertyMapping propertyMapping)
    {
      this.document = new XmlDocument();
      XmlElement element = this.document.CreateElement("property");
      if (propertyMapping.IsSpecified("Access"))
        element.WithAtt("access", propertyMapping.Access);
      if (propertyMapping.IsSpecified("Generated"))
        element.WithAtt("generated", propertyMapping.Generated);
      if (propertyMapping.IsSpecified("Name"))
        element.WithAtt("name", propertyMapping.Name);
      if (propertyMapping.IsSpecified("OptimisticLock"))
        element.WithAtt("optimistic-lock", propertyMapping.OptimisticLock);
      if (propertyMapping.IsSpecified("Insert"))
        element.WithAtt("insert", propertyMapping.Insert);
      if (propertyMapping.IsSpecified("Update"))
        element.WithAtt("update", propertyMapping.Update);
      if (propertyMapping.IsSpecified("Formula"))
        element.WithAtt("formula", propertyMapping.Formula);
      if (propertyMapping.IsSpecified("Type"))
        element.WithAtt("type", propertyMapping.Type);
      if (propertyMapping.IsSpecified("Lazy"))
        element.WithAtt("lazy", propertyMapping.Lazy);
      this.document.AppendChild((XmlNode) element);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ColumnMapping>().Write(columnMapping));
    }
  }
}
