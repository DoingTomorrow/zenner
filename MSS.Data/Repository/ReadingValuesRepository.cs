// Decompiled with JetBrains decompiler
// Type: MSS.Data.Repository.ReadingValuesRepository
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Interfaces;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Repository
{
  public class ReadingValuesRepository(ISession session) : 
    MSS.Data.Repository.Repository<MeterReadingValue>(session),
    IReadingValuesRepository
  {
    public List<OrderReadingValues> LoadOrderReadingValues(List<Guid> readingValuesIds)
    {
      if (readingValuesIds == null || readingValuesIds.Count == 0)
        return (List<OrderReadingValues>) null;
      return this._session.Query<OrderReadingValues>().Where<OrderReadingValues>((Expression<Func<OrderReadingValues, bool>>) (orv => readingValuesIds.Contains(orv.MeterReadingValue.Id))).Fetch<OrderReadingValues, Order>((Expression<Func<OrderReadingValues, Order>>) (orv => orv.OrderObj)).Fetch<OrderReadingValues, MeterReadingValue>((Expression<Func<OrderReadingValues, MeterReadingValue>>) (orv => orv.MeterReadingValue)).ToList<OrderReadingValues>();
    }

    public List<JobReadingValues> LoadJobReadingValues(List<Guid> readingValuesIds)
    {
      if (readingValuesIds == null || readingValuesIds.Count == 0)
        return (List<JobReadingValues>) null;
      return this._session.Query<JobReadingValues>().Where<JobReadingValues>((Expression<Func<JobReadingValues, bool>>) (jrv => readingValuesIds.Contains(jrv.ReadingValue.Id))).Fetch<JobReadingValues, MssReadingJob>((Expression<Func<JobReadingValues, MssReadingJob>>) (jrv => jrv.Job)).Fetch<JobReadingValues, MeterReadingValue>((Expression<Func<JobReadingValues, MeterReadingValue>>) (jrv => jrv.ReadingValue)).ToList<JobReadingValues>();
    }
  }
}
