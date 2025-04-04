// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ComponentInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class ComponentInspector : 
    ComponentBaseInspector,
    IComponentInspector,
    IComponentBaseInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    private readonly InspectorModelMapper<IComponentInspector, ComponentMapping> mappedProperties = new InspectorModelMapper<IComponentInspector, ComponentMapping>();
    private readonly ComponentMapping mapping;

    public ComponentInspector(ComponentMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
      this.mappedProperties.Map((Expression<Func<IComponentInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<ComponentMapping, object>>) (x => (object) x.Lazy));
    }

    public override bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public bool LazyLoad => this.mapping.Lazy;
  }
}
