// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableRowColumnFieldCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableRowColumnFieldCollection : 
    ExcelPivotTableFieldCollectionBase<ExcelPivotTableField>
  {
    internal string _topNode;

    internal ExcelPivotTableRowColumnFieldCollection(ExcelPivotTable table, string topNode)
      : base(table)
    {
      this._topNode = topNode;
    }

    public ExcelPivotTableField Add(ExcelPivotTableField Field)
    {
      this.SetFlag(Field, true);
      this._list.Add(Field);
      return Field;
    }

    internal ExcelPivotTableField Insert(ExcelPivotTableField Field, int Index)
    {
      this.SetFlag(Field, true);
      this._list.Insert(Index, Field);
      return Field;
    }

    private void SetFlag(ExcelPivotTableField field, bool value)
    {
      switch (this._topNode)
      {
        case "rowFields":
          if (field.IsColumnField || field.IsPageField)
            throw new Exception("This field is a column or page field. Can't add it to the RowFields collection");
          field.IsRowField = value;
          field.Axis = ePivotFieldAxis.Row;
          break;
        case "colFields":
          if (field.IsRowField || field.IsPageField)
            throw new Exception("This field is a row or page field. Can't add it to the ColumnFields collection");
          field.IsColumnField = value;
          field.Axis = ePivotFieldAxis.Column;
          break;
        case "pageFields":
          if (field.IsColumnField || field.IsRowField)
            throw new Exception("Field is a column or row field. Can't add it to the PageFields collection");
          if (this._table.Address._fromRow < 3)
            throw new Exception(string.Format("A pivot table with page fields must be located above row 3. Currenct location is {0}", (object) this._table.Address.Address));
          field.IsPageField = value;
          field.Axis = ePivotFieldAxis.Page;
          break;
      }
    }

    public void Remove(ExcelPivotTableField Field)
    {
      if (!this._list.Contains(Field))
        throw new ArgumentException("Field not in collection");
      this.SetFlag(Field, false);
      this._list.Remove(Field);
    }

    public void RemoveAt(int Index)
    {
      if (Index > -1 && Index < this._list.Count)
        throw new IndexOutOfRangeException();
      this.SetFlag(this._list[Index], false);
      this._list.RemoveAt(Index);
    }
  }
}
