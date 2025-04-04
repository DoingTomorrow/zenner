// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverOrderBuilderBase`3
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverOrderBuilderBase<TReturn, TRoot, TSubType> where TReturn : IQueryOver<TRoot, TSubType>
  {
    protected TReturn root;
    protected LambdaExpression path;
    protected bool isAlias;
    protected ExpressionProcessor.ProjectionInfo projection;

    protected QueryOverOrderBuilderBase(TReturn root, Expression<Func<TSubType, object>> path)
    {
      this.root = root;
      this.path = (LambdaExpression) path;
      this.isAlias = false;
    }

    protected QueryOverOrderBuilderBase(TReturn root, Expression<Func<object>> path, bool isAlias)
    {
      this.root = root;
      this.path = (LambdaExpression) path;
      this.isAlias = isAlias;
    }

    protected QueryOverOrderBuilderBase(TReturn root, ExpressionProcessor.ProjectionInfo projection)
    {
      this.root = root;
      this.projection = projection;
    }

    private void AddOrder(
      Func<string, Order> orderStringDelegate,
      Func<IProjection, Order> orderProjectionDelegate)
    {
      if (this.projection != null)
        this.root.UnderlyingCriteria.AddOrder(this.projection.CreateOrder(orderStringDelegate, orderProjectionDelegate));
      else if (this.isAlias)
        this.root.UnderlyingCriteria.AddOrder(ExpressionProcessor.ProcessOrder(this.path, orderStringDelegate));
      else
        this.root.UnderlyingCriteria.AddOrder(ExpressionProcessor.ProcessOrder(this.path, orderStringDelegate, orderProjectionDelegate));
    }

    public TReturn Asc
    {
      get
      {
        this.AddOrder(new Func<string, Order>(Order.Asc), new Func<IProjection, Order>(Order.Asc));
        return this.root;
      }
    }

    public TReturn Desc
    {
      get
      {
        this.AddOrder(new Func<string, Order>(Order.Desc), new Func<IProjection, Order>(Order.Desc));
        return this.root;
      }
    }
  }
}
