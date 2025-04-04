// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Minomat.MinomatMetersSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Minomat
{
  public class MinomatMetersSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public int SignalStrength { get; set; }

    public MeterStatusEnum? Status { get; set; }

    public Guid? MinomatId { get; set; }

    public Guid? MeterId { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
