// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.JobsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using Common.Library.NHibernate.Data;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Meters;
using MSS.DIConfiguration;
using MSS.DTO.Jobs;
using MSS.DTO.MessageHandler;
using MSS.DTO.Reporting;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class JobsViewModel : ValidationViewModelBase
  {
    private readonly IWindowFactory _windowFactory;
    private bool _canViewDefinitionJobs;
    private bool _canViewServiceJobs;
    private bool _canDeleteJobDefinitions;
    private bool _canDeleteServiceJobs;
    private ViewModelBase _messageUserControlScenarios;
    private ViewModelBase _messageUserControlJobDefinitions;
    private ViewModelBase _messageUserControlJobs;
    private string _jobEntityNumberValue = string.Empty;
    private IEnumerable<JobLogsDTO> _jobLogs = (IEnumerable<JobLogsDTO>) new RadObservableCollection<JobLogsDTO>();
    private DateTime? _endDateLogValue;
    private DateTime? _startDateJobLogValue;
    private readonly IRepositoryFactory _repositoryFactory;
    private IEnumerable<ScenarioDTO> _getScenarios;
    private DateTime? _startDateLogValue;
    private DateTime? _endDateJobLogValue;
    private bool _canAddServiceJobs;
    public RadObservableCollection<MssReadingJobDto> _jobCollection;
    public RadObservableCollection<JobDefinitionDto> _jobDefinitions;
    private JobDefinitionDto _selectedJobDefinition;
    private string _pageSize;
    private bool _isJobsTabSelected;
    private bool _isJobLogsTabSelected;
    private bool _isJobDefinitionsTabSelected;
    private bool _isScenariosTabSelected;
    private int _selectedIndex;
    private ApplicationTabsEnum _selectedTab;
    private bool _createJobDefinitionVisibility;
    private bool _editJobDefinitionVisibility;
    private bool _deleteJobDefinitionVisibility;
    private bool _isDeleteJobDefinitionEnabled;
    private bool _createJobVisibility;
    private bool _deleteJobVisibility;
    private bool _editScenarioVisibility;

    [Inject]
    public JobsViewModel(IRepositoryFactory repositoryFactory, IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._repositoryFactory.GetRepository<MSS.Core.Model.Reporting.JobLogs>();
      this._windowFactory = windowFactory;
      this._getScenarios = this.GetJobsManagerInstance().GetScenarioDTOs();
      this._pageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this.StartDateJobLogValue = new DateTime?(DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)));
      this.EndDateJobLogValue = new DateTime?(DateTime.Now.AddDays(3.0));
      Mapper.CreateMap<MSS.Core.Model.Reporting.JobLogs, JobLogsDTO>().ForMember((Expression<Func<JobLogsDTO, object>>) (j => j.JobName), (Action<IMemberConfigurationExpression<MSS.Core.Model.Reporting.JobLogs>>) (j => j.ResolveUsing((Func<MSS.Core.Model.Reporting.JobLogs, object>) (jl => (object) jl.Job.JobDefinition.Name))));
      EventPublisher.Register<SaveJobDefinitionEvent>(new Action<SaveJobDefinitionEvent>(this.SaveJobDefinition));
      EventPublisher.Register<RemoveJobDefinitionEvent>(new Action<RemoveJobDefinitionEvent>(this.RefreshJobDefinitionGrid));
      EventPublisher.Register<RefreshGridEvent>(new Action<RefreshGridEvent>(this.RefreshJobsGrid));
      EventPublisher.Register<RefreshJobGrid>(new Action<RefreshJobGrid>(this.UpdateRow));
      EventPublisher.Register<RemoveJobEvent>(new Action<RemoveJobEvent>(this.RemoveJobEv));
      EventPublisher.Register<SelectedTabChanged>((Action<SelectedTabChanged>) (changed => this.SelectedTab = changed.SelectedTab));
      EventPublisher.Register<SelectedTabValue>(new Action<SelectedTabValue>(this.SetTab));
      UsersManager usersManager1 = new UsersManager(this._repositoryFactory);
      this.IsJobsTabVisible = usersManager1.HasRight(OperationEnum.JobView.ToString());
      this.IsJobLogsTabVisible = usersManager1.HasRight(OperationEnum.JobLogsView.ToString());
      this._canViewDefinitionJobs = usersManager1.HasRight(OperationEnum.JobDefinitionView.ToString());
      this._canViewServiceJobs = usersManager1.HasRight(OperationEnum.ServiceJobView.ToString());
      this.IsJobDefinitionsTabVisible = this._canViewDefinitionJobs || this._canViewServiceJobs;
      this.IsScenariosTabVisible = usersManager1.HasRight(OperationEnum.ScenarioView.ToString());
      if (this.IsJobsTabVisible)
        this.SelectedIndex = 0;
      else if (this.IsJobLogsTabVisible)
        this.SelectedIndex = 1;
      else if (this.IsJobDefinitionsTabVisible)
        this.SelectedIndex = 2;
      else if (this.IsScenariosTabVisible)
        this.SelectedIndex = 3;
      this.CreateJobDefinitionVisibility = usersManager1.HasRight(OperationEnum.JobDefinitionCreate.ToString()) || usersManager1.HasRight(OperationEnum.ServiceJobCreate.ToString());
      this.EditJobDefinitionVisibility = usersManager1.HasRight(OperationEnum.JobDefinitionEdit.ToString());
      UsersManager usersManager2 = usersManager1;
      OperationEnum operationEnum = OperationEnum.JobDelete;
      string operation1 = operationEnum.ToString();
      this._canDeleteServiceJobs = usersManager2.HasRight(operation1);
      UsersManager usersManager3 = usersManager1;
      operationEnum = OperationEnum.JobDefinitionDelete;
      string operation2 = operationEnum.ToString();
      this._canDeleteJobDefinitions = usersManager3.HasRight(operation2);
      this.DeleteJobDefinitionVisibility = this._canDeleteServiceJobs || this._canDeleteJobDefinitions;
      UsersManager usersManager4 = usersManager1;
      operationEnum = OperationEnum.JobCreate;
      string operation3 = operationEnum.ToString();
      this.CreateJobVisibility = usersManager4.HasRight(operation3);
      UsersManager usersManager5 = usersManager1;
      operationEnum = OperationEnum.ServiceJobDelete;
      string operation4 = operationEnum.ToString();
      this.DeleteJobVisibility = usersManager5.HasRight(operation4);
      UsersManager usersManager6 = usersManager1;
      operationEnum = OperationEnum.ScenarioEdit;
      string operation5 = operationEnum.ToString();
      this.EditScenarioVisibility = usersManager6.HasRight(operation5);
    }

    public ViewModelBase MessageUserControlScenarios
    {
      get => this._messageUserControlScenarios;
      set
      {
        this._messageUserControlScenarios = value;
        this.OnPropertyChanged(nameof (MessageUserControlScenarios));
      }
    }

    public ViewModelBase MessageUserControlJobDefinitions
    {
      get => this._messageUserControlJobDefinitions;
      set
      {
        this._messageUserControlJobDefinitions = value;
        this.OnPropertyChanged(nameof (MessageUserControlJobDefinitions));
      }
    }

    public ViewModelBase MessageUserControlJobs
    {
      get => this._messageUserControlJobs;
      set
      {
        this._messageUserControlJobs = value;
        this.OnPropertyChanged(nameof (MessageUserControlJobs));
      }
    }

    public string JobEntityNumberValue
    {
      get => this._jobEntityNumberValue;
      set
      {
        this._jobEntityNumberValue = value;
        this.OnPropertyChanged(nameof (JobEntityNumberValue));
      }
    }

    public IEnumerable<JobLogsDTO> JobLogs
    {
      get => this._jobLogs;
      set
      {
        this._jobLogs = value;
        this.OnPropertyChanged(nameof (JobLogs));
      }
    }

    public DateTime? EndDateLogValue
    {
      get => this._endDateLogValue;
      set
      {
        this._endDateLogValue = value;
        this.ValidateProperty("MasterNumberValue");
        this.OnPropertyChanged(nameof (EndDateLogValue));
      }
    }

    public DateTime? StartDateJobLogValue
    {
      get => this._startDateJobLogValue;
      set
      {
        this._startDateJobLogValue = value;
        this.ValidateProperty("JobEntityNumberValue");
        this.OnPropertyChanged(nameof (StartDateJobLogValue));
      }
    }

    public IEnumerable<ScenarioDTO> GetScenarios => this._getScenarios;

    public DateTime? StartDateLogValue
    {
      get => this._startDateLogValue;
      set
      {
        this._startDateLogValue = value;
        this.ValidateProperty("MasterNumberValue");
        this.OnPropertyChanged(nameof (StartDateLogValue));
      }
    }

    public DateTime? EndDateJobLogValue
    {
      get => this._endDateJobLogValue;
      set
      {
        this._endDateJobLogValue = value;
        this.ValidateProperty("JobEntityNumberValue");
        this.OnPropertyChanged(nameof (EndDateJobLogValue));
      }
    }

    public bool CanAddServiceJobs
    {
      get => this._canAddServiceJobs;
      set
      {
        this._canAddServiceJobs = value;
        this.OnPropertyChanged(nameof (CanAddServiceJobs));
      }
    }

    private void RemoveJobEv(RemoveJobEvent ev)
    {
      MssReadingJob byId = this._repositoryFactory.GetRepository<MssReadingJob>().GetById((object) ev.Job.Id);
      byId.IsDeactivated = true;
      byId.EndDate = new DateTime?(DateTime.Now);
      this._repositoryFactory.GetRepository<MssReadingJob>().Update(byId);
      this.OnPropertyChanged("JobCollection");
    }

    private void UpdateRow(RefreshJobGrid ev)
    {
      this._repositoryFactory.GetRepository<MssReadingJob>().Refresh((object) ev.Job.Id);
      this.OnPropertyChanged("JobCollection");
    }

    private void RefreshJobsGrid(RefreshGridEvent ev)
    {
      this._repositoryFactory.GetSession().Clear();
      this.OnPropertyChanged("JobCollection");
    }

    private void RefreshJobDefinitionGrid(RemoveJobDefinitionEvent removeJobDefinitionEvent)
    {
      ISession session = this._repositoryFactory.GetSession();
      try
      {
        session.BeginTransaction();
        JobDefinition jobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) removeJobDefinitionEvent.JobDefinition.Id);
        jobDefinition.IsDeactivated = true;
        jobDefinition.EndDate = new DateTime?(DateTime.Now);
        this._repositoryFactory.GetRepository<JobDefinition>().TransactionalUpdate(jobDefinition);
        if (jobDefinition.ScenarioJobDefinitions != null && jobDefinition.ScenarioJobDefinitions.Count > 0)
        {
          IRepository<ScenarioJobDefinition> repository = this._repositoryFactory.GetRepository<ScenarioJobDefinition>();
          TypeHelperExtensionMethods.ForEach<ScenarioJobDefinition>((IEnumerable<ScenarioJobDefinition>) jobDefinition.ScenarioJobDefinitions, new Action<ScenarioJobDefinition>(repository.TransactionalDelete));
        }
        IRepository<MssReadingJob> jobsRepository = this._repositoryFactory.GetRepository<MssReadingJob>();
        IList<MssReadingJob> mssReadingJobList = jobsRepository.SearchFor((Expression<Func<MssReadingJob, bool>>) (j => j.JobDefinition.Id == jobDefinition.Id && !j.IsDeactivated));
        if (mssReadingJobList != null && mssReadingJobList.Count > 0)
          TypeHelperExtensionMethods.ForEach<MssReadingJob>((IEnumerable<MssReadingJob>) mssReadingJobList, (Action<MssReadingJob>) (j =>
          {
            j.IsDeactivated = true;
            jobsRepository.TransactionalUpdate(j);
          }));
        session.Transaction.Commit();
        session.Clear();
        this.OnPropertyChanged("JobCollection");
        this.OnPropertyChanged("JobDefinitions");
      }
      catch (Exception ex)
      {
        if (session.IsOpen && session.Transaction.IsActive)
          session.Transaction.Rollback();
        throw;
      }
    }

    private void SaveJobDefinition(SaveJobDefinitionEvent jobDefinitionObject)
    {
      bool flag = false;
      IRepository<JobDefinition> repository = this._repositoryFactory.GetRepository<JobDefinition>();
      JobDefinition entity = repository.FirstOrDefault((Expression<Func<JobDefinition, bool>>) (x => x.Id == jobDefinitionObject.JobDefinition.Id));
      if (entity != null)
        flag = true;
      else
        entity = new JobDefinition();
      entity.EquipmentModel = jobDefinitionObject.JobDefinition.EquipmentModel;
      entity.System = jobDefinitionObject.JobDefinition.System;
      entity.Filter = jobDefinitionObject.JobDefinition.Filter;
      entity.Name = jobDefinitionObject.JobDefinition.Name;
      entity.StartDate = new DateTime?(DateTime.Now);
      entity.IsDeactivated = false;
      entity.Interval = jobDefinitionObject.JobDefinition.Interval;
      entity.EquipmentParams = jobDefinitionObject.JobDefinition.EquipmentParams;
      entity.ProfileType = jobDefinitionObject.JobDefinition.ProfileType;
      entity.ServiceJob = jobDefinitionObject.JobDefinition.ServiceJob;
      entity.QuarterHour = jobDefinitionObject.JobDefinition.QuarterHour;
      entity.Day = jobDefinitionObject.JobDefinition.Day;
      entity.Month = jobDefinitionObject.JobDefinition.Month;
      entity.DueDate = jobDefinitionObject.JobDefinition.DueDate;
      entity.ProfileTypeParams = jobDefinitionObject.JobDefinition.ProfileTypeParams;
      repository.Update(entity);
      if (flag)
        GMMJobsManager.Instance(DIConfigurator.GetConfigurator().Get<IRepositoryFactoryCreator>(), false).UpdateJobsRelatedToJobDefinition(entity.Id);
      this.OnPropertyChanged("JobDefinitions");
    }

    private JobsManager GetJobsManagerInstance() => new JobsManager(this._repositoryFactory);

    public ICommand SearchJobLogs
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          if (!this.IsValid)
            return;
          GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.JOBS_LOAD_LOGS), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.MSS_CLIENT_ARCHIVE_MESSAGE));
          BackgroundWorker backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          backgroundWorker.DoWork += (DoWorkEventHandler) delegate
          {
            this.JobLogs = (IEnumerable<JobLogsDTO>) new JobLogsManager(this._repositoryFactory).LoadJobLogs(this.JobEntityNumberValue, this.StartDateJobLogValue, this.EndDateJobLogValue);
          };
          backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            pd.OnRequestClose(false);
            MSS.DTO.Message.Message message = (MSS.DTO.Message.Message) null;
            if (args.Cancelled)
              message = new MSS.DTO.Message.Message()
              {
                MessageType = MessageTypeEnum.Warning,
                MessageText = Resources.MSS_Client_Archivation_Cancelled
              };
            else if (args.Error != null)
            {
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
            }
            else
            {
              if (!this.JobLogs.Any<JobLogsDTO>())
                MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.LOGS_NOVALUES_FOUND, false);
              message = new MSS.DTO.Message.Message()
              {
                MessageType = MessageTypeEnum.Success,
                MessageText = Resources.MSS_Client_Archivation_Succedded
              };
            }
            if (message == null)
              return;
            EventPublisher.Publish<ActionFinished>(new ActionFinished()
            {
              Message = message
            }, (IViewModel) this);
          });
          backgroundWorker.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
        });
      }
    }

    public ICommand EditScenarioCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ScenarioDTO scenarioDto = parameter as ScenarioDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditScenarioViewModel>((IParameter) new ConstructorArgument("sm", (object) scenarioDto)));
          this.MessageUserControlScenarios = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage) : MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          this._getScenarios = this.GetJobsManagerInstance().GetScenarioDTOs();
          this.OnPropertyChanged("GetScenarios");
        }));
      }
    }

    public ICommand StartJob
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (selectedJob =>
        {
          GMMJobsManager gmmJobsManager = GMMJobsManager.Instance(DIConfigurator.GetConfigurator().Get<IRepositoryFactoryCreator>(), false);
          List<Meter> meters = (List<Meter>) null;
          if (selectedJob != null)
          {
            MssReadingJobDto mssReadingJobDto = (MssReadingJobDto) selectedJob;
            mssReadingJobDto.Status = JobStatusEnum.Active.ToString();
            MssReadingJob byId = this._repositoryFactory.GetRepository<MssReadingJob>().GetById((object) mssReadingJobDto.Id);
            byId.Status = JobStatusEnum.Active;
            HibernateMultipleDatabasesManager.Update((object) byId, this._repositoryFactory.GetSession());
            Guid rootNodeId = new Guid();
            if (byId.RootNode != null)
            {
              Structure structure = new StructuresManager(this._repositoryFactory).LoadStructure(byId.RootNode.Id);
              meters = structure.Meters;
              rootNodeId = structure.RootNodeId;
            }
            try
            {
              gmmJobsManager.AddJob(byId, meters, rootNodeId);
            }
            catch (Exception ex)
            {
              GMMJobsLogger.LogJobError(ex);
              MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), ex.Message, false);
            }
          }
          this.OnPropertyChanged("JobCollection");
        }));
      }
    }

    public ICommand StopJob
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (selectedJob =>
        {
          GMMJobsManager gmmJobsManager = GMMJobsManager.Instance(DIConfigurator.GetConfigurator().Get<IRepositoryFactoryCreator>(), false);
          if (selectedJob != null)
          {
            MssReadingJobDto mssReadingJobDto = (MssReadingJobDto) selectedJob;
            mssReadingJobDto.Status = JobStatusEnum.Inactive.ToString();
            MssReadingJob byId = this._repositoryFactory.GetRepository<MssReadingJob>().GetById((object) mssReadingJobDto.Id);
            byId.Status = JobStatusEnum.Inactive;
            HibernateMultipleDatabasesManager.Update((object) byId, this._repositoryFactory.GetSession());
            gmmJobsManager.RemoveJob(byId);
          }
          this.OnPropertyChanged("JobCollection");
        }));
      }
    }

    public ICommand ViewJobStructure
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ViewJobStructureViewModel>((IParameter) new ConstructorArgument("selectedJob", (object) (parameter as MssReadingJobDto))))));
      }
    }

    public ICommand ShowJobLogsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (selectedJob => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<JobLogsForJobViewModel>((IParameter) new ConstructorArgument("jobDto", (object) (selectedJob as MssReadingJobDto))))));
      }
    }

    public ICommand AddJob
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AddMssReadingJobViewModel>());
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControlJobs = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControlJobs = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand EditJob
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AddMssReadingJobViewModel>((IParameter) new ConstructorArgument("jobDto", parameter)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControlJobs = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControlJobs = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand RemoveJob
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSS_Client.ViewModel.Jobs.RemoveJob>((IParameter) new ConstructorArgument("job", parameter)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControlJobs = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControlJobs = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand AddJobDefinition
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AddJobDefinitionViewModel>());
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControlJobDefinitions = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControlJobDefinitions = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand EditJobDefinition
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AddJobDefinitionViewModel>((IParameter) new ConstructorArgument("jdDto", parameter)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControlJobDefinitions = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControlJobDefinitions = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand RemoveJobDefinition
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MSS_Client.ViewModel.Jobs.RemoveJobDefinition>((IParameter) new ConstructorArgument("jobDefinition", parameter)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControlJobDefinitions = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControlJobDefinitions = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public RadObservableCollection<MssReadingJobDto> JobCollection
    {
      get => new JobsManager(this._repositoryFactory).GetMssReadingJobsDto();
      set => this._jobCollection = value;
    }

    public RadObservableCollection<JobDefinitionDto> JobDefinitions
    {
      get
      {
        return new JobsManager(this._repositoryFactory).GetJobDefinitionDto(this._canViewServiceJobs, this._canViewDefinitionJobs);
      }
      set => this._jobDefinitions = value;
    }

    public JobDefinitionDto SelectedJobDefinition
    {
      get => this._selectedJobDefinition;
      set
      {
        this._selectedJobDefinition = value;
        this.OnPropertyChanged(nameof (SelectedJobDefinition));
        this.IsDeleteJobDefinitionEnabled = !string.IsNullOrWhiteSpace(this._selectedJobDefinition?.FilterName) ? this._canDeleteJobDefinitions : this._canDeleteServiceJobs;
      }
    }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
      }
    }

    public bool IsJobsTabSelected
    {
      get => this._isJobsTabSelected;
      set
      {
        this._isJobsTabSelected = value;
        if (!this._isJobsTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.Job
        }, (IViewModel) this);
      }
    }

    public bool IsJobLogsTabSelected
    {
      get => this._isJobLogsTabSelected;
      set
      {
        this._isJobLogsTabSelected = value;
        if (!this._isJobLogsTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.JobLogs
        }, (IViewModel) this);
      }
    }

    public bool IsJobDefinitionsTabSelected
    {
      get => this._isJobDefinitionsTabSelected;
      set
      {
        this._isJobDefinitionsTabSelected = value;
        if (!this._isJobDefinitionsTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.JobDefinitions
        }, (IViewModel) this);
      }
    }

    public bool IsScenariosTabSelected
    {
      get => this._isScenariosTabSelected;
      set
      {
        this._isScenariosTabSelected = value;
        if (!this._isScenariosTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.Scenarios
        }, (IViewModel) this);
      }
    }

    public bool IsJobsTabVisible { get; set; }

    public bool IsJobLogsTabVisible { get; set; }

    public bool IsJobDefinitionsTabVisible { get; set; }

    public bool IsScenariosTabVisible { get; set; }

    public int SelectedIndex
    {
      get => this._selectedIndex;
      set
      {
        this._selectedIndex = value;
        this.OnPropertyChanged(nameof (SelectedIndex));
      }
    }

    private void SetTab(SelectedTabValue selectedTabValue)
    {
      switch (selectedTabValue.Tab)
      {
        case ApplicationTabsEnum.Job:
          this.SelectedIndex = 0;
          break;
        case ApplicationTabsEnum.JobLogs:
          this.SelectedIndex = 1;
          break;
        case ApplicationTabsEnum.JobDefinitions:
          this.SelectedIndex = 2;
          break;
        case ApplicationTabsEnum.Scenarios:
          this.SelectedIndex = 3;
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

    private void SetSelectedTab()
    {
      switch (this.SelectedTab)
      {
        case ApplicationTabsEnum.Job:
          this._isJobsTabSelected = true;
          break;
        case ApplicationTabsEnum.JobLogs:
          this._isJobLogsTabSelected = true;
          break;
        case ApplicationTabsEnum.JobDefinitions:
          this._isJobDefinitionsTabSelected = true;
          break;
        case ApplicationTabsEnum.Scenarios:
          this._isScenariosTabSelected = true;
          break;
      }
    }

    public bool CreateJobDefinitionVisibility
    {
      get => this._createJobDefinitionVisibility;
      set
      {
        this._createJobDefinitionVisibility = value;
        this.OnPropertyChanged(nameof (CreateJobDefinitionVisibility));
      }
    }

    public bool EditJobDefinitionVisibility
    {
      get => this._editJobDefinitionVisibility;
      set
      {
        this._editJobDefinitionVisibility = value;
        this.OnPropertyChanged(nameof (EditJobDefinitionVisibility));
      }
    }

    public bool DeleteJobDefinitionVisibility
    {
      get => this._deleteJobDefinitionVisibility;
      set
      {
        this._deleteJobDefinitionVisibility = value;
        this.OnPropertyChanged(nameof (DeleteJobDefinitionVisibility));
      }
    }

    public bool IsDeleteJobDefinitionEnabled
    {
      get => this._isDeleteJobDefinitionEnabled;
      set
      {
        this._isDeleteJobDefinitionEnabled = value;
        this.OnPropertyChanged(nameof (IsDeleteJobDefinitionEnabled));
      }
    }

    public bool CreateJobVisibility
    {
      get => this._createJobVisibility;
      set
      {
        this._createJobVisibility = value;
        this.OnPropertyChanged(nameof (CreateJobVisibility));
      }
    }

    public bool DeleteJobVisibility
    {
      get => this._deleteJobVisibility;
      set
      {
        this._deleteJobVisibility = value;
        this.OnPropertyChanged(nameof (DeleteJobVisibility));
      }
    }

    public bool EditScenarioVisibility
    {
      get => this._editScenarioVisibility;
      set
      {
        this._editScenarioVisibility = value;
        this.OnPropertyChanged(nameof (EditScenarioVisibility));
      }
    }
  }
}
