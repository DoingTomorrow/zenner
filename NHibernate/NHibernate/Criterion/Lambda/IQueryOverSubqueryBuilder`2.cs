// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.Lambda.IQueryOverSubqueryBuilder`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Criterion.Lambda
{
  public class IQueryOverSubqueryBuilder<TRoot, TSubType>(IQueryOver<TRoot, TSubType> root) : 
    QueryOverSubqueryBuilderBase<IQueryOver<TRoot, TSubType>, TRoot, TSubType, IQueryOverSubqueryPropertyBuilder<TRoot, TSubType>>(root)
  {
  }
}
