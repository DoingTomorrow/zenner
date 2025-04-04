// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Meters.ArchiveMeterReadingValue
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Meters
{
  public class ArchiveMeterReadingValue
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual Guid MeterId { get; set; }

    public virtual string MeterSerialNumber { get; set; }

    public virtual DateTime Date { get; set; }

    public virtual double Value { get; set; }

    public virtual long ValueId { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime? ExportedOn { get; set; }

    public virtual Guid UnitId { get; set; }

    public virtual long PhysicalQuantity { get; set; }

    public virtual long MeterType { get; set; }

    public virtual long CalculationStart { get; set; }

    public virtual long Creation { get; set; }

    public virtual long Calculation { get; set; }

    public virtual long StorageInterval { get; set; }

    [ExcludeProperty]
    public virtual Guid ArchiveJobId { get; set; }
  }
}
