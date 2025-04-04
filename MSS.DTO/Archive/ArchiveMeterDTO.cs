// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Archive.ArchiveMeterDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using System;

#nullable disable
namespace MSS.DTO.Archive
{
  public class ArchiveMeterDTO : DTOBase
  {
    public Guid Id { get; set; }

    public string SerialNumber { get; set; }

    public string ShortDeviceNo { get; set; }

    public string CompletDevice { get; set; }

    public DeviceTypeEnum DeviceType { get; set; }

    public string RoomTypeCode { get; set; }

    public double? StartValue { get; set; }

    public string ReadingUnitCode { get; set; }

    public double? DecimalPlaces { get; set; }

    public string ChannelCode { get; set; }

    public string ConnectedDeviceTypeCode { get; set; }

    public bool IsDeactivated { get; set; }
  }
}
