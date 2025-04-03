// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_StructureNodeEquipmentSettings
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_StructureNodeEquipmentSettings : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _EquipmentName;
    private string _EquipmentParams;
    private string _SystemName;
    private string _ScanMode;
    private string _ScanParams;
    private string _ReadingProfileName;
    private Guid? _StructureNodeId;
    private DateTime? _LastChangedOn;
    private string _ReadingProfileParams;
    private string _DeviceModelReadingParams;

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
    public string EquipmentName
    {
      get => this._EquipmentName;
      set
      {
        this.OnPropertyChanging(nameof (EquipmentName));
        this._EquipmentName = value;
        this.OnPropertyChanged(nameof (EquipmentName));
      }
    }

    [MaxLength(255)]
    public string EquipmentParams
    {
      get => this._EquipmentParams;
      set
      {
        this.OnPropertyChanging(nameof (EquipmentParams));
        this._EquipmentParams = value;
        this.OnPropertyChanged(nameof (EquipmentParams));
      }
    }

    [MaxLength(255)]
    public string SystemName
    {
      get => this._SystemName;
      set
      {
        this.OnPropertyChanging(nameof (SystemName));
        this._SystemName = value;
        this.OnPropertyChanged(nameof (SystemName));
      }
    }

    [MaxLength(255)]
    public string ScanMode
    {
      get => this._ScanMode;
      set
      {
        this.OnPropertyChanging(nameof (ScanMode));
        this._ScanMode = value;
        this.OnPropertyChanged(nameof (ScanMode));
      }
    }

    [MaxLength(255)]
    public string ScanParams
    {
      get => this._ScanParams;
      set
      {
        this.OnPropertyChanging(nameof (ScanParams));
        this._ScanParams = value;
        this.OnPropertyChanged(nameof (ScanParams));
      }
    }

    [MaxLength(255)]
    public string ReadingProfileName
    {
      get => this._ReadingProfileName;
      set
      {
        this.OnPropertyChanging(nameof (ReadingProfileName));
        this._ReadingProfileName = value;
        this.OnPropertyChanged(nameof (ReadingProfileName));
      }
    }

    public Guid? StructureNodeId
    {
      get => this._StructureNodeId;
      set
      {
        this.OnPropertyChanging(nameof (StructureNodeId));
        this._StructureNodeId = value;
        this.OnPropertyChanged(nameof (StructureNodeId));
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

    [MaxLength(8000)]
    public string ReadingProfileParams
    {
      get => this._ReadingProfileParams;
      set
      {
        this.OnPropertyChanging(nameof (ReadingProfileParams));
        this._ReadingProfileParams = value;
        this.OnPropertyChanged(nameof (ReadingProfileParams));
      }
    }

    public string DeviceModelReadingParams
    {
      get => this._DeviceModelReadingParams;
      set
      {
        this.OnPropertyChanging(nameof (DeviceModelReadingParams));
        this._DeviceModelReadingParams = value;
        this.OnPropertyChanged(nameof (DeviceModelReadingParams));
      }
    }
  }
}
