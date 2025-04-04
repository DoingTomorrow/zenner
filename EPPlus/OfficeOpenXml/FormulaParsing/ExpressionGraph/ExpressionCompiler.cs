// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionCompiler
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileStrategy;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class ExpressionCompiler : IExpressionCompiler
  {
    private IEnumerable<Expression> _expressions;
    private IExpressionConverter _expressionConverter;
    private ICompileStrategyFactory _compileStrategyFactory;

    public ExpressionCompiler()
      : this((IExpressionConverter) new ExpressionConverter(), (ICompileStrategyFactory) new CompileStrategyFactory())
    {
    }

    public ExpressionCompiler(
      IExpressionConverter expressionConverter,
      ICompileStrategyFactory compileStrategyFactory)
    {
      this._expressionConverter = expressionConverter;
      this._compileStrategyFactory = compileStrategyFactory;
    }

    public CompileResult Compile(IEnumerable<Expression> expressions)
    {
      this._expressions = expressions;
      return this.PerformCompilation();
    }

    public CompileResult Compile(
      string worksheet,
      int row,
      int column,
      IEnumerable<Expression> expressions)
    {
      this._expressions = expressions;
      return this.PerformCompilation(worksheet, row, column);
    }

    private CompileResult PerformCompilation(string worksheet = "", int row = -1, int column = -1)
    {
      IEnumerable<Expression> source = this.HandleGroupedExpressions();
      while (source.Any<Expression>((Func<Expression, bool>) (x => x.Operator != null)))
        source = this.HandlePrecedenceLevel(this.FindLowestPrecedence());
      return this._expressions.Any<Expression>() ? source.First<Expression>().Compile() : CompileResult.Empty;
    }

    private IEnumerable<Expression> HandleGroupedExpressions()
    {
      if (!this._expressions.Any<Expression>())
        return Enumerable.Empty<Expression>();
      Expression first = this._expressions.First<Expression>();
      foreach (Expression expression1 in this._expressions.Where<Expression>((Func<Expression, bool>) (x => x.IsGroupedExpression)))
      {
        CompileResult compileResult = expression1.Compile();
        if (compileResult != CompileResult.Empty)
        {
          Expression expression2 = this._expressionConverter.FromCompileResult(compileResult);
          expression2.Operator = expression1.Operator;
          expression2.Prev = expression1.Prev;
          expression2.Next = expression1.Next;
          if (expression1.Prev != null)
            expression1.Prev.Next = expression2;
          if (expression1 == first)
            first = expression2;
        }
      }
      return this.RefreshList(first);
    }

    private IEnumerable<Expression> HandlePrecedenceLevel(int precedence)
    {
      Expression first = this._expressions.First<Expression>();
      IEnumerable<Expression> source = this._expressions.Where<Expression>((Func<Expression, bool>) (x => x.Operator != null && x.Operator.Precedence == precedence));
      source.Last<Expression>();
      Expression expression1 = source.First<Expression>();
      do
      {
        Expression expression2 = this._compileStrategyFactory.Create(expression1).Compile();
        if (expression1 == first)
          first = expression2;
        expression1 = expression2;
      }
      while (expression1 != null && expression1.Operator != null && expression1.Operator.Precedence == precedence);
      return this.RefreshList(first);
    }

    private int FindLowestPrecedence()
    {
      return this._expressions.Where<Expression>((Func<Expression, bool>) (x => x.Operator != null)).Min<Expression>((Func<Expression, int>) (x => x.Operator.Precedence));
    }

    private IEnumerable<Expression> RefreshList(Expression first)
    {
      List<Expression> expressionList = new List<Expression>();
      Expression expression = first;
      expressionList.Add(expression);
      for (; expression.Next != null; expression = expression.Next)
        expressionList.Add(expression.Next);
      this._expressions = (IEnumerable<Expression>) expressionList;
      return (IEnumerable<Expression>) expressionList;
    }
  }
}
