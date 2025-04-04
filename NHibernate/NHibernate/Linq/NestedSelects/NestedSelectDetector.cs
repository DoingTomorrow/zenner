// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NestedSelects.NestedSelectDetector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Visitors;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.NestedSelects
{
  internal class NestedSelectDetector : NhExpressionTreeVisitor
  {
    private readonly ISessionFactory sessionFactory;
    private readonly ICollection<Expression> _expressions = (ICollection<Expression>) new List<Expression>();

    public NestedSelectDetector(ISessionFactory sessionFactory)
    {
      this.sessionFactory = sessionFactory;
    }

    public ICollection<Expression> Expressions => this._expressions;

    public bool HasSubqueries => this.Expressions.Count > 0;

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      if (expression.QueryModel.ResultOperators.Count == 0)
        this.Expressions.Add((Expression) expression);
      return base.VisitSubQueryExpression(expression);
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      if (expression.Expression is QuerySourceReferenceExpression)
      {
        Type propertyOrFieldType = expression.Member.GetPropertyOrFieldType();
        if (propertyOrFieldType != null && propertyOrFieldType.IsCollectionType() && this.IsMappedCollection(expression.Member))
          this.Expressions.Add((Expression) expression);
      }
      return base.VisitMemberExpression(expression);
    }

    private bool IsMappedCollection(MemberInfo memberInfo)
    {
      return this.sessionFactory.GetCollectionMetadata(memberInfo.DeclaringType.FullName + "." + memberInfo.Name) != null;
    }
  }
}
