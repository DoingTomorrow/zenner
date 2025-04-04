// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.ExpressionsHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Util
{
  public static class ExpressionsHelper
  {
    public static MemberInfo DecodeMemberAccessExpression<TEntity, TResult>(
      Expression<Func<TEntity, TResult>> expression)
    {
      if (expression.Body.NodeType != ExpressionType.MemberAccess)
        throw new HibernateException(string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}", (object) expression.Body.NodeType));
      return ((MemberExpression) expression.Body).Member;
    }
  }
}
