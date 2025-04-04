// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverFetchBuilderBase`3
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverFetchBuilderBase<TReturn, TRoot, TSubType> where TReturn : IQueryOver<TRoot, TSubType>
  {
    protected TReturn root;
    protected string path;

    protected QueryOverFetchBuilderBase(TReturn root, Expression<Func<TRoot, object>> path)
    {
      this.root = root;
      this.path = ExpressionProcessor.FindMemberExpression(path.Body);
    }

    public TReturn Eager
    {
      get
      {
        this.root.UnderlyingCriteria.SetFetchMode(this.path, FetchMode.Join);
        return this.root;
      }
    }

    public TReturn Lazy
    {
      get
      {
        this.root.UnderlyingCriteria.SetFetchMode(this.path, FetchMode.Select);
        return this.root;
      }
    }

    public TReturn Default
    {
      get
      {
        this.root.UnderlyingCriteria.SetFetchMode(this.path, FetchMode.Default);
        return this.root;
      }
    }
  }
}
