// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Sync.ApplicationSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace MSS.DTO.Sync
{
  [DataContract]
  public class ApplicationSync
  {
    public ApplicationSync()
    {
      this.StructureNodeTypeSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.StructureNodeSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.StructureNodeLinksSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.MeterSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.RoomTypeSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.MeasureUnitSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.ChannelSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.ConnectedDeviceTypeSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.LocationSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.TenantSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
      this.ScenarioSyncEnumerable = (IEnumerable<SimpleSyncObject>) new List<SimpleSyncObject>();
    }

    [DataMember]
    public IEnumerable<SimpleSyncObject> StructureNodeTypeSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> StructureNodeSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> StructureNodeLinksSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> MeterSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> RoomTypeSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> MeasureUnitSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> ChannelSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> ConnectedDeviceTypeSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> LocationSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> TenantSyncEnumerable { get; set; }

    [DataMember]
    public IEnumerable<SimpleSyncObject> ScenarioSyncEnumerable { get; set; }
  }
}
