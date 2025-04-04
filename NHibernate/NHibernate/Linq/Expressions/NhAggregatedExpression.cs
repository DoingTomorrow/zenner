// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Expressions.NhAggregatedExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Expressions
{
  public class NhAggregatedExpression : Expression
  {
    public Expression Expression { get; set; }

    public NhAggregatedExpression(Expression expression, NhExpressionType type)
      : base((ExpressionType) type, expression.Type)
    {
      this.Expression = expression;
    }

    public NhAggregatedExpression(
      Expression expression,
      Type expressionType,
      NhExpressionType type)
      : base((ExpressionType) type, expressionType)
    {
      this.Expression = expression;
    }
  }
}
