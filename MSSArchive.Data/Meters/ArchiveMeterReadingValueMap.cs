// Decompiled with JetBrains decompiler
// Type: MSSArchive.Data.Mappings.Meters.ArchiveMeterReadingValueMap
// Assembly: MSSArchive.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C71A41A-539A-4545-909E-692571DC7265
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Data.dll

using FluentNHibernate.Mapping;
using MSSArchive.Core.Model.Meters;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSSArchive.Data.Mappings.Meters
{
  public sealed class ArchiveMeterReadingValueMap : ClassMap<ArchiveMeterReadingValue>
  {
    public ArchiveMeterReadingValueMap()
    {
      this.Table("t_ReadingValues");
      this.Id((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.ArchiveEntityId));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (n => (object) n.Id));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.Date));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.Value));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.ValueId));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.MeterId));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => m.MeterSerialNumber)).Index("IX_MeterSerialNumber");
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.CreatedOn));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.ExportedOn)).Nullable();
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.UnitId));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.StorageInterval));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.PhysicalQuantity));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.MeterType));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.CalculationStart));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.Creation));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.Calculation));
      this.Map((Expression<Func<ArchiveMeterReadingValue, object>>) (m => (object) m.ArchiveJobId));
    }
  }
}
