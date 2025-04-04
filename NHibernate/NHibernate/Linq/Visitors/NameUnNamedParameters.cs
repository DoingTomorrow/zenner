// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.NameUnNamedParameters
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  [Obsolete("This class is not required by NHibernate anymore.")]
  public class NameUnNamedParameters : NhExpressionTreeVisitor
  {
    private readonly Dictionary<ParameterExpression, ParameterExpression> _renamedParameters = new Dictionary<ParameterExpression, ParameterExpression>();

    public static Expression Visit(Expression expression)
    {
      return new NameUnNamedParameters().VisitExpression(expression);
    }

    protected override Expression VisitParameterExpression(ParameterExpression expression)
    {
      if (!string.IsNullOrEmpty(expression.Name))
        return base.VisitParameterExpression(expression);
      ParameterExpression parameterExpression;
      if (this._renamedParameters.TryGetValue(expression, out parameterExpression))
        return (Expression) parameterExpression;
      parameterExpression = Expression.Parameter(expression.Type, Guid.NewGuid().ToString());
      this._renamedParameters.Add(expression, parameterExpression);
      return (Expression) parameterExpression;
    }
  }
}
