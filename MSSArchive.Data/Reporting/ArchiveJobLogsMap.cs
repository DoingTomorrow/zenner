// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Reporting.ArchiveJobLogsMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Jobs;
using MSSArchive.Core.Model.Reporting;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Reporting
{
  public class ArchiveJobLogsMap : ClassMap<ArchiveJobLogs>
  {
    public ArchiveJobLogsMap()
    {
      this.Table("t_JobLogs");
      this.Id((Expression<Func<ArchiveJobLogs, object>>) (l => (object) l.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveJobLogs, object>>) (l => (object) l.Id));
      this.Map((Expression<Func<ArchiveJobLogs, object>>) (jl => jl.JobEntityNumber));
      this.References<ArchiveMssReadingJob>((Expression<Func<ArchiveJobLogs, ArchiveMssReadingJob>>) (m => m.Job), "JobId").Not.LazyLoad();
      this.Map((Expression<Func<ArchiveJobLogs, object>>) (jl => (object) jl.StartDate));
      this.Map((Expression<Func<ArchiveJobLogs, object>>) (jl => (object) jl.CreatedOn));
      this.Map((Expression<Func<ArchiveJobLogs, object>>) (jl => (object) jl.EndDate));
      this.Map((Expression<Func<ArchiveJobLogs, object>>) (jl => (object) jl.Status));
      this.Map((Expression<Func<ArchiveJobLogs, object>>) (jl => jl.Message)).Length(500);
      this.References<ArchiveInformation>((Expression<Func<ArchiveJobLogs, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_JobLogs_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
