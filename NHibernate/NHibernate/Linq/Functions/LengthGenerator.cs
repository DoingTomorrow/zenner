// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.LengthGenerator
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
  public class LengthGenerator : BaseHqlGeneratorForProperty
  {
    public LengthGenerator()
    {
      this.SupportedProperties = (IEnumerable<MemberInfo>) new MemberInfo[1]
      {
        ReflectionHelper.GetProperty<string, int>((Expression<Func<string, int>>) (x => x.Length))
      };
    }

    public override HqlTreeNode BuildHql(
      MemberInfo member,
      Expression expression,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      return (HqlTreeNode) treeBuilder.MethodCall("length", visitor.Visit(expression).AsExpression());
    }
  }
}
