// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.VersionInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class VersionInspector : ColumnBasedInspector, IVersionInspector, IInspector
  {
    private readonly InspectorModelMapper<IVersionInspector, VersionMapping> propertyMappings = new InspectorModelMapper<IVersionInspector, VersionMapping>();
    private readonly VersionMapping mapping;

    public VersionInspector(VersionMapping mapping)
      : base(mapping.Columns)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IVersionInspector, object>>) (x => (object) x.Nullable), "NotNull");
    }

    public System.Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public string Name => this.mapping.Name;

    public Access Access => Access.FromString(this.mapping.Access);

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return (IEnumerable<IColumnInspector>) this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>().ToList<IColumnInspector>();
      }
    }

    public Generated Generated => Generated.FromString(this.mapping.Generated);

    public string UnsavedValue => this.mapping.UnsavedValue;

    public TypeReference Type => this.mapping.Type;
  }
}
