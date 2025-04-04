// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterDTO : DTOBase
  {
    private double? _evaluationFactor;

    public Guid Id { get; set; }

    public string SerialNumber { get; set; }

    public string ShortDeviceNo { get; set; }

    public string CompletDevice { get; set; }

    public DeviceTypeEnum DeviceType { get; set; }

    public RoomType Room { get; set; }

    public double? StartValue { get; set; }

    public MeasureUnit ReadingUnit { get; set; }

    public double? DecimalPlaces { get; set; }

    public double? ImpulsValue { get; set; }

    public MeasureUnit ImpulsUnit { get; set; }

    public Channel Channel { get; set; }

    public ConnectedDeviceType ConnectedDeviceType { get; set; }

    public double? EvaluationFactor
    {
      get => this._evaluationFactor;
      set
      {
        this._evaluationFactor = value;
        this.OnPropertyChanged(nameof (EvaluationFactor));
      }
    }

    public bool IsDeactivated { get; set; }

    public byte[] GMMParameters { get; set; }

    public DateTime? ConfigDate { get; set; }

    public bool? IsConfigured { get; set; }

    public Guid? ReplacedMeterId { get; set; }

    public List<MeterReplacementHistorySerializableDTO> MeterReplacementHistoryList { get; set; }

    public bool IsReplaced { get; set; }

    public string AES { get; set; }

    public int? PrimaryAddress { get; set; }

    public string Manufacturer { get; set; }

    public DeviceMediumEnum? Medium { get; set; }

    public string Generation { get; set; }

    public string TenantNo { get; set; }

    public MeterMBusStateEnum? MBusStateEnum { get; set; }

    public string InputNumber { get; set; }

    public bool IsReceived { get; set; }

    public bool IsError { get; set; }

    public MbusRadioMeter MbusRadioMeter { get; set; }

    public List<MSS.Core.Model.Meters.MeterRadioDetails> MeterRadioDetails { get; set; }

    public string GMMAdditionalInfo { get; set; }

    public DateTime? LastChangedOn { get; set; }

    public bool ReadingEnabled { get; set; }

    public List<MinomatMeter> MinomatMeters { get; set; }
  }
}
