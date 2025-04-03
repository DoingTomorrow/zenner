// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_Tenant
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_Tenant : SQLiteOfflineEntity
  {
    private Guid _Id;
    private int _TenantNr;
    private string _Name;
    private string _FloorNr;
    private string _FloorName;
    private string _ApartmentNr;
    private string _Direction;
    private string _Description;
    private string _CustomerTenantNo;
    private int? _RealTenantNo;
    private bool? _IsDeactivated;
    private string _CreatedBy;
    private string _UpdatedBy;
    private DateTime? _LastChangedOn;
    private string _Entrance;

    [PrimaryKey]
    public Guid Id
    {
      get => this._Id;
      set
      {
        this.OnPropertyChanging(nameof (Id));
        this._Id = value;
        this.OnPropertyChanged(nameof (Id));
      }
    }

    public int TenantNr
    {
      get => this._TenantNr;
      set
      {
        this.OnPropertyChanging(nameof (TenantNr));
        this._TenantNr = value;
        this.OnPropertyChanged(nameof (TenantNr));
      }
    }

    [MaxLength(30)]
    public string Name
    {
      get => this._Name;
      set
      {
        this.OnPropertyChanging(nameof (Name));
        this._Name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    [MaxLength(3)]
    public string FloorNr
    {
      get => this._FloorNr;
      set
      {
        this.OnPropertyChanging(nameof (FloorNr));
        this._FloorNr = value;
        this.OnPropertyChanged(nameof (FloorNr));
      }
    }

    [MaxLength(10)]
    public string FloorName
    {
      get => this._FloorName;
      set
      {
        this.OnPropertyChanging(nameof (FloorName));
        this._FloorName = value;
        this.OnPropertyChanged(nameof (FloorName));
      }
    }

    [MaxLength(3)]
    public string ApartmentNr
    {
      get => this._ApartmentNr;
      set
      {
        this.OnPropertyChanging(nameof (ApartmentNr));
        this._ApartmentNr = value;
        this.OnPropertyChanged(nameof (ApartmentNr));
      }
    }

    [MaxLength(10)]
    public string Direction
    {
      get => this._Direction;
      set
      {
        this.OnPropertyChanging(nameof (Direction));
        this._Direction = value;
        this.OnPropertyChanged(nameof (Direction));
      }
    }

    [MaxLength(500)]
    public string Description
    {
      get => this._Description;
      set
      {
        this.OnPropertyChanging(nameof (Description));
        this._Description = value;
        this.OnPropertyChanged(nameof (Description));
      }
    }

    [MaxLength(30)]
    public string CustomerTenantNo
    {
      get => this._CustomerTenantNo;
      set
      {
        this.OnPropertyChanging(nameof (CustomerTenantNo));
        this._CustomerTenantNo = value;
        this.OnPropertyChanged(nameof (CustomerTenantNo));
      }
    }

    public int? RealTenantNo
    {
      get => this._RealTenantNo;
      set
      {
        this.OnPropertyChanging(nameof (RealTenantNo));
        this._RealTenantNo = value;
        this.OnPropertyChanged(nameof (RealTenantNo));
      }
    }

    public bool? IsDeactivated
    {
      get => this._IsDeactivated;
      set
      {
        this.OnPropertyChanging(nameof (IsDeactivated));
        this._IsDeactivated = value;
        this.OnPropertyChanged(nameof (IsDeactivated));
      }
    }

    [MaxLength(255)]
    public string CreatedBy
    {
      get => this._CreatedBy;
      set
      {
        this.OnPropertyChanging(nameof (CreatedBy));
        this._CreatedBy = value;
        this.OnPropertyChanged(nameof (CreatedBy));
      }
    }

    [MaxLength(255)]
    public string UpdatedBy
    {
      get => this._UpdatedBy;
      set
      {
        this.OnPropertyChanging(nameof (UpdatedBy));
        this._UpdatedBy = value;
        this.OnPropertyChanged(nameof (UpdatedBy));
      }
    }

    public DateTime? LastChangedOn
    {
      get => this._LastChangedOn;
      set
      {
        this.OnPropertyChanging(nameof (LastChangedOn));
        this._LastChangedOn = value;
        this.OnPropertyChanged(nameof (LastChangedOn));
      }
    }

    [MaxLength(255)]
    public string Entrance
    {
      get => this._Entrance;
      set
      {
        this.OnPropertyChanging(nameof (Entrance));
        this._Entrance = value;
        this.OnPropertyChanged(nameof (Entrance));
      }
    }
  }
}
