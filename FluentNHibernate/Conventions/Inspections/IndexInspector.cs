// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IndexInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class IndexInspector : IIndexInspector, IIndexInspectorBase, IInspector
  {
    private readonly InspectorModelMapper<IIndexInspector, IndexMapping> mappedProperties = new InspectorModelMapper<IIndexInspector, IndexMapping>();
    private readonly IndexMapping mapping;

    public IndexInspector(IndexMapping mapping) => this.mapping = mapping;

    public System.Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Type.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public TypeReference Type => this.mapping.Type;

    public IEnumerable<IColumnInspector> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInspector>((Func<ColumnMapping, ColumnInspector>) (x => new ColumnInspector(this.mapping.ContainingEntityType, x))).Cast<IColumnInspector>();
      }
    }
  }
}
