// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_OrderMessages
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_OrderMessages : SQLiteOfflineEntity
  {
    private Guid _Id;
    private Guid? _OrderId;
    private Guid? _MeterId;
    private string _Level;
    private DateTime? _Timepoint;
    private string _Message;
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

    public Guid? OrderId
    {
      get => this._OrderId;
      set
      {
        this.OnPropertyChanging(nameof (OrderId));
        this._OrderId = value;
        this.OnPropertyChanged(nameof (OrderId));
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

    public string Level
    {
      get => this._Level;
      set
      {
        this.OnPropertyChanging(nameof (Level));
        this._Level = value;
        this.OnPropertyChanged(nameof (Level));
      }
    }

    public DateTime? Timepoint
    {
      get => this._Timepoint;
      set
      {
        this.OnPropertyChanging(nameof (Timepoint));
        this._Timepoint = value;
        this.OnPropertyChanged(nameof (Timepoint));
      }
    }

    public string Message
    {
      get => this._Message;
      set
      {
        this.OnPropertyChanging(nameof (Message));
        this._Message = value;
        this.OnPropertyChanged(nameof (Message));
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
