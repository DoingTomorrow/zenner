// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.GetValueOrDefaultGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Linq.Visitors;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Functions
{
  internal class GetValueOrDefaultGenerator : IHqlGeneratorForMethod, IRuntimeMethodHqlGenerator
  {
    public bool SupportsMethod(MethodInfo method)
    {
      return method != null && string.Equals(method.Name, "GetValueOrDefault", StringComparison.OrdinalIgnoreCase) && method.IsMethodOf(typeof (Nullable<>));
    }

    public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method)
    {
      return (IHqlGeneratorForMethod) this;
    }

    public IEnumerable<MethodInfo> SupportedMethods
    {
      get => throw new NotSupportedException("This is an runtime method generator");
    }

    public HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      return treeBuilder.Coalesce(visitor.Visit(targetObject).AsExpression(), GetValueOrDefaultGenerator.GetRhs(method, arguments, treeBuilder, visitor));
    }

    private static HqlExpression GetRhs(
      MethodInfo method,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      if (arguments.Count > 0)
        return visitor.Visit(arguments[0]).AsExpression();
      Type returnType = method.ReturnType;
      object instance = returnType.IsValueType ? Activator.CreateInstance(returnType) : (object) null;
      return (HqlExpression) treeBuilder.Constant(instance);
    }
  }
}
