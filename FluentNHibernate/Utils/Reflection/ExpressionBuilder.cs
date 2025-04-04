// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.Reflection.ExpressionBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Utils.Reflection
{
  public class ExpressionBuilder
  {
    public static Expression<Func<T, object>> Create<T>(Member member)
    {
      return member is PropertyMember ? ExpressionBuilder.Create<T>((PropertyInfo) member.MemberInfo) : throw new InvalidOperationException("Cannot create property expression from non-property Member.");
    }

    public static Expression<Func<T, object>> Create<T>(PropertyInfo property)
    {
      return (Expression<Func<T, object>>) ExpressionBuilder.Create(property, typeof (T));
    }

    public static object Create(PropertyInfo property, Type type)
    {
      ParameterExpression parameterExpression = Expression.Parameter(type, "entity");
      return (object) Expression.Lambda((Expression) Expression.Convert((Expression) Expression.Property((Expression) parameterExpression, property), typeof (object)), parameterExpression);
    }
  }
}
