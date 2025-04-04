// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ManyToOneInspector
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
  public class ManyToOneInspector : 
    IManyToOneInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    private readonly InspectorModelMapper<IManyToOneInspector, ManyToOneMapping> propertyMappings = new InspectorModelMapper<IManyToOneInspector, ManyToOneMapping>();
    private readonly ManyToOneMapping mapping;

    public ManyToOneInspector(ManyToOneMapping mapping)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IManyToOneInspector, object>>) (x => x.LazyLoad), (Expression<Func<ManyToOneMapping, object>>) (x => x.Lazy));
      this.propertyMappings.Map((Expression<Func<IManyToOneInspector, object>>) (x => (object) x.Nullable), "NotNull");
    }

    public Access Access => Access.FromString(this.mapping.Access);

    public NotFound NotFound => NotFound.FromString(this.mapping.NotFound);

    public string PropertyRef => this.mapping.PropertyRef;

    public bool Update => this.mapping.Update;

    public bool Nullable
    {
      get
      {
        return this.mapping.Columns.Any<ColumnMapping>() && !this.mapping.Columns.First<ColumnMapping>().NotNull;
      }
    }

    public bool OptimisticLock => this.mapping.OptimisticLock;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      string mappedProperty = this.propertyMappings.Get(property);
      return this.mapping.Columns.Any<ColumnMapping>((Func<ColumnMapping, bool>) (x => x.IsSpecified(mappedProperty))) || this.mapping.IsSpecified(mappedProperty);
    }

    public Member Property => this.mapping.Member;

    public string Name => this.mapping.Name;

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }

    public Cascade Cascade => Cascade.FromString(this.mapping.Cascade);

    public string Formula => this.mapping.Formula;

    public TypeReference Class => this.mapping.Class;

    public Fetch Fetch => Fetch.FromString(this.mapping.Fetch);

    public string ForeignKey => this.mapping.ForeignKey;

    public bool Insert => this.mapping.Insert;

    public Laziness LazyLoad => new Laziness(this.mapping.Lazy);
  }
}
