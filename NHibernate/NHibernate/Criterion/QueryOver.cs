// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.QueryOver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public abstract class QueryOver
  {
    protected ICriteria criteria;
    protected CriteriaImpl impl;

    public static QueryOver<T, T> Of<T>() => new QueryOver<T, T>();

    public static QueryOver<T, T> Of<T>(Expression<Func<T>> alias) => new QueryOver<T, T>(alias);

    public static QueryOver<T, T> Of<T>(string entityName) => new QueryOver<T, T>(entityName);

    public static QueryOver<T, T> Of<T>(string entityName, Expression<Func<T>> alias)
    {
      return new QueryOver<T, T>(entityName, alias);
    }

    public ICriteria UnderlyingCriteria => this.criteria;

    public ICriteria RootCriteria => (ICriteria) this.impl;

    public DetachedCriteria DetachedCriteria
    {
      get => new DetachedCriteria(this.impl, (ICriteria) this.impl);
    }
  }
}
