// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Archiving.ArchivingViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using Microsoft.Win32;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Archiving;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Archiving;
using MSS.DIConfiguration;
using MSS.DTO.Archive;
using MSS.DTO.MessageHandler;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MSSArchive.Core.Model.Meters;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Archiving
{
  internal class ArchivingViewModel : ValidationViewModelBase
  {
    public readonly IRepositoryFactory RepositoryFactory;
    public readonly IRepository<ArchiveJob> _archiveJobRepository;
    private readonly IWindowFactory _windowFactory;
    private ViewModelBase _messageUserControl;
    private BackgroundWorker _backgroundWorkerExport;
    private bool _searchDone;
    private string _searchArchiveText;
    private List<ArchiveEntity> _archiveEntityCollection;
    private DateTime _selectableDateEndForStartDate = DateTime.Now.AddDays(-1.0);
    private DateTime _selectedStartDate = DateTime.Now.AddDays(-1.0);
    private DateTime _selectedEndDate = DateTime.Now.AddDays(-1.0);
    private bool _isSearchReadingValues;
    private bool _isSearchLogs;
    private ViewModelBase _searchArchiveViewModel;
    private string _archiveName = string.Empty;
    private IEnumerable<ArchiveJobDTO> _getArchiveJobs;
    private bool _isArchiveNowTabVisible;
    private bool _isArchiveNowTabSelected;
    private bool _isSearchArchiveTabVisible;
    private bool _isSearchArchiveTabSelected;
    private bool _isArchiveJobsTabVisible;
    private bool _isArchiveJobsTabSelected;
    private bool _createArchiveJobVisibility;
    private bool _deleteArchiveJobVisiblity;
    private bool _runArchiveJobVisibility;
    private bool _exportArchiveVisibility;
    private bool _archiveAndKeepDataVisibility;
    private bool _archiveAndDeleteDataVisibility;

    [Inject]
    public ArchivingViewModel(IRepositoryFactory repositoryFactory, IWindowFactory windowFactory)
    {
      this.RepositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._archiveJobRepository = repositoryFactory.GetRepository<ArchiveJob>();
      this.ArchivedEntitiesCollection = ((IEnumerable<ArchivedEntitiesEnum>) Enum.GetValues(typeof (ArchivedEntitiesEnum))).Select<ArchivedEntitiesEnum, ArchiveEntity>((Func<ArchivedEntitiesEnum, ArchiveEntity>) (archivedEntitiesEnum => new ArchiveEntity()
      {
        IsChecked = false,
        Name = archivedEntitiesEnum.GetStringValue(),
        ArchivedEntityEnum = archivedEntitiesEnum
      })).ToList<ArchiveEntity>();
      Mapper.CreateMap<ArchiveJob, ArchiveJobDTO>().ForMember((Expression<Func<ArchiveJobDTO, object>>) (ajDto => ajDto.ArchivedEntities), (Action<IMemberConfigurationExpression<ArchiveJob>>) (s => s.ResolveUsing((Func<ArchiveJob, object>) (aj => (object) ArchivingHelper.ArchivedEntitiesString(aj.ArchivedEntities))))).ForMember((Expression<Func<ArchiveJobDTO, object>>) (ajDto => ajDto.ArchivedEntitiesList), (Action<IMemberConfigurationExpression<ArchiveJob>>) (s => s.ResolveUsing((Func<ArchiveJob, object>) (aj => (object) ArchivingHelper.DeserializeArchivedEntities(aj.ArchivedEntities)))));
      this.GetArchiveJobs = this.GetArchiveJobDTOs();
      this.IsSearchReadingValues = true;
      EventPublisher.Register<ArchiveSearched>(new Action<ArchiveSearched>(this.SetArchiveFiltering));
      UsersManager usersManager1 = new UsersManager(this.RepositoryFactory);
      this.ArchiveAndKeepDataVisibility = usersManager1.HasRight(OperationEnum.ArchivingOnDemand.ToString());
      this.ArchiveAndDeleteDataVisibility = usersManager1.HasRight(OperationEnum.CleanupOnDemand.ToString());
      this.IsArchiveNowTabVisible = false;
      this.IsArchiveNowTabSelected = this.IsArchiveNowTabVisible;
      this.IsSearchArchiveTabVisible = usersManager1.HasRight(OperationEnum.ArchivedDataView.ToString());
      this.IsSearchArchiveTabSelected = !this.IsArchiveNowTabSelected && this.IsSearchArchiveTabVisible;
      UsersManager usersManager2 = usersManager1;
      OperationEnum operationEnum = OperationEnum.ArchiveJobView;
      string operation1 = operationEnum.ToString();
      this.IsArchiveJobsTabVisible = usersManager2.HasRight(operation1);
      this.IsArchiveJobsTabSelected = !this.IsArchiveNowTabSelected && !this.IsSearchArchiveTabSelected && this.IsArchiveJobsTabVisible;
      UsersManager usersManager3 = usersManager1;
      operationEnum = OperationEnum.ArchivedDataExport;
      string operation2 = operationEnum.ToString();
      this.ExportArchiveVisibility = usersManager3.HasRight(operation2);
      UsersManager usersManager4 = usersManager1;
      operationEnum = OperationEnum.ArchiveJobCreate;
      string operation3 = operationEnum.ToString();
      this.CreateArchiveJobVisibility = usersManager4.HasRight(operation3);
      UsersManager usersManager5 = usersManager1;
      operationEnum = OperationEnum.ArchiveJobDelete;
      string operation4 = operationEnum.ToString();
      this.DeleteArchiveJobVisibility = usersManager5.HasRight(operation4);
      UsersManager usersManager6 = usersManager1;
      operationEnum = OperationEnum.ArchiveJobRun;
      string operation5 = operationEnum.ToString();
      this.RunArchiveJobVisibility = usersManager6.HasRight(operation5);
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

    public ICommand RunArchiveJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ArchiveJobDTO archiveJobVm = parameter as ArchiveJobDTO;
          if (archiveJobVm == null)
            return;
          GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_CLIENT_ARCHIVE_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.MSS_CLIENT_ARCHIVE_MESSAGE));
          BackgroundWorker backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            ArchiveManagerADO archiveManagerAdo = new ArchiveManagerADO(this.RepositoryFactory);
            ArchiveJob byId = this.RepositoryFactory.GetRepository<ArchiveJob>().GetById((object) archiveJobVm.Id);
            archiveManagerAdo.Archive(byId);
            if (archiveJobVm.DeleteAfterArchive)
              new CleanupManager(this.RepositoryFactory).Cleanup(byId);
            byId.LastExecutionDate = new DateTime?(DateTime.Now);
            this.RepositoryFactory.GetRepository<ArchiveJob>().Update(byId);
            this.GetArchiveJobs = this.GetArchiveJobDTOs();
          });
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
              message = new MSS.DTO.Message.Message()
              {
                MessageType = MessageTypeEnum.Success,
                MessageText = Resources.MSS_Client_Archivation_Succedded
              };
            if (message == null)
              return;
            EventPublisher.Publish<ActionFinished>(new ActionFinished()
            {
              Message = message
            }, (IViewModel) this);
          });
          backgroundWorker.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
        }));
      }
    }

    public ICommand ArchiveAndKeepDataCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!this.IsValid)
            return;
          GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_CLIENT_ARCHIVE_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.MSS_CLIENT_ARCHIVE_MESSAGE));
          BackgroundWorker backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          ArchiveManagerADO archiveManagerAdo;
          backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) => archiveManagerAdo = new ArchiveManagerADO(this.RepositoryFactory));
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
              message = new MSS.DTO.Message.Message()
              {
                MessageType = MessageTypeEnum.Success,
                MessageText = Resources.MSS_Client_Archivation_Succedded
              };
            if (message == null)
              return;
            EventPublisher.Publish<ActionFinished>(new ActionFinished()
            {
              Message = message
            }, (IViewModel) this);
          });
          backgroundWorker.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
        }));
      }
    }

    public ICommand ArchiveAndDeleteDataCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!this.IsValid)
            return;
          GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_CLIENT_ARCHIVE_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.MSS_CLIENT_ARCHIVE_MESSAGE));
          BackgroundWorker backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            ArchiveDetailsADO archiveDetailsAdo = new ArchiveDetailsADO()
            {
              StartTime = this.SelectedStartDate,
              EndTime = this.SelectedEndDate,
              ArchivedEntities = this.ArchivedEntitiesCollection,
              ArchiveName = this.ArchiveName
            };
            ArchiveManagerADO archiveManagerAdo = new ArchiveManagerADO(this.RepositoryFactory);
            CleanupManager cleanupManager = new CleanupManager(this.RepositoryFactory);
          });
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
              message = new MSS.DTO.Message.Message()
              {
                MessageType = MessageTypeEnum.Success,
                MessageText = Resources.MSS_Client_Archivation_Succedded
              };
            if (message == null)
              return;
            EventPublisher.Publish<ActionFinished>(new ActionFinished()
            {
              Message = message
            }, (IViewModel) this);
          });
          backgroundWorker.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
        }));
      }
    }

    public ICommand CreateArchiveJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditArchiveJobViewModel>((IParameter) new ConstructorArgument("archiveJob", (object) null)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
            this.GetArchiveJobs = this.GetArchiveJobDTOs();
            this.OnPropertyChanged("GetArchiveJobs");
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand EditArchiveJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ArchiveJobDTO archiveJobDto = parameter as ArchiveJobDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditArchiveJobViewModel>((IParameter) new ConstructorArgument("archiveJob", (object) archiveJobDto)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            if (archiveJobDto != null)
              this._archiveJobRepository.Refresh((object) archiveJobDto.Id);
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
            this.GetArchiveJobs = this.GetArchiveJobDTOs();
            this.OnPropertyChanged("GetArchiveJobs");
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand DeleteArchiveJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ArchiveJobDTO archiveJobDto = parameter as ArchiveJobDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DeleteArchiveJobViewModel>((IParameter) new ConstructorArgument("archiveJob", (object) archiveJobDto)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
            this.GetArchiveJobs = this.GetArchiveJobDTOs();
            this.OnPropertyChanged("GetArchiveJobs");
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    private void WriteArchiveList<T>(
      CSVManager csvManager,
      string fileName,
      Expression<Func<T, bool>> expression)
    {
      ISessionFactory factoryMssArchive = ArchiveManagerNHibernate.GetSessionFactoryMSSArchive();
      PagingProvider<T> pagingProvider = expression != null ? new PagingProvider<T>(factoryMssArchive, expression) : new PagingProvider<T>(factoryMssArchive);
      int num = pagingProvider.FetchCount();
      int parameterValue = MSS.Business.Utils.AppContext.Current.GetParameterValue<int>("LoadSizeForExportOfArchive");
      ReportingHelper reportingHelper = new ReportingHelper();
      IList<T> archiveList = pagingProvider.FetchRange(1, parameterValue);
      int startIndex = 1;
      StreamWriter streamWriter = new StreamWriter(fileName, false);
      streamWriter.WriteLine(reportingHelper.WriteArchiveListHeader<T>());
      streamWriter.Close();
      while (startIndex < num)
      {
        List<string[]> archiveListRows = reportingHelper.GetArchiveListRows<T>((IEnumerable<T>) archiveList);
        startIndex += parameterValue;
        archiveList = pagingProvider.FetchRange(startIndex, parameterValue);
        csvManager.WriteToFile(fileName, archiveListRows);
      }
    }

    private void CancelProcess(object sender, EventArgs e)
    {
      this._backgroundWorkerExport.CancelAsync();
    }

    public ICommand ExportArchiveCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_Client_Export), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.EXPORT_INSTALLATION_ORDER_TEXT));
          this._backgroundWorkerExport = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          SaveFileDialog saveStructureDialog = new SaveFileDialog()
          {
            Filter = "CSV Document|*.csv|Xcel Document|*.xlsx",
            Title = "Save archive to file"
          };
          bool? isOkButton = saveStructureDialog.ShowDialog();
          this._backgroundWorkerExport.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            Expression<Func<ArchiveMeterReadingValue, bool>> expression = (Expression<Func<ArchiveMeterReadingValue, bool>>) null;
            if (this.SearchArchiveText != null)
              expression = (Expression<Func<ArchiveMeterReadingValue, bool>>) (mrv => mrv.MeterSerialNumber == this.\u003C\u003E4__this.SearchArchiveText);
            DoWorkEventArgs doWorkEventArgs = args;
            doWorkEventArgs.Cancel = ((doWorkEventArgs.Cancel ? 1 : 0) | (!isOkButton.HasValue ? 1 : (!isOkButton.Value ? 1 : 0))) != 0;
            if (saveStructureDialog.FileName == string.Empty)
              return;
            switch (saveStructureDialog.FilterIndex)
            {
              case 1:
                this.WriteArchiveList<ArchiveMeterReadingValue>(new CSVManager(), saveStructureDialog.FileName.Replace(".csv", "_ArchiveMeterReadingValues.csv"), expression);
                break;
              case 2:
                new XCellManager().WriteArchiveToFile<ArchiveMeterReadingValue>(saveStructureDialog.FileName, expression);
                break;
            }
          });
          this._backgroundWorkerExport.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            pd.OnRequestClose(false);
            if (args.Cancelled)
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
            else if (args.Error != null)
            {
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
            }
            else
              this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
          });
          this._backgroundWorkerExport.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, this._backgroundWorkerExport);
        }));
      }
    }

    public IEnumerable<ArchiveJobDTO> GetArchiveJobDTOs()
    {
      return (IEnumerable<ArchiveJobDTO>) Mapper.Map<List<ArchiveJob>, List<ArchiveJobDTO>>(this.RepositoryFactory.GetRepository<ArchiveJob>().GetAll().ToList<ArchiveJob>());
    }

    private void SetArchiveFiltering(ArchiveSearched obj)
    {
      this.SearchDone = true;
      this.SearchArchiveText = obj.SearchedText;
    }

    public bool SearchDone
    {
      get => this._searchDone;
      set
      {
        this._searchDone = value;
        this.OnPropertyChanged(nameof (SearchDone));
      }
    }

    public string SearchArchiveText
    {
      get => this._searchArchiveText;
      set
      {
        this._searchArchiveText = value;
        this.OnPropertyChanged(nameof (SearchArchiveText));
      }
    }

    public bool IsCheckedReadingData { get; set; }

    public bool IsCheckedOrders { get; set; }

    public bool IsCheckedJobs { get; set; }

    public bool IsCheckedLogs { get; set; }

    public List<ArchiveEntity> ArchivedEntitiesCollection
    {
      get => this._archiveEntityCollection;
      set
      {
        this._archiveEntityCollection = value;
        this.OnPropertyChanged(nameof (ArchivedEntitiesCollection));
      }
    }

    public DateTime SelectableDateEnd => DateTime.Now.AddDays(-1.0);

    public DateTime SelectableDateEndForStartDate
    {
      get => this._selectableDateEndForStartDate;
      set
      {
        this._selectableDateEndForStartDate = value;
        this.OnPropertyChanged(nameof (SelectableDateEndForStartDate));
      }
    }

    public DateTime SelectedStartDate
    {
      get => this._selectedStartDate;
      set
      {
        this._selectedStartDate = value;
        this.OnPropertyChanged(nameof (SelectedStartDate));
      }
    }

    public DateTime SelectedEndDate
    {
      get => this._selectedEndDate;
      set
      {
        this._selectedEndDate = value;
        this.SelectableDateEndForStartDate = this._selectedEndDate;
        if (this.SelectedStartDate > this._selectedEndDate)
          this.SelectedStartDate = this._selectedEndDate;
        this.OnPropertyChanged(nameof (SelectedEndDate));
      }
    }

    public bool IsSearchReadingValues
    {
      get => this._isSearchReadingValues;
      set
      {
        this._isSearchReadingValues = value;
        if (this._isSearchReadingValues)
          this.SearchArchiveViewModel = (ViewModelBase) DIConfigurator.GetConfigurator().Get<SearchReadingValuesViewModel>();
        this.OnPropertyChanged(nameof (IsSearchReadingValues));
      }
    }

    public bool IsSearchLogs
    {
      get => this._isSearchLogs;
      set
      {
        this._isSearchLogs = value;
        if (this._isSearchLogs)
          this.SearchArchiveViewModel = (ViewModelBase) DIConfigurator.GetConfigurator().Get<SearchLogsViewModel>();
        this.OnPropertyChanged(nameof (IsSearchLogs));
      }
    }

    public ViewModelBase SearchArchiveViewModel
    {
      get => this._searchArchiveViewModel;
      set
      {
        if (this._searchArchiveViewModel != null)
          this._searchArchiveViewModel.Dispose();
        this._searchArchiveViewModel = value;
        this.OnPropertyChanged(nameof (SearchArchiveViewModel));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Archive_ArchiveNameValidationMessage")]
    public string ArchiveName
    {
      get => this._archiveName;
      set
      {
        this._archiveName = value;
        this.OnPropertyChanged(nameof (ArchiveName));
      }
    }

    public IEnumerable<ArchiveJobDTO> GetArchiveJobs
    {
      get => this._getArchiveJobs;
      set
      {
        this._getArchiveJobs = value;
        this.OnPropertyChanged(nameof (GetArchiveJobs));
      }
    }

    public bool IsArchiveNowTabVisible
    {
      get => this._isArchiveNowTabVisible;
      set
      {
        this._isArchiveNowTabVisible = value;
        this.OnPropertyChanged(nameof (IsArchiveNowTabVisible));
      }
    }

    public bool IsArchiveNowTabSelected
    {
      get => this._isArchiveNowTabSelected;
      set
      {
        this._isArchiveNowTabSelected = value;
        this.OnPropertyChanged(nameof (IsArchiveNowTabSelected));
      }
    }

    public bool IsSearchArchiveTabVisible
    {
      get => this._isSearchArchiveTabVisible;
      set
      {
        this._isSearchArchiveTabVisible = value;
        this.OnPropertyChanged(nameof (IsSearchArchiveTabVisible));
      }
    }

    public bool IsSearchArchiveTabSelected
    {
      get => this._isSearchArchiveTabSelected;
      set
      {
        this._isSearchArchiveTabSelected = value;
        this.OnPropertyChanged(nameof (IsSearchArchiveTabSelected));
      }
    }

    public bool IsArchiveJobsTabVisible
    {
      get => this._isArchiveJobsTabVisible;
      set
      {
        this._isArchiveJobsTabVisible = value;
        this.OnPropertyChanged(nameof (IsArchiveJobsTabVisible));
      }
    }

    public bool IsArchiveJobsTabSelected
    {
      get => this._isArchiveJobsTabSelected;
      set
      {
        this._isArchiveJobsTabSelected = value;
        this.OnPropertyChanged(nameof (IsArchiveJobsTabSelected));
      }
    }

    public bool CreateArchiveJobVisibility
    {
      get => this._createArchiveJobVisibility;
      set
      {
        this._createArchiveJobVisibility = value;
        this.OnPropertyChanged(nameof (CreateArchiveJobVisibility));
      }
    }

    public bool DeleteArchiveJobVisibility
    {
      get => this._deleteArchiveJobVisiblity;
      set
      {
        this._deleteArchiveJobVisiblity = value;
        this.OnPropertyChanged(nameof (DeleteArchiveJobVisibility));
      }
    }

    public bool RunArchiveJobVisibility
    {
      get => this._runArchiveJobVisibility;
      set
      {
        this._runArchiveJobVisibility = value;
        this.OnPropertyChanged(nameof (RunArchiveJobVisibility));
      }
    }

    public bool ExportArchiveVisibility
    {
      get => this._exportArchiveVisibility;
      set
      {
        this._exportArchiveVisibility = value;
        this.OnPropertyChanged(nameof (ExportArchiveVisibility));
      }
    }

    public bool ArchiveAndKeepDataVisibility
    {
      get => this._archiveAndKeepDataVisibility;
      set
      {
        this._archiveAndKeepDataVisibility = value;
        this.OnPropertyChanged(nameof (ArchiveAndKeepDataVisibility));
      }
    }

    public bool ArchiveAndDeleteDataVisibility
    {
      get => this._archiveAndDeleteDataVisibility;
      set
      {
        this._archiveAndDeleteDataVisibility = value;
        this.OnPropertyChanged(nameof (ArchiveAndDeleteDataVisibility));
      }
    }
  }
}
