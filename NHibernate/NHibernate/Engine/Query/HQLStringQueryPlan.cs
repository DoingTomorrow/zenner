// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.HQLStringQueryPlan
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class HQLStringQueryPlan : HQLQueryPlan
  {
    public HQLStringQueryPlan(
      string hql,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
      : this(hql, (string) null, shallow, enabledFilters, factory)
    {
    }

    protected internal HQLStringQueryPlan(
      string hql,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
      : base(hql, HQLStringQueryPlan.CreateTranslators(hql, collectionRole, shallow, enabledFilters, factory))
    {
    }

    private static IQueryTranslator[] CreateTranslators(
      string hql,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
    {
      return factory.Settings.QueryTranslatorFactory.CreateQueryTranslators(hql, collectionRole, shallow, enabledFilters, factory);
    }
  }
}
