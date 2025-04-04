// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.LocationSerializableDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class LocationSerializableDTO
  {
    public Guid Id { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string ZipCode { get; set; }

    public string BuildingNr { get; set; }

    public string Description { get; set; }

    public int Generation { get; set; }

    public Guid? ScenarioId { get; set; }

    public DateTime? DueDate { get; set; }

    public bool IsDeactivated { get; set; }

    public bool? HasMaster { get; set; }

    public Guid CountryId { get; set; }

    public DateTime? LastChangedOn { get; set; }
  }
}
