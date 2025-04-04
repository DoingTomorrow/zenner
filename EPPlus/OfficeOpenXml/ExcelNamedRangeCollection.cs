// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.ExcelNamedRangeCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml
{
  public class ExcelNamedRangeCollection : IEnumerable<ExcelNamedRange>, IEnumerable
  {
    internal ExcelWorksheet _ws;
    internal ExcelWorkbook _wb;
    private List<ExcelNamedRange> _list = new List<ExcelNamedRange>();
    private Dictionary<string, int> _dic = new Dictionary<string, int>();

    internal ExcelNamedRangeCollection(ExcelWorkbook wb)
    {
      this._wb = wb;
      this._ws = (ExcelWorksheet) null;
    }

    internal ExcelNamedRangeCollection(ExcelWorkbook wb, ExcelWorksheet ws)
    {
      this._wb = wb;
      this._ws = ws;
    }

    public ExcelNamedRange Add(string Name, ExcelRangeBase Range)
    {
      ExcelNamedRange excelNamedRange = !Range.IsName ? new ExcelNamedRange(Name, this._ws, Range.Worksheet, Range.Address, this._dic.Count) : new ExcelNamedRange(Name, this._wb, this._ws, this._dic.Count);
      this.AddName(Name, excelNamedRange);
      return excelNamedRange;
    }

    private void AddName(string Name, ExcelNamedRange item)
    {
      this._dic.Add(Name.ToLower(), this._list.Count);
      this._list.Add(item);
    }

    public ExcelNamedRange AddValue(string Name, object value)
    {
      ExcelNamedRange excelNamedRange = new ExcelNamedRange(Name, this._wb, this._ws, this._dic.Count);
      excelNamedRange.NameValue = value;
      this.AddName(Name, excelNamedRange);
      return excelNamedRange;
    }

    [Obsolete("Call AddFormula() instead.  See Issue Tracker Id #14687")]
    public ExcelNamedRange AddFormla(string Name, string Formula) => this.AddFormula(Name, Formula);

    public ExcelNamedRange AddFormula(string Name, string Formula)
    {
      ExcelNamedRange excelNamedRange = new ExcelNamedRange(Name, this._wb, this._ws, this._dic.Count);
      excelNamedRange.NameFormula = Formula;
      this.AddName(Name, excelNamedRange);
      return excelNamedRange;
    }

    public void Remove(string Name)
    {
      Name = Name.ToLower();
      if (!this._dic.ContainsKey(Name))
        return;
      int index1 = this._dic[Name];
      for (int index2 = index1 + 1; index2 < this._list.Count; ++index2)
      {
        this._dic.Remove(this._list[index2].Name.ToLower());
        --this._list[index2].Index;
        this._dic.Add(this._list[index2].Name.ToLower(), this._list[index2].Index);
      }
      this._dic.Remove(Name);
      this._list.RemoveAt(index1);
    }

    public bool ContainsKey(string key) => this._dic.ContainsKey(key.ToLower());

    public int Count => this._dic.Count;

    public ExcelNamedRange this[string Name] => this._list[this._dic[Name.ToLower()]];

    public ExcelNamedRange this[int Index] => this._list[Index];

    public IEnumerator<ExcelNamedRange> GetEnumerator()
    {
      return (IEnumerator<ExcelNamedRange>) this._list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
  }
}
