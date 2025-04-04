// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlIdentityBasedWriter
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Visitors;
using System.Xml;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlIdentityBasedWriter : NullMappingModelVisitor, IXmlWriter<IIdentityMapping>
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    private XmlDocument document;

    public XmlIdentityBasedWriter(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public XmlDocument Write(IIdentityMapping mappingModel)
    {
      this.document = (XmlDocument) null;
      mappingModel.AcceptVisitor((IMappingModelVisitor) this);
      return this.document;
    }

    public override void ProcessId(IdMapping mapping)
    {
      this.document = this.serviceLocator.GetWriter<IdMapping>().Write(mapping);
    }

    public override void ProcessCompositeId(CompositeIdMapping idMapping)
    {
      this.document = this.serviceLocator.GetWriter<CompositeIdMapping>().Write(idMapping);
    }
  }
}
