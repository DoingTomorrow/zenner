// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.Meters.MeterMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.Meters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.Meters
{
  public sealed class MeterMap : ClassMap<Meter>
  {
    public MeterMap()
    {
      this.Table("t_Meter");
      this.Id((Expression<Func<Meter, object>>) (n => (object) n.Id)).GeneratedBy.Assigned();
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.ImpulsValue));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.StartValue));
      this.Map((Expression<Func<Meter, object>>) (m => m.SerialNumber)).Length(200);
      this.Map((Expression<Func<Meter, object>>) (m => m.ShortDeviceNo)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<Meter, object>>) (m => m.CompletDevice)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.EvaluationFactor));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.DeviceType));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.DecimalPlaces));
      this.References<RoomType>((Expression<Func<Meter, RoomType>>) (m => m.Room), "RoomTypeId").Not.LazyLoad();
      this.References<Channel>((Expression<Func<Meter, Channel>>) (m => m.Channel), "ChannelId").Not.LazyLoad();
      this.References<MeasureUnit>((Expression<Func<Meter, MeasureUnit>>) (m => m.ImpulsUnit), "ImpulsUnitId").Not.LazyLoad();
      this.References<MeasureUnit>((Expression<Func<Meter, MeasureUnit>>) (m => m.ReadingUnit), "ReadingUnitId").Not.LazyLoad();
      this.References<ConnectedDeviceType>((Expression<Func<Meter, ConnectedDeviceType>>) (m => m.ConnectedDeviceType), "ConnectedDeviceTypeId").Not.LazyLoad();
      this.HasMany<MeterRadioDetails>((Expression<Func<Meter, IEnumerable<MeterRadioDetails>>>) (m => m.MeterRadioDetailsList)).KeyColumn("MeterId").Cascade.Delete();
      this.HasOne<MbusRadioMeter>((Expression<Func<Meter, MbusRadioMeter>>) (m => m.MbusRadioMeter)).PropertyRef((Expression<Func<MbusRadioMeter, object>>) (x => x.Meter)).Not.LazyLoad().Cascade.Delete();
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.IsDeactivated));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.IsConfigured)).Nullable();
      this.Map((Expression<Func<Meter, object>>) (t => t.CreatedBy));
      this.Map((Expression<Func<Meter, object>>) (t => t.UpdatedBy));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.NrOfImpulses));
      this.Map((Expression<Func<Meter, object>>) (m => m.StartValueImpulses));
      this.Map((Expression<Func<Meter, object>>) (m => m.GMMParameters)).Nullable().Length(int.MaxValue);
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.ConfigDate));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.IsReplaced));
      this.Map((Expression<Func<Meter, object>>) (m => m.AES)).Length(50);
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.PrimaryAddress)).Length(50);
      this.Map((Expression<Func<Meter, object>>) (m => m.Manufacturer)).Length(50);
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.Medium)).Length(50);
      this.Map((Expression<Func<Meter, object>>) (m => m.Generation)).Length(50);
      this.Map((Expression<Func<Meter, object>>) (m => m.InputNumber));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.IsReceived));
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.IsError));
      this.Map((Expression<Func<Meter, object>>) (m => m.GMMAdditionalInfo)).Nullable();
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.LastChangedOn)).Nullable();
      this.Map((Expression<Func<Meter, object>>) (m => (object) m.ReadingEnabled));
    }
  }
}
