// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Query.FilterQueryPlan
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine.Query
{
  [Serializable]
  public class FilterQueryPlan : HQLStringQueryPlan
  {
    private readonly string collectionRole;

    public FilterQueryPlan(
      string hql,
      string collectionRole,
      bool shallow,
      IDictionary<string, IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
      : base(hql, collectionRole, shallow, enabledFilters, factory)
    {
      this.collectionRole = collectionRole;
    }

    public string CollectionRole => this.collectionRole;
  }
}
