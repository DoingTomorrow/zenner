// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ExcelConnect
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ExcelConnect
  {
    private _Application MyExcel;
    private _Workbook MyWorkbook;
    private Worksheet MainWorkSheet;
    private Microsoft.Office.Interop.Excel.Range MainWorkSheetRang;
    private object[,] MainWorkSheetArray;
    private int MaxWorkSheetArrayRows;
    private int MaxWorkSheetArrayColumns;
    private int WorksheetIndex = 1;

    public ExcelConnect(string workBookPath)
    {
      this.MyExcel = (_Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
      // ISSUE: reference to a compiler-generated method
      this.MyExcel.Workbooks.Open(workBookPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.MainWorkSheet = ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__0, this.MyExcel.Workbooks[(object) 1].Worksheets[(object) 1]);
      this.MainWorkSheetRang = this.MainWorkSheet.UsedRange;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object[,]>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (object[,]), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.MainWorkSheetArray = ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__1.Target((CallSite) ExcelConnect.\u003C\u003Eo__8.\u003C\u003Ep__1, this.MainWorkSheetRang.get_Value((object) XlRangeValueDataType.xlRangeValueDefault));
      this.MaxWorkSheetArrayRows = this.MainWorkSheetArray.GetLength(0);
      this.MaxWorkSheetArrayColumns = this.MainWorkSheetArray.GetLength(1);
    }

    public string[] GetWorkbookRow(int row, int columns)
    {
      if (row >= this.MaxWorkSheetArrayRows)
        return (string[]) null;
      int num = columns;
      if (this.MaxWorkSheetArrayColumns < num)
        num = this.MaxWorkSheetArrayColumns;
      string[] workbookRow = new string[columns];
      for (int index = 0; index < num; ++index)
      {
        object obj = this.MainWorkSheetArray.GetValue(row + 1, index + 1);
        if (obj != null)
          workbookRow[index] = obj.ToString();
      }
      return workbookRow;
    }

    public object[,] GetWorkSheetData(string workSheetName)
    {
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: variable of a compiler-generated type
      Worksheet worksheet1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.MainWorkSheet = worksheet1 = ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__0, this.MyExcel.Workbooks[(object) 1].Worksheets[(object) workSheetName]);
      // ISSUE: variable of a compiler-generated type
      Worksheet worksheet2 = worksheet1;
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range usedRange = worksheet2.UsedRange;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object[,]>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (object[,]), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__1.Target((CallSite) ExcelConnect.\u003C\u003Eo__10.\u003C\u003Ep__1, usedRange.get_Value((object) XlRangeValueDataType.xlRangeValueDefault));
    }

    public ExcelConnect()
    {
      this.MyExcel = (_Application) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
      // ISSUE: reference to a compiler-generated method
      this.MyWorkbook = (_Workbook) this.MyExcel.Workbooks.Add((object) XlWBATemplate.xlWBATWorksheet);
    }

    public void Close()
    {
      try
      {
        // ISSUE: reference to a compiler-generated method
        this.MyWorkbook.Close(Type.Missing, Type.Missing, Type.Missing);
        // ISSUE: reference to a compiler-generated method
        this.MyExcel.Application.Quit();
      }
      catch
      {
      }
    }

    public bool AddTable(DataTable TheTable, string TableName, bool TryParse, bool wrapText = true)
    {
      return this.AddTable(TheTable, TableName, 0, TryParse, wrapText);
    }

    public bool AddTable(
      DataTable TheTable,
      string TableName,
      int TheOffset,
      bool TryParse,
      bool wrapText = true)
    {
      List<int> colIdsToExport = new List<int>();
      for (int index = 0; index < TheTable.Columns.Count; ++index)
        colIdsToExport.Add(index);
      return this.AddTable(TheTable, TableName, TheOffset, colIdsToExport, TryParse, wrapText);
    }

    public bool AddTable(
      DataTable TheTable,
      string TableName,
      List<int> colIdsToExport,
      bool TryParse,
      bool wrapText = true)
    {
      return this.AddTable(TheTable, TableName, 0, colIdsToExport, TryParse, wrapText);
    }

    public bool AddTable(
      DataTable TheTable,
      string TableName,
      int TheOffset,
      List<int> colIdsToExport,
      bool TryParse,
      bool wrapText = true)
    {
      if (this.WorksheetIndex > this.MyWorkbook.Worksheets.Count)
        return false;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (_Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      _Worksheet worksheet = ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__0, this.MyWorkbook.Worksheets[(object) this.WorksheetIndex]);
      worksheet.Name = TableName;
      for (int index = 0; index < colIdsToExport.Count; ++index)
        worksheet.Cells[(object) (1 + TheOffset), (object) (1 + index)] = (object) TheTable.Columns[colIdsToExport[index]].ColumnName.ToString();
      object[,] objArray = new object[TheTable.Rows.Count, colIdsToExport.Count];
      for (int index1 = 0; index1 < TheTable.Rows.Count; ++index1)
      {
        for (int index2 = 0; index2 < colIdsToExport.Count; ++index2)
        {
          if (TryParse)
          {
            string s = TheTable.Rows[index1][colIdsToExport[index2]].ToString();
            double result1;
            DateTime result2;
            objArray[index1, index2] = !double.TryParse(s, NumberStyles.AllowDecimalPoint, (IFormatProvider) FixedFormates.TheFormates, out result1) ? (!DateTime.TryParse(s, (IFormatProvider) FixedFormates.TheFormates, DateTimeStyles.None, out result2) ? (object) s : (object) result2.ToString()) : (object) result1;
          }
          else
            objArray[index1, index2] = TheTable.Rows[index1][colIdsToExport[index2]];
        }
      }
      string str = "A" + (TheOffset + 2).ToString();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      worksheet.get_Range((object) (str + ":" + this.GetExcelColumnName(colIdsToExport.Count) + (TheTable.Rows.Count + 1 + TheOffset).ToString()), Type.Missing).set_Value(Type.Missing, (object) objArray);
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "WrapText", typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool, object> target1 = ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool, object>> p3 = ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object, object> target2 = ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object, object>> p2 = ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__1 = CallSite<Func<CallSite, _Worksheet, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__1.Target((CallSite) ExcelConnect.\u003C\u003Eo__16.\u003C\u003Ep__1, worksheet);
      object cell1 = worksheet.Cells[(object) 1, (object) 1];
      object cell2 = worksheet.Cells[(object) (TheTable.Rows.Count + 1), (object) colIdsToExport.Count];
      object obj2 = target2((CallSite) p2, obj1, cell1, cell2);
      int num = wrapText ? 1 : 0;
      object obj3 = target1((CallSite) p3, obj2, num != 0);
      if (!wrapText)
      {
        // ISSUE: reference to a compiler-generated method
        worksheet.UsedRange.Columns.AutoFit();
      }
      return true;
    }

    public void MarkTable(DataTable TheTable, List<int> TheRowsToMark, Color TheMarkColor)
    {
      this.MarkTable(TheTable, 0, TheRowsToMark, TheMarkColor);
    }

    public void MarkTable(
      DataTable TheTable,
      int TheOffset,
      List<int> TheRowsToMark,
      Color TheMarkColor)
    {
      if (this.WorksheetIndex > this.MyWorkbook.Worksheets.Count)
        throw new ApplicationException("Worksheet index out of range!");
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (_Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      _Worksheet worksheet = ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__0, this.MyWorkbook.Worksheets[(object) this.WorksheetIndex]);
      for (int index = 0; index < TheTable.Rows.Count; ++index)
      {
        if (TheRowsToMark.Contains(index))
        {
          // ISSUE: reference to a compiler-generated field
          if (ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Microsoft.Office.Interop.Excel.Range>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Microsoft.Office.Interop.Excel.Range), typeof (ExcelConnect)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, Microsoft.Office.Interop.Excel.Range> target1 = ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, Microsoft.Office.Interop.Excel.Range>> p3 = ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object, object> target2 = ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object, object>> p2 = ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__2;
          // ISSUE: reference to a compiler-generated field
          if (ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__1 = CallSite<Func<CallSite, _Worksheet, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj1 = ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__1.Target((CallSite) ExcelConnect.\u003C\u003Eo__18.\u003C\u003Ep__1, worksheet);
          object cell1 = worksheet.Cells[(object) (index + 2 + TheOffset), (object) 1];
          object cell2 = worksheet.Cells[(object) (index + 2 + TheOffset), (object) TheTable.Columns.Count];
          object obj2 = target2((CallSite) p2, obj1, cell1, cell2);
          // ISSUE: variable of a compiler-generated type
          Microsoft.Office.Interop.Excel.Range range = target1((CallSite) p3, obj2);
          range.Rows.Cells.Interior.Color = (object) ColorTranslator.ToOle(Color.LightYellow);
        }
      }
    }

    public void AddCaption(string TheCaption)
    {
      if (this.WorksheetIndex > this.MyWorkbook.Worksheets.Count)
        throw new ApplicationException("Worksheet index out of range!");
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__19.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__19.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (_Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      _Worksheet worksheet = ExcelConnect.\u003C\u003Eo__19.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__19.\u003C\u003Ep__0, this.MyWorkbook.Worksheets[(object) this.WorksheetIndex]);
      worksheet.Cells[(object) 1, (object) 1] = (object) TheCaption;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range((object) "A1", (object) "A1");
      range.Font.Size = (object) 16;
      range.Font.Bold = (object) true;
    }

    public void AddGrid(DataTable TheTable, int TheOffset)
    {
      if (this.WorksheetIndex > this.MyWorkbook.Worksheets.Count)
        throw new ApplicationException("Worksheet index out of range!");
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (_Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      _Worksheet worksheet = ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__0, this.MyWorkbook.Worksheets[(object) this.WorksheetIndex]);
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Microsoft.Office.Interop.Excel.Range>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Microsoft.Office.Interop.Excel.Range), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Microsoft.Office.Interop.Excel.Range> target1 = ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Microsoft.Office.Interop.Excel.Range>> p3 = ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object, object> target2 = ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object, object>> p2 = ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__1 = CallSite<Func<CallSite, _Worksheet, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__1.Target((CallSite) ExcelConnect.\u003C\u003Eo__20.\u003C\u003Ep__1, worksheet);
      object cell1 = worksheet.Cells[(object) (1 + TheOffset), (object) 1];
      object cell2 = worksheet.Cells[(object) (TheTable.Rows.Count + 1 + TheOffset), (object) TheTable.Columns.Count];
      object obj2 = target2((CallSite) p2, obj1, cell1, cell2);
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range range = target1((CallSite) p3, obj2);
      foreach (Microsoft.Office.Interop.Excel.Range cell3 in range.Cells)
      {
        // ISSUE: reference to a compiler-generated method
        cell3.BorderAround2(Type.Missing, Color: Type.Missing, ThemeColor: Type.Missing);
      }
    }

    public void SetTableHeadersFontBold(DataTable TheTable, int TheOffset)
    {
      if (this.WorksheetIndex > this.MyWorkbook.Worksheets.Count)
        throw new ApplicationException("Worksheet index out of range!");
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (_Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      _Worksheet worksheet = ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__0, this.MyWorkbook.Worksheets[(object) this.WorksheetIndex]);
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Microsoft.Office.Interop.Excel.Range>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Microsoft.Office.Interop.Excel.Range), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, Microsoft.Office.Interop.Excel.Range> target1 = ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, Microsoft.Office.Interop.Excel.Range>> p3 = ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object, object, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object, object> target2 = ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object, object>> p2 = ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__1 = CallSite<Func<CallSite, _Worksheet, object>>.Create(Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof (ExcelConnect), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__1.Target((CallSite) ExcelConnect.\u003C\u003Eo__21.\u003C\u003Ep__1, worksheet);
      object cell1 = worksheet.Cells[(object) (1 + TheOffset), (object) 1];
      object cell2 = worksheet.Cells[(object) (1 + TheOffset), (object) TheTable.Columns.Count];
      object obj2 = target2((CallSite) p2, obj1, cell1, cell2);
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range range = target1((CallSite) p3, obj2);
      range.Font.Bold = (object) true;
      // ISSUE: reference to a compiler-generated method
      worksheet.UsedRange.Columns.AutoFit();
    }

    private string GetExcelColumnName(int columnNumber)
    {
      int num1 = columnNumber;
      string excelColumnName = string.Empty;
      int num2;
      for (; num1 > 0; num1 = (num1 - num2) / 26)
      {
        num2 = (num1 - 1) % 26;
        excelColumnName = Convert.ToChar(65 + num2).ToString() + excelColumnName;
      }
      return excelColumnName;
    }

    public bool AddQ1Q2Q3FormatedTable(DataTable TheTable, string TableName, string format)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < TheTable.Columns.Count; ++index)
        intList.Add(index);
      if (this.WorksheetIndex > this.MyWorkbook.Worksheets.Count)
        return false;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__23.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__23.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (_Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      _Worksheet worksheet = ExcelConnect.\u003C\u003Eo__23.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__23.\u003C\u003Ep__0, this.MyWorkbook.Worksheets[(object) this.WorksheetIndex]);
      worksheet.Name = TableName;
      for (int index = 0; index < intList.Count; ++index)
        worksheet.Cells[(object) 1, (object) (1 + index)] = (object) TheTable.Columns[intList[index]].ColumnName.ToString();
      object[,] objArray = new object[TheTable.Rows.Count, intList.Count];
      for (int index1 = 0; index1 < TheTable.Rows.Count; ++index1)
      {
        for (int index2 = 0; index2 < intList.Count; ++index2)
          objArray[index1, index2] = TheTable.Rows[index1][intList[index2]];
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range((object) ("A2:" + this.GetExcelColumnName(intList.Count) + (TheTable.Rows.Count + 1).ToString()), Type.Missing);
      // ISSUE: reference to a compiler-generated method
      range.set_Value(Type.Missing, (object) objArray);
      // ISSUE: reference to a compiler-generated method
      worksheet.UsedRange.Columns.AutoFit();
      range.NumberFormat = (object) format;
      return true;
    }

    public bool AddHydraulicFormatedTable(DataTable TheTable)
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < TheTable.Columns.Count; ++index)
        intList.Add(index);
      if (this.WorksheetIndex > this.MyWorkbook.Worksheets.Count)
        return false;
      // ISSUE: reference to a compiler-generated field
      if (ExcelConnect.\u003C\u003Eo__24.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ExcelConnect.\u003C\u003Eo__24.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, _Worksheet>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (_Worksheet), typeof (ExcelConnect)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      _Worksheet worksheet = ExcelConnect.\u003C\u003Eo__24.\u003C\u003Ep__0.Target((CallSite) ExcelConnect.\u003C\u003Eo__24.\u003C\u003Ep__0, this.MyWorkbook.Worksheets[(object) this.WorksheetIndex]);
      worksheet.Name = "Test results";
      for (int index = 0; index < intList.Count; ++index)
        worksheet.Cells[(object) 1, (object) (1 + index)] = (object) TheTable.Columns[intList[index]].ColumnName.ToString();
      object[,] objArray = new object[TheTable.Rows.Count, intList.Count];
      for (int index1 = 0; index1 < TheTable.Rows.Count; ++index1)
      {
        for (int index2 = 0; index2 < intList.Count; ++index2)
          objArray[index1, index2] = TheTable.Rows[index1][intList[index2]];
      }
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range range1 = worksheet.get_Range((object) ("A2:" + this.GetExcelColumnName(intList.Count) + (TheTable.Rows.Count + 1).ToString()), Type.Missing);
      // ISSUE: reference to a compiler-generated method
      range1.set_Value(Type.Missing, (object) objArray);
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      Microsoft.Office.Interop.Excel.Range range2 = worksheet.get_Range((object) ("D3:" + this.GetExcelColumnName(intList.Count - 2) + (TheTable.Rows.Count + 1).ToString()), Type.Missing);
      // ISSUE: reference to a compiler-generated method
      worksheet.UsedRange.Columns.AutoFit();
      range2.NumberFormat = (object) "0.00%";
      return true;
    }

    public bool ShowWorkbook()
    {
      this.MyExcel.Visible = true;
      this.MyExcel.UserControl = true;
      this.MyWorkbook = (_Workbook) null;
      this.MyExcel = (_Application) null;
      this.WorksheetIndex = 1;
      return true;
    }
  }
}
