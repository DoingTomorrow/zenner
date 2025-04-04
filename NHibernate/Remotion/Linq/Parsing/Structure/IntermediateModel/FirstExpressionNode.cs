// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.FirstExpressionNode
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
  public class FirstExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression optionalPredicate) : ResultOperatorExpressionNodeBase(parseInfo, optionalPredicate, (LambdaExpression) null)
  {
    public static readonly MethodInfo[] SupportedMethods = new MethodInfo[8]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IQueryable<object>).First<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IQueryable<object>).First<object>(default (Expression<Func<object, bool>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IQueryable<object>).FirstOrDefault<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IQueryable<object>).FirstOrDefault<object>(default (Expression<Func<object, bool>>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IEnumerable<object>).First<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IEnumerable<object>).First<object>(default (Func<object, bool>)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IEnumerable<object>).FirstOrDefault<object>())),
      MethodCallExpressionNodeBase.GetSupportedMethod<object>((Expression<Func<object>>) (() => default (IEnumerable<object>).FirstOrDefault<object>(default (Func<object, bool>))))
    };

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionToBeResolved), expressionToBeResolved);
      throw this.CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new FirstResultOperator(this.ParsedExpression.Method.Name.EndsWith("OrDefault"));
    }
  }
}
