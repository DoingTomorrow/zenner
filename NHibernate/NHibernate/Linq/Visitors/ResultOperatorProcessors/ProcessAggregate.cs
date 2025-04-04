// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessAggregate
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessAggregate : IResultOperatorProcessor<AggregateResultOperator>
  {
    public void Process(
      AggregateResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      Expression itemExpression = ((StreamedSequenceInfo) queryModelVisitor.PreviousEvaluationType).ItemExpression;
      Type type = itemExpression.Type;
      ParameterExpression replacementExpression = Expression.Parameter(type, "item");
      LambdaExpression lambdaExpression = Expression.Lambda(ReplacingExpressionTreeVisitor.Replace(itemExpression, (Expression) replacementExpression, resultOperator.Func.Body), resultOperator.Func.Parameters[0], replacementExpression);
      ParameterExpression parameterExpression = Expression.Parameter(typeof (IEnumerable<>).MakeGenericType(typeof (object)), "inputList");
      MethodCallExpression methodCallExpression = Expression.Call(EnumerableHelper.GetMethod("Cast", new Type[1]
      {
        typeof (IEnumerable)
      }, new Type[1]{ type }), (Expression) parameterExpression);
      MethodCallExpression body = Expression.Call(ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IEnumerable<object>).Aggregate<object>(default (Func<object, object, object>)))).GetGenericMethodDefinition().MakeGenericMethod(type), (Expression) methodCallExpression, (Expression) lambdaExpression);
      tree.AddListTransformer(Expression.Lambda((Expression) body, parameterExpression));
    }
  }
}
