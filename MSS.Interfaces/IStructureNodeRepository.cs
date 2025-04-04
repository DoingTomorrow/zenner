// Decompiled with JetBrains decompiler
// Type: MSS.Interfaces.IStructureNodeRepository
// Assembly: MSS.Interfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 178808BA-C10E-4054-B175-D79F79744EFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Interfaces.dll

using MSS.Core.Model.Structures;
using MSS.DTO.Structures;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Interfaces
{
  public interface IStructureNodeRepository
  {
    IList<StructureNode> SearchStructureNodes(string text);

    Structure LoadStructure(Guid rootNodeId);

    Dictionary<Guid, Location> GetLocationsForMinomats();

    Dictionary<Guid, Location> GetLocationsForMeters(List<Guid> meterIds);

    List<StructureNodeLinks> GetStructureLinksWithNodes(
      StructureTypeEnum? structureType,
      Guid rootNodeId,
      out Dictionary<Guid, object> entitiesDictionary,
      out List<string> duplicateMeterSerialNumbers);

    List<StructureNodeLinks> GetStructureRootLinks(
      StructureTypeEnum structureType,
      out Dictionary<Guid, object> entitiesDictionary);
  }
}
