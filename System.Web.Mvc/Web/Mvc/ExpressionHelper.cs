// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionHelper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc.ExpressionUtil;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public static class ExpressionHelper
  {
    public static string GetExpressionText(string expression)
    {
      return !string.Equals(expression, "model", StringComparison.OrdinalIgnoreCase) ? expression : string.Empty;
    }

    public static string GetExpressionText(LambdaExpression expression)
    {
      Stack<string> source = new Stack<string>();
      Expression expression1 = expression.Body;
      while (expression1 != null)
      {
        if (expression1.NodeType == ExpressionType.Call)
        {
          MethodCallExpression methodCallExpression = (MethodCallExpression) expression1;
          if (ExpressionHelper.IsSingleArgumentIndexer((Expression) methodCallExpression))
          {
            source.Push(ExpressionHelper.GetIndexerInvocation(methodCallExpression.Arguments.Single<Expression>(), expression.Parameters.ToArray<ParameterExpression>()));
            expression1 = methodCallExpression.Object;
          }
          else
            break;
        }
        else if (expression1.NodeType == ExpressionType.ArrayIndex)
        {
          BinaryExpression binaryExpression = (BinaryExpression) expression1;
          source.Push(ExpressionHelper.GetIndexerInvocation(binaryExpression.Right, expression.Parameters.ToArray<ParameterExpression>()));
          expression1 = binaryExpression.Left;
        }
        else if (expression1.NodeType == ExpressionType.MemberAccess)
        {
          MemberExpression memberExpression = (MemberExpression) expression1;
          source.Push("." + memberExpression.Member.Name);
          expression1 = memberExpression.Expression;
        }
        else if (expression1.NodeType == ExpressionType.Parameter)
        {
          source.Push(string.Empty);
          expression1 = (Expression) null;
        }
        else
          break;
      }
      if (source.Count > 0 && string.Equals(source.Peek(), ".model", StringComparison.OrdinalIgnoreCase))
        source.Pop();
      if (source.Count <= 0)
        return string.Empty;
      return source.Aggregate<string>((Func<string, string, string>) ((left, right) => left + right)).TrimStart('.');
    }

    private static string GetIndexerInvocation(
      Expression expression,
      ParameterExpression[] parameters)
    {
      Expression<Func<object, object>> lambdaExpression = Expression.Lambda<Func<object, object>>((Expression) Expression.Convert(expression, typeof (object)), Expression.Parameter(typeof (object), (string) null));
      Func<object, object> func;
      try
      {
        func = CachedExpressionCompiler.Process<object, object>(lambdaExpression);
      }
      catch (InvalidOperationException ex)
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ExpressionHelper_InvalidIndexerExpression, new object[2]
        {
          (object) expression,
          (object) parameters[0].Name
        }), (Exception) ex);
      }
      return "[" + Convert.ToString(func((object) null), (IFormatProvider) CultureInfo.InvariantCulture) + "]";
    }

    internal static bool IsSingleArgumentIndexer(Expression expression)
    {
      MethodCallExpression methodExpression = expression as MethodCallExpression;
      return methodExpression != null && methodExpression.Arguments.Count == 1 && methodExpression.Method.DeclaringType.GetDefaultMembers().OfType<PropertyInfo>().Any<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.GetGetMethod() == methodExpression.Method));
    }
  }
}
