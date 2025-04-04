// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.JoinExpressionNode
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
  public class JoinExpressionNode : 
    MethodCallExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).Join<object, object, object, object>(default (IEnumerable<object>), (Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, object, object>>) ((o, i) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).Join<object, object, object, object>(default (IEnumerable<object>), (Func<object, object>) (o => (object) null), (Func<object, object>) (o => (object) null), (Func<object, object, object>) ((o, i) => (object) null))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedOuterKeySelector;
    private readonly ResolvedExpressionCache<Expression> _cachedInnerKeySelector;
    private readonly ResolvedExpressionCache<Expression> _cachedResultSelector;

    public JoinExpressionNode(
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
      this.InnerSequence = innerSequence;
      this.OuterKeySelector = outerKeySelector;
      this.InnerKeySelector = innerKeySelector;
      this.ResultSelector = resultSelector;
      this._cachedOuterKeySelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
      this._cachedInnerKeySelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
      this._cachedResultSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public Expression InnerSequence { get; private set; }

    public LambdaExpression OuterKeySelector { get; private set; }

    public LambdaExpression InnerKeySelector { get; private set; }

    public LambdaExpression ResultSelector { get; private set; }

    public Expression GetResolvedOuterKeySelector(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedOuterKeySelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.OuterKeySelector.Body, this.OuterKeySelector.Parameters[0], clauseGenerationContext)));
    }

    public Expression GetResolvedInnerKeySelector(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedInnerKeySelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, this.InnerKeySelector.Parameters[0], this.InnerKeySelector.Body, clauseGenerationContext)));
    }

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
      Expression resolvedResultSelector = this.GetResolvedResultSelector(clauseGenerationContext);
      return ReplacingExpressionTreeVisitor.Replace((Expression) inputParameter, resolvedResultSelector, expressionToBeResolved);
    }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      JoinClause joinClause = this.CreateJoinClause(clauseGenerationContext);
      queryModel.BodyClauses.Add((IBodyClause) joinClause);
      queryModel.SelectClause.Selector = this.GetResolvedResultSelector(clauseGenerationContext);
      return queryModel;
    }

    public JoinClause CreateJoinClause(ClauseGenerationContext clauseGenerationContext)
    {
      ConstantExpression innerKeySelector = Expression.Constant((object) null);
      JoinClause contextInfo = new JoinClause(this.ResultSelector.Parameters[1].Name, this.ResultSelector.Parameters[1].Type, this.InnerSequence, this.GetResolvedOuterKeySelector(clauseGenerationContext), (Expression) innerKeySelector);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) contextInfo);
      contextInfo.InnerKeySelector = this.GetResolvedInnerKeySelector(clauseGenerationContext);
      return contextInfo;
    }
  }
}
