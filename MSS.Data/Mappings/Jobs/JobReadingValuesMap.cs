// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Jobs.JobReadingValuesMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Jobs
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
