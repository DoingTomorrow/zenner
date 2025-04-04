// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelDataProvider
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public abstract class ExcelDataProvider : IDisposable
  {
    public abstract ExcelNamedRangeCollection GetWorksheetNames();

    public abstract ExcelNamedRangeCollection GetWorkbookNameValues();

    public abstract ExcelDataProvider.IRangeInfo GetRange(
      string worksheetName,
      int row,
      int column,
      string address);

    public abstract ExcelDataProvider.INameInfo GetName(string worksheet, string name);

    public abstract IEnumerable<object> GetRangeValues(string address);

    public abstract string GetRangeFormula(string worksheetName, int row, int column);

    public abstract List<Token> GetRangeFormulaTokens(string worksheetName, int row, int column);

    public abstract bool IsRowHidden(string worksheetName, int row);

    public abstract object GetCellValue(string sheetName, int row, int col);

    public abstract void Dispose();

    public abstract int ExcelMaxColumns { get; }

    public abstract int ExcelMaxRows { get; }

    public abstract object GetRangeValue(string worksheetName, int row, int column);

    public abstract string GetFormat(object value, string format);

    public interface IRangeInfo : 
      IEnumerator<ExcelDataProvider.ICellInfo>,
      IDisposable,
      IEnumerator,
      IEnumerable<ExcelDataProvider.ICellInfo>,
      IEnumerable
    {
      bool IsEmpty { get; }

      bool IsMulti { get; }

      int GetNCells();

      ExcelAddressBase Address { get; }

      object GetValue(int row, int col);

      object GetOffset(int rowOffset, int colOffset);
    }

    public interface ICellInfo
    {
      string Address { get; }

      int Row { get; }

      int Column { get; }

      string Formula { get; }

      object Value { get; }

      double ValueDouble { get; }

      double ValueDoubleLogical { get; }

      bool IsHiddenRow { get; }

      bool IsExcelError { get; }

      IList<Token> Tokens { get; }
    }

    public interface INameInfo
    {
      ulong Id { get; set; }

      string Worksheet { get; set; }

      string Name { get; set; }

      string Formula { get; set; }

      IList<Token> Tokens { get; }

      object Value { get; set; }
    }
  }
}
