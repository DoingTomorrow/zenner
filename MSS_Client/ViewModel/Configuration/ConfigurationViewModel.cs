// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Configuration.ConfigurationViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using Microsoft.Win32;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.GMMWrapper;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Client.UI.Common.Utils;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Meters;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ZENNER;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Configuration
{
  public class ConfigurationViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly ConfiguratorManager configManager;
    private Dictionary<string, IFirmwareConfigurator> _firmwareCache;
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly IEnumerable<string> _licenseDeviceTypes;
    private DeviceReaderManager _deviceReaderManager;
    public IQuickMessageShower MessageShower = (IQuickMessageShower) new QuickMessageShower();
    private const int MAX_DEVICES = 6;
    private readonly bool isTabletMode;
    private ConfigurationParametersVmPart _configurationParameters;
    private string _busyContent;
    private bool _isUpgradeFirmwareButtonEnabled;
    private bool _deviceConfigureVisibility;
    private bool _deviceReadVisibility;
    private bool _readLoggerVisiblity;
    private DeviceModel _selectedDeviceModel;
    private List<ProfileType> _profileTypes;
    private ProfileType _selectedProfileType;
    private Visibility _btnChangeProfileTypeParametersVisibility = Visibility.Collapsed;
    private Visibility _isProfileTypeEnabled = Visibility.Collapsed;
    private ViewModelBase _messageUserControl;
    private string _selectedEquipmentName;
    private bool _isReadValuesButtonEnabled;
    private EquipmentGroup _selectedEquipmentGroup;
    private EquipmentModel _selectedEquipmentModel;
    private bool _isBusy;
    private bool _isTextBoxRowVisible;
    private bool _isDeviceModelEnabled;
    private ConnectionAdjuster _connectionAdjuster;
    private Visibility _expertConfigurationVisibility = Visibility.Collapsed;
    private bool _isExpertConfigurationEnabled;
    private bool _isEquipmentChangeableParametersGridVisible;
    private bool _areChangeableParametersGridsVisible;
    private bool _isDeviceGroupVisible;
    private DeviceGroup _selectedDeviceGroup;

    [Inject]
    public ConfigurationViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      IRepository<ApplicationParameter> repository = repositoryFactory.GetRepository<ApplicationParameter>();
      this.isTabletMode = CustomerConfiguration.GetPropertyValue<bool>("IsTabletMode");
      this.configManager = GmmInterface.ConfiguratorManager;
      this.configManager.BatterieLow += (System.EventHandler) ((o, message) => Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_MinoConnect_Battery_Low))));
      this._configurationParameters = new ConfigurationParametersVmPart((IDeviceConfigurationParameterCollector) new DeviceConfigurationParameterCollector((IConfiguratorManager) new ConfiguratorManagerWrapper(), (IConfigurationValuesToListConfigConverter) new ConfigurationValuesToListConfigConverter()), (IDeviceConfigurationParameterWriter) new DeviceConfigurationParameterWriter((IConfiguratorManager) new ConfiguratorManagerWrapper()));
      this._licenseDeviceTypes = LicenseHelper.GetDeviceTypes();
      EventPublisher.Register<UpdateDefaultEquipment>(new Action<UpdateDefaultEquipment>(this.UpdateDefaultEquipmentEvent));
      GmmInterface.DeviceManager.SelectedFilter = (string) null;
      string equipmentName = repository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == "DefaultEquipment")).Value;
      string dbParamsString = repository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (_ => _.Parameter == "DefaultEquipmentParams")).Value;
      ApplicationParameter applicationParameter = repository.FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (p => p.Parameter == "ExpertConfigurationMode"));
      bool result = false;
      this.ExpertConfigurationVisibility = applicationParameter == null || !bool.TryParse(applicationParameter.Value, out result) || !bool.Parse(applicationParameter.Value) ? Visibility.Collapsed : Visibility.Visible;
      if (equipmentName == null)
        return;
      this.SelectedEquipmentModel = GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (e => e.Name == equipmentName));
      if (this.SelectedEquipmentModel != null)
      {
        MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateEquipmentWithSavedParams(this.SelectedEquipmentModel, dbParamsString);
        this.SelectedEquipmentName = this.SelectedEquipmentModel.Name;
      }
      UsersManager usersManager1 = new UsersManager(this._repositoryFactory);
      UsersManager usersManager2 = usersManager1;
      OperationEnum operationEnum = OperationEnum.DeviceConfigure;
      string operation1 = operationEnum.ToString();
      this.DeviceConfigureVisibility = usersManager2.HasRight(operation1);
      UsersManager usersManager3 = usersManager1;
      operationEnum = OperationEnum.DeviceRead;
      string operation2 = operationEnum.ToString();
      this.DeviceReadVisibility = usersManager3.HasRight(operation2);
      UsersManager usersManager4 = usersManager1;
      operationEnum = OperationEnum.HistoryView;
      string operation3 = operationEnum.ToString();
      this.ReadLoggerVisibility = usersManager4.HasRight(operation3);
      this.EquipmentGroupCollection = new ObservableCollection<EquipmentGroup>((IEnumerable<EquipmentGroup>) GmmInterface.DeviceManager.GetEquipmentGroups().OrderBy<EquipmentGroup, string>((Func<EquipmentGroup, string>) (eg => eg.Name)));
      if (this.SelectedEquipmentModel?.Name == null)
        return;
      this.SelectedEquipmentGroup = this.SelectedEquipmentModel.EquipmentGroup;
      EquipmentModel equipmentModel = this.EquipmentCollection.FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (e => e.Name == this.SelectedEquipmentModel.Name));
      if (equipmentModel.ChangeableParameters != null)
        equipmentModel.ChangeableParameters.ForEach((Action<ChangeableParameter>) (p =>
        {
          ChangeableParameter changeableParameter = this.SelectedEquipmentModel.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == p.Key));
          if (changeableParameter == null)
            return;
          p.Value = changeableParameter.Value;
        }));
      this.SelectedEquipmentModel = equipmentModel;
    }

    private void UpdateDefaultEquipmentEvent(UpdateDefaultEquipment ev)
    {
      this.SelectedEquipmentModel = ev.SelectedEquipmentModel;
      this.SelectedEquipmentName = ev.SelectedEquipmentModel.Name;
      this.SelectedEquipmentModel.ChangeableParameters = GMMHelper.ReplaceValuesInChangeableParameters(ev.SelectedEquipmentModel.ChangeableParameters, ev.ChangeableParameters);
    }

    public ConfigurationParametersVmPart ConfigurationParameters
    {
      get => this._configurationParameters;
      set
      {
        this._configurationParameters = value;
        this.OnPropertyChanged(nameof (ConfigurationParameters));
      }
    }

    public string BusyContent
    {
      get => this._busyContent;
      set
      {
        if (!(this._busyContent != value))
          return;
        this._busyContent = value;
        this.OnPropertyChanged(nameof (BusyContent));
      }
    }

    public bool IsUpgradeFirmwareButtonEnabled
    {
      get => this._isUpgradeFirmwareButtonEnabled;
      set
      {
        this._isUpgradeFirmwareButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsUpgradeFirmwareButtonEnabled));
      }
    }

    public bool DeviceConfigureVisibility
    {
      get => this._deviceConfigureVisibility;
      set
      {
        this._deviceConfigureVisibility = value;
        this.OnPropertyChanged(nameof (DeviceConfigureVisibility));
      }
    }

    public bool DeviceReadVisibility
    {
      get => this._deviceReadVisibility;
      set
      {
        this._deviceReadVisibility = value;
        this.OnPropertyChanged(nameof (DeviceReadVisibility));
      }
    }

    public bool ReadLoggerVisibility
    {
      get => this._readLoggerVisiblity;
      set
      {
        this._readLoggerVisiblity = value;
        this.OnPropertyChanged(nameof (ReadLoggerVisibility));
      }
    }

    public IEnumerable<DeviceModel> DeviceModelCollection
    {
      get
      {
        this.SelectedDeviceModel = (DeviceModel) null;
        if (this.SelectedEquipmentModel == null)
          return (IEnumerable<DeviceModel>) null;
        List<DeviceModel> deviceModelList = new List<DeviceModel>();
        List<DeviceModel> deviceModels;
        if (this.IsDeviceGroupVisible)
        {
          if (this.SelectedDeviceGroup == null)
            return (IEnumerable<DeviceModel>) null;
          deviceModels = GmmInterface.DeviceManager.GetDeviceModels(this.SelectedDeviceGroup);
        }
        else
          deviceModels = GmmInterface.DeviceManager.GetDeviceModels(this.SelectedEquipmentModel);
        return (IEnumerable<DeviceModel>) deviceModels.Where<DeviceModel>((Func<DeviceModel, bool>) (deviceModel => this._licenseDeviceTypes.Contains<string>(deviceModel.Name))).OrderBy<DeviceModel, string>((Func<DeviceModel, string>) (d => d.Name)).ToList<DeviceModel>();
      }
    }

    public DeviceModel SelectedDeviceModel
    {
      get => this._selectedDeviceModel;
      set
      {
        this._selectedDeviceModel = value;
        this.IsProfileTypeEnabled = this.SelectedEquipmentModel == null || this.SelectedDeviceModel == null ? Visibility.Collapsed : Visibility.Visible;
        this.IsReadValuesButtonEnabled = value != null && this.SelectedEquipmentGroup.Name == "MinoConnect" && this._selectedDeviceModel.Parameters != null && this._selectedDeviceModel.Parameters.ContainsKey(ConnectionProfileParameter.LoggerReadingPossible);
        if (this._selectedDeviceModel?.Name == "EDC Radio" || this._selectedDeviceModel?.Name == "PDC Radio")
        {
          this.IsUpgradeFirmwareButtonEnabled = true;
          this._firmwareCache = new Dictionary<string, IFirmwareConfigurator>()
          {
            {
              "EDC Radio",
              (IFirmwareConfigurator) new EDCConfigurator(this.SelectedEquipmentModel)
            },
            {
              "PDC Radio",
              (IFirmwareConfigurator) new PDCConfigurator(this.SelectedEquipmentModel, (IHandlerManager) new HandlerManagerWrapper(), (IDeviceManager) new DeviceManagerWrapper())
            }
          };
        }
        else
          this.IsUpgradeFirmwareButtonEnabled = false;
        this.OnPropertyChanged(nameof (SelectedDeviceModel));
        if (value != null)
        {
          string currentlySelectedProfileType = this.SelectedProfileType != null ? this.SelectedProfileType.Name : string.Empty;
          this.ProfileTypes = GmmInterface.DeviceManager.GetProfileTypes(this.SelectedDeviceModel, this.SelectedEquipmentModel);
          this.SelectedProfileType = this.ProfileTypes.Any<ProfileType>((Func<ProfileType, bool>) (p => p.Name == currentlySelectedProfileType)) ? this.ProfileTypes.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (p => p.Name == currentlySelectedProfileType)) : this.ProfileTypes.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (item => item.Name.Contains("IR"))) ?? this.ProfileTypes.FirstOrDefault<ProfileType>();
        }
        else
        {
          this.ProfileTypes = (List<ProfileType>) null;
          this.SelectedProfileType = (ProfileType) null;
        }
        this.SetIsExpertConfigurationEnabled();
        this.SetAreChangeableParametersGridsVisible();
        this.ConfigurationParameters.ClearCurrentlyShownControls();
      }
    }

    public List<ProfileType> ProfileTypes
    {
      get => this._profileTypes;
      set
      {
        this._profileTypes = value;
        this.OnPropertyChanged(nameof (ProfileTypes));
      }
    }

    public ProfileType SelectedProfileType
    {
      get => this._selectedProfileType;
      set
      {
        this._selectedProfileType = value;
        this.OnPropertyChanged(nameof (SelectedProfileType));
        this.BtnChangeProfileTypeParametersVisibility = this.ExpertConfigurationVisibility == Visibility.Visible || this.SelectedProfileType == null || this.SelectedProfileType.ChangeableParameters == null || this.SelectedProfileType.ChangeableParameters.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        this.SetIsExpertConfigurationEnabled();
        this.SetAreChangeableParametersGridsVisible();
        this.RefreshChangeableParameters(true);
      }
    }

    public Visibility BtnChangeProfileTypeParametersVisibility
    {
      get => this._btnChangeProfileTypeParametersVisibility;
      set
      {
        this._btnChangeProfileTypeParametersVisibility = value;
        this.OnPropertyChanged(nameof (BtnChangeProfileTypeParametersVisibility));
      }
    }

    public Visibility IsProfileTypeEnabled
    {
      get => this._isProfileTypeEnabled;
      set
      {
        this._isProfileTypeEnabled = value;
        this.OnPropertyChanged(nameof (IsProfileTypeEnabled));
      }
    }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public string SelectedEquipmentName
    {
      get => this._selectedEquipmentName;
      set
      {
        this._selectedEquipmentName = value;
        this.OnPropertyChanged(nameof (SelectedEquipmentName));
      }
    }

    public bool IsReadValuesButtonEnabled
    {
      get => this._isReadValuesButtonEnabled;
      set
      {
        this._isReadValuesButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsReadValuesButtonEnabled));
      }
    }

    public string ChangeDefaultEquipmentTitle { get; set; }

    public List<MSS.Business.Modules.Configuration.Config> EquipmentConfigsList { get; set; }

    public ObservableCollection<EquipmentGroup> EquipmentGroupCollection { get; set; }

    public ObservableCollection<EquipmentModel> EquipmentCollection { get; set; }

    [Required(ErrorMessage = "EQUIPMENT_GROUP_SELECTION_REQUIRED")]
    public EquipmentGroup SelectedEquipmentGroup
    {
      get => this._selectedEquipmentGroup;
      set
      {
        this._selectedEquipmentGroup = value;
        this.EquipmentCollection = new ObservableCollection<EquipmentModel>((IEnumerable<EquipmentModel>) GmmInterface.DeviceManager.GetEquipmentModels(this.SelectedEquipmentGroup).OrderBy<EquipmentModel, string>((Func<EquipmentModel, string>) (e => e.Name)));
        this.OnPropertyChanged("EquipmentCollection");
      }
    }

    [Required(ErrorMessage = "EQUIPMENT_MODEL_SELECTION_REQUIRED")]
    public EquipmentModel SelectedEquipmentModel
    {
      get => this._selectedEquipmentModel;
      set
      {
        this._selectedEquipmentModel = value;
        this.OnPropertyChanged(nameof (SelectedEquipmentModel));
        List<DeviceModel> list = GmmInterface.DeviceManager.GetDeviceModels(this.SelectedEquipmentModel).Where<DeviceModel>((Func<DeviceModel, bool>) (deviceModel => this._licenseDeviceTypes.Contains<string>(deviceModel.Name))).ToList<DeviceModel>();
        this.IsDeviceGroupVisible = this.SelectedEquipmentModel != null && list.Count > 6;
        this.OnPropertyChanged("DeviceGroupCollection");
        this.OnPropertyChanged("DeviceModelCollection");
        this.IsProfileTypeEnabled = this.SelectedEquipmentModel == null || this.SelectedDeviceModel == null ? Visibility.Collapsed : Visibility.Visible;
        this.IsDeviceModelEnabled = this.SelectedEquipmentModel != null && (this.IsDeviceGroupVisible && this.SelectedDeviceGroup != null || !this.IsDeviceGroupVisible);
        this.SetAreChangeableParametersGridsVisible();
      }
    }

    public bool IsBusy
    {
      get => this._isBusy;
      set
      {
        if (this._isBusy == value)
          return;
        this._isBusy = value;
        this.OnPropertyChanged(nameof (IsBusy));
      }
    }

    public bool IsTextBoxRowVisible
    {
      get => this._isTextBoxRowVisible;
      set
      {
        this._isTextBoxRowVisible = value;
        this.OnPropertyChanged(nameof (IsTextBoxRowVisible));
      }
    }

    public bool IsDeviceModelEnabled
    {
      get => this._isDeviceModelEnabled;
      set
      {
        this._isDeviceModelEnabled = value;
        this.OnPropertyChanged(nameof (IsDeviceModelEnabled));
      }
    }

    public Visibility ExpertConfigurationVisibility
    {
      get => this._expertConfigurationVisibility;
      set
      {
        this._expertConfigurationVisibility = value;
        this.OnPropertyChanged(nameof (ExpertConfigurationVisibility));
      }
    }

    public bool IsExpertConfigurationEnabled
    {
      get => this._isExpertConfigurationEnabled;
      set
      {
        this._isExpertConfigurationEnabled = value;
        this.OnPropertyChanged(nameof (IsExpertConfigurationEnabled));
      }
    }

    public bool IsEquipmentChangeableParametersGridVisible
    {
      get => this._isEquipmentChangeableParametersGridVisible;
      set
      {
        this._isEquipmentChangeableParametersGridVisible = value;
        this.OnPropertyChanged(nameof (IsEquipmentChangeableParametersGridVisible));
      }
    }

    public List<MSS.Business.Modules.Configuration.Config> ChangeableParameters { get; set; }

    public bool AreChangeableParametersGridsVisible
    {
      get => this._areChangeableParametersGridsVisible;
      set
      {
        this._areChangeableParametersGridsVisible = value;
        this.OnPropertyChanged(nameof (AreChangeableParametersGridsVisible));
      }
    }

    public bool IsDeviceGroupVisible
    {
      get => this._isDeviceGroupVisible;
      set
      {
        this._isDeviceGroupVisible = value;
        this.OnPropertyChanged(nameof (IsDeviceGroupVisible));
      }
    }

    public DeviceGroup SelectedDeviceGroup
    {
      get => this._selectedDeviceGroup;
      set
      {
        this._selectedDeviceGroup = value;
        this.OnPropertyChanged(nameof (SelectedDeviceGroup));
        this.IsDeviceModelEnabled = this.SelectedEquipmentModel != null && (this.IsDeviceGroupVisible && this.SelectedDeviceGroup != null || !this.IsDeviceGroupVisible);
        this.OnPropertyChanged("DeviceModelCollection");
      }
    }

    public List<DeviceGroup> DeviceGroupCollection
    {
      get
      {
        this.SelectedDeviceGroup = (DeviceGroup) null;
        if (this.SelectedEquipmentModel == null)
          return (List<DeviceGroup>) null;
        List<DeviceGroup> deviceGroups = GmmInterface.DeviceManager.GetDeviceGroups();
        List<DeviceGroup> deviceGroupCollection = new List<DeviceGroup>();
        List<DeviceModel> list = GmmInterface.DeviceManager.GetDeviceModels(this.SelectedEquipmentModel).Where<DeviceModel>((Func<DeviceModel, bool>) (deviceModel => this._licenseDeviceTypes.Contains<string>(deviceModel.Name))).ToList<DeviceModel>();
        foreach (DeviceGroup deviceGroup1 in deviceGroups)
        {
          DeviceGroup deviceGroup = deviceGroup1;
          if (list.Any<DeviceModel>((Func<DeviceModel, bool>) (d => d.DeviceGroup != null && d.DeviceGroup.Name == deviceGroup.Name)))
            deviceGroupCollection.Add(deviceGroup);
        }
        return deviceGroupCollection;
      }
    }

    public System.Windows.Input.ICommand OpenUpdateFirmwareDialogCommad
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.ConfigurationParameters.ClearCurrentlyShownControls();
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<FirmwareUpdateViewModel>((IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("firmwareCache", (object) this._firmwareCache), (IParameter) new ConstructorArgument("selectedDeviceModel", (object) this.SelectedDeviceModel)));
        }));
      }
    }

    private void BackgroundWorkerReadDeviceDoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        string name = (string) e.Argument;
        Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);
        this.ValidateProperty("SelectedEquipmentModel");
        this.ValidateProperty("SelectedDeviceModel");
        this.ValidateChangeableParameters(this.ChangeableParameters);
        bool isValid = this.IsValid;
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, (Delegate) (() =>
        {
          this.BusyContent = Resources.MSS_Configuration_Reading;
          this.IsBusy = isValid;
        }));
        this.ResetConnectionAdjuster();
        MSS.DTO.Message.Message message = this.ConfigurationParameters.ReadConfiguration(isValid, this._connectionAdjuster);
        this.LogChangeableParameters(this.ConfigurationParameters.ConfigValuesCollection);
        e.Result = (object) message;
      }
      catch (Exception ex)
      {
        e.Result = (object) ex;
      }
    }

    private void BackgroundWorkerReadDeviceRunWorkerCompleted(
      object sender,
      RunWorkerCompletedEventArgs e)
    {
      this.IsBusy = false;
      if (e.Result is Exception)
      {
        string message = ((Exception) e.Result).Message;
        if (((Exception) e.Result).InnerException != null && ((Exception) e.Result).InnerException.Message != null)
          message = ((Exception) e.Result).InnerException.Message;
        ConfigurationViewModel.logger.Log<Exception>(NLog.LogLevel.Error, e.Error);
        MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), message, false);
      }
      else
      {
        if (!(e.Result is MSS.DTO.Message.Message))
          return;
        this.MessageUserControl = this.MessageShower.Show((MSS.DTO.Message.Message) e.Result);
      }
    }

    public System.Windows.Input.ICommand ReadConfigurationCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          try
          {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(this.BackgroundWorkerReadDeviceDoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BackgroundWorkerReadDeviceRunWorkerCompleted);
            backgroundWorker.RunWorkerAsync((object) Thread.CurrentThread.CurrentCulture.Name);
          }
          catch (Exception ex)
          {
            ConfigurationViewModel.logger.Log<Exception>(NLog.LogLevel.Error, ex);
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), ex.Message, false);
          }
          finally
          {
            this.IsBusy = false;
          }
        }));
      }
    }

    public System.Windows.Input.ICommand TestConfigurationCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.IsBusy = true;
          try
          {
          }
          catch (Exception ex)
          {
            ConfigurationViewModel.logger.Log<Exception>(NLog.LogLevel.Error, ex);
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), ex.Message, false);
            this.IsBusy = false;
          }
          this.IsBusy = false;
        }));
      }
    }

    public System.Windows.Input.ICommand ExpertConfigurationCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this._connectionAdjuster == null)
            return;
          this.ResetConnectionAdjuster();
          ExpertConfigurationViewModel configurationViewModel = DIConfigurator.GetConfigurator().Get<ExpertConfigurationViewModel>((IParameter) new ConstructorArgument("connectionAdjuster", (object) this._connectionAdjuster));
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) configurationViewModel);
          if (newModalDialog.HasValue & newModalDialog.Value)
          {
            this._connectionAdjuster = configurationViewModel.SelectedConnectionAdjuster;
            this.RefreshChangeableParameters(false);
          }
        }));
      }
    }

    public System.Windows.Input.ICommand WriteConfigurationCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.BusyContent = Resources.MSS_Configuration_Writing;
          this.IsBusy = true;
          this.ValidateProperty("SelectedEquipmentModel");
          this.ValidateProperty("SelectedDeviceModel");
          try
          {
            this.MessageUserControl = this.MessageShower.Show(this.ConfigurationParameters.WriteConfiguration(this.IsValid, _));
          }
          catch (Exception ex)
          {
            ConfigurationViewModel.logger.Log<Exception>(NLog.LogLevel.Error, ex);
            string warningMessage = !string.IsNullOrEmpty(ex.Message) ? ex.Message : Resources.MSS_Client_ErrorWritingToDevice;
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), warningMessage, false);
          }
          finally
          {
            this.IsBusy = false;
          }
        }));
      }
    }

    public System.Windows.Input.ICommand ExportConfigurationParametersToPDFCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Filter = "PDF Document|*.pdf";
          saveFileDialog.Title = Resources.MSS_Client_SaveConfigurationParametersToFile;
          bool? nullable = saveFileDialog.ShowDialog();
          if (saveFileDialog.FileName == string.Empty || !nullable.HasValue || !nullable.Value)
            return;
          this.IsBusy = true;
          this.MessageUserControl = this.MessageShower.Show(this.ConfigurationParameters.ExportConfigurationParametersToPdf(this.SelectedDeviceModel.Name, saveFileDialog.SafeFileName, saveFileDialog.OpenFile()));
          this.IsBusy = false;
        }));
      }
    }

    public System.Windows.Input.ICommand StartReadingDeviceValuesCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.ConfigurationParameters.ClearCurrentlyShownControls();
          this.ValidateProperty("SelectedEquipmentModel");
          this.ValidateProperty("SelectedDeviceModel");
          this.ValidateProperty("SerialNo");
          if (!this.IsValid)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(new MSS.DTO.Message.Message()
            {
              MessageType = MessageTypeEnum.Validation,
              MessageText = MessageCodes.ValidationError.GetStringValue()
            }.MessageText);
          }
          else
          {
            this.ResetConnectionAdjuster();
            if (this.CheckIfLoRaDevice())
              this.ReadLoRaDeviceReadingValues();
            else
              this.ReadNonLoRaDeviceReadingValues();
          }
        }));
      }
    }

    private bool CheckIfLoRaDevice()
    {
      return this.SelectedDeviceModel.Parameters.ContainsKey(ConnectionProfileParameter.Handler) && (this.SelectedDeviceModel.Parameters[ConnectionProfileParameter.Handler] == "EDCL_Handler" || this.SelectedDeviceModel.Parameters[ConnectionProfileParameter.Handler] == "M8_Handler" || this.SelectedDeviceModel.Parameters[ConnectionProfileParameter.Handler] == "PDCL2_Handler");
    }

    private async void ReadLoRaDeviceReadingValues()
    {
      this.BusyContent = Resources.MSS_Configuration_Reading;
      this.IsBusy = true;
      ZENNER.CommonLibrary.Entities.Meter meter = new ZENNER.CommonLibrary.Entities.Meter()
      {
        ID = Guid.Empty,
        DeviceModel = this.SelectedDeviceModel,
        ConnectionAdjuster = this._connectionAdjuster
      };
      this.SelectedEquipmentModel.ChangeableParameters[0].Value = this.ChangeableParameters.Where<MSS.Business.Modules.Configuration.Config>((Func<MSS.Business.Modules.Configuration.Config, bool>) (x => x.PropertyName == "Port")).Select<MSS.Business.Modules.Configuration.Config, string>((Func<MSS.Business.Modules.Configuration.Config, string>) (x => x.PropertyValue)).FirstOrDefault<string>();
      ZENNER.ICommand cmd = await this.configManager.ConnectAsync(this.SelectedEquipmentModel, meter, this.SelectedProfileType);
      List<Device> devices = await cmd.ReadValuesAsync((ProgressHandler) null, new CancellationTokenSource().Token);
      List<MeterReadingValue> readingValues = new List<MeterReadingValue>();
      if (devices != null && devices.Count > 0)
      {
        foreach (Device device in devices)
        {
          if (device.ValueSets != null)
          {
            foreach (KeyValuePair<long, SortedList<DateTime, double>> valueSet1 in device.ValueSets)
            {
              KeyValuePair<long, SortedList<DateTime, double>> valueSet = valueSet1;
              long valueId = valueSet.Key;
              foreach (KeyValuePair<DateTime, double> keyValuePair in valueSet.Value)
              {
                KeyValuePair<DateTime, double> valueReceived = keyValuePair;
                MeterReadingValue readingValue = new MeterReadingValue()
                {
                  MeterSerialNumber = device.ID,
                  CreatedOn = DateTime.Now,
                  Date = valueReceived.Key,
                  Value = valueReceived.Value,
                  ValueId = valueId,
                  StorageInterval = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueId),
                  PhysicalQuantity = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueId),
                  MeterType = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueId),
                  CalculationStart = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueId),
                  Creation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueId),
                  Calculation = (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueId),
                  Unit = new MeasureUnit()
                  {
                    Code = ValueIdent.GetUnit(Convert.ToInt64(valueId))
                  }
                };
                readingValues.Add(readingValue);
                readingValue = (MeterReadingValue) null;
                valueReceived = new KeyValuePair<DateTime, double>();
              }
              valueSet = new KeyValuePair<long, SortedList<DateTime, double>>();
            }
          }
        }
      }
      cmd.Disconnect();
      this.OnMeterValuesReceivedHandler((object) null, readingValues);
    }

    private void ReadNonLoRaDeviceReadingValues()
    {
      ZENNER.CommonLibrary.Entities.Meter meter = new ZENNER.CommonLibrary.Entities.Meter()
      {
        ID = Guid.Empty,
        DeviceModel = this.SelectedDeviceModel,
        ConnectionAdjuster = this._connectionAdjuster
      };
      this._deviceReaderManager = new DeviceReaderManager(this._repositoryFactory);
      this._deviceReaderManager.OnMeterValuesReceivedHandler += new EventHandler<List<MeterReadingValue>>(this.OnMeterValuesReceivedHandler);
      this._deviceReaderManager.OnErrorReceivedHandler += new EventHandler<Exception>(this.OnErrorReceivedHandler);
      this._deviceReaderManager.OnMissingTranslationRule += new EventHandler<string>(this.OnMissingTranslationRule);
      this._deviceReaderManager.StartReadingValues(meter, this.SelectedEquipmentModel, this.SelectedProfileType ?? GmmInterface.DeviceManager.GetProfileTypes(meter).FirstOrDefault<ProfileType>());
      this.BusyContent = Resources.MSS_Configuration_Reading;
      this.IsBusy = true;
    }

    private void ValidateChangeableParameters(List<MSS.Business.Modules.Configuration.Config> changeableParameters)
    {
      if (changeableParameters == null)
      {
        this.IsValid = false;
      }
      else
      {
        if (new ValidationRuleUtils().AreChangeableParametersValid(changeableParameters.Where<MSS.Business.Modules.Configuration.Config>((Func<MSS.Business.Modules.Configuration.Config, bool>) (p => p.Parameter is ChangeableParameter && (p.Parameter as ChangeableParameter).ParameterUsing != 0)).ToList<MSS.Business.Modules.Configuration.Config>()))
          return;
        this.IsValid = false;
      }
    }

    public override List<string> ValidateProperty(string propertyName)
    {
      List<string> validationErrors = new List<string>();
      string propertyName1 = this.GetPropertyName<DeviceGroup>((Expression<Func<DeviceGroup>>) (() => this.SelectedDeviceGroup));
      if (propertyName == propertyName1)
      {
        this.AddValidationError(validationErrors, Resources.MSS_Client_Structures_DeviceGroupErrorToolTip, this.IsDeviceGroupVisible && this.SelectedDeviceGroup == null);
        return validationErrors;
      }
      string propertyName2 = this.GetPropertyName<EquipmentModel>((Expression<Func<EquipmentModel>>) (() => this.SelectedEquipmentModel));
      if (propertyName == propertyName2)
      {
        this.AddValidationError(validationErrors, Resources.MSS_Client_Configuration_EquipmentErrorToolTip, this.SelectedEquipmentModel == null);
        return validationErrors;
      }
      string propertyName3 = this.GetPropertyName<DeviceModel>((Expression<Func<DeviceModel>>) (() => this.SelectedDeviceModel));
      if (propertyName == propertyName3)
        this.AddValidationError(validationErrors, Resources.MSS_Client_Configuration_DeviceModelErrorToolTip, this.SelectedDeviceModel == null);
      return validationErrors;
    }

    private void AddValidationError(
      List<string> validationErrors,
      string errorMessage,
      bool condition)
    {
      if (!condition)
        return;
      validationErrors.Add(errorMessage);
      this.IsValid = false;
    }

    private void OnMeterValuesReceivedHandler(
      object sender,
      List<MeterReadingValue> meterReadingValues)
    {
      ObservableCollection<MeterReadingValueDTO> readingValues = new ObservableCollection<MeterReadingValueDTO>();
      Mapper.CreateMap<MeterReadingValue, MeterReadingValueDTO>();
      foreach (object meterReadingValue in meterReadingValues)
      {
        MeterReadingValueDTO meterReadingValueDto = Mapper.Map<MeterReadingValueDTO>(meterReadingValue);
        readingValues.Add(meterReadingValueDto);
      }
      this.IsBusy = false;
      Application.Current.Dispatcher.Invoke((Action) (() => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MeterReadingValuesViewModel>((IParameter) new ConstructorArgument("readingValues", (object) readingValues)))));
    }

    private void OnErrorReceivedHandler(object sender, Exception e)
    {
      this.IsBusy = false;
      Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(e.Message)));
    }

    private void OnMissingTranslationRule(object sender, string e)
    {
      Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(e)));
    }

    private void SetIsExpertConfigurationEnabled()
    {
      this.IsExpertConfigurationEnabled = this.ExpertConfigurationVisibility == Visibility.Visible && this.SelectedEquipmentModel != null && this.SelectedDeviceModel != null && this.SelectedProfileType != null;
    }

    private void ResetConnectionAdjuster()
    {
      if (this._connectionAdjuster == null)
        return;
      this._connectionAdjuster = new ConnectionAdjuster(this._connectionAdjuster.ConnectionProfileID, this._connectionAdjuster.Name, GMMHelper.ReplaceValuesInChangeableParameters(this._connectionAdjuster.SetupParameters, this.ChangeableParameters));
    }

    private void PublishEventToRefreshTheChangeableParametersGrid(
      List<MSS.Business.Modules.Configuration.Config> changeableParameters,
      string stackPanelName,
      ChangeableParameterUsings type,
      double firstColumnPercentage,
      double gridWidth)
    {
      EventPublisher.Publish<ChangeableParametersLoadedEvent>(new ChangeableParametersLoadedEvent()
      {
        ChangeableParameters = changeableParameters,
        StackPanelName = stackPanelName,
        Type = type,
        GridFirstColumnPercentage = firstColumnPercentage,
        GridWidth = gridWidth
      }, (IViewModel) this);
    }

    private void SetAreChangeableParametersGridsVisible()
    {
      this.AreChangeableParametersGridsVisible = this.SelectedEquipmentModel != null && this.SelectedDeviceModel != null && this.SelectedProfileType != null;
    }

    private void PublishEventsToRefreshTheChangeableParametersGrids()
    {
      this.PublishEventToRefreshTheChangeableParametersGrid(this.ChangeableParameters, "EquipmentChangeableParametersStackPanel", ChangeableParameterUsings.changableByEquipment, 25.0, this.isTabletMode ? 425.0 : 330.0);
      this.PublishEventToRefreshTheChangeableParametersGrid(this.ChangeableParameters, "DeviceModelChangeableParametersStackPanel", ChangeableParameterUsings.changableByDevice, this.isTabletMode ? 45.0 : 40.0, this.isTabletMode ? 380.0 : 310.0);
      this.PublishEventToRefreshTheChangeableParametersGrid(this.ChangeableParameters, "ProfileTypeChangeableParametersStackPanel", ChangeableParameterUsings.changableByProfileType, 50.0, this.isTabletMode ? 350.0 : 300.0);
    }

    private void RefreshChangeableParameters(bool resetConnectionAdjuster)
    {
      if (!this.AreChangeableParametersGridsVisible)
        return;
      if (resetConnectionAdjuster)
      {
        if (this._connectionAdjuster == null)
          this._connectionAdjuster = GmmInterface.DeviceManager.GetConnectionAdjuster(this.SelectedDeviceModel, this.SelectedEquipmentModel, this.SelectedProfileType);
        if (this.ChangeableParameters != null)
        {
          this.SelectedEquipmentModel.ChangeableParameters = this.GetUpdatedChangeableParameters(this.SelectedEquipmentModel.ChangeableParameters, this.ChangeableParameters, ChangeableParameterUsings.changableByEquipment);
          this.SelectedDeviceModel.ChangeableParameters = this.GetUpdatedChangeableParameters(this.SelectedDeviceModel.ChangeableParameters, this.ChangeableParameters, ChangeableParameterUsings.changableByDevice);
          this.SelectedProfileType.ChangeableParameters = this.GetUpdatedChangeableParameters(this.SelectedProfileType.ChangeableParameters, this.ChangeableParameters, ChangeableParameterUsings.changableByProfileType);
        }
        this._connectionAdjuster = GmmInterface.DeviceManager.GetConnectionAdjuster(this.SelectedDeviceModel, this.SelectedEquipmentModel, this.SelectedProfileType);
        int connectionProfileId = this._connectionAdjuster.ConnectionProfileID;
        string name = this._connectionAdjuster.Name;
        List<ChangeableParameter> setupParameters1 = this._connectionAdjuster.SetupParameters;
        List<MSS.Business.Modules.Configuration.Config> changeableParameters = this.ChangeableParameters;
        List<MSS.Business.Modules.Configuration.Config> list = changeableParameters != null ? changeableParameters.Where<MSS.Business.Modules.Configuration.Config>((Func<MSS.Business.Modules.Configuration.Config, bool>) (p => p.Parameter is ChangeableParameter && ((ChangeableParameter) p.Parameter).ParameterUsing != 0)).ToList<MSS.Business.Modules.Configuration.Config>() : (List<MSS.Business.Modules.Configuration.Config>) null;
        List<ChangeableParameter> setupParameters2 = GMMHelper.ReplaceValuesInChangeableParameters(setupParameters1, list, true);
        this._connectionAdjuster = new ConnectionAdjuster(connectionProfileId, name, setupParameters2);
        this.ChangeableParameters = this._connectionAdjuster != null ? MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(this._connectionAdjuster.SetupParameters) : new List<MSS.Business.Modules.Configuration.Config>();
      }
      else
      {
        this.ChangeableParameters = this._connectionAdjuster != null ? MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(this._connectionAdjuster.SetupParameters) : new List<MSS.Business.Modules.Configuration.Config>();
        this._connectionAdjuster = new ConnectionAdjuster(this._connectionAdjuster.ConnectionProfileID, this._connectionAdjuster.Name, GMMHelper.ReplaceValuesInChangeableParameters(this._connectionAdjuster.SetupParameters, this.ChangeableParameters));
      }
      this.PublishEventsToRefreshTheChangeableParametersGrids();
    }

    private void LogChangeableParameters(
      List<ConfigurationPerChannel> configurationPerChannels)
    {
      if (configurationPerChannels.Count < 1)
        return;
      ConfigurationViewModel.logger.Log(NLog.LogLevel.Debug, "After reading the device " + this.SelectedDeviceModel.Name + " with " + this.SelectedEquipmentModel.Name + " on " + this.SelectedProfileType.Name);
      foreach (ConfigurationPerChannel configurationPerChannel in configurationPerChannels)
      {
        ConfigurationViewModel.logger.Log(NLog.LogLevel.Debug, configurationPerChannel.ConfigValues.Count.ToString() + " changeable parameters received on channel " + (object) configurationPerChannel.ChannelNr);
        foreach (MSS.Business.Modules.Configuration.Config configValue in configurationPerChannel.ConfigValues)
        {
          string str1 = configValue.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? ((KeyValuePair<OverrideID, ConfigurationParameter>) configValue.Parameter).Value.IsFunction.ToString() : "-";
          string str2 = configValue.Parameter is KeyValuePair<OverrideID, ConfigurationParameter> ? ((KeyValuePair<OverrideID, ConfigurationParameter>) configValue.Parameter).Value.IsEditable.ToString() : "-";
          ConfigurationViewModel.logger.Log(NLog.LogLevel.Debug, "Property: " + configValue.PropertyName + " ; ReadOnly: " + configValue.IsReadOnly.ToString() + " ; IsEditable: " + str2 + " ; IsFunction: " + str1 + " ; has Value: " + configValue.PropertyValue);
        }
      }
    }

    private List<ChangeableParameter> GetUpdatedChangeableParameters(
      List<ChangeableParameter> listToBeUpdated,
      List<MSS.Business.Modules.Configuration.Config> listToUpdateFrom,
      ChangeableParameterUsings type)
    {
      return GMMHelper.ReplaceValuesInChangeableParameters(listToBeUpdated, listToUpdateFrom.Where<MSS.Business.Modules.Configuration.Config>((Func<MSS.Business.Modules.Configuration.Config, bool>) (p => p.Parameter is ChangeableParameter && ((ChangeableParameter) p.Parameter).ParameterUsing == type)).ToList<MSS.Business.Modules.Configuration.Config>());
    }
  }
}
