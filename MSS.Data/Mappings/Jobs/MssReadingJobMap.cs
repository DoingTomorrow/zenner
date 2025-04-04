// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Jobs.MssReadingJobMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Jobs
{
  public class MssReadingJobMap : ClassMap<MssReadingJob>
  {
    public MssReadingJobMap()
    {
      this.Table("t_Job");
      this.Id((Expression<Func<MssReadingJob, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MssReadingJob, object>>) (jd => (object) jd.StartDate)).Nullable();
      this.Map((Expression<Func<MssReadingJob, object>>) (jd => (object) jd.EndDate)).Nullable();
      this.Map((Expression<Func<MssReadingJob, object>>) (jd => (object) jd.IsDeactivated));
      this.Map((Expression<Func<MssReadingJob, object>>) (jd => (object) jd.IsUpdate));
      this.Map((Expression<Func<MssReadingJob, object>>) (jd => (object) jd.LastExecutionDate));
      this.Map((Expression<Func<MssReadingJob, object>>) (jd => jd.ErrorMessage));
      this.Map((Expression<Func<MssReadingJob, object>>) (jd => (object) jd.Status));
      this.Map((Expression<Func<MssReadingJob, object>>) (m => (object) m.CreatedOn));
      this.Map((Expression<Func<MssReadingJob, object>>) (m => (object) m.LastUpdatedOn));
      this.References<JobDefinition>((Expression<Func<MssReadingJob, JobDefinition>>) (ur => ur.JobDefinition), "JobDefinitionId").Class<JobDefinition>();
      this.References<Scenario>((Expression<Func<MssReadingJob, Scenario>>) (ur => ur.Scenario), "ScenarioId").Class<Scenario>();
      this.References<Minomat>((Expression<Func<MssReadingJob, Minomat>>) (ur => ur.Minomat), "MinomatId").Class<Minomat>();
      this.References<StructureNode>((Expression<Func<MssReadingJob, StructureNode>>) (ur => ur.RootNode), "StructureNodeId").Class<StructureNode>();
    }
  }
}
