// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_Filters
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_Filters : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _Name;
    private string _Description;

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

    [MaxLength(1000)]
    public string Description
    {
      get => this._Description;
      set
      {
        this.OnPropertyChanging(nameof (Description));
        this._Description = value;
        this.OnPropertyChanged(nameof (Description));
      }
    }
  }
}
