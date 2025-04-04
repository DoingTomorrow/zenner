// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessNonAggregatingGroupBy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.ResultOperators;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessNonAggregatingGroupBy : IResultOperatorProcessor<NonAggregatingGroupBy>
  {
    public void Process(
      NonAggregatingGroupBy resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      Expression selector = queryModelVisitor.Model.SelectClause.Selector;
      Expression keySelector = resultOperator.GroupBy.KeySelector;
      Expression elementSelector = resultOperator.GroupBy.ElementSelector;
      Type type1 = selector.Type;
      Type type2 = keySelector.Type;
      Type type3 = elementSelector.Type;
      ParameterExpression parameterExpression = Expression.Parameter(typeof (IEnumerable<object>), "list");
      LambdaExpression lambdaExpression1 = ReverseResolvingExpressionTreeVisitor.ReverseResolve(selector, keySelector);
      LambdaExpression lambdaExpression2 = ReverseResolvingExpressionTreeVisitor.ReverseResolve(selector, elementSelector);
      MethodInfo method1 = EnumerableHelper.GetMethod("GroupBy", new Type[3]
      {
        typeof (IEnumerable<>),
        typeof (Func<,>),
        typeof (Func<,>)
      }, new Type[3]{ type1, type2, type3 });
      MethodInfo method2 = EnumerableHelper.GetMethod("Cast", new Type[1]
      {
        typeof (IEnumerable)
      }, new Type[1]{ type1 });
      MethodInfo method3 = EnumerableHelper.GetMethod("ToList", new Type[1]
      {
        typeof (IEnumerable<>)
      }, new Type[1]{ resultOperator.GroupBy.ItemType });
      Expression expression = (Expression) Expression.Call(method2, (Expression) parameterExpression);
      MethodCallExpression methodCallExpression = Expression.Call(method1, expression, (Expression) lambdaExpression1, (Expression) lambdaExpression2);
      LambdaExpression lambda = Expression.Lambda((Expression) Expression.Call(method3, (Expression) methodCallExpression), parameterExpression);
      tree.AddListTransformer(lambda);
    }
  }
}
