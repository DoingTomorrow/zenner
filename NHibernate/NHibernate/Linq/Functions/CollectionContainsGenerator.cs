// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.CollectionContainsGenerator
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
  public class CollectionContainsGenerator : BaseHqlGeneratorForMethod
  {
    public CollectionContainsGenerator()
    {
      this.SupportedMethods = (IEnumerable<MethodInfo>) new MethodInfo[2]
      {
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IQueryable<object>).Contains<object>((object) null))),
        ReflectionHelper.GetMethodDefinition((Expression<Action>) (() => default (IEnumerable<object>).Contains<object>((object) null)))
      };
    }

    public override HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      HqlAlias alias = treeBuilder.Alias("x");
      ParameterExpression left = Expression.Parameter(targetObject.Type, "x");
      HqlWhere where = treeBuilder.Where(visitor.Visit((Expression) Expression.Lambda((Expression) Expression.Equal((Expression) left, arguments[0]), left)).AsExpression());
      return (HqlTreeNode) treeBuilder.Exists(treeBuilder.Query(treeBuilder.SelectFrom(treeBuilder.From(treeBuilder.Range(visitor.Visit(targetObject), alias))), where));
    }
  }
}
