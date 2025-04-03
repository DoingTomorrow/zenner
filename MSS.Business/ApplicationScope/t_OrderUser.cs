// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_OrderUser
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_OrderUser : SQLiteOfflineEntity
  {
    private Guid _Id;
    private Guid? _OrderId;
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
