// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.LocationDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class LocationDTO : DTOBase
  {
    private string _status;

    public Guid Id { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string ZipCode { get; set; }

    public string BuildingNr { get; set; }

    public string Description { get; set; }

    public GenerationEnum Generation { get; set; }

    public Scenario Scenario { get; set; }

    public DateTime? DueDate { get; set; }

    public bool IsDeactivated { get; set; }

    public bool? HasMaster { get; set; }

    public ScaleEnum Scale { get; set; }

    public string Status
    {
      get => this._status;
      set
      {
        this._status = value;
        this.OnPropertyChanged(nameof (Status));
      }
    }

    public DateTime? LastChangedOn { get; set; }
  }
}
