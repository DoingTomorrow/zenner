// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.Structure
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.Meters;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.DTO.Structures
{
  public class Structure
  {
    public Guid RootNodeId { get; set; }

    public List<StructureNodeLinks> Links { get; set; }

    public List<StructureNode> Nodes { get; set; }

    public List<Meter> Meters { get; set; }

    public List<Location> Locations { get; set; }

    public List<Tenant> Tenants { get; set; }

    public List<Minomat> Minomats { get; set; }

    public List<MeterReplacementHistorySerializableDTO> MeterReplacementHistory { get; set; }
  }
}
