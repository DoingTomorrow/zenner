// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ColumnMappingCollection`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ColumnMappingCollection<TParent> : IEnumerable<ColumnMapping>, IEnumerable
  {
    private readonly IList<ColumnMapping> columns = (IList<ColumnMapping>) new List<ColumnMapping>();
    private readonly TParent parent;

    public ColumnMappingCollection(TParent parent) => this.parent = parent;

    public TParent Add(string name)
    {
      ColumnMapping columnMapping = new ColumnMapping();
      columnMapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 2, name);
      this.columns.Add(columnMapping);
      return this.parent;
    }

    public TParent Add(params string[] names)
    {
      foreach (string name in names)
        this.Add(name);
      return this.parent;
    }

    public TParent Add(string columnName, Action<ColumnPart> customColumnMapping)
    {
      ColumnMapping columnMapping = new ColumnMapping();
      columnMapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 2, columnName);
      ColumnPart columnPart = new ColumnPart(columnMapping);
      customColumnMapping(columnPart);
      this.columns.Add(columnMapping);
      return this.parent;
    }

    public TParent Add(ColumnMapping column)
    {
      this.columns.Add(column);
      return this.parent;
    }

    public TParent Clear()
    {
      this.columns.Clear();
      return this.parent;
    }

    public int Count => this.columns.Count;

    public IEnumerator<ColumnMapping> GetEnumerator() => this.columns.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
