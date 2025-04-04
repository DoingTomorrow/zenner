// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterSerializableDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using System;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterSerializableDTO
  {
    public Guid Id { get; set; }

    public string SerialNumber { get; set; }

    public string ShortDeviceNo { get; set; }

    public string CompletDevice { get; set; }

    public DeviceTypeEnum DeviceType { get; set; }

    public Guid RoomTypeId { get; set; }

    public string RoomTypeCode { get; set; }

    public double? StartValue { get; set; }

    public Guid ReadingUnitId { get; set; }

    public string ReadingUnitCelestaCode { get; set; }

    public double? ImpulsValue { get; set; }

    public Guid ImpulsUnitId { get; set; }

    public string ImpulsUnitCelestaCode { get; set; }

    public Guid ChannelId { get; set; }

    public Guid ConnectedDeviceTypeId { get; set; }

    public double? EvaluationFactor { get; set; }

    public bool IsDeactivated { get; set; }

    public bool? IsConfigured { get; set; }

    public ReadingValueStatusEnum? Status { get; set; }

    public bool IsReplaced { get; set; }

    public int? PrimaryAddress { get; set; }

    public string Manufacturer { get; set; }

    public DeviceMediumEnum? Medium { get; set; }

    public string Generation { get; set; }

    public string InputNumber { get; set; }

    public string DeviceInfo { get; set; }

    public Guid MbusRadioMeterId { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string HouseNumber { get; set; }

    public string HouseNumberSupplement { get; set; }

    public string ApartmentNumber { get; set; }

    public string ZipCode { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Location { get; set; }

    public string RadioSerialNumber { get; set; }

    public DateTime? LastChangedOn { get; set; }

    public bool ReadingEnabled { get; set; }
  }
}
