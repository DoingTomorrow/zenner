// Decompiled with JetBrains decompiler
// Type: ConfigurationScope.t_JobDefinitions
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ConfigurationScope
{
  public class t_JobDefinitions : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _Name;
    private string _EquipmentModel;
    private string _System;
    private string _ServiceJob;
    private string _EquipmentParams;
    private string _ProfileType;
    private DateTime? _StartDate;
    private DateTime? _EndDate;
    private bool? _IsDeactivated;
    private long? _DueDate;
    private long? _Month;
    private long? _Day;
    private long? _QuarterHour;
    private byte[] _Interval;
    private Guid? _FilterId;
    private string _ProfileTypeParams;

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

    [MaxLength(255)]
    public string EquipmentModel
    {
      get => this._EquipmentModel;
      set
      {
        this.OnPropertyChanging(nameof (EquipmentModel));
        this._EquipmentModel = value;
        this.OnPropertyChanged(nameof (EquipmentModel));
      }
    }

    [MaxLength(255)]
    public string System
    {
      get => this._System;
      set
      {
        this.OnPropertyChanging(nameof (System));
        this._System = value;
        this.OnPropertyChanged(nameof (System));
      }
    }

    [MaxLength(255)]
    public string ServiceJob
    {
      get => this._ServiceJob;
      set
      {
        this.OnPropertyChanging(nameof (ServiceJob));
        this._ServiceJob = value;
        this.OnPropertyChanged(nameof (ServiceJob));
      }
    }

    [MaxLength(8000)]
    public string EquipmentParams
    {
      get => this._EquipmentParams;
      set
      {
        this.OnPropertyChanging(nameof (EquipmentParams));
        this._EquipmentParams = value;
        this.OnPropertyChanged(nameof (EquipmentParams));
      }
    }

    [MaxLength(255)]
    public string ProfileType
    {
      get => this._ProfileType;
      set
      {
        this.OnPropertyChanging(nameof (ProfileType));
        this._ProfileType = value;
        this.OnPropertyChanged(nameof (ProfileType));
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

    public bool? IsDeactivated
    {
      get => this._IsDeactivated;
      set
      {
        this.OnPropertyChanging(nameof (IsDeactivated));
        this._IsDeactivated = value;
        this.OnPropertyChanged(nameof (IsDeactivated));
      }
    }

    public long? DueDate
    {
      get => this._DueDate;
      set
      {
        this.OnPropertyChanging(nameof (DueDate));
        this._DueDate = value;
        this.OnPropertyChanged(nameof (DueDate));
      }
    }

    public long? Month
    {
      get => this._Month;
      set
      {
        this.OnPropertyChanging(nameof (Month));
        this._Month = value;
        this.OnPropertyChanged(nameof (Month));
      }
    }

    public long? Day
    {
      get => this._Day;
      set
      {
        this.OnPropertyChanging(nameof (Day));
        this._Day = value;
        this.OnPropertyChanged(nameof (Day));
      }
    }

    public long? QuarterHour
    {
      get => this._QuarterHour;
      set
      {
        this.OnPropertyChanging(nameof (QuarterHour));
        this._QuarterHour = value;
        this.OnPropertyChanged(nameof (QuarterHour));
      }
    }

    [MaxLength(8000)]
    public byte[] Interval
    {
      get => this._Interval;
      set
      {
        this.OnPropertyChanging(nameof (Interval));
        this._Interval = value;
        this.OnPropertyChanged(nameof (Interval));
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

    [MaxLength(8000)]
    public string ProfileTypeParams
    {
      get => this._ProfileTypeParams;
      set
      {
        this.OnPropertyChanging(nameof (ProfileTypeParams));
        this._ProfileTypeParams = value;
        this.OnPropertyChanged(nameof (ProfileTypeParams));
      }
    }
  }
}
