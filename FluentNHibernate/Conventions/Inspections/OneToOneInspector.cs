// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.OneToOneInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class OneToOneInspector : IOneToOneInspector, IInspector
  {
    private readonly InspectorModelMapper<IOneToOneInspector, OneToOneMapping> propertyMappings = new InspectorModelMapper<IOneToOneInspector, OneToOneMapping>();
    private readonly OneToOneMapping mapping;

    public OneToOneInspector(OneToOneMapping mapping)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IOneToOneInspector, object>>) (x => x.LazyLoad), (Expression<Func<OneToOneMapping, object>>) (x => x.Lazy));
    }

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public Access Access => Access.FromString(this.mapping.Access);

    public Cascade Cascade => Cascade.FromString(this.mapping.Cascade);

    public TypeReference Class => this.mapping.Class;

    public bool Constrained => this.mapping.Constrained;

    public Fetch Fetch => Fetch.FromString(this.mapping.Fetch);

    public string ForeignKey => this.mapping.ForeignKey;

    public Laziness LazyLoad => new Laziness(this.mapping.Lazy);

    public string Name => this.mapping.Name;

    public string PropertyRef => this.mapping.PropertyRef;
  }
}
