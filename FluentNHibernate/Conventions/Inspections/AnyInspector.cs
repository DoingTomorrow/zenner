// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.AnyInspector
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
  public class AnyInspector : IAnyInspector, IAccessInspector, IInspector
  {
    private readonly InspectorModelMapper<IAnyInspector, AnyMapping> propertyMappings = new InspectorModelMapper<IAnyInspector, AnyMapping>();
    private readonly AnyMapping mapping;

    public AnyInspector(AnyMapping mapping)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IAnyInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<AnyMapping, object>>) (x => (object) x.Lazy));
    }

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public Access Access => Access.FromString(this.mapping.Access);

    public Cascade Cascade => Cascade.FromString(this.mapping.Cascade);

    public IEnumerable<IColumnInspector> IdentifierColumns
    {
      get
      {
        return this.mapping.IdentifierColumns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }

    public string IdType => this.mapping.IdType;

    public bool Insert => this.mapping.Insert;

    public TypeReference MetaType => this.mapping.MetaType;

    public IEnumerable<IMetaValueInspector> MetaValues
    {
      get
      {
        return this.mapping.MetaValues.Select<MetaValueMapping, MetaValueInspector>((Func<MetaValueMapping, MetaValueInspector>) (x => new MetaValueInspector(x))).Cast<IMetaValueInspector>();
      }
    }

    public string Name => this.mapping.Name;

    public IEnumerable<IColumnInspector> TypeColumns
    {
      get
      {
        return this.mapping.TypeColumns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }

    public bool Update => this.mapping.Update;

    public bool LazyLoad => this.mapping.Lazy;

    public bool OptimisticLock => this.mapping.OptimisticLock;
  }
}
