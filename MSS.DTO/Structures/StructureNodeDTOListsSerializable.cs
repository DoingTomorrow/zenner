// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.StructureNodeDTOListsSerializable
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Meters;
using System.Collections.Generic;

#nullable disable
namespace MSS.DTO.Structures
{
  public class StructureNodeDTOListsSerializable
  {
    public IEnumerable<StructureNodeSerializableDTO> structureNodeList { get; set; }

    public IEnumerable<StructureNodeLinksSerializableDTO> structureNodesLinksList { get; set; }

    public IEnumerable<StructureNodeTypeSerializableDTO> strucureNodeTypesEnumerable { get; set; }

    public IEnumerable<StructureNodeSerializableDTO> structureNodesResultEnumerable { get; set; }

    public IEnumerable<MeterSerializableDTO> meterList { get; set; }

    public IEnumerable<LocationSerializableDTO> locationList { get; set; }

    public IEnumerable<TenantSerializableDTO> tenantList { get; set; }
  }
}
