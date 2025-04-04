// Decompiled with JetBrains decompiler
// Type: ExpressionSerialization.Evaluator
// Assembly: ExpressionSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D52D7A-23AF-4AE6-9DD2-C2DCB4AD474C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ExpressionSerialization.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace ExpressionSerialization
{
  public static class Evaluator
  {
    public static Expression PartialEval(
      Expression expression,
      Func<Expression, bool> fnCanBeEvaluated)
    {
      return new Evaluator.SubtreeEvaluator(new Evaluator.Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
    }

    public static Expression PartialEval(Expression expression)
    {
      return Evaluator.PartialEval(expression, new Func<Expression, bool>(Evaluator.CanBeEvaluatedLocally));
    }

    private static bool CanBeEvaluatedLocally(Expression expression)
    {
      return expression.NodeType != ExpressionType.Parameter;
    }

    private class SubtreeEvaluator : ExpressionVisitor
    {
      private HashSet<Expression> candidates;

      internal SubtreeEvaluator(HashSet<Expression> candidates) => this.candidates = candidates;

      internal Expression Eval(Expression exp) => this.Visit(exp);

      public override Expression Visit(Expression exp)
      {
        if (exp == null)
          return (Expression) null;
        return this.candidates.Contains(exp) ? this.Evaluate(exp) : base.Visit(exp);
      }

      private Expression Evaluate(Expression e)
      {
        switch (e.NodeType)
        {
          case ExpressionType.Constant:
            return e;
          case ExpressionType.Lambda:
            return e;
          default:
            return (Expression) Expression.Constant(Expression.Lambda(e).Compile().DynamicInvoke((object[]) null), e.Type);
        }
      }
    }

    private class Nominator : ExpressionVisitor
    {
      private Func<Expression, bool> fnCanBeEvaluated;
      private HashSet<Expression> candidates;
      private bool cannotBeEvaluated;

      internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
      {
        this.fnCanBeEvaluated = fnCanBeEvaluated;
      }

      internal HashSet<Expression> Nominate(Expression expression)
      {
        this.candidates = new HashSet<Expression>();
        this.Visit(expression);
        return this.candidates;
      }

      public override Expression Visit(Expression expression)
      {
        if (expression != null)
        {
          bool cannotBeEvaluated = this.cannotBeEvaluated;
          this.cannotBeEvaluated = false;
          base.Visit(expression);
          if (!this.cannotBeEvaluated)
          {
            if (this.fnCanBeEvaluated(expression))
              this.candidates.Add(expression);
            else
              this.cannotBeEvaluated = true;
          }
          this.cannotBeEvaluated |= cannotBeEvaluated;
        }
        return expression;
      }
    }
  }
}
