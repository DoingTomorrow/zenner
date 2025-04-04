// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.UnionExpressionNode
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
  public class UnionExpressionNode : 
    ResultOperatorExpressionNodeBase,
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[2]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).Union<object>(default (IEnumerable<object>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).Union<object>(default (IEnumerable<object>))))
    };

    public UnionExpressionNode(MethodCallExpressionParseInfo parseInfo, Expression source2)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (source2), source2);
      this.Source2 = source2;
      this.ItemType = ReflectionUtility.GetItemTypeOfIEnumerable(parseInfo.ParsedExpression.Type, "expression");
    }

    public Expression Source2 { get; private set; }

    public Type ItemType { get; private set; }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return QuerySourceExpressionNodeUtility.ReplaceParameterWithReference((IQuerySourceExpressionNode) this, inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      UnionResultOperator contextInfo = new UnionResultOperator(this.AssociatedIdentifier, this.ItemType, this.Source2);
      clauseGenerationContext.AddContextInfo((IExpressionNode) this, (object) contextInfo);
      return (ResultOperatorBase) contextInfo;
    }
  }
}
