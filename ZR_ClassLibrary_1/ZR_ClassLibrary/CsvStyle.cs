// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.CsvStyle
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

#nullable disable
namespace ZR_ClassLibrary
{
  [Serializable]
  public sealed class CsvStyle
  {
    public const string FILE_EXTENSION = ".sty";

    [XmlElement("Name", typeof (string))]
    public string Name { get; set; }

    [XmlElement("FirstRowContainsColumnNames", typeof (bool))]
    public bool FirstRowContainsColumnNames { get; set; }

    [XmlElement("StyleString", typeof (string))]
    public string StyleString { get; set; }

    public static CsvStyle DefaultStyle1
    {
      get
      {
        return new CsvStyle()
        {
          Name = "Example 1",
          FirstRowContainsColumnNames = true,
          StyleString = "$TimePoint$;$SerialNr$;$NodeName$;$PValueName$;$PValue$;$Unit$"
        };
      }
    }

    public static CsvStyle DefaultStyle2
    {
      get
      {
        return new CsvStyle()
        {
          Name = "Example 2",
          FirstRowContainsColumnNames = true,
          StyleString = "$TimePoint$      $[:d]TimePoint$         $[,-30:D]TimePoint$     $[:ddd, MMM d, yyyy]TimePoint$            $[:hh:mm]TimePoint$       $[,-20:MMMM-yyyy]TimePoint$"
        };
      }
    }

    public static CsvStyle DefaultStyle3
    {
      get
      {
        return new CsvStyle()
        {
          Name = "Example 3",
          FirstRowContainsColumnNames = true,
          StyleString = "|$[,20]SerialNr$|$[,-10]SerialNr$|$[,10]SerialNr$|$[,-20]SerialNr$|"
        };
      }
    }

    public string GenerateFile(DataTable sourceTable)
    {
      if (sourceTable == null)
        return string.Empty;
      if (!sourceTable.Columns.Contains("NodeID"))
        throw new ArgumentException("The column name NodeID does not exist in the target table!");
      if (string.IsNullOrEmpty(this.StyleString))
        throw new ArgumentNullException();
      if (this.StyleString.IndexOfAny(new char[2]
      {
        '{',
        '}'
      }) > 0)
        throw new ArgumentNullException("StyleString contains invalid chars!");
      Dictionary<int, string> fullAdditionalInfos = MeterDatabase.GetFullAdditionalInfos(sourceTable);
      DataTable table = this.PrepareDataTable(sourceTable, fullAdditionalInfos);
      string formatString = this.GenerateFormatString(table);
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        if (this.FirstRowContainsColumnNames)
        {
          List<string> stringList = new List<string>();
          foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
            stringList.Add(column.ColumnName);
          stringBuilder.AppendFormat((IFormatProvider) FixedFormates.TheFormates, formatString, (object[]) stringList.ToArray());
          stringBuilder.Append(ZR_Constants.SystemNewLine);
        }
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        {
          stringBuilder.AppendFormat((IFormatProvider) FixedFormates.TheFormates, formatString, row.ItemArray);
          stringBuilder.Append(ZR_Constants.SystemNewLine);
        }
      }
      catch
      {
        return string.Empty;
      }
      return stringBuilder.ToString();
    }

    public static List<string> GetAvailableColumns(DataTable table)
    {
      Dictionary<int, string> fullAdditionalInfos = MeterDatabase.GetFullAdditionalInfos(table);
      DataTable dataTable = new CsvStyle().PrepareDataTable(table, fullAdditionalInfos);
      List<string> availableColumns = new List<string>();
      foreach (DataColumn column in (InternalDataCollectionBase) dataTable.Columns)
        availableColumns.Add(column.ColumnName);
      return availableColumns;
    }

    public static string GetPath(CsvStyle style)
    {
      return style == null ? (string) null : Path.Combine(SystemValues.ExportStylesPath, style.Name + ".sty");
    }

    public static void Save(CsvStyle style)
    {
      if (style == null)
        return;
      string fileName = !string.IsNullOrEmpty(style.Name) ? CsvStyle.GetPath(style) : throw new ArgumentNullException("CsvStyle.Name");
      CsvStyle.Save(style, fileName);
    }

    public static void Save(CsvStyle style, string fileName)
    {
      if (style == null)
        return;
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException(nameof (fileName));
      File.Delete(fileName);
      using (Stream stream = (Stream) File.Open(fileName, FileMode.CreateNew, FileAccess.Write))
        new XmlSerializer(typeof (CsvStyle)).Serialize(stream, (object) style);
    }

    public static List<CsvStyle> Load()
    {
      string[] files = Directory.GetFiles(SystemValues.ExportStylesPath, "*.sty");
      if (files.Length == 0)
        return (List<CsvStyle>) null;
      List<CsvStyle> csvStyleList = new List<CsvStyle>(files.Length);
      foreach (string fileName in files)
      {
        CsvStyle csvStyle = CsvStyle.Load(fileName);
        if (csvStyle != null)
          csvStyleList.Add(csvStyle);
      }
      return csvStyleList;
    }

    public static CsvStyle Load(string fileName)
    {
      using (Stream stream = (Stream) File.Open(fileName, FileMode.Open, FileAccess.Read))
        return (CsvStyle) new XmlSerializer(typeof (CsvStyle)).Deserialize(stream);
    }

    public static void Delete(CsvStyle style)
    {
      if (style == null)
        return;
      File.Delete(Path.Combine(SystemValues.ExportStylesPath, style.Name + ".sty"));
    }

    private DataTable PrepareDataTable(
      DataTable sourceTable,
      Dictionary<int, string> additionalInfos)
    {
      if (additionalInfos == null || additionalInfos.Count <= 0)
        return sourceTable;
      DataTable dataTable = sourceTable.Copy();
      foreach (KeyValuePair<int, string> additionalInfo in additionalInfos)
      {
        foreach (string key in ParameterService.GetKeys(additionalInfo.Value.Trim(';')))
        {
          if (!dataTable.Columns.Contains(key))
            dataTable.Columns.Add(key, typeof (string));
        }
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int integer = Util.ToInteger(row["NodeID"]);
        if (additionalInfos.ContainsKey(integer))
        {
          string additionalInfo = additionalInfos[integer];
          char[] chArray = new char[1]{ ';' };
          foreach (string key in ParameterService.GetKeys(additionalInfo.Trim(chArray)))
            row[key] = (object) ParameterService.GetParameter(additionalInfos[integer].Trim(';'), key);
        }
      }
      return dataTable;
    }

    private string GenerateFormatString(DataTable table)
    {
      string input = this.StyleString;
      for (int index = 0; index < table.Columns.Count; ++index)
      {
        Regex regex1 = new Regex("\\$" + table.Columns[index].ColumnName + "\\$");
        if (regex1.IsMatch(input))
          input = regex1.Replace(input, "{" + index.ToString() + "}");
        Regex regex2 = new Regex("\\$\\[[^\\[]+\\]" + table.Columns[index].ColumnName + "\\$");
        if (regex2.IsMatch(input))
        {
          foreach (Match match in regex2.Matches(input))
          {
            string str1 = match.Value.Substring(2);
            string str2 = str1.Substring(0, str1.Length - 1).Replace("]" + table.Columns[index].ColumnName, string.Empty);
            input = input.Replace(match.Value, "{" + index.ToString() + str2 + "}");
          }
        }
      }
      return input;
    }
  }
}
