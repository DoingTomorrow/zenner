// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class FunctionExpression : AtomicExpression
  {
    private readonly ParsingContext _parsingContext;
    private readonly FunctionCompilerFactory _functionCompilerFactory = new FunctionCompilerFactory();
    private readonly bool _isNegated;

    public FunctionExpression(string expression, ParsingContext parsingContext, bool isNegated)
      : base(expression)
    {
      this._parsingContext = parsingContext;
      this._isNegated = isNegated;
      base.AddChild((Expression) new FunctionArgumentExpression((Expression) this));
    }

    public override CompileResult Compile()
    {
      try
      {
        CompileResult compileResult = this._functionCompilerFactory.Create(this._parsingContext.Configuration.FunctionRepository.GetFunction(this.ExpressionString)).Compile(this.HasChildren ? this.Children : Enumerable.Empty<Expression>(), this._parsingContext);
        if (!this._isNegated)
          return compileResult;
        if (!compileResult.IsNumeric)
          throw new ExcelErrorValueException(eErrorType.Value);
        return new CompileResult((object) (compileResult.ResultNumeric * -1.0), compileResult.DataType);
      }
      catch (ExcelErrorValueException ex)
      {
        return new CompileResult((object) ex.ErrorValue, DataType.ExcelError);
      }
    }

    public override Expression PrepareForNextChild()
    {
      return base.AddChild((Expression) new FunctionArgumentExpression((Expression) this));
    }

    public override bool HasChildren
    {
      get
      {
        return this.Children.Any<Expression>() && this.Children.First<Expression>().Children.Any<Expression>();
      }
    }

    public override Expression AddChild(Expression child)
    {
      this.Children.Last<Expression>().AddChild(child);
      return child;
    }
  }
}
