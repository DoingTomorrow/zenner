// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlReferenceComponentWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlReferenceComponentWriter : 
    BaseXmlComponentWriter,
    IXmlWriter<ReferenceComponentMapping>
  {
    private IXmlWriter<IComponentMapping> innerWriter;

    public XmlReferenceComponentWriter(IXmlWriterServiceLocator serviceLocator)
      : base(serviceLocator)
    {
      this.innerWriter = serviceLocator.GetWriter<IComponentMapping>();
    }

    public XmlDocument Write(ReferenceComponentMapping mappingModel)
    {
      return this.innerWriter.Write((IComponentMapping) mappingModel.MergedModel);
    }
  }
}
