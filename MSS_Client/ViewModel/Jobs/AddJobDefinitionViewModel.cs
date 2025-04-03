// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.AddJobDefinitionViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.UsersManagement;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Jobs;
using MSS.DIConfiguration;
using MSS.DTO.Jobs;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MSS_Client.ViewModel.DataFilters;
using MSS_Client.ViewModel.Settings;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Telerik.Windows.Data;
using ZENNER;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class AddJobDefinitionViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly bool _isUpdate;
    private bool _isSameConfig;
    private readonly JobDefinitionDto _jdDto = new JobDefinitionDto();
    private ServiceTask _selectedServiceJob;
    private IEnumerable<ServiceTask> _serviceJobs = (IEnumerable<ServiceTask>) new RadObservableCollection<ServiceTask>();
    private DeviceModel _selectedSystem;
    private IEnumerable<DeviceModel> _systemList;
    private string _selectedEquipmentstring;
    private bool _isVisible;
    private string _equipmentParams;
    private ViewModelBase _messageUserControl;
    private ProfileType _profileType;
    private IEnumerable<ProfileType> _profileTypes;
    private RadObservableCollection<Rules> _rules;
    private MSS.Core.Model.DataFilters.Filter _selectedFilter;
    private string _name;
    private string _title;
    private IEnumerable<MSS.Core.Model.DataFilters.Filter> _filterCollection;
    private bool _serviceJobVisibility;

    public AddJobDefinitionViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._isUpdate = false;
      this._isSameConfig = this._isUpdate;
      this.IsReadingJob = true;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      EventPublisher.Register<RefreshFilters>(new Action<RefreshFilters>(this.RefreshFilter));
      EventPublisher.Register<SendSerializedDataEvent>(new Action<SendSerializedDataEvent>(this.RegisterToSerializedEvent));
      EventPublisher.Register<UpdateDefaultEquipment>(new Action<UpdateDefaultEquipment>(this.UpdateDefaultEquipmentEvent));
      EventPublisher.Register<SetSystemChangeableParamsEvent>(new Action<SetSystemChangeableParamsEvent>(this.SetSystemChangeableParams));
      this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
      this.Title = Resources.MSS_Client_Jobs_CreateJobDefinition_Title;
      if (MSS.Business.Utils.AppContext.Current.DefaultEquipment != null)
      {
        Task<List<Config>> equipmentConfigsList = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.CreateEquipmentConfigsList(MSS.Business.Utils.AppContext.Current.DefaultEquipment);
        this.SelectedEquipmentName = MSS.Business.Utils.AppContext.Current.DefaultEquipment.Name;
        this.EquipmentParams = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(equipmentConfigsList.Result, MSS.Business.Utils.AppContext.Current.DefaultEquipment.ChangeableParameters);
      }
      else
        this.SelectedEquipmentName = string.Empty;
      this.ServiceJobVisibility = new UsersManager(this._repositoryFactory).HasRight(OperationEnum.ServiceJobCreate.ToString());
    }

    public AddJobDefinitionViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      JobDefinitionDto jdDto)
    {
      this._jdDto = jdDto;
      this._isUpdate = true;
      this._isSameConfig = this._isUpdate;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this.IsReadingJob = true;
      EventPublisher.Register<RefreshFilters>(new Action<RefreshFilters>(this.RefreshFilter));
      EventPublisher.Register<SendSerializedDataEvent>(new Action<SendSerializedDataEvent>(this.RegisterToSerializedEvent));
      EventPublisher.Register<UpdateDefaultEquipment>(new Action<UpdateDefaultEquipment>(this.UpdateDefaultEquipmentEvent));
      EventPublisher.Register<SetSystemChangeableParamsEvent>(new Action<SetSystemChangeableParamsEvent>(this.SetSystemChangeableParams));
      this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
      this.Title = Resources.MSS_Client_Jobs_EditJobDefinition_Title;
      JobDefinition jd = this._repositoryFactory.GetRepository<JobDefinition>().FirstOrDefault((Expression<Func<JobDefinition, bool>>) (x => x.Id == jdDto.Id));
      if (jd != null)
      {
        this.Name = jdDto.Name;
        EquipmentModel equipmentModel = GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.Name == jd.EquipmentModel));
        if (equipmentModel != null)
          this.SelectedEquipmentName = equipmentModel.Name;
        if (jd.Filter != null)
          this.SelectedFilter = this.FilterCollection.FirstOrDefault<MSS.Core.Model.DataFilters.Filter>((Func<MSS.Core.Model.DataFilters.Filter, bool>) (x => x.Id == jd.Filter.Id));
        else
          this.IsServiceJob = true;
        this.SelectedSystem = this.SystemList.FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.Name == jd.System));
        this.IntervalBytes = jdDto.Interval;
        this.EquipmentParams = jdDto.EquipmentParams;
        if (!string.IsNullOrEmpty(this.SelectedEquipmentName) && this.SelectedSystem != null)
        {
          this.ProfileTypes = (IEnumerable<ProfileType>) GmmInterface.DeviceManager.GetProfileTypes(this.SelectedSystem, GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.Name == this.SelectedEquipmentName)), new ProfileTypeTags?(ProfileTypeTags.JobManager));
          if (this.ProfileTypes.Count<ProfileType>() != 0)
          {
            this.ProfileType = this.ProfileTypes.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (x => x.Name == jdDto.ProfileType));
            if (this.ProfileType != null && !string.IsNullOrEmpty(jdDto.ProfileTypeParams))
              MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateProfileTypeWithSavedParams(this.ProfileType, jdDto.ProfileTypeParams);
          }
        }
        if (!string.IsNullOrEmpty(jdDto.ServiceJob))
        {
          this.ServiceJobs = (IEnumerable<ServiceTask>) ServiceTaskManager.GetServices(GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.SystemDevice).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.Name == this.SelectedSystem.Description))).OrderBy<ServiceTask, string>((Func<ServiceTask, string>) (s => s.Description));
          this.SelectedServiceJob = this.ServiceJobs.FirstOrDefault<ServiceTask>((Func<ServiceTask, bool>) (x => x.Description == jdDto.ServiceJob));
        }
        this.DueDate = jd.DueDate;
        this.Month = jd.Month;
        this.Day = jd.Day;
        this.QuarterHour = jd.QuarterHour;
      }
      this.ServiceJobVisibility = new UsersManager(this._repositoryFactory).HasRight(OperationEnum.ServiceJobCreate.ToString());
    }

    private void SetSystemChangeableParams(SetSystemChangeableParamsEvent ev)
    {
      this.DueDate = ev.DueDate;
      this.Month = ev.Month;
      this.Day = ev.Day;
      this.QuarterHour = ev.QuarterHour;
      this._isSameConfig = true;
    }

    private void UpdateDefaultEquipmentEvent(UpdateDefaultEquipment ev)
    {
      this.SelectedEquipmentName = ev.SelectedEquipmentModel.Name;
      this.EquipmentParams = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(ev.ChangeableParameters, ev.SelectedEquipmentModel.ChangeableParameters);
    }

    private void RegisterToSerializedEvent(SendSerializedDataEvent ev)
    {
      this.IntervalBytes = ev.SerializedObject;
    }

    private void RefreshFilter(RefreshFilters obj)
    {
      this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
    }

    public byte[] IntervalBytes { get; set; }

    public IEnumerable<EquipmentModel> EquipmentList
    {
      get => (IEnumerable<EquipmentModel>) GmmInterface.DeviceManager.GetEquipmentModels();
    }

    public bool IsReadingJob { get; set; }

    public bool IsServiceJob { get; set; }

    public ServiceTask SelectedServiceJob
    {
      get => this._selectedServiceJob;
      set
      {
        this._selectedServiceJob = value;
        this.OnPropertyChanged(nameof (SelectedServiceJob));
      }
    }

    public IEnumerable<ServiceTask> ServiceJobs
    {
      get => this._serviceJobs;
      set
      {
        this._serviceJobs = value;
        this.OnPropertyChanged(nameof (ServiceJobs));
      }
    }

    [Required(ErrorMessage = "MSS_CLIENT_CREATE_JOB_SYSTEM")]
    public DeviceModel SelectedSystem
    {
      get => this._selectedSystem;
      set
      {
        this._selectedSystem = value;
        if (!string.IsNullOrEmpty(this.SelectedEquipmentName))
        {
          this.ProfileTypes = (IEnumerable<ProfileType>) GmmInterface.DeviceManager.GetProfileTypes(this.SelectedSystem, GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.Name == this.SelectedEquipmentName)), new ProfileTypeTags?(ProfileTypeTags.JobManager));
          if (this.SelectedSystem != null)
          {
            DeviceModel model = GmmInterface.DeviceManager.GetDeviceModels(DeviceModelTags.SystemDevice).FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.Name == this.SelectedSystem.Description));
            if (model != null)
              this.ServiceJobs = (IEnumerable<ServiceTask>) ServiceTaskManager.GetServices(model).OrderBy<ServiceTask, string>((Func<ServiceTask, string>) (s => s.Description));
          }
        }
        this.IsVisible = this.SelectedSystem != null && (this.SelectedSystem.Description == "Minomat V4 Master" || this.SelectedSystem.Description == "Minomat V4 Slave");
        if (this.SelectedSystem != null && this._isUpdate && this.SelectedSystem.Description != this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) this._jdDto.Id).System)
        {
          this.DueDate = new TimeSpan?();
          this.Month = new TimeSpan?();
          this.Day = new TimeSpan?();
          this.QuarterHour = new TimeSpan?();
          this._isSameConfig = false;
        }
        else if (this._isUpdate && this.SelectedSystem != null && this.SelectedSystem.Description == this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) this._jdDto.Id).System)
        {
          JobDefinition jobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().FirstOrDefault((Expression<Func<JobDefinition, bool>>) (x => x.Id == this._jdDto.Id));
          this.DueDate = jobDefinition.DueDate;
          this.Month = jobDefinition.Month;
          this.Day = jobDefinition.Day;
          this.QuarterHour = jobDefinition.QuarterHour;
          this._isSameConfig = true;
        }
        this.OnPropertyChanged(nameof (SelectedSystem));
      }
    }

    public IEnumerable<DeviceModel> SystemList
    {
      get => this._systemList;
      set
      {
        this._systemList = value;
        this.OnPropertyChanged(nameof (SystemList));
      }
    }

    public System.Windows.Input.ICommand OpenFilterCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<FilterViewModel>());
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public System.Windows.Input.ICommand OpenEquipmentSelection
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          EquipmentModel equipment = GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.Name == this.SelectedEquipmentName));
          ConfigChangeableParamsViewModel changeableParamsViewModel;
          if (equipment != null)
          {
            MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateEquipmentWithSavedParams(equipment, this.EquipmentParams);
            changeableParamsViewModel = DIConfigurator.GetConfigurator().Get<ConfigChangeableParamsViewModel>((IParameter) new ConstructorArgument("equipmentModel", (object) equipment));
          }
          else
            changeableParamsViewModel = DIConfigurator.GetConfigurator().Get<ConfigChangeableParamsViewModel>();
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) changeableParamsViewModel);
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_DefaultEquipment_Update_Message);
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public System.Windows.Input.ICommand OpenSystemSelection
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<SystemSelectionViewModel>((IParameter) new ConstructorArgument("selectedSystem", (object) this.SelectedSystem), (IParameter) new ConstructorArgument("isUpdate", (object) this._isSameConfig), (IParameter) new ConstructorArgument("jobDto", (object) this._jdDto), (IParameter) new ConstructorArgument("currentParams", (object) new ChangeableParametersSystem()
          {
            Day = this.Day,
            DueDate = this.DueDate,
            Month = this.Month,
            QuarterHour = this.QuarterHour
          })));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_DefaultEquipment_Update_Message);
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public System.Windows.Input.ICommand OpenIntervalCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<IntervalsViewModel>((IParameter) new ConstructorArgument("interval", (object) this.IntervalBytes)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public System.Windows.Input.ICommand ChangeProfileTypeCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.ProfileType == null)
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_NoProfileSelected), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
          else if (this.ProfileTypes != null && this.ProfileType.ChangeableParameters != null)
          {
            ProfileTypeViewModel profileTypeViewModel = DIConfigurator.GetConfigurator().Get<ProfileTypeViewModel>((IParameter) new ConstructorArgument("profileTypeCollection", (object) this.ProfileTypes), (IParameter) new ConstructorArgument("selectedProfileType", (object) this.ProfileType));
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) profileTypeViewModel);
            if (!(newModalDialog.HasValue & newModalDialog.Value))
              return;
            this.ProfileType = profileTypeViewModel.SelectedProfileType;
          }
          else
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_CannotModifyChangeableParamsDueToLicense), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
        }));
      }
    }

    public System.Windows.Input.ICommand SaveJobDefinition
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!this.IsValid)
            return;
          if (string.IsNullOrEmpty(this.SelectedEquipmentName))
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_CREATE_JOB_REQUIRED_EQUIPMENT);
          else if (this.IntervalBytes == null)
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_CREATE_JOB_REQUIRED_INTERVAL);
          else if (this.IsReadingJob && this.SelectedFilter == null)
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_CREATE_JOB_REQUIRED_FILTER);
          else if (this.IsServiceJob && this.SelectedServiceJob == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CLIENT_CREATE_JOB_REQUIRED_SERVICE);
          }
          else
          {
            JobDefinition jobDefinition = new JobDefinition();
            if (this._isUpdate)
              jobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().FirstOrDefault((Expression<Func<JobDefinition, bool>>) (x => x.Id == this._jdDto.Id)) ?? new JobDefinition();
            jobDefinition.EquipmentModel = this.SelectedEquipmentName;
            jobDefinition.System = this.SelectedSystem.Name;
            if (this.IsReadingJob)
            {
              jobDefinition.Filter = this.SelectedFilter;
              jobDefinition.ServiceJob = (string) null;
            }
            else
            {
              jobDefinition.ServiceJob = this.SelectedServiceJob.Method.Name;
              jobDefinition.Filter = (MSS.Core.Model.DataFilters.Filter) null;
            }
            jobDefinition.Name = this.Name;
            jobDefinition.StartDate = new DateTime?(DateTime.Now);
            jobDefinition.IsDeactivated = false;
            jobDefinition.EndDate = new DateTime?(DateTime.Now);
            jobDefinition.Interval = this.IntervalBytes;
            jobDefinition.EquipmentParams = this.EquipmentParams ?? "";
            jobDefinition.ProfileType = this.ProfileType.Name;
            if (this.ProfileType?.ChangeableParameters != null)
            {
              List<Config> changeableParameters = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(this.ProfileType.ChangeableParameters);
              jobDefinition.ProfileTypeParams = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(changeableParameters, this.ProfileType.ChangeableParameters);
            }
            TimeSpan? nullable = this.QuarterHour;
            int num;
            if (nullable.HasValue)
            {
              nullable = this.Day;
              if (nullable.HasValue)
              {
                nullable = this.Month;
                if (nullable.HasValue)
                {
                  nullable = this.DueDate;
                  num = nullable.HasValue ? 1 : 0;
                  goto label_21;
                }
              }
            }
            num = 0;
label_21:
            if (num != 0)
            {
              jobDefinition.QuarterHour = this.QuarterHour;
              jobDefinition.Day = this.Day;
              jobDefinition.Month = this.Month;
              jobDefinition.DueDate = this.DueDate;
            }
            else if (this.SelectedSystem.ChangeableParameters != null)
            {
              ChangeableParameter dueDateTimeSpan1 = this.SelectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationDueDate"));
              ChangeableParameter dueDateTimeSpan2 = this.SelectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationMonth"));
              ChangeableParameter dueDateTimeSpan3 = this.SelectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationDay"));
              ChangeableParameter dueDateTimeSpan4 = this.SelectedSystem.ChangeableParameters.FirstOrDefault<ChangeableParameter>((Func<ChangeableParameter, bool>) (x => x.Key == "MinomatV4_DurationQuarterHour"));
              jobDefinition.DueDate = this.GetTimeSpan(dueDateTimeSpan1);
              jobDefinition.Month = this.GetTimeSpan(dueDateTimeSpan2);
              jobDefinition.Day = this.GetTimeSpan(dueDateTimeSpan3);
              jobDefinition.QuarterHour = this.GetTimeSpan(dueDateTimeSpan4);
            }
            EventPublisher.Publish<SaveJobDefinitionEvent>(new SaveJobDefinitionEvent()
            {
              JobDefinition = jobDefinition
            }, (IViewModel) this);
            this.OnRequestClose(true);
          }
        }));
      }
    }

    private TimeSpan? GetTimeSpan(ChangeableParameter dueDateTimeSpan)
    {
      TimeSpan? timeSpan = new TimeSpan?();
      if (dueDateTimeSpan != null)
      {
        string[] strArray = dueDateTimeSpan.Value.Split(':');
        timeSpan = strArray.Length != 4 ? new TimeSpan?(new TimeSpan(0, int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]))) : (int.Parse(strArray[0]) >= 0 ? new TimeSpan?(new TimeSpan(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]), int.Parse(strArray[3]))) : new TimeSpan?(new TimeSpan(int.Parse(strArray[0]), -int.Parse(strArray[1]), -int.Parse(strArray[2]), -int.Parse(strArray[3]))));
      }
      return timeSpan;
    }

    public TimeSpan? DueDate { get; set; }

    public TimeSpan? Month { get; set; }

    public TimeSpan? Day { get; set; }

    public TimeSpan? QuarterHour { get; set; }

    [Required(ErrorMessage = "MSS_CLIENT_CREATE_JOB_REQUIRED_EQUIPMENT_NAME")]
    public string SelectedEquipmentName
    {
      get => this._selectedEquipmentstring;
      set
      {
        this._selectedEquipmentstring = value;
        EquipmentModel equipmentModel = GmmInterface.DeviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (x => x.Name == this.SelectedEquipmentName));
        this.SystemList = (IEnumerable<DeviceModel>) GmmInterface.DeviceManager.GetDeviceModels(equipmentModel, new DeviceModelTags?(DeviceModelTags.SystemDevice));
        if (this.SelectedSystem != null)
        {
          this.ProfileTypes = (IEnumerable<ProfileType>) GmmInterface.DeviceManager.GetProfileTypes(this.SelectedSystem, equipmentModel, new ProfileTypeTags?(ProfileTypeTags.JobManager));
          this.SelectedSystem = (DeviceModel) null;
        }
        this.OnPropertyChanged(nameof (SelectedEquipmentName));
      }
    }

    public bool IsVisible
    {
      get => this._isVisible;
      set
      {
        this._isVisible = value;
        this.OnPropertyChanged(nameof (IsVisible));
      }
    }

    public string EquipmentParams
    {
      get => this._equipmentParams;
      set
      {
        this._equipmentParams = value;
        this.OnPropertyChanged(nameof (EquipmentParams));
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

    [Required(ErrorMessage = "MSS_CLIENT_CREATE_JOB_PROFILE_TYPE_REQUIRED")]
    public ProfileType ProfileType
    {
      get => this._profileType;
      set
      {
        this._profileType = value;
        this.OnPropertyChanged(nameof (ProfileType));
      }
    }

    public IEnumerable<ProfileType> ProfileTypes
    {
      get => this._profileTypes;
      set
      {
        this._profileTypes = value;
        this.OnPropertyChanged(nameof (ProfileTypes));
      }
    }

    public RadObservableCollection<Rules> FilterRules
    {
      get => this._rules;
      set
      {
        this._rules = value;
        this.OnPropertyChanged(nameof (FilterRules));
      }
    }

    public MSS.Core.Model.DataFilters.Filter SelectedFilter
    {
      get => this._selectedFilter;
      set
      {
        this._selectedFilter = value;
        this.OnPropertyChanged(nameof (SelectedFilter));
      }
    }

    [Required(ErrorMessage = "MSS_CLIENT_CREATE_JOB_REQUIRED_NAME")]
    public string Name
    {
      get => this._name;
      set
      {
        this._name = value;
        this.OnPropertyChanged(nameof (Name));
      }
    }

    public string Title
    {
      get => this._title;
      set
      {
        this._title = value;
        this.OnPropertyChanged(nameof (Title));
      }
    }

    public IEnumerable<MSS.Core.Model.DataFilters.Filter> FilterCollection
    {
      get => this._filterCollection;
      set
      {
        this._filterCollection = value;
        this.OnPropertyChanged(nameof (FilterCollection));
      }
    }

    public bool ServiceJobVisibility
    {
      get => this._serviceJobVisibility;
      set
      {
        this._serviceJobVisibility = value;
        this.OnPropertyChanged(nameof (ServiceJobVisibility));
      }
    }
  }
}
