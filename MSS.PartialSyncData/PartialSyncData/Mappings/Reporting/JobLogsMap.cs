// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Reporting.JobLogsMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Reporting;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Reporting
{
  public class JobLogsMap : ClassMap<JobLogs>
  {
    public JobLogsMap()
    {
      this.Table("t_JobLogs");
      this.Id((Expression<Func<JobLogs, object>>) (jl => (object) jl.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<JobLogs, object>>) (jl => jl.JobEntityNumber));
      this.References<MssReadingJob>((Expression<Func<JobLogs, MssReadingJob>>) (m => m.Job), "JobId").Not.LazyLoad();
      this.Map((Expression<Func<JobLogs, object>>) (jl => (object) jl.StartDate));
      this.Map((Expression<Func<JobLogs, object>>) (jl => (object) jl.CreatedOn));
      this.Map((Expression<Func<JobLogs, object>>) (jl => (object) jl.EndDate));
      this.Map((Expression<Func<JobLogs, object>>) (jl => (object) jl.Status));
      this.Map((Expression<Func<JobLogs, object>>) (jl => jl.Message)).Length(500);
    }
  }
}
