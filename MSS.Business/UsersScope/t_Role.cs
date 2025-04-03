// Decompiled with JetBrains decompiler
// Type: UsersScope.t_Role
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace UsersScope
{
  public class t_Role : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _Name;
    private bool _IsStandard;
    private bool _IsDeactivated;

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
    public string Name
    {
      get => this._Name;
      set
      {
        this.OnPropertyChanging(nameof (Name));
        this._Name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    public bool IsStandard
    {
      get => this._IsStandard;
      set
      {
        this.OnPropertyChanging(nameof (IsStandard));
        this._IsStandard = value;
        this.OnPropertyChanged(nameof (IsStandard));
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
  }
}
