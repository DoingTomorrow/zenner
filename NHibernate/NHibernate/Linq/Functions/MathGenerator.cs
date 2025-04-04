// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.MathGenerator
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
  public class MathGenerator : BaseHqlGeneratorForMethod
  {
    public MathGenerator()
    {
      this.SupportedMethods = (IEnumerable<MethodInfo>) new MethodInfo[35]
      {
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sin(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Cos(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Tan(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sinh(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Cosh(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Tanh(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Asin(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Acos(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Atan(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Atan2(0.0, 0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sqrt(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Abs(0M))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Abs(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Abs(0.0f))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Abs(0L))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Abs(0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Abs((short) 0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Abs((sbyte) 0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sign(0M))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sign(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sign(0.0f))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sign(0L))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sign(0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sign((short) 0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Sign((sbyte) 0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Round(0M))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Round(0M, 0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Round(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Round(0.0, 0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Floor(0M))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Floor(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Ceiling(0M))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Ceiling(0.0))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Truncate(0M))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => Math.Truncate(0.0)))
      };
    }

    public override HqlTreeNode BuildHql(
      MethodInfo method,
      Expression expression,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      string lowerInvariant = method.Name.ToLowerInvariant();
      HqlExpression hqlExpression = visitor.Visit(arguments[0]).AsExpression();
      return arguments.Count == 2 ? (HqlTreeNode) treeBuilder.MethodCall(lowerInvariant, hqlExpression, visitor.Visit(arguments[1]).AsExpression()) : (HqlTreeNode) treeBuilder.MethodCall(lowerInvariant, hqlExpression);
    }
  }
}
