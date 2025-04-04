// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Orders.OrderSerializableStructure
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.DTO.Meters;
using MSS.DTO.Structures;
using System.Collections.Generic;

#nullable disable
namespace MSS.DTO.Orders
{
  public class OrderSerializableStructure
  {
    public List<StructureNodeSerializableDTO> structureNodeList { get; set; }

    public List<StructureNodeLinksSerializableDTO> structureNodesLinksList { get; set; }

    public List<StructureNodeTypeSerializableDTO> strucureNodeTypesEnumerable { get; set; }

    public List<StructureNodeSerializableDTO> structureNodesResultEnumerable { get; set; }

    public List<MeterSerializableDTO> meterList { get; set; }

    public List<LocationSerializableDTO> locationList { get; set; }

    public List<TenantSerializableDTO> tenantList { get; set; }

    public List<MinomatSerializableDTO> minomatList { get; set; }

    public List<MeterReplacementHistorySerializableDTO> meterReplacementHistoryList { get; set; }
  }
}
