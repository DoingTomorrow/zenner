// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlCollectionWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
using System;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlCollectionWriter : NullMappingModelVisitor, IXmlWriter<CollectionMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;
    private Collection collection;

    public XmlCollectionWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(CollectionMapping mappingModel)
    {
      this.collection = mappingModel.Collection;
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
      IXmlWriter<CollectionMapping> xmlWriter;
      switch (mapping.Collection)
      {
        case Collection.Array:
          xmlWriter = (IXmlWriter<CollectionMapping>) new XmlArrayWriter(this.serviceLocator);
          break;
        case Collection.Bag:
          xmlWriter = (IXmlWriter<CollectionMapping>) new XmlBagWriter(this.serviceLocator);
          break;
        case Collection.Map:
          xmlWriter = (IXmlWriter<CollectionMapping>) new XmlMapWriter(this.serviceLocator);
          break;
        case Collection.List:
          xmlWriter = (IXmlWriter<CollectionMapping>) new XmlListWriter(this.serviceLocator);
          break;
        case Collection.Set:
          xmlWriter = (IXmlWriter<CollectionMapping>) new XmlSetWriter(this.serviceLocator);
          break;
        default:
          throw new InvalidOperationException("Unrecognised collection type " + (object) mapping.Collection);
      }
      this.document = xmlWriter.Write(mapping);
    }
  }
}
