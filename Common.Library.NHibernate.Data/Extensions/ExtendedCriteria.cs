// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.ExtendedCriteria
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using NHibernate;
using NHibernate.Criterion;
using System;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public class ExtendedCriteria
  {
    private QueryPropertyAliasesList aliasesList;
    private ICriteria criteria;

    public ExtendedCriteria(ICriteria criteria) => this.criteria = criteria;

    public ExtendedCriteria AddAliases(QueryPropertyAliasesList aliasesList)
    {
      this.aliasesList = aliasesList;
      return this;
    }

    public ExtendedCriteria AddWhere(IWhereClauseInfo whereClauseInfo)
    {
      this.criteria.Add(this.ProcessWhereClauseExpression(whereClauseInfo));
      return this;
    }

    private ICriterion ProcessWhereClauseExpression(IWhereClauseInfo clause)
    {
      switch (clause)
      {
        case SimpleWhereClauseInfo _:
          SimpleWhereClauseInfo simpleWhereClauseInfo = clause as SimpleWhereClauseInfo;
          string str = (string) null;
          if (this.aliasesList != null)
            str = this.aliasesList.GetPropertyForAlias(simpleWhereClauseInfo.PropertyName);
          return QueryExtensions.TranslateExpressionOperation(str ?? simpleWhereClauseInfo.PropertyName, simpleWhereClauseInfo.Operator, simpleWhereClauseInfo.Value);
        case CompositeWhereClauseInfo _:
          CompositeWhereClauseInfo compositeWhereClauseInfo = clause as CompositeWhereClauseInfo;
          if (compositeWhereClauseInfo.LogicalOperator.Equals("or", StringComparison.CurrentCultureIgnoreCase))
            return (ICriterion) Restrictions.Or(this.ProcessWhereClauseExpression(compositeWhereClauseInfo.LeftHandSideClause), this.ProcessWhereClauseExpression(compositeWhereClauseInfo.RightHandSideClause));
          if (compositeWhereClauseInfo.LogicalOperator.Equals("and", StringComparison.CurrentCultureIgnoreCase))
            return (ICriterion) Restrictions.And(this.ProcessWhereClauseExpression(compositeWhereClauseInfo.LeftHandSideClause), this.ProcessWhereClauseExpression(compositeWhereClauseInfo.RightHandSideClause));
          break;
      }
      return (ICriterion) null;
    }

    public ExtendedCriteria AddOrder(OrderClauseInfo orderClauseInfo)
    {
      string propertyName = (string) null;
      if (this.aliasesList != null)
        propertyName = this.aliasesList.GetPropertyForAlias(orderClauseInfo.PropertyName);
      if (propertyName == null)
        propertyName = orderClauseInfo.PropertyName;
      this.criteria.AddOrder(orderClauseInfo.Direction.Equals((object) OrderDirection.Asc) ? Order.Asc(propertyName) : Order.Desc(propertyName));
      return this;
    }

    public ICriteria ProcessCriteria() => this.criteria;

    public ExtendedCriteria SetProjections(ProjectionList list)
    {
      this.criteria.SetProjection((IProjection) list);
      return this;
    }
  }
}
