// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.DynamicComponentPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class DynamicComponentPart<T> : 
    ComponentPartBase<T, DynamicComponentPart<T>>,
    IComponentMappingProvider
  {
    private readonly Type entity;
    private readonly MappingProviderStore providers;

    public DynamicComponentPart(Type entity, Member member)
      : this(entity, member, new AttributeStore(), new MappingProviderStore())
    {
    }

    private DynamicComponentPart(
      Type entity,
      Member member,
      AttributeStore underlyingStore,
      MappingProviderStore providers)
      : base(underlyingStore, member, providers)
    {
      this.entity = entity;
      this.providers = providers;
    }

    protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
    {
      ComponentMapping componentMappingRoot = new ComponentMapping(ComponentType.DynamicComponent, store);
      componentMappingRoot.ContainingEntityType = this.entity;
      return componentMappingRoot;
    }

    public PropertyPart Map(string key) => this.Map<string>(key);

    public PropertyPart Map<TProperty>(string key)
    {
      PropertyPart propertyPart = new PropertyPart(MemberExtensions.ToMember(new DummyPropertyInfo(key, typeof (TProperty))), typeof (T));
      this.providers.Properties.Add((IPropertyMappingProvider) propertyPart);
      return propertyPart;
    }

    IComponentMapping IComponentMappingProvider.GetComponentMapping()
    {
      return (IComponentMapping) this.CreateComponentMapping();
    }
  }
}
