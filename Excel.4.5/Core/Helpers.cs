// Decompiled with JetBrains decompiler
// Type: Excel.Core.Helpers
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Excel.Core
{
  internal static class Helpers
  {
    private static Regex re = new Regex("_x([0-9A-F]{4,4})_");

    public static bool IsSingleByteEncoding(Encoding encoding) => encoding.IsSingleByte;

    public static double Int64BitsToDouble(long value)
    {
      return BitConverter.ToDouble(BitConverter.GetBytes(value), 0);
    }

    public static string ConvertEscapeChars(string input)
    {
      return Helpers.re.Replace(input, (MatchEvaluator) (m => ((char) uint.Parse(m.Groups[1].Value, NumberStyles.HexNumber)).ToString()));
    }

    public static object ConvertFromOATime(double value)
    {
      if (value >= 0.0 && value < 60.0)
        ++value;
      return (object) DateTime.FromOADate(value);
    }

    internal static void FixDataTypes(DataSet dataset)
    {
      List<DataTable> dataTableList = new List<DataTable>(dataset.Tables.Count);
      bool flag = false;
      foreach (DataTable table in (InternalDataCollectionBase) dataset.Tables)
      {
        if (table.Rows.Count == 0)
        {
          dataTableList.Add(table);
        }
        else
        {
          DataTable dataTable = (DataTable) null;
          for (int index = 0; index < table.Columns.Count; ++index)
          {
            Type type1 = (Type) null;
            foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
            {
              if (!row.IsNull(index))
              {
                Type type2 = row[index].GetType();
                if (Type.op_Inequality(type2, type1))
                {
                  if (Type.op_Equality(type1, (Type) null))
                  {
                    type1 = type2;
                  }
                  else
                  {
                    type1 = (Type) null;
                    break;
                  }
                }
              }
            }
            if (Type.op_Inequality(type1, (Type) null))
            {
              flag = true;
              if (dataTable == null)
                dataTable = table.Clone();
              dataTable.Columns[index].DataType = type1;
            }
          }
          if (dataTable != null)
          {
            dataTable.BeginLoadData();
            foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
              dataTable.ImportRow(row);
            dataTable.EndLoadData();
            dataTableList.Add(dataTable);
          }
          else
            dataTableList.Add(table);
        }
      }
      if (!flag)
        return;
      dataset.Tables.Clear();
      dataset.Tables.AddRange(dataTableList.ToArray());
    }

    public static void AddColumnHandleDuplicate(DataTable table, string columnName)
    {
      string str = columnName;
      DataColumn column = table.Columns[columnName];
      int num = 1;
      while (column != null)
      {
        str = string.Format("{0}_{1}", (object) columnName, (object) num);
        column = table.Columns[str];
        ++num;
      }
      table.Columns.Add(str, typeof (object));
    }
  }
}
