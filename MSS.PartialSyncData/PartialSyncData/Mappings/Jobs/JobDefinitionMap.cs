// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Jobs.JobDefinitionMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Jobs
{
  public class JobDefinitionMap : ClassMap<JobDefinition>
  {
    public JobDefinitionMap()
    {
      this.Table("t_JobDefinitions");
      this.Id((Expression<Func<JobDefinition, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.Name)).Not.Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.EquipmentModel)).Not.Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.System)).Not.Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.ServiceJob));
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.EquipmentParams)).Length(1000).Not.Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.ProfileType)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => (object) jd.StartDate)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => (object) jd.EndDate)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => (object) jd.IsDeactivated));
      this.Map((Expression<Func<JobDefinition, object>>) (jd => (object) jd.DueDate)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => (object) jd.Month)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => (object) jd.Day)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => (object) jd.QuarterHour)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.ProfileTypeParams)).Nullable();
      this.Map((Expression<Func<JobDefinition, object>>) (jd => jd.Interval)).Nullable().Length(int.MaxValue);
      this.HasMany<ScenarioJobDefinition>((Expression<Func<JobDefinition, IEnumerable<ScenarioJobDefinition>>>) (c => c.ScenarioJobDefinitions)).KeyColumn("JobDefinitionId").Inverse();
      this.HasMany<MssReadingJob>((Expression<Func<JobDefinition, IEnumerable<MssReadingJob>>>) (c => c.Jobs)).KeyColumn("JobDefinitionId").Inverse();
      this.References<MSS.Core.Model.DataFilters.Filter>((Expression<Func<JobDefinition, MSS.Core.Model.DataFilters.Filter>>) (ur => ur.Filter), "FilterId").Class<MSS.Core.Model.DataFilters.Filter>();
    }
  }
}
