// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.MinomatMeterMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public class MinomatMeterMap : ClassMap<MinomatMeter>
  {
    public MinomatMeterMap()
    {
      this.Table("t_MinomatMeters");
      this.Id((Expression<Func<MinomatMeter, object>>) (c => (object) c.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<MinomatMeter, object>>) (c => (object) c.SignalStrength));
      this.Map((Expression<Func<MinomatMeter, object>>) (c => (object) c.Status));
      this.Map((Expression<Func<MinomatMeter, object>>) (c => (object) c.LastChangedOn)).Nullable();
      this.References<Minomat>((Expression<Func<MinomatMeter, Minomat>>) (c => c.Minomat), "MinomatId");
      this.References<Meter>((Expression<Func<MinomatMeter, Meter>>) (c => c.Meter), "MeterId");
    }
  }
}
