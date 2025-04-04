// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations.NullableValueTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation.PredefinedTransformations
{
  public class NullableValueTransformer : IExpressionTransformer<MemberExpression>
  {
    public ExpressionType[] SupportedExpressionTypes
    {
      get
      {
        return new ExpressionType[1]
        {
          ExpressionType.MemberAccess
        };
      }
    }

    public Expression Transform(MemberExpression expression)
    {
      ArgumentUtility.CheckNotNull<MemberExpression>(nameof (expression), expression);
      if (expression.Member.Name == "Value" && this.IsDeclaredByNullableType(expression.Member))
        return (Expression) Expression.Convert(expression.Expression, expression.Type);
      return expression.Member.Name == "HasValue" && this.IsDeclaredByNullableType(expression.Member) ? (Expression) Expression.NotEqual(expression.Expression, (Expression) Expression.Constant((object) null, expression.Member.DeclaringType)) : (Expression) expression;
    }

    private bool IsDeclaredByNullableType(MemberInfo memberInfo)
    {
      return memberInfo.DeclaringType.IsGenericType && memberInfo.DeclaringType.GetGenericTypeDefinition() == typeof (Nullable<>);
    }
  }
}
