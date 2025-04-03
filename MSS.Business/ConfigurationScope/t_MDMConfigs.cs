// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_MDMConfigs
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_MDMConfigs : SQLiteOfflineEntity
  {
    private int _Id;
    private string _MDMPassword;
    private string _MDMUser;
    private string _MDMUrl;
    private int? _Company;
    private string _CustomerNumber;
    private Guid? _CountryId;

    [PrimaryKey]
    public int Id
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
    public string MDMPassword
    {
      get => this._MDMPassword;
      set
      {
        this.OnPropertyChanging(nameof (MDMPassword));
        this._MDMPassword = value;
        this.OnPropertyChanged(nameof (MDMPassword));
      }
    }

    [MaxLength(255)]
    public string MDMUser
    {
      get => this._MDMUser;
      set
      {
        this.OnPropertyChanging(nameof (MDMUser));
        this._MDMUser = value;
        this.OnPropertyChanged(nameof (MDMUser));
      }
    }

    [MaxLength(255)]
    public string MDMUrl
    {
      get => this._MDMUrl;
      set
      {
        this.OnPropertyChanging(nameof (MDMUrl));
        this._MDMUrl = value;
        this.OnPropertyChanged(nameof (MDMUrl));
      }
    }

    public int? Company
    {
      get => this._Company;
      set
      {
        this.OnPropertyChanging(nameof (Company));
        this._Company = value;
        this.OnPropertyChanged(nameof (Company));
      }
    }

    [MaxLength(255)]
    public string CustomerNumber
    {
      get => this._CustomerNumber;
      set
      {
        this.OnPropertyChanging(nameof (CustomerNumber));
        this._CustomerNumber = value;
        this.OnPropertyChanged(nameof (CustomerNumber));
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
  }
}
