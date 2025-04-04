// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.BaseHqlGeneratorForProperty
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Linq.Visitors;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public abstract class BaseHqlGeneratorForProperty : IHqlGeneratorForProperty
  {
    public IEnumerable<MemberInfo> SupportedProperties { get; protected set; }

    public abstract HqlTreeNode BuildHql(
      MemberInfo member,
      Expression expression,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor);
  }
}
