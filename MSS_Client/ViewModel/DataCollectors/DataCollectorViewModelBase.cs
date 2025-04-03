// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataCollectors.DataCollectorViewModelBase
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Windows.Data;
using ZENNER;

#nullable disable
namespace MSS_Client.ViewModel.DataCollectors
{
  public class DataCollectorViewModelBase : ValidationViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    protected Minomat selectedMinomat;
    private bool _isInMasterPool;
    private bool _isMaster;
    private StatusMinomatEnum _status;
    private string _masterRadioId;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private Provider _provider;
    private string _simPin;
    private string _accessPoint;
    private string _userId;
    private string _userPassword;
    private bool _registered;
    private bool _notRegistered;
    private string _challenge;
    private string _gsmId;
    private int? _polling;
    private string _hostandPort;
    private string _url;
    private DateTime? _dateAppended;
    private string _appendedBy;
    private string _sessionKey;
    private Country _selectedCountry;
    private string _simCardNumber;
    private bool _minomatScenarioEditable;
    private ScenarioDTO _selectedScenario;
    private List<ScenarioDTO> _scenarioList = new List<ScenarioDTO>();
    private bool _masterPoolAddVisibility;

    public DataCollectorViewModelBase(IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      UsersManager usersManager = new UsersManager(this._repositoryFactory);
      this.MasterPoolAddVisibility = usersManager.HasRight(OperationEnum.MasterPoolAdd.ToString());
      this.IsMinomatScenarioEditable = usersManager.HasRight(OperationEnum.MinomatScenarioEdit.ToString());
      this.Registered = false;
      this.IsInMasterPool = this.MasterPoolAddVisibility;
      this.AppendedBy = string.Format("{0} {1}", (object) MSS.Business.Utils.AppContext.Current.LoggedUser.FirstName, (object) MSS.Business.Utils.AppContext.Current.LoggedUser.LastName);
      this.DateAppended = new DateTime?(DateTime.Now);
      IRepository<ApplicationParameter> repository = this._repositoryFactory.GetRepository<ApplicationParameter>();
      int result;
      int.TryParse(repository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (Polling))).Value, out result);
      this.Polling = new int?(result);
      this.HostAndPort = repository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (HostAndPort))).Value;
      this.Url = repository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (Url))).Value;
      this.Challenge = MeterListenerManager.CreateRandomChallengeKey().ToString();
      this.GsmId = string.Empty;
      this.SessionKey = MeterListenerManager.CreateRandomSessionKey().ToString();
    }

    public RadObservableCollection<StatusMinomatEnum> GetListofStatuses
    {
      get
      {
        RadObservableCollection<StatusMinomatEnum> getListofStatuses = new RadObservableCollection<StatusMinomatEnum>();
        List<StatusMinomatEnum> list = Enum.GetValues(typeof (StatusMinomatEnum)).Cast<StatusMinomatEnum>().ToList<StatusMinomatEnum>();
        if (list.Any<StatusMinomatEnum>())
          getListofStatuses = new RadObservableCollection<StatusMinomatEnum>((IEnumerable<StatusMinomatEnum>) list);
        return getListofStatuses;
      }
    }

    public RadObservableCollection<Provider> GetListofProviders
    {
      get
      {
        RadObservableCollection<Provider> getListofProviders = new RadObservableCollection<Provider>();
        IOrderedEnumerable<Provider> orderedEnumerable = this._repositoryFactory.GetRepository<Provider>().GetAll().OrderBy<Provider, string>((Func<Provider, string>) (p => p.ProviderName));
        if (orderedEnumerable.Any<Provider>())
          getListofProviders = new RadObservableCollection<Provider>((IEnumerable<Provider>) orderedEnumerable);
        return getListofProviders;
      }
    }

    public System.Windows.Input.ICommand CreateMinomatCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (!this.IsValid)
            return;
          Minomat minomat = new Minomat()
          {
            RadioId = this.MasterRadioId,
            StartDate = this.StartDate,
            EndDate = this.EndDate,
            Provider = this.SelectedProvider,
            Status = this.Status.ToString(),
            Registered = this.Registered,
            Challenge = this.Challenge,
            GsmId = this.GsmId,
            Polling = this.Polling ?? 0,
            HostAndPort = this.HostAndPort,
            Url = this.Url,
            CreatedOn = new DateTime?(this.DateAppended ?? new DateTime(1800, 1, 1)),
            CreatedBy = MSS.Business.Utils.AppContext.Current.LoggedUser.Id.ToString(),
            CreatedByName = string.Format("{0} {1}", (object) MSS.Business.Utils.AppContext.Current.LoggedUser.FirstName, (object) MSS.Business.Utils.AppContext.Current.LoggedUser.LastName),
            IsDeactivated = false,
            IsInMasterPool = this.IsInMasterPool,
            SimPin = this.SimPin,
            AccessPoint = this.AccessPoint,
            UserId = this.UserId,
            UserPassword = this.UserPassword,
            SessionKey = this.SessionKey,
            IsMaster = true,
            Country = this.SelectedCountry,
            Scenario = this.SelectedScenario != null ? this._repositoryFactory.GetRepository<Scenario>().GetById((object) this.SelectedScenario.Id) : (Scenario) null,
            SimCardNumber = this.SimCardNumber ?? ""
          };
          this._repositoryFactory.GetRepository<Minomat>().Insert(minomat);
          MessageHandler.LogDebug("Minomat created from Minomats module. RadioId: " + minomat.RadioId);
          MinomatJobsManager.AddMinomat(minomat);
          this.OnRequestClose(true);
        }));
      }
    }

    public System.Windows.Input.ICommand EditMinomatCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (!this.IsMaster)
            this.IsValid = ((this.IsValid ? 1 : 0) | (string.IsNullOrEmpty(this.MasterRadioId) ? 0 : (this.SelectedCountry != null ? 1 : 0))) != 0;
          if (!this.IsValid)
            return;
          this.selectedMinomat = this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (x => x.Id == this.selectedMinomat.Id));
          this.selectedMinomat.RadioId = this.MasterRadioId;
          this.selectedMinomat.Provider = this.SelectedProvider;
          this.selectedMinomat.GsmId = this.GsmId;
          this.selectedMinomat.Polling = this.Polling ?? 0;
          this.selectedMinomat.HostAndPort = this.HostAndPort;
          this.selectedMinomat.Url = this.Url;
          this.selectedMinomat.IsInMasterPool = this.IsInMasterPool;
          this.selectedMinomat.SimPin = this.SimPin;
          this.selectedMinomat.AccessPoint = this.AccessPoint;
          this.selectedMinomat.UserId = this.UserId;
          this.selectedMinomat.UserPassword = this.UserPassword;
          this.selectedMinomat.SessionKey = this.SessionKey;
          this.selectedMinomat.Country = this.SelectedCountry;
          this.selectedMinomat.Scenario = this.SelectedScenario != null ? this._repositoryFactory.GetRepository<Scenario>().GetById((object) this.SelectedScenario.Id) : (Scenario) null;
          this.selectedMinomat.IsMaster = this.IsMaster;
          this.selectedMinomat.LastUpdatedBy = string.Format("{0} {1}", (object) MSS.Business.Utils.AppContext.Current.LoggedUser.FirstName, (object) MSS.Business.Utils.AppContext.Current.LoggedUser.LastName);
          this.selectedMinomat.SimCardNumber = this.SimCardNumber ?? "";
          this._repositoryFactory.GetRepository<Minomat>().Update(this.selectedMinomat);
          EventPublisher.Publish<MinomatUpdate>(new MinomatUpdate()
          {
            IsUpdate = true,
            Ids = new List<Guid>()
            {
              this.selectedMinomat.Id
            }
          }, (IViewModel) this);
          this.OnRequestClose(true);
        }));
      }
    }

    public System.Windows.Input.ICommand DeleteMinomatCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) delegate
        {
          this.selectedMinomat = this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (x => x.Id == this.selectedMinomat.Id));
          this.selectedMinomat.IsDeactivated = true;
          this.selectedMinomat.EndDate = new DateTime?(DateTime.Now);
          this._repositoryFactory.GetRepository<Minomat>().Update(this.selectedMinomat);
          if (this.selectedMinomat.GsmId != null && MeterListenerManager.GetMinomat(Convert.ToUInt32(this.selectedMinomat.GsmId)) != null)
            MeterListenerManager.DeleteMinomat(Convert.ToUInt32(this.selectedMinomat.GsmId));
          EventPublisher.Publish<MinomatUpdate>(new MinomatUpdate()
          {
            IsUpdate = true,
            Ids = new List<Guid>()
            {
              this.selectedMinomat.Id
            }
          }, (IViewModel) this);
          this.OnRequestClose(true);
        });
      }
    }

    public bool IsInMasterPool
    {
      get => this._isInMasterPool;
      set
      {
        this._isInMasterPool = value;
        this.OnPropertyChanged(nameof (IsInMasterPool));
      }
    }

    public bool IsMaster
    {
      get => this._isMaster;
      set
      {
        this._isMaster = value;
        this.OnPropertyChanged(nameof (IsMaster));
      }
    }

    public StatusMinomatEnum Status
    {
      get => this._status;
      set
      {
        this._status = value;
        this.OnPropertyChanged(nameof (Status));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_MASTER_RADIO_ID")]
    [Range(47100000, 48099999, ErrorMessage = "MSS_MINOMATS_RADIO_ID_BETWEEN")]
    public string MasterRadioId
    {
      get => this._masterRadioId;
      set
      {
        this._masterRadioId = value;
        this.OnPropertyChanged(nameof (MasterRadioId));
        this.GsmId = value;
      }
    }

    public DateTime? StartDate
    {
      get => this._startDate;
      set
      {
        this._startDate = value;
        this.OnPropertyChanged(nameof (StartDate));
      }
    }

    public DateTime? EndDate
    {
      get => this._endDate;
      set
      {
        this._endDate = value;
        this.OnPropertyChanged(nameof (EndDate));
      }
    }

    public Provider SelectedProvider
    {
      get => this._provider;
      set
      {
        this._provider = value;
        this.SimPin = this._provider.SimPin;
        this.AccessPoint = this._provider.AccessPoint;
        this.UserId = this._provider.UserId;
        this.UserPassword = this._provider.UserPassword;
        this.OnPropertyChanged(nameof (SelectedProvider));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_SIMPIN")]
    public string SimPin
    {
      get => this._simPin;
      set
      {
        this._simPin = value;
        this.OnPropertyChanged(nameof (SimPin));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_ACCESSPOINT")]
    public string AccessPoint
    {
      get => this._accessPoint;
      set
      {
        this._accessPoint = value;
        this.OnPropertyChanged(nameof (AccessPoint));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_USERID")]
    public string UserId
    {
      get => this._userId;
      set
      {
        this._userId = value;
        this.OnPropertyChanged(nameof (UserId));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_USERPASS")]
    public string UserPassword
    {
      get => this._userPassword;
      set
      {
        this._userPassword = value;
        this.OnPropertyChanged(nameof (UserPassword));
      }
    }

    public bool Registered
    {
      get => this._registered;
      set
      {
        this._registered = value;
        this.OnPropertyChanged(nameof (Registered));
      }
    }

    public bool NotRegistered
    {
      get => this._notRegistered;
      set
      {
        this._notRegistered = value;
        this.OnPropertyChanged(nameof (NotRegistered));
      }
    }

    public string Challenge
    {
      get => this._challenge;
      set
      {
        this._challenge = value;
        this.OnPropertyChanged(nameof (Challenge));
      }
    }

    public string GsmId
    {
      get => this._gsmId;
      set
      {
        this._gsmId = value;
        this.OnPropertyChanged(nameof (GsmId));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_POLLING_REQUIRED")]
    public int? Polling
    {
      get => this._polling;
      set
      {
        this._polling = value;
        this.OnPropertyChanged(nameof (Polling));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_HOSTANDPORT_REQUIRED")]
    public string HostAndPort
    {
      get => this._hostandPort;
      set
      {
        this._hostandPort = value;
        this.OnPropertyChanged(nameof (HostAndPort));
      }
    }

    public string Url
    {
      get => this._url;
      set
      {
        this._url = value;
        this.OnPropertyChanged(nameof (Url));
      }
    }

    public DateTime? DateAppended
    {
      get => this._dateAppended;
      set
      {
        this._dateAppended = value;
        this.OnPropertyChanged(nameof (DateAppended));
      }
    }

    public string AppendedBy
    {
      get => this._appendedBy;
      set
      {
        this._appendedBy = value;
        this.OnPropertyChanged(nameof (AppendedBy));
      }
    }

    public string SessionKey
    {
      get => this._sessionKey;
      set
      {
        this._sessionKey = value;
        this.OnPropertyChanged(nameof (SessionKey));
      }
    }

    [Required(ErrorMessage = "MSS_MINOMAT_COUNTRY_REQUIRED")]
    public Country SelectedCountry
    {
      get => this._selectedCountry;
      set
      {
        this._selectedCountry = value;
        this.OnPropertyChanged(nameof (SelectedCountry));
      }
    }

    public string SimCardNumber
    {
      get => this._simCardNumber;
      set
      {
        this._simCardNumber = value;
        this.OnPropertyChanged(nameof (SimCardNumber));
      }
    }

    public RadObservableCollection<Country> GetListofCountries
    {
      get
      {
        RadObservableCollection<Country> getListofCountries = new RadObservableCollection<Country>();
        IOrderedEnumerable<Country> orderedEnumerable = this._repositoryFactory.GetRepository<Country>().GetAll().OrderBy<Country, string>((Func<Country, string>) (c => c.Name));
        if (orderedEnumerable.Any<Country>())
          getListofCountries = new RadObservableCollection<Country>((IEnumerable<Country>) orderedEnumerable);
        return getListofCountries;
      }
    }

    public bool IsMinomatScenarioEditable
    {
      get => this._minomatScenarioEditable;
      set
      {
        this._minomatScenarioEditable = value;
        this.OnPropertyChanged(nameof (IsMinomatScenarioEditable));
      }
    }

    public ScenarioDTO SelectedScenario
    {
      get => this._selectedScenario;
      set
      {
        this._selectedScenario = value;
        this.OnPropertyChanged(nameof (SelectedScenario));
      }
    }

    public RadObservableCollection<ScenarioDTO> GetListofScenarios
    {
      get
      {
        RadObservableCollection<ScenarioDTO> observableCollection = new RadObservableCollection<ScenarioDTO>();
        if (this._scenarioList.Count == 0)
          this._scenarioList.AddRange(new JobsManager(this._repositoryFactory).GetScenarioDTOs());
        return new RadObservableCollection<ScenarioDTO>((IEnumerable<ScenarioDTO>) this._scenarioList);
      }
    }

    public bool MasterPoolAddVisibility
    {
      get => this._masterPoolAddVisibility;
      set
      {
        this._masterPoolAddVisibility = value;
        this.OnPropertyChanged(nameof (MasterPoolAddVisibility));
      }
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.MasterRadioId));
      string propertyName2 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.SimCardNumber));
      if (propertyName == propertyName1)
      {
        ICollection<string> validationErrors;
        this.ValidateMasterRadioId(this.MasterRadioId, out validationErrors);
        this.IsValid &= validationErrors.Count <= 0;
        return validationErrors.ToList<string>();
      }
      if (!(propertyName == propertyName2) || string.IsNullOrEmpty(this._simCardNumber))
        return new List<string>();
      ICollection<string> validationErrors1;
      this.ValidateSimCardNumber(this._simCardNumber, out validationErrors1);
      if (validationErrors1.Count > 0)
        this.IsValid = false;
      return validationErrors1.ToList<string>();
    }

    public bool ValidateMasterRadioId(
      string masterRadioId,
      out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      IList<Minomat> source = this._repositoryFactory.GetRepository<Minomat>().SearchFor((Expression<Func<Minomat, bool>>) (x => x.RadioId == masterRadioId && x.IsDeactivated == false));
      if (this.selectedMinomat == null && source.Any<Minomat>())
        validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
      else if (source.Any<Minomat>() && source.FirstOrDefault<Minomat>((Func<Minomat, bool>) (x => x.RadioId != null && x.RadioId != this.selectedMinomat.RadioId)) != null)
        validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
      return validationErrors.Count == 0;
    }

    public bool ValidateSimCardNumber(
      string simCardNumber,
      out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      if (!string.IsNullOrEmpty(simCardNumber) && simCardNumber.Length != 30)
        validationErrors.Add(Resources.MSS_MinomatMaster_SimCardNumber_TooFewDigits);
      return validationErrors.Count == 0;
    }
  }
}
