// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraphBuilder
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Operators;
using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class ExpressionGraphBuilder : IExpressionGraphBuilder
  {
    private readonly OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraph _graph = new OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraph();
    private readonly IExpressionFactory _expressionFactory;
    private readonly ParsingContext _parsingContext;
    private int _tokenIndex;
    private bool _negateNextExpression;

    public ExpressionGraphBuilder(
      ExcelDataProvider excelDataProvider,
      ParsingContext parsingContext)
      : this((IExpressionFactory) new ExpressionFactory(excelDataProvider, parsingContext), parsingContext)
    {
    }

    public ExpressionGraphBuilder(
      IExpressionFactory expressionFactory,
      ParsingContext parsingContext)
    {
      this._expressionFactory = expressionFactory;
      this._parsingContext = parsingContext;
    }

    public OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraph Build(
      IEnumerable<Token> tokens)
    {
      this._tokenIndex = 0;
      this._graph.Reset();
      this.BuildUp(tokens != null ? tokens.ToArray<Token>() : new Token[0], (Expression) null);
      return this._graph;
    }

    private void BuildUp(Token[] tokens, Expression parent)
    {
      for (; this._tokenIndex < tokens.Length; ++this._tokenIndex)
      {
        Token token = tokens[this._tokenIndex];
        IOperator op = (IOperator) null;
        if (token.TokenType == TokenType.Operator && OperatorsDict.Instance.TryGetValue(token.Value, out op))
          this.SetOperatorOnExpression(parent, op);
        else if (token.TokenType == TokenType.Function)
          this.BuildFunctionExpression(tokens, parent, token.Value);
        else if (token.TokenType == TokenType.OpeningEnumerable)
        {
          ++this._tokenIndex;
          this.BuildEnumerableExpression(tokens, parent);
        }
        else if (token.TokenType == TokenType.OpeningParenthesis)
        {
          ++this._tokenIndex;
          this.BuildGroupExpression(tokens, parent);
        }
        else
        {
          if (token.TokenType == TokenType.ClosingParenthesis || token.TokenType == TokenType.ClosingEnumerable)
            break;
          if (token.TokenType == TokenType.Negator)
            this._negateNextExpression = true;
          else if (token.TokenType == TokenType.Percent)
          {
            this.SetOperatorOnExpression(parent, Operator.Percent);
            if (parent == null)
              this._graph.Add(ConstantExpressions.Percent);
            else
              parent.AddChild(ConstantExpressions.Percent);
          }
          else
            this.CreateAndAppendExpression(ref parent, token);
        }
      }
    }

    private void BuildEnumerableExpression(Token[] tokens, Expression parent)
    {
      if (parent == null)
      {
        this._graph.Add((Expression) new EnumerableExpression());
        this.BuildUp(tokens, this._graph.Current);
      }
      else
      {
        EnumerableExpression enumerableExpression = new EnumerableExpression();
        parent.AddChild((Expression) enumerableExpression);
        this.BuildUp(tokens, (Expression) enumerableExpression);
      }
    }

    private void CreateAndAppendExpression(ref Expression parent, Token token)
    {
      if (this.IsWaste(token))
        return;
      if (parent != null && (token.TokenType == TokenType.Comma || token.TokenType == TokenType.SemiColon))
      {
        parent = parent.PrepareForNextChild();
      }
      else
      {
        if (this._negateNextExpression)
        {
          token.Negate();
          this._negateNextExpression = false;
        }
        Expression expression = this._expressionFactory.Create(token);
        if (parent == null)
          this._graph.Add(expression);
        else
          parent.AddChild(expression);
      }
    }

    private bool IsWaste(Token token) => token.TokenType == TokenType.String;

    private void BuildFunctionExpression(Token[] tokens, Expression parent, string funcName)
    {
      if (parent == null)
      {
        this._graph.Add((Expression) new FunctionExpression(funcName, this._parsingContext, this._negateNextExpression));
        this._negateNextExpression = false;
        this.HandleFunctionArguments(tokens, this._graph.Current);
      }
      else
      {
        FunctionExpression functionExpression = new FunctionExpression(funcName, this._parsingContext, this._negateNextExpression);
        this._negateNextExpression = false;
        parent.AddChild((Expression) functionExpression);
        this.HandleFunctionArguments(tokens, (Expression) functionExpression);
      }
    }

    private void HandleFunctionArguments(Token[] tokens, Expression function)
    {
      ++this._tokenIndex;
      if (((IEnumerable<Token>) tokens).ElementAt<Token>(this._tokenIndex).TokenType != TokenType.OpeningParenthesis)
        throw new ExcelErrorValueException(eErrorType.Value);
      ++this._tokenIndex;
      this.BuildUp(tokens, function.Children.First<Expression>());
    }

    private void BuildGroupExpression(Token[] tokens, Expression parent)
    {
      if (parent == null)
      {
        this._graph.Add((Expression) new GroupExpression());
        this.BuildUp(tokens, this._graph.Current);
      }
      else
      {
        if (parent.IsGroupedExpression || parent is FunctionArgumentExpression)
        {
          GroupExpression groupExpression = new GroupExpression();
          parent.AddChild((Expression) groupExpression);
          this.BuildUp(tokens, (Expression) groupExpression);
        }
        this.BuildUp(tokens, parent);
      }
    }

    private void SetOperatorOnExpression(Expression parent, IOperator op)
    {
      if (parent == null)
      {
        this._graph.Current.Operator = op;
      }
      else
      {
        Expression expression;
        if (parent is FunctionArgumentExpression)
        {
          expression = parent.Children.Last<Expression>();
        }
        else
        {
          expression = parent.Children.Last<Expression>();
          if (expression is FunctionArgumentExpression)
            expression = expression.Children.Last<Expression>();
        }
        expression.Operator = op;
      }
    }
  }
}
