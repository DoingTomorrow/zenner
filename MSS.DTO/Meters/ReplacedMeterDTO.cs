// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.ReplacedMeterDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using System;

#nullable disable
namespace MSS.DTO.Meters
{
  public class ReplacedMeterDTO
  {
    public int Id { get; set; }

    public string SerialNumber { get; set; }

    public DeviceTypeEnum DeviceType { get; set; }

    public DateTime ReplacementDate { get; set; }
  }
}
