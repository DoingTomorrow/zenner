// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MethodCallExpressionNodeBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public abstract class MethodCallExpressionNodeBase : IExpressionNode
  {
    private IExpressionNode _source;

    protected static MethodInfo GetSupportedMethod<T>(Expression<Func<T>> methodCall)
    {
      ArgumentUtility.CheckNotNull<Expression<Func<T>>>(nameof (methodCall), methodCall);
      return MethodInfoBasedNodeTypeRegistry.GetRegisterableMethodDefinition(ReflectionUtility.GetMethod<T>(methodCall));
    }

    protected MethodCallExpressionNodeBase(MethodCallExpressionParseInfo parseInfo)
    {
      this.AssociatedIdentifier = parseInfo.AssociatedIdentifier;
      this.Source = parseInfo.Source;
      this.NodeResultType = parseInfo.ParsedExpression.Type;
    }

    public string AssociatedIdentifier { get; private set; }

    public IExpressionNode Source
    {
      get => this._source;
      protected set
      {
        this._source = ArgumentUtility.CheckNotNull<IExpressionNode>(nameof (value), value);
      }
    }

    public Type NodeResultType { get; private set; }

    public abstract Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext);

    protected abstract QueryModel ApplyNodeSpecificSemantics(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext);

    public QueryModel Apply(QueryModel queryModel, ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel = this.WrapQueryModelAfterEndOfQuery(queryModel, clauseGenerationContext);
      queryModel = this.ApplyNodeSpecificSemantics(queryModel, clauseGenerationContext);
      this.SetResultTypeOverride(queryModel);
      return queryModel;
    }

    protected virtual QueryModel WrapQueryModelAfterEndOfQuery(
      QueryModel queryModel,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      return this.Source is ResultOperatorExpressionNodeBase source ? this.WrapQueryModel(queryModel, source.AssociatedIdentifier, clauseGenerationContext) : queryModel;
    }

    protected virtual void SetResultTypeOverride(QueryModel queryModel)
    {
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      queryModel.ResultTypeOverride = this.NodeResultType;
    }

    private QueryModel WrapQueryModel(
      QueryModel queryModel,
      string associatedIdentifier,
      ClauseGenerationContext clauseGenerationContext)
    {
      SubQueryExpression subQueryExpression = new SubQueryExpression(queryModel);
      MainSourceExpressionNode sourceExpressionNode = new MainSourceExpressionNode(associatedIdentifier, (Expression) subQueryExpression);
      this.Source = (IExpressionNode) sourceExpressionNode;
      return sourceExpressionNode.Apply((QueryModel) null, clauseGenerationContext);
    }

    protected InvalidOperationException CreateResolveNotSupportedException()
    {
      return new InvalidOperationException(this.GetType().Name + " does not support resolving of expressions, because it does not stream any data to the following node.");
    }

    protected InvalidOperationException CreateOutputParameterNotSupportedException()
    {
      return new InvalidOperationException(this.GetType().Name + " does not support creating a parameter for its output because it does not stream any data to the following node.");
    }
  }
}
