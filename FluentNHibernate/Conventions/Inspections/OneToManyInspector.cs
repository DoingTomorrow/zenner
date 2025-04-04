// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.OneToManyInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class OneToManyInspector : IOneToManyInspector, IRelationshipInspector, IInspector
  {
    private readonly InspectorModelMapper<IOneToManyInspector, OneToManyMapping> mappedProperties = new InspectorModelMapper<IOneToManyInspector, OneToManyMapping>();
    private readonly OneToManyMapping mapping;

    public OneToManyInspector(OneToManyMapping mapping) => this.mapping = mapping;

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Class.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public Type ChildType => this.mapping.ChildType;

    public TypeReference Class => this.mapping.Class;

    public NotFound NotFound => NotFound.FromString(this.mapping.NotFound);
  }
}
