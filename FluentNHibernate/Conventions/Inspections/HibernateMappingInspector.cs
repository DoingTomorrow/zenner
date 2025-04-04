// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.HibernateMappingInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class HibernateMappingInspector : IHibernateMappingInspector, IInspector
  {
    private readonly InspectorModelMapper<IHibernateMappingInspector, HibernateMapping> propertyMappings = new InspectorModelMapper<IHibernateMappingInspector, HibernateMapping>();
    private readonly HibernateMapping mapping;

    public HibernateMappingInspector(HibernateMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.Classes.First<ClassMapping>().Type;

    public string StringIdentifierForModel => this.mapping.Classes.First<ClassMapping>().Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public string Catalog => this.mapping.Catalog;

    public Access DefaultAccess => Access.FromString(this.mapping.DefaultAccess);

    public Cascade DefaultCascade => Cascade.FromString(this.mapping.DefaultCascade);

    public bool DefaultLazy => this.mapping.DefaultLazy;

    public bool AutoImport => this.mapping.AutoImport;

    public string Schema => this.mapping.Schema;
  }
}
