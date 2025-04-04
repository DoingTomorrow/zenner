// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Reporting.JobLogsMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Reporting;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Reporting
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
