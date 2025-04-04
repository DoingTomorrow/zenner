// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IdentityInspector
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
  public class IdentityInspector : 
    ColumnBasedInspector,
    IIdentityInspector,
    IExposedThroughPropertyInspector,
    IIdentityInspectorBase,
    IInspector
  {
    private readonly InspectorModelMapper<IIdentityInspector, IdMapping> propertyMappings = new InspectorModelMapper<IIdentityInspector, IdMapping>();
    private readonly IdMapping mapping;

    public IdentityInspector(IdMapping mapping)
      : base(mapping.Columns)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IIdentityInspector, object>>) (x => (object) x.Nullable), "NotNull");
    }

    public System.Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public Member Property => this.mapping.Member;

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.EntityType, x))).Cast<IColumnInspector>();
      }
    }

    public IGeneratorInspector Generator
    {
      get
      {
        return this.mapping.Generator == null ? (IGeneratorInspector) new GeneratorInspector(new GeneratorMapping()) : (IGeneratorInspector) new GeneratorInspector(this.mapping.Generator);
      }
    }

    public string UnsavedValue => this.mapping.UnsavedValue;

    public string Name => this.mapping.Name;

    public Access Access => Access.FromString(this.mapping.Access);

    public TypeReference Type => this.mapping.Type;
  }
}
