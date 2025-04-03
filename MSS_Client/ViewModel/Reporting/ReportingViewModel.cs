// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Reporting.ReportingViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using Microsoft.Win32;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.MDMCommunication.Business.Managers;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.DataFilters;
using MSS_Client.ViewModel.GenericProgressDialog;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Type;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace MSS_Client.ViewModel.Reporting
{
  public class ReportingViewModel : ValidationViewModelBase
  {
    private ViewModelBase _messageUserControlAutomatedExports;
    private ViewModelBase _messageUserControlMeterData;
    private BackgroundWorker _backgroundWorkerMDM;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private readonly StructuresManager _structuresManager;
    private BackgroundWorker _backgroundWorkerSync;
    private IList<string> foundMeters = (IList<string>) new List<string>();
    private string _pageSize = string.Empty;
    private bool _isPrintButtonEnabled;
    private IEnumerable<MSS.Core.Model.DataFilters.Filter> _filterCollection;
    private MSS.Core.Model.DataFilters.Filter _selectedFilter;
    private DateTime? _startDate;
    private DateTime? _endDateJobLogValue;
    private DateTime? _endDate;
    private IEnumerable<MeterReadingValue> _readingValues = (IEnumerable<MeterReadingValue>) new List<MeterReadingValue>();
    private IEnumerable<MeterReadingValueDTO> _readingValuesDto = (IEnumerable<MeterReadingValueDTO>) new List<MeterReadingValueDTO>();
    private StructureType _selectedType;
    private IEnumerable<StructureNodeDTO> _structureNodeCollection;
    private StructureNodeDTO _selectedStructureNodeDto;
    private bool _isFiltered;
    private bool _isMonthly;
    private bool _isDaily;
    private bool _isYearly;
    private DateTime? _startDateJobLogValue;
    private int _alternationCountNumber = 2;
    private bool _isAutomatedExportsTabVisible;
    private bool _isCreateAutomatedExportVisible;
    private bool _isDeleteAutomatedExportVisible;
    private bool _isMeterDataTabVisible;
    private bool _meterDataExportVisibility;

    [Inject]
    public ReportingViewModel(IRepositoryFactory repositoryFactory, IWindowFactory windowFactory)
    {
      this.PageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      Mapper.CreateMap<JobLogs, JobLogsDTO>().ForMember((Expression<Func<JobLogsDTO, object>>) (j => j.JobName), (Action<IMemberConfigurationExpression<JobLogs>>) (j => j.ResolveUsing((Func<JobLogs, object>) (jl => (object) jl.Job.JobDefinition.Name))));
      Mapper.CreateMap<MeterReadingValue, MeterReadingValueDTO>();
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      this._structuresManager = new StructuresManager(this._repositoryFactory);
      this.StructureNodeCollection = (IEnumerable<StructureNodeDTO>) this._structuresManager.GetStructureNodesCollection(StructureTypeEnum.Physical);
      ObservableCollection<StructureType> observableCollection = new ObservableCollection<StructureType>();
      observableCollection.Add(new StructureType()
      {
        IdEnum = StructureTypeEnum.Physical,
        Name = Resources.MSS_Client_Orders_PhysicalStructure
      });
      observableCollection.Add(new StructureType()
      {
        IdEnum = StructureTypeEnum.Logical,
        Name = Resources.MSS_Client_Orders_LogicalStructure
      });
      this.StructureTypeCollection = observableCollection;
      this.SelectedType = this.StructureTypeCollection.First<StructureType>();
      this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
      this.SelectedFilter = this.FilterCollection.FirstOrDefault<MSS.Core.Model.DataFilters.Filter>((Func<MSS.Core.Model.DataFilters.Filter, bool>) (x => x.Name == "Any"));
      this.IsFiltered = false;
      this.IsDaily = true;
      DateTime now = DateTime.Now;
      this.StartDate = new DateTime?(now.Subtract(new TimeSpan(3, 0, 0, 0)));
      now = DateTime.Now;
      this.EndDate = new DateTime?(now.AddDays(3.0));
      EventPublisher.Register<ActionSearchByText>(new Action<ActionSearchByText>(this.UpdateMeterReadingValuesDTO));
      this.IsPrintButtonEnabled = false;
      UsersManager usersManager1 = new UsersManager(this._repositoryFactory);
      bool flag1 = usersManager1.HasRight(OperationEnum.AutomatedExportView.ToString());
      bool flag2 = usersManager1.HasRight(OperationEnum.AutomatedExportCreate.ToString());
      UsersManager usersManager2 = usersManager1;
      OperationEnum operationEnum = OperationEnum.AutomatedExportDelete;
      string operation1 = operationEnum.ToString();
      bool flag3 = usersManager2.HasRight(operation1);
      this.IsAutomatedExportsTabVisible = flag1;
      this.IsCreateAutomatedExportVisible = flag1 & flag2;
      this.IsDeleteAutomatedExportVisible = flag1 & flag3;
      UsersManager usersManager3 = usersManager1;
      operationEnum = OperationEnum.ReadingDataView;
      string operation2 = operationEnum.ToString();
      this.IsMeterDataTabVisible = usersManager3.HasRight(operation2);
      UsersManager usersManager4 = usersManager1;
      operationEnum = OperationEnum.ReadingDataExport;
      string operation3 = operationEnum.ToString();
      this.MeterDataExportVisibility = usersManager4.HasRight(operation3);
      this.IsMeterDataTabSelected = this.IsMeterDataTabVisible;
      this.IsAutomatedExportJobTabSelected = !this.IsMeterDataTabVisible;
    }

    public ICommand GetConsumptionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (parameter != null)
          {
            if (((StructureNodeDTO) parameter).Entity is MeterDTO entity2)
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ConsumptionViewModel>((IParameter) new ConstructorArgument("selectedMeter", (object) entity2), (IParameter) new ConstructorArgument("startDate", (object) this.StartDate), (IParameter) new ConstructorArgument("endDate", (object) this.EndDate), (IParameter) new ConstructorArgument("filter", (object) this.SelectedFilter)));
            else
              MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), Resources.MSS_Client_Consumption_MissingMeterSelection_Warning, false);
          }
          else
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.ToString(), Resources.MSS_Client_Consumption_MissingMeterSelection_Warning, false);
        }));
      }
    }

    public ICommand CreateJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ExportJobViewModel>((IParameter) new ConstructorArgument("action", (object) "create")));
          this.MessageUserControlAutomatedExports = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage) : MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          this.OnPropertyChanged("GetJobs");
        });
      }
    }

    public ICommand EditJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          AutomatedExportJobDTO automatedExportJobDto = parameter as AutomatedExportJobDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ExportJobViewModel>((IParameter) new ConstructorArgument("exportJob", (object) automatedExportJobDto), (IParameter) new ConstructorArgument("action", (object) "edit")));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this._repositoryFactory.GetRepository<AutomatedExportJob>().Refresh((object) automatedExportJobDto.Id);
            this.MessageUserControlAutomatedExports = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          }
          else
            this.MessageUserControlAutomatedExports = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this.OnPropertyChanged("GetJobs");
        }));
      }
    }

    public ICommand DeleteJobCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          try
          {
            AutomatedExportJobDTO automatedExportJobDto = parameter as AutomatedExportJobDTO;
            new ReportingManager(this._repositoryFactory).DeleteExportJob(this._repositoryFactory.GetRepository<AutomatedExportJob>().GetById((object) automatedExportJobDto.Id));
            this.MessageUserControlAutomatedExports = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          }
          catch (Exception ex)
          {
            this.MessageUserControlAutomatedExports = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            throw;
          }
          this.OnPropertyChanged("GetJobs");
        }));
      }
    }

    public ICommand ExportAllReadingValues
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) delegate
        {
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ExportFileSettingsViewModel>());
        });
      }
    }

    public ICommand SearchCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          string searchText = parameter as string;
          if (string.IsNullOrEmpty(searchText))
            return;
          foreach (StructureNodeDTO structureNode in this.StructureNodeCollection)
          {
            IEnumerable<StructureNodeDTO> source1 = StructuresHelper.Descendants(structureNode).Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n =>
            {
              if (n.Name.Contains(searchText) || n.Description != null && n.Description.Contains(searchText))
                return true;
              return n.Entity is MeterDTO && ((MeterDTO) n.Entity).SerialNumber.Contains(searchText);
            }));
            if (!(source1 is IList<StructureNodeDTO> structureNodeDtoList2))
              structureNodeDtoList2 = (IList<StructureNodeDTO>) source1.ToList<StructureNodeDTO>();
            IList<StructureNodeDTO> source2 = structureNodeDtoList2;
            structureNode.IsExpanded = source2.Any<StructureNodeDTO>();
            foreach (StructureNodeDTO structureNodeDto in (IEnumerable<StructureNodeDTO>) source2)
              structureNodeDto.BackgroundColor = (Brush) Brushes.LightGreen;
          }
        }));
      }
    }

    public ICommand OpenFilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<FilterViewModel>());
          this.RefreshFilter(new RefreshFilters()
          {
            isRefresh = true
          });
        }));
      }
    }

    public ICommand SearchForReadingValues
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          GenericProgressDialogViewModel vm = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.METER_DATA_LOAD_READING_VALUES_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.METER_DATA_LOAD_READING_VALUES));
          this._backgroundWorkerSync = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          this._backgroundWorkerSync.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            try
            {
              this.AlternationCountNumber = 2;
              MappingsManager.MeterReadingValue_to_MeterReadingValueDTO();
              StructureNodeDTO node = (StructureNodeDTO) parameter;
              this.foundMeters = (IList<string>) new List<string>();
              this.GetMetersFromList(node);
              string[] array = this.foundMeters.ToArray<string>();
              bool isAny = new List<long>().Contains(0L);
              if (this.IsFiltered)
              {
                if (this.IsDaily)
                  this.GetFilteredReadingValues(array, this.SelectedFilter.Rules, "day", isAny);
                if (this.IsMonthly)
                  this.GetFilteredReadingValues(array, this.SelectedFilter.Rules, "month", isAny);
                if (!this.IsYearly)
                  return;
                this.GetFilteredReadingValues(array, this.SelectedFilter.Rules, "year", isAny);
              }
              else
              {
                IList<MeterReadingValue> source;
                if (isAny)
                {
                  source = this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("ReadingValues").Add((ICriterion) Restrictions.In("ReadingValues.MeterSerialNumber", (object[]) array)).Add((ICriterion) Restrictions.Gt("ReadingValues.Date", (object) this.StartDate)).Add((ICriterion) Restrictions.Lt("ReadingValues.Date", (object) this.EndDate)).List<MeterReadingValue>();
                }
                else
                {
                  ICriteria criteria = this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("ReadingValues").Add((ICriterion) Restrictions.In("ReadingValues.MeterSerialNumber", (object[]) array)).Add((ICriterion) Restrictions.Gt("ReadingValues.Date", (object) this.StartDate)).Add((ICriterion) Restrictions.Lt("ReadingValues.Date", (object) this.EndDate));
                  this.GetCriteria(criteria, this.SelectedFilter.Rules);
                  source = criteria.List<MeterReadingValue>();
                }
                this.ReadingValuesDto = (IEnumerable<MeterReadingValueDTO>) Mapper.Map<IEnumerable<MeterReadingValue>, ObservableCollection<MeterReadingValueDTO>>((IEnumerable<MeterReadingValue>) source);
                this.SetRowColors(this.ReadingValuesDto.ToList<MeterReadingValueDTO>());
                this.MeterReadingValuesDTO = Mapper.Map<IEnumerable<MeterReadingValue>, ObservableCollection<MeterReadingValueDTO>>(this.ReadingValues);
                Application.Current.Dispatcher.Invoke<ViewModelBase>((Func<ViewModelBase>) (() => this.MessageUserControlMeterData = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue())));
              }
            }
            catch (Exception ex)
            {
              Application.Current.Dispatcher.Invoke((Action) (() =>
              {
                this.MessageUserControlMeterData = MessageHandlingManager.ShowWarningMessage(MessageCodes.Error.GetStringValue() + " " + ex.Message);
                MSS.Business.Errors.MessageHandler.LogException(ex.Message);
                MessageHandlingManager.ShowExceptionMessageDialog(ex.Message, this._windowFactory);
              }));
            }
          });
          this._backgroundWorkerSync.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) => vm.OnRequestClose(false));
          this._backgroundWorkerSync.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) vm, this._backgroundWorkerSync);
        }));
      }
    }

    public ICommand ExportDataCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.ReadingValuesDto == null || this.ReadingValuesDto.ToList<MeterReadingValueDTO>().Count == 0)
            return;
          CultureInfo cultureInfo = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
          Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
          ReportingManager reportingManager = new ReportingManager(this._repositoryFactory);
          SaveFileDialog saveFileDialog = new SaveFileDialog()
          {
            Filter = "CSV Document|*.csv",
            Title = "Save structure to file"
          };
          bool? nullable = saveFileDialog.ShowDialog();
          if (saveFileDialog.FileName == string.Empty)
            return;
          List<string[]> readingValuesList = reportingManager.CreateReadingValuesList(this.ReadingValuesDto.ToList<MeterReadingValueDTO>());
          try
          {
            if (nullable.HasValue && nullable.Value)
            {
              try
              {
                List<string[]> nodeList = CSVManager.AddQuatForCSV(readingValuesList);
                new CSVManager().WriteToFile(saveFileDialog.FileName, nodeList);
                this.MessageUserControlMeterData = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
              }
              catch (Exception ex)
              {
                this.MessageUserControlMeterData = MessageHandlingManager.ShowWarningMessage(MessageCodes.Error.GetStringValue() + " " + ex.Message);
                MSS.Business.Errors.MessageHandler.LogException(ex.Message);
                MessageHandlingManager.ShowExceptionMessageDialog(ex.Message, this._windowFactory);
              }
            }
            else
              this.MessageUserControlMeterData = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          }
          finally
          {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
          }
        }));
      }
    }

    public ICommand PrintCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<PrintOptionsViewModel>((IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory), (IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("readingValues", (object) this.ReadingValuesDto)))));
      }
    }

    public ICommand MDMExportCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          GenericProgressDialogViewModel vm = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_Client_MDMExportTitle), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.MSS_Client_MDMExportMessage));
          this._backgroundWorkerMDM = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          this._backgroundWorkerMDM.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            MDMManager mdmManager = new MDMManager(this._repositoryFactory);
            mdmManager.SavePortfolioRecord();
            mdmManager.SaveBuildingRecord();
            mdmManager.SaveTenantInfoRecord();
            mdmManager.SaveTenantFlatRecord();
            mdmManager.SaveDeviceInfoRecord();
            mdmManager.SaveReadDataRecord();
            mdmManager.SaveAddressRecord();
            mdmManager.SaveDCUInfoRecord();
            mdmManager.SaveDCUConnectionRecord();
            mdmManager.SaveAMRRouteRecord();
            mdmManager.SaveTestConfigRunRecord();
            mdmManager.SaveTestConfigDeviceRecord();
          });
          this._backgroundWorkerMDM.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            vm.OnRequestClose(false);
            if (args.Cancelled)
              this.MessageUserControlAutomatedExports = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_MDMJobs_Cancelled);
            else if (args.Error != null)
            {
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
            }
            else
              this.MessageUserControlAutomatedExports = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_MDMJobs_Succedded);
          });
          this._backgroundWorkerMDM.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) vm, this._backgroundWorkerMDM);
        }));
      }
    }

    public ObservableCollection<MeterReadingValueDTO> MeterReadingValuesDTO { get; set; }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
      }
    }

    public bool IsPrintButtonEnabled
    {
      get => this._isPrintButtonEnabled;
      set
      {
        this._isPrintButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsPrintButtonEnabled));
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

    public MSS.Core.Model.DataFilters.Filter SelectedFilter
    {
      get => this._selectedFilter;
      set
      {
        this._selectedFilter = value;
        this.OnPropertyChanged(nameof (SelectedFilter));
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

    public DateTime? EndDate
    {
      get => this._endDate;
      set
      {
        this._endDate = value;
        this.OnPropertyChanged(nameof (EndDate));
      }
    }

    public IEnumerable<MeterReadingValue> ReadingValues
    {
      get => this._readingValues;
      set
      {
        this._readingValues = value;
        this.OnPropertyChanged(nameof (ReadingValues));
      }
    }

    public IEnumerable<MeterReadingValueDTO> ReadingValuesDto
    {
      get => this._readingValuesDto;
      set
      {
        this._readingValuesDto = value;
        this.OnPropertyChanged(nameof (ReadingValuesDto));
        this.IsPrintButtonEnabled = this._readingValuesDto != null && this._readingValuesDto.ToList<MeterReadingValueDTO>().Any<MeterReadingValueDTO>();
      }
    }

    public StructureType SelectedType
    {
      get => this._selectedType;
      set
      {
        this._selectedType = value;
        this.StructureNodeCollection = (IEnumerable<StructureNodeDTO>) this._structuresManager.GetStructureNodesCollection(this.SelectedType.IdEnum == StructureTypeEnum.Logical ? StructureTypeEnum.Logical : StructureTypeEnum.Physical);
        this.OnPropertyChanged(nameof (SelectedType));
      }
    }

    public ObservableCollection<StructureType> StructureTypeCollection { get; set; }

    public IEnumerable<StructureNodeDTO> StructureNodeCollection
    {
      get => this._structureNodeCollection;
      set
      {
        this._structureNodeCollection = value;
        this.OnPropertyChanged(nameof (StructureNodeCollection));
      }
    }

    public StructureNodeDTO SelectedStructureNodeItem
    {
      get => this._selectedStructureNodeDto;
      set
      {
        this._selectedStructureNodeDto = value;
        this.OnPropertyChanged(nameof (SelectedStructureNodeItem));
      }
    }

    public ViewModelBase MessageUserControlAutomatedExports
    {
      get => this._messageUserControlAutomatedExports;
      set
      {
        this._messageUserControlAutomatedExports = value;
        this.OnPropertyChanged(nameof (MessageUserControlAutomatedExports));
      }
    }

    public ViewModelBase MessageUserControlMeterData
    {
      get => this._messageUserControlMeterData;
      set
      {
        this._messageUserControlMeterData = value;
        this.OnPropertyChanged(nameof (MessageUserControlMeterData));
      }
    }

    public bool IsFiltered
    {
      get => this._isFiltered;
      set
      {
        this._isFiltered = value;
        this.OnPropertyChanged(nameof (IsFiltered));
      }
    }

    public bool IsMonthly
    {
      get => this._isMonthly;
      set
      {
        this._isMonthly = value;
        this.OnPropertyChanged(nameof (IsMonthly));
      }
    }

    public bool IsDaily
    {
      get => this._isDaily;
      set
      {
        this._isDaily = value;
        this.OnPropertyChanged(nameof (IsDaily));
      }
    }

    public bool IsYearly
    {
      get => this._isYearly;
      set
      {
        this._isYearly = value;
        this.OnPropertyChanged(nameof (IsYearly));
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

    public int AlternationCountNumber
    {
      get => this._alternationCountNumber;
      set
      {
        this._alternationCountNumber = value;
        this.OnPropertyChanged(nameof (AlternationCountNumber));
      }
    }

    public bool IsAutomatedExportJobTabSelected { get; set; }

    public bool IsMeterDataTabSelected { get; set; }

    public bool IsAutomatedExportsTabVisible
    {
      get => this._isAutomatedExportsTabVisible;
      set
      {
        this._isAutomatedExportsTabVisible = value;
        this.OnPropertyChanged(nameof (IsAutomatedExportsTabVisible));
      }
    }

    public bool IsCreateAutomatedExportVisible
    {
      get => this._isCreateAutomatedExportVisible;
      set
      {
        this._isCreateAutomatedExportVisible = value;
        this.OnPropertyChanged(nameof (IsCreateAutomatedExportVisible));
      }
    }

    public bool IsDeleteAutomatedExportVisible
    {
      get => this._isDeleteAutomatedExportVisible;
      set
      {
        this._isDeleteAutomatedExportVisible = value;
        this.OnPropertyChanged(nameof (IsDeleteAutomatedExportVisible));
      }
    }

    public bool IsMeterDataTabVisible
    {
      get => this._isMeterDataTabVisible;
      set
      {
        this._isMeterDataTabVisible = value;
        this.OnPropertyChanged(nameof (IsMeterDataTabVisible));
      }
    }

    public bool MeterDataExportVisibility
    {
      get => this._meterDataExportVisibility;
      set
      {
        this._meterDataExportVisibility = value;
        this.OnPropertyChanged(nameof (MeterDataExportVisibility));
      }
    }

    private void RefreshFilter(RefreshFilters obj)
    {
      if (!obj.isRefresh)
        return;
      string selectedName = this.SelectedFilter.Name;
      this._repositoryFactory.GetSession().Clear();
      this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>().GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
      this.SelectedFilter = this.FilterCollection.FirstOrDefault<MSS.Core.Model.DataFilters.Filter>((Func<MSS.Core.Model.DataFilters.Filter, bool>) (x => x.Name == selectedName)) ?? this.FilterCollection.FirstOrDefault<MSS.Core.Model.DataFilters.Filter>((Func<MSS.Core.Model.DataFilters.Filter, bool>) (x => true));
    }

    private void UpdateMeterReadingValuesDTO(ActionSearchByText actionSearchByText)
    {
      string searchedText = actionSearchByText.SearchedText;
      if (!(searchedText != string.Empty))
        return;
      bool flag = false;
      foreach (MeterReadingValueDTO meterReadingValueDto in this.ReadingValuesDto)
      {
        if (meterReadingValueDto.MeterSerialNumber.Contains(searchedText))
        {
          flag = true;
          meterReadingValueDto.BackgroundColor = (Brush) Brushes.LightGreen;
        }
        else
          meterReadingValueDto.BackgroundColor = (Brush) Brushes.Transparent;
      }
      this.AlternationCountNumber = flag ? 0 : 2;
    }

    public void GetMetersFromList(StructureNodeDTO node)
    {
      if (node.Entity is MeterDTO)
        this.foundMeters.Add((node.Entity as MeterDTO).SerialNumber);
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
      {
        if (subNode.Entity is MeterDTO)
          this.foundMeters.Add((subNode.Entity as MeterDTO).SerialNumber);
        if (subNode.SubNodes.Count != 0)
          this.GetMetersFromList(subNode);
      }
    }

    private void CreateMessage(ActionSyncFinished messageFinished)
    {
      if (messageFinished.Message.MessageType != MessageTypeEnum.Success)
        return;
      this.MessageUserControlAutomatedExports = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
    }

    public IEnumerable<AutomatedExportJobDTO> GetJobs
    {
      get
      {
        IList<AutomatedExportJob> all = this._repositoryFactory.GetRepository<AutomatedExportJob>().GetAll();
        MappingsManager.AutomatedExportJob_to_AutomatedExportJobDTO();
        return (IEnumerable<AutomatedExportJobDTO>) Mapper.Map<IList<AutomatedExportJob>, IList<AutomatedExportJobDTO>>(all);
      }
    }

    private void GetFilteredReadingValues(
      string[] serialNumbers,
      IList<Rules> rules,
      string selection,
      bool isAny)
    {
      MappingsManager.MeterReadingValue_to_MeterReadingValueDTO();
      DetachedCriteria dc = DetachedCriteria.For<MeterReadingValue>("RD").SetProjection((IProjection) Projections.ProjectionList().Add((IProjection) Projections.Max("RD.Date"), "MaxDate").Add((IProjection) Projections.GroupProperty((IProjection) Projections.Property("MeterId"))).Add((IProjection) Projections.GroupProperty(Projections.SqlFunction(selection, (IType) NHibernateUtil.DateTime, (IProjection) Projections.Property("RD.Date"))))).Add((ICriterion) Restrictions.EqProperty("ReadingValues.Date", (IProjection) Projections.Max("RD.Date"))).Add((ICriterion) Restrictions.EqProperty("ReadingValues.MeterId", "RD.MeterId"));
      IList<MeterReadingValue> source;
      if (isAny)
      {
        source = this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("ReadingValues").Add((ICriterion) Subqueries.Exists(dc)).Add((ICriterion) Restrictions.In("ReadingValues.MeterSerialNumber", (object[]) serialNumbers)).Add((ICriterion) Restrictions.Gt("ReadingValues.Date", (object) this.StartDate)).Add((ICriterion) Restrictions.Lt("ReadingValues.Date", (object) this.EndDate)).List<MeterReadingValue>();
      }
      else
      {
        ICriteria criteria = this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("ReadingValues").Add((ICriterion) Subqueries.Exists(dc)).Add((ICriterion) Restrictions.In("ReadingValues.MeterSerialNumber", (object[]) serialNumbers)).Add((ICriterion) Restrictions.Gt("ReadingValues.Date", (object) this.StartDate)).Add((ICriterion) Restrictions.Lt("ReadingValues.Date", (object) this.EndDate));
        this.GetCriteria(criteria, rules);
        source = criteria.List<MeterReadingValue>();
      }
      this.ReadingValuesDto = (IEnumerable<MeterReadingValueDTO>) Mapper.Map<IEnumerable<MeterReadingValue>, ObservableCollection<MeterReadingValueDTO>>((IEnumerable<MeterReadingValue>) source);
    }

    private ICriteria GetCriteria(ICriteria criteria, IList<Rules> rules)
    {
      Disjunction disjunction = Restrictions.Disjunction();
      TypeHelperExtensionMethods.ForEach<Rules>((IEnumerable<Rules>) rules, (Action<Rules>) (x =>
      {
        Conjunction conjunction = Restrictions.Conjunction();
        if (x.Calculation != 0)
          conjunction.Add((ICriterion) Restrictions.Eq("ReadingValues.Calculation", (object) (long) x.Calculation));
        if (x.CalculationStart != 0)
          conjunction.Add((ICriterion) Restrictions.Eq("ReadingValues.CalculationStart", (object) (long) x.CalculationStart));
        if (x.MeterType != 0)
          conjunction.Add((ICriterion) Restrictions.Eq("ReadingValues.MeterType", (object) (long) x.MeterType));
        if (x.PhysicalQuantity != 0)
          conjunction.Add((ICriterion) Restrictions.Eq("ReadingValues.PhysicalQuantity", (object) (long) x.PhysicalQuantity));
        if (x.StorageInterval != 0)
          conjunction.Add((ICriterion) Restrictions.Eq("ReadingValues.StorageInterval", (object) (long) x.StorageInterval));
        if (x.Creation != 0)
          conjunction.Add((ICriterion) Restrictions.Eq("ReadingValues.Creation", (object) (long) x.Creation));
        if (x.RuleIndex != 0)
          conjunction.Add((ICriterion) Restrictions.Eq("ReadingValues.RuleIndex", (object) (long) x.RuleIndex));
        disjunction.Add((ICriterion) conjunction);
      }));
      criteria.Add((ICriterion) disjunction);
      return criteria;
    }

    private bool ReadingValuesCriteria(MeterReadingValue readingValue, Guid meterId)
    {
      DateTime? nullable;
      int num1;
      if (this.StartDate.HasValue)
      {
        nullable = this.EndDate;
        num1 = nullable.HasValue ? 1 : 0;
      }
      else
        num1 = 0;
      if (num1 != 0)
      {
        int num2;
        if (readingValue.MeterId == meterId)
        {
          DateTime date1 = readingValue.Date;
          nullable = this.EndDate;
          if ((nullable.HasValue ? (date1 <= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
          {
            DateTime date2 = readingValue.Date;
            nullable = this.StartDate;
            num2 = nullable.HasValue ? (date2 >= nullable.GetValueOrDefault() ? 1 : 0) : 0;
            goto label_8;
          }
        }
        num2 = 0;
label_8:
        return num2 != 0;
      }
      nullable = this.StartDate;
      int num3;
      if (nullable.HasValue)
      {
        nullable = this.EndDate;
        num3 = !nullable.HasValue ? 1 : 0;
      }
      else
        num3 = 0;
      if (num3 != 0)
      {
        int num4;
        if (readingValue.MeterId == meterId)
        {
          DateTime date = readingValue.Date;
          nullable = this.StartDate;
          num4 = nullable.HasValue ? (date >= nullable.GetValueOrDefault() ? 1 : 0) : 0;
        }
        else
          num4 = 0;
        return num4 != 0;
      }
      nullable = this.StartDate;
      int num5;
      if (!nullable.HasValue)
      {
        nullable = this.EndDate;
        num5 = nullable.HasValue ? 1 : 0;
      }
      else
        num5 = 0;
      if (num5 == 0)
        return readingValue.MeterId == meterId;
      int num6;
      if (readingValue.MeterId == meterId)
      {
        DateTime date = readingValue.Date;
        nullable = this.EndDate;
        num6 = nullable.HasValue ? (date <= nullable.GetValueOrDefault() ? 1 : 0) : 0;
      }
      else
        num6 = 0;
      return num6 != 0;
    }

    private void SetRowColors(List<MeterReadingValueDTO> readingValues)
    {
      if (readingValues.Count <= 0)
        return;
      DateTime date = readingValues[0].Date;
      bool flag = true;
      foreach (MeterReadingValueDTO readingValue in readingValues)
      {
        if (readingValue.Date != date)
        {
          date = readingValue.Date;
          flag = !flag;
        }
        readingValue.IsDarkRowColor = flag;
      }
    }
  }
}
