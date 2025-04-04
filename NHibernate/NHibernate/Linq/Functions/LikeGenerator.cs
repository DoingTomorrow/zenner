// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Functions.LikeGenerator
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
  public class LikeGenerator : IHqlGeneratorForMethod, IRuntimeMethodHqlGenerator
  {
    public IEnumerable<MethodInfo> SupportedMethods => throw new NotImplementedException();

    public HqlTreeNode BuildHql(
      MethodInfo method,
      Expression targetObject,
      ReadOnlyCollection<Expression> arguments,
      HqlTreeBuilder treeBuilder,
      IHqlExpressionVisitor visitor)
    {
      return (HqlTreeNode) treeBuilder.Like(visitor.Visit(arguments[0]).AsExpression(), visitor.Visit(arguments[1]).AsExpression());
    }

    public bool SupportsMethod(MethodInfo method)
    {
      return method != null && method.Name == "Like" && method.GetParameters().Length == 2 && method.DeclaringType != null && method.DeclaringType.FullName.EndsWith("SqlMethods");
    }

    public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method)
    {
      return (IHqlGeneratorForMethod) this;
    }
  }
}
