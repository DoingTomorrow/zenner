// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.TransformingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using Remotion.Linq.Utilities;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors
{
  public class TransformingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly IExpressionTranformationProvider _tranformationProvider;

    public static Expression Transform(
      Expression expression,
      IExpressionTranformationProvider tranformationProvider)
    {
      ArgumentUtility.CheckNotNull<IExpressionTranformationProvider>(nameof (tranformationProvider), tranformationProvider);
      return new TransformingExpressionTreeVisitor(tranformationProvider).VisitExpression(expression);
    }

    protected TransformingExpressionTreeVisitor(
      IExpressionTranformationProvider tranformationProvider)
    {
      ArgumentUtility.CheckNotNull<IExpressionTranformationProvider>(nameof (tranformationProvider), tranformationProvider);
      this._tranformationProvider = tranformationProvider;
    }

    public override Expression VisitExpression(Expression expression)
    {
      Expression expression1 = base.VisitExpression(expression);
      if (expression1 == null)
        return expression1;
      foreach (ExpressionTransformation transformation in this._tranformationProvider.GetTransformations(expression1))
      {
        Expression expression2 = transformation(expression1);
        Trace.Assert(expression2 != null);
        if (expression2 != expression1)
          return this.VisitExpression(expression2);
      }
      return expression1;
    }
  }
}
