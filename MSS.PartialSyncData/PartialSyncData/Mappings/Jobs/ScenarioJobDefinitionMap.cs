// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Jobs.ScenarioJobDefinitionMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Jobs
{
  public class ScenarioJobDefinitionMap : ClassMap<ScenarioJobDefinition>
  {
    public ScenarioJobDefinitionMap()
    {
      this.Table("t_ScenarioJobDefinitions");
      this.Id((Expression<Func<ScenarioJobDefinition, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.References<Scenario>((Expression<Func<ScenarioJobDefinition, Scenario>>) (ur => ur.Scenario), "ScenarioId").Class<Scenario>();
      this.References<JobDefinition>((Expression<Func<ScenarioJobDefinition, JobDefinition>>) (ur => ur.JobDefinition), "JobDefinitionId").Class<JobDefinition>();
    }
  }
}
