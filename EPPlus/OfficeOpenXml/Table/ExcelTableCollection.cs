// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.ExcelTableCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table
{
  public class ExcelTableCollection : IEnumerable<ExcelTable>, IEnumerable
  {
    private List<ExcelTable> _tables = new List<ExcelTable>();
    internal Dictionary<string, int> _tableNames = new Dictionary<string, int>();
    private ExcelWorksheet _ws;

    internal ExcelTableCollection(ExcelWorksheet ws)
    {
      ZipPackage package = ws._package.Package;
      this._ws = ws;
      foreach (XmlElement selectNode in ws.WorksheetXml.SelectNodes("//d:tableParts/d:tablePart", ws.NameSpaceManager))
      {
        ExcelTable excelTable = new ExcelTable(ws.Part.GetRelationship(selectNode.GetAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships")), ws);
        if (excelTable.Id + 1 > this._ws.Workbook._nextTableID)
          this._ws.Workbook._nextTableID = excelTable.Id + 1;
        this._tableNames.Add(excelTable.Name, this._tables.Count);
        this._tables.Add(excelTable);
      }
    }

    private ExcelTable Add(ExcelTable tbl)
    {
      this._tables.Add(tbl);
      this._tableNames.Add(tbl.Name, this._tables.Count - 1);
      if (tbl.Id >= this._ws.Workbook._nextTableID)
        this._ws.Workbook._nextTableID = tbl.Id + 1;
      return tbl;
    }

    public ExcelTable Add(ExcelAddressBase Range, string Name)
    {
      if (string.IsNullOrEmpty(Name))
        Name = this.GetNewTableName();
      else if (this._ws.Workbook.ExistsTableName(Name))
        throw new ArgumentException("Tablename is not unique");
      foreach (ExcelTable table in this._tables)
      {
        if (table.Address.Collide(Range) != ExcelAddressBase.eAddressCollition.No)
          throw new ArgumentException(string.Format("Table range collides with table {0}", (object) table.Name));
      }
      return this.Add(new ExcelTable(this._ws, Range, Name, this._ws.Workbook._nextTableID));
    }

    internal string GetNewTableName()
    {
      string Name = "Table1";
      int num = 2;
      while (this._ws.Workbook.ExistsTableName(Name))
        Name = string.Format("Table{0}", (object) num++);
      return Name;
    }

    public int Count => this._tables.Count;

    public ExcelTable GetFromRange(ExcelRangeBase Range)
    {
      foreach (ExcelTable table in Range.Worksheet.Tables)
      {
        if (table.Address._address == Range._address)
          return table;
      }
      return (ExcelTable) null;
    }

    public ExcelTable this[int Index]
    {
      get
      {
        if (Index < 0 || Index >= this._tables.Count)
          throw new ArgumentOutOfRangeException("Table index out of range");
        return this._tables[Index];
      }
    }

    public ExcelTable this[string Name]
    {
      get
      {
        return this._tableNames.ContainsKey(Name) ? this._tables[this._tableNames[Name]] : (ExcelTable) null;
      }
    }

    public IEnumerator<ExcelTable> GetEnumerator()
    {
      return (IEnumerator<ExcelTable>) this._tables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._tables.GetEnumerator();
  }
}
