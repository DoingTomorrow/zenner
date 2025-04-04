// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.CompareGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Linq.Visitors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  internal class CompareGenerator : BaseHqlGeneratorForMethod, IRuntimeMethodHqlGenerator
  {
    private static readonly HashSet<MethodInfo> ActingMethods;

    internal static bool IsCompareMethod(MethodInfo methodInfo)
    {
      if (CompareGenerator.ActingMethods.Contains(methodInfo))
        return true;
      return methodInfo != null && methodInfo.Name == "Compare" && methodInfo.DeclaringType != null && methodInfo.DeclaringType.FullName == "System.Data.Services.Providers.DataServiceProviderMethods";
    }

    public CompareGenerator()
    {
      this.SupportedMethods = (IEnumerable<MethodInfo>) CompareGenerator.ActingMethods.ToArray<MethodInfo>();
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
      HqlExpression lhs1 = visitor.Visit(expression1).AsExpression();
      HqlExpression rhs1 = visitor.Visit(expression2).AsExpression();
      HqlExpression lhs2 = visitor.Visit(expression1).AsExpression();
      HqlExpression rhs2 = visitor.Visit(expression2).AsExpression();
      return (HqlTreeNode) treeBuilder.Case(new HqlWhen[2]
      {
        treeBuilder.When((HqlExpression) treeBuilder.Equality(lhs1, rhs1), (HqlExpression) treeBuilder.Constant((object) 0)),
        treeBuilder.When((HqlExpression) treeBuilder.GreaterThan(lhs2, rhs2), (HqlExpression) treeBuilder.Constant((object) 1))
      }, (HqlExpression) treeBuilder.Constant((object) -1));
    }

    public bool SupportsMethod(MethodInfo method) => CompareGenerator.IsCompareMethod(method);

    public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method)
    {
      return (IHqlGeneratorForMethod) this;
    }

    static CompareGenerator()
    {
      HashSet<MethodInfo> methodInfoSet = new HashSet<MethodInfo>();
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => string.Compare(default (string), default (string)))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<string>((Expression<Action<string>>) (s => s.CompareTo(s))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<char>((Expression<Action<char>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<byte>((Expression<Action<byte>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<sbyte>((Expression<Action<sbyte>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<short>((Expression<Action<short>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<ushort>((Expression<Action<ushort>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<int>((Expression<Action<int>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<uint>((Expression<Action<uint>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<long>((Expression<Action<long>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<ulong>((Expression<Action<ulong>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<float>((Expression<Action<float>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<double>((Expression<Action<double>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<Decimal>((Expression<Action<Decimal>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<DateTime>((Expression<Action<DateTime>>) (x => x.CompareTo(x))));
      methodInfoSet.Add(ReflectionHelper.GetMethodDefinition<DateTimeOffset>((Expression<Action<DateTimeOffset>>) (x => x.CompareTo(x))));
      CompareGenerator.ActingMethods = methodInfoSet;
    }
  }
}
