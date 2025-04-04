// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Meters.ArchiveMeterMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Meters
{
  public sealed class ArchiveMeterMap : ClassMap<ArchiveMeter>
  {
    public ArchiveMeterMap()
    {
      this.Table("t_Meter");
      this.Id((Expression<Func<ArchiveMeter, object>>) (t => (object) t.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveMeter, object>>) (t => (object) t.Id));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => (object) m.StartValue));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => m.SerialNumber)).Length(200);
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => m.ShortDeviceNo)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => m.CompletDevice)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => (object) m.DeviceType));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => (object) m.DecimalPlaces));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => m.RoomTypeCode));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => m.ChannelCode));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => m.ReadingUnitCode));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => m.ConnectedDeviceTypeCode));
      this.Map((Expression<Func<ArchiveMeter, object>>) (m => (object) m.IsDeactivated));
      this.References<ArchiveInformation>((Expression<Func<ArchiveMeter, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_Meter_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
