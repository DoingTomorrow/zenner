// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ExpressionParameterVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Param;
using NHibernate.Type;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class ExpressionParameterVisitor : NhExpressionTreeVisitor
  {
    private readonly Dictionary<ConstantExpression, NamedParameter> _parameters = new Dictionary<ConstantExpression, NamedParameter>();
    private readonly ISessionFactory _sessionFactory;

    public ExpressionParameterVisitor(ISessionFactory sessionFactory)
    {
      this._sessionFactory = sessionFactory;
    }

    public static IDictionary<ConstantExpression, NamedParameter> Visit(
      Expression expression,
      ISessionFactory sessionFactory)
    {
      ExpressionParameterVisitor parameterVisitor = new ExpressionParameterVisitor(sessionFactory);
      parameterVisitor.VisitExpression(expression);
      return (IDictionary<ConstantExpression, NamedParameter>) parameterVisitor._parameters;
    }

    protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
    {
      return VisitorUtil.IsDynamicComponentDictionaryGetter(expression, this._sessionFactory) ? (Expression) expression : base.VisitMethodCallExpression(expression);
    }

    protected override Expression VisitConstantExpression(ConstantExpression expression)
    {
      if (!this._parameters.ContainsKey(expression) && !typeof (IQueryable).IsAssignableFrom(expression.Type) && !ExpressionParameterVisitor.IsNullObject(expression))
      {
        IType type = (IType) null;
        if (expression.Value == null)
          type = NHibernateUtil.GuessType(expression.Type);
        this._parameters.Add(expression, new NamedParameter("p" + (object) (this._parameters.Count + 1), expression.Value, type));
      }
      return base.VisitConstantExpression(expression);
    }

    private static bool IsNullObject(ConstantExpression expression)
    {
      return expression.Type == typeof (object) && expression.Value == null;
    }
  }
}
