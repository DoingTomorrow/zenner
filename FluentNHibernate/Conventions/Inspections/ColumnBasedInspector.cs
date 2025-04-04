// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ColumnBasedInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public abstract class ColumnBasedInspector
  {
    private readonly IEnumerable<ColumnMapping> columns;

    protected ColumnBasedInspector(IEnumerable<ColumnMapping> columns) => this.columns = columns;

    private T GetValueFromColumns<T>(Func<ColumnMapping, object> property)
    {
      ColumnMapping columnMapping = this.columns.FirstOrDefault<ColumnMapping>();
      return columnMapping != null ? (T) property(columnMapping) : default (T);
    }

    public int Length
    {
      get => this.GetValueFromColumns<int>((Func<ColumnMapping, object>) (x => (object) x.Length));
    }

    public bool Nullable
    {
      get
      {
        return !this.GetValueFromColumns<bool>((Func<ColumnMapping, object>) (x => (object) x.NotNull));
      }
    }

    public string SqlType
    {
      get
      {
        return this.GetValueFromColumns<string>((Func<ColumnMapping, object>) (x => (object) x.SqlType));
      }
    }

    public bool Unique
    {
      get => this.GetValueFromColumns<bool>((Func<ColumnMapping, object>) (x => (object) x.Unique));
    }

    public string UniqueKey
    {
      get
      {
        return this.GetValueFromColumns<string>((Func<ColumnMapping, object>) (x => (object) x.UniqueKey));
      }
    }

    public string Index
    {
      get
      {
        return this.GetValueFromColumns<string>((Func<ColumnMapping, object>) (x => (object) x.Index));
      }
    }

    public string Check
    {
      get
      {
        return this.GetValueFromColumns<string>((Func<ColumnMapping, object>) (x => (object) x.Check));
      }
    }

    public string Default
    {
      get
      {
        return this.GetValueFromColumns<string>((Func<ColumnMapping, object>) (x => (object) x.Default));
      }
    }

    public int Precision
    {
      get
      {
        return this.GetValueFromColumns<int>((Func<ColumnMapping, object>) (x => (object) x.Precision));
      }
    }

    public int Scale
    {
      get => this.GetValueFromColumns<int>((Func<ColumnMapping, object>) (x => (object) x.Scale));
    }
  }
}
