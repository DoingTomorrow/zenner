// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.AggregateExpressionNode
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
  public class AggregateExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<System.Func<object>>) (() => default (IQueryable<object>).Aggregate<object>((Expression<System.Func<object, object, object>>) ((o1, o2) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<System.Func<object>>) (() => default (IEnumerable<object>).Aggregate<object>((System.Func<object, object, object>) ((o1, o2) => (object) null))))
    };
    private readonly ResolvedExpressionCache<LambdaExpression> _cachedFunc;

    public AggregateExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression func)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      if (func.Parameters.Count != 2)
        throw new ArgumentException("Func must have exactly two parameters.", nameof (func));
      this.Func = func;
      this._cachedFunc = new ResolvedExpressionCache<LambdaExpression>((IExpressionNode) this);
    }

    public LambdaExpression Func { get; private set; }

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
      return (ResultOperatorBase) new AggregateResultOperator(this.GetResolvedFunc(clauseGenerationContext));
    }
  }
}
