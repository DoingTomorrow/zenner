// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessFirstOrSingleBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessFirstOrSingleBase
  {
    protected static void AddClientSideEval(
      MethodInfo target,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      Type type = queryModelVisitor.Model.SelectClause.Selector.Type;
      target = target.MakeGenericMethod(type);
      ParameterExpression parameterExpression = Expression.Parameter(typeof (IQueryable<>).MakeGenericType(type), (string) null);
      LambdaExpression lambda = Expression.Lambda((Expression) Expression.Call(target, (Expression) parameterExpression), parameterExpression);
      tree.AddPostExecuteTransformer(lambda);
    }
  }
}
