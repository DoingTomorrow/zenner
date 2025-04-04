// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.FormulaCell
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  internal class FormulaCell
  {
    internal int tokenIx;
    internal int addressIx;
    internal CellsStoreEnumerator<object> iterator;
    internal ExcelWorksheet ws;

    internal int Index { get; set; }

    internal int SheetID { get; set; }

    internal int Row { get; set; }

    internal int Column { get; set; }

    internal string Formula { get; set; }

    internal List<Token> Tokens { get; set; }
  }
}
