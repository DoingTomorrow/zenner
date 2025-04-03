// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_MeterReplacementHistory
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_MeterReplacementHistory : SQLiteOfflineEntity
  {
    private Guid _Id;
    private DateTime? _ReplacementDate;
    private Guid? _CurrentMeterId;
    private Guid? _ReplacedMeterId;
    private Guid? _ReplacedById;
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

    public DateTime? ReplacementDate
    {
      get => this._ReplacementDate;
      set
      {
        this.OnPropertyChanging(nameof (ReplacementDate));
        this._ReplacementDate = value;
        this.OnPropertyChanged(nameof (ReplacementDate));
      }
    }

    public Guid? CurrentMeterId
    {
      get => this._CurrentMeterId;
      set
      {
        this.OnPropertyChanging(nameof (CurrentMeterId));
        this._CurrentMeterId = value;
        this.OnPropertyChanged(nameof (CurrentMeterId));
      }
    }

    public Guid? ReplacedMeterId
    {
      get => this._ReplacedMeterId;
      set
      {
        this.OnPropertyChanging(nameof (ReplacedMeterId));
        this._ReplacedMeterId = value;
        this.OnPropertyChanged(nameof (ReplacedMeterId));
      }
    }

    public Guid? ReplacedById
    {
      get => this._ReplacedById;
      set
      {
        this.OnPropertyChanging(nameof (ReplacedById));
        this._ReplacedById = value;
        this.OnPropertyChanged(nameof (ReplacedById));
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
