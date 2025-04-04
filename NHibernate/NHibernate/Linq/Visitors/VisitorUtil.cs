// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.VisitorUtil
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Metadata;
using NHibernate.Type;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class VisitorUtil
  {
    public static bool IsDynamicComponentDictionaryGetter(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      ISessionFactory sessionFactory,
      out string memberName)
    {
      memberName = (string) null;
      if (method.Name != "get_Item" || !typeof (IDictionary).IsAssignableFrom(targetObject.Type) || !(arguments.First<Expression>() is ConstantExpression constantExpression) || constantExpression.Type != typeof (string))
        return false;
      memberName = (string) constantExpression.Value;
      if (!(targetObject is MemberExpression memberExpression))
        return false;
      IClassMetadata classMetadata = sessionFactory.GetClassMetadata(memberExpression.Expression.Type);
      if (classMetadata == null)
        return false;
      IType propertyType = classMetadata.GetPropertyType(memberExpression.Member.Name);
      return propertyType != null && propertyType.IsComponentType;
    }

    public static bool IsDynamicComponentDictionaryGetter(
      MethodCallExpression expression,
      ISessionFactory sessionFactory,
      out string memberName)
    {
      return VisitorUtil.IsDynamicComponentDictionaryGetter(expression.Method, expression.Object, expression.Arguments, sessionFactory, out memberName);
    }

    public static bool IsDynamicComponentDictionaryGetter(
      MethodCallExpression expression,
      ISessionFactory sessionFactory)
    {
      return VisitorUtil.IsDynamicComponentDictionaryGetter(expression, sessionFactory, out string _);
    }
  }
}
