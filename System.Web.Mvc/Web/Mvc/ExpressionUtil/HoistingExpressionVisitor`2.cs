// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.HoistingExpressionVisitor`2
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal sealed class HoistingExpressionVisitor<TIn, TOut> : ExpressionVisitor
  {
    private static readonly ParameterExpression _hoistedConstantsParamExpr = Expression.Parameter(typeof (List<object>), "hoistedConstants");
    private int _numConstantsProcessed;

    private HoistingExpressionVisitor()
    {
    }

    public static Expression<Hoisted<TIn, TOut>> Hoist(Expression<Func<TIn, TOut>> expr)
    {
      return Expression.Lambda<Hoisted<TIn, TOut>>(new HoistingExpressionVisitor<TIn, TOut>().Visit(expr.Body), expr.Parameters[0], HoistingExpressionVisitor<TIn, TOut>._hoistedConstantsParamExpr);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
      return (Expression) Expression.Convert((Expression) Expression.Property((Expression) HoistingExpressionVisitor<TIn, TOut>._hoistedConstantsParamExpr, "Item", (Expression) Expression.Constant((object) this._numConstantsProcessed++)), node.Type);
    }
  }
}
