// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.FormulaParserManager
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class FormulaParserManager
  {
    private readonly FormulaParser _parser;

    internal FormulaParserManager(FormulaParser parser)
    {
      Require.That<FormulaParser>(parser).Named(nameof (parser)).IsNotNull<FormulaParser>();
      this._parser = parser;
    }

    public void LoadFunctionModule(IFunctionModule module)
    {
      this._parser.Configure((Action<ParsingConfiguration>) (x => x.FunctionRepository.LoadModule(module)));
    }

    public void AddOrReplaceFunction(string functionName, ExcelFunction functionImpl)
    {
      this._parser.Configure((Action<ParsingConfiguration>) (x => x.FunctionRepository.AddOrReplaceFunction(functionName, functionImpl)));
    }

    public IEnumerable<string> GetImplementedFunctionNames()
    {
      List<string> list = this._parser.FunctionNames.ToList<string>();
      list.Sort((Comparison<string>) ((x, y) => string.Compare(x, y, StringComparison.Ordinal)));
      return (IEnumerable<string>) list;
    }

    public object Parse(string formula) => this._parser.Parse(formula);
  }
}
