// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.StructureNodeSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class StructureNodeSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public Guid EntityId { get; set; }

    public string EntityName { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid NodeTypeId { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
