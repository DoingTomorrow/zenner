// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Expressions.NhAverageExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Expressions
{
  public class NhAverageExpression(Expression expression) : NhAggregatedExpression(expression, NhAverageExpression.CalculateAverageType(expression.Type), NhExpressionType.Average)
  {
    private static Type CalculateAverageType(Type inputType)
    {
      bool flag = false;
      if (inputType.IsNullable())
      {
        flag = true;
        inputType = inputType.NullableOf();
      }
      switch (Type.GetTypeCode(inputType))
      {
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
        case TypeCode.Single:
        case TypeCode.Double:
          return !flag ? typeof (double) : typeof (double?);
        case TypeCode.Decimal:
          return !flag ? typeof (Decimal) : typeof (Decimal?);
        default:
          throw new NotSupportedException(inputType.FullName);
      }
    }
  }
}
