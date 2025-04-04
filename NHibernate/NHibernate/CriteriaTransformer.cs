// Decompiled with JetBrains decompiler
// Type: NHibernate.CriteriaTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Impl;

#nullable disable
namespace NHibernate
{
  public static class CriteriaTransformer
  {
    public static DetachedCriteria TransformToRowCount(DetachedCriteria criteria)
    {
      return new DetachedCriteria(CriteriaTransformer.TransformToRowCount((CriteriaImpl) criteria.GetCriteriaImpl().Clone()));
    }

    public static ICriteria TransformToRowCount(ICriteria criteria)
    {
      return (ICriteria) CriteriaTransformer.TransformToRowCount((CriteriaImpl) criteria.Clone());
    }

    private static CriteriaImpl TransformToRowCount(CriteriaImpl criteria)
    {
      criteria.ClearOrders();
      criteria.SetFirstResult(0).SetMaxResults(RowSelection.NoValue).SetProjection(Projections.RowCount());
      return criteria;
    }

    public static DetachedCriteria Clone(DetachedCriteria criteria)
    {
      return new DetachedCriteria((CriteriaImpl) criteria.GetCriteriaImpl().Clone());
    }

    public static ICriteria Clone(ICriteria criteria) => (ICriteria) criteria.Clone();
  }
}
