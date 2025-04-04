// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Jobs.JobReadingValuesMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Jobs
{
  public class JobReadingValuesMap : ClassMap<JobReadingValues>
  {
    public JobReadingValuesMap()
    {
      this.Table("t_JobReadingValues");
      this.Id((Expression<Func<JobReadingValues, object>>) (j => (object) j.Id)).GeneratedBy.GuidComb();
      this.References<MssReadingJob>((Expression<Func<JobReadingValues, MssReadingJob>>) (j => j.Job), "JobId").Class<MssReadingJob>();
      this.References<MeterReadingValue>((Expression<Func<JobReadingValues, MeterReadingValue>>) (j => j.ReadingValue), "ReadingValueId").Class<MeterReadingValue>();
    }
  }
}
