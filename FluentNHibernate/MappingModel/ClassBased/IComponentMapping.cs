// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.IComponentMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  public interface IComponentMapping : IMapping
  {
    bool HasColumnPrefix { get; }

    string ColumnPrefix { get; }

    ParentMapping Parent { get; }

    bool Insert { get; }

    bool Update { get; }

    string Access { get; }

    Type ContainingEntityType { get; }

    string Name { get; }

    Member Member { get; }

    Type Type { get; }

    bool OptimisticLock { get; }

    bool Unique { get; }

    IEnumerable<ManyToOneMapping> References { get; }

    IEnumerable<CollectionMapping> Collections { get; }

    IEnumerable<PropertyMapping> Properties { get; }

    IEnumerable<IComponentMapping> Components { get; }

    IEnumerable<OneToOneMapping> OneToOnes { get; }

    IEnumerable<AnyMapping> Anys { get; }

    ComponentType ComponentType { get; }

    TypeReference Class { get; }

    bool Lazy { get; }

    void AddProperty(PropertyMapping mapping);

    void AddComponent(IComponentMapping mapping);

    void AddOneToOne(OneToOneMapping mapping);

    void AddCollection(CollectionMapping mapping);

    void AddReference(ManyToOneMapping mapping);

    void AddAny(AnyMapping mapping);
  }
}
