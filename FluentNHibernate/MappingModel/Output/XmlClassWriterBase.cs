// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlClassWriterBase
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
  public abstract class XmlClassWriterBase : NullMappingModelVisitor
  {
    private readonly IXmlWriterServiceLocator serviceLocator;
    protected XmlDocument document;

    protected XmlClassWriterBase(IXmlWriterServiceLocator serviceLocator)
    {
      this.serviceLocator = serviceLocator;
    }

    public override void Visit(PropertyMapping propertyMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<PropertyMapping>().Write(propertyMapping));
    }

    public override void Visit(VersionMapping versionMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<VersionMapping>().Write(versionMapping));
    }

    public override void Visit(OneToOneMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<OneToOneMapping>().Write(mapping));
    }

    public override void Visit(ManyToOneMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<ManyToOneMapping>().Write(mapping));
    }

    public override void Visit(AnyMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<AnyMapping>().Write(mapping));
    }

    public override void Visit(CollectionMapping collectionMapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<CollectionMapping>().Write(collectionMapping));
    }

    public override void Visit(StoredProcedureMapping mapping)
    {
      this.document.ImportAndAppendChild(this.serviceLocator.GetWriter<StoredProcedureMapping>().Write(mapping));
    }
  }
}
