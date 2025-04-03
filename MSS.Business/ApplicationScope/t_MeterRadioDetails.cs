// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_MeterRadioDetails
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_MeterRadioDetails : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _dgMessbereich;
    private string _dgRealErfasser;
    private string _dgReg1DakonSerNr;
    private string _dgReg1Flag;
    private string _dgReg1Mode;
    private string _dgReg1Signal;
    private string _dgReg2DakonSerNr;
    private string _dgReg2Flag;
    private string _dgReg2Mode;
    private string _dgReg2Signal;
    private string _dgReg3DakonSernr;
    private string _dgReg3Flag;
    private string _dgReg3Mode;
    private string _dgReg3Signal;
    private string _dgZaehlerNr;
    private string _Street;
    private string _GemSerialNumber;
    private string _Scenario;
    private Guid? _MeterId;
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

    [MaxLength(20)]
    public string dgMessbereich
    {
      get => this._dgMessbereich;
      set
      {
        this.OnPropertyChanging(nameof (dgMessbereich));
        this._dgMessbereich = value;
        this.OnPropertyChanged(nameof (dgMessbereich));
      }
    }

    [MaxLength(10)]
    public string dgRealErfasser
    {
      get => this._dgRealErfasser;
      set
      {
        this.OnPropertyChanging(nameof (dgRealErfasser));
        this._dgRealErfasser = value;
        this.OnPropertyChanged(nameof (dgRealErfasser));
      }
    }

    [MaxLength(12)]
    public string dgReg1DakonSerNr
    {
      get => this._dgReg1DakonSerNr;
      set
      {
        this.OnPropertyChanging(nameof (dgReg1DakonSerNr));
        this._dgReg1DakonSerNr = value;
        this.OnPropertyChanged(nameof (dgReg1DakonSerNr));
      }
    }

    [MaxLength(5)]
    public string dgReg1Flag
    {
      get => this._dgReg1Flag;
      set
      {
        this.OnPropertyChanging(nameof (dgReg1Flag));
        this._dgReg1Flag = value;
        this.OnPropertyChanged(nameof (dgReg1Flag));
      }
    }

    [MaxLength(20)]
    public string dgReg1Mode
    {
      get => this._dgReg1Mode;
      set
      {
        this.OnPropertyChanging(nameof (dgReg1Mode));
        this._dgReg1Mode = value;
        this.OnPropertyChanged(nameof (dgReg1Mode));
      }
    }

    [MaxLength(3)]
    public string dgReg1Signal
    {
      get => this._dgReg1Signal;
      set
      {
        this.OnPropertyChanging(nameof (dgReg1Signal));
        this._dgReg1Signal = value;
        this.OnPropertyChanged(nameof (dgReg1Signal));
      }
    }

    [MaxLength(12)]
    public string dgReg2DakonSerNr
    {
      get => this._dgReg2DakonSerNr;
      set
      {
        this.OnPropertyChanging(nameof (dgReg2DakonSerNr));
        this._dgReg2DakonSerNr = value;
        this.OnPropertyChanged(nameof (dgReg2DakonSerNr));
      }
    }

    [MaxLength(5)]
    public string dgReg2Flag
    {
      get => this._dgReg2Flag;
      set
      {
        this.OnPropertyChanging(nameof (dgReg2Flag));
        this._dgReg2Flag = value;
        this.OnPropertyChanged(nameof (dgReg2Flag));
      }
    }

    [MaxLength(20)]
    public string dgReg2Mode
    {
      get => this._dgReg2Mode;
      set
      {
        this.OnPropertyChanging(nameof (dgReg2Mode));
        this._dgReg2Mode = value;
        this.OnPropertyChanged(nameof (dgReg2Mode));
      }
    }

    [MaxLength(3)]
    public string dgReg2Signal
    {
      get => this._dgReg2Signal;
      set
      {
        this.OnPropertyChanging(nameof (dgReg2Signal));
        this._dgReg2Signal = value;
        this.OnPropertyChanged(nameof (dgReg2Signal));
      }
    }

    [MaxLength(12)]
    public string dgReg3DakonSernr
    {
      get => this._dgReg3DakonSernr;
      set
      {
        this.OnPropertyChanging(nameof (dgReg3DakonSernr));
        this._dgReg3DakonSernr = value;
        this.OnPropertyChanged(nameof (dgReg3DakonSernr));
      }
    }

    [MaxLength(5)]
    public string dgReg3Flag
    {
      get => this._dgReg3Flag;
      set
      {
        this.OnPropertyChanging(nameof (dgReg3Flag));
        this._dgReg3Flag = value;
        this.OnPropertyChanged(nameof (dgReg3Flag));
      }
    }

    [MaxLength(20)]
    public string dgReg3Mode
    {
      get => this._dgReg3Mode;
      set
      {
        this.OnPropertyChanging(nameof (dgReg3Mode));
        this._dgReg3Mode = value;
        this.OnPropertyChanged(nameof (dgReg3Mode));
      }
    }

    [MaxLength(3)]
    public string dgReg3Signal
    {
      get => this._dgReg3Signal;
      set
      {
        this.OnPropertyChanging(nameof (dgReg3Signal));
        this._dgReg3Signal = value;
        this.OnPropertyChanged(nameof (dgReg3Signal));
      }
    }

    [MaxLength(16)]
    public string dgZaehlerNr
    {
      get => this._dgZaehlerNr;
      set
      {
        this.OnPropertyChanging(nameof (dgZaehlerNr));
        this._dgZaehlerNr = value;
        this.OnPropertyChanged(nameof (dgZaehlerNr));
      }
    }

    [MaxLength(320)]
    public string Street
    {
      get => this._Street;
      set
      {
        this.OnPropertyChanging(nameof (Street));
        this._Street = value;
        this.OnPropertyChanged(nameof (Street));
      }
    }

    [MaxLength(255)]
    public string GemSerialNumber
    {
      get => this._GemSerialNumber;
      set
      {
        this.OnPropertyChanging(nameof (GemSerialNumber));
        this._GemSerialNumber = value;
        this.OnPropertyChanged(nameof (GemSerialNumber));
      }
    }

    [MaxLength(255)]
    public string Scenario
    {
      get => this._Scenario;
      set
      {
        this.OnPropertyChanging(nameof (Scenario));
        this._Scenario = value;
        this.OnPropertyChanged(nameof (Scenario));
      }
    }

    public Guid? MeterId
    {
      get => this._MeterId;
      set
      {
        this.OnPropertyChanging(nameof (MeterId));
        this._MeterId = value;
        this.OnPropertyChanged(nameof (MeterId));
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
