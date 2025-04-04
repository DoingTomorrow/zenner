// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.IMappingModelVisitor
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public interface IMappingModelVisitor
  {
    void ProcessId(IdMapping idMapping);

    void ProcessNaturalId(NaturalIdMapping naturalIdMapping);

    void ProcessCache(CacheMapping mapping);

    void ProcessCompositeId(CompositeIdMapping idMapping);

    void ProcessClass(ClassMapping classMapping);

    void ProcessImport(ImportMapping importMapping);

    void ProcessHibernateMapping(HibernateMapping hibernateMapping);

    void ProcessProperty(PropertyMapping propertyMapping);

    void ProcessManyToOne(ManyToOneMapping manyToOneMapping);

    void ProcessKey(KeyMapping keyMapping);

    void ProcessGenerator(GeneratorMapping generatorMapping);

    void ProcessColumn(ColumnMapping columnMapping);

    void ProcessOneToMany(OneToManyMapping oneToManyMapping);

    void ProcessManyToMany(ManyToManyMapping manyToManyMapping);

    void ProcessSubclass(SubclassMapping subclassMapping);

    void ProcessDiscriminator(DiscriminatorMapping discriminatorMapping);

    void ProcessComponent(ComponentMapping mapping);

    void ProcessComponent(ReferenceComponentMapping componentMapping);

    void ProcessIndex(IIndexMapping indexMapping);

    void ProcessIndex(IndexMapping indexMapping);

    void ProcessIndex(IndexManyToManyMapping indexMapping);

    void ProcessParent(ParentMapping parentMapping);

    void ProcessJoin(JoinMapping joinMapping);

    void ProcessCompositeElement(CompositeElementMapping compositeElementMapping);

    void ProcessVersion(VersionMapping mapping);

    void ProcessOneToOne(OneToOneMapping mapping);

    void ProcessAny(AnyMapping mapping);

    void ProcessMetaValue(MetaValueMapping mapping);

    void ProcessKeyProperty(KeyPropertyMapping mapping);

    void ProcessKeyManyToOne(KeyManyToOneMapping mapping);

    void ProcessElement(ElementMapping mapping);

    void ProcessFilter(FilterMapping mapping);

    void ProcessFilterDefinition(FilterDefinitionMapping mapping);

    void ProcessStoredProcedure(StoredProcedureMapping mapping);

    void ProcessTuplizer(TuplizerMapping mapping);

    void ProcessCollection(CollectionMapping mapping);

    void Visit(IEnumerable<HibernateMapping> mappings);

    void Visit(IdMapping mapping);

    void Visit(NaturalIdMapping naturalIdMapping);

    void Visit(ClassMapping classMapping);

    void Visit(CacheMapping mapping);

    void Visit(ImportMapping importMapping);

    void Visit(IIdentityMapping identityMapping);

    void Visit(CollectionMapping collectionMapping);

    void Visit(PropertyMapping propertyMapping);

    void Visit(ManyToOneMapping manyToOneMapping);

    void Visit(KeyMapping keyMapping);

    void Visit(ICollectionRelationshipMapping relationshipMapping);

    void Visit(GeneratorMapping generatorMapping);

    void Visit(ColumnMapping columnMapping);

    void Visit(SubclassMapping subclassMapping);

    void Visit(DiscriminatorMapping discriminatorMapping);

    void Visit(IComponentMapping componentMapping);

    void Visit(IIndexMapping indexMapping);

    void Visit(ParentMapping parentMapping);

    void Visit(JoinMapping joinMapping);

    void Visit(CompositeElementMapping compositeElementMapping);

    void Visit(VersionMapping versionMapping);

    void Visit(OneToOneMapping mapping);

    void Visit(OneToManyMapping mapping);

    void Visit(ManyToManyMapping mapping);

    void Visit(AnyMapping mapping);

    void Visit(MetaValueMapping mapping);

    void Visit(KeyPropertyMapping mapping);

    void Visit(KeyManyToOneMapping mapping);

    void Visit(ElementMapping mapping);

    void Visit(FilterMapping mapping);

    void Visit(FilterDefinitionMapping mapping);

    void Visit(StoredProcedureMapping mapping);

    void Visit(TuplizerMapping mapping);
  }
}
