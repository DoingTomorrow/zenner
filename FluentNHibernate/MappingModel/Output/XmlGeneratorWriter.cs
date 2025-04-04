// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlGeneratorWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlGeneratorWriter : NullMappingModelVisitor, IXmlWriter<GeneratorMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(GeneratorMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessGenerator(GeneratorMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "generator");
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) mapping.Params)
        element.AddElement("param").WithAtt("name", keyValuePair.Key).InnerXml = keyValuePair.Value;
    }
  }
}
