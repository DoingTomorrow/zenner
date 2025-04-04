// Decompiled with JetBrains decompiler
// Type: NHibernate.AdoNet.ColumnNameCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.AdoNet
{
  public class ColumnNameCache
  {
    private readonly Dictionary<string, int?> columnNameToIndexCache;

    public ColumnNameCache(int columnCount)
    {
      this.columnNameToIndexCache = new Dictionary<string, int?>(columnCount);
    }

    public int GetIndexForColumnName(string columnName, ResultSetWrapper rs)
    {
      int? nullable;
      this.columnNameToIndexCache.TryGetValue(columnName, out nullable);
      if (nullable.HasValue)
        return nullable.Value;
      int ordinal = rs.Target.GetOrdinal(columnName);
      this.columnNameToIndexCache[columnName] = new int?(ordinal);
      return ordinal;
    }
  }
}
