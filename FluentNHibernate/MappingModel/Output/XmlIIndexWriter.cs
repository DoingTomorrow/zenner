// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlIIndexWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlIIndexWriter : NullMappingModelVisitor, IXmlWriter<IIndexMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlIIndexWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(IIndexMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessIndex(IndexMapping mapping)
    {
      this.document = this.serviceLocator.GetWriter<IndexMapping>().Write(mapping);
    }

    public override void ProcessIndex(IndexManyToManyMapping mapping)
    {
      this.document = this.serviceLocator.GetWriter<IndexManyToManyMapping>().Write(mapping);
    }
  }
}
