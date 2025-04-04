// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverLockBuilderBase`3
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverLockBuilderBase<TReturn, TRoot, TSubType> where TReturn : IQueryOver<TRoot, TSubType>
  {
    protected TReturn root;
    protected string alias;

    protected QueryOverLockBuilderBase(TReturn root, Expression<Func<object>> alias)
    {
      this.root = root;
      if (alias == null)
        return;
      this.alias = ExpressionProcessor.FindMemberExpression(alias.Body);
    }

    private void SetLockMode(LockMode lockMode)
    {
      if (this.alias != null)
        this.root.UnderlyingCriteria.SetLockMode(this.alias, lockMode);
      else
        this.root.UnderlyingCriteria.SetLockMode(lockMode);
    }

    public TReturn Force
    {
      get
      {
        this.SetLockMode(LockMode.Force);
        return this.root;
      }
    }

    public TReturn None
    {
      get
      {
        this.SetLockMode(LockMode.None);
        return this.root;
      }
    }

    public TReturn Read
    {
      get
      {
        this.SetLockMode(LockMode.Read);
        return this.root;
      }
    }

    public TReturn Upgrade
    {
      get
      {
        this.SetLockMode(LockMode.Upgrade);
        return this.root;
      }
    }

    public TReturn UpgradeNoWait
    {
      get
      {
        this.SetLockMode(LockMode.UpgradeNoWait);
        return this.root;
      }
    }

    public TReturn Write
    {
      get
      {
        this.SetLockMode(LockMode.Write);
        return this.root;
      }
    }
  }
}
