// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.DataCollectors.DataCollectorsViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.DataCollectorsManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Client.UI.Desktop.View.DataCollectors;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Utils;
using MSS.DIConfiguration;
using MSS.DTO.Clients;
using MSS.DTO.MessageHandler;
using MSS.DTO.Minomat;
using MSS.DTO.Reporting;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Data;

#nullable disable
namespace MSS_Client.ViewModel.DataCollectors
{
  public class DataCollectorsViewModel : ValidationViewModelBase
  {
    private IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private IRepository<MSS.Core.Model.DataCollectors.Minomat> _minomatRepository;
    private ViewModelBase _messageUserControlItems;
    private ViewModelBase _messageUserControlItemsPool;
    private MinomatDTO _selectedMinomat;
    private ObservableCollection<MinomatDTO> _getDataCollectorsItems;
    private ObservableCollection<MinomatDTO> _getDataCollectorsItemsPool;
    private IEnumerable<MinomatCommunicationLogDTO> _minomatCommunicationLogs = (IEnumerable<MinomatCommunicationLogDTO>) new RadObservableCollection<MinomatCommunicationLogDTO>();
    private DateTime? _endDateLogValue;
    private DateTime? _startDateLogValue;
    private string _masterDateLogValue = string.Empty;
    private string _pageSizePool;
    private string _pageSize;
    private bool _isDataCollectorsItemsSelected;
    private int _selectedIndex;
    private bool _isDataCollectorsItemsPoolSelected;
    private bool _isBusy;
    private bool _masterPoolAddVisibility;
    private bool _masterPoolDeleteVisibility;
    private bool _isMasterPoolTabVisible;
    private bool _addMinomatVisibility;
    private bool _editMinomatVisibility;
    private bool _deleteMinomatVisibility;

    [Inject]
    public DataCollectorsViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this.IsBusy = true;
      this._windowFactory = windowFactory;
      this._repositoryFactory = repositoryFactory;
      this._minomatRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>();
      UsersManager usersManager = new UsersManager(this._repositoryFactory);
      this.AddMinomatVisibility = usersManager.HasRight(OperationEnum.DataCollectorAdd.ToString());
      this.EditMinomatVisibility = usersManager.HasRight(OperationEnum.DataCollectorEdit.ToString());
      this.DeleteMinomatVisibility = usersManager.HasRight(OperationEnum.DataCollectorDelete.ToString());
      this.MasterPoolAddVisibility = usersManager.HasRight(OperationEnum.MasterPoolAdd.ToString());
      this.MasterPoolDeleteVisibility = usersManager.HasRight(OperationEnum.MasterPoolDelete.ToString());
      this.IsMasterPoolTabVisible = usersManager.HasRight(OperationEnum.MasterPoolView.ToString());
      EventPublisher.Register<ActionSearch<MinomatDTO>>(new Action<ActionSearch<MinomatDTO>>(this.RefreshMinomatsAfterSearch));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      EventPublisher.Register<MinomatUpdate>(new Action<MinomatUpdate>(this.RefreshMinomats));
      EventPublisher.Register<SelectedTabValue>(new Action<SelectedTabValue>(this.SetTab));
      this._pageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this._pageSizePool = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this.InitializeMinomats();
    }

    public ViewModelBase MessageUserControlItems
    {
      get => this._messageUserControlItems;
      set
      {
        this._messageUserControlItems = value;
        this.OnPropertyChanged(nameof (MessageUserControlItems));
      }
    }

    public ViewModelBase MessageUserControlItemsPool
    {
      get => this._messageUserControlItemsPool;
      set
      {
        this._messageUserControlItemsPool = value;
        this.OnPropertyChanged(nameof (MessageUserControlItemsPool));
      }
    }

    private void SetTab(SelectedTabValue selectedTabValue)
    {
      switch (selectedTabValue.Tab)
      {
        case ApplicationTabsEnum.DataCollectors:
          this.SelectedIndex = 0;
          break;
        case ApplicationTabsEnum.DataCollectorsPool:
          this.SelectedIndex = 1;
          break;
      }
    }

    public MinomatDTO SelectedMinomat
    {
      get => this._selectedMinomat;
      set
      {
        this._selectedMinomat = value;
        this.OnPropertyChanged(nameof (SelectedMinomat));
        this.OnPropertyChanged("EnableLoggingtVisibility");
        this.OnPropertyChanged("DisableLoggingVisibility");
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

    public ObservableCollection<MinomatDTO> GetDataCollectorsItems
    {
      get => this._getDataCollectorsItems;
      set
      {
        this._getDataCollectorsItems = value;
        this.OnPropertyChanged(nameof (GetDataCollectorsItems));
      }
    }

    public ObservableCollection<MinomatDTO> GetDataCollectorsItemsPool
    {
      get => this._getDataCollectorsItemsPool;
      set
      {
        this._getDataCollectorsItemsPool = value;
        this.OnPropertyChanged(nameof (GetDataCollectorsItemsPool));
      }
    }

    public IEnumerable<MinomatCommunicationLogDTO> MinomatCommunicationLogs
    {
      get => this._minomatCommunicationLogs;
      set
      {
        this._minomatCommunicationLogs = value;
        this.OnPropertyChanged(nameof (MinomatCommunicationLogs));
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

    public string MasterNumberValue
    {
      get => this._masterDateLogValue;
      set
      {
        this._masterDateLogValue = value;
        this.OnPropertyChanged(nameof (MasterNumberValue));
      }
    }

    public string PageSizePool
    {
      get => this._pageSizePool;
      set
      {
        this._pageSizePool = value;
        this.OnPropertyChanged(nameof (PageSizePool));
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

    public bool IsDataCollectorsItemsSelected
    {
      get => this._isDataCollectorsItemsSelected;
      set
      {
        this._isDataCollectorsItemsSelected = value;
        if (!this._isDataCollectorsItemsSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.DataCollectors
        }, (IViewModel) this);
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

    public bool IsDataCollectorsItemsPoolSelected
    {
      get => this._isDataCollectorsItemsPoolSelected;
      set
      {
        this._isDataCollectorsItemsPoolSelected = value;
        if (!this._isDataCollectorsItemsPoolSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.DataCollectorsPool
        }, (IViewModel) this);
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

    public bool MasterPoolAddVisibility
    {
      get => this._masterPoolAddVisibility;
      set
      {
        this._masterPoolAddVisibility = value;
        this.OnPropertyChanged(nameof (MasterPoolAddVisibility));
      }
    }

    public bool MasterPoolDeleteVisibility
    {
      get => this._masterPoolDeleteVisibility;
      set
      {
        this._masterPoolDeleteVisibility = value;
        this.OnPropertyChanged(nameof (MasterPoolDeleteVisibility));
      }
    }

    public bool IsMasterPoolTabVisible
    {
      get => this._isMasterPoolTabVisible;
      set
      {
        this._isMasterPoolTabVisible = value;
        this.OnPropertyChanged(nameof (IsMasterPoolTabVisible));
      }
    }

    private void RefreshMinomats(MinomatUpdate minomatUpdate)
    {
      minomatUpdate.Ids.ForEach((Action<Guid>) (x => this._repositoryFactory.GetRepository<MSS.Core.Model.DataCollectors.Minomat>().Refresh((object) x)));
    }

    private void RefreshMinomatsAfterSearch(ActionSearch<MinomatDTO> update)
    {
      bool flag = update.Message == null;
      switch (update.SelectedTab)
      {
        case ApplicationTabsEnum.DataCollectors:
          this.GetDataCollectorsItems = update.ObservableCollection.Count == 0 ? this.GetDataCollectorsManagerInstance().GetMinomatDTOs() : update.ObservableCollection;
          break;
        case ApplicationTabsEnum.DataCollectorsPool:
          this.GetDataCollectorsItemsPool = update.ObservableCollection.Count == 0 ? this.GetDataCollectorsManagerInstance().GetMinomatPoolDTOs() : update.ObservableCollection;
          break;
      }
      if (flag)
        return;
      if (this.IsDataCollectorsItemsSelected)
        this.MessageUserControlItems = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
      if (!this.IsDataCollectorsItemsPoolSelected)
        return;
      this.MessageUserControlItemsPool = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
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
      if (this.IsDataCollectorsItemsSelected)
        this.MessageUserControlItems = viewModelBase;
      if (!this.IsDataCollectorsItemsPoolSelected)
        return;
      this.MessageUserControlItemsPool = viewModelBase;
    }

    protected DataCollectorsManager GetDataCollectorsManagerInstance()
    {
      return new DataCollectorsManager(this._repositoryFactory);
    }

    public ICommand SearchMinomatCommunicationLogs
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
          backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            MSS.Core.Model.DataCollectors.Minomat master = this._minomatRepository.FirstOrDefault((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, bool>>) (m => m.RadioId == this.MasterNumberValue && m.GsmId != default (string)));
            Expression<Func<MinomatConnectionLog, bool>> expression = PredicateBuilder.True<MinomatConnectionLog>();
            if (master != null)
              expression = expression.And<MinomatConnectionLog>((Expression<Func<MinomatConnectionLog, bool>>) (log => log.MinomatId == (Guid?) master.Id));
            DateTime? nullable = this.StartDateLogValue;
            if (nullable.HasValue)
              expression = expression.And<MinomatConnectionLog>((Expression<Func<MinomatConnectionLog, bool>>) (l => (DateTime?) l.TimePoint >= this.StartDateLogValue));
            nullable = this.EndDateLogValue;
            if (nullable.HasValue)
              expression = expression.And<MinomatConnectionLog>((Expression<Func<MinomatConnectionLog, bool>>) (l => (DateTime?) l.TimePoint <= this.EndDateLogValue));
            this.MinomatCommunicationLogs = (IEnumerable<MinomatCommunicationLogDTO>) new RadObservableCollection<MinomatCommunicationLogDTO>((IEnumerable<MinomatCommunicationLogDTO>) this._repositoryFactory.GetRepository<MinomatConnectionLog>().SearchFor(expression).Select<MinomatConnectionLog, MinomatCommunicationLogDTO>((Func<MinomatConnectionLog, MinomatCommunicationLogDTO>) (connLog => new MinomatCommunicationLogDTO()
            {
              MasterRadioId = master != null ? master.RadioId : string.Empty,
              GsmID = connLog.GsmID,
              ChallengeKey = connLog.ChallengeKey,
              TimePoint = connLog.TimePoint,
              SessionKey = connLog.SessionKey
            })).ToList<MinomatCommunicationLogDTO>());
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
            {
              if (this.MinomatCommunicationLogs.Count<MinomatCommunicationLogDTO>() == 0)
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

    public ICommand AddDataCollectorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<AddToMasterPoolViewModel>());
          if (newModalDialog.HasValue)
            this.MessageUserControlItemsPool = newModalDialog.Value ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation) : MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this.GetDataCollectorsItems = this.GetDataCollectorsManagerInstance().GetMinomatDTOs();
          this.GetDataCollectorsItemsPool = this.GetDataCollectorsManagerInstance().GetMinomatPoolDTOs();
        }));
      }
    }

    public ICommand RemoveDataCollectorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          IKernel configurator = DIConfigurator.GetConfigurator();
          RemoveDataCollector removeDataCollector = new RemoveDataCollector()
          {
            Owner = Application.Current.Windows[0],
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            DataContext = (object) configurator.Get<MasterPoolViewModel>()
          };
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<RemoveFromMasterPoolViewModel>());
          if (newModalDialog.HasValue)
            this.MessageUserControlItemsPool = newModalDialog.Value ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation) : MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this.GetDataCollectorsItems = this.GetDataCollectorsManagerInstance().GetMinomatDTOs();
          this.GetDataCollectorsItemsPool = this.GetDataCollectorsManagerInstance().GetMinomatPoolDTOs();
        }));
      }
    }

    public async void InitializeMinomats()
    {
      await Task.Run((Action) (() =>
      {
        Mapper.CreateMap<MSS.Core.Model.DataCollectors.Minomat, MinomatDTO>().ForMember((Expression<Func<MinomatDTO, object>>) (x => (object) x.idEnumStatus), (Action<IMemberConfigurationExpression<MSS.Core.Model.DataCollectors.Minomat>>) (y => y.ResolveUsing((Func<MSS.Core.Model.DataCollectors.Minomat, object>) (x => x.Status != null ? (object) (int) Enum.Parse(typeof (StatusMinomatEnum), x.Status) : (object) 0))));
        Mapper.CreateMap<MinomatDTO, MSS.Core.Model.DataCollectors.Minomat>().ForMember((Expression<Func<MSS.Core.Model.DataCollectors.Minomat, object>>) (x => x.Status), (Action<IMemberConfigurationExpression<MinomatDTO>>) (y => y.ResolveUsing((Func<MinomatDTO, object>) (x => (object) (StatusMinomatEnum) x.idEnumStatus))));
        DataCollectorsManager collectorsManager = new DataCollectorsManager(this._repositoryFactory);
        this.GetDataCollectorsItems = collectorsManager.GetMinomatDTOs();
        this.GetDataCollectorsItemsPool = collectorsManager.GetMinomatPoolDTOs();
      }));
      this.IsBusy = false;
    }

    public ICommand EnableLoggingCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (param =>
        {
          if (param is MinomatDTO minomatDto2)
          {
            MSS.Core.Model.DataCollectors.Minomat byId = this._minomatRepository.GetById((object) minomatDto2.Id);
            if (byId != null)
            {
              byId.LoggingEnabled = true;
              this._minomatRepository.Update(byId);
              minomatDto2.LoggingEnabled = true;
            }
          }
          this.OnPropertyChanged("EnableLoggingtVisibility");
          this.OnPropertyChanged("DisableLoggingVisibility");
        }));
      }
    }

    public ICommand DisableLoggingCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (param =>
        {
          if (param is MinomatDTO minomatDto2)
          {
            MSS.Core.Model.DataCollectors.Minomat byId = this._minomatRepository.GetById((object) minomatDto2.Id);
            if (byId != null)
            {
              byId.LoggingEnabled = false;
              this._minomatRepository.Update(byId);
              minomatDto2.LoggingEnabled = false;
            }
          }
          this.OnPropertyChanged("EnableLoggingtVisibility");
          this.OnPropertyChanged("DisableLoggingVisibility");
        }));
      }
    }

    public ICommand ViewStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (param =>
        {
          if (!(param is MinomatDTO minomatDto2))
            return;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<StructureMinomatViewModel>((IParameter) new ConstructorArgument("minomat", (object) minomatDto2)));
        }));
      }
    }

    public ICommand CreateDataCollectionCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateDataCollectorsViewModel>());
          if (newModalDialog.HasValue)
            this.MessageUserControlItems = newModalDialog.Value ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation) : MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this.GetDataCollectorsItems = this.GetDataCollectorsManagerInstance().GetMinomatDTOs();
          this.GetDataCollectorsItemsPool = this.GetDataCollectorsManagerInstance().GetMinomatPoolDTOs();
        }));
      }
    }

    public ICommand DeleteDataCollectorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MinomatDTO minomatDto = (MinomatDTO) _;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DeleteDataCollectorViewModel>((IParameter) new ConstructorArgument("minomat", (object) minomatDto)));
          if (!newModalDialog.HasValue)
            return;
          this.MessageUserControlItems = newModalDialog.Value ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation) : MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this.GetDataCollectorsItems = this.GetDataCollectorsManagerInstance().GetMinomatDTOs();
          this.GetDataCollectorsItemsPool = this.GetDataCollectorsManagerInstance().GetMinomatPoolDTOs();
        }));
      }
    }

    public ICommand EditDataCollectorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MinomatDTO minomatDto = (MinomatDTO) _;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditDataCollectorViewModel>((IParameter) new ConstructorArgument("minomat", (object) minomatDto)));
          if (newModalDialog.HasValue)
            this.MessageUserControlItems = newModalDialog.Value ? MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation) : MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this.GetDataCollectorsItems = this.GetDataCollectorsManagerInstance().GetMinomatDTOs();
          this.GetDataCollectorsItemsPool = this.GetDataCollectorsManagerInstance().GetMinomatPoolDTOs();
        }));
      }
    }

    public RadObservableCollection<EnumObj> GetListofStatuses
    {
      get
      {
        RadObservableCollection<EnumObj> getListofStatuses = new RadObservableCollection<EnumObj>();
        IEnumerable<StatusMinomatEnum> source1 = Enum.GetValues(typeof (StatusMinomatEnum)).Cast<StatusMinomatEnum>();
        if (!(source1 is StatusMinomatEnum[] statusMinomatEnumArray))
          statusMinomatEnumArray = source1.ToArray<StatusMinomatEnum>();
        StatusMinomatEnum[] source2 = statusMinomatEnumArray;
        if (((IEnumerable<StatusMinomatEnum>) source2).Count<StatusMinomatEnum>() == 0)
          return getListofStatuses;
        for (int index = 0; index < ((IEnumerable<StatusMinomatEnum>) source2).Count<StatusMinomatEnum>(); ++index)
        {
          switch (source2[index])
          {
            case StatusMinomatEnum.New:
              EnumObj enumObj1 = new EnumObj()
              {
                IdEnum = index,
                StatusFromObj = Resources.MSS_MINOMAT_STATUS_NEW
              };
              getListofStatuses.Add(enumObj1);
              break;
            case StatusMinomatEnum.BuiltIn:
              EnumObj enumObj2 = new EnumObj()
              {
                IdEnum = index,
                StatusFromObj = Resources.MSS_MINOMAT_STATUS_BUILTIN
              };
              getListofStatuses.Add(enumObj2);
              break;
          }
        }
        return getListofStatuses;
      }
    }

    public bool AddMinomatVisibility
    {
      get => this._addMinomatVisibility;
      set
      {
        this._addMinomatVisibility = value;
        this.OnPropertyChanged(nameof (AddMinomatVisibility));
      }
    }

    public bool EditMinomatVisibility
    {
      get => this._editMinomatVisibility;
      set
      {
        this._editMinomatVisibility = value;
        this.OnPropertyChanged(nameof (EditMinomatVisibility));
      }
    }

    public bool DeleteMinomatVisibility
    {
      get => this._deleteMinomatVisibility;
      set
      {
        this._deleteMinomatVisibility = value;
        this.OnPropertyChanged(nameof (DeleteMinomatVisibility));
      }
    }

    public bool EnableLoggingtVisibility
    {
      get
      {
        return this.SelectedMinomat != null && this.SelectedMinomat.IsMaster && !this.SelectedMinomat.LoggingEnabled;
      }
      set => this.OnPropertyChanged(nameof (EnableLoggingtVisibility));
    }

    public bool DisableLoggingVisibility
    {
      get
      {
        return this.SelectedMinomat != null && this.SelectedMinomat.IsMaster && this.SelectedMinomat.LoggingEnabled;
      }
      set => this.OnPropertyChanged(nameof (DisableLoggingVisibility));
    }
  }
}
