// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Structures.ScenarioMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Structures
{
  public sealed class ScenarioMap : ClassMap<Scenario>
  {
    public ScenarioMap()
    {
      this.Table("t_Scenario");
      this.Id((Expression<Func<Scenario, object>>) (s => (object) s.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Scenario, object>>) (s => (object) s.Code)).Not.Nullable();
      this.Map((Expression<Func<Scenario, object>>) (c => c.CelestaCode));
      this.HasMany<ScenarioJobDefinition>((Expression<Func<Scenario, IEnumerable<ScenarioJobDefinition>>>) (c => c.ScenarioJobDefinitions)).KeyColumn("ScenarioId").Cascade.Delete().Inverse();
    }
  }
}
