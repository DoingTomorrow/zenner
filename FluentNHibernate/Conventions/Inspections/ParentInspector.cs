// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ParentInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class ParentInspector : IParentInspector, IInspector
  {
    private readonly InspectorModelMapper<IPropertyInspector, ParentMapping> mappedProperties = new InspectorModelMapper<IPropertyInspector, ParentMapping>();
    private readonly ParentMapping mapping;

    public ParentInspector(ParentMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public string Name => this.mapping.Name;
  }
}
