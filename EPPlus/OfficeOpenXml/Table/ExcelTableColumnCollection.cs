// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.ExcelTableColumnCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace OfficeOpenXml.Table
{
  public class ExcelTableColumnCollection : IEnumerable<ExcelTableColumn>, IEnumerable
  {
    private List<ExcelTableColumn> _cols = new List<ExcelTableColumn>();
    private Dictionary<string, int> _colNames = new Dictionary<string, int>();

    public ExcelTableColumnCollection(ExcelTable table)
    {
      this.Table = table;
      foreach (XmlNode selectNode in table.TableXml.SelectNodes("//d:table/d:tableColumns/d:tableColumn", table.NameSpaceManager))
      {
        this._cols.Add(new ExcelTableColumn(table.NameSpaceManager, selectNode, table, this._cols.Count));
        this._colNames.Add(this._cols[this._cols.Count - 1].Name, this._cols.Count - 1);
      }
    }

    public ExcelTable Table { get; private set; }

    public int Count => this._cols.Count;

    public ExcelTableColumn this[int Index]
    {
      get
      {
        if (Index < 0 || Index >= this._cols.Count)
          throw new ArgumentOutOfRangeException("Column index out of range");
        return this._cols[Index];
      }
    }

    public ExcelTableColumn this[string Name]
    {
      get
      {
        return this._colNames.ContainsKey(Name) ? this._cols[this._colNames[Name]] : (ExcelTableColumn) null;
      }
    }

    IEnumerator<ExcelTableColumn> IEnumerable<ExcelTableColumn>.GetEnumerator()
    {
      return (IEnumerator<ExcelTableColumn>) this._cols.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._cols.GetEnumerator();
  }
}
