// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Meters.ArchiveMeter
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSS.Core.Model.Meters;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSSArchive.Core.Model.Meters
{
  public class ArchiveMeter
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual string SerialNumber { get; set; }

    public virtual string ShortDeviceNo { get; set; }

    public virtual string CompletDevice { get; set; }

    public virtual DeviceTypeEnum DeviceType { get; set; }

    public virtual string RoomTypeCode { get; set; }

    public virtual double? StartValue { get; set; }

    public virtual string ReadingUnitCode { get; set; }

    public virtual double? DecimalPlaces { get; set; }

    public virtual string ChannelCode { get; set; }

    public virtual string ConnectedDeviceTypeCode { get; set; }

    public virtual bool IsDeactivated { get; set; }

    [ExcludeProperty]
    public virtual IList<ArchiveMeterRadioDetails> MeterRadioDetailsList { get; set; }

    [ExcludeProperty]
    public virtual MbusRadioMeter MbusRadioMeter { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
