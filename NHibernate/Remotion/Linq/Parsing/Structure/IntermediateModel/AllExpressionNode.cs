// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.AllExpressionNode
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
  public class AllExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<bool>((Expression<Func<bool>>) (() => default (IQueryable<object>).All<object>(default (Expression<Func<object, bool>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<bool>((Expression<Func<bool>>) (() => default (IEnumerable<object>).All<object>(default (Func<object, bool>))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedPredicate;

    public AllExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression predicate)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (predicate), predicate);
      this.Predicate = predicate;
      this._cachedPredicate = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression Predicate { get; private set; }

    public Expression GetResolvedPredicate(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedPredicate.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.Predicate.Body, this.Predicate.Parameters[0], clauseGenerationContext)));
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
      return (ResultOperatorBase) new AllResultOperator(this.GetResolvedPredicate(clauseGenerationContext));
    }
  }
}
