// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlFilterWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlFilterWriter : NullMappingModelVisitor, IXmlWriter<FilterMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(FilterMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessFilter(FilterMapping filterDefinitionMapping)
    {
      this.document = new XmlDocument();
      XmlElement element = this.document.CreateElement("filter");
      element.WithAtt("name", filterDefinitionMapping.Name);
      if (!string.IsNullOrEmpty(filterDefinitionMapping.Condition))
        element.WithAtt("condition", filterDefinitionMapping.Condition);
      this.document.AppendChild((XmlNode) element);
    }
  }
}
