// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.PropertyInspector
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
  public class PropertyInspector : 
    ColumnBasedInspector,
    IPropertyInspector,
    IReadOnlyInspector,
    IExposedThroughPropertyInspector,
    IInspector,
    IAccessInspector
  {
    private readonly InspectorModelMapper<IPropertyInspector, PropertyMapping> propertyMappings = new InspectorModelMapper<IPropertyInspector, PropertyMapping>();
    private readonly PropertyMapping mapping;

    public PropertyInspector(PropertyMapping mapping)
      : base(mapping.Columns)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IPropertyInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<PropertyMapping, object>>) (x => (object) x.Lazy));
      this.propertyMappings.Map((Expression<Func<IPropertyInspector, object>>) (x => (object) x.Nullable), "NotNull");
    }

    public bool Insert => this.mapping.Insert;

    public bool Update => this.mapping.Update;

    public string Formula => this.mapping.Formula;

    public TypeReference Type => this.mapping.Type;

    public string Name => this.mapping.Name;

    public bool OptimisticLock => this.mapping.OptimisticLock;

    public Generated Generated => Generated.FromString(this.mapping.Generated);

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return (IEnumerable<IColumnInspector>) this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>().ToList<IColumnInspector>();
      }
    }

    public bool LazyLoad => this.mapping.Lazy;

    public Access Access
    {
      get => this.mapping.Access != null ? Access.FromString(this.mapping.Access) : Access.Unset;
    }

    public System.Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool ReadOnly => this.mapping.Insert && this.mapping.Update;

    public Member Property => this.mapping.Member;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }
  }
}
