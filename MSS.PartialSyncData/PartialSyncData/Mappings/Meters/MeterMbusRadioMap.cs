// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.MeterMbusRadioMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public sealed class MeterMbusRadioMap : ClassMap<MbusRadioMeter>
  {
    public MeterMbusRadioMap()
    {
      this.Table("t_MeterMbusRadio");
      this.Id((Expression<Func<MbusRadioMeter, object>>) (n => (object) n.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.City));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.Street));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.HouseNumber));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.HouseNumberSupplement));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.ApartmentNumber));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.ZipCode));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.FirstName));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.LastName));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.Location));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => m.RadioSerialNumber));
      this.Map((Expression<Func<MbusRadioMeter, object>>) (m => (object) m.LastChangedOn)).Nullable();
      this.References<Meter>((Expression<Func<MbusRadioMeter, Meter>>) (c => c.Meter), "MeterId").Unique().Not.LazyLoad();
    }
  }
}
