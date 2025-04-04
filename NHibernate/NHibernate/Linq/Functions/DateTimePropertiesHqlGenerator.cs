// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.DateTimePropertiesHqlGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Linq.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public class DateTimePropertiesHqlGenerator : IHqlGeneratorForProperty
  {
    private readonly MemberInfo[] supportedProperties;

    public DateTimePropertiesHqlGenerator()
    {
      this.supportedProperties = new MemberInfo[7]
      {
        ReflectionHelper.GetProperty<DateTime, int>((Expression<Func<DateTime, int>>) (x => x.Year)),
        ReflectionHelper.GetProperty<DateTime, int>((Expression<Func<DateTime, int>>) (x => x.Month)),
        ReflectionHelper.GetProperty<DateTime, int>((Expression<Func<DateTime, int>>) (x => x.Day)),
        ReflectionHelper.GetProperty<DateTime, int>((Expression<Func<DateTime, int>>) (x => x.Hour)),
        ReflectionHelper.GetProperty<DateTime, int>((Expression<Func<DateTime, int>>) (x => x.Minute)),
        ReflectionHelper.GetProperty<DateTime, int>((Expression<Func<DateTime, int>>) (x => x.Second)),
        ReflectionHelper.GetProperty<DateTime, DateTime>((Expression<Func<DateTime, DateTime>>) (x => x.Date))
      };
    }

    public IEnumerable<MemberInfo> SupportedProperties
    {
      get => (IEnumerable<MemberInfo>) this.supportedProperties;
    }

    public virtual HqlTreeNode BuildHql(
      MemberInfo member,
      Expression expression,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      return (HqlTreeNode) treeBuilder.MethodCall(member.Name.ToLowerInvariant(), visitor.Visit(expression).AsExpression());
    }
  }
}
