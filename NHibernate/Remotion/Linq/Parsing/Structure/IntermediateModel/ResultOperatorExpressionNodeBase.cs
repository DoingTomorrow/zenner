// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.ResultOperatorExpressionNodeBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public abstract class ResultOperatorExpressionNodeBase : MethodCallExpressionNodeBase
  {
    protected ResultOperatorExpressionNodeBase(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression optionalPredicate,
      LambdaExpression optionalSelector)
      : base(parseInfo)
    {
      if (optionalPredicate != null && optionalPredicate.Parameters.Count != 1)
        throw new ArgumentException("OptionalPredicate must have exactly one parameter.", nameof (optionalPredicate));
      if (optionalSelector != null && optionalSelector.Parameters.Count != 1)
        throw new ArgumentException("OptionalSelector must have exactly one parameter.", nameof (optionalSelector));
      if (optionalPredicate != null)
        this.Source = (IExpressionNode) new WhereExpressionNode(parseInfo, optionalPredicate);
      if (optionalSelector != null)
        this.Source = (IExpressionNode) new SelectExpressionNode(new MethodCallExpressionParseInfo(parseInfo.AssociatedIdentifier, this.Source, parseInfo.ParsedExpression), optionalSelector);
      this.ParsedExpression = parseInfo.ParsedExpression;
    }

    protected abstract ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext);

    public MethodCallExpression ParsedExpression { get; private set; }

    protected override QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      ResultOperatorBase resultOperator = this.CreateResultOperator(clauseGenerationContext);
      queryModel.ResultOperators.Add(resultOperator);
      return queryModel;
    }

    protected override QueryModel WrapQueryModelAfterEndOfQuery(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      return queryModel;
    }
  }
}
