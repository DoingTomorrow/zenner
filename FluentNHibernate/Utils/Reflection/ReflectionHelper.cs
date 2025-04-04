// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Utils.Reflection.ReflectionHelper
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Utils.Reflection
{
  public static class ReflectionHelper
  {
    public static Member GetMember<TModel, TReturn>(Expression<Func<TModel, TReturn>> expression)
    {
      return ReflectionHelper.GetMember(expression.Body);
    }

    public static Member GetMember<TModel>(Expression<Func<TModel, object>> expression)
    {
      return ReflectionHelper.GetMember(expression.Body);
    }

    public static Accessor GetAccessor<MODEL>(Expression<Func<MODEL, object>> expression)
    {
      return ReflectionHelper.getAccessor(ReflectionHelper.GetMemberExpression(expression.Body));
    }

    public static Accessor GetAccessor<MODEL, T>(Expression<Func<MODEL, T>> expression)
    {
      return ReflectionHelper.getAccessor(ReflectionHelper.GetMemberExpression(expression.Body));
    }

    private static bool IsIndexedPropertyAccess(Expression expression)
    {
      return ReflectionHelper.IsMethodExpression(expression) && expression.ToString().Contains("get_Item");
    }

    private static bool IsMethodExpression(Expression expression)
    {
      switch (expression)
      {
        case MethodCallExpression _:
          return true;
        case UnaryExpression _:
          return ReflectionHelper.IsMethodExpression((expression as UnaryExpression).Operand);
        default:
          return false;
      }
    }

    private static Member GetMember(Expression expression)
    {
      if (ReflectionHelper.IsIndexedPropertyAccess(expression))
        return MemberExtensions.ToMember(ReflectionHelper.GetDynamicComponentProperty(expression));
      return ReflectionHelper.IsMethodExpression(expression) ? MemberExtensions.ToMember(((MethodCallExpression) expression).Method) : ReflectionHelper.GetMemberExpression(expression).Member.ToMember();
    }

    private static PropertyInfo GetDynamicComponentProperty(Expression expression)
    {
      Type type = (Type) null;
      MethodCallExpression methodCallExpression = (MethodCallExpression) null;
      UnaryExpression unaryExpression;
      for (Expression expression1 = expression; expression1 != null; expression1 = unaryExpression.Operand)
      {
        if (expression1.NodeType == ExpressionType.Call)
        {
          methodCallExpression = expression1 as MethodCallExpression;
          type = type ?? methodCallExpression.Method.ReturnType;
          break;
        }
        unaryExpression = expression1.NodeType == ExpressionType.Convert ? (UnaryExpression) expression1 : throw new ArgumentException("Expression not supported", nameof (expression));
        type = unaryExpression.Type;
      }
      return (PropertyInfo) new DummyPropertyInfo((string) (methodCallExpression.Arguments[0] as ConstantExpression).Value, type);
    }

    private static MemberExpression GetMemberExpression(Expression expression)
    {
      return ReflectionHelper.GetMemberExpression(expression, true);
    }

    private static MemberExpression GetMemberExpression(Expression expression, bool enforceCheck)
    {
      MemberExpression memberExpression = (MemberExpression) null;
      if (expression.NodeType == ExpressionType.Convert)
        memberExpression = ((UnaryExpression) expression).Operand as MemberExpression;
      else if (expression.NodeType == ExpressionType.MemberAccess)
        memberExpression = expression as MemberExpression;
      return !enforceCheck || memberExpression != null ? memberExpression : throw new ArgumentException("Not a member access", nameof (expression));
    }

    private static Accessor getAccessor(MemberExpression memberExpression)
    {
      List<Member> memberList = new List<Member>();
      for (; memberExpression != null; memberExpression = memberExpression.Expression as MemberExpression)
        memberList.Add(memberExpression.Member.ToMember());
      if (memberList.Count == 1)
        return (Accessor) new SingleMember(memberList[0]);
      memberList.Reverse();
      return (Accessor) new PropertyChain(memberList.ToArray());
    }
  }
}
