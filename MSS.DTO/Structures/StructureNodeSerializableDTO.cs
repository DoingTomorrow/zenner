// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.StructureNodeSerializableDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class StructureNodeSerializableDTO
  {
    public virtual Guid Id { get; set; }

    public virtual Guid EntityId { get; set; }

    public virtual string EntityName { get; set; }

    public virtual string Name { get; set; }

    public virtual string Description { get; set; }

    public virtual Guid NodeType { get; set; }

    public virtual DateTime? StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual DateTime? LastChangedOn { get; set; }
  }
}
