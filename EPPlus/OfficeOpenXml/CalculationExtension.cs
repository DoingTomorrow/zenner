// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.CalculationExtension
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml
{
  public static class CalculationExtension
  {
    public static void Calculate(this ExcelWorkbook workbook)
    {
      workbook.Calculate(new ExcelCalculationOption()
      {
        AllowCirculareReferences = false
      });
    }

    public static void Calculate(this ExcelWorkbook workbook, ExcelCalculationOption options)
    {
      CalculationExtension.Init(workbook);
      DependencyChain dc = DependencyChainFactory.Create(workbook, options);
      FormulaParser formulaParser = workbook.FormulaParser;
      CalculationExtension.CalcChain(workbook, formulaParser, dc);
      workbook._isCalculated = true;
    }

    public static void Calculate(this ExcelWorksheet worksheet)
    {
      worksheet.Calculate(new ExcelCalculationOption());
    }

    public static void Calculate(this ExcelWorksheet worksheet, ExcelCalculationOption options)
    {
      CalculationExtension.Init(worksheet.Workbook);
      FormulaParser formulaParser = worksheet.Workbook.FormulaParser;
      DependencyChain dc = DependencyChainFactory.Create(worksheet, options);
      CalculationExtension.CalcChain(worksheet.Workbook, formulaParser, dc);
    }

    public static void Calculate(this ExcelRangeBase range)
    {
      range.Calculate(new ExcelCalculationOption());
    }

    public static void Calculate(this ExcelRangeBase range, ExcelCalculationOption options)
    {
      CalculationExtension.Init(range._workbook);
      FormulaParser formulaParser = range._workbook.FormulaParser;
      DependencyChain dc = DependencyChainFactory.Create(range, options);
      CalculationExtension.CalcChain(range._workbook, formulaParser, dc);
    }

    public static object Calculate(this ExcelWorksheet worksheet, string Formula)
    {
      return worksheet.Calculate(Formula, new ExcelCalculationOption());
    }

    public static object Calculate(
      this ExcelWorksheet worksheet,
      string Formula,
      ExcelCalculationOption options)
    {
      try
      {
        worksheet.CheckSheetType();
        if (string.IsNullOrEmpty(Formula.Trim()))
          return (object) null;
        CalculationExtension.Init(worksheet.Workbook);
        FormulaParser formulaParser = worksheet.Workbook.FormulaParser;
        if (Formula[0] == '=')
          Formula = Formula.Substring(1);
        DependencyChain dc = DependencyChainFactory.Create(worksheet, Formula, options);
        FormulaCell formulaCell = dc.list[0];
        dc.CalcOrder.RemoveAt(dc.CalcOrder.Count - 1);
        CalculationExtension.CalcChain(worksheet.Workbook, formulaParser, dc);
        return formulaParser.ParseCell((IEnumerable<Token>) formulaCell.Tokens, worksheet.Name, -1, -1);
      }
      catch (Exception ex)
      {
        return (object) new ExcelErrorValueException(ex.Message, ExcelErrorValue.Create(eErrorType.Value));
      }
    }

    private static void CalcChain(ExcelWorkbook wb, FormulaParser parser, DependencyChain dc)
    {
      foreach (int index in dc.CalcOrder)
      {
        FormulaCell formulaCell = dc.list[index];
        try
        {
          ExcelWorksheet bySheetId = wb.Worksheets.GetBySheetID(formulaCell.SheetID);
          object cell = parser.ParseCell((IEnumerable<Token>) formulaCell.Tokens, bySheetId == null ? "" : bySheetId.Name, formulaCell.Row, formulaCell.Column);
          CalculationExtension.SetValue(wb, formulaCell, cell);
        }
        catch (FormatException ex)
        {
          throw ex;
        }
        catch (Exception ex)
        {
          ExcelErrorValue v = ExcelErrorValue.Parse("#VALUE!");
          CalculationExtension.SetValue(wb, formulaCell, (object) v);
        }
      }
    }

    private static void Init(ExcelWorkbook workbook)
    {
      workbook._formulaTokens = new CellStore<List<Token>>();
      foreach (ExcelWorksheet worksheet in workbook.Worksheets)
      {
        if (!(worksheet is ExcelChartsheet))
        {
          if (worksheet._formulaTokens != null)
            worksheet._formulaTokens.Dispose();
          worksheet._formulaTokens = new CellStore<List<Token>>();
        }
      }
    }

    private static void SetValue(ExcelWorkbook workbook, FormulaCell item, object v)
    {
      if (item.Column == 0)
      {
        if (item.SheetID <= 0)
          workbook.Names[item.Row].NameValue = v;
        else
          workbook.Worksheets.GetBySheetID(item.SheetID).Names[item.Row].NameValue = v;
      }
      else
        workbook.Worksheets.GetBySheetID(item.SheetID)._values.SetValue(item.Row, item.Column, v);
    }
  }
}
