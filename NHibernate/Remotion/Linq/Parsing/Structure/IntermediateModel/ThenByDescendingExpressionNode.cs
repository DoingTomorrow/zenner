// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ThenByDescendingExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class ThenByDescendingExpressionNode : MethodCallExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IOrderedQueryable<object>>((Expression<Func<IOrderedQueryable<object>>>) (() => default (IOrderedQueryable<object>).ThenByDescending<object, object>(default (Expression<Func<object, object>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IOrderedEnumerable<object>>((Expression<Func<IOrderedEnumerable<object>>>) (() => default (IOrderedEnumerable<object>).ThenByDescending<object, object>(default (Func<object, object>))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedSelector;

    public ThenByDescendingExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression keySelector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (keySelector), keySelector);
      if (keySelector != null && keySelector.Parameters.Count != 1)
        throw new ArgumentException("KeySelector must have exactly one parameter.", nameof (keySelector));
      this.KeySelector = keySelector;
      this._cachedSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression KeySelector { get; private set; }

    public Expression GetResolvedKeySelector(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.KeySelector.Body, this.KeySelector.Parameters[0], clauseGenerationContext)));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return this.Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      (this.GetOrderByClause(queryModel) ?? throw new ParserException("ThenByDescending expressions must follow OrderBy, OrderByDescending, ThenBy, or ThenByDescending expressions.")).Orderings.Add(new Ordering(this.GetResolvedKeySelector(clauseGenerationContext), OrderingDirection.Desc));
      return queryModel;
    }

    private OrderByClause GetOrderByClause(QueryModel queryModel)
    {
      return queryModel.BodyClauses.Count == 0 ? (OrderByClause) null : queryModel.BodyClauses[queryModel.BodyClauses.Count - 1] as OrderByClause;
    }
  }
}
