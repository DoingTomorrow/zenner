// Decompiled with JetBrains decompiler
// Type: Common.Library.NHibernate.Data.Extensions.DataPagingInfo
// Assembly: Common.Library.NHibernate.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8E2D87B3-234F-4936-9817-E8F0EDC71AA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.NHibernate.Data.dll

using System.Collections.Generic;

#nullable disable
namespace Common.Library.NHibernate.Data.Extensions
{
  public struct DataPagingInfo
  {
    public const int ALL_RECORDS = -1;

    public int Page { get; set; }

    public int PageSize { get; set; }

    public IEnumerable<OrderClauseInfo> OrderClauses { get; set; }

    public IEnumerable<IWhereClauseInfo> WhereClauses { get; set; }
  }
}
