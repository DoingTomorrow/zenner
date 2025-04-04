// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.CountExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class CountExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression optionalPredicate) : ResultOperatorExpressionNodeBase(parseInfo, optionalPredicate, (LambdaExpression) null)
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[9]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IQueryable<object>).Count<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IQueryable<object>).Count<object>(default (Expression<Func<object, bool>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IEnumerable<object>).Count<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (IEnumerable<object>).Count<object>(default (Func<object, bool>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (List<int>).Count)),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (ICollection<int>).Count)),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (ArrayList).Count)),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (ICollection).Count)),
      MethodCallExpressionNodeBase.GetSupportedMethod<int>((Expression<Func<int>>) (() => default (Array).Length))
    };

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
      return (ResultOperatorBase) new CountResultOperator();
    }
  }
}
