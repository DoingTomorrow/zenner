// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlArrayWriter
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
  public class XmlArrayWriter : BaseXmlCollectionWriter, IXmlWriter<CollectionMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;

    public XmlArrayWriter(IXmlWriterServiceLocator serviceLocator)
      : base(serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(CollectionMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
      this.document = new XmlDocument();
      this.WriteBaseCollectionAttributes(XmlExtensions.AddElement(this.document, "array"), mapping);
    }

    public override void Visit(IIndexMapping indexMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<IIndexMapping>().Write(indexMapping));
    }
  }
}
