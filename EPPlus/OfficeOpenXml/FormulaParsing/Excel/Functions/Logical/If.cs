// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Logical.If
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Logical
{
  public class If : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 3);
      bool flag = this.ArgToBool(arguments, 0);
      object obj1 = arguments.ElementAt<FunctionArgument>(1).Value;
      object obj2 = arguments.ElementAt<FunctionArgument>(2).Value;
      CompileResultFactory compileResultFactory = new CompileResultFactory();
      return !flag ? compileResultFactory.Create(obj2) : compileResultFactory.Create(obj1);
    }
  }
}
