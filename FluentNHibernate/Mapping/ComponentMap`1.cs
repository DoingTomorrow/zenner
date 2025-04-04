// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ComponentMap`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ComponentMap<T> : 
    ComponentPartBase<T, ComponentMap<T>>,
    IExternalComponentMappingProvider
  {
    private readonly AttributeStore attributes;

    public ComponentMap()
      : this(new AttributeStore())
    {
    }

    internal ComponentMap(AttributeStore attributes)
      : base(attributes, (Member) null)
    {
      this.attributes = attributes;
    }

    public override ReferenceComponentPart<TComponent> Component<TComponent>(
      Expression<Func<T, TComponent>> member)
    {
      if (typeof (TComponent) == typeof (T))
        throw new NotSupportedException("Nested components of the same type are not supported in ComponentMap.");
      return base.Component<TComponent>(member);
    }

    protected override ComponentMapping CreateComponentMappingRoot(AttributeStore store)
    {
      return (ComponentMapping) new ExternalComponentMapping(ComponentType.Component, this.attributes.Clone());
    }

    ExternalComponentMapping IExternalComponentMappingProvider.GetComponentMapping()
    {
      return ((ExternalComponentMapping) this.CreateComponentMapping()).DeepClone<ExternalComponentMapping>();
    }

    Type IExternalComponentMappingProvider.Type => typeof (T);
  }
}
