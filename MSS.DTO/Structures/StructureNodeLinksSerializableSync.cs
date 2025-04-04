// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.StructureNodeLinksSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Structures;
using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class StructureNodeLinksSerializableSync : ISerializableObject
  {
    public Guid Id { get; set; }

    public Guid NodeId { get; set; }

    public Guid ParentNodeId { get; set; }

    public Guid RootNodeId { get; set; }

    public StructureTypeEnum StructureType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int OrderNr { get; set; }

    public bool IsDuplicate { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
