// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Settings.SettingsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using FluentNHibernate.Conventions;
using Microsoft.Win32;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Languages;
using MSS.Business.Modules.ClientsManagement;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.DataCollectorsManagement;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Modules.WCFRelated;
using MSS.Business.Utils;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.MDM;
using MSS.Core.Model.Meters;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.Clients;
using MSS.DTO.MDM;
using MSS.DTO.MessageHandler;
using MSS.DTO.Minomat;
using MSS.DTO.Providers;
using MSS.DTO.Structures;
using MSS.DTO.Users;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Settings.Selector;
using MSS_Client.ViewModel.Structures;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Settings
{
  public class SettingsViewModel : ValidationViewModelBase
  {
    private readonly IRepository<ApplicationParameter> _appParamRepository;
    private readonly IRepository<MeasureUnit> _measureUniteRepository;
    private readonly IRepository<UserDeviceTypeSettings> _userDeviceTypeSettingsRepository;
    private static bool _userDeviceTypesSettingsExisting = true;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly string _equipmentName;
    private Language _currentLanguage;
    private EquipmentSelector _equipmentSelector;
    private bool _isExpertConfigurationMode;
    private bool _isGeneralSettingsTabSelected;
    private bool _isServerSettingsTabSelected;
    private bool _isMinomatSettingsTabSelected;
    private bool _isEquipmentSettingsTabSelected;
    private ApplicationTabsEnum _selectedTab;
    private string _pageSize = string.Empty;
    private string _batchSize = string.Empty;
    private int _pageSizeNumber;
    private int _batchSizeNumber;
    private bool _refresh;
    private string _serverIp = string.Empty;
    private string _status = string.Empty;
    private bool _minomatsTabVisibility;
    private bool _minomatImportButtonVisibility;
    private ViewModelBase _messageUserControlGeneralSettings;
    private ViewModelBase _messageUserControlServerSettings;
    private ViewModelBase _messageUserControlMinomatSettings;
    private ViewModelBase _messageUserControlEquipmentSettings;
    private int _selectedIndex;
    private Language _selectedLanguage;
    private bool _isServerIpEnabled;
    private string _hostAndPort = string.Empty;
    private bool _useMasterpool;
    private bool _serverControlsVisibility;
    private string _url = string.Empty;
    private string _polling = string.Empty;
    private bool _changeServerVisibility;

    [Inject]
    public SettingsViewModel(IRepositoryFactory repositoryFactory, IWindowFactory windowFactory)
    {
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this._appParamRepository = repositoryFactory.GetRepository<ApplicationParameter>();
      this.ServerIp = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (ServerIp))).Value;
      this.BatchSize = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (BatchSize))).Value;
      this.PageSize = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (PageSize))).Value;
      this.HostAndPort = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (HostAndPort))).Value;
      this.Url = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (Url))).Value;
      this.Polling = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == nameof (Polling))).Value;
      this.UseMasterpool = bool.Parse(this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == "MinomatUseMasterpool")).Value);
      this.Refresh = !string.IsNullOrEmpty(this.ServerIp);
      this.ClientsList = this.MyClients();
      UsersManager usersManager = new UsersManager(this._repositoryFactory);
      this.MinomatTabVisibility = usersManager.HasRight(OperationEnum.DataCollectorView.ToString());
      this.MinomatImportButtonVisibility = usersManager.HasRight(OperationEnum.DataCollectorImport.ToString());
      this.ServerControlsVisibility = !LicenseHelper.IsStandaloneClientLicense(LicenseHelper.GetValidHardwareKey());
      this.ChangeServerVisibility = usersManager.HasRight(OperationEnum.ApplicationSettingsChangeServer.ToString());
      this._equipmentName = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == "DefaultEquipment")).Value;
      this._equipmentSelector = new EquipmentSelector(this._equipmentName, this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == "DefaultEquipmentParams")).Value);
      ApplicationParameter applicationParameter = this._appParamRepository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (p => p.Parameter == "ExpertConfigurationMode"));
      this.IsExpertConfigurationMode = applicationParameter != null && bool.TryParse(applicationParameter.Value, out this._isExpertConfigurationMode) && bool.Parse(applicationParameter.Value);
      Mapper.CreateMap<Provider, ProviderDTO>();
      Mapper.CreateMap<MSS.Core.Model.UsersManagement.Country, CountryDTO>();
      Mapper.CreateMap<MDMConfigs, MDMConfigsDTO>().ForMember((Expression<Func<MDMConfigsDTO, object>>) (x => (object) x.Country), (Action<IMemberConfigurationExpression<MDMConfigs>>) (x => x.MapFrom<Guid>((Expression<Func<MDMConfigs, Guid>>) (y => y.Country.Id))));
      repositoryFactory.GetRepository<MSS.Core.Model.MSSClient.MSSClient>();
      this._measureUniteRepository = repositoryFactory.GetRepository<MeasureUnit>();
      this._userDeviceTypeSettingsRepository = repositoryFactory.GetRepository<UserDeviceTypeSettings>();
      this.SelectedIndex = 0;
      this.CountryList = this.GetCountries();
      if (this.LanguageList == null)
      {
        ObservableCollection<Language> observableCollection = new ObservableCollection<Language>();
        observableCollection.Add(new Language(LangEnum.English, "pack://application:,,,/Styles;component/Images/Universal/english.png"));
        observableCollection.Add(new Language(LangEnum.German, "pack://application:,,,/Styles;component/Images/Universal/german.png"));
        this.LanguageList = observableCollection;
      }
      LangEnum lang = (LangEnum) System.Enum.Parse(typeof (LangEnum), this._repositoryFactory.GetRepository<User>().FirstOrDefault((Expression<Func<User, bool>>) (u => u.Id == MSS.Business.Utils.AppContext.Current.LoggedUser.Id)).Language);
      this.SelectedLanguage = this.LanguageList.FirstOrDefault<Language>((Func<Language, bool>) (l => l.Name == lang));
      this._currentLanguage = this.SelectedLanguage;
      this.RefreshStatus();
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      EventPublisher.Register<SelectedTabValue>(new Action<SelectedTabValue>(this.SetTab));
      EventPublisher.Register<SelectedTabChanged>((Action<SelectedTabChanged>) (changed => this.SelectedTab = changed.SelectedTab));
    }

    public EquipmentSelector EquipmentSelectorProperty
    {
      get => this._equipmentSelector;
      set
      {
        this._equipmentSelector = value;
        this.OnPropertyChanged(nameof (EquipmentSelectorProperty));
      }
    }

    public bool IsExpertConfigurationMode
    {
      get => this._isExpertConfigurationMode;
      set
      {
        this._isExpertConfigurationMode = value;
        this.OnPropertyChanged(nameof (IsExpertConfigurationMode));
      }
    }

    public bool IsGeneralSettingsTabSelected
    {
      get => this._isGeneralSettingsTabSelected;
      set
      {
        this._isGeneralSettingsTabSelected = value;
        if (!this._isGeneralSettingsTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.GeneralSettings
        }, (IViewModel) this);
      }
    }

    public bool IsServerSettingsTabSelected
    {
      get => this._isServerSettingsTabSelected;
      set
      {
        this._isServerSettingsTabSelected = value;
        if (!this._isServerSettingsTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.ServerSettings
        }, (IViewModel) this);
      }
    }

    public bool IsMinomatSettingsTabSelected
    {
      get => this._isMinomatSettingsTabSelected;
      set
      {
        this._isMinomatSettingsTabSelected = value;
        if (!this._isMinomatSettingsTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.MinomatSettings
        }, (IViewModel) this);
      }
    }

    public bool IsEquipmentSettingsTabSelected
    {
      get => this._isEquipmentSettingsTabSelected;
      set
      {
        this._isEquipmentSettingsTabSelected = value;
        if (!this._isEquipmentSettingsTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.EquipmentSettings
        }, (IViewModel) this);
      }
    }

    private void SetSelectedTab()
    {
      switch (this.SelectedTab)
      {
        case ApplicationTabsEnum.GeneralSettings:
          this._isGeneralSettingsTabSelected = true;
          break;
        case ApplicationTabsEnum.ServerSettings:
          this._isServerSettingsTabSelected = true;
          break;
        case ApplicationTabsEnum.MinomatSettings:
          this._isMinomatSettingsTabSelected = true;
          break;
        case ApplicationTabsEnum.EquipmentSettings:
          this._isMinomatSettingsTabSelected = true;
          break;
      }
    }

    public ApplicationTabsEnum SelectedTab
    {
      get => this._selectedTab;
      set
      {
        this._selectedTab = value;
        this.SetSelectedTab();
      }
    }

    private void SetTab(SelectedTabValue selectedTabValue)
    {
      switch (selectedTabValue.Tab)
      {
        case ApplicationTabsEnum.GeneralSettings:
          this.SelectedIndex = 0;
          break;
        case ApplicationTabsEnum.ServerSettings:
          this.SelectedIndex = 1;
          break;
        case ApplicationTabsEnum.MinomatSettings:
          this.SelectedIndex = 2;
          break;
        case ApplicationTabsEnum.EquipmentSettings:
          this.SelectedIndex = 3;
          break;
      }
    }

    public IEnumerable<UserDeviceTypeSettingsDTO> GetUserDeviceTypeSettings
    {
      get
      {
        IEnumerable<UserDeviceTypeSettings> source = this._userDeviceTypeSettingsRepository.GetAll().Where<UserDeviceTypeSettings>((Func<UserDeviceTypeSettings, bool>) (x => x.User != null && x.User.Id == MSS.Business.Utils.AppContext.Current.LoggedUser.Id));
        List<UserDeviceTypeSettingsDTO> deviceTypeSettings = new List<UserDeviceTypeSettingsDTO>();
        if (EnumerableExtensionsForConventions.IsEmpty<UserDeviceTypeSettings>(source))
        {
          SettingsViewModel._userDeviceTypesSettingsExisting = false;
          deviceTypeSettings = ((IEnumerable<DeviceTypeEnum>) System.Enum.GetValues(typeof (DeviceTypeEnum))).Select<DeviceTypeEnum, UserDeviceTypeSettingsDTO>((Func<DeviceTypeEnum, UserDeviceTypeSettingsDTO>) (deviceType => new UserDeviceTypeSettingsDTO()
          {
            DeviceType = deviceType,
            User = MSS.Business.Utils.AppContext.Current.LoggedUser
          })).ToList<UserDeviceTypeSettingsDTO>();
        }
        else
          deviceTypeSettings.AddRange(source.Select<UserDeviceTypeSettings, UserDeviceTypeSettingsDTO>((Func<UserDeviceTypeSettings, UserDeviceTypeSettingsDTO>) (setting => new UserDeviceTypeSettingsDTO()
          {
            Id = setting.Id,
            User = setting.User,
            DecimalPlaces = setting.DecimalPlaces,
            DeviceType = setting.DeviceType,
            DisplayUnitId = setting.DisplayUnit == null ? Guid.Empty : setting.DisplayUnit.Id
          })));
        return (IEnumerable<UserDeviceTypeSettingsDTO>) deviceTypeSettings;
      }
    }

    private void CreateMessage(ActionSyncFinished messageFinished)
    {
      ViewModelBase viewModelBase = (ViewModelBase) null;
      switch (messageFinished.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          viewModelBase = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          viewModelBase = MessageHandlingManager.ShowWarningMessage(messageFinished.Message.MessageText);
          break;
      }
      if (this.IsGeneralSettingsTabSelected)
        this.MessageUserControlGeneralSettings = viewModelBase;
      if (this.IsServerSettingsTabSelected)
        this.MessageUserControlServerSettings = viewModelBase;
      if (this.IsMinomatSettingsTabSelected)
        this.MessageUserControlMinomatSettings = viewModelBase;
      if (!this.IsEquipmentSettingsTabSelected)
        return;
      this.MessageUserControlEquipmentSettings = viewModelBase;
    }

    public async void RefreshStatus()
    {
      SettingsConnectionManager ch = new SettingsConnectionManager();
      if (string.IsNullOrEmpty(this.ServerIp))
        return;
      if (this.IsValidIp(this.ServerIp))
      {
        bool testConnectionToSrv = ch.TestConnectiontoServer(this.ServerIp);
        if (!testConnectionToSrv)
        {
          this.StatusRefresh = CultureResources.GetValue("MSS_Error_Message_Server_Not_Found");
        }
        else
        {
          StatusEnum status;
          try
          {
            string statusRef = await SettingsConnectionManager.ServerMethodCall(this.ServerIp);
            if (string.IsNullOrEmpty(statusRef))
              return;
            status = (StatusEnum) System.Enum.Parse(typeof (StatusEnum), statusRef);
            statusRef = (string) null;
          }
          catch (Exception ex)
          {
            this.StatusRefresh = CultureResources.GetValue("MSS_Error_Message_Server_Not_Found");
            return;
          }
          switch (status)
          {
            case StatusEnum.accepted:
              this.StatusRefresh = CultureResources.GetValue("MSS_StatusEnum_Accepted");
              break;
            case StatusEnum.disctonnected:
              this.StatusRefresh = CultureResources.GetValue("MSS_StatusEnum_Disconnected");
              break;
            case StatusEnum.pending:
              this.StatusRefresh = CultureResources.GetValue("MSS_StatusEnum_Pending");
              break;
          }
        }
      }
      else
      {
        if (this.IsGeneralSettingsTabSelected)
          this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.Invalid_IP.GetStringValue());
        if (this.IsServerSettingsTabSelected)
          this.MessageUserControlServerSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.Invalid_IP.GetStringValue());
        if (this.IsMinomatSettingsTabSelected)
          this.MessageUserControlMinomatSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.Invalid_IP.GetStringValue());
        if (this.IsEquipmentSettingsTabSelected)
          this.MessageUserControlEquipmentSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.Invalid_IP.GetStringValue());
      }
    }

    public ViewModelBase MessageUserControlGeneralSettings
    {
      get => this._messageUserControlGeneralSettings;
      set
      {
        this._messageUserControlGeneralSettings = value;
        this.OnPropertyChanged(nameof (MessageUserControlGeneralSettings));
      }
    }

    public ViewModelBase MessageUserControlServerSettings
    {
      get => this._messageUserControlServerSettings;
      set
      {
        this._messageUserControlServerSettings = value;
        this.OnPropertyChanged(nameof (MessageUserControlServerSettings));
      }
    }

    public ViewModelBase MessageUserControlMinomatSettings
    {
      get => this._messageUserControlMinomatSettings;
      set
      {
        this._messageUserControlMinomatSettings = value;
        this.OnPropertyChanged(nameof (MessageUserControlMinomatSettings));
      }
    }

    public ViewModelBase MessageUserControlEquipmentSettings
    {
      get => this._messageUserControlEquipmentSettings;
      set
      {
        this._messageUserControlEquipmentSettings = value;
        this.OnPropertyChanged(nameof (MessageUserControlEquipmentSettings));
      }
    }

    public int SelectedIndex
    {
      get => this._selectedIndex;
      set
      {
        this._selectedIndex = value;
        this.OnPropertyChanged(nameof (SelectedIndex));
      }
    }

    public string HandHeldDb { get; set; }

    public string Office { get; set; }

    public string Country { get; set; }

    public Language SelectedLanguage
    {
      get => this._selectedLanguage;
      set
      {
        this._selectedLanguage = value;
        this.OnPropertyChanged(nameof (SelectedLanguage));
      }
    }

    public string ServerIp
    {
      get => this._serverIp;
      set
      {
        this._serverIp = value;
        this.OnPropertyChanged(nameof (ServerIp));
      }
    }

    public bool Refresh
    {
      get => this._refresh;
      set
      {
        this._refresh = value;
        this.IsServerIpEnabled = !this.Refresh;
        this.OnPropertyChanged(nameof (Refresh));
      }
    }

    public bool IsServerIpEnabled
    {
      get => this._isServerIpEnabled;
      set
      {
        this._isServerIpEnabled = value;
        this.OnPropertyChanged(nameof (IsServerIpEnabled));
      }
    }

    public string StatusRefresh
    {
      get => this._status;
      set
      {
        this._status = value;
        this.OnPropertyChanged(nameof (StatusRefresh));
      }
    }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          this._pageSize = value;
        }
        else
        {
          bool flag = this.IsValidPageSize(value, out this._pageSizeNumber);
          this._pageSize = value;
          if (flag)
          {
            this._pageSize = this._pageSizeNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            this.OnPropertyChanged(nameof (PageSize));
          }
          else
            this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.PageSize_Not_A_Number.GetStringValue());
        }
      }
    }

    public string BatchSize
    {
      get => this._batchSize;
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          this._batchSize = value;
        }
        else
        {
          bool flag = this.IsValidBatchSize(value, out this._batchSizeNumber);
          this._batchSize = value;
          if (flag)
          {
            this._batchSize = this._batchSizeNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            this.OnPropertyChanged(nameof (BatchSize));
          }
          else
            this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.BatchSize_Not_A_Number.GetStringValue());
        }
      }
    }

    public string HostAndPort
    {
      get => this._hostAndPort;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this._hostAndPort = value;
        this.OnPropertyChanged(nameof (HostAndPort));
      }
    }

    public bool UseMasterpool
    {
      get => this._useMasterpool;
      set
      {
        this._useMasterpool = value;
        this.OnPropertyChanged(nameof (UseMasterpool));
      }
    }

    public bool MinomatTabVisibility
    {
      get => this._minomatsTabVisibility;
      set
      {
        this._minomatsTabVisibility = value;
        this.OnPropertyChanged(nameof (MinomatTabVisibility));
      }
    }

    public bool MinomatImportButtonVisibility
    {
      get => this._minomatImportButtonVisibility;
      set
      {
        this._minomatImportButtonVisibility = value;
        this.OnPropertyChanged(nameof (MinomatImportButtonVisibility));
      }
    }

    public bool ServerControlsVisibility
    {
      get => this._serverControlsVisibility;
      set
      {
        this._serverControlsVisibility = value;
        this.OnPropertyChanged(nameof (ServerControlsVisibility));
      }
    }

    public string Url
    {
      get => this._url;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this._url = value;
        this.OnPropertyChanged(nameof (Url));
      }
    }

    public string Polling
    {
      get => this._polling;
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this._polling = value;
        this.OnPropertyChanged(nameof (Polling));
      }
    }

    public bool ChangeServerVisibility
    {
      get => this._changeServerVisibility;
      set
      {
        this._changeServerVisibility = value;
        this.OnPropertyChanged(nameof (ChangeServerVisibility));
      }
    }

    public ObservableCollection<Language> LanguageList { get; set; }

    public ObservableCollection<MeasureUnit> MeasureUnits
    {
      get
      {
        IList<MeasureUnit> all = this._measureUniteRepository.GetAll();
        ObservableCollection<MeasureUnit> measureUnits = new ObservableCollection<MeasureUnit>();
        foreach (MeasureUnit measureUnit in (IEnumerable<MeasureUnit>) all)
          measureUnits.Add(new MeasureUnit()
          {
            Id = measureUnit.Id,
            Code = measureUnit.Code
          });
        return measureUnits;
      }
    }

    public ObservableCollection<MeasureUnit> DisplayMeasureUnits
    {
      get
      {
        IList<MeasureUnit> all = this._measureUniteRepository.GetAll();
        ObservableCollection<MeasureUnit> displayMeasureUnits = new ObservableCollection<MeasureUnit>();
        foreach (MeasureUnit measureUnit in (IEnumerable<MeasureUnit>) all)
          displayMeasureUnits.Add(new MeasureUnit()
          {
            Id = measureUnit.Id,
            Code = measureUnit.Code
          });
        return displayMeasureUnits;
      }
    }

    public List<MyClient> ClientsList { get; set; }

    private List<MyClient> MyClients()
    {
      ObservableCollection<MyClient> source = new ObservableCollection<MyClient>();
      IEnumerable<MSS.Core.Model.MSSClient.MSSClient> clients = new ClientsManager(this._repositoryFactory).GetClients();
      List<MyClient> list = new List<MyClient>();
      if (!(clients is MSS.Core.Model.MSSClient.MSSClient[] mssClientArray1))
        mssClientArray1 = clients.ToArray<MSS.Core.Model.MSSClient.MSSClient>();
      MSS.Core.Model.MSSClient.MSSClient[] mssClientArray2 = mssClientArray1;
      foreach (MSS.Core.Model.MSSClient.MSSClient mssClient in mssClientArray2)
      {
        MyClient myClient = new MyClient()
        {
          Id = mssClient.Id,
          ApprovedOn = mssClient.ApprovedOn,
          UniqueClientRequest = mssClient.UniqueClientRequest,
          UserId = mssClient.UserId,
          UserName = mssClient.UserName
        };
        StatusEnum? status = mssClient.Status;
        if (status.HasValue)
        {
          switch (status.GetValueOrDefault())
          {
            case StatusEnum.accepted:
              myClient.IconUrl = "pack://application:,,,/Styles;component/Images/Settings/light-green.png";
              myClient.idEnumStatus = 0;
              break;
            case StatusEnum.disctonnected:
              myClient.IconUrl = "pack://application:,,,/Styles;component/Images/Settings/light-red.png";
              myClient.idEnumStatus = 1;
              break;
            case StatusEnum.pending:
              myClient.IconUrl = "pack://application:,,,/Styles;component/Images/Settings/light-yellow.png";
              myClient.idEnumStatus = 2;
              break;
          }
        }
        list.Add(myClient);
      }
      if (mssClientArray2.Length != 0)
        source = new ObservableCollection<MyClient>(list);
      return source.ToList<MyClient>();
    }

    public RadObservableCollection<ProviderDTO> GetProviders
    {
      get
      {
        RadObservableCollection<ProviderDTO> observableCollection = new RadObservableCollection<ProviderDTO>();
        List<Provider> list = this._repositoryFactory.GetRepository<Provider>().GetAll().ToList<Provider>();
        return list.Count == 0 ? observableCollection : new RadObservableCollection<ProviderDTO>((IEnumerable<ProviderDTO>) Mapper.Map<List<Provider>, List<ProviderDTO>>(list));
      }
    }

    public RadObservableCollection<MDMConfigsDTO> GetMdmConfigs
    {
      get
      {
        RadObservableCollection<MDMConfigsDTO> observableCollection = new RadObservableCollection<MDMConfigsDTO>();
        List<MDMConfigs> list = this._repositoryFactory.GetRepository<MDMConfigs>().GetAll().ToList<MDMConfigs>();
        return list.Count == 0 ? observableCollection : new RadObservableCollection<MDMConfigsDTO>((IEnumerable<MDMConfigsDTO>) Mapper.Map<List<MDMConfigs>, List<MDMConfigsDTO>>(list));
      }
    }

    public RadObservableCollection<CountryDTO> CountryList { get; set; }

    public RadObservableCollection<CountryDTO> GetCountries()
    {
      RadObservableCollection<CountryDTO> observableCollection = new RadObservableCollection<CountryDTO>();
      RadObservableCollection<MSS.Core.Model.UsersManagement.Country> countries = new UsersManager(this._repositoryFactory).GetCountries();
      return countries.Count == 0 ? observableCollection : Mapper.Map<RadObservableCollection<MSS.Core.Model.UsersManagement.Country>, RadObservableCollection<CountryDTO>>(countries);
    }

    public ObservableCollection<EnumObj> GetStatus
    {
      get
      {
        ObservableCollection<EnumObj> getStatus = new ObservableCollection<EnumObj>();
        IEnumerable<StatusEnum> source1 = System.Enum.GetValues(typeof (StatusEnum)).Cast<StatusEnum>();
        if (!(source1 is StatusEnum[] statusEnumArray))
          statusEnumArray = source1.ToArray<StatusEnum>();
        StatusEnum[] source2 = statusEnumArray;
        if (((IEnumerable<StatusEnum>) source2).Count<StatusEnum>() != 0)
        {
          for (int index = 0; index < ((IEnumerable<StatusEnum>) source2).Count<StatusEnum>(); ++index)
          {
            switch (source2[index])
            {
              case StatusEnum.accepted:
                EnumObj enumObj1 = new EnumObj()
                {
                  IdEnum = index,
                  StatusFromObj = CultureResources.GetValue("MSS_StatusEnum_Accepted")
                };
                getStatus.Add(enumObj1);
                break;
              case StatusEnum.disctonnected:
                EnumObj enumObj2 = new EnumObj()
                {
                  IdEnum = index,
                  StatusFromObj = CultureResources.GetValue("MSS_StatusEnum_Disconnected")
                };
                getStatus.Add(enumObj2);
                break;
              case StatusEnum.pending:
                EnumObj enumObj3 = new EnumObj()
                {
                  IdEnum = index,
                  StatusFromObj = CultureResources.GetValue("MSS_StatusEnum_Pending")
                };
                getStatus.Add(enumObj3);
                break;
            }
          }
        }
        return getStatus;
      }
    }

    public ObservableCollection<MSS.Core.Model.MSSClient.MSSClient> GetClientsFromDb
    {
      get
      {
        ObservableCollection<MSS.Core.Model.MSSClient.MSSClient> getClientsFromDb = new ObservableCollection<MSS.Core.Model.MSSClient.MSSClient>();
        IEnumerable<MSS.Core.Model.MSSClient.MSSClient> clients = new ClientsManager(this._repositoryFactory).GetClients();
        if (clients.Count<MSS.Core.Model.MSSClient.MSSClient>() != 0)
          getClientsFromDb = new ObservableCollection<MSS.Core.Model.MSSClient.MSSClient>(clients);
        return getClientsFromDb;
      }
    }

    public ObservableCollection<ScenarioDTO> GetScenarios
    {
      get
      {
        ObservableCollection<ScenarioDTO> getScenarios = new ObservableCollection<ScenarioDTO>();
        IEnumerable<ScenarioDTO> scenarioDtOs = new JobsManager(this._repositoryFactory).GetScenarioDTOs();
        if (scenarioDtOs.Count<ScenarioDTO>() != 0)
          getScenarios = new ObservableCollection<ScenarioDTO>(scenarioDtOs);
        return getScenarios;
      }
    }

    private IEnumerable<MSS.Core.Model.MSSClient.MSSClient> UpdateClientsList(
      ObservableCollection<MSS.Core.Model.MSSClient.MSSClient> getClientsFromDb,
      List<MyClient> clientsFromGrid)
    {
      int count = clientsFromGrid.Count;
      for (int index = 0; index < count; ++index)
      {
        StatusEnum? status = getClientsFromDb[index].Status;
        StatusEnum statusEnum = StatusEnum.accepted;
        if ((status.GetValueOrDefault() == statusEnum ? (!status.HasValue ? 1 : 0) : 1) != 0 && clientsFromGrid[index].idEnumStatus == 0)
        {
          getClientsFromDb[index].ApprovedOn = DateTime.Now;
          clientsFromGrid[index].ApprovedOn = getClientsFromDb[index].ApprovedOn;
        }
        getClientsFromDb[index].Status = new StatusEnum?((StatusEnum) clientsFromGrid[index].idEnumStatus);
        clientsFromGrid[index].idEnumStatus = clientsFromGrid[index].idEnumStatus;
        this.MessageUserControlServerSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue());
      }
      return (IEnumerable<MSS.Core.Model.MSSClient.MSSClient>) getClientsFromDb;
    }

    public ICommand buttonUpdateEquipmentSettings
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
          if (this.EquipmentSelectorProperty.SelectedEquipmentModel != null)
          {
            this.MessageUserControlEquipmentSettings = (ViewModelBase) null;
            Task<List<Config>> equipmentConfigsList = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.CreateEquipmentConfigsList(this.EquipmentSelectorProperty.SelectedEquipmentModel);
            parametersManagement.UpdateDefaultEquipment(this.EquipmentSelectorProperty.SelectedEquipmentModel, equipmentConfigsList.Result);
            parametersManagement.ResetSystemAndScanMode();
            this.MessageUserControlEquipmentSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue());
          }
          if (!parametersManagement.UpdateExpertConfigurationMode(this.IsExpertConfigurationMode))
            return;
          this.MessageUserControlEquipmentSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue());
        });
      }
    }

    public ICommand ImportMinomatsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = "Excel Document|*.xlsx",
            Title = "Import minomats from file",
            RestoreDirectory = true
          };
          openFileDialog.ShowDialog();
          if (openFileDialog.FileName == string.Empty)
            return;
          List<MinomatImportDTO> minomatImportDtoList;
          try
          {
            minomatImportDtoList = new XCellManager().ReadMinomatsFromFile(openFileDialog.FileName);
          }
          catch (Exception ex)
          {
            MSS.Business.Errors.MessageHandler.LogException(ex);
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_MinomatsImport_OpenFileError, false);
            return;
          }
          DataCollectorsManager collectorsManager = new DataCollectorsManager(this._repositoryFactory);
          List<string> minomatsByRadioId = collectorsManager.GetExistingMinomatsByRadioId(minomatImportDtoList.Select<MinomatImportDTO, string>((Func<MinomatImportDTO, string>) (m => m.MINOLID)).ToList<string>());
          List<string> ignoredRadioIds = new List<string>();
          if (minomatsByRadioId.Any<string>())
          {
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<WarningWithListBoxViewModel>((IParameter) new ConstructorArgument("existingItems", (object) minomatsByRadioId), (IParameter) new ConstructorArgument("warningMessage", (object) Resources.MSS_ImportMinomats_ExistingRadioIds)));
            if (!newModalDialog.HasValue || !newModalDialog.Value)
              ignoredRadioIds = minomatsByRadioId;
          }
          try
          {
            collectorsManager.ImportMinomats(minomatImportDtoList, DIConfigurator.GetConfigurator().Get<IRepositoryFactoryCreator>(), ignoredRadioIds);
          }
          catch (BaseApplicationException ex)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), ex.Message, false);
          }
          this.MessageUserControlMinomatSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
        }));
      }
    }

    public ICommand buttonChangeServer
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditServerPathViewModel>())));
      }
    }

    public ICommand buttonRefreshStatus
    {
      get
      {
        MSS.Business.Modules.AppParametersManagement.AppParametersManagement appParamsManager = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
        ApplicationParameter serverUrlDatabase = appParamsManager.GetAppParam("ServerIp");
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.MessageUserControlGeneralSettings = (ViewModelBase) null;
          SettingsConnectionManager connectionManager = new SettingsConnectionManager();
          if (this.IsValidIp(this.ServerIp))
          {
            if (serverUrlDatabase.Value.Equals(this.ServerIp) && this.StatusRefresh == MessageCodes.Server_Not_Found.GetStringValue())
              this.InsertClient(serverUrlDatabase, appParamsManager);
            if (connectionManager.TestConnectiontoServer(this.ServerIp))
            {
              this.RefreshStatus();
              this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Refresh_Successfully.GetStringValue());
            }
            else
            {
              this.StatusRefresh = MessageCodes.Server_Not_Found.GetStringValue();
              this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowWarningMessage(MessageCodes.Server_Not_Available.GetStringValue());
            }
          }
          else
            this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.Invalid_IP.GetStringValue());
        }));
      }
    }

    public ICommand buttonUpdateGeneralSettings
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool flag = this._currentLanguage.Name != this.SelectedLanguage.Name;
          IEnumerable<UserDeviceTypeSettingsDTO> source = parameter as IEnumerable<UserDeviceTypeSettingsDTO>;
          this.SetCulture();
          MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
          parametersManagement.GetAppParam("ServerIp");
          ApplicationParameter appParam1 = parametersManagement.GetAppParam("PageSize");
          ApplicationParameter appParam2 = parametersManagement.GetAppParam("BatchSize");
          if (!this.BatchSize.Equals(appParam2.Value) && !string.IsNullOrEmpty(this.BatchSize))
          {
            if (!this.IsValidBatchSize(this.BatchSize, out int _))
            {
              this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.BatchSize_Not_A_Number.GetStringValue());
              return;
            }
            appParam2.Value = this.BatchSize;
            parametersManagement.Update(appParam2);
            MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
          }
          if (!this.PageSize.Equals(appParam1.Value))
          {
            if (!string.IsNullOrEmpty(this.PageSize))
            {
              if (!this.IsValidPageSize(this.PageSize, out int _))
              {
                this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.PageSize_Not_A_Number.GetStringValue());
                return;
              }
              appParam1.Value = this.PageSize;
              parametersManagement.Update(appParam1);
              MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
            }
            MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(DIConfigurator.GetConfigurator().Get<IRepository<ApplicationParameter>>().GetAll());
          }
          this.MessageUserControlGeneralSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue());
          UserSettingsManager userSettingsManager = new UserSettingsManager(this._repositoryFactory);
          if (source == null)
            return;
          if (SettingsViewModel._userDeviceTypesSettingsExisting)
          {
            userSettingsManager.UpdateUserDeviceTypesSettings(source.ToList<UserDeviceTypeSettingsDTO>());
          }
          else
          {
            userSettingsManager.CreateUserDeviceTypesSettings(source.ToList<UserDeviceTypeSettingsDTO>());
            SettingsViewModel._userDeviceTypesSettingsExisting = true;
          }
          if (!flag)
            return;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_LanguageChange_RestartApplication), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) true)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            Process.Start(new ProcessStartInfo()
            {
              Arguments = "/C choice /C Y /N /D Y /T 1 & START \"\" \"" + Assembly.GetExecutingAssembly().Location + "\"",
              WindowStyle = ProcessWindowStyle.Hidden,
              CreateNoWindow = true,
              FileName = "cmd.exe"
            });
            Application.Current.Shutdown();
          }
        }));
      }
    }

    public void SetCulture()
    {
      UsersManager usersManager = new UsersManager(this._repositoryFactory);
      Guid id = MSS.Business.Utils.AppContext.Current.LoggedUser.Id;
      if (this.SelectedLanguage.Name == LangEnum.German)
      {
        usersManager.SetLanguage(id, LangEnum.German.ToString());
        MSS.Business.Utils.AppContext.Current.LoggedUser.Language = LangEnum.German.ToString();
      }
      else
      {
        usersManager.SetLanguage(id, LangEnum.English.ToString());
        MSS.Business.Utils.AppContext.Current.LoggedUser.Language = LangEnum.English.ToString();
      }
    }

    public ICommand buttonUpdateServerSettings
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          new ClientsManager(this._repositoryFactory).UpdateClientsDb(this.UpdateClientsList(this.GetClientsFromDb, this.ClientsList));
          this.MessageUserControlServerSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue());
        });
      }
    }

    public ICommand buttonUpdateMinomatSettings
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.MessageUserControlMinomatSettings = (ViewModelBase) null;
          MSS.Business.Modules.AppParametersManagement.AppParametersManagement parametersManagement = new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory);
          ApplicationParameter appParam1 = parametersManagement.GetAppParam("Polling");
          ApplicationParameter appParam2 = parametersManagement.GetAppParam("Url");
          ApplicationParameter appParam3 = parametersManagement.GetAppParam("HostAndPort");
          ApplicationParameter appParam4 = parametersManagement.GetAppParam("MinomatUseMasterpool");
          if (!this.UseMasterpool.ToString().Equals(appParam4.Value))
          {
            appParam4.Value = this.UseMasterpool.ToString();
            parametersManagement.Update(appParam4);
            MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
          }
          if (!this.Polling.Equals(appParam1.Value) && !string.IsNullOrEmpty(this.Polling))
          {
            appParam1.Value = this.Polling;
            parametersManagement.Update(appParam1);
            MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
          }
          if (!this.Url.Equals(appParam2.Value) && !string.IsNullOrEmpty(this.Url))
          {
            appParam2.Value = this.Url;
            parametersManagement.Update(appParam2);
            MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
          }
          if (!this.HostAndPort.Equals(appParam3.Value) && !string.IsNullOrEmpty(this.HostAndPort))
          {
            appParam3.Value = this.HostAndPort;
            parametersManagement.Update(appParam3);
            MSS.Business.Utils.AppContext.Current.LoadApplicationParameters(this._appParamRepository.GetAll());
          }
          new UsersManager(this._repositoryFactory).UpdateCountries(this.CountryList);
          this.MessageUserControlMinomatSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue());
        });
      }
    }

    public ICommand InsertOrEditCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is ProviderDTO providerDto2))
            return;
          IRepository<Provider> repository = this._repositoryFactory.GetRepository<Provider>();
          Provider byId = repository.GetById((object) providerDto2.Id);
          if (byId != null)
          {
            byId.ProviderName = providerDto2.ProviderName;
            byId.SimPin = providerDto2.SimPin;
            byId.AccessPoint = providerDto2.AccessPoint;
            byId.UserId = providerDto2.UserId;
            byId.UserPassword = providerDto2.UserPassword;
            repository.Update(byId);
          }
          else
            repository.Insert(new Provider()
            {
              ProviderName = providerDto2.ProviderName,
              SimPin = providerDto2.SimPin,
              AccessPoint = providerDto2.AccessPoint,
              UserId = providerDto2.UserId,
              UserPassword = providerDto2.UserPassword
            });
          this.OnPropertyChanged("GetProviders");
        }));
      }
    }

    public ICommand DeleteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ProviderDTO providerDto = parameter as ProviderDTO;
          IRepository<Provider> repository = this._repositoryFactory.GetRepository<Provider>();
          repository.Delete(repository.GetById((object) providerDto.Id));
        }));
      }
    }

    public ICommand InsertOrEditMdmConfigCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is MDMConfigsDTO mdmConfigsDto2))
            return;
          IRepository<MDMConfigs> repository = this._repositoryFactory.GetRepository<MDMConfigs>();
          MDMConfigs byId = repository.GetById((object) mdmConfigsDto2.Id);
          if (byId != null)
          {
            byId.Company = mdmConfigsDto2.Company;
            byId.CustomerNumber = mdmConfigsDto2.CustomerNumber;
            byId.MDMPassword = mdmConfigsDto2.MDMPassword;
            byId.MDMUrl = mdmConfigsDto2.MDMUrl;
            byId.MDMUser = mdmConfigsDto2.MDMUser;
            byId.Country = this._repositoryFactory.GetRepository<MSS.Core.Model.UsersManagement.Country>().GetById((object) mdmConfigsDto2.Country);
            repository.Update(byId);
          }
          else
            repository.Insert(new MDMConfigs()
            {
              Company = mdmConfigsDto2.Company,
              CustomerNumber = mdmConfigsDto2.CustomerNumber,
              MDMPassword = mdmConfigsDto2.MDMPassword,
              MDMUrl = mdmConfigsDto2.MDMUrl,
              MDMUser = mdmConfigsDto2.MDMUser,
              Country = this._repositoryFactory.GetRepository<MSS.Core.Model.UsersManagement.Country>().GetById((object) mdmConfigsDto2.Country)
            });
          this.OnPropertyChanged("GetMdmConfigs");
        }));
      }
    }

    public ICommand DeleteMdmConfigCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          MDMConfigsDTO mdmConfigsDto = parameter as MDMConfigsDTO;
          IRepository<MDMConfigs> repository = this._repositoryFactory.GetRepository<MDMConfigs>();
          repository.Delete(repository.GetById((object) mdmConfigsDto.Id));
        }));
      }
    }

    private bool InsertClient(
      ApplicationParameter serverUrlDatabase,
      MSS.Business.Modules.AppParametersManagement.AppParametersManagement appParamsManager)
    {
      if (!string.IsNullOrEmpty(this.ServerIp))
      {
        try
        {
          if (this.IsValidIp(this.ServerIp))
          {
            serverUrlDatabase.Value = this.ServerIp;
            new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory).Update(serverUrlDatabase);
            this.RefreshStatus();
            using (ServiceClient serviceClient = new ServiceClient(this.ServerIp))
              serviceClient.InsertNewClient(MSS.Business.Utils.AppContext.Current.MSSClientId, MSS.Business.Utils.AppContext.Current.LoggedUser.Username, MSS.Business.Utils.AppContext.Current.LoggedUser.Id);
            this.Refresh = true;
            this.MessageUserControlServerSettings = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Save.GetStringValue());
          }
          else
          {
            this.MessageUserControlServerSettings = MessageHandlingManager.ShowValidationMessage(MessageCodes.Invalid_IP.GetStringValue());
            this.Refresh = true;
            return true;
          }
        }
        catch (BaseApplicationException ex)
        {
          this.Refresh = true;
          this.MessageUserControlServerSettings = MessageHandlingManager.ShowWarningMessage(MessageCodes.Server_Not_Available.GetStringValue());
        }
      }
      return false;
    }

    public bool IsValidIp(string addr)
    {
      return !string.IsNullOrEmpty(addr) && IPAddress.TryParse(addr, out IPAddress _);
    }

    public bool IsValidPageSize(string pageSize, out int number)
    {
      int result;
      if (!int.TryParse(pageSize, out result))
      {
        number = 0;
        return false;
      }
      number = result;
      return result > 0;
    }

    public bool IsValidBatchSize(string batchSize, out int number)
    {
      int result;
      if (!int.TryParse(batchSize, out result))
      {
        number = 0;
        return false;
      }
      number = result;
      return result > 0;
    }
  }
}
