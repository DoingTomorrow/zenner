// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class ExpressionFactory : IExpressionFactory
  {
    private readonly ExcelDataProvider _excelDataProvider;
    private readonly ParsingContext _parsingContext;

    public ExpressionFactory(ExcelDataProvider excelDataProvider, ParsingContext context)
    {
      this._excelDataProvider = excelDataProvider;
      this._parsingContext = context;
    }

    public Expression Create(Token token)
    {
      switch (token.TokenType)
      {
        case TokenType.String:
          return (Expression) new StringExpression(token.Value);
        case TokenType.Integer:
          return (Expression) new IntegerExpression(token.Value, token.IsNegated);
        case TokenType.Boolean:
          return (Expression) new BooleanExpression(token.Value);
        case TokenType.Decimal:
          return (Expression) new DecimalExpression(token.Value, token.IsNegated);
        case TokenType.ExcelAddress:
          return (Expression) new ExcelAddressExpression(token.Value, this._excelDataProvider, this._parsingContext, token.IsNegated);
        case TokenType.NameValue:
          return (Expression) new NamedValueExpression(token.Value, this._parsingContext);
        case TokenType.InvalidReference:
          return (Expression) new ExcelErrorExpression(token.Value, ExcelErrorValue.Create(eErrorType.Ref));
        case TokenType.NumericError:
          return (Expression) new ExcelErrorExpression(token.Value, ExcelErrorValue.Create(eErrorType.Num));
        case TokenType.ValueDataTypeError:
          return (Expression) new ExcelErrorExpression(token.Value, ExcelErrorValue.Create(eErrorType.Value));
        case TokenType.Null:
          return (Expression) new ExcelErrorExpression(token.Value, ExcelErrorValue.Create(eErrorType.Null));
        default:
          return (Expression) new StringExpression(token.Value);
      }
    }
  }
}
