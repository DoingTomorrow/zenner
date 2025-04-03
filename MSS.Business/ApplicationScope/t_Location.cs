// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_Location
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_Location : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _City;
    private string _Street;
    private string _ZipCode;
    private string _BuildingNr;
    private string _Description;
    private DateTime _DueDate;
    private string _Generation;
    private string _Scale;
    private bool _HasMaster;
    private bool? _IsDeactivated;
    private string _Office;
    private string _CreatedBy;
    private string _UpdatedBy;
    private DateTime? _LastUpdateBuildingNo;
    private Guid _ScenarioId;
    private Guid? _CountryId;
    private DateTime? _LastChangedOn;

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

    [MaxLength(25)]
    public string City
    {
      get => this._City;
      set
      {
        this.OnPropertyChanging(nameof (City));
        this._City = value;
        this.OnPropertyChanged(nameof (City));
      }
    }

    [MaxLength(90)]
    public string Street
    {
      get => this._Street;
      set
      {
        this.OnPropertyChanging(nameof (Street));
        this._Street = value;
        this.OnPropertyChanged(nameof (Street));
      }
    }

    [MaxLength(10)]
    public string ZipCode
    {
      get => this._ZipCode;
      set
      {
        this.OnPropertyChanging(nameof (ZipCode));
        this._ZipCode = value;
        this.OnPropertyChanged(nameof (ZipCode));
      }
    }

    [MaxLength(13)]
    public string BuildingNr
    {
      get => this._BuildingNr;
      set
      {
        this.OnPropertyChanging(nameof (BuildingNr));
        this._BuildingNr = value;
        this.OnPropertyChanged(nameof (BuildingNr));
      }
    }

    [MaxLength(250)]
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

    public DateTime DueDate
    {
      get => this._DueDate;
      set
      {
        this.OnPropertyChanging(nameof (DueDate));
        this._DueDate = value;
        this.OnPropertyChanged(nameof (DueDate));
      }
    }

    [MaxLength(255)]
    public string Generation
    {
      get => this._Generation;
      set
      {
        this.OnPropertyChanging(nameof (Generation));
        this._Generation = value;
        this.OnPropertyChanged(nameof (Generation));
      }
    }

    [MaxLength(255)]
    public string Scale
    {
      get => this._Scale;
      set
      {
        this.OnPropertyChanging(nameof (Scale));
        this._Scale = value;
        this.OnPropertyChanged(nameof (Scale));
      }
    }

    public bool HasMaster
    {
      get => this._HasMaster;
      set
      {
        this.OnPropertyChanging(nameof (HasMaster));
        this._HasMaster = value;
        this.OnPropertyChanged(nameof (HasMaster));
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
    public string Office
    {
      get => this._Office;
      set
      {
        this.OnPropertyChanging(nameof (Office));
        this._Office = value;
        this.OnPropertyChanged(nameof (Office));
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

    public DateTime? LastUpdateBuildingNo
    {
      get => this._LastUpdateBuildingNo;
      set
      {
        this.OnPropertyChanging(nameof (LastUpdateBuildingNo));
        this._LastUpdateBuildingNo = value;
        this.OnPropertyChanged(nameof (LastUpdateBuildingNo));
      }
    }

    public Guid ScenarioId
    {
      get => this._ScenarioId;
      set
      {
        this.OnPropertyChanging(nameof (ScenarioId));
        this._ScenarioId = value;
        this.OnPropertyChanged(nameof (ScenarioId));
      }
    }

    public Guid? CountryId
    {
      get => this._CountryId;
      set
      {
        this.OnPropertyChanging(nameof (CountryId));
        this._CountryId = value;
        this.OnPropertyChanged(nameof (CountryId));
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
  }
}
