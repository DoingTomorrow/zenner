// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SimpleExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class SimpleExpression : AbstractCriterion
  {
    private readonly IProjection _projection;
    private readonly string propertyName;
    private readonly object value;
    private bool ignoreCase;
    private readonly string op;
    private static readonly Type[] CallToStringTypes = new Type[2]
    {
      typeof (DateTime),
      typeof (string)
    };

    protected internal SimpleExpression(IProjection projection, object value, string op)
    {
      this._projection = projection;
      this.value = value;
      this.op = op;
    }

    public SimpleExpression(string propertyName, object value, string op)
    {
      this.propertyName = propertyName;
      this.value = value;
      this.op = op;
    }

    public SimpleExpression(string propertyName, object value, string op, bool ignoreCase)
      : this(propertyName, value, op)
    {
      this.ignoreCase = ignoreCase;
    }

    public SimpleExpression IgnoreCase()
    {
      this.ignoreCase = true;
      return this;
    }

    public string PropertyName => this.propertyName;

    public object Value => this.value;

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      SqlString[] simpleExpression = CriterionUtil.GetColumnNamesForSimpleExpression(this.propertyName, this._projection, criteriaQuery, criteria, enabledFilters, (ICriterion) this, this.value);
      Parameter[] array = criteriaQuery.NewQueryParameter(this.GetParameterTypedValue(criteria, criteriaQuery)).ToArray<Parameter>();
      if (this.ignoreCase)
      {
        if (simpleExpression.Length != 1)
          throw new HibernateException("case insensitive expression may only be applied to single-column properties: " + this.propertyName);
        return new SqlStringBuilder(6).Add(criteriaQuery.Factory.Dialect.LowercaseFunction).Add("(").Add(simpleExpression[0]).Add(")").Add(this.Op).Add(((IEnumerable<Parameter>) array).Single<Parameter>()).ToSqlString();
      }
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(4 * simpleExpression.Length);
      for (int index = 0; index < simpleExpression.Length; ++index)
      {
        if (index > 0)
          sqlStringBuilder.Add(" and ");
        sqlStringBuilder.Add(simpleExpression[index]).Add(this.Op).Add(array[index]);
      }
      return sqlStringBuilder.ToSqlString();
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      List<TypedValue> typedValueList = new List<TypedValue>();
      if (this._projection != null)
        typedValueList.AddRange((IEnumerable<TypedValue>) this._projection.GetTypedValues(criteria, criteriaQuery));
      typedValueList.Add(this.GetParameterTypedValue(criteria, criteriaQuery));
      return typedValueList.ToArray();
    }

    public TypedValue GetParameterTypedValue(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      object obj = this.ignoreCase ? (object) this.value.ToString().ToLower() : this.value;
      if (this._projection == null)
        return criteriaQuery.GetTypedValue(criteria, this.propertyName, obj);
      return ((IEnumerable<TypedValue>) CriterionUtil.GetTypedValues(criteriaQuery, criteria, this._projection, (string) null, obj)).Single<TypedValue>();
    }

    public override IProjection[] GetProjections()
    {
      if (this._projection == null)
        return (IProjection[]) null;
      return new IProjection[1]{ this._projection };
    }

    public override string ToString()
    {
      return ((object) this._projection ?? (object) this.propertyName).ToString() + this.Op + this.ValueToStrings();
    }

    protected virtual string Op => this.op;

    private string ValueToStrings()
    {
      if (this.value == null)
        return "null";
      Type type = this.value.GetType();
      return type.IsPrimitive || ((IEnumerable<Type>) SimpleExpression.CallToStringTypes).Any<Type>((Func<Type, bool>) (t => t.IsAssignableFrom(type))) ? this.value.ToString() : ObjectUtils.IdentityToString(this.value);
    }
  }
}
