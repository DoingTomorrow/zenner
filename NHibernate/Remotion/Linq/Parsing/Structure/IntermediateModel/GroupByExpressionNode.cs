// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.GroupByExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class GroupByExpressionNode : 
    ResultOperatorExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[4]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<IGrouping<object, object>>>((Expression<Func<IQueryable<IGrouping<object, object>>>>) (() => default (IQueryable<object>).GroupBy<object, object>((Expression<Func<object, object>>) (o => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<IGrouping<object, object>>>((Expression<Func<IEnumerable<IGrouping<object, object>>>>) (() => default (IEnumerable<object>).GroupBy<object, object>((Func<object, object>) (o => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<IGrouping<object, object>>>((Expression<Func<IQueryable<IGrouping<object, object>>>>) (() => default (IQueryable<object>).GroupBy<object, object, object>((Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, object>>) (o => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<IGrouping<object, object>>>((Expression<Func<IEnumerable<IGrouping<object, object>>>>) (() => default (IEnumerable<object>).GroupBy<object, object, object>((Func<object, object>) (o => (object) null), (Func<object, object>) (o => (object) null))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedKeySelector;
    private readonly ResolvedExpressionCache<Expression> _cachedElementSelector;

    public GroupByExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression keySelector,
      LambdaExpression optionalElementSelector)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (keySelector), keySelector);
      if (keySelector.Parameters.Count != 1)
        throw new ArgumentException("KeySelector must have exactly one parameter.", nameof (keySelector));
      if (optionalElementSelector != null && optionalElementSelector.Parameters.Count != 1)
        throw new ArgumentException("ElementSelector must have exactly one parameter.", nameof (optionalElementSelector));
      this.KeySelector = keySelector;
      this.OptionalElementSelector = optionalElementSelector;
      this._cachedKeySelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
      if (optionalElementSelector == null)
        return;
      this._cachedElementSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression KeySelector { get; private set; }

    public LambdaExpression OptionalElementSelector { get; private set; }

    public Expression GetResolvedKeySelector(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedKeySelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.KeySelector.Body, this.KeySelector.Parameters[0], clauseGenerationContext)));
    }

    public Expression GetResolvedOptionalElementSelector(
      ClauseGenerationContext clauseGenerationContext)
    {
      return this.OptionalElementSelector == null ? (Expression) null : this._cachedElementSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.OptionalElementSelector.Body, this.OptionalElementSelector.Parameters[0], clauseGenerationContext)));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      return QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      GroupResultOperator contextInfo = new GroupResultOperator(this.AssociatedIdentifier, this.GetResolvedKeySelector(clauseGenerationContext), this.GetResolvedOptionalElementSelector(clauseGenerationContext) ?? this.Source.Resolve(this.KeySelector.Parameters[0], (Expression) this.KeySelector.Parameters[0], clauseGenerationContext));
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) contextInfo);
      return (ResultOperatorBase) contextInfo;
    }
  }
}
