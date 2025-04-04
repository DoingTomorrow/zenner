// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.DataCollectors.MinomatConnectionLogMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.DataCollectors
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
