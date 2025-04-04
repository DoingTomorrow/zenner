// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.DynamicComponentInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class DynamicComponentInspector : 
    ComponentBaseInspector,
    IDynamicComponentInspector,
    IComponentBaseInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    private readonly InspectorModelMapper<IDynamicComponentInspector, ComponentMapping> mappedProperties = new InspectorModelMapper<IDynamicComponentInspector, ComponentMapping>();
    private readonly ComponentMapping mapping;

    public DynamicComponentInspector(ComponentMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public override bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }
  }
}
