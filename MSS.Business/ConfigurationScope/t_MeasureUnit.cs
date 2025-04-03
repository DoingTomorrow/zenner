// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_MeasureUnit
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_MeasureUnit : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _Code;
    private string _CelestaCode;

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
    public string Code
    {
      get => this._Code;
      set
      {
        this.OnPropertyChanging(nameof (Code));
        this._Code = value;
        this.OnPropertyChanged(nameof (Code));
      }
    }

    [MaxLength(255)]
    public string CelestaCode
    {
      get => this._CelestaCode;
      set
      {
        this.OnPropertyChanging(nameof (CelestaCode));
        this._CelestaCode = value;
        this.OnPropertyChanged(nameof (CelestaCode));
      }
    }
  }
}
