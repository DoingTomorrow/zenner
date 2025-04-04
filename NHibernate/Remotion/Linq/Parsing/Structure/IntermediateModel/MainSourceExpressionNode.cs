// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.MainSourceExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class MainSourceExpressionNode : IQuerySourceExpressionNode, IExpressionNode
  {
    public MainSourceExpressionNode(string associatedIdentifier, Expression expression)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (associatedIdentifier), associatedIdentifier);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      this.QuerySourceType = expression.Type;
      this.QuerySourceElementType = ReflectionUtility.GetItemTypeOfIEnumerable(expression.Type, nameof (expression));
      this.AssociatedIdentifier = associatedIdentifier;
      this.ParsedExpression = expression;
    }

    public Type QuerySourceElementType { get; private set; }

    public Type QuerySourceType { get; set; }

    public Expression ParsedExpression { get; private set; }

    public string AssociatedIdentifier { get; set; }

    public IExpressionNode Source => (IExpressionNode) null;

    public Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    public QueryModel Apply(QueryModel queryModel, ClauseGenerationContext clauseGenerationContext)
    {
      if (queryModel != null)
        throw new ArgumentException("QueryModel has to be null because MainSourceExpressionNode marks the start of a query.", nameof (queryModel));
      MainFromClause mainFromClause = this.CreateMainFromClause(clauseGenerationContext);
      SelectClause selectClause = new SelectClause((Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause));
      return new QueryModel(mainFromClause, selectClause)
      {
        ResultTypeOverride = this.QuerySourceType
      };
    }

    private MainFromClause CreateMainFromClause(ClauseGenerationContext clauseGenerationContext)
    {
      MainFromClause contextInfo = new MainFromClause(this.AssociatedIdentifier, this.QuerySourceElementType, this.ParsedExpression);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) contextInfo);
      return contextInfo;
    }
  }
}
