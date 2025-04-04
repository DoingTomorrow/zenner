// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.KeyManyToOneInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class KeyManyToOneInspector : IKeyManyToOneInspector, IInspector
  {
    private readonly InspectorModelMapper<IKeyManyToOneInspector, KeyManyToOneMapping> mappedProperties = new InspectorModelMapper<IKeyManyToOneInspector, KeyManyToOneMapping>();
    private readonly KeyManyToOneMapping mapping;

    public KeyManyToOneInspector(KeyManyToOneMapping mapping)
    {
      this.mapping = mapping;
      this.mappedProperties.Map((Expression<Func<IKeyManyToOneInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<KeyManyToOneMapping, object>>) (x => (object) x.Lazy));
    }

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public Access Access => Access.FromString(this.mapping.Access);

    public TypeReference Class => this.mapping.Class;

    public string ForeignKey => this.mapping.ForeignKey;

    public bool LazyLoad => this.mapping.Lazy;

    public string Name => this.mapping.Name;

    public NotFound NotFound => NotFound.FromString(this.mapping.NotFound);

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }
  }
}
