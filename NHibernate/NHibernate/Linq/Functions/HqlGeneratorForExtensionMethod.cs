// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.HqlGeneratorForExtensionMethod
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
  public class HqlGeneratorForExtensionMethod : BaseHqlGeneratorForMethod
  {
    private readonly string _name;

    public HqlGeneratorForExtensionMethod(LinqExtensionMethodAttribute attribute, MethodInfo method)
    {
      this._name = string.IsNullOrEmpty(attribute.Name) ? method.Name : attribute.Name;
    }

    public override HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      IEnumerable<HqlExpression> parameters = visitor.Visit(targetObject).Union(arguments.Select<Expression, HqlTreeNode>((Func<Expression, HqlTreeNode>) (a => visitor.Visit(a)))).Cast<HqlExpression>();
      return (HqlTreeNode) treeBuilder.MethodCall(this._name, parameters);
    }
  }
}
