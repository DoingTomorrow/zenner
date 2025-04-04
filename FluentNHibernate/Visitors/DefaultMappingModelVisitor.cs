// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.DefaultMappingModelVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public abstract class DefaultMappingModelVisitor : NullMappingModelVisitor
  {
    public override void ProcessIndex(IndexManyToManyMapping indexMapping)
    {
      this.ProcessIndex((IIndexMapping) indexMapping);
    }

    public override void ProcessIndex(IndexMapping indexMapping)
    {
      this.ProcessIndex((IIndexMapping) indexMapping);
    }

    protected virtual void ProcessCollectionContents(
      ICollectionRelationshipMapping relationshipMapping)
    {
    }

    public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
    {
      this.ProcessCollectionContents((ICollectionRelationshipMapping) oneToManyMapping);
    }

    protected virtual void ProcessIdentity(IIdentityMapping idMapping)
    {
    }

    public override void ProcessId(IdMapping idMapping)
    {
      this.ProcessIdentity((IIdentityMapping) idMapping);
    }

    public override void ProcessCompositeId(CompositeIdMapping idMapping)
    {
      this.ProcessIdentity((IIdentityMapping) idMapping);
    }

    protected virtual void ProcessClassBase(ClassMappingBase classMapping)
    {
    }

    public override void ProcessClass(ClassMapping classMapping)
    {
      this.ProcessClassBase((ClassMappingBase) classMapping);
    }

    public override void ProcessSubclass(SubclassMapping subclassMapping)
    {
      this.ProcessClassBase((ClassMappingBase) subclassMapping);
    }

    public override void Visit(IEnumerable<HibernateMapping> mappings)
    {
      mappings.Each<HibernateMapping>((Action<HibernateMapping>) (x => x.AcceptVisitor((IMappingModelVisitor) this)));
    }

    public override void Visit(AnyMapping mapping)
    {
      mapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(ClassMapping classMapping)
    {
      classMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(IIdentityMapping identityMapping)
    {
      identityMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(CollectionMapping collectionMapping)
    {
      collectionMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(PropertyMapping propertyMapping)
    {
      propertyMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(ManyToOneMapping manyToOneMapping)
    {
      manyToOneMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(KeyMapping keyMapping)
    {
      keyMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(ICollectionRelationshipMapping relationshipMapping)
    {
      relationshipMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(GeneratorMapping generatorMapping)
    {
      generatorMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(ColumnMapping columnMapping)
    {
      columnMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(SubclassMapping subclassMapping)
    {
      subclassMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(DiscriminatorMapping discriminatorMapping)
    {
      discriminatorMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(IComponentMapping componentMapping)
    {
      componentMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(JoinMapping joinMapping)
    {
      joinMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(CompositeElementMapping compositeElementMapping)
    {
      compositeElementMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(VersionMapping versionMapping)
    {
      versionMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(OneToOneMapping mapping)
    {
      mapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(IIndexMapping indexMapping)
    {
      indexMapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(KeyPropertyMapping mapping)
    {
      mapping.AcceptVisitor((IMappingModelVisitor) this);
    }

    public override void Visit(KeyManyToOneMapping mapping)
    {
      mapping.AcceptVisitor((IMappingModelVisitor) this);
    }
  }
}
