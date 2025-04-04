// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.TenantDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class TenantDTO : DTOBase
  {
    private int? _noOfDevices;

    public Guid Id { get; set; }

    public int TenantNr { get; set; }

    public string Name { get; set; }

    public string FloorNr { get; set; }

    public string FloorName { get; set; }

    public string ApartmentNr { get; set; }

    public string Direction { get; set; }

    public string Description { get; set; }

    public string CustomerTenantNo { get; set; }

    public string Entrance { get; set; }

    public bool IsDeactivated { get; set; }

    public DateTime? LastChangedOn { get; set; }

    public int? NoOfDevices
    {
      get => this._noOfDevices;
      set
      {
        this._noOfDevices = value;
        this.OnPropertyChanged(nameof (NoOfDevices));
      }
    }
  }
}
