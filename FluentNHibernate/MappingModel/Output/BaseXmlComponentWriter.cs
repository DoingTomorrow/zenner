// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.BaseXmlComponentWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public abstract class BaseXmlComponentWriter : XmlClassWriterBase
  {
    private readonly IXmlWriterServiceLocator serviceLocator;

    protected BaseXmlComponentWriter(IXmlWriterServiceLocator serviceLocator)
      : base(serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    protected XmlDocument WriteComponent(string element, IComponentMapping mapping)
    {
      XmlDocument document = new XmlDocument();
      XmlElement element1 = XmlExtensions.AddElement(document, element);
      if (mapping.IsSpecified("Name"))
        element1.WithAtt("name", mapping.Name);
      if (mapping.IsSpecified("Insert"))
        element1.WithAtt("insert", mapping.Insert);
      if (mapping.IsSpecified("Update"))
        element1.WithAtt("update", mapping.Update);
      if (mapping.IsSpecified("Access"))
        element1.WithAtt("access", mapping.Access);
      if (mapping.IsSpecified("OptimisticLock"))
        element1.WithAtt("optimistic-lock", mapping.OptimisticLock);
      return document;
    }

    public override void Visit(IComponentMapping componentMapping)
    {
      this.document.ImportAndAppendChild(new XmlComponentWriter(this.serviceLocator).Write(componentMapping));
    }

    public override void Visit(ParentMapping parentMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ParentMapping>().Write(parentMapping));
    }
  }
}
