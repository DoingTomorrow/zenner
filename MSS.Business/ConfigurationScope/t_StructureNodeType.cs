// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_StructureNodeType
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_StructureNodeType : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _Name;
    private string _IconPath;
    private bool? _IsFixed;
    private bool? _IsLogical;
    private bool? _IsPhysical;

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

    [MaxLength(200)]
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

    [MaxLength(200)]
    public string IconPath
    {
      get => this._IconPath;
      set
      {
        this.OnPropertyChanging(nameof (IconPath));
        this._IconPath = value;
        this.OnPropertyChanged(nameof (IconPath));
      }
    }

    public bool? IsFixed
    {
      get => this._IsFixed;
      set
      {
        this.OnPropertyChanging(nameof (IsFixed));
        this._IsFixed = value;
        this.OnPropertyChanged(nameof (IsFixed));
      }
    }

    public bool? IsLogical
    {
      get => this._IsLogical;
      set
      {
        this.OnPropertyChanging(nameof (IsLogical));
        this._IsLogical = value;
        this.OnPropertyChanged(nameof (IsLogical));
      }
    }

    public bool? IsPhysical
    {
      get => this._IsPhysical;
      set
      {
        this.OnPropertyChanging(nameof (IsPhysical));
        this._IsPhysical = value;
        this.OnPropertyChanged(nameof (IsPhysical));
      }
    }
  }
}
