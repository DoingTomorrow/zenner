// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlFilterDefinitionWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Type;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlFilterDefinitionWriter : 
    NullMappingModelVisitor,
    IXmlWriter<FilterDefinitionMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(FilterDefinitionMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessFilterDefinition(FilterDefinitionMapping filterDefinitionMapping)
    {
      this.document = new XmlDocument();
      XmlElement element1 = this.document.CreateElement("filter-def");
      element1.WithAtt("name", filterDefinitionMapping.Name);
      if (!string.IsNullOrEmpty(filterDefinitionMapping.Condition))
        element1.WithAtt("condition", filterDefinitionMapping.Condition);
      foreach (KeyValuePair<string, IType> parameter in (IEnumerable<KeyValuePair<string, IType>>) filterDefinitionMapping.Parameters)
      {
        XmlElement element2 = this.document.CreateElement("filter-param");
        element2.WithAtt("name", parameter.Key);
        element2.WithAtt("type", parameter.Value.Name);
        element1.AppendChild((XmlNode) element2);
      }
      this.document.AppendChild((XmlNode) element1);
    }
  }
}
