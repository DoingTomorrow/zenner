// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.EagerFetching.Parsing.FetchExpressionNodeBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.EagerFetching.Parsing
{
  public abstract class FetchExpressionNodeBase : ResultOperatorExpressionNodeBase
  {
    protected FetchExpressionNodeBase(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression relatedObjectSelector)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (relatedObjectSelector), relatedObjectSelector);
      if (!(relatedObjectSelector.Body is MemberExpression body))
        throw new ArgumentException(string.Format("A fetch request must be a simple member access expression; '{0}' is a {1} instead.", (object) relatedObjectSelector.Body, (object) relatedObjectSelector.Body.GetType().Name), nameof (relatedObjectSelector));
      if (body.Expression.NodeType != ExpressionType.Parameter)
        throw new ArgumentException(string.Format("A fetch request must be a simple member access expression of the kind o => o.Related; '{0}' is too complex.", (object) relatedObjectSelector.Body), nameof (relatedObjectSelector));
      this.RelationMember = body.Member;
    }

    public MemberInfo RelationMember { get; private set; }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      return this.Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
    }
  }
}
