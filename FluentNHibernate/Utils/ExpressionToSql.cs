// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.ExpressionToSql
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Utils
{
  public class ExpressionToSql
  {
    public static string Convert<T>(Expression<Func<T, object>> expression)
    {
      if (expression.Body is MemberExpression)
        return ExpressionToSql.Convert<T>(expression, (MemberExpression) expression.Body);
      if (expression.Body is MethodCallExpression)
        return ExpressionToSql.Convert((MethodCallExpression) expression.Body);
      if (expression.Body is UnaryExpression)
        return ExpressionToSql.Convert<T>(expression, (UnaryExpression) expression.Body);
      return expression.Body is ConstantExpression ? ExpressionToSql.Convert((ConstantExpression) expression.Body) : throw new InvalidOperationException("Unable to convert expression to SQL");
    }

    public static string Convert<T>(Expression<Func<T, bool>> expression)
    {
      if (expression.Body is BinaryExpression)
        return ExpressionToSql.Convert<T>((BinaryExpression) expression.Body);
      if (expression.Body is MemberExpression body1 && body1.Type == typeof (bool))
        return ExpressionToSql.Convert<T>(ExpressionToSql.CreateExpression<T>((Expression) body1)) + " = " + ExpressionToSql.Convert((object) true);
      if (expression.Body is UnaryExpression body2 && body2.Type == typeof (bool) && body2.NodeType == ExpressionType.Not)
        return ExpressionToSql.Convert<T>(ExpressionToSql.CreateExpression<T>(body2.Operand)) + " = " + ExpressionToSql.Convert((object) false);
      throw new InvalidOperationException("Unable to convert expression to SQL");
    }

    private static string Convert<T>(Expression<Func<T, object>> expression, MemberExpression body)
    {
      MemberInfo member = body.Member;
      return member.DeclaringType == typeof (T) ? member.Name : ExpressionToSql.Convert(expression.Compile()(default (T)));
    }

    private static string Convert(MethodCallExpression body)
    {
      return ExpressionToSql.Convert(body.Method.Invoke((object) body.Object, (object[]) null));
    }

    private static string Convert<T>(Expression<Func<T, object>> expression, UnaryExpression body)
    {
      if (body.Operand is ConstantExpression operand1)
        return ExpressionToSql.Convert(operand1);
      if (body.Operand is MemberExpression operand2)
        return ExpressionToSql.Convert<T>(expression, operand2);
      if (body.Operand is UnaryExpression operand3 && operand3.NodeType == ExpressionType.Convert)
        return ExpressionToSql.Convert<T>(expression, operand3);
      throw new InvalidOperationException("Unable to convert expression to SQL");
    }

    private static string Convert(ConstantExpression expression)
    {
      return expression.Type.IsEnum ? ExpressionToSql.Convert((object) (int) expression.Value) : ExpressionToSql.Convert(expression.Value);
    }

    private static Expression<Func<T, object>> CreateExpression<T>(Expression body)
    {
      Expression expression = body;
      ParameterExpression parameterExpression = Expression.Parameter(typeof (T), "x");
      if (expression.Type.IsValueType)
        expression = (Expression) Expression.Convert(expression, typeof (object));
      return (Expression<Func<T, object>>) Expression.Lambda(typeof (Func<T, object>), expression, parameterExpression);
    }

    private static string Convert<T>(BinaryExpression expression)
    {
      string str1 = ExpressionToSql.Convert<T>(ExpressionToSql.CreateExpression<T>(expression.Left));
      string str2 = ExpressionToSql.Convert<T>(ExpressionToSql.CreateExpression<T>(expression.Right));
      string str3;
      switch (expression.NodeType)
      {
        case ExpressionType.GreaterThan:
          str3 = ">";
          break;
        case ExpressionType.GreaterThanOrEqual:
          str3 = ">=";
          break;
        case ExpressionType.LessThan:
          str3 = "<";
          break;
        case ExpressionType.LessThanOrEqual:
          str3 = "<=";
          break;
        case ExpressionType.NotEqual:
          str3 = "!=";
          break;
        default:
          str3 = "=";
          break;
      }
      return str1 + " " + str3 + " " + str2;
    }

    private static string Convert(object value)
    {
      switch (value)
      {
        case string _:
          return "'" + value + "'";
        case bool flag:
          return !flag ? "0" : "1";
        default:
          return value.ToString();
      }
    }
  }
}
