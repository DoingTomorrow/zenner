// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessClientSideSelect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.GroupBy;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessClientSideSelect : IResultOperatorProcessor<ClientSideSelect>
  {
    public void Process(
      ClientSideSelect resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      Type type = resultOperator.SelectClause.Parameters[0].Type;
      Type genericArgument = resultOperator.SelectClause.Type.GetGenericArguments()[1];
      ParameterExpression parameterExpression = Expression.Parameter(typeof (IEnumerable<>).MakeGenericType(type), "inputList");
      MethodInfo method = EnumerableHelper.GetMethod("Select", new Type[2]
      {
        typeof (IEnumerable<>),
        typeof (Func<,>)
      }, new Type[2]{ type, genericArgument });
      LambdaExpression lambda = Expression.Lambda((Expression) Expression.Call(EnumerableHelper.GetMethod("ToList", new Type[1]
      {
        typeof (IEnumerable<>)
      }, new Type[1]{ genericArgument }), (Expression) Expression.Call(method, (Expression) parameterExpression, (Expression) resultOperator.SelectClause)), parameterExpression);
      tree.AddListTransformer(lambda);
    }
  }
}
