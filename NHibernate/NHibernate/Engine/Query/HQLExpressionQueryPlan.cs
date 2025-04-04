// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.HQLExpressionQueryPlan
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql;
using NHibernate.Hql.Ast.ANTLR;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class HQLExpressionQueryPlan : HQLQueryPlan, IQueryExpressionPlan, IQueryPlan
  {
    public IQueryExpression QueryExpression { get; protected set; }

    private HQLExpressionQueryPlan(HQLQueryPlan source, IQueryExpression newQueryExpression)
      : base(source)
    {
      this.QueryExpression = newQueryExpression;
    }

    internal HQLExpressionQueryPlan Copy(IQueryExpression newExpression)
    {
      return new HQLExpressionQueryPlan((HQLQueryPlan) this, newExpression);
    }

    public HQLExpressionQueryPlan(
      string expressionStr,
      IQueryExpression queryExpression,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
      : this(expressionStr, queryExpression, (string) null, shallow, enabledFilters, factory)
    {
    }

    protected internal HQLExpressionQueryPlan(
      string expressionStr,
      IQueryExpression queryExpression,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
      : base(expressionStr, HQLExpressionQueryPlan.CreateTranslators(expressionStr, queryExpression, collectionRole, shallow, enabledFilters, factory))
    {
      this.QueryExpression = queryExpression;
    }

    private static IQueryTranslator[] CreateTranslators(
      string expressionStr,
      IQueryExpression queryExpression,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
    {
      return new ASTQueryTranslatorFactory().CreateQueryTranslators(expressionStr, queryExpression, collectionRole, shallow, enabledFilters, factory);
    }
  }
}
