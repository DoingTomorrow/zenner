// Decompiled with JetBrains decompiler
// Type: AutoMapper.Impl.ReflectionHelper
// Assembly: AutoMapper, Version=3.1.1.0, Culture=neutral, PublicKeyToken=be96cd2c38ef1005
// MVID: D6FEE810-B806-4119-85A4-5044E7EED03C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AutoMapper.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace AutoMapper.Impl
{
  public static class ReflectionHelper
  {
    public static MemberInfo FindProperty(LambdaExpression lambdaExpression)
    {
      Expression expression = (Expression) lambdaExpression;
      bool flag = false;
      while (!flag)
      {
        switch (expression.NodeType)
        {
          case ExpressionType.Convert:
            expression = ((UnaryExpression) expression).Operand;
            continue;
          case ExpressionType.Lambda:
            expression = ((LambdaExpression) expression).Body;
            continue;
          case ExpressionType.MemberAccess:
            MemberExpression memberExpression = (MemberExpression) expression;
            if (memberExpression.Expression.NodeType != ExpressionType.Parameter && memberExpression.Expression.NodeType != ExpressionType.Convert)
              throw new ArgumentException(string.Format("Expression '{0}' must resolve to top-level member and not any child object's properties. Use a custom resolver on the child type or the AfterMap option instead.", new object[1]
              {
                (object) lambdaExpression
              }), nameof (lambdaExpression));
            return memberExpression.Member;
          default:
            flag = true;
            continue;
        }
      }
      throw new AutoMapperConfigurationException("Custom configuration for members is only supported for top-level individual members on a type.");
    }

    public static Type GetMemberType(this MemberInfo memberInfo)
    {
      if ((object) (memberInfo as MethodInfo) != null)
        return ((MethodInfo) memberInfo).ReturnType;
      if ((object) (memberInfo as PropertyInfo) != null)
        return ((PropertyInfo) memberInfo).PropertyType;
      return (object) (memberInfo as FieldInfo) != null ? ((FieldInfo) memberInfo).FieldType : (Type) null;
    }

    public static IMemberGetter ToMemberGetter(this MemberInfo accessorCandidate)
    {
      if ((object) accessorCandidate == null)
        return (IMemberGetter) null;
      if ((object) (accessorCandidate as PropertyInfo) != null)
        return (IMemberGetter) new PropertyGetter((PropertyInfo) accessorCandidate);
      if ((object) (accessorCandidate as FieldInfo) != null)
        return (IMemberGetter) new FieldGetter((FieldInfo) accessorCandidate);
      return (object) (accessorCandidate as MethodInfo) != null ? (IMemberGetter) new MethodGetter((MethodInfo) accessorCandidate) : (IMemberGetter) null;
    }

    public static IMemberAccessor ToMemberAccessor(this MemberInfo accessorCandidate)
    {
      FieldInfo fieldInfo = accessorCandidate as FieldInfo;
      if ((object) fieldInfo != null)
        return !accessorCandidate.DeclaringType.IsValueType ? (IMemberAccessor) new FieldAccessor(fieldInfo) : (IMemberAccessor) new ValueTypeFieldAccessor(fieldInfo);
      PropertyInfo propertyInfo = accessorCandidate as PropertyInfo;
      if ((object) propertyInfo == null)
        return (IMemberAccessor) null;
      return !accessorCandidate.DeclaringType.IsValueType ? (IMemberAccessor) new PropertyAccessor(propertyInfo) : (IMemberAccessor) new ValueTypePropertyAccessor(propertyInfo);
    }
  }
}
