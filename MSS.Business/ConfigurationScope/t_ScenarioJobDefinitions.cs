// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_ScenarioJobDefinitions
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_ScenarioJobDefinitions : SQLiteOfflineEntity
  {
    private Guid _Id;
    private Guid? _ScenarioId;
    private Guid? _JobDefinitionId;

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

    public Guid? ScenarioId
    {
      get => this._ScenarioId;
      set
      {
        this.OnPropertyChanging(nameof (ScenarioId));
        this._ScenarioId = value;
        this.OnPropertyChanged(nameof (ScenarioId));
      }
    }

    public Guid? JobDefinitionId
    {
      get => this._JobDefinitionId;
      set
      {
        this.OnPropertyChanging(nameof (JobDefinitionId));
        this._JobDefinitionId = value;
        this.OnPropertyChanged(nameof (JobDefinitionId));
      }
    }
  }
}
