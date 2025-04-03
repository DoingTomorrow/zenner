// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_Meter
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_Meter : SQLiteOfflineEntity
  {
    private Guid _Id;
    private double? _ImpulsValue;
    private double? _StartValue;
    private string _SerialNumber;
    private string _ShortDeviceNo;
    private string _CompletDevice;
    private double? _EvaluationFactor;
    private string _DeviceType;
    private double? _DecimalPlaces;
    private bool? _IsDeactivated;
    private bool? _IsConfigured;
    private string _CreatedBy;
    private string _UpdatedBy;
    private DateTime? _LastChangedOn;
    private int? _NrOfImpulses;
    private string _StartValueImpulses;
    private byte[] _GMMParameters;
    private DateTime? _ConfigDate;
    private bool? _IsReplaced;
    private string _AES;
    private int? _PrimaryAddress;
    private string _Manufacturer;
    private string _Medium;
    private string _Generation;
    private Guid? _RoomTypeId;
    private Guid? _ChannelId;
    private Guid? _ImpulsUnitId;
    private Guid? _ReadingUnitId;
    private Guid? _ConnectedDeviceTypeId;
    private string _InputNumber;
    private bool? _IsReceived;
    private bool? _IsError;
    private bool _ReadingEnabled;
    private string _GMMAdditionalInfo;

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

    public double? ImpulsValue
    {
      get => this._ImpulsValue;
      set
      {
        this.OnPropertyChanging(nameof (ImpulsValue));
        this._ImpulsValue = value;
        this.OnPropertyChanged(nameof (ImpulsValue));
      }
    }

    public double? StartValue
    {
      get => this._StartValue;
      set
      {
        this.OnPropertyChanging(nameof (StartValue));
        this._StartValue = value;
        this.OnPropertyChanged(nameof (StartValue));
      }
    }

    [MaxLength(200)]
    public string SerialNumber
    {
      get => this._SerialNumber;
      set
      {
        this.OnPropertyChanging(nameof (SerialNumber));
        this._SerialNumber = value;
        this.OnPropertyChanged(nameof (SerialNumber));
      }
    }

    [MaxLength(255)]
    public string ShortDeviceNo
    {
      get => this._ShortDeviceNo;
      set
      {
        this.OnPropertyChanging(nameof (ShortDeviceNo));
        this._ShortDeviceNo = value;
        this.OnPropertyChanged(nameof (ShortDeviceNo));
      }
    }

    [MaxLength(255)]
    public string CompletDevice
    {
      get => this._CompletDevice;
      set
      {
        this.OnPropertyChanging(nameof (CompletDevice));
        this._CompletDevice = value;
        this.OnPropertyChanged(nameof (CompletDevice));
      }
    }

    public double? EvaluationFactor
    {
      get => this._EvaluationFactor;
      set
      {
        this.OnPropertyChanging(nameof (EvaluationFactor));
        this._EvaluationFactor = value;
        this.OnPropertyChanged(nameof (EvaluationFactor));
      }
    }

    [MaxLength(255)]
    public string DeviceType
    {
      get => this._DeviceType;
      set
      {
        this.OnPropertyChanging(nameof (DeviceType));
        this._DeviceType = value;
        this.OnPropertyChanged(nameof (DeviceType));
      }
    }

    public double? DecimalPlaces
    {
      get => this._DecimalPlaces;
      set
      {
        this.OnPropertyChanging(nameof (DecimalPlaces));
        this._DecimalPlaces = value;
        this.OnPropertyChanged(nameof (DecimalPlaces));
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

    public int? NrOfImpulses
    {
      get => this._NrOfImpulses;
      set
      {
        this.OnPropertyChanging(nameof (NrOfImpulses));
        this._NrOfImpulses = value;
        this.OnPropertyChanged(nameof (NrOfImpulses));
      }
    }

    [MaxLength(255)]
    public string StartValueImpulses
    {
      get => this._StartValueImpulses;
      set
      {
        this.OnPropertyChanging(nameof (StartValueImpulses));
        this._StartValueImpulses = value;
        this.OnPropertyChanged(nameof (StartValueImpulses));
      }
    }

    [MaxLength(8000)]
    public byte[] GMMParameters
    {
      get => this._GMMParameters;
      set
      {
        this.OnPropertyChanging(nameof (GMMParameters));
        this._GMMParameters = value;
        this.OnPropertyChanged(nameof (GMMParameters));
      }
    }

    public DateTime? ConfigDate
    {
      get => this._ConfigDate;
      set
      {
        this.OnPropertyChanging(nameof (ConfigDate));
        this._ConfigDate = value;
        this.OnPropertyChanged(nameof (ConfigDate));
      }
    }

    public bool? IsReplaced
    {
      get => this._IsReplaced;
      set
      {
        this.OnPropertyChanging(nameof (IsReplaced));
        this._IsReplaced = value;
        this.OnPropertyChanged(nameof (IsReplaced));
      }
    }

    [MaxLength(50)]
    public string AES
    {
      get => this._AES;
      set
      {
        this.OnPropertyChanging(nameof (AES));
        this._AES = value;
        this.OnPropertyChanged(nameof (AES));
      }
    }

    public int? PrimaryAddress
    {
      get => this._PrimaryAddress;
      set
      {
        this.OnPropertyChanging(nameof (PrimaryAddress));
        this._PrimaryAddress = value;
        this.OnPropertyChanged(nameof (PrimaryAddress));
      }
    }

    [MaxLength(50)]
    public string Manufacturer
    {
      get => this._Manufacturer;
      set
      {
        this.OnPropertyChanging(nameof (Manufacturer));
        this._Manufacturer = value;
        this.OnPropertyChanged(nameof (Manufacturer));
      }
    }

    [MaxLength(50)]
    public string Medium
    {
      get => this._Medium;
      set
      {
        this.OnPropertyChanging(nameof (Medium));
        this._Medium = value;
        this.OnPropertyChanged(nameof (Medium));
      }
    }

    [MaxLength(50)]
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

    public Guid? RoomTypeId
    {
      get => this._RoomTypeId;
      set
      {
        this.OnPropertyChanging(nameof (RoomTypeId));
        this._RoomTypeId = value;
        this.OnPropertyChanged(nameof (RoomTypeId));
      }
    }

    public Guid? ChannelId
    {
      get => this._ChannelId;
      set
      {
        this.OnPropertyChanging(nameof (ChannelId));
        this._ChannelId = value;
        this.OnPropertyChanged(nameof (ChannelId));
      }
    }

    public Guid? ImpulsUnitId
    {
      get => this._ImpulsUnitId;
      set
      {
        this.OnPropertyChanging(nameof (ImpulsUnitId));
        this._ImpulsUnitId = value;
        this.OnPropertyChanged(nameof (ImpulsUnitId));
      }
    }

    public Guid? ReadingUnitId
    {
      get => this._ReadingUnitId;
      set
      {
        this.OnPropertyChanging(nameof (ReadingUnitId));
        this._ReadingUnitId = value;
        this.OnPropertyChanged(nameof (ReadingUnitId));
      }
    }

    public Guid? ConnectedDeviceTypeId
    {
      get => this._ConnectedDeviceTypeId;
      set
      {
        this.OnPropertyChanging(nameof (ConnectedDeviceTypeId));
        this._ConnectedDeviceTypeId = value;
        this.OnPropertyChanged(nameof (ConnectedDeviceTypeId));
      }
    }

    [MaxLength(255)]
    public string InputNumber
    {
      get => this._InputNumber;
      set
      {
        this.OnPropertyChanging(nameof (InputNumber));
        this._InputNumber = value;
        this.OnPropertyChanged(nameof (InputNumber));
      }
    }

    public bool? IsReceived
    {
      get => this._IsReceived;
      set
      {
        this.OnPropertyChanging(nameof (IsReceived));
        this._IsReceived = value;
        this.OnPropertyChanged(nameof (IsReceived));
      }
    }

    public bool? IsError
    {
      get => this._IsError;
      set
      {
        this.OnPropertyChanging(nameof (IsError));
        this._IsError = value;
        this.OnPropertyChanged(nameof (IsError));
      }
    }

    public bool ReadingEnabled
    {
      get => this._ReadingEnabled;
      set
      {
        this.OnPropertyChanging(nameof (ReadingEnabled));
        this._ReadingEnabled = value;
        this.OnPropertyChanged(nameof (ReadingEnabled));
      }
    }

    [MaxLength(255)]
    public string GMMAdditionalInfo
    {
      get => this._GMMAdditionalInfo;
      set
      {
        this.OnPropertyChanging(nameof (GMMAdditionalInfo));
        this._GMMAdditionalInfo = value;
        this.OnPropertyChanged(nameof (GMMAdditionalInfo));
      }
    }
  }
}
