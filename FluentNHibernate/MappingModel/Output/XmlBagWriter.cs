// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlBagWriter
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
  public class XmlBagWriter(IXmlWriterServiceLocator serviceLocator) : 
    BaseXmlCollectionWriter(serviceLocator),
    IXmlWriter<CollectionMapping>
  {
    public XmlDocument Write(CollectionMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessCollection(CollectionMapping mapping)
    {
      this.document = new XmlDocument();
      XmlElement element = XmlExtensions.AddElement(this.document, "bag");
      this.WriteBaseCollectionAttributes(element, mapping);
      if (!mapping.IsSpecified("OrderBy"))
        return;
      element.WithAtt("order-by", mapping.OrderBy);
    }
  }
}
