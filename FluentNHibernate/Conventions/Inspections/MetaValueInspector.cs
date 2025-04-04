// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.MetaValueInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class MetaValueInspector : IMetaValueInspector, IInspector
  {
    private readonly InspectorModelMapper<IMetaValueInspector, MetaValueMapping> propertyMappings = new InspectorModelMapper<IMetaValueInspector, MetaValueMapping>();
    private readonly MetaValueMapping mapping;

    public MetaValueInspector(MetaValueMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Class.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public TypeReference Class => this.mapping.Class;

    public string Value => this.mapping.Value;
  }
}
