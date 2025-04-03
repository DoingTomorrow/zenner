// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_MeterMbusRadio
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_MeterMbusRadio : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _City;
    private string _Street;
    private string _HouseNumber;
    private string _HouseNumberSupplement;
    private string _ApartmentNumber;
    private string _ZipCode;
    private string _FirstName;
    private string _LastName;
    private string _Location;
    private string _RadioSerialNumber;
    private Guid? _MeterId;
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

    public string HouseNumber
    {
      get => this._HouseNumber;
      set
      {
        this.OnPropertyChanging(nameof (HouseNumber));
        this._HouseNumber = value;
        this.OnPropertyChanged(nameof (HouseNumber));
      }
    }

    public string HouseNumberSupplement
    {
      get => this._HouseNumberSupplement;
      set
      {
        this.OnPropertyChanging(nameof (HouseNumberSupplement));
        this._HouseNumberSupplement = value;
        this.OnPropertyChanged(nameof (HouseNumberSupplement));
      }
    }

    public string ApartmentNumber
    {
      get => this._ApartmentNumber;
      set
      {
        this.OnPropertyChanging(nameof (ApartmentNumber));
        this._ApartmentNumber = value;
        this.OnPropertyChanged(nameof (ApartmentNumber));
      }
    }

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

    public string RadioSerialNumber
    {
      get => this._RadioSerialNumber;
      set
      {
        this.OnPropertyChanging(nameof (RadioSerialNumber));
        this._RadioSerialNumber = value;
        this.OnPropertyChanged(nameof (RadioSerialNumber));
      }
    }

    public Guid? MeterId
    {
      get => this._MeterId;
      set
      {
        this.OnPropertyChanging(nameof (MeterId));
        this._MeterId = value;
        this.OnPropertyChanged(nameof (MeterId));
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
