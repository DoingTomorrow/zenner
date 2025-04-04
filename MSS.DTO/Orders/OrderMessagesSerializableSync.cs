// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Orders.OrderMessagesSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Utils;
using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Orders
{
  public class OrderMessagesSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid MeterId { get; set; }

    public MessageLevelsEnum Level { get; set; }

    public DateTime Timepoint { get; set; }

    public string Message { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
