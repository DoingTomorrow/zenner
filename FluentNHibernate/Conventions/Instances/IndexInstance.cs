// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IndexInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class IndexInstance : 
    IndexInspector,
    IIndexInstance,
    IIndexInstanceBase,
    IIndexInspector,
    IIndexInspectorBase,
    IInspector
  {
    private readonly IndexMapping mapping;

    public IndexInstance(IndexMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void Column(string columnName)
    {
      ColumnMapping columnMapping = this.mapping.Columns.FirstOrDefault<ColumnMapping>();
      ColumnMapping mapping = columnMapping == null ? new ColumnMapping() : columnMapping.Clone();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 1, columnName);
      this.mapping.AddColumn(1, mapping);
    }
  }
}
