// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.CacheInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class CacheInspector : ICacheInspector, IInspector
  {
    private readonly InspectorModelMapper<ICacheInspector, CacheMapping> propertyMappings = new InspectorModelMapper<ICacheInspector, CacheMapping>();
    private readonly CacheMapping mapping;

    public CacheInspector(CacheMapping mapping) => this.mapping = mapping;

    public string Usage => this.mapping.Usage;

    public string Region => this.mapping.Region;

    public Include Include => Include.FromString(this.mapping.Include);

    public Type EntityType => this.mapping.ContainedEntityType;

    public string StringIdentifierForModel => this.mapping.Usage;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }
  }
}
