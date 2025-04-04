// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlStoredProcedureWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlStoredProcedureWriter : XmlClassWriterBase, IXmlWriter<StoredProcedureMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;

    public XmlStoredProcedureWriter(IXmlWriterServiceLocator serviceLocator)
      : base(serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(StoredProcedureMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessStoredProcedure(StoredProcedureMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, mapping.SPType);
      element.WithAtt("check", mapping.Check);
      element.InnerXml = mapping.Query;
    }

    public override void Visit(StoredProcedureMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<StoredProcedureMapping>().Write(mapping));
    }
  }
}
