// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.RowCountInt64Projection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public class RowCountInt64Projection : RowCountProjection
  {
    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return new IType[1]{ (IType) NHibernateUtil.Int64 };
    }
  }
}
