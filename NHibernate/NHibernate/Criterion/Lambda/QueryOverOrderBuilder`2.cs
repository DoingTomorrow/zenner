// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.QueryOverOrderBuilder`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class QueryOverOrderBuilder<TRoot, TSubType> : 
    QueryOverOrderBuilderBase<QueryOver<TRoot, TSubType>, TRoot, TSubType>
  {
    public QueryOverOrderBuilder(
      QueryOver<TRoot, TSubType> root,
      Expression<Func<TSubType, object>> path)
      : base(root, path)
    {
    }

    public QueryOverOrderBuilder(
      QueryOver<TRoot, TSubType> root,
      Expression<Func<object>> path,
      bool isAlias)
      : base(root, path, isAlias)
    {
    }

    public QueryOverOrderBuilder(
      QueryOver<TRoot, TSubType> root,
      ExpressionProcessor.ProjectionInfo projection)
      : base(root, projection)
    {
    }
  }
}
