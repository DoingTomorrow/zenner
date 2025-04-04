// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.IQueryOverRestrictionBuilder`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class IQueryOverRestrictionBuilder<TRoot, TSubType>(
    IQueryOver<TRoot, TSubType> root,
    ExpressionProcessor.ProjectionInfo projection) : 
    QueryOverRestrictionBuilderBase<IQueryOver<TRoot, TSubType>, TRoot, TSubType>(root, projection)
  {
    public IQueryOverRestrictionBuilder<TRoot, TSubType> Not
    {
      get
      {
        this.isNot = !this.isNot;
        return this;
      }
    }
  }
}
