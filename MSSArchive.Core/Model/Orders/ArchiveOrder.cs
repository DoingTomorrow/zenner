// Decompiled with JetBrains decompiler
// Type: MSSArchive.Core.Model.Orders.ArchiveOrder
// Assembly: MSSArchive.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 12C35498-930F-45CB-8642-1B6443FD9A3F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSSArchive.Core.dll

using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSSArchive.Core.Model.Archiving;
using MSSArchive.Core.Utils;
using System;

#nullable disable
namespace MSSArchive.Core.Model.Orders
{
  public class ArchiveOrder
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual string InstallationNumber { get; set; }

    public virtual Guid RootStructureNodeId { get; set; }

    public virtual string Details { get; set; }

    public virtual bool Exported { get; set; }

    public virtual StatusOrderEnum Status { get; set; }

    public virtual string DeviceNumber { get; set; }

    public virtual byte[] TestConfig { get; set; }

    public virtual DateTime DueDate { get; set; }

    public virtual bool IsDeactivated { get; set; }

    public virtual OrderTypeEnum OrderType { get; set; }

    public virtual CloseOrderReasonsEnum CloseOrderReason { get; set; }

    public virtual Guid? LockedBy { get; set; }

    public virtual byte[] StructureBytes { get; set; }

    public virtual StructureTypeEnum? StructureType { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime? LastUpdatedOn { get; set; }

    public virtual string OrderRules { get; set; }

    public virtual string OrderUsers { get; set; }

    [ExcludeProperty]
    public virtual ArchiveInformation ArchiveInformation { get; set; }
  }
}
