// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.LongCountExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class LongCountExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression optionalPredicate) : ResultOperatorExpressionNodeBase(parseInfo, optionalPredicate, (LambdaExpression) null)
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[5]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IQueryable<object>).LongCount<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IQueryable<object>).LongCount<object>(default (Expression<Func<object, bool>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IEnumerable<object>).LongCount<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (IEnumerable<object>).LongCount<object>(default (Func<object, bool>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<long>((Expression<Func<long>>) (() => default (Array).LongLength))
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
      return (ResultOperatorBase) new LongCountResultOperator();
    }
  }
}
