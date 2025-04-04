// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlComponentWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlComponentWriter(IXmlWriterServiceLocator serviceLocator) : 
    BaseXmlComponentWriter(serviceLocator),
    IXmlWriter<IComponentMapping>
  {
    public XmlDocument Write(IComponentMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessComponent(ComponentMapping mapping)
    {
      this.document = this.WriteComponent(mapping.ComponentType.GetElementName(), (IComponentMapping) mapping);
      if (mapping.IsSpecified("Class"))
        this.document.DocumentElement.WithAtt("class", mapping.Class);
      if (!mapping.IsSpecified("Lazy"))
        return;
      this.document.DocumentElement.WithAtt("lazy", mapping.Lazy);
    }
  }
}
