// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers.FunctionCompilerFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers
{
  public class FunctionCompilerFactory
  {
    public virtual FunctionCompiler Create(ExcelFunction function)
    {
      if (function.IsLookupFuction)
        return (FunctionCompiler) new LookupFunctionCompiler(function);
      if (function.IsErrorHandlingFunction)
        return (FunctionCompiler) new ErrorHandlingFunctionCompiler(function);
      return function is If ? (FunctionCompiler) new IfFunctionCompiler(function) : (FunctionCompiler) new DefaultCompiler(function);
    }
  }
}
