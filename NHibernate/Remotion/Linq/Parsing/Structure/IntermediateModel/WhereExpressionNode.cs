// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.WhereExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class WhereExpressionNode : MethodCallExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).Where<object>((Expression<Func<object, bool>>) (o => true)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).Where<object>((Func<object, bool>) (o => true))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedPredicate;

    public WhereExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression predicate)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (predicate), predicate);
      if (predicate.Parameters.Count != 1)
        throw new ArgumentException("Predicate must have exactly one parameter.", nameof (predicate));
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
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return this.Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      WhereClause whereClause = new WhereClause(this.GetResolvedPredicate(clauseGenerationContext));
      queryModel.BodyClauses.Add((IBodyClause) whereClause);
      return queryModel;
    }
  }
}
