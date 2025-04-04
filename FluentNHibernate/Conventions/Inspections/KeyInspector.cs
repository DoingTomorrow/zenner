// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.KeyInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class KeyInspector : IKeyInspector, IInspector
  {
    private readonly InspectorModelMapper<IKeyInspector, KeyMapping> propertyMappings = new InspectorModelMapper<IKeyInspector, KeyMapping>();
    private readonly KeyMapping mapping;

    public KeyInspector(KeyMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => "";

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }

    public string ForeignKey => this.mapping.ForeignKey;

    public OnDelete OnDelete => OnDelete.FromString(this.mapping.OnDelete);

    public string PropertyRef => this.mapping.PropertyRef;
  }
}
