// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_MinomatRadioDetails
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_MinomatRadioDetails : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _Location;
    private string _Entrance;
    private bool? _IsConfigured;
    private DateTime? _DueDate;
    private bool? _CanRegiesterMoreThanOne;
    private int? _ReservedPlaces;
    private DateTime? _InstalledOn;
    private DateTime? _LastConnection;
    private string _Channel;
    private string _NrOfReceivedDevices;
    private string _NrOfAssignedDevices;
    private string _NrOfRegisteredDevices;
    private int? _NetExtensionMode;
    private Guid? _MinomatId;
    private string _NodeId;
    private string _Description;
    private string _NetId;
    private string _StatusDevices;
    private string _StatusNetwork;
    private DateTime? _LastStartOn;
    private DateTime? _LastChangedOn;
    private DateTime? _LastRegisteredOn;
    private string _GSMStatus;
    private DateTime? _GSMStatusDate;

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

    [MaxLength(255)]
    public string Location
    {
      get => this._Location;
      set
      {
        this.OnPropertyChanging(nameof (Location));
        this._Location = value;
        this.OnPropertyChanged(nameof (Location));
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

    public bool? IsConfigured
    {
      get => this._IsConfigured;
      set
      {
        this.OnPropertyChanging(nameof (IsConfigured));
        this._IsConfigured = value;
        this.OnPropertyChanged(nameof (IsConfigured));
      }
    }

    public DateTime? DueDate
    {
      get => this._DueDate;
      set
      {
        this.OnPropertyChanging(nameof (DueDate));
        this._DueDate = value;
        this.OnPropertyChanged(nameof (DueDate));
      }
    }

    public bool? CanRegiesterMoreThanOne
    {
      get => this._CanRegiesterMoreThanOne;
      set
      {
        this.OnPropertyChanging(nameof (CanRegiesterMoreThanOne));
        this._CanRegiesterMoreThanOne = value;
        this.OnPropertyChanged(nameof (CanRegiesterMoreThanOne));
      }
    }

    public int? ReservedPlaces
    {
      get => this._ReservedPlaces;
      set
      {
        this.OnPropertyChanging(nameof (ReservedPlaces));
        this._ReservedPlaces = value;
        this.OnPropertyChanged(nameof (ReservedPlaces));
      }
    }

    public DateTime? InstalledOn
    {
      get => this._InstalledOn;
      set
      {
        this.OnPropertyChanging(nameof (InstalledOn));
        this._InstalledOn = value;
        this.OnPropertyChanged(nameof (InstalledOn));
      }
    }

    public DateTime? LastConnection
    {
      get => this._LastConnection;
      set
      {
        this.OnPropertyChanging(nameof (LastConnection));
        this._LastConnection = value;
        this.OnPropertyChanged(nameof (LastConnection));
      }
    }

    [MaxLength(255)]
    public string Channel
    {
      get => this._Channel;
      set
      {
        this.OnPropertyChanging(nameof (Channel));
        this._Channel = value;
        this.OnPropertyChanged(nameof (Channel));
      }
    }

    [MaxLength(255)]
    public string NrOfReceivedDevices
    {
      get => this._NrOfReceivedDevices;
      set
      {
        this.OnPropertyChanging(nameof (NrOfReceivedDevices));
        this._NrOfReceivedDevices = value;
        this.OnPropertyChanged(nameof (NrOfReceivedDevices));
      }
    }

    [MaxLength(255)]
    public string NrOfAssignedDevices
    {
      get => this._NrOfAssignedDevices;
      set
      {
        this.OnPropertyChanging(nameof (NrOfAssignedDevices));
        this._NrOfAssignedDevices = value;
        this.OnPropertyChanged(nameof (NrOfAssignedDevices));
      }
    }

    [MaxLength(255)]
    public string NrOfRegisteredDevices
    {
      get => this._NrOfRegisteredDevices;
      set
      {
        this.OnPropertyChanging(nameof (NrOfRegisteredDevices));
        this._NrOfRegisteredDevices = value;
        this.OnPropertyChanged(nameof (NrOfRegisteredDevices));
      }
    }

    public int? NetExtensionMode
    {
      get => this._NetExtensionMode;
      set
      {
        this.OnPropertyChanging(nameof (NetExtensionMode));
        this._NetExtensionMode = value;
        this.OnPropertyChanged(nameof (NetExtensionMode));
      }
    }

    public Guid? MinomatId
    {
      get => this._MinomatId;
      set
      {
        this.OnPropertyChanging(nameof (MinomatId));
        this._MinomatId = value;
        this.OnPropertyChanged(nameof (MinomatId));
      }
    }

    [MaxLength(255)]
    public string NodeId
    {
      get => this._NodeId;
      set
      {
        this.OnPropertyChanging(nameof (NodeId));
        this._NodeId = value;
        this.OnPropertyChanged(nameof (NodeId));
      }
    }

    [MaxLength(255)]
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

    [MaxLength(255)]
    public string NetId
    {
      get => this._NetId;
      set
      {
        this.OnPropertyChanging(nameof (NetId));
        this._NetId = value;
        this.OnPropertyChanged(nameof (NetId));
      }
    }

    [MaxLength(255)]
    public string StatusDevices
    {
      get => this._StatusDevices;
      set
      {
        this.OnPropertyChanging(nameof (StatusDevices));
        this._StatusDevices = value;
        this.OnPropertyChanged(nameof (StatusDevices));
      }
    }

    [MaxLength(255)]
    public string StatusNetwork
    {
      get => this._StatusNetwork;
      set
      {
        this.OnPropertyChanging(nameof (StatusNetwork));
        this._StatusNetwork = value;
        this.OnPropertyChanged(nameof (StatusNetwork));
      }
    }

    public DateTime? LastStartOn
    {
      get => this._LastStartOn;
      set
      {
        this.OnPropertyChanging(nameof (LastStartOn));
        this._LastStartOn = value;
        this.OnPropertyChanged(nameof (LastStartOn));
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

    public DateTime? LastRegisteredOn
    {
      get => this._LastRegisteredOn;
      set
      {
        this.OnPropertyChanging(nameof (LastRegisteredOn));
        this._LastRegisteredOn = value;
        this.OnPropertyChanged(nameof (LastRegisteredOn));
      }
    }

    [MaxLength(255)]
    public string GSMStatus
    {
      get => this._GSMStatus;
      set
      {
        this.OnPropertyChanging(nameof (GSMStatus));
        this._GSMStatus = value;
        this.OnPropertyChanged(nameof (GSMStatus));
      }
    }

    public DateTime? GSMStatusDate
    {
      get => this._GSMStatusDate;
      set
      {
        this.OnPropertyChanging(nameof (GSMStatusDate));
        this._GSMStatusDate = value;
        this.OnPropertyChanged(nameof (GSMStatusDate));
      }
    }
  }
}
