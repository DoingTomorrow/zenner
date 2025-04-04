// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.StructureNodeLinksSerializableDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Structures;
using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class StructureNodeLinksSerializableDTO
  {
    public virtual Guid Id { get; set; }

    public virtual Guid NodeId { get; set; }

    public virtual Guid ParentNodeId { get; set; }

    public virtual Guid RootNodeId { get; set; }

    public virtual StructureTypeEnum StructureType { get; set; }

    public virtual DateTime? StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual int OrderNr { get; set; }

    public virtual DateTime? LastChangedOn { get; set; }
  }
}
