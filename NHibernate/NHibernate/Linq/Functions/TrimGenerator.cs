// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.TrimGenerator
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
  public class TrimGenerator : BaseHqlGeneratorForMethod
  {
    public TrimGenerator()
    {
      this.SupportedMethods = (IEnumerable<MethodInfo>) new MethodInfo[4]
      {
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.Trim())),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.Trim(new char[]
        {
          'a'
        }))),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.TrimStart(new char[]
        {
          'a'
        }))),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.TrimEnd(new char[]
        {
          'a'
        })))
      };
    }

    public override HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      string ident = !(method.Name == "TrimStart") ? (!(method.Name == "TrimEnd") ? "both" : "trailing") : "leading";
      string str = "";
      if (method.GetParameters().Length > 0)
      {
        foreach (char ch in (char[]) ((ConstantExpression) arguments[0]).Value)
          str += (string) (object) ch;
      }
      return str == "" ? (HqlTreeNode) treeBuilder.MethodCall("trim", (HqlExpression) treeBuilder.Ident(ident), (HqlExpression) treeBuilder.Ident("from"), visitor.Visit(targetObject).AsExpression()) : (HqlTreeNode) treeBuilder.MethodCall("trim", (HqlExpression) treeBuilder.Ident(ident), (HqlExpression) treeBuilder.Constant((object) str), (HqlExpression) treeBuilder.Ident("from"), visitor.Visit(targetObject).AsExpression());
    }
  }
}
