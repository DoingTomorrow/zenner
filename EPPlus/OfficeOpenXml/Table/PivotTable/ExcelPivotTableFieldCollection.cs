// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Table.PivotTable.ExcelPivotTableFieldCollection
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.Table.PivotTable
{
  public class ExcelPivotTableFieldCollection : 
    ExcelPivotTableFieldCollectionBase<ExcelPivotTableField>
  {
    internal ExcelPivotTableFieldCollection(ExcelPivotTable table, string topNode)
      : base(table)
    {
    }

    public ExcelPivotTableField this[string name]
    {
      get
      {
        foreach (ExcelPivotTableField excelPivotTableField in this._list)
        {
          if (excelPivotTableField.Name == name)
            return excelPivotTableField;
        }
        return (ExcelPivotTableField) null;
      }
    }

    public ExcelPivotTableField GetDateGroupField(eDateGroupBy GroupBy)
    {
      foreach (ExcelPivotTableField dateGroupField in this._list)
      {
        if (dateGroupField.Grouping is ExcelPivotTableFieldDateGroup && ((ExcelPivotTableFieldDateGroup) dateGroupField.Grouping).GroupBy == GroupBy)
          return dateGroupField;
      }
      return (ExcelPivotTableField) null;
    }

    public ExcelPivotTableField GetNumericGroupField()
    {
      foreach (ExcelPivotTableField numericGroupField in this._list)
      {
        if (numericGroupField.Grouping is ExcelPivotTableFieldNumericGroup)
          return numericGroupField;
      }
      return (ExcelPivotTableField) null;
    }
  }
}
