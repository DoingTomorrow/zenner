// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.DependencyChainFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  internal static class DependencyChainFactory
  {
    internal static DependencyChain Create(ExcelWorkbook wb, ExcelCalculationOption options)
    {
      DependencyChain depChain = new DependencyChain();
      foreach (ExcelWorksheet worksheet in wb.Worksheets)
      {
        if (!(worksheet is ExcelChartsheet))
        {
          DependencyChainFactory.GetChain(depChain, wb.FormulaParser.Lexer, (ExcelRangeBase) worksheet.Cells, options);
          DependencyChainFactory.GetWorksheetNames(worksheet, depChain, options);
        }
      }
      foreach (ExcelNamedRange name in wb.Names)
      {
        if (name.NameValue == null)
          DependencyChainFactory.GetChain(depChain, wb.FormulaParser.Lexer, name, options);
      }
      return depChain;
    }

    internal static DependencyChain Create(ExcelWorksheet ws, ExcelCalculationOption options)
    {
      ws.CheckSheetType();
      DependencyChain depChain = new DependencyChain();
      DependencyChainFactory.GetChain(depChain, ws.Workbook.FormulaParser.Lexer, (ExcelRangeBase) ws.Cells, options);
      DependencyChainFactory.GetWorksheetNames(ws, depChain, options);
      return depChain;
    }

    internal static DependencyChain Create(
      ExcelWorksheet ws,
      string Formula,
      ExcelCalculationOption options)
    {
      ws.CheckSheetType();
      DependencyChain depChain = new DependencyChain();
      DependencyChainFactory.GetChain(depChain, ws.Workbook.FormulaParser.Lexer, ws, Formula, options);
      return depChain;
    }

    private static void GetWorksheetNames(
      ExcelWorksheet ws,
      DependencyChain depChain,
      ExcelCalculationOption options)
    {
      foreach (ExcelNamedRange name in ws.Names)
      {
        if (!string.IsNullOrEmpty(name.NameFormula))
          DependencyChainFactory.GetChain(depChain, ws.Workbook.FormulaParser.Lexer, name, options);
      }
    }

    internal static DependencyChain Create(ExcelRangeBase range, ExcelCalculationOption options)
    {
      DependencyChain depChain = new DependencyChain();
      DependencyChainFactory.GetChain(depChain, range.Worksheet.Workbook.FormulaParser.Lexer, range, options);
      return depChain;
    }

    private static void GetChain(
      DependencyChain depChain,
      ILexer lexer,
      ExcelNamedRange name,
      ExcelCalculationOption options)
    {
      ExcelWorksheet worksheet = name.Worksheet;
      ulong cellId = ExcelCellBase.GetCellID(worksheet == null ? 0 : worksheet.SheetID, name.Index, 0);
      if (depChain.index.ContainsKey(cellId))
        return;
      FormulaCell f = new FormulaCell()
      {
        SheetID = worksheet == null ? 0 : worksheet.SheetID,
        Row = name.Index,
        Column = 0,
        Formula = name.NameFormula
      };
      if (string.IsNullOrEmpty(f.Formula))
        return;
      f.Tokens = lexer.Tokenize(f.Formula).ToList<Token>();
      if (worksheet == null)
        name._workbook._formulaTokens.SetValue(name.Index, 0, f.Tokens);
      else
        worksheet._formulaTokens.SetValue(name.Index, 0, f.Tokens);
      depChain.Add(f);
      DependencyChainFactory.FollowChain(depChain, lexer, name._workbook, worksheet, f, options);
    }

    private static void GetChain(
      DependencyChain depChain,
      ILexer lexer,
      ExcelWorksheet ws,
      string formula,
      ExcelCalculationOption options)
    {
      FormulaCell f = new FormulaCell()
      {
        SheetID = ws.SheetID,
        Row = -1,
        Column = -1
      };
      f.Formula = formula;
      if (string.IsNullOrEmpty(f.Formula))
        return;
      f.Tokens = lexer.Tokenize(f.Formula).ToList<Token>();
      depChain.Add(f);
      DependencyChainFactory.FollowChain(depChain, lexer, ws.Workbook, ws, f, options);
    }

    private static void GetChain(
      DependencyChain depChain,
      ILexer lexer,
      ExcelRangeBase Range,
      ExcelCalculationOption options)
    {
      ExcelWorksheet worksheet = Range.Worksheet;
      CellsStoreEnumerator<object> cellsStoreEnumerator = new CellsStoreEnumerator<object>(worksheet._formulas, Range.Start.Row, Range.Start.Column, Range.End.Row, Range.End.Column);
      while (cellsStoreEnumerator.Next())
      {
        if (cellsStoreEnumerator.Value != null && !(cellsStoreEnumerator.Value.ToString().Trim() == ""))
        {
          ulong cellId = ExcelCellBase.GetCellID(worksheet.SheetID, cellsStoreEnumerator.Row, cellsStoreEnumerator.Column);
          if (!depChain.index.ContainsKey(cellId))
          {
            FormulaCell f = new FormulaCell()
            {
              SheetID = worksheet.SheetID,
              Row = cellsStoreEnumerator.Row,
              Column = cellsStoreEnumerator.Column
            };
            f.Formula = !(cellsStoreEnumerator.Value is int) ? cellsStoreEnumerator.Value.ToString() : worksheet._sharedFormulas[(int) cellsStoreEnumerator.Value].GetFormula(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column);
            if (!string.IsNullOrEmpty(f.Formula))
            {
              f.Tokens = lexer.Tokenize(f.Formula).ToList<Token>();
              worksheet._formulaTokens.SetValue(cellsStoreEnumerator.Row, cellsStoreEnumerator.Column, f.Tokens);
              depChain.Add(f);
              DependencyChainFactory.FollowChain(depChain, lexer, worksheet.Workbook, worksheet, f, options);
            }
          }
        }
      }
    }

    private static void FollowChain(
      DependencyChain depChain,
      ILexer lexer,
      ExcelWorkbook wb,
      ExcelWorksheet ws,
      FormulaCell f,
      ExcelCalculationOption options)
    {
      Stack<FormulaCell> formulaCellStack = new Stack<FormulaCell>();
label_38:
      while (true)
      {
        while (f.tokenIx < f.Tokens.Count)
        {
          Token token = f.Tokens[f.tokenIx];
          if (token.TokenType == TokenType.ExcelAddress)
          {
            ExcelFormulaAddress excelFormulaAddress = new ExcelFormulaAddress(token.Value);
            if (excelFormulaAddress.Table != null)
              excelFormulaAddress.SetRCFromTable(ws._package, new ExcelAddressBase(f.Row, f.Column, f.Row, f.Column));
            if (excelFormulaAddress.WorkSheet == null && excelFormulaAddress.Collide(new ExcelAddressBase(f.Row, f.Column, f.Row, f.Column)) != ExcelAddressBase.eAddressCollition.No)
              throw new CircularReferenceException(string.Format("Circular Reference in cell {0}", (object) ExcelCellBase.GetAddress(f.Row, f.Column)));
            if (excelFormulaAddress._fromRow > 0 && excelFormulaAddress._fromCol > 0)
            {
              if (string.IsNullOrEmpty(excelFormulaAddress.WorkSheet))
              {
                if (f.ws == null)
                  f.ws = ws;
                else if (f.ws.SheetID != f.SheetID)
                  f.ws = wb.Worksheets.GetBySheetID(f.SheetID);
              }
              else
                f.ws = wb.Worksheets[excelFormulaAddress.WorkSheet];
              if (f.ws != null)
              {
                f.iterator = new CellsStoreEnumerator<object>(f.ws._formulas, excelFormulaAddress.Start.Row, excelFormulaAddress.Start.Column, excelFormulaAddress.End.Row, excelFormulaAddress.End.Column);
                goto label_54;
              }
            }
          }
          else if (token.TokenType == TokenType.NameValue)
          {
            string ws1;
            string address;
            ExcelAddressBase.SplitAddress(token.Value, out string _, out ws1, out address, f.ws == null ? "" : f.ws.Name);
            ExcelNamedRange excelNamedRange;
            if (!string.IsNullOrEmpty(ws1))
            {
              if (f.ws == null)
                f.ws = wb.Worksheets[ws1];
              excelNamedRange = !f.ws.Names.ContainsKey(token.Value) ? (!wb.Names.ContainsKey(address) ? (ExcelNamedRange) null : wb.Names[address]) : f.ws.Names[address];
              if (excelNamedRange != null)
                f.ws = excelNamedRange.Worksheet;
            }
            else if (wb.Names.ContainsKey(address))
            {
              excelNamedRange = wb.Names[token.Value];
              if (string.IsNullOrEmpty(ws1))
                f.ws = excelNamedRange.Worksheet;
            }
            else
              excelNamedRange = (ExcelNamedRange) null;
            if (excelNamedRange != null)
            {
              if (string.IsNullOrEmpty(excelNamedRange.NameFormula))
              {
                f.iterator = new CellsStoreEnumerator<object>(f.ws._formulas, excelNamedRange.Start.Row, excelNamedRange.Start.Column, excelNamedRange.End.Row, excelNamedRange.End.Column);
                goto label_54;
              }
              else
              {
                ulong cellId = ExcelCellBase.GetCellID(excelNamedRange.LocalSheetId, excelNamedRange.Index, 0);
                if (!depChain.index.ContainsKey(cellId))
                {
                  FormulaCell f1 = new FormulaCell()
                  {
                    SheetID = excelNamedRange.LocalSheetId,
                    Row = excelNamedRange.Index,
                    Column = 0
                  };
                  f1.Formula = excelNamedRange.NameFormula;
                  f1.Tokens = lexer.Tokenize(f1.Formula).ToList<Token>();
                  depChain.Add(f1);
                  formulaCellStack.Push(f);
                  f = f1;
                  continue;
                }
                if (formulaCellStack.Count > 0)
                {
                  foreach (FormulaCell formulaCell in formulaCellStack)
                  {
                    if ((long) ExcelCellBase.GetCellID(formulaCell.SheetID, formulaCell.Row, formulaCell.Column) == (long) cellId)
                      throw new CircularReferenceException(string.Format("Circular Reference in name {0}", (object) excelNamedRange.Name));
                  }
                }
              }
            }
          }
          ++f.tokenIx;
        }
        depChain.CalcOrder.Add(f.Index);
        if (formulaCellStack.Count > 0)
          f = formulaCellStack.Pop();
        else
          break;
label_54:
        while (f.iterator.Next())
        {
          object key = f.iterator.Value;
          if (key != null && !(key.ToString().Trim() == ""))
          {
            ulong cellId = ExcelCellBase.GetCellID(f.ws.SheetID, f.iterator.Row, f.iterator.Column);
            if (!depChain.index.ContainsKey(cellId))
            {
              FormulaCell f2 = new FormulaCell()
              {
                SheetID = f.ws.SheetID,
                Row = f.iterator.Row,
                Column = f.iterator.Column
              };
              f2.Formula = !(f.iterator.Value is int) ? key.ToString() : f.ws._sharedFormulas[(int) key].GetFormula(f.iterator.Row, f.iterator.Column);
              f2.ws = f.ws;
              f2.Tokens = lexer.Tokenize(f2.Formula).ToList<Token>();
              ws._formulaTokens.SetValue(f2.Row, f2.Column, f2.Tokens);
              depChain.Add(f2);
              formulaCellStack.Push(f);
              f = f2;
              goto label_38;
            }
            else if (formulaCellStack.Count > 0)
            {
              foreach (FormulaCell formulaCell in formulaCellStack)
              {
                if ((long) ExcelCellBase.GetCellID(formulaCell.ws.SheetID, formulaCell.iterator.Row, formulaCell.iterator.Column) == (long) cellId)
                {
                  if (!options.AllowCirculareReferences)
                    throw new CircularReferenceException(string.Format("Circular Reference in cell {0}!{1}", (object) formulaCell.ws.Name, (object) ExcelCellBase.GetAddress(f.Row, f.Column)));
                  f = formulaCellStack.Pop();
                  break;
                }
              }
            }
          }
        }
        ++f.tokenIx;
      }
    }
  }
}
