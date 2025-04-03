// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_MinomatMeters
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_MinomatMeters : SQLiteOfflineEntity
  {
    private Guid _Id;
    private int? _SignalStrength;
    private string _Status;
    private Guid? _MinomatId;
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

    public int? SignalStrength
    {
      get => this._SignalStrength;
      set
      {
        this.OnPropertyChanging(nameof (SignalStrength));
        this._SignalStrength = value;
        this.OnPropertyChanged(nameof (SignalStrength));
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
