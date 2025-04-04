// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.NamedValueExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class NamedValueExpression : AtomicExpression
  {
    private readonly ParsingContext _parsingContext;

    public NamedValueExpression(string expression, ParsingContext parsingContext)
      : base(expression)
    {
      this._parsingContext = parsingContext;
    }

    public override CompileResult Compile()
    {
      ExcelDataProvider.INameInfo name = this._parsingContext.ExcelDataProvider.GetName(this._parsingContext.Scopes.Current.Address.Worksheet, this.ExpressionString);
      if (name == null)
        throw new ExcelErrorValueException(ExcelErrorValue.Create(eErrorType.Name));
      if (name.Value == null)
        return (CompileResult) null;
      if (!(name.Value is ExcelDataProvider.IRangeInfo))
        return new CompileResultFactory().Create(name.Value);
      ExcelDataProvider.IRangeInfo source = (ExcelDataProvider.IRangeInfo) name.Value;
      if (source.IsMulti)
        return new CompileResult(name.Value, DataType.Enumerable);
      return source.IsEmpty ? (CompileResult) null : new CompileResultFactory().Create(source.First<ExcelDataProvider.ICellInfo>().Value);
    }
  }
}
