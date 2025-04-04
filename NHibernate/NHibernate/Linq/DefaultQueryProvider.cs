// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.DefaultQueryProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq
{
  public class DefaultQueryProvider : INhQueryProvider, IQueryProvider
  {
    public DefaultQueryProvider(ISessionImplementor session) => this.Session = session;

    protected virtual ISessionImplementor Session { get; private set; }

    public virtual object Execute(Expression expression)
    {
      IQuery query;
      NhLinqExpression nhQuery;
      return this.ExecuteQuery(this.PrepareQuery(expression, out query, out nhQuery), query, nhQuery);
    }

    public TResult Execute<TResult>(Expression expression) => (TResult) this.Execute(expression);

    public virtual IQueryable CreateQuery(Expression expression)
    {
      return (IQueryable) ReflectionHelper.GetMethodDefinition<DefaultQueryProvider>((Expression<Action<DefaultQueryProvider>>) (p => p.CreateQuery<object>(default (Expression)))).MakeGenericMethod(expression.Type.GetGenericArguments()[0]).Invoke((object) this, (object[]) new Expression[1]
      {
        expression
      });
    }

    public virtual IQueryable<T> CreateQuery<T>(Expression expression)
    {
      return (IQueryable<T>) new NhQueryable<T>((IQueryProvider) this, expression);
    }

    public virtual object ExecuteFuture(Expression expression)
    {
      IQuery query;
      NhLinqExpression nhQuery;
      return this.ExecuteFutureQuery(this.PrepareQuery(expression, out query, out nhQuery), query, nhQuery);
    }

    protected NhLinqExpression PrepareQuery(
      Expression expression,
      out IQuery query,
      out NhLinqExpression nhQuery)
    {
      NhLinqExpression nhLinqExpression = new NhLinqExpression(expression, (ISessionFactory) this.Session.Factory);
      query = this.Session.CreateQuery((IQueryExpression) nhLinqExpression);
      nhQuery = query.As<ExpressionQueryImpl>().QueryExpression.As<NhLinqExpression>();
      DefaultQueryProvider.SetParameters(query, nhLinqExpression.ParameterValuesByName);
      this.SetResultTransformerAndAdditionalCriteria(query, nhQuery, nhLinqExpression.ParameterValuesByName);
      return nhLinqExpression;
    }

    protected virtual object ExecuteFutureQuery(
      NhLinqExpression nhLinqExpression,
      IQuery query,
      NhLinqExpression nhQuery)
    {
      MethodInfo methodInfo;
      if (nhLinqExpression.ReturnType == NhLinqExpressionReturnType.Sequence)
        methodInfo = typeof (IQuery).GetMethod("Future").MakeGenericMethod(nhQuery.Type);
      else
        methodInfo = typeof (IQuery).GetMethod("FutureValue").MakeGenericMethod(nhQuery.Type);
      object obj = methodInfo.Invoke((object) query, new object[0]);
      if ((object) nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer != null)
        ((IDelayedValue) obj).ExecuteOnEval = nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer;
      return obj;
    }

    protected virtual object ExecuteQuery(
      NhLinqExpression nhLinqExpression,
      IQuery query,
      NhLinqExpression nhQuery)
    {
      IList source = query.List();
      if ((object) nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer != null)
      {
        try
        {
          return nhQuery.ExpressionToHqlTranslationResults.PostExecuteTransformer.DynamicInvoke((object) source.AsQueryable());
        }
        catch (TargetInvocationException ex)
        {
          throw ex.InnerException;
        }
      }
      else
        return nhLinqExpression.ReturnType == NhLinqExpressionReturnType.Sequence ? (object) source.AsQueryable() : source[0];
    }

    private static void SetParameters(
      IQuery query,
      IDictionary<string, Tuple<object, IType>> parameters)
    {
      foreach (string namedParameter in query.NamedParameters)
      {
        Tuple<object, IType> parameter = parameters[namedParameter];
        if (parameter.First == null)
        {
          if (typeof (ICollection).IsAssignableFrom(parameter.Second.ReturnedClass))
            query.SetParameterList(namedParameter, (IEnumerable) null, parameter.Second);
          else
            query.SetParameter(namedParameter, (object) null, parameter.Second);
        }
        else if (parameter.First is ICollection)
          query.SetParameterList(namedParameter, (IEnumerable) parameter.First);
        else if (parameter.Second != null)
          query.SetParameter(namedParameter, parameter.First, parameter.Second);
        else
          query.SetParameter(namedParameter, parameter.First);
      }
    }

    public void SetResultTransformerAndAdditionalCriteria(
      IQuery query,
      NhLinqExpression nhExpression,
      IDictionary<string, Tuple<object, IType>> parameters)
    {
      query.SetResultTransformer((IResultTransformer) nhExpression.ExpressionToHqlTranslationResults.ResultTransformer);
      foreach (Action<IQuery, IDictionary<string, Tuple<object, IType>>> additionalCriterion in nhExpression.ExpressionToHqlTranslationResults.AdditionalCriteria)
        additionalCriterion(query, parameters);
    }
  }
}
