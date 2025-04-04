// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Meters.ArchiveMbusRadioMeterMap
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
  public class ArchiveMbusRadioMeterMap : ClassMap<ArchiveMbusRadioMeter>
  {
    public ArchiveMbusRadioMeterMap()
    {
      this.Table("t_MeterMbusRadio");
      this.Id((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => (object) t.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => (object) t.Id));
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.City)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.Street)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.HouseNumber)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.HouseNumberSupplement)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.ApartmentNumber)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.ZipCode)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.FirstName)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.LastName)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.Location)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (t => t.RadioSerialNumber)).Length((int) byte.MaxValue);
      this.Map((Expression<Func<ArchiveMbusRadioMeter, object>>) (m => (object) m.MeterId));
      this.References<ArchiveInformation>((Expression<Func<ArchiveMbusRadioMeter, ArchiveInformation>>) (m => m.ArchiveInformation)).Column("t_MeterMbusRadio_ArchiveInformationId").Nullable().Not.LazyLoad();
    }
  }
}
