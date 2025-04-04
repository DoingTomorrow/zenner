// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionTreeVisitors.FormattingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ExpressionTreeVisitors
{
  public class FormattingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    public static string Format(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return new FormattingExpressionTreeVisitor().VisitExpression(expression).ToString();
    }

    private FormattingExpressionTreeVisitor()
    {
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      ArgumentUtility.CheckNotNull<QuerySourceReferenceExpression>(nameof (expression), expression);
      return (Expression) Expression.Parameter(expression.Type, "[" + expression.ReferencedQuerySource.ItemName + "]");
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      ArgumentUtility.CheckNotNull<SubQueryExpression>(nameof (expression), expression);
      return (Expression) Expression.Parameter(expression.Type, "{" + (object) expression.QueryModel + "}");
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      return (Expression) Expression.Parameter(expression.Type, expression.ToString());
    }

    protected internal override Expression VisitExtensionExpression(ExtensionExpression expression)
    {
      ArgumentUtility.CheckNotNull<ExtensionExpression>(nameof (expression), expression);
      return (Expression) Expression.Parameter(expression.Type, expression.ToString());
    }
  }
}
