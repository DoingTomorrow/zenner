// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.SchemaHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public static class SchemaHelper
  {
    public static object GetValue(DataRow row, params string[] alternativeColumnNames)
    {
      if (alternativeColumnNames == null)
        throw new ArgumentNullException(nameof (alternativeColumnNames));
      if (alternativeColumnNames.Length < 1)
        throw new ArgumentOutOfRangeException(nameof (alternativeColumnNames), "At least one name must be given.");
      foreach (string alternativeColumnName in alternativeColumnNames)
      {
        if (row.Table.Columns.Contains(alternativeColumnName))
          return row[alternativeColumnName];
      }
      throw new Exception("None of the alternative column names found in the DataTable.");
    }

    public static string GetString(DataRow row, params string[] alternativeColumnNames)
    {
      return Convert.ToString(SchemaHelper.GetValue(row, alternativeColumnNames));
    }
  }
}
