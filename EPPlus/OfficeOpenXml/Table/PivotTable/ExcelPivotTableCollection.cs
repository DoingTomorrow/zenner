// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Packaging;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableCollection : IEnumerable<ExcelPivotTable>, IEnumerable
  {
    private List<ExcelPivotTable> _pivotTables = new List<ExcelPivotTable>();
    internal Dictionary<string, int> _pivotTableNames = new Dictionary<string, int>();
    private ExcelWorksheet _ws;

    internal ExcelPivotTableCollection(ExcelWorksheet ws)
    {
      ZipPackage package = ws._package.Package;
      this._ws = ws;
      foreach (ZipPackageRelationship relationship in ws.Part.GetRelationships())
      {
        if (relationship.RelationshipType == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotTable")
        {
          ExcelPivotTable excelPivotTable = new ExcelPivotTable(relationship, ws);
          this._pivotTableNames.Add(excelPivotTable.Name, this._pivotTables.Count);
          if (excelPivotTable.Id + 1 > this._ws.Workbook._nextPivotTableID)
            this._ws.Workbook._nextPivotTableID = excelPivotTable.Id + 1;
          this._pivotTables.Add(excelPivotTable);
        }
      }
    }

    private ExcelPivotTable Add(ExcelPivotTable tbl)
    {
      this._pivotTables.Add(tbl);
      this._pivotTableNames.Add(tbl.Name, this._pivotTables.Count - 1);
      if (tbl.Id >= this._ws.Workbook._nextPivotTableID)
        this._ws.Workbook._nextPivotTableID = tbl.Id + 1;
      return tbl;
    }

    public ExcelPivotTable Add(ExcelAddressBase Range, ExcelRangeBase Source, string Name)
    {
      if (string.IsNullOrEmpty(Name))
        Name = this.GetNewTableName();
      if (Range.WorkSheet != this._ws.Name)
        throw new Exception("The Range must be in the current worksheet");
      if (this._ws.Workbook.ExistsTableName(Name))
        throw new ArgumentException("Tablename is not unique");
      foreach (ExcelPivotTable pivotTable in this._pivotTables)
      {
        if (pivotTable.Address.Collide(Range) != ExcelAddressBase.eAddressCollition.No)
          throw new ArgumentException(string.Format("Table range collides with table {0}", (object) pivotTable.Name));
      }
      return this.Add(new ExcelPivotTable(this._ws, Range, Source, Name, this._ws.Workbook._nextPivotTableID++));
    }

    internal string GetNewTableName()
    {
      string Name = "Pivottable1";
      int num = 2;
      while (this._ws.Workbook.ExistsPivotTableName(Name))
        Name = string.Format("Pivottable{0}", (object) num++);
      return Name;
    }

    public int Count => this._pivotTables.Count;

    public ExcelPivotTable this[int Index]
    {
      get
      {
        if (Index < 0 || Index >= this._pivotTables.Count)
          throw new ArgumentOutOfRangeException("PivotTable index out of range");
        return this._pivotTables[Index];
      }
    }

    public ExcelPivotTable this[string Name]
    {
      get
      {
        return this._pivotTableNames.ContainsKey(Name) ? this._pivotTables[this._pivotTableNames[Name]] : (ExcelPivotTable) null;
      }
    }

    public IEnumerator<ExcelPivotTable> GetEnumerator()
    {
      return (IEnumerator<ExcelPivotTable>) this._pivotTables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._pivotTables.GetEnumerator();
  }
}
