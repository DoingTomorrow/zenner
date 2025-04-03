// Decompiled with JetBrains decompiler
// Type: ApplicationScope.t_Minomat
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.ClientServices.SQLite;
using System;

#nullable disable
namespace ApplicationScope
{
  public class t_Minomat : SQLiteOfflineEntity
  {
    private Guid _Id;
    private string _RadioId;
    private string _Status;
    private bool? _Registered;
    private string _HostAndPort;
    private string _Url;
    private string _CreatedBy;
    private DateTime? _CreatedOn;
    private string _LastUpdatedBy;
    private DateTime? _LastChangedOn;
    private string _Challenge;
    private string _GsmId;
    private bool? _IsDeactivated;
    private DateTime? _StartDate;
    private DateTime? _EndDate;
    private int? _Polling;
    private bool? _IsMaster;
    private bool? _IsInMasterPool;
    private string _ProviderName;
    private string _SimPin;
    private string _AccessPoint;
    private string _UserId;
    private string _UserPassword;
    private string _SessionKey;
    private string _CommParameter;
    private string _CreatedByName;
    private Guid? _ScenarioId;
    private Guid? _ProviderId;
    private Guid? _CountryId;
    private bool? _LoggingEnabled;
    private string _MinomatMasterId;
    private string _SimCardNumber;

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
    public string RadioId
    {
      get => this._RadioId;
      set
      {
        this.OnPropertyChanging(nameof (RadioId));
        this._RadioId = value;
        this.OnPropertyChanged(nameof (RadioId));
      }
    }

    [MaxLength(255)]
    public string Status
    {
      get => this._Status;
      set
      {
        this.OnPropertyChanging(nameof (Status));
        this._Status = value;
        this.OnPropertyChanged(nameof (Status));
      }
    }

    public bool? Registered
    {
      get => this._Registered;
      set
      {
        this.OnPropertyChanging(nameof (Registered));
        this._Registered = value;
        this.OnPropertyChanged(nameof (Registered));
      }
    }

    [MaxLength(255)]
    public string HostAndPort
    {
      get => this._HostAndPort;
      set
      {
        this.OnPropertyChanging(nameof (HostAndPort));
        this._HostAndPort = value;
        this.OnPropertyChanged(nameof (HostAndPort));
      }
    }

    [MaxLength(255)]
    public string Url
    {
      get => this._Url;
      set
      {
        this.OnPropertyChanging(nameof (Url));
        this._Url = value;
        this.OnPropertyChanged(nameof (Url));
      }
    }

    [MaxLength(255)]
    public string CreatedBy
    {
      get => this._CreatedBy;
      set
      {
        this.OnPropertyChanging(nameof (CreatedBy));
        this._CreatedBy = value;
        this.OnPropertyChanged(nameof (CreatedBy));
      }
    }

    public DateTime? CreatedOn
    {
      get => this._CreatedOn;
      set
      {
        this.OnPropertyChanging(nameof (CreatedOn));
        this._CreatedOn = value;
        this.OnPropertyChanged(nameof (CreatedOn));
      }
    }

    [MaxLength(255)]
    public string LastUpdatedBy
    {
      get => this._LastUpdatedBy;
      set
      {
        this.OnPropertyChanging(nameof (LastUpdatedBy));
        this._LastUpdatedBy = value;
        this.OnPropertyChanged(nameof (LastUpdatedBy));
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

    [MaxLength(255)]
    public string Challenge
    {
      get => this._Challenge;
      set
      {
        this.OnPropertyChanging(nameof (Challenge));
        this._Challenge = value;
        this.OnPropertyChanged(nameof (Challenge));
      }
    }

    [MaxLength(255)]
    public string GsmId
    {
      get => this._GsmId;
      set
      {
        this.OnPropertyChanging(nameof (GsmId));
        this._GsmId = value;
        this.OnPropertyChanged(nameof (GsmId));
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

    public int? Polling
    {
      get => this._Polling;
      set
      {
        this.OnPropertyChanging(nameof (Polling));
        this._Polling = value;
        this.OnPropertyChanged(nameof (Polling));
      }
    }

    public bool? IsMaster
    {
      get => this._IsMaster;
      set
      {
        this.OnPropertyChanging(nameof (IsMaster));
        this._IsMaster = value;
        this.OnPropertyChanged(nameof (IsMaster));
      }
    }

    public bool? IsInMasterPool
    {
      get => this._IsInMasterPool;
      set
      {
        this.OnPropertyChanging(nameof (IsInMasterPool));
        this._IsInMasterPool = value;
        this.OnPropertyChanged(nameof (IsInMasterPool));
      }
    }

    [MaxLength(255)]
    public string ProviderName
    {
      get => this._ProviderName;
      set
      {
        this.OnPropertyChanging(nameof (ProviderName));
        this._ProviderName = value;
        this.OnPropertyChanged(nameof (ProviderName));
      }
    }

    [MaxLength(255)]
    public string SimPin
    {
      get => this._SimPin;
      set
      {
        this.OnPropertyChanging(nameof (SimPin));
        this._SimPin = value;
        this.OnPropertyChanged(nameof (SimPin));
      }
    }

    [MaxLength(255)]
    public string AccessPoint
    {
      get => this._AccessPoint;
      set
      {
        this.OnPropertyChanging(nameof (AccessPoint));
        this._AccessPoint = value;
        this.OnPropertyChanged(nameof (AccessPoint));
      }
    }

    [MaxLength(255)]
    public string UserId
    {
      get => this._UserId;
      set
      {
        this.OnPropertyChanging(nameof (UserId));
        this._UserId = value;
        this.OnPropertyChanged(nameof (UserId));
      }
    }

    [MaxLength(255)]
    public string UserPassword
    {
      get => this._UserPassword;
      set
      {
        this.OnPropertyChanging(nameof (UserPassword));
        this._UserPassword = value;
        this.OnPropertyChanged(nameof (UserPassword));
      }
    }

    [MaxLength(255)]
    public string SessionKey
    {
      get => this._SessionKey;
      set
      {
        this.OnPropertyChanging(nameof (SessionKey));
        this._SessionKey = value;
        this.OnPropertyChanged(nameof (SessionKey));
      }
    }

    [MaxLength(255)]
    public string CommParameter
    {
      get => this._CommParameter;
      set
      {
        this.OnPropertyChanging(nameof (CommParameter));
        this._CommParameter = value;
        this.OnPropertyChanged(nameof (CommParameter));
      }
    }

    [MaxLength(255)]
    public string CreatedByName
    {
      get => this._CreatedByName;
      set
      {
        this.OnPropertyChanging(nameof (CreatedByName));
        this._CreatedByName = value;
        this.OnPropertyChanged(nameof (CreatedByName));
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

    public Guid? ProviderId
    {
      get => this._ProviderId;
      set
      {
        this.OnPropertyChanging(nameof (ProviderId));
        this._ProviderId = value;
        this.OnPropertyChanged(nameof (ProviderId));
      }
    }

    public Guid? CountryId
    {
      get => this._CountryId;
      set
      {
        this.OnPropertyChanging(nameof (CountryId));
        this._CountryId = value;
        this.OnPropertyChanged(nameof (CountryId));
      }
    }

    public bool? LoggingEnabled
    {
      get => this._LoggingEnabled;
      set
      {
        this.OnPropertyChanging(nameof (LoggingEnabled));
        this._LoggingEnabled = value;
        this.OnPropertyChanged(nameof (LoggingEnabled));
      }
    }

    [MaxLength(255)]
    public string MinomatMasterId
    {
      get => this._MinomatMasterId;
      set
      {
        this.OnPropertyChanging(nameof (MinomatMasterId));
        this._MinomatMasterId = value;
        this.OnPropertyChanged(nameof (MinomatMasterId));
      }
    }

    public string SimCardNumber
    {
      get => this._SimCardNumber;
      set
      {
        this.OnPropertyChanging(nameof (SimCardNumber));
        this._SimCardNumber = value;
        this.OnPropertyChanged(nameof (SimCardNumber));
      }
    }
  }
}
