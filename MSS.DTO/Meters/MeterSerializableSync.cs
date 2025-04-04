// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public string SerialNumber { get; set; }

    public string CompletDevice { get; set; }

    public DeviceTypeEnum DeviceType { get; set; }

    public Guid? RoomId { get; set; }

    public double? StartValue { get; set; }

    public Guid? ReadingUnitId { get; set; }

    public double? DecimalPlaces { get; set; }

    public double? ImpulsValue { get; set; }

    public Guid? ImpulsUnitId { get; set; }

    public Guid? ChannelId { get; set; }

    public Guid? ConnectedDeviceTypeId { get; set; }

    public double? EvaluationFactor { get; set; }

    public bool IsDeactivated { get; set; }

    public bool? IsConfigured { get; set; }

    public byte[] GMMParameters { get; set; }

    public DateTime? ConfigDate { get; set; }

    public string CreatedBy { get; set; }

    public string UpdatedBy { get; set; }

    public string StartValueImpulses { get; set; }

    public int NrOfImpulses { get; set; }

    public bool IsReplaced { get; set; }

    public string AES { get; set; }

    public int? PrimaryAddress { get; set; }

    public string Manufacturer { get; set; }

    public DeviceMediumEnum? Medium { get; set; }

    public string Generation { get; set; }

    public string InputNumber { get; set; }

    public bool IsReceived { get; set; }

    public bool IsError { get; set; }

    public DateTime? LastChangedOn { get; set; }

    public string GMMAdditionalInfo { get; set; }

    public bool ReadingEnabled { get; set; }
  }
}
