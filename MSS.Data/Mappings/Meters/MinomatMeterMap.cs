// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.MinomatMeterMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public class MinomatMeterMap : ClassMap<MinomatMeter>
  {
    public MinomatMeterMap()
    {
      this.Table("t_MinomatMeters");
      this.Id((Expression<Func<MinomatMeter, object>>) (c => (object) c.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<MinomatMeter, object>>) (c => (object) c.SignalStrength));
      this.Map((Expression<Func<MinomatMeter, object>>) (c => (object) c.Status));
      this.Map((Expression<Func<MinomatMeter, object>>) (c => (object) c.LastChangedOn)).Nullable();
      this.References<Minomat>((Expression<Func<MinomatMeter, Minomat>>) (c => c.Minomat), "MinomatId");
      this.References<Meter>((Expression<Func<MinomatMeter, Meter>>) (c => c.Meter), "MeterId");
    }
  }
}
