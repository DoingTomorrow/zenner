// Decompiled with JetBrains decompiler
// Type: UsersScope.t_UserDeviceTypeSettings
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace UsersScope
{
  public class t_UserDeviceTypeSettings : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _DeviceType;
    private double? _DecimalPlaces;
    private Guid? _DisplayUnitId;
    private Guid? _UserId;

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

    public Guid? DisplayUnitId
    {
      get => this._DisplayUnitId;
      set
      {
        this.OnPropertyChanging(nameof (DisplayUnitId));
        this._DisplayUnitId = value;
        this.OnPropertyChanged(nameof (DisplayUnitId));
      }
    }

    public Guid? UserId
    {
      get => this._UserId;
      set
      {
        this.OnPropertyChanging(nameof (UserId));
        this._UserId = value;
        this.OnPropertyChanged(nameof (UserId));
      }
    }
  }
}
