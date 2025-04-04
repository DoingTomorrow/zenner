// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ManyToManyInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class ManyToManyInspector : IManyToManyInspector, IRelationshipInspector, IInspector
  {
    private readonly InspectorModelMapper<IManyToManyInspector, ManyToManyMapping> mappedProperties = new InspectorModelMapper<IManyToManyInspector, ManyToManyMapping>();
    private readonly ManyToManyMapping mapping;

    public ManyToManyInspector(ManyToManyMapping mapping)
    {
      this.mapping = mapping;
      this.mappedProperties.Map((Expression<Func<IManyToManyInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<ManyToManyMapping, object>>) (x => (object) x.Lazy));
    }

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Class.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }

    public Type ChildType => this.mapping.ChildType;

    public TypeReference Class => this.mapping.Class;

    public Fetch Fetch => Fetch.FromString(this.mapping.Fetch);

    public string ForeignKey => this.mapping.ForeignKey;

    public bool LazyLoad => this.mapping.Lazy;

    public NotFound NotFound => NotFound.FromString(this.mapping.NotFound);

    public Type ParentType => this.mapping.ParentType;

    public string Where => this.mapping.Where;

    public string OrderBy => this.mapping.OrderBy;
  }
}
