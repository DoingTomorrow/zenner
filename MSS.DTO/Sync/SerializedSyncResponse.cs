// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Sync.SerializedSyncResponse
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Meters;
using MSS.DTO.Minomat;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using System.Collections.Generic;

#nullable disable
namespace MSS.DTO.Sync
{
  public class SerializedSyncResponse
  {
    public List<OrderSerializableSync> SerializedOrders { get; set; } = (List<OrderSerializableSync>) null;

    public List<OrderUserSerializableSync> SerializedOrderUser { get; set; } = (List<OrderUserSerializableSync>) null;

    public List<OrderMessagesSerializableSync> SerializedOrderMessages { get; set; } = (List<OrderMessagesSerializableSync>) null;

    public List<StructureNodeSerializableSync> SerializedStructureNode { get; set; } = (List<StructureNodeSerializableSync>) null;

    public List<StructureNodeLinksSerializableSync> SerializedStructureNodeLinks { get; set; } = (List<StructureNodeLinksSerializableSync>) null;

    public List<StructureNodeEquipmentSettingsSerializableSync> SerializedStructureNodeEquipmentSettings { get; set; } = (List<StructureNodeEquipmentSettingsSerializableSync>) null;

    public List<MeterSerializableSync> SerializedMeter { get; set; } = (List<MeterSerializableSync>) null;

    public List<MeterRadioDetailsSerializableSync> SerializedMeterRadioDetails { get; set; } = (List<MeterRadioDetailsSerializableSync>) null;

    public List<MeterReplacementHistorySerializableSync> SerializedMeterReplacementHistory { get; set; } = (List<MeterReplacementHistorySerializableSync>) null;

    public List<MeterMBusRadioSerializableSync> SerializedMeterMBusRadio { get; set; } = (List<MeterMBusRadioSerializableSync>) null;

    public List<NoteSerializableSync> SerializedNote { get; set; } = (List<NoteSerializableSync>) null;

    public List<MinomatSerializableSync> SerializedMinomat { get; set; } = (List<MinomatSerializableSync>) null;

    public List<MinomatRadioDetailsSerializableSync> SerializedMinomatRadioDetails { get; set; } = (List<MinomatRadioDetailsSerializableSync>) null;

    public List<MinomatMetersSerializableSync> SerializedMinomatMeters { get; set; } = (List<MinomatMetersSerializableSync>) null;

    public List<TenantSerializableSync> SerializedTenant { get; set; } = (List<TenantSerializableSync>) null;

    public List<LocationSerializableSync> SerializedLocation { get; set; } = (List<LocationSerializableSync>) null;

    public List<MinomatSerializableSync> SerializedMinomatPool { get; set; } = (List<MinomatSerializableSync>) null;
  }
}
