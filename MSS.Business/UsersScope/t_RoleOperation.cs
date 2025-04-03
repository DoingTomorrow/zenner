// Decompiled with JetBrains decompiler
// Type: UsersScope.t_RoleOperation
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace UsersScope
{
  public class t_RoleOperation : SQLiteOfflineEntity
  {
    private Guid _Id;
    private Guid? _RoleId;
    private Guid? _OperationId;

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

    public Guid? RoleId
    {
      get => this._RoleId;
      set
      {
        this.OnPropertyChanging(nameof (RoleId));
        this._RoleId = value;
        this.OnPropertyChanged(nameof (RoleId));
      }
    }

    public Guid? OperationId
    {
      get => this._OperationId;
      set
      {
        this.OnPropertyChanging(nameof (OperationId));
        this._OperationId = value;
        this.OnPropertyChanged(nameof (OperationId));
      }
    }
  }
}
