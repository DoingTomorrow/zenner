// Decompiled with JetBrains decompiler
// Type: UsersScope.t_User
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace UsersScope
{
  public class t_User : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _FirstName;
    private string _LastName;
    private string _Password;
    private string _Username;
    private string _Language;
    private bool _IsDeactivated;
    private string _Office;
    private Guid? _CountryId;

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

    [MaxLength(100)]
    public string FirstName
    {
      get => this._FirstName;
      set
      {
        this.OnPropertyChanging(nameof (FirstName));
        this._FirstName = value;
        this.OnPropertyChanged(nameof (FirstName));
      }
    }

    [MaxLength(100)]
    public string LastName
    {
      get => this._LastName;
      set
      {
        this.OnPropertyChanging(nameof (LastName));
        this._LastName = value;
        this.OnPropertyChanged(nameof (LastName));
      }
    }

    [MaxLength(100)]
    public string Password
    {
      get => this._Password;
      set
      {
        this.OnPropertyChanging(nameof (Password));
        this._Password = value;
        this.OnPropertyChanged(nameof (Password));
      }
    }

    [MaxLength(100)]
    public string Username
    {
      get => this._Username;
      set
      {
        this.OnPropertyChanging(nameof (Username));
        this._Username = value;
        this.OnPropertyChanged(nameof (Username));
      }
    }

    [MaxLength(100)]
    public string Language
    {
      get => this._Language;
      set
      {
        this.OnPropertyChanging(nameof (Language));
        this._Language = value;
        this.OnPropertyChanged(nameof (Language));
      }
    }

    public bool IsDeactivated
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
