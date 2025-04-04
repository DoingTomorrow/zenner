// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.SelectManyExpressionNode
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
  public class SelectManyExpressionNode : 
    MethodCallExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[4]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).SelectMany<object, object[], object>((Expression<Func<object, IEnumerable<object[]>>>) (o => default (IEnumerable<object[]>)), default (Expression<Func<object, object[], object>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).SelectMany<object, object[], object>((Func<object, IEnumerable<object[]>>) (o => default (IEnumerable<object[]>)), default (Func<object, object[], object>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object[]>>((Expression<Func<IQueryable<object[]>>>) (() => default (IQueryable<object>).SelectMany<object, object[]>((Expression<Func<object, IEnumerable<object[]>>>) (o => default (IEnumerable<object[]>))))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object[]>>((Expression<Func<IEnumerable<object[]>>>) (() => default (IEnumerable<object>).SelectMany<object, object[]>((Func<object, IEnumerable<object[]>>) (o => default (IEnumerable<object[]>)))))
    };
    private readonly ResolvedExpressionCache<Expression> _cachedCollectionSelector;
    private readonly ResolvedExpressionCache<Expression> _cachedResultSelector;

    public SelectManyExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression collectionSelector,
      LambdaExpression resultSelector)
      : base(parseInfo)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (collectionSelector), collectionSelector);
      if (collectionSelector.Parameters.Count != 1)
        throw new ArgumentException("Collection selector must have exactly one parameter.", nameof (collectionSelector));
      this.CollectionSelector = collectionSelector;
      if (resultSelector != null)
      {
        if (resultSelector.Parameters.Count != 2)
          throw new ArgumentException("Result selector must have exactly two parameters.", nameof (resultSelector));
        this.ResultSelector = resultSelector;
      }
      else
      {
        ParameterExpression parameterExpression = Expression.Parameter(collectionSelector.Parameters[0].Type, collectionSelector.Parameters[0].Name);
        ParameterExpression body = Expression.Parameter(ReflectionUtility.GetItemTypeOfIEnumerable(this.CollectionSelector.Body.Type, nameof (collectionSelector)), parseInfo.AssociatedIdentifier);
        this.ResultSelector = Expression.Lambda((Expression) body, parameterExpression, body);
      }
      this._cachedCollectionSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
      this._cachedResultSelector = new ResolvedExpressionCache<Expression>((IExpressionNode) this);
    }

    public LambdaExpression CollectionSelector { get; private set; }

    public LambdaExpression ResultSelector { get; private set; }

    public Expression GetResolvedCollectionSelector(ClauseGenerationContext clauseGenerationContext)
    {
      return this._cachedCollectionSelector.GetOrCreate((Func<ExpressionResolver, Expression>) (r => r.GetResolvedExpression(this.CollectionSelector.Body, this.CollectionSelector.Parameters[0], clauseGenerationContext)));
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
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      AdditionalFromClause contextInfo = new AdditionalFromClause(this.ResultSelector.Parameters[1].Name, this.ResultSelector.Parameters[1].Type, this.GetResolvedCollectionSelector(clauseGenerationContext));
      queryModel.BodyClauses.Add((IBodyClause) contextInfo);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) contextInfo);
      queryModel.SelectClause.Selector = this.GetResolvedResultSelector(clauseGenerationContext);
      return queryModel;
    }
  }
}
