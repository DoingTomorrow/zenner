// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.StructureNodeEquipmentSettingsSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class StructureNodeEquipmentSettingsSerializableSync : ISerializableObject
  {
    public virtual Guid Id { get; set; }

    public virtual Guid StructureNodeId { get; set; }

    public virtual string EquipmentName { get; set; }

    public virtual string EquipmentParams { get; set; }

    public virtual string SystemName { get; set; }

    public virtual string ScanMode { get; set; }

    public virtual string ScanParams { get; set; }

    public virtual string ReadingProfileName { get; set; }

    public virtual DateTime? LastChangedOn { get; set; }
  }
}
