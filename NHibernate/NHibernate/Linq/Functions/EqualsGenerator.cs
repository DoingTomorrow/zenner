// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.EqualsGenerator
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
  public class EqualsGenerator : BaseHqlGeneratorForMethod
  {
    public EqualsGenerator()
    {
      this.SupportedMethods = (IEnumerable<MethodInfo>) new MethodInfo[17]
      {
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => string.Equals(default (string), default (string)))),
        ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<char>((Expression<Action<char>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<sbyte>((Expression<Action<sbyte>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<byte>((Expression<Action<byte>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<short>((Expression<Action<short>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<ushort>((Expression<Action<ushort>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<int>((Expression<Action<int>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<uint>((Expression<Action<uint>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<long>((Expression<Action<long>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<ulong>((Expression<Action<ulong>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<float>((Expression<Action<float>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<double>((Expression<Action<double>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<Decimal>((Expression<Action<Decimal>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<Guid>((Expression<Action<Guid>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<DateTime>((Expression<Action<DateTime>>) (x => x.Equals(x))),
        ReflectionHelper.GetMethodDefinition<DateTimeOffset>((Expression<Action<DateTimeOffset>>) (x => x.Equals(x)))
      };
    }

    public override HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      Expression expression1 = arguments.Count == 1 ? targetObject : arguments[0];
      Expression expression2 = arguments.Count == 1 ? arguments[0] : arguments[1];
      return (HqlTreeNode) treeBuilder.Equality(visitor.Visit(expression1).AsExpression(), visitor.Visit(expression2).AsExpression());
    }
  }
}
