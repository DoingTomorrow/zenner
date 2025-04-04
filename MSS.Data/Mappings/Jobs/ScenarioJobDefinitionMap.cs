// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Jobs.ScenarioJobDefinitionMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Jobs
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
