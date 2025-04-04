// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.SubQueryFindingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.Structure;
using Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors;
using Remotion.Linq.Utilities;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors
{
  public class SubQueryFindingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly INodeTypeProvider _nodeTypeProvider;
    private readonly ExpressionTreeParser _expressionTreeParser;
    private readonly QueryParser _queryParser;

    public static Expression Process(Expression expressionTree, INodeTypeProvider nodeTypeProvider)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      return new SubQueryFindingExpressionTreeVisitor(nodeTypeProvider).VisitExpression(expressionTree);
    }

    private SubQueryFindingExpressionTreeVisitor(INodeTypeProvider nodeTypeProvider)
    {
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      this._nodeTypeProvider = nodeTypeProvider;
      this._expressionTreeParser = new ExpressionTreeParser(this._nodeTypeProvider, (IExpressionTreeProcessor) new NullExpressionTreeProcessor());
      this._queryParser = new QueryParser(this._expressionTreeParser);
    }

    public override Expression VisitExpression(Expression expression)
    {
      MethodCallExpression operatorExpression = this._expressionTreeParser.GetQueryOperatorExpression(expression);
      return operatorExpression != null && this._nodeTypeProvider.IsRegistered(operatorExpression.Method) ? (Expression) this.CreateSubQueryNode(operatorExpression) : base.VisitExpression(expression);
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression;
    }

    private SubQueryExpression CreateSubQueryNode(MethodCallExpression methodCallExpression)
    {
      return new SubQueryExpression(this._queryParser.GetParsedQuery((Expression) methodCallExpression));
    }
  }
}
