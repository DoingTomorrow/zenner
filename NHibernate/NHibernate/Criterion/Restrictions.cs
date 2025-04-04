// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Restrictions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion.Lambda;
using NHibernate.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  public class Restrictions
  {
    internal Restrictions()
    {
    }

    public static AbstractCriterion IdEq(object value)
    {
      return (AbstractCriterion) new IdentifierEqExpression(value);
    }

    public static AbstractCriterion IdEq(IProjection projection)
    {
      return (AbstractCriterion) new IdentifierEqExpression(projection);
    }

    public static SimpleExpression Eq(string propertyName, object value)
    {
      return new SimpleExpression(propertyName, value, " = ");
    }

    public static SimpleExpression Eq(IProjection projection, object value)
    {
      return new SimpleExpression(projection, value, " = ");
    }

    public static SimpleExpression Like(string propertyName, object value)
    {
      return new SimpleExpression(propertyName, value, " like ");
    }

    public static AbstractCriterion Like(
      string propertyName,
      string value,
      MatchMode matchMode,
      char? escapeChar)
    {
      return (AbstractCriterion) new LikeExpression(propertyName, value, matchMode, escapeChar, false);
    }

    public static SimpleExpression Like(IProjection projection, object value)
    {
      return new SimpleExpression(projection, value, " like ");
    }

    public static SimpleExpression Like(IProjection projection, string value, MatchMode matchMode)
    {
      return new SimpleExpression(projection, (object) matchMode.ToMatchString(value), " like ");
    }

    public static SimpleExpression Like(string propertyName, string value, MatchMode matchMode)
    {
      return new SimpleExpression(propertyName, (object) matchMode.ToMatchString(value), " like ");
    }

    public static AbstractCriterion InsensitiveLike(
      string propertyName,
      string value,
      MatchMode matchMode)
    {
      return (AbstractCriterion) new InsensitiveLikeExpression(propertyName, value, matchMode);
    }

    public static AbstractCriterion InsensitiveLike(
      IProjection projection,
      string value,
      MatchMode matchMode)
    {
      return (AbstractCriterion) new InsensitiveLikeExpression(projection, value, matchMode);
    }

    public static AbstractCriterion InsensitiveLike(string propertyName, object value)
    {
      return (AbstractCriterion) new InsensitiveLikeExpression(propertyName, value);
    }

    public static AbstractCriterion InsensitiveLike(IProjection projection, object value)
    {
      return (AbstractCriterion) new InsensitiveLikeExpression(projection, value);
    }

    public static SimpleExpression Gt(string propertyName, object value)
    {
      return new SimpleExpression(propertyName, value, " > ");
    }

    public static SimpleExpression Gt(IProjection projection, object value)
    {
      return new SimpleExpression(projection, value, " > ");
    }

    public static SimpleExpression Lt(string propertyName, object value)
    {
      return new SimpleExpression(propertyName, value, " < ");
    }

    public static SimpleExpression Lt(IProjection projection, object value)
    {
      return new SimpleExpression(projection, value, " < ");
    }

    public static SimpleExpression Le(string propertyName, object value)
    {
      return new SimpleExpression(propertyName, value, " <= ");
    }

    public static SimpleExpression Le(IProjection projection, object value)
    {
      return new SimpleExpression(projection, value, " <= ");
    }

    public static SimpleExpression Ge(string propertyName, object value)
    {
      return new SimpleExpression(propertyName, value, " >= ");
    }

    public static SimpleExpression Ge(IProjection projection, object value)
    {
      return new SimpleExpression(projection, value, " >= ");
    }

    public static AbstractCriterion Between(string propertyName, object lo, object hi)
    {
      return (AbstractCriterion) new BetweenExpression(propertyName, lo, hi);
    }

    public static AbstractCriterion Between(IProjection projection, object lo, object hi)
    {
      return (AbstractCriterion) new BetweenExpression(projection, lo, hi);
    }

    public static AbstractCriterion In(string propertyName, object[] values)
    {
      return (AbstractCriterion) new InExpression(propertyName, values);
    }

    public static AbstractCriterion In(IProjection projection, object[] values)
    {
      return (AbstractCriterion) new InExpression(projection, values);
    }

    public static AbstractCriterion In(IProjection projection, ICollection values)
    {
      object[] values1 = new object[values.Count];
      values.CopyTo((Array) values1, 0);
      return (AbstractCriterion) new InExpression(projection, values1);
    }

    public static AbstractCriterion In(string propertyName, ICollection values)
    {
      object[] values1 = new object[values.Count];
      values.CopyTo((Array) values1, 0);
      return (AbstractCriterion) new InExpression(propertyName, values1);
    }

    public static AbstractCriterion InG<T>(string propertyName, IEnumerable<T> values)
    {
      object[] values1 = new object[values.Count<T>()];
      int index = 0;
      foreach (T obj in values)
      {
        values1[index] = (object) obj;
        ++index;
      }
      return (AbstractCriterion) new InExpression(propertyName, values1);
    }

    public static AbstractCriterion InG<T>(IProjection projection, IEnumerable<T> values)
    {
      object[] values1 = new object[values.Count<T>()];
      int index = 0;
      foreach (T obj in values)
      {
        values1[index] = (object) obj;
        ++index;
      }
      return (AbstractCriterion) new InExpression(projection, values1);
    }

    public static AbstractCriterion IsNull(string propertyName)
    {
      return (AbstractCriterion) new NullExpression(propertyName);
    }

    public static AbstractCriterion IsNull(IProjection projection)
    {
      return (AbstractCriterion) new NullExpression(projection);
    }

    public static AbstractCriterion EqProperty(string propertyName, string otherPropertyName)
    {
      return (AbstractCriterion) new EqPropertyExpression(propertyName, otherPropertyName);
    }

    public static AbstractCriterion EqProperty(IProjection projection, string otherPropertyName)
    {
      return (AbstractCriterion) new EqPropertyExpression(projection, otherPropertyName);
    }

    public static AbstractCriterion EqProperty(IProjection lshProjection, IProjection rshProjection)
    {
      return (AbstractCriterion) new EqPropertyExpression(lshProjection, rshProjection);
    }

    public static AbstractCriterion EqProperty(string propertyName, IProjection rshProjection)
    {
      return (AbstractCriterion) new EqPropertyExpression(propertyName, rshProjection);
    }

    public static AbstractCriterion NotEqProperty(string propertyName, string otherPropertyName)
    {
      return (AbstractCriterion) new NotExpression((ICriterion) new EqPropertyExpression(propertyName, otherPropertyName));
    }

    public static AbstractCriterion NotEqProperty(IProjection projection, string otherPropertyName)
    {
      return (AbstractCriterion) new NotExpression((ICriterion) new EqPropertyExpression(projection, otherPropertyName));
    }

    public static AbstractCriterion NotEqProperty(
      IProjection lhsProjection,
      IProjection rhsProjection)
    {
      return (AbstractCriterion) new NotExpression((ICriterion) new EqPropertyExpression(lhsProjection, rhsProjection));
    }

    public static AbstractCriterion NotEqProperty(string propertyName, IProjection rhsProjection)
    {
      return (AbstractCriterion) new NotExpression((ICriterion) new EqPropertyExpression(propertyName, rhsProjection));
    }

    public static AbstractCriterion GtProperty(string propertyName, string otherPropertyName)
    {
      return (AbstractCriterion) new GtPropertyExpression(propertyName, otherPropertyName);
    }

    public static AbstractCriterion GtProperty(IProjection projection, string otherPropertyName)
    {
      return (AbstractCriterion) new GtPropertyExpression(projection, otherPropertyName);
    }

    public static AbstractCriterion GtProperty(string propertyName, IProjection projection)
    {
      return (AbstractCriterion) new GtPropertyExpression(propertyName, projection);
    }

    public static AbstractCriterion GtProperty(IProjection lhsProjection, IProjection rhsProjection)
    {
      return (AbstractCriterion) new GtPropertyExpression(lhsProjection, rhsProjection);
    }

    public static AbstractCriterion GeProperty(string propertyName, string otherPropertyName)
    {
      return (AbstractCriterion) new GePropertyExpression(propertyName, otherPropertyName);
    }

    public static AbstractCriterion GeProperty(IProjection lhsProjection, IProjection rhsProjection)
    {
      return (AbstractCriterion) new GePropertyExpression(lhsProjection, rhsProjection);
    }

    public static AbstractCriterion GeProperty(IProjection projection, string otherPropertyName)
    {
      return (AbstractCriterion) new GePropertyExpression(projection, otherPropertyName);
    }

    public static AbstractCriterion GeProperty(string propertyName, IProjection projection)
    {
      return (AbstractCriterion) new GePropertyExpression(propertyName, projection);
    }

    public static AbstractCriterion LtProperty(string propertyName, string otherPropertyName)
    {
      return (AbstractCriterion) new LtPropertyExpression(propertyName, otherPropertyName);
    }

    public static AbstractCriterion LtProperty(IProjection projection, string otherPropertyName)
    {
      return (AbstractCriterion) new LtPropertyExpression(projection, otherPropertyName);
    }

    public static AbstractCriterion LtProperty(string propertyName, IProjection projection)
    {
      return (AbstractCriterion) new LtPropertyExpression(propertyName, projection);
    }

    public static AbstractCriterion LtProperty(IProjection lhsProjection, IProjection rhsProjection)
    {
      return (AbstractCriterion) new LtPropertyExpression(lhsProjection, rhsProjection);
    }

    public static AbstractCriterion LeProperty(string propertyName, string otherPropertyName)
    {
      return (AbstractCriterion) new LePropertyExpression(propertyName, otherPropertyName);
    }

    public static AbstractCriterion LeProperty(IProjection projection, string otherPropertyName)
    {
      return (AbstractCriterion) new LePropertyExpression(projection, otherPropertyName);
    }

    public static AbstractCriterion LeProperty(string propertyName, IProjection projection)
    {
      return (AbstractCriterion) new LePropertyExpression(propertyName, projection);
    }

    public static AbstractCriterion LeProperty(IProjection lhsProjection, IProjection rhsProjection)
    {
      return (AbstractCriterion) new LePropertyExpression(lhsProjection, rhsProjection);
    }

    public static AbstractCriterion IsNotNull(string propertyName)
    {
      return (AbstractCriterion) new NotNullExpression(propertyName);
    }

    public static AbstractCriterion IsNotNull(IProjection projection)
    {
      return (AbstractCriterion) new NotNullExpression(projection);
    }

    public static AbstractEmptinessExpression IsNotEmpty(string propertyName)
    {
      return (AbstractEmptinessExpression) new IsNotEmptyExpression(propertyName);
    }

    public static AbstractEmptinessExpression IsEmpty(string propertyName)
    {
      return (AbstractEmptinessExpression) new IsEmptyExpression(propertyName);
    }

    public static AbstractCriterion And(ICriterion lhs, ICriterion rhs)
    {
      return (AbstractCriterion) new AndExpression(lhs, rhs);
    }

    public static AbstractCriterion Or(ICriterion lhs, ICriterion rhs)
    {
      return (AbstractCriterion) new OrExpression(lhs, rhs);
    }

    public static AbstractCriterion Not(ICriterion expression)
    {
      return (AbstractCriterion) new NotExpression(expression);
    }

    public static NHibernate.Criterion.Conjunction Conjunction() => new NHibernate.Criterion.Conjunction();

    public static NHibernate.Criterion.Disjunction Disjunction() => new NHibernate.Criterion.Disjunction();

    public static AbstractCriterion AllEq(IDictionary propertyNameValues)
    {
      NHibernate.Criterion.Conjunction conjunction = Restrictions.Conjunction();
      foreach (DictionaryEntry propertyNameValue in propertyNameValues)
        conjunction.Add((ICriterion) Restrictions.Eq(propertyNameValue.Key.ToString(), propertyNameValue.Value));
      return (AbstractCriterion) conjunction;
    }

    public static NaturalIdentifier NaturalId() => new NaturalIdentifier();

    public static ICriterion Where<T>(Expression<Func<T, bool>> expression)
    {
      return ExpressionProcessor.ProcessExpression<T>(expression);
    }

    public static ICriterion Where(Expression<Func<bool>> expression)
    {
      return ExpressionProcessor.ProcessExpression(expression);
    }

    public static ICriterion WhereNot<T>(Expression<Func<T, bool>> expression)
    {
      return (ICriterion) Restrictions.Not(ExpressionProcessor.ProcessExpression<T>(expression));
    }

    public static ICriterion WhereNot(Expression<Func<bool>> expression)
    {
      return (ICriterion) Restrictions.Not(ExpressionProcessor.ProcessExpression(expression));
    }

    public static LambdaRestrictionBuilder On<T>(Expression<Func<T, object>> expression)
    {
      return new LambdaRestrictionBuilder(ExpressionProcessor.FindMemberProjection(expression.Body));
    }

    public static LambdaRestrictionBuilder On(Expression<Func<object>> expression)
    {
      return new LambdaRestrictionBuilder(ExpressionProcessor.FindMemberProjection(expression.Body));
    }
  }
}
