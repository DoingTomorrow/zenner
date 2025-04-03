// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_CelestaReadingDeviceTypes
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;

#nullable disable
namespace ConfigurationScope
{
  public class t_CelestaReadingDeviceTypes : SQLiteOfflineEntity
  {
    private int _Id;
    private int _Type;
    private string _Messbereich;
    private string _CelestaId;

    [PrimaryKey]
    public int Id
    {
      get => this._Id;
      set
      {
        this.OnPropertyChanging(nameof (Id));
        this._Id = value;
        this.OnPropertyChanged(nameof (Id));
      }
    }

    public int Type
    {
      get => this._Type;
      set
      {
        this.OnPropertyChanging(nameof (Type));
        this._Type = value;
        this.OnPropertyChanged(nameof (Type));
      }
    }

    [MaxLength(255)]
    public string Messbereich
    {
      get => this._Messbereich;
      set
      {
        this.OnPropertyChanging(nameof (Messbereich));
        this._Messbereich = value;
        this.OnPropertyChanged(nameof (Messbereich));
      }
    }

    [MaxLength(255)]
    public string CelestaId
    {
      get => this._CelestaId;
      set
      {
        this.OnPropertyChanging(nameof (CelestaId));
        this._CelestaId = value;
        this.OnPropertyChanged(nameof (CelestaId));
      }
    }
  }
}
