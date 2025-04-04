// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.DataCollectors.MinomatConnectionLogMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.DataCollectors
{
  public class MinomatConnectionLogMap : ClassMap<MinomatConnectionLog>
  {
    public MinomatConnectionLogMap()
    {
      this.Table("t_MinomatConnectionLog");
      this.Id((Expression<Func<MinomatConnectionLog, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => m.ChallengeKey));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => m.GsmID));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => (object) m.IsTestConnection));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => m.MinolId));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => m.SessionKey));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => (object) m.TimePoint));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => (object) m.MinomatId));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => m.ScenarioNumber));
      this.Map((Expression<Func<MinomatConnectionLog, object>>) (m => (object) m.LastDCUConnectionMDMExportOn));
    }
  }
}
