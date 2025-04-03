// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.EditMinomatViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.MessageHandler;
using MSS.DTO.Minomat;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class EditMinomatViewModel : ValidationViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private MSS.Core.Model.DataCollectors.Minomat selectedMinomat;
    private StructureNodeDTO _structureNode;
    private Country _selectedCountry;
    private bool _minomatScenarioEditable;
    private ScenarioDTO _selectedScenario;
    private List<ScenarioDTO> _scenarioList = new List<ScenarioDTO>();
    private string _name;
    private string _description;
    private bool _isInMasterPool;
    private bool _isMaster;
    private StatusMinomatEnum _status;
    private string _radioId;
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

    public EditMinomatViewModel(
      bool isExistingEntity,
      IRepositoryFactory repositoryFactory,
      MinomatSerializableDTO minomatSerializable,
      StructureNodeDTO node)
    {
      this._structureNode = node;
      this._repositoryFactory = repositoryFactory;
      Mapper.CreateMap<MinomatSerializableDTO, MinomatDTO>().ForMember((Expression<Func<MinomatDTO, object>>) (x => x.Provider), (Action<IMemberConfigurationExpression<MinomatSerializableDTO>>) (y => y.Ignore()));
      Mapper.CreateMap<MSS.Core.Model.DataCollectors.Minomat, MinomatDTO>();
      Mapper.CreateMap<MinomatDTO, MSS.Core.Model.DataCollectors.Minomat>();
      MinomatDTO minomat = new MinomatDTO();
      if (!isExistingEntity)
      {
        Mapper.Map<MSS.Core.Model.DataCollectors.Minomat, MinomatDTO>(this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().GetById((object) minomatSerializable.Id), minomat);
        Provider provider = this._repositoryFactory.GetRepository<Provider>().FirstOrDefault((Expression<Func<Provider, bool>>) (x => x.Id == minomat.Provider.Id));
        if (provider != null)
          minomat.Provider = provider;
      }
      else
      {
        Mapper.Map<MinomatSerializableDTO, MinomatDTO>(minomatSerializable, minomat);
        minomat.Country = this._repositoryFactory.GetRepository<Country>().GetById((object) minomatSerializable.CountryId);
        Guid providerId = minomatSerializable.ProviderId;
        if (minomatSerializable.ProviderId != Guid.Empty)
          minomat.Provider = this._repositoryFactory.GetRepository<Provider>().GetById((object) minomatSerializable.ProviderId);
      }
      this.selectedMinomat = Mapper.Map<MinomatDTO, MSS.Core.Model.DataCollectors.Minomat>(minomat);
      this._repositoryFactory = repositoryFactory;
      this.RadioId = minomat.RadioId;
      this.StartDate = minomat.StartDate;
      this.EndDate = minomat.EndDate;
      if (minomat.Provider != null)
      {
        Provider byId = this._repositoryFactory.GetRepository<Provider>().GetById((object) minomat.Provider.Id);
        if (byId != null)
          this.SelectedProvider = byId;
      }
      this.Status = (StatusMinomatEnum) Enum.ToObject(typeof (StatusMinomatEnum), minomat.idEnumStatus);
      this.Registered = minomat.Registered;
      this.NotRegistered = !minomat.Registered;
      this.Challenge = minomat.Challenge;
      this.GsmId = minomat.GsmId;
      this.Polling = new int?(minomat.Polling);
      this.HostAndPort = minomat.HostAndPort;
      this.Url = minomat.Url;
      this.DateAppended = minomat.CreatedOn;
      Guid createBy;
      if (Guid.TryParse(minomat.CreatedBy, out createBy))
      {
        User user = this._repositoryFactory.GetRepository<User>().FirstOrDefault((Expression<Func<User, bool>>) (x => x.Id == createBy));
        if (user != null)
          this.AppendedBy = string.Format("{0} {1}", (object) user.FirstName, (object) user.LastName);
      }
      else
        this.AppendedBy = minomat.CreatedBy;
      this.IsInMasterPool = minomat.IsInMasterPool;
      this.IsMaster = minomat.IsMaster;
      this.SimPin = minomat.SimPin;
      this.AccessPoint = minomat.AccessPoint;
      this.UserId = minomat.UserId;
      this.UserPassword = minomat.UserPassword;
      this.SessionKey = minomat.SessionKey;
      this.SelectedCountry = minomat.Country;
      this.SelectedScenario = minomat.Scenario != null ? this.GetListofScenarios.FirstOrDefault<ScenarioDTO>((Func<ScenarioDTO, bool>) (x => x.Id == minomat.Scenario.Id)) : (ScenarioDTO) null;
      this.Name = node.Name;
      this.Description = node.Description;
      this.IsMinomatScenarioEditable = new UsersManager(this._repositoryFactory).HasRight(OperationEnum.MinomatScenarioEdit.ToString());
    }

    public bool IsReading { get; set; }

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

    public ICommand EditMinomatCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (!this.IsMaster && !string.IsNullOrEmpty(this.RadioId) && this.SelectedCountry != null)
            this.IsValid = true;
          if (!this.IsValid)
            return;
          if (!this.IsReading)
            this.selectedMinomat = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().FirstOrDefault((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => x.Id == this.selectedMinomat.Id));
          this.selectedMinomat.RadioId = this.RadioId;
          this.selectedMinomat.Provider = this.SelectedProvider;
          this.selectedMinomat.GsmId = this.GsmId;
          this.selectedMinomat.Polling = this.Polling.HasValue ? this.Polling.Value : 0;
          this.selectedMinomat.HostAndPort = this.HostAndPort;
          this.selectedMinomat.Url = this.Url;
          this.selectedMinomat.IsInMasterPool = this.IsInMasterPool;
          this.selectedMinomat.SimPin = this.SimPin;
          this.selectedMinomat.AccessPoint = this.AccessPoint;
          this.selectedMinomat.UserId = this.UserId;
          this.selectedMinomat.UserPassword = this.UserPassword;
          this.selectedMinomat.SessionKey = this.SessionKey;
          this.selectedMinomat.IsMaster = this.IsMaster;
          this.selectedMinomat.Country = this._repositoryFactory.GetRepository<Country>().FirstOrDefault((Expression<Func<Country, bool>>) (x => x.Id == this.SelectedCountry.Id));
          this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().Update(this.selectedMinomat);
          this.selectedMinomat.Scenario = this.SelectedScenario != null ? this._repositoryFactory.GetRepository<Scenario>().GetById((object) this.SelectedScenario.Id) : (Scenario) null;
          if (!string.IsNullOrEmpty(this.Name))
            this._structureNode.Name = this.Name;
          if (!string.IsNullOrEmpty(this.Description))
            this._structureNode.Description = this.Description;
          EventPublisher.Publish<ActionStructureAndEntitiesUpdate>(new ActionStructureAndEntitiesUpdate()
          {
            Minomat = this.selectedMinomat,
            Node = this._structureNode,
            Guid = this._structureNode.RootNode != this._structureNode ? this._structureNode.RootNode.Id : this._structureNode.Id,
            Message = new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Success,
              MessageText = MessageCodes.Success_Save.GetStringValue()
            },
            Name = this._structureNode.Name,
            Description = this._structureNode.Description
          }, (IViewModel) this);
          this.OnRequestClose(true);
        });
      }
    }

    public ICommand DeleteMinomatCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.selectedMinomat = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().FirstOrDefault((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => x.Id == this.selectedMinomat.Id));
          this.selectedMinomat.IsDeactivated = true;
          this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().Update(this.selectedMinomat);
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

    public string Name
    {
      get => this._name;
      set
      {
        this._name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    public string Description
    {
      get => this._description;
      set
      {
        this._description = value;
        this.OnPropertyChanged(nameof (Description));
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
    public string RadioId
    {
      get => this._radioId;
      set
      {
        this._radioId = value;
        this.OnPropertyChanged(nameof (RadioId));
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

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.RadioId));
      if (!(propertyName == propertyName1))
        return new List<string>();
      ICollection<string> validationErrors;
      this.ValidateMasterRadioId(this.RadioId, out validationErrors);
      if (validationErrors.Count > 0)
        this.IsValid = false;
      return validationErrors.ToList<string>();
    }

    public bool ValidateMasterRadioId(
      string masterRadioId,
      out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      IList<MSS.Core.Model.DataCollectors.Minomat> source = this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().SearchFor((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (x => x.RadioId == masterRadioId));
      if (this.selectedMinomat == null && source.Any<MSS.Core.Model.DataCollectors.Minomat>())
        validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
      else if (source.Any<MSS.Core.Model.DataCollectors.Minomat>() && source.FirstOrDefault<MSS.Core.Model.DataCollectors.Minomat>((Func<MSS.Core.Model.DataCollectors.Minomat, bool>) (x => x.RadioId != null && x.RadioId != this.selectedMinomat.RadioId)) != null)
        validationErrors.Add(Resources.MSS_MINOMAT_RADIO_ID_NOT_UNIQUE);
      return validationErrors.Count == 0;
    }
  }
}
