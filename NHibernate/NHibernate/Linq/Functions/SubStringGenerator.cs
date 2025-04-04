// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.SubStringGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Linq.Visitors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  public class SubStringGenerator : BaseHqlGeneratorForMethod
  {
    public SubStringGenerator()
    {
      this.SupportedMethods = (IEnumerable<MethodInfo>) new MethodInfo[2]
      {
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.Substring(0))),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.Substring(0, 0)))
      };
    }

    public override HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      HqlExpression hqlExpression1 = visitor.Visit(targetObject).AsExpression();
      HqlAdd hqlAdd = treeBuilder.Add(visitor.Visit(arguments[0]).AsExpression(), (HqlExpression) treeBuilder.Constant((object) 1));
      if (arguments.Count == 1)
        return (HqlTreeNode) treeBuilder.MethodCall("substring", hqlExpression1, (HqlExpression) hqlAdd);
      HqlExpression hqlExpression2 = visitor.Visit(arguments[1]).AsExpression();
      return (HqlTreeNode) treeBuilder.MethodCall("substring", hqlExpression1, (HqlExpression) hqlAdd, hqlExpression2);
    }
  }
}
