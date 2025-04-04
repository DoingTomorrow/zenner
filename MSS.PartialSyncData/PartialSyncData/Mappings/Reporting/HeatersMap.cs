// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Reporting.HeatersMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Reporting;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Reporting
{
  public class HeatersMap : ClassMap<Heaters>
  {
    public HeatersMap()
    {
      this.Table("t_Heaters");
      this.Id((Expression<Func<Heaters, object>>) (rtd => (object) rtd.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<Heaters, object>>) (rt => rt.Name));
      this.Map((Expression<Func<Heaters, object>>) (rt => rt.Description));
      this.Map((Expression<Func<Heaters, object>>) (rt => rt.GroupName));
      this.Map((Expression<Func<Heaters, object>>) (rt => (object) rt.EvaluationFactor));
    }
  }
}
