// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Orders.ReadingValuesDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using MSS.Core.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.DTO.Orders
{
  public class ReadingValuesDTO : DTOBase
  {
    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    [DeviceTypeVisibility(DeviceTypeEnum.MinomessMicroRadio3)]
    [DeviceTypeVisibility(DeviceTypeEnum.M7)]
    [DeviceTypeVisibility(DeviceTypeEnum.M6)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5MBus)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5Radio)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusT1)]
    [DeviceTypeVisibility(DeviceTypeEnum.MBus)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCRadio)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataN1)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.WR3)]
    [DeviceTypeVisibility(DeviceTypeEnum.Zelsius)]
    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    [Range(0.0, 1.7976931348623157E+308)]
    public virtual double ActualValue { get; set; }

    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    [DeviceTypeVisibility(DeviceTypeEnum.MinomessMicroRadio3)]
    [DeviceTypeVisibility(DeviceTypeEnum.M7)]
    [DeviceTypeVisibility(DeviceTypeEnum.M6)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusT1)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCRadio)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5MBus)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5Radio)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataN1)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.WR3)]
    [DeviceTypeVisibility(DeviceTypeEnum.Zelsius)]
    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    public virtual double DueDateValue { get; set; }

    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    [DeviceTypeVisibility(DeviceTypeEnum.MinomessMicroRadio3)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusT1)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCRadio)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5MBus)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5Radio)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataN1)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.WR3)]
    [DeviceTypeVisibility(DeviceTypeEnum.Zelsius)]
    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    public virtual Guid UnitId { get; set; }

    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCWMBusT1)]
    [DeviceTypeVisibility(DeviceTypeEnum.EDCRadio)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5MBus)]
    [DeviceTypeVisibility(DeviceTypeEnum.C5Radio)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataN1)]
    [DeviceTypeVisibility(DeviceTypeEnum.MultidataS1)]
    [DeviceTypeVisibility(DeviceTypeEnum.WR3)]
    [DeviceTypeVisibility(DeviceTypeEnum.Zelsius)]
    [DeviceTypeVisibility(DeviceTypeEnum.MinotelContactRadio3)]
    public virtual ValueIdent.ValueIdPart_MeterType Register { get; set; }

    public virtual Guid MeterId { get; set; }

    public virtual Guid OrderId { get; set; }
  }
}
