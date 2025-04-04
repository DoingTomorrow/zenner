// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.SelectExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

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
  public class SelectExpressionNode : MethodCallExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).Select<object, object>((Expression<Func<object, object>>) (o => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).Select<object, object>((Func<object, object>) (o => (object) null))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedSelector;

    public SelectExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression selector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (selector), selector);
      if (selector.Parameters.Count != 1)
        throw new ArgumentException("Selector must have exactly one parameter.", nameof (selector));
      this.Selector = selector;
      this._cachedSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression Selector { get; private set; }

    public Expression GetResolvedSelector(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.Selector.Body, this.Selector.Parameters[0], clauseGenerationContext)));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      Expression resolvedSelector = this.GetResolvedSelector(clauseGenerationContext);
      return ReplacingExpressionTreeVisitor.Replace((Expression) inputParameter, resolvedSelector, expressionToBeResolved);
    }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel.SelectClause.Selector = this.GetResolvedSelector(clauseGenerationContext);
      return queryModel;
    }
  }
}
