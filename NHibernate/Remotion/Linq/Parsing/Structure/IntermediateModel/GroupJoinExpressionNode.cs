// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.GroupJoinExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class GroupJoinExpressionNode : 
    MethodCallExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).GroupJoin<object, object, object, object>(default (IEnumerable<object>), (Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, IEnumerable<object>, object>>) ((o, i) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).GroupJoin<object, object, object, object>(default (IEnumerable<object>), (Func<object, object>) (o => (object) null), (Func<object, object>) (o => (object) null), (Func<object, IEnumerable<object>, object>) ((o, i) => (object) null))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedResultSelector;

    public GroupJoinExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      Expression innerSequence,
      LambdaExpression outerKeySelector,
      LambdaExpression innerKeySelector,
      LambdaExpression resultSelector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (innerSequence), innerSequence);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (outerKeySelector), outerKeySelector);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (innerKeySelector), innerKeySelector);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (resultSelector), resultSelector);
      if (outerKeySelector.Parameters.Count != 1)
        throw new ArgumentException("Outer key selector must have exactly one parameter.", nameof (outerKeySelector));
      if (innerKeySelector.Parameters.Count != 1)
        throw new ArgumentException("Inner key selector must have exactly one parameter.", nameof (innerKeySelector));
      if (resultSelector.Parameters.Count != 2)
        throw new ArgumentException("Result selector must have exactly two parameters.", nameof (resultSelector));
      LambdaExpression resultSelector1 = Expression.Lambda((Expression) Expression.Constant((object) null), outerKeySelector.Parameters[0], innerKeySelector.Parameters[0]);
      this.JoinExpressionNode = new JoinExpressionNode(parseInfo, innerSequence, outerKeySelector, innerKeySelector, resultSelector1);
      this.ResultSelector = resultSelector;
      this._cachedResultSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public JoinExpressionNode JoinExpressionNode { get; private set; }

    public LambdaExpression ResultSelector { get; private set; }

    public Expression GetResolvedResultSelector(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedResultSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, this.ResultSelector.Parameters[1], this.ResultSelector.Body, clauseGenerationContext), this.ResultSelector.Parameters[0], clauseGenerationContext)));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      ArgumentUtility.CheckNotNull<ClauseGenerationContext>(nameof (clauseGenerationContext), clauseGenerationContext);
      Expression resolvedResultSelector = this.GetResolvedResultSelector(clauseGenerationContext);
      return ReplacingExpressionTreeVisitor.Replace((Expression) inputParameter, resolvedResultSelector, expressionToBeResolved);
    }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      GroupJoinClause contextInfo = new GroupJoinClause(this.ResultSelector.Parameters[1].Name, this.ResultSelector.Parameters[1].Type, this.JoinExpressionNode.CreateJoinClause(clauseGenerationContext));
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) contextInfo);
      queryModel.BodyClauses.Add((IBodyClause) contextInfo);
      queryModel.SelectClause.Selector = this.GetResolvedResultSelector(clauseGenerationContext);
      return queryModel;
    }
  }
}
