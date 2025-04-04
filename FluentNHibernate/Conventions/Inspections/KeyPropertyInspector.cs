// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.KeyPropertyInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class KeyPropertyInspector : IKeyPropertyInspector, IInspector
  {
    private readonly InspectorModelMapper<IKeyPropertyInspector, KeyPropertyMapping> mappedProperties = new InspectorModelMapper<IKeyPropertyInspector, KeyPropertyMapping>();
    private readonly KeyPropertyMapping mapping;

    public KeyPropertyInspector(KeyPropertyMapping mapping) => this.mapping = mapping;

    public System.Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public Access Access => Access.FromString(this.mapping.Access);

    public string Name => this.mapping.Name;

    public TypeReference Type => this.mapping.Type;

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }

    public int Length => this.mapping.Length;
  }
}
