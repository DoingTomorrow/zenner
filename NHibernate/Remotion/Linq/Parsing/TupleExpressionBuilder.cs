// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.TupleExpressionBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing
{
  public class TupleExpressionBuilder
  {
    public static Expression AggregateExpressionsIntoTuple(IEnumerable<Expression> expressions)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<Expression>>(nameof (expressions), expressions);
      return expressions.Reverse<Expression>().Aggregate<Expression>((Func<Expression, Expression, Expression>) ((current, expression) => TupleExpressionBuilder.CreateTupleExpression(expression, current)));
    }

    public static IEnumerable<Expression> GetExpressionsFromTuple(Expression tupleExpression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (tupleExpression), tupleExpression);
      for (; tupleExpression.Type.IsGenericType && tupleExpression.Type.GetGenericTypeDefinition() == typeof (KeyValuePair<,>); tupleExpression = (Expression) Expression.MakeMemberAccess(tupleExpression, (MemberInfo) tupleExpression.Type.GetProperty("Value")))
        yield return (Expression) Expression.MakeMemberAccess(tupleExpression, (MemberInfo) tupleExpression.Type.GetProperty("Key"));
      yield return tupleExpression;
    }

    private static Expression CreateTupleExpression(Expression left, Expression right)
    {
      Type type = typeof (KeyValuePair<,>).MakeGenericType(left.Type, right.Type);
      return (Expression) Expression.New(type.GetConstructor(new Type[2]
      {
        left.Type,
        right.Type
      }), (IEnumerable<Expression>) new Expression[2]
      {
        left,
        right
      }, (MemberInfo[]) new MethodInfo[2]
      {
        type.GetMethod("get_Key"),
        type.GetMethod("get_Value")
      });
    }
  }
}
