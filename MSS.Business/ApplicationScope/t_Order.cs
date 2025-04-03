// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_Order
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_Order : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _InstallationNumber;
    private Guid? _RootStructureNodeId;
    private string _Details;
    private bool? _Exported;
    private string _Status;
    private string _DeviceNumber;
    private byte[] _TestConfig;
    private DateTime? _DueDate;
    private bool? _IsDeactivated;
    private string _OrderType;
    private string _CloseOrderReason;
    private Guid? _LockedBy;
    private byte[] _StructureBytes;
    private string _StructureType;
    private DateTime? _CreatedOn;
    private DateTime? _LastChangedOn;
    private Guid? _FilterId;

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
    public string InstallationNumber
    {
      get => this._InstallationNumber;
      set
      {
        this.OnPropertyChanging(nameof (InstallationNumber));
        this._InstallationNumber = value;
        this.OnPropertyChanged(nameof (InstallationNumber));
      }
    }

    public Guid? RootStructureNodeId
    {
      get => this._RootStructureNodeId;
      set
      {
        this.OnPropertyChanging(nameof (RootStructureNodeId));
        this._RootStructureNodeId = value;
        this.OnPropertyChanged(nameof (RootStructureNodeId));
      }
    }

    [MaxLength(1000)]
    public string Details
    {
      get => this._Details;
      set
      {
        this.OnPropertyChanging(nameof (Details));
        this._Details = value;
        this.OnPropertyChanged(nameof (Details));
      }
    }

    public bool? Exported
    {
      get => this._Exported;
      set
      {
        this.OnPropertyChanging(nameof (Exported));
        this._Exported = value;
        this.OnPropertyChanged(nameof (Exported));
      }
    }

    [MaxLength(255)]
    public string Status
    {
      get => this._Status;
      set
      {
        this.OnPropertyChanging(nameof (Status));
        this._Status = value;
        this.OnPropertyChanged(nameof (Status));
      }
    }

    [MaxLength(255)]
    public string DeviceNumber
    {
      get => this._DeviceNumber;
      set
      {
        this.OnPropertyChanging(nameof (DeviceNumber));
        this._DeviceNumber = value;
        this.OnPropertyChanged(nameof (DeviceNumber));
      }
    }

    [MaxLength(8000)]
    public byte[] TestConfig
    {
      get => this._TestConfig;
      set
      {
        this.OnPropertyChanging(nameof (TestConfig));
        this._TestConfig = value;
        this.OnPropertyChanged(nameof (TestConfig));
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
    public string OrderType
    {
      get => this._OrderType;
      set
      {
        this.OnPropertyChanging(nameof (OrderType));
        this._OrderType = value;
        this.OnPropertyChanged(nameof (OrderType));
      }
    }

    [MaxLength(255)]
    public string CloseOrderReason
    {
      get => this._CloseOrderReason;
      set
      {
        this.OnPropertyChanging(nameof (CloseOrderReason));
        this._CloseOrderReason = value;
        this.OnPropertyChanged(nameof (CloseOrderReason));
      }
    }

    public Guid? LockedBy
    {
      get => this._LockedBy;
      set
      {
        this.OnPropertyChanging(nameof (LockedBy));
        this._LockedBy = value;
        this.OnPropertyChanged(nameof (LockedBy));
      }
    }

    [MaxLength(8000)]
    public byte[] StructureBytes
    {
      get => this._StructureBytes;
      set
      {
        this.OnPropertyChanging(nameof (StructureBytes));
        this._StructureBytes = value;
        this.OnPropertyChanged(nameof (StructureBytes));
      }
    }

    [MaxLength(255)]
    public string StructureType
    {
      get => this._StructureType;
      set
      {
        this.OnPropertyChanging(nameof (StructureType));
        this._StructureType = value;
        this.OnPropertyChanged(nameof (StructureType));
      }
    }

    public DateTime? CreatedOn
    {
      get => this._CreatedOn;
      set
      {
        this.OnPropertyChanging(nameof (CreatedOn));
        this._CreatedOn = value;
        this.OnPropertyChanged(nameof (CreatedOn));
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

    public Guid? FilterId
    {
      get => this._FilterId;
      set
      {
        this.OnPropertyChanging(nameof (FilterId));
        this._FilterId = value;
        this.OnPropertyChanged(nameof (FilterId));
      }
    }
  }
}
