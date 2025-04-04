// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionTreeVisitors.CloningExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ExpressionTreeVisitors
{
  public class CloningExpressionTreeVisitor : ReferenceReplacingExpressionTreeVisitor
  {
    public static Expression AdjustExpressionAfterCloning(
      Expression expression,
      QuerySourceMapping querySourceMapping)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      return new CloningExpressionTreeVisitor(querySourceMapping, false).VisitExpression(expression);
    }

    private CloningExpressionTreeVisitor(
      QuerySourceMapping querySourceMapping,
      bool ignoreUnmappedReferences)
      : base(querySourceMapping, ignoreUnmappedReferences)
    {
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      return (Expression) new SubQueryExpression(expression.QueryModel.Clone(this.QuerySourceMapping));
    }
  }
}
