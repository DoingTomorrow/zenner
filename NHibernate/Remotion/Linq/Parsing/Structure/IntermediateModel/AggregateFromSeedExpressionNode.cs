// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.AggregateFromSeedExpressionNode
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
  public class AggregateFromSeedExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[4]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<System.Func<object>>) (() => default (IQueryable<object>).Aggregate<object, object>((object) null, (Expression<System.Func<object, object, object>>) ((o1, o2) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<System.Func<object>>) (() => default (IQueryable<object>).Aggregate<object, object, object>((object) null, (Expression<System.Func<object, object, object>>) ((o1, o2) => (object) null), (Expression<System.Func<object, object>>) (o => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<System.Func<object>>) (() => default (IEnumerable<object>).Aggregate<object, object>((object) null, (System.Func<object, object, object>) ((o1, o2) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<System.Func<object>>) (() => default (IEnumerable<object>).Aggregate<object, object, object>((object) null, (System.Func<object, object, object>) ((o1, o2) => (object) null), (System.Func<object, object>) (o => (object) null))))
    };
    private readonly ResolvedExpressionCache<LambdaExpression> _cachedFunc;

    public AggregateFromSeedExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      Expression seed,
      LambdaExpression func,
      LambdaExpression optionalResultSelector)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (seed), seed);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      if (func.Parameters.Count != 2)
        throw new ArgumentException("Func must have exactly two parameters.", nameof (func));
      if (optionalResultSelector != null && optionalResultSelector.Parameters.Count != 1)
        throw new ArgumentException("Result selector must have exactly one parameter.", nameof (optionalResultSelector));
      this.Seed = seed;
      this.Func = func;
      this.OptionalResultSelector = optionalResultSelector;
      this._cachedFunc = new ResolvedExpressionCache<LambdaExpression>((IExpressionNode) this);
    }

    public Expression Seed { get; private set; }

    public LambdaExpression Func { get; private set; }

    public LambdaExpression OptionalResultSelector { get; private set; }

    public LambdaExpression GetResolvedFunc(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedFunc.GetOrCreate((System.Func<ExpressionResolver, LambdaExpression>) (r => Expression.Lambda(r.GetResolvedExpression(this.Func.Body, this.Func.Parameters[1], clauseGenerationContext), this.Func.Parameters[0])));
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      throw this.CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new AggregateFromSeedResultOperator(this.Seed, this.GetResolvedFunc(clauseGenerationContext), this.OptionalResultSelector);
    }
  }
}
