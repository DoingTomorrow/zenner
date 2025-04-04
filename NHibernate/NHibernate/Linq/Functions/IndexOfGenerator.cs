// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.IndexOfGenerator
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
  public class IndexOfGenerator : BaseHqlGeneratorForMethod
  {
    public IndexOfGenerator()
    {
      this.SupportedMethods = (IEnumerable<MethodInfo>) new MethodInfo[4]
      {
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.IndexOf(' '))),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.IndexOf(" "))),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.IndexOf(' ', 0))),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.IndexOf(" ", 0)))
      };
    }

    public override HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      return arguments.Count == 1 ? (HqlTreeNode) treeBuilder.MethodCall("locate", visitor.Visit(arguments[0]).AsExpression(), visitor.Visit(targetObject).AsExpression(), (HqlExpression) treeBuilder.Constant((object) 0)) : (HqlTreeNode) treeBuilder.MethodCall("locate", visitor.Visit(arguments[0]).AsExpression(), visitor.Visit(targetObject).AsExpression(), visitor.Visit(arguments[1]).AsExpression());
    }
  }
}
