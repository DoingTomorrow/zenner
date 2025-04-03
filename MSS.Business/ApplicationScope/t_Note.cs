// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_Note
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_Note : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _NoteDescription;
    private Guid? _NoteTypeId;
    private DateTime? _LastChangedOn;
    private Guid? _StructureNodeId;

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

    [MaxLength(255)]
    public string NoteDescription
    {
      get => this._NoteDescription;
      set
      {
        this.OnPropertyChanging(nameof (NoteDescription));
        this._NoteDescription = value;
        this.OnPropertyChanged(nameof (NoteDescription));
      }
    }

    public Guid? NoteTypeId
    {
      get => this._NoteTypeId;
      set
      {
        this.OnPropertyChanging(nameof (NoteTypeId));
        this._NoteTypeId = value;
        this.OnPropertyChanged(nameof (NoteTypeId));
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

    public Guid? StructureNodeId
    {
      get => this._StructureNodeId;
      set
      {
        this.OnPropertyChanging(nameof (StructureNodeId));
        this._StructureNodeId = value;
        this.OnPropertyChanged(nameof (StructureNodeId));
      }
    }
  }
}
