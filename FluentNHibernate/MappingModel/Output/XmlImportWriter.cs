// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlImportWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlImportWriter : NullMappingModelVisitor, IXmlWriter<ImportMapping>
  {
    private XmlDocument document;

    public XmlDocument Write(ImportMapping mappingModel)
    {
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessImport(ImportMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = this.document.CreateElement("import");
      if (mapping.IsSpecified("Class"))
        element.WithAtt("class", mapping.Class);
      if (mapping.IsSpecified("Rename"))
        element.WithAtt("rename", mapping.Rename);
      this.document.AppendChild((XmlNode) element);
    }
  }
}
