// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.GeneratorInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class GeneratorInspector : IGeneratorInspector, IInspector
  {
    private readonly InspectorModelMapper<IGeneratorInspector, GeneratorMapping> propertyMappings = new InspectorModelMapper<IGeneratorInspector, GeneratorMapping>();
    private readonly GeneratorMapping mapping;

    public GeneratorInspector(GeneratorMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Class;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public string Class => this.mapping.Class;

    public IDictionary<string, string> Params
    {
      get => (IDictionary<string, string>) new Dictionary<string, string>(this.mapping.Params);
    }
  }
}
