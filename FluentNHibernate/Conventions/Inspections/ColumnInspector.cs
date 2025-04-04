// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ColumnInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class ColumnInspector : IColumnInspector, IInspector
  {
    private readonly ColumnMapping mapping;
    private readonly InspectorModelMapper<IColumnInspector, ColumnMapping> propertyMappings = new InspectorModelMapper<IColumnInspector, ColumnMapping>();

    public ColumnInspector(Type containingEntityType, ColumnMapping mapping)
    {
      this.EntityType = containingEntityType;
      this.mapping = mapping;
    }

    public Type EntityType { get; private set; }

    public string Name => this.mapping.Name;

    public string Check => this.mapping.Check;

    public string Index => this.mapping.Index;

    public int Length => this.mapping.Length;

    public bool NotNull => this.mapping.NotNull;

    public string SqlType => this.mapping.SqlType;

    public bool Unique => this.mapping.Unique;

    public string UniqueKey => this.mapping.UniqueKey;

    public int Precision => this.mapping.Precision;

    public int Scale => this.mapping.Scale;

    public string Default => this.mapping.Default;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }
  }
}
