// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ColumnPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ColumnPart
  {
    private ColumnMapping columnMapping;
    private bool nextBool = true;

    public ColumnPart(ColumnMapping columnMapping) => this.columnMapping = columnMapping;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ColumnPart Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public ColumnPart Name(string columnName)
    {
      this.columnMapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 2, columnName);
      return this;
    }

    public ColumnPart Length(int length)
    {
      this.columnMapping.Set<int>((Expression<Func<ColumnMapping, int>>) (x => x.Length), 2, length);
      return this;
    }

    public ColumnPart Nullable()
    {
      this.columnMapping.Set<bool>((Expression<Func<ColumnMapping, bool>>) (x => x.NotNull), 2, (!this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
      return this;
    }

    public ColumnPart Unique()
    {
      this.columnMapping.Set<bool>((Expression<Func<ColumnMapping, bool>>) (x => x.Unique), 2, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
      return this;
    }

    public ColumnPart UniqueKey(string key)
    {
      this.columnMapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.UniqueKey), 2, key);
      return this;
    }

    public ColumnPart SqlType(string sqlType)
    {
      this.columnMapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.SqlType), 2, sqlType);
      return this;
    }

    public ColumnPart Index(string index)
    {
      this.columnMapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Index), 2, index);
      return this;
    }
  }
}
