// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.KeyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class KeyInstance : KeyInspector, IKeyInstance, IKeyInspector, IInspector
  {
    private readonly KeyMapping mapping;

    public KeyInstance(KeyMapping mapping)
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

    public void ForeignKey(string constraint)
    {
      this.mapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.ForeignKey), 1, constraint);
    }

    public void PropertyRef(string property)
    {
      this.mapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.PropertyRef), 1, property);
    }

    public IEnumerable<IColumnInstance> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInstance>((Func<ColumnMapping, ColumnInstance>) (x => new ColumnInstance(this.mapping.ContainingEntityType, x))).Cast<IColumnInstance>();
      }
    }

    public void CascadeOnDelete()
    {
      this.mapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.OnDelete), 1, "cascade");
    }
  }
}
