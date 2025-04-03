// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_Provider
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_Provider : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _ProviderName;
    private string _SimPin;
    private string _AccessPoint;
    private string _UserId;
    private string _UserPassword;

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
    public string ProviderName
    {
      get => this._ProviderName;
      set
      {
        this.OnPropertyChanging(nameof (ProviderName));
        this._ProviderName = value;
        this.OnPropertyChanged(nameof (ProviderName));
      }
    }

    [MaxLength(255)]
    public string SimPin
    {
      get => this._SimPin;
      set
      {
        this.OnPropertyChanging(nameof (SimPin));
        this._SimPin = value;
        this.OnPropertyChanged(nameof (SimPin));
      }
    }

    [MaxLength(255)]
    public string AccessPoint
    {
      get => this._AccessPoint;
      set
      {
        this.OnPropertyChanging(nameof (AccessPoint));
        this._AccessPoint = value;
        this.OnPropertyChanged(nameof (AccessPoint));
      }
    }

    [MaxLength(255)]
    public string UserId
    {
      get => this._UserId;
      set
      {
        this.OnPropertyChanging(nameof (UserId));
        this._UserId = value;
        this.OnPropertyChanged(nameof (UserId));
      }
    }

    [MaxLength(255)]
    public string UserPassword
    {
      get => this._UserPassword;
      set
      {
        this.OnPropertyChanging(nameof (UserPassword));
        this._UserPassword = value;
        this.OnPropertyChanged(nameof (UserPassword));
      }
    }
  }
}
