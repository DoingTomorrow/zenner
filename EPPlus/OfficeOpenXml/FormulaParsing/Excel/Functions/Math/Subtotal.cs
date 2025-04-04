// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Subtotal
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Subtotal : ExcelFunction
  {
    private Dictionary<int, HiddenValuesHandlingFunction> _functions = new Dictionary<int, HiddenValuesHandlingFunction>();

    public Subtotal() => this.Initialize();

    private void Initialize()
    {
      this._functions[1] = (HiddenValuesHandlingFunction) new Average();
      this._functions[2] = (HiddenValuesHandlingFunction) new Count();
      this._functions[3] = (HiddenValuesHandlingFunction) new CountA();
      this._functions[4] = (HiddenValuesHandlingFunction) new Max();
      this._functions[5] = (HiddenValuesHandlingFunction) new Min();
      this._functions[6] = (HiddenValuesHandlingFunction) new Product();
      this._functions[7] = (HiddenValuesHandlingFunction) new Stdev();
      this._functions[8] = (HiddenValuesHandlingFunction) new StdevP();
      this._functions[9] = (HiddenValuesHandlingFunction) new Sum();
      this._functions[10] = (HiddenValuesHandlingFunction) new Var();
      this._functions[11] = (HiddenValuesHandlingFunction) new VarP();
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Average(), 101);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Count(), 102);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new CountA(), 103);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Max(), 104);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Min(), 105);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Product(), 106);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Stdev(), 107);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new StdevP(), 108);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Sum(), 109);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new Var(), 110);
      this.AddHiddenValueHandlingFunction((HiddenValuesHandlingFunction) new VarP(), 111);
    }

    private void AddHiddenValueHandlingFunction(HiddenValuesHandlingFunction func, int funcNum)
    {
      func.IgnoreHiddenValues = true;
      this._functions[funcNum] = func;
    }

    public override void BeforeInvoke(ParsingContext context)
    {
      base.BeforeInvoke(context);
      context.Scopes.Current.IsSubtotal = true;
    }

    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      int funcNum = this.ArgToInt(arguments, 0);
      if (context.Scopes.Current.Parent != null && context.Scopes.Current.Parent.IsSubtotal)
        return this.CreateResult((object) 0.0, DataType.Decimal);
      IEnumerable<FunctionArgument> arguments1 = arguments.Skip<FunctionArgument>(1);
      CompileResult compileResult = this.GetFunctionByCalcType(funcNum).Execute(arguments1, context);
      compileResult.IsResultOfSubtotal = true;
      return compileResult;
    }

    private ExcelFunction GetFunctionByCalcType(int funcNum)
    {
      if (!this._functions.ContainsKey(funcNum))
        this.ThrowExcelErrorValueException(eErrorType.Value);
      return (ExcelFunction) this._functions[funcNum];
    }
  }
}
