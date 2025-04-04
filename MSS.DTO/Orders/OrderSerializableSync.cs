// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Orders.OrderSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Orders
{
  public class OrderSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public string InstallationNumber { get; set; }

    public Guid RootStructureNodeId { get; set; }

    public string Details { get; set; }

    public bool Exported { get; set; }

    public StatusOrderEnum Status { get; set; }

    public string DeviceNumber { get; set; }

    public byte[] TestConfig { get; set; }

    public DateTime DueDate { get; set; }

    public bool IsDeactivated { get; set; }

    public OrderTypeEnum OrderType { get; set; }

    public CloseOrderReasonsEnum CloseOrderReason { get; set; }

    public Guid? LockedBy { get; set; }

    public Guid? FilterId { get; set; }

    public byte[] StructureBytes { get; set; }

    public StructureTypeEnum? StructureType { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
