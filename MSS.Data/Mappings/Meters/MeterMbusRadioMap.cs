// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.Meters.MeterMbusRadioMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.Meters
{
  public sealed class MeterMbusRadioMap : ClassMap<MbusRadioMeter>
  {
    public MeterMbusRadioMap()
    {
      this.Table("t_MeterMbusRadio");
      this.Id((Expression<Func<MbusRadioMeter, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
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
