// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_Rules
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_Rules : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _PhysicalQuantity;
    private string _MeterType;
    private string _Calculation;
    private string _CalculationStart;
    private string _StorageInterval;
    private string _Creation;
    private int _RuleIndex;
    private string _ValueId;
    private Guid? _FilterId;

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
    public string PhysicalQuantity
    {
      get => this._PhysicalQuantity;
      set
      {
        this.OnPropertyChanging(nameof (PhysicalQuantity));
        this._PhysicalQuantity = value;
        this.OnPropertyChanged(nameof (PhysicalQuantity));
      }
    }

    [MaxLength(100)]
    public string MeterType
    {
      get => this._MeterType;
      set
      {
        this.OnPropertyChanging(nameof (MeterType));
        this._MeterType = value;
        this.OnPropertyChanged(nameof (MeterType));
      }
    }

    [MaxLength(100)]
    public string Calculation
    {
      get => this._Calculation;
      set
      {
        this.OnPropertyChanging(nameof (Calculation));
        this._Calculation = value;
        this.OnPropertyChanged(nameof (Calculation));
      }
    }

    [MaxLength(100)]
    public string CalculationStart
    {
      get => this._CalculationStart;
      set
      {
        this.OnPropertyChanging(nameof (CalculationStart));
        this._CalculationStart = value;
        this.OnPropertyChanged(nameof (CalculationStart));
      }
    }

    [MaxLength(100)]
    public string StorageInterval
    {
      get => this._StorageInterval;
      set
      {
        this.OnPropertyChanging(nameof (StorageInterval));
        this._StorageInterval = value;
        this.OnPropertyChanged(nameof (StorageInterval));
      }
    }

    [MaxLength(100)]
    public string Creation
    {
      get => this._Creation;
      set
      {
        this.OnPropertyChanging(nameof (Creation));
        this._Creation = value;
        this.OnPropertyChanged(nameof (Creation));
      }
    }

    public int RuleIndex
    {
      get => this._RuleIndex;
      set
      {
        this.OnPropertyChanging(nameof (RuleIndex));
        this._RuleIndex = value;
        this.OnPropertyChanged(nameof (RuleIndex));
      }
    }

    [MaxLength(255)]
    public string ValueId
    {
      get => this._ValueId;
      set
      {
        this.OnPropertyChanging(nameof (ValueId));
        this._ValueId = value;
        this.OnPropertyChanged(nameof (ValueId));
      }
    }

    public Guid? FilterId
    {
      get => this._FilterId;
      set
      {
        this.OnPropertyChanging(nameof (FilterId));
        this._FilterId = value;
        this.OnPropertyChanged(nameof (FilterId));
      }
    }
  }
}
