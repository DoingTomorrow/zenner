// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Jobs.ArchiveMssReadingJobMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Jobs;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Jobs
{
  public class ArchiveMssReadingJobMap : ClassMap<ArchiveMssReadingJob>
  {
    public ArchiveMssReadingJobMap()
    {
      this.Table("t_Job");
      this.Id((Expression<Func<ArchiveMssReadingJob, object>>) (m => (object) m.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (n => (object) n.Id));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.StartDate)).Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.EndDate)).Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.IsDeactivated));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (n => (object) n.JobDefinitionId));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.JobDefinitionName)).Not.Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.JobDefinitionEquipmentModel)).Not.Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.JobDefinitionServiceJob));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.JobDefinitionEquipmentParams)).Length(1000).Not.Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.JobDefinitionSystem)).Not.Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.JobDefinitionProfileType)).Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.JobDefinitionStartDate)).Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.JobDefinitionEndDate)).Nullable();
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.JobDefinitionIsDeactivated));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (m => m.JobDefinitionRules));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.JobDefinitionInterval)).Nullable().Length(int.MaxValue);
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (m => m.Scenario));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (m => m.Minomat));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (m => m.RootNode));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.IsUpdate));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.LastExecutionDate));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => jd.ErrorMessage));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (jd => (object) jd.Status));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (m => (object) m.CreatedOn));
      this.Map((Expression<Func<ArchiveMssReadingJob, object>>) (m => (object) m.LastUpdatedOn));
    }
  }
}
