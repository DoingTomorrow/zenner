// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.IHasMappedMembers
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  public interface IHasMappedMembers
  {
    IEnumerable<PropertyMapping> Properties { get; }

    IEnumerable<CollectionMapping> Collections { get; }

    IEnumerable<ManyToOneMapping> References { get; }

    IEnumerable<IComponentMapping> Components { get; }

    IEnumerable<OneToOneMapping> OneToOnes { get; }

    IEnumerable<AnyMapping> Anys { get; }

    IEnumerable<FilterMapping> Filters { get; }

    void AddProperty(PropertyMapping property);

    void AddCollection(CollectionMapping collection);

    void AddReference(ManyToOneMapping manyToOne);

    void AddComponent(IComponentMapping component);

    void AddOneToOne(OneToOneMapping mapping);

    void AddAny(AnyMapping mapping);

    void AddFilter(FilterMapping mapping);
  }
}
