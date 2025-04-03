// Decompiled with JetBrains decompiler
// Type: ReadingValuesScope.t_OrderReadingValues
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ReadingValuesScope
{
  public class t_OrderReadingValues : SQLiteOfflineEntity
  {
    private Guid _Id;
    private Guid? _OrderId;
    private Guid? _MeterReadingValueId;

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

    public Guid? MeterReadingValueId
    {
      get => this._MeterReadingValueId;
      set
      {
        this.OnPropertyChanging(nameof (MeterReadingValueId));
        this._MeterReadingValueId = value;
        this.OnPropertyChanged(nameof (MeterReadingValueId));
      }
    }
  }
}
