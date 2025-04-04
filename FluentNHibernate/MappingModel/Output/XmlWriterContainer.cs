// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Output.XmlWriterContainer
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using System;

#nullable disable
namespace FluentNHibernate.MappingModel.Output
{
  public class XmlWriterContainer : Container
  {
    public XmlWriterContainer()
    {
      this.Register<IXmlWriterServiceLocator>((Func<Container, object>) (c => (object) new XmlWriterServiceLocator((Container) this)));
      this.RegisterWriter<HibernateMapping>((Func<Container, object>) (c => (object) new XmlHibernateMappingWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ClassMapping>((Func<Container, object>) (c => (object) new XmlClassWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ImportMapping>((Func<Container, object>) (c => (object) new XmlImportWriter()));
      this.RegisterWriter<PropertyMapping>((Func<Container, object>) (c => (object) new XmlPropertyWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterIdWriters();
      this.RegisterComponentWriters();
      this.RegisterWriter<NaturalIdMapping>((Func<Container, object>) (c => (object) new XmlNaturalIdWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ColumnMapping>((Func<Container, object>) (c => (object) new XmlColumnWriter()));
      this.RegisterWriter<JoinMapping>((Func<Container, object>) (c => (object) new XmlJoinWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<DiscriminatorMapping>((Func<Container, object>) (c => (object) new XmlDiscriminatorWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<KeyMapping>((Func<Container, object>) (c => (object) new XmlKeyWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ParentMapping>((Func<Container, object>) (c => (object) new XmlParentWriter()));
      this.RegisterWriter<CompositeElementMapping>((Func<Container, object>) (c => (object) new XmlCompositeElementWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<VersionMapping>((Func<Container, object>) (c => (object) new XmlVersionWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<CacheMapping>((Func<Container, object>) (c => (object) new XmlCacheWriter()));
      this.RegisterWriter<OneToOneMapping>((Func<Container, object>) (c => (object) new XmlOneToOneWriter()));
      this.RegisterWriter<CollectionMapping>((Func<Container, object>) (c => (object) new XmlCollectionWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<IIndexMapping>((Func<Container, object>) (c => (object) new XmlIIndexWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<IndexMapping>((Func<Container, object>) (c => (object) new XmlIndexWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<IndexManyToManyMapping>((Func<Container, object>) (c => (object) new XmlIndexManyToManyWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ElementMapping>((Func<Container, object>) (c => (object) new XmlElementWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<OneToManyMapping>((Func<Container, object>) (c => (object) new XmlOneToManyWriter()));
      this.RegisterWriter<AnyMapping>((Func<Container, object>) (c => (object) new XmlAnyWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<MetaValueMapping>((Func<Container, object>) (c => (object) new XmlMetaValueWriter()));
      this.RegisterWriter<ICollectionRelationshipMapping>((Func<Container, object>) (c => (object) new XmlCollectionRelationshipWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ManyToOneMapping>((Func<Container, object>) (c => (object) new XmlManyToOneWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ManyToManyMapping>((Func<Container, object>) (c => (object) new XmlManyToManyWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<SubclassMapping>((Func<Container, object>) (c => (object) new XmlSubclassWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<FilterMapping>((Func<Container, object>) (c => (object) new XmlFilterWriter()));
      this.RegisterWriter<FilterDefinitionMapping>((Func<Container, object>) (c => (object) new XmlFilterDefinitionWriter()));
      this.RegisterWriter<StoredProcedureMapping>((Func<Container, object>) (c => (object) new XmlStoredProcedureWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<TuplizerMapping>((Func<Container, object>) (c => (object) new XmlTuplizerWriter()));
    }

    private void RegisterIdWriters()
    {
      this.RegisterWriter<IIdentityMapping>((Func<Container, object>) (c => (object) new XmlIdentityBasedWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<IdMapping>((Func<Container, object>) (c => (object) new XmlIdWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<CompositeIdMapping>((Func<Container, object>) (c => (object) new XmlCompositeIdWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<GeneratorMapping>((Func<Container, object>) (c => (object) new XmlGeneratorWriter()));
      this.RegisterWriter<KeyPropertyMapping>((Func<Container, object>) (c => (object) new XmlKeyPropertyWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<KeyManyToOneMapping>((Func<Container, object>) (c => (object) new XmlKeyManyToOneWriter(c.Resolve<IXmlWriterServiceLocator>())));
    }

    private void RegisterComponentWriters()
    {
      this.RegisterWriter<IComponentMapping>((Func<Container, object>) (c => (object) new XmlComponentWriter(c.Resolve<IXmlWriterServiceLocator>())));
      this.RegisterWriter<ReferenceComponentMapping>((Func<Container, object>) (c => (object) new XmlReferenceComponentWriter(c.Resolve<IXmlWriterServiceLocator>())));
    }

    private void RegisterWriter<T>(Func<Container, object> instantiate)
    {
      this.Register<IXmlWriter<T>>(instantiate);
    }
  }
}
