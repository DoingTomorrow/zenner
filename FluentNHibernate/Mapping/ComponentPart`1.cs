// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ComponentPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ComponentPart<T> : ComponentPartBase<T, ComponentPart<T>>, IComponentMappingProvider
  {
    private readonly Type entity;
    private readonly AttributeStore attributes;

    public ComponentPart(Type entity, Member property)
      : this(entity, property, new AttributeStore())
    {
    }

    private ComponentPart(Type entity, Member property, AttributeStore attributes)
      : base(attributes, property)
    {
      this.attributes = attributes;
      this.entity = entity;
    }

    public ComponentPart<T> LazyLoad()
    {
      this.attributes.Set("Lazy", 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    IComponentMapping IComponentMappingProvider.GetComponentMapping()
    {
      return (IComponentMapping) this.CreateComponentMapping();
    }

    protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
    {
      ComponentMapping componentMapping = new ComponentMapping(ComponentType.Component, store);
      componentMapping.ContainingEntityType = this.entity;
      ComponentMapping componentMappingRoot = componentMapping;
      componentMappingRoot.Set<TypeReference>((Expression<Func<ComponentMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(typeof (T)));
      return componentMappingRoot;
    }
  }
}
