// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.IQueryOverLockBuilder`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class IQueryOverLockBuilder<TRoot, TSubType>(
    IQueryOver<TRoot, TSubType> root,
    Expression<Func<object>> alias) : 
    QueryOverLockBuilderBase<IQueryOver<TRoot, TSubType>, TRoot, TSubType>(root, alias)
  {
  }
}
