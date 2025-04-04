// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.DiscriminatorInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class DiscriminatorInspector : ColumnBasedInspector, IDiscriminatorInspector, IInspector
  {
    private readonly InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping> propertyMappings = new InspectorModelMapper<IDiscriminatorInspector, DiscriminatorMapping>();
    private readonly DiscriminatorMapping mapping;

    public DiscriminatorInspector(DiscriminatorMapping mapping)
      : base(mapping.Columns)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IDiscriminatorInspector, object>>) (x => (object) x.Nullable), "NotNull");
    }

    public bool Insert => this.mapping.Insert;

    public bool Force => this.mapping.Force;

    public string Formula => this.mapping.Formula;

    public TypeReference Type => this.mapping.Type;

    public System.Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Type.Name;

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return (IEnumerable<IColumnInspector>) this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>().ToList<IColumnInspector>();
      }
    }

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }
  }
}
