// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterReplacementHistorySerializableDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using System;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterReplacementHistorySerializableDTO
  {
    public Guid CurrentMeterId { get; set; }

    public Guid ReplacedMeterId { get; set; }

    public string ReplacedMeterSerialNumber { get; set; }

    public DeviceTypeEnum ReplacedMeterDeviceType { get; set; }

    public DateTime ReplacementDate { get; set; }

    public Guid ReplacedByUserId { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
