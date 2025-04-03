// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_StructureNode
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_StructureNode : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _Name;
    private string _Description;
    private Guid? _EntityId;
    private string _EntityName;
    private DateTime? _StartDate;
    private DateTime? _EndDate;
    private Guid? _NodeTypeId;
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

    public Guid? EntityId
    {
      get => this._EntityId;
      set
      {
        this.OnPropertyChanging(nameof (EntityId));
        this._EntityId = value;
        this.OnPropertyChanged(nameof (EntityId));
      }
    }

    [MaxLength(200)]
    public string EntityName
    {
      get => this._EntityName;
      set
      {
        this.OnPropertyChanging(nameof (EntityName));
        this._EntityName = value;
        this.OnPropertyChanged(nameof (EntityName));
      }
    }

    public DateTime? StartDate
    {
      get => this._StartDate;
      set
      {
        this.OnPropertyChanging(nameof (StartDate));
        this._StartDate = value;
        this.OnPropertyChanged(nameof (StartDate));
      }
    }

    public DateTime? EndDate
    {
      get => this._EndDate;
      set
      {
        this.OnPropertyChanging(nameof (EndDate));
        this._EndDate = value;
        this.OnPropertyChanged(nameof (EndDate));
      }
    }

    public Guid? NodeTypeId
    {
      get => this._NodeTypeId;
      set
      {
        this.OnPropertyChanging(nameof (NodeTypeId));
        this._NodeTypeId = value;
        this.OnPropertyChanged(nameof (NodeTypeId));
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
