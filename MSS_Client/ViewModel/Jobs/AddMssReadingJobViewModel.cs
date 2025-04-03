// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Jobs.AddMssReadingJobViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.Events;
using MSS.Business.Modules.JobsManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Jobs;
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.Jobs;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Telerik.Windows.Data;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Jobs
{
  public class AddMssReadingJobViewModel : ValidationViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly bool _isUpdate;
    private readonly MssReadingJobDto _job = new MssReadingJobDto();
    private ViewModelBase _messageUserControl;
    private bool _isMBusJob;
    private bool _isMinomatJob;
    private string _title;
    private string _structureRootInfo;
    private Guid _structureRootId;
    private IEnumerable<ServiceTask> _serviceJobs = (IEnumerable<ServiceTask>) new RadObservableCollection<ServiceTask>();
    private RadObservableCollection<JobDefinitionDto> _jobList = new RadObservableCollection<JobDefinitionDto>();
    private JobDefinitionDto _selectedJobDefinition;
    private JobDefinitionDto _selectedMinomatJobDefinition;
    private IEnumerable<string> _serialNumberList;
    private string _serialNumber;

    public AddMssReadingJobViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._isUpdate = false;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._jobList = new JobsManager(this._repositoryFactory).GetJobDefinitionDto();
      this.Title = Resources.MSS_CLIENT_CREATE_JOB_TITLE;
      EventPublisher.Register<AssignMinomatEvent>(new Action<AssignMinomatEvent>(this.WriteRadioId));
      EventPublisher.Register<AssignStructureEvent>(new Action<AssignStructureEvent>(this.AssignStructure));
      this.CurrentMinomats = (IEnumerable<Minomat>) this._repositoryFactory.GetRepository<Minomat>().Where((Expression<Func<Minomat, bool>>) (x => x.IsDeactivated == false && x.IsMaster));
      this.IsMBusJob = true;
      this.SerialNumberList = (IEnumerable<string>) this._repositoryFactory.GetRepository<Minomat>().Where((Expression<Func<Minomat, bool>>) (x => x.EndDate == new DateTime?() && x.IsMaster)).OrderBy<Minomat, string>((Expression<Func<Minomat, string>>) (x => x.RadioId)).Select<Minomat, string>((Expression<Func<Minomat, string>>) (x => x.RadioId));
    }

    private void AssignStructure(AssignStructureEvent ev)
    {
      this.StructureRootInfo = ev.StructureNode.Name + "\n" + ev.StructureNode.Description;
      this.StructureRootId = ev.StructureNode.Id;
    }

    private void WriteRadioId(AssignMinomatEvent ev) => this.SerialNumber = ev.Minomat.RadioId;

    public AddMssReadingJobViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory,
      MssReadingJobDto jobDto)
    {
      this._job = jobDto;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._jobList = new JobsManager(this._repositoryFactory).GetJobDefinitionDto();
      this.CurrentMinomats = (IEnumerable<Minomat>) this._repositoryFactory.GetRepository<Minomat>().Where((Expression<Func<Minomat, bool>>) (x => x.IsDeactivated == false && x.IsMaster));
      EventPublisher.Register<AssignMinomatEvent>(new Action<AssignMinomatEvent>(this.WriteRadioId));
      EventPublisher.Register<AssignStructureEvent>(new Action<AssignStructureEvent>(this.AssignStructure));
      this.Title = Resources.MSS_CLIENT_UPDATE_JOB_TITLE;
      this.SerialNumberList = (IEnumerable<string>) this._repositoryFactory.GetRepository<Minomat>().Where((Expression<Func<Minomat, bool>>) (x => x.EndDate == new DateTime?() && x.IsMaster)).OrderBy<Minomat, string>((Expression<Func<Minomat, string>>) (x => x.RadioId)).Select<Minomat, string>((Expression<Func<Minomat, string>>) (x => x.RadioId));
      this._isUpdate = true;
      if (jobDto.MinomatId == Guid.Empty)
      {
        this.IsMBusJob = true;
        this.IsMinomatJob = false;
      }
      else
      {
        this.IsMBusJob = false;
        this.IsMinomatJob = true;
      }
      MssReadingJob job = this._repositoryFactory.GetRepository<MssReadingJob>().FirstOrDefault((Expression<Func<MssReadingJob, bool>>) (x => x.Id == jobDto.Id));
      if (job == null)
        return;
      if (this.IsMBusJob)
      {
        this.SelectedJobDefinition = this._jobList.FirstOrDefault<JobDefinitionDto>((Func<JobDefinitionDto, bool>) (x => x.Id == job.JobDefinition.Id));
        StructureNode byId = this._repositoryFactory.GetRepository<StructureNode>().GetById((object) jobDto.StructureNodeId);
        if (byId != null)
        {
          this.StructureRootInfo = byId.Name + "\n" + byId.Description;
          this.StructureRootId = byId.Id;
        }
      }
      else
      {
        this.SerialNumber = this._repositoryFactory.GetRepository<Minomat>().GetById((object) jobDto.MinomatId).RadioId;
        this.SelectedMinomatJobDefinition = this._jobList.FirstOrDefault<JobDefinitionDto>((Func<JobDefinitionDto, bool>) (x => x.Id == job.JobDefinition.Id));
      }
    }

    public bool IsMBusJob
    {
      get => this._isMBusJob;
      set
      {
        this._isMBusJob = value;
        this.OnPropertyChanged(nameof (IsMBusJob));
      }
    }

    public bool IsMinomatJob
    {
      get => this._isMinomatJob;
      set
      {
        this._isMinomatJob = value;
        this.OnPropertyChanged(nameof (IsMinomatJob));
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

    [Required(ErrorMessage = "MSS_JOBS_STRUCTURE_NAME_REQUIRED")]
    public string StructureRootInfo
    {
      get => this._structureRootInfo;
      set
      {
        this._structureRootInfo = value;
        this.OnPropertyChanged(nameof (StructureRootInfo));
      }
    }

    public Guid StructureRootId
    {
      get => this._structureRootId;
      set
      {
        this._structureRootId = value;
        this.OnPropertyChanged(nameof (StructureRootId));
      }
    }

    public IEnumerable<Minomat> CurrentMinomats { get; set; }

    [Required(ErrorMessage = "MSS_JOB_JOBDEFINITION_REQUIRED")]
    public IEnumerable<ServiceTask> ServiceJobs
    {
      get => this._serviceJobs;
      set
      {
        this._serviceJobs = value;
        this.OnPropertyChanged(nameof (ServiceJobs));
      }
    }

    public RadObservableCollection<JobDefinitionDto> JobDefinitions
    {
      get => this._jobList;
      set
      {
        this._jobList = value;
        this.OnPropertyChanged("SelectedJobDefinition");
      }
    }

    [Required(ErrorMessage = "MSS_JOB_JOBDEFINITION_REQUIRED")]
    public JobDefinitionDto SelectedJobDefinition
    {
      get => this._selectedJobDefinition;
      set
      {
        this._selectedJobDefinition = value;
        this.OnPropertyChanged(nameof (SelectedJobDefinition));
      }
    }

    [Required(ErrorMessage = "MSS_JOB_JOBDEFINITION_REQUIRED")]
    public JobDefinitionDto SelectedMinomatJobDefinition
    {
      get => this._selectedMinomatJobDefinition;
      set
      {
        this._selectedMinomatJobDefinition = value;
        this.OnPropertyChanged(nameof (SelectedMinomatJobDefinition));
      }
    }

    public IEnumerable<string> SerialNumberList
    {
      get => this._serialNumberList;
      set
      {
        this._serialNumberList = value;
        this.OnPropertyChanged(nameof (SerialNumberList));
      }
    }

    [Required(ErrorMessage = "MSS_JOB_SERIAL_NUMBER_REQUIRED")]
    public string SerialNumber
    {
      get => this._serialNumber;
      set
      {
        this._serialNumber = value;
        this.OnPropertyChanged(nameof (SerialNumber));
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

    public ICommand UpdateJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this.RefreshIsValid();
          if (!this.IsValid)
            return;
          if (!this._isUpdate)
          {
            if (this.IsMBusJob)
            {
              JobDefinition jobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) this.SelectedJobDefinition.Id);
              if (jobDefinition != null)
              {
                Scenario scenario = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (x => x.JobDefinition.Id == jobDefinition.Id)).Select<ScenarioJobDefinition, Scenario>((Func<ScenarioJobDefinition, Scenario>) (x => x.Scenario)).FirstOrDefault<Scenario>();
                MssReadingJob entity = new MssReadingJob()
                {
                  RootNode = this._repositoryFactory.GetRepository<StructureNode>().GetById((object) this.StructureRootId),
                  StartDate = new DateTime?(DateTime.Now),
                  IsDeactivated = false,
                  JobDefinition = jobDefinition,
                  Scenario = scenario,
                  IsUpdate = true,
                  LastExecutionDate = new DateTime?(),
                  Status = JobStatusEnum.Isnew,
                  CreatedOn = DateTime.Today,
                  LastUpdatedOn = new DateTime?(DateTime.Today)
                };
                this._repositoryFactory.GetRepository<MssReadingJob>().Update(entity);
                EventPublisher.Publish<RefreshGridEvent>(new RefreshGridEvent(), (IViewModel) this);
              }
            }
            if (this.IsMinomatJob)
            {
              JobDefinition jobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) this.SelectedMinomatJobDefinition.Id);
              if (jobDefinition != null)
              {
                Scenario scenario = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (x => x.JobDefinition.Id == jobDefinition.Id)).Select<ScenarioJobDefinition, Scenario>((Func<ScenarioJobDefinition, Scenario>) (x => x.Scenario)).FirstOrDefault<Scenario>();
                MssReadingJob entity = new MssReadingJob()
                {
                  Minomat = this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (x => x.RadioId == this.SerialNumber)),
                  StartDate = new DateTime?(DateTime.Now),
                  IsDeactivated = false,
                  JobDefinition = jobDefinition,
                  Scenario = scenario,
                  IsUpdate = true,
                  Status = JobStatusEnum.Isnew,
                  CreatedOn = DateTime.Today,
                  LastUpdatedOn = new DateTime?(DateTime.Today)
                };
                this._repositoryFactory.GetRepository<MssReadingJob>().Update(entity);
                EventPublisher.Publish<RefreshGridEvent>(new RefreshGridEvent(), (IViewModel) this);
              }
            }
            this.OnRequestClose(true);
          }
          else
          {
            if (this.IsMBusJob)
            {
              JobDefinition jobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) this.SelectedJobDefinition.Id);
              Scenario scenario = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (x => x.JobDefinition.Id == jobDefinition.Id)).Select<ScenarioJobDefinition, Scenario>((Func<ScenarioJobDefinition, Scenario>) (x => x.Scenario)).FirstOrDefault<Scenario>();
              if (jobDefinition != null)
              {
                MssReadingJob byId = this._repositoryFactory.GetRepository<MssReadingJob>().GetById((object) this._job.Id);
                byId.StartDate = new DateTime?(DateTime.Now);
                byId.IsDeactivated = false;
                byId.JobDefinition = jobDefinition;
                byId.RootNode = this._repositoryFactory.GetRepository<StructureNode>().GetById((object) this.StructureRootId);
                byId.Scenario = scenario;
                byId.Minomat = (Minomat) null;
                this._repositoryFactory.GetRepository<MssReadingJob>().Update(byId);
                EventPublisher.Publish<RefreshJobGrid>(new RefreshJobGrid()
                {
                  Job = byId
                }, (IViewModel) this);
              }
            }
            if (this.IsMinomatJob)
            {
              JobDefinition jobDefinition = this._repositoryFactory.GetRepository<JobDefinition>().GetById((object) this.SelectedMinomatJobDefinition.Id);
              if (jobDefinition != null)
              {
                Scenario scenario = this._repositoryFactory.GetRepository<ScenarioJobDefinition>().SearchFor((Expression<Func<ScenarioJobDefinition, bool>>) (x => x.JobDefinition.Id == jobDefinition.Id)).Select<ScenarioJobDefinition, Scenario>((Func<ScenarioJobDefinition, Scenario>) (x => x.Scenario)).FirstOrDefault<Scenario>();
                MssReadingJob byId = this._repositoryFactory.GetRepository<MssReadingJob>().GetById((object) this._job.Id);
                byId.StartDate = new DateTime?(DateTime.Now);
                byId.IsDeactivated = false;
                byId.JobDefinition = jobDefinition;
                byId.Scenario = scenario;
                byId.RootNode = (StructureNode) null;
                byId.Minomat = this._repositoryFactory.GetRepository<Minomat>().FirstOrDefault((Expression<Func<Minomat, bool>>) (x => x.RadioId == this.SerialNumber));
                byId.IsUpdate = true;
                this._repositoryFactory.GetRepository<MssReadingJob>().Update(byId);
                EventPublisher.Publish<RefreshJobGrid>(new RefreshJobGrid()
                {
                  Job = byId
                }, (IViewModel) this);
              }
            }
            this.OnRequestClose(true);
          }
        });
      }
    }

    public ICommand OpenStructuresCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AssignStructureViewModel>());
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand OpenStructureSelection
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AssignStructureMbusViewModel>());
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public void RefreshIsValid()
    {
      this.IsValid = ((this.IsValid ? 1 : 0) | (!this.IsMBusJob || this.SelectedJobDefinition == null ? 0 : (!string.IsNullOrEmpty(this.StructureRootInfo) ? 1 : 0))) != 0;
      this.IsValid = ((this.IsValid ? 1 : 0) | (!this.IsMinomatJob || this.SelectedMinomatJobDefinition == null ? 0 : (this.SerialNumberList.Contains<string>(this.SerialNumber) ? 1 : 0))) != 0;
    }
  }
}
