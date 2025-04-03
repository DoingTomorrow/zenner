// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_StructureNodeLinks
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_StructureNodeLinks : SQLiteOfflineEntity
  {
    private Guid _Id;
    private Guid? _NodeId;
    private Guid? _ParentNodeId;
    private Guid? _RootNodeId;
    private string _StructureType;
    private DateTime? _StartDate;
    private DateTime? _EndDate;
    private int? _OrderNr;
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

    public Guid? NodeId
    {
      get => this._NodeId;
      set
      {
        this.OnPropertyChanging(nameof (NodeId));
        this._NodeId = value;
        this.OnPropertyChanged(nameof (NodeId));
      }
    }

    public Guid? ParentNodeId
    {
      get => this._ParentNodeId;
      set
      {
        this.OnPropertyChanging(nameof (ParentNodeId));
        this._ParentNodeId = value;
        this.OnPropertyChanged(nameof (ParentNodeId));
      }
    }

    public Guid? RootNodeId
    {
      get => this._RootNodeId;
      set
      {
        this.OnPropertyChanging(nameof (RootNodeId));
        this._RootNodeId = value;
        this.OnPropertyChanged(nameof (RootNodeId));
      }
    }

    [MaxLength(20)]
    public string StructureType
    {
      get => this._StructureType;
      set
      {
        this.OnPropertyChanging(nameof (StructureType));
        this._StructureType = value;
        this.OnPropertyChanged(nameof (StructureType));
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

    public int? OrderNr
    {
      get => this._OrderNr;
      set
      {
        this.OnPropertyChanging(nameof (OrderNr));
        this._OrderNr = value;
        this.OnPropertyChanged(nameof (OrderNr));
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
