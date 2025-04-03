// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.StructuresViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using GMMToMSSMigrator;
using Microsoft.Win32;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.Reporting;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Reporting;
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MSS_Client.ViewModel.Meters;
using MSS_Client.ViewModel.RadioTest;
using MVVM.Commands;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class StructuresViewModel : MVVM.ViewModel.ViewModelBase
  {
    private readonly IRepository<Location> _locationRepository;
    private readonly IRepository<Tenant> _tenantRepository;
    private readonly IRepository<MSS.Core.Model.Meters.Meter> _meterRepository;
    private readonly IRepository<Minomat> _minomatRepository;
    private readonly IRepository<MeterReadingValue> _meterReadingValueRepository;
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private bool _canViewMeter;
    private int _selectedIndex;
    private ApplicationTabsEnum _selectedTab;
    private bool _isExpanded = false;
    private bool _logicalIsExpanded = false;
    private bool _fixedIsExpanded = false;
    private bool _structureVisibility;
    private MVVM.ViewModel.ViewModelBase _messageUserControlPhysical;
    private MVVM.ViewModel.ViewModelBase _messageUserControlLogical;
    private MVVM.ViewModel.ViewModelBase _messageUserControlFixed;
    private string _pageSize = string.Empty;
    private ObservableCollection<StructureNodeDTO> _structureNodeCollection;
    private bool _isBusy;
    private string _folderPath;
    private StructureNodeDTO _selectedItem;
    private bool _isRootItemSelected;
    private bool _isPhysicalTabSelected;
    private ObservableCollection<StructureNodeDTO> _logicalStructureNodeCollection;
    private StructureNodeDTO _selectedLogicalItem;
    private bool _isRootLogicalItemSelected;
    private bool _isLogicalTabSelected;
    private ObservableCollection<StructureNodeDTO> _fixedStructureNodeCollection;
    private StructureNodeDTO _selectedFixedItem;
    private bool _isRootFixedItemSelected;
    private bool _isFixedTabSelected;
    private bool _installationOrderAttachTestVisibility;
    private bool _createPhysicalStructuresVisibility;
    private bool _editPhysicalStructuresVisibility;
    private bool _removePhysicalStructuresVisibility;
    private bool _deletePhysicalStructuresVisibility;
    private bool _importPhysicalStructuresVisibility;
    private bool _exportPhysicalStructuresVisibility;
    private bool _readingValuesVisibility;
    private bool _createLogicalStructuresVisibility;
    private bool _editLogicalStructuresVisibility;
    private bool _removeLogicalStructuresVisibility;
    private bool _deleteLogicalStructuresVisibility;
    private bool _importLogicalStructuresVisibility;
    private bool _exportLogicalStructuresVisibility;
    private bool _createFixedStructuresVisibility;
    private bool _editFixedStructuresVisibility;
    private bool _removeFixedStructuresVisibility;
    private bool _deleteFixedStructuresVisibility;
    private bool _importFixedStructuresVisibility;
    private bool _exportFixedStructuresVisibility;

    [Inject]
    public StructuresViewModel(IRepositoryFactory repositoryFactory, IWindowFactory windowFactory)
    {
      this.IsBusy = true;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._meterRepository = repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>();
      this._locationRepository = repositoryFactory.GetRepository<Location>();
      this._tenantRepository = repositoryFactory.GetRepository<Tenant>();
      this._meterReadingValueRepository = repositoryFactory.GetRepository<MeterReadingValue>();
      this._minomatRepository = repositoryFactory.GetRepository<Minomat>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      EventPublisher.Register<StructureUpdated>(new Action<StructureUpdated>(this.RefreshStructure));
      EventPublisher.Register<ActionSearch<StructureNodeDTO>>(new Action<ActionSearch<StructureNodeDTO>>(this.RefreshStructuresAfterSearch));
      EventPublisher.Register<GridShouldBeUpdated>(new Action<GridShouldBeUpdated>(this.RefreshStructuresAfterSync));
      EventPublisher.Register<RefreshFixedStructuresEvent>(new Action<RefreshFixedStructuresEvent>(this.RefreshStructureAfterSettingEvFactor));
      EventPublisher.Register<MSS.Business.Events.ShowMessage>(new Action<MSS.Business.Events.ShowMessage>(this.ShowCancelMessage));
      this.IsRemoveVisible = false;
      UsersManager usersManager = new UsersManager(this._repositoryFactory);
      this.CreatePhysicalStructuresVisibility = usersManager.HasRight(OperationEnum.PhysicalStructureCreate.ToString());
      this.EditPhysicalStructuresVisibility = usersManager.HasRight(OperationEnum.PhysicalStructureEdit.ToString());
      this.DeletePhysicalStructuresVisibility = usersManager.HasRight(OperationEnum.PhysicalStructureDelete.ToString());
      this.RemovePhysicalStructuresVisibility = usersManager.HasRight(OperationEnum.PhysicalStructureRemove.ToString());
      this.ImportPhysicalStructuresVisibility = usersManager.HasRight(OperationEnum.PhysicalStructureImport.ToString());
      this.ExportPhysicalStructuresVisibility = usersManager.HasRight(OperationEnum.PhysicalStructureExport.ToString());
      this.CreateLogicalStructuresVisibility = usersManager.HasRight(OperationEnum.LogicalStructureCreate.ToString());
      this.EditLogicalStructuresVisibility = usersManager.HasRight(OperationEnum.LogicalStructureEdit.ToString());
      this.DeleteLogicalStructuresVisibility = usersManager.HasRight(OperationEnum.LogicalStructureDelete.ToString());
      this.RemoveLogicalStructuresVisibility = usersManager.HasRight(OperationEnum.LogicalStructureRemove.ToString());
      this.ImportLogicalStructuresVisibility = usersManager.HasRight(OperationEnum.LogicalStructureImport.ToString());
      this.ExportLogicalStructuresVisibility = usersManager.HasRight(OperationEnum.LogicalStructureExport.ToString());
      this.CreateFixedStructuresVisibility = usersManager.HasRight(OperationEnum.FixedStructureCreate.ToString());
      this.EditFixedStructuresVisibility = usersManager.HasRight(OperationEnum.FixedStructureEdit.ToString());
      this.DeleteFixedStructuresVisibility = usersManager.HasRight(OperationEnum.FixedStructureDelete.ToString());
      this.RemoveFixedStructuresVisibility = usersManager.HasRight(OperationEnum.FixedStructureRemove.ToString());
      this.ImportFixedStructuresVisibility = usersManager.HasRight(OperationEnum.FixedStructureImport.ToString());
      this.ExportFixedStructuresVisibility = usersManager.HasRight(OperationEnum.FixedStructureExport.ToString());
      this.ExportFixedStructureDevicesVisibility = usersManager.HasRight(OperationEnum.FixedStructureExport.ToString());
      this.InstallationOrderAttachTestVisibility = usersManager.HasRight(OperationEnum.InstallationOrderAttachTest.ToString());
      this.UnlockStructureVisibility = usersManager.HasRight(OperationEnum.UnlockStructure.ToString());
      this.ReadingValuesVisibility = usersManager.HasRight(OperationEnum.ReadingDataView.ToString());
      this.IsPhysicalTabVisible = usersManager.HasRight(OperationEnum.PhysicalStructureView.ToString());
      this.IsLogicalTabVisible = usersManager.HasRight(OperationEnum.LogicalStructureView.ToString());
      this.IsFixedTabVisible = usersManager.HasRight(OperationEnum.FixedStructureView.ToString());
      this.EvaluationFactorVisibility = LicenseHelper.LicenseIsDisplayEvaluationFactor();
      this.PageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      this._canViewMeter = usersManager.HasRight(OperationEnum.MeterView.ToString());
      if (this.IsPhysicalTabVisible)
        this.SelectedIndex = 0;
      else if (this.IsLogicalTabVisible)
        this.SelectedIndex = 1;
      else if (this.IsFixedTabVisible)
        this.SelectedIndex = 2;
      this.InitializeStructures();
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      EventPublisher.Register<SelectedTabChanged>((Action<SelectedTabChanged>) (changed => this.SelectedTab = changed.SelectedTab));
      EventPublisher.Register<SelectedTabValue>(new Action<SelectedTabValue>(this.SetTab));
      EventPublisher.Register<AttachTestConfigMessage>(new Action<AttachTestConfigMessage>(this.CreateMessage));
      EventPublisher.Register<LoadSubNodesForRoot>(new Action<LoadSubNodesForRoot>(this.LoadSubNodesForRootNodeEvent));
    }

    private void LoadSubNodesForRootNodeEvent(LoadSubNodesForRoot rootNodeEv)
    {
      this.IsBusy = true;
      BackgroundWorker backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
      {
        StructureNodeDTO rootNode = rootNodeEv.RootNode;
        List<object> objectList = args.Argument as List<object>;
        this.LoadSubNodesForRootNode(objectList[0].As<IRepositoryFactory>(), objectList[1].As<IList<MSS.Core.Model.Structures.StructureNodeType>>(), objectList[2].As<IList<MeterReplacementHistory>>(), rootNode);
      });
      backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
      {
        rootNodeEv.RootNode.SubNodes = new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) rootNodeEv.RootNode.SubNodes.OrderBy<StructureNodeDTO, int>((Func<StructureNodeDTO, int>) (structure => !(structure.Entity is TenantDTO entity2) ? structure.OrderNr : entity2.TenantNr)));
        if (!args.Cancelled && args.Error != null)
        {
          MSS.Business.Errors.MessageHandler.LogException(args.Error);
          MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
        }
        Dispatcher.CurrentDispatcher.Invoke((Action) (() => this.IsBusy = false));
      });
      List<object> objectList1 = new List<object>()
      {
        (object) this._repositoryFactory,
        (object) this._repositoryFactory.GetRepository<MSS.Core.Model.Structures.StructureNodeType>().GetAll(),
        (object) this._repositoryFactory.GetRepository<MeterReplacementHistory>().GetAll()
      };
      backgroundWorker.RunWorkerAsync((object) objectList1);
    }

    public void LoadSubNodesForRootNode(
      IRepositoryFactory repositoryFactory,
      IList<MSS.Core.Model.Structures.StructureNodeType> structureNodeTypeList,
      IList<MeterReplacementHistory> meterReplacementHistoryList,
      StructureNodeDTO rootNode,
      bool canViewMeters = true)
    {
      StructureTypeEnum? structureType = rootNode.StructureType;
      ObservableCollection<StructureNodeDTO> collectionWithChildren = this.GetStructuresManagerInstance().GetNodeCollectionWithChildren(repositoryFactory.GetStructureNodeRepository(), structureType, structureNodeTypeList, meterReplacementHistoryList, rootNode.Id);
      if (rootNode.NodeType.Name == "Location")
      {
        Guid? entityId = repositoryFactory.GetRepository<StructureNode>().FirstOrDefault((Expression<Func<StructureNode, bool>>) (item => item.Id == rootNode.Id && item.EndDate == new DateTime?()))?.EntityId;
        if (entityId.HasValue)
        {
          object entity = (object) StructuresHelper.GetEntity<Location>(entityId.Value, repositoryFactory.GetSession());
          rootNode.Entity = StructuresHelper.GetEntityDTO(StructureNodeTypeEnum.Location, entity);
        }
      }
      if (collectionWithChildren.Count <= 0)
        return;
      TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) collectionWithChildren.First<StructureNodeDTO>().SubNodes, (Action<StructureNodeDTO>) (subnode =>
      {
        if (!(subnode.NodeType.Name == "Meter") && !(subnode.NodeType.Name == "RadioMeter"))
          return;
        Guid? entityId = this._repositoryFactory.GetRepository<StructureNode>().FirstOrDefault((Expression<Func<StructureNode, bool>>) (item => item.Id == subnode.Id && item.EndDate == new DateTime?()))?.EntityId;
        if (entityId.HasValue)
        {
          object entity = (object) StructuresHelper.GetEntity<MSS.Core.Model.Meters.Meter>(entityId.Value, this._repositoryFactory.GetSession());
          subnode.Entity = StructuresHelper.GetEntityDTO(subnode.NodeType.Name == "Meter" ? StructureNodeTypeEnum.Meter : StructureNodeTypeEnum.RadioMeter, entity);
        }
      }));
      rootNode.SubNodes = collectionWithChildren.First<StructureNodeDTO>().SubNodes;
    }

    public void LoadSubNodesForRootNode(StructureNodeDTO rootNode, bool canViewMeters = true)
    {
      this.LoadSubNodesForRootNode(this._repositoryFactory, this._repositoryFactory.GetRepository<MSS.Core.Model.Structures.StructureNodeType>().GetAll(), this._repositoryFactory.GetRepository<MeterReplacementHistory>().GetAll(), rootNode, canViewMeters);
    }

    private void RefreshStructureAfterSettingEvFactor(RefreshFixedStructuresEvent ev)
    {
      this._repositoryFactory.GetSession().Clear();
      this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
    }

    public object SoapToFromFile(string filePath)
    {
      FileStream serializationStream = (FileStream) null;
      object fromFile;
      try
      {
        serializationStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        fromFile = new SoapFormatter()
        {
          Binder = ((SerializationBinder) new CustomSerializationBinder())
        }.Deserialize((Stream) serializationStream);
      }
      catch (Exception ex)
      {
        throw new BaseApplicationException(Resources.MSS_SAS_IMPORT_ERROR_XmlFileFormat, ex);
      }
      finally
      {
        serializationStream?.Close();
      }
      return fromFile;
    }

    private void CreateMessage(AttachTestConfigMessage messageFinished)
    {
      MVVM.ViewModel.ViewModelBase viewModelBase = (MVVM.ViewModel.ViewModelBase) null;
      switch (messageFinished.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          viewModelBase = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          viewModelBase = MessageHandlingManager.ShowWarningMessage(messageFinished.Message.MessageText);
          break;
      }
      if (this.IsFixedTabSelected)
        this.MessageUserControlFixed = viewModelBase;
      if (this.IsLogicalTabSelected)
        this.MessageUserControlLogical = viewModelBase;
      if (!this.IsPhysicalTabSelected)
        return;
      this.MessageUserControlPhysical = viewModelBase;
    }

    private void SetTab(SelectedTabValue selectedTabValue)
    {
      switch (selectedTabValue.Tab)
      {
        case ApplicationTabsEnum.StructuresPhysical:
          this.SelectedIndex = 0;
          break;
        case ApplicationTabsEnum.StructuresLogical:
          this.SelectedIndex = 1;
          break;
        case ApplicationTabsEnum.StructuresFixed:
          this.SelectedIndex = 2;
          break;
      }
    }

    private void CreateMessage(ActionSyncFinished messageFinished)
    {
      MVVM.ViewModel.ViewModelBase viewModelBase = (MVVM.ViewModel.ViewModelBase) null;
      switch (messageFinished.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          viewModelBase = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          viewModelBase = MessageHandlingManager.ShowWarningMessage(messageFinished.Message.MessageText);
          break;
      }
      if (this.IsFixedTabSelected)
        this.MessageUserControlFixed = viewModelBase;
      if (this.IsLogicalTabSelected)
        this.MessageUserControlLogical = viewModelBase;
      if (!this.IsPhysicalTabSelected)
        return;
      this.MessageUserControlPhysical = viewModelBase;
    }

    private void RefreshStructuresAfterSearch(ActionSearch<StructureNodeDTO> update)
    {
      bool flag = update.Message == null;
      switch (update.SelectedTab)
      {
        case ApplicationTabsEnum.StructuresPhysical:
          this.StructureNodeCollection = update.ObservableCollection.Count == 0 ? this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical) : update.ObservableCollection;
          this.StructureExpanded = flag;
          break;
        case ApplicationTabsEnum.StructuresLogical:
          this.LogicalStructureNodeCollection = update.ObservableCollection.Count == 0 ? this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical) : update.ObservableCollection;
          this.LogicalStructureExpanded = flag;
          break;
        case ApplicationTabsEnum.StructuresFixed:
          this.FixedStructureNodeCollection = update.ObservableCollection.Count == 0 ? this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true) : update.ObservableCollection;
          this.FixedStructureExpanded = flag;
          break;
      }
      if (flag)
        return;
      if (this.IsFixedTabSelected)
        this.MessageUserControlFixed = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
      if (this.IsLogicalTabSelected)
        this.MessageUserControlLogical = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
      if (!this.IsPhysicalTabSelected)
        return;
      this.MessageUserControlPhysical = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
    }

    private void RefreshStructuresAfterSync(GridShouldBeUpdated args)
    {
      this._repositoryFactory.GetSession().Clear();
      this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
    }

    private void RefreshStructure(StructureUpdated update)
    {
      if (update.Guid != Guid.Empty)
      {
        this._structureNodeRepository.Refresh((object) update.Guid);
        this._structureNodeLinksRepository.Refresh((object) update.LinkGuid);
      }
      else
      {
        this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
        this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
        this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed);
      }
      if (update.EntityId != Guid.Empty)
      {
        switch (update.EntityType)
        {
          case StructureNodeTypeEnum.Location:
            this._locationRepository.Refresh((object) update.EntityId);
            break;
          case StructureNodeTypeEnum.Tenant:
            this._tenantRepository.Refresh((object) update.EntityId);
            break;
          case StructureNodeTypeEnum.Meter:
          case StructureNodeTypeEnum.RadioMeter:
            this._meterRepository.Refresh((object) update.EntityId);
            break;
          case StructureNodeTypeEnum.MinomatMaster:
          case StructureNodeTypeEnum.MinomatSlave:
            this._minomatRepository.Refresh((object) update.EntityId);
            break;
        }
      }
      update.RootNode?.LoadChildren();
      this.ShowActionMessage(update.Message);
    }

    private void ShowCancelMessage(MSS.Business.Events.ShowMessage message)
    {
      this.ShowActionMessage(message.Message);
    }

    private void ShowActionMessage(MSS.DTO.Message.Message message)
    {
      MVVM.ViewModel.ViewModelBase viewModelBase = (MVVM.ViewModel.ViewModelBase) null;
      switch (message.MessageType)
      {
        case MessageTypeEnum.Success:
          viewModelBase = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_MessageCodes_SuccessOperation);
          break;
        case MessageTypeEnum.Warning:
          viewModelBase = MessageHandlingManager.ShowWarningMessage(message.MessageText);
          break;
      }
      if (this.IsFixedTabSelected)
        this.MessageUserControlFixed = viewModelBase;
      if (this.IsLogicalTabSelected)
        this.MessageUserControlLogical = viewModelBase;
      if (!this.IsPhysicalTabSelected)
        return;
      this.MessageUserControlPhysical = viewModelBase;
    }

    private StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    private ReportingManager GetReportingManagerInstance()
    {
      return new ReportingManager(this._repositoryFactory);
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

    private void SetSelectedTab()
    {
      switch (this.SelectedTab)
      {
        case ApplicationTabsEnum.StructuresPhysical:
          this._isPhysicalTabSelected = true;
          break;
        case ApplicationTabsEnum.StructuresLogical:
          this._isLogicalTabSelected = true;
          break;
        case ApplicationTabsEnum.StructuresFixed:
          this._isFixedTabSelected = true;
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

    public bool StructureExpanded
    {
      get => this._isExpanded;
      set
      {
        this._isExpanded = value;
        this.OnPropertyChanged(nameof (StructureExpanded));
      }
    }

    public bool LogicalStructureExpanded
    {
      get => this._logicalIsExpanded;
      set
      {
        this._logicalIsExpanded = value;
        this.OnPropertyChanged(nameof (LogicalStructureExpanded));
      }
    }

    public bool FixedStructureExpanded
    {
      get => this._fixedIsExpanded;
      set
      {
        this._fixedIsExpanded = value;
        this.OnPropertyChanged(nameof (FixedStructureExpanded));
      }
    }

    public bool StructuresVisibility
    {
      get => this._structureVisibility;
      set
      {
        this._structureVisibility = value;
        this.OnPropertyChanged(nameof (StructuresVisibility));
      }
    }

    public MVVM.ViewModel.ViewModelBase MessageUserControlPhysical
    {
      get => this._messageUserControlPhysical;
      set
      {
        this._messageUserControlPhysical = value;
        this.OnPropertyChanged(nameof (MessageUserControlPhysical));
      }
    }

    public MVVM.ViewModel.ViewModelBase MessageUserControlLogical
    {
      get => this._messageUserControlLogical;
      set
      {
        this._messageUserControlLogical = value;
        this.OnPropertyChanged(nameof (MessageUserControlLogical));
      }
    }

    public MVVM.ViewModel.ViewModelBase MessageUserControlFixed
    {
      get => this._messageUserControlFixed;
      set
      {
        this._messageUserControlFixed = value;
        this.OnPropertyChanged(nameof (MessageUserControlFixed));
      }
    }

    public bool UnlockStructureVisibility { get; set; }

    public string PageSize
    {
      get => this._pageSize;
      set
      {
        this._pageSize = value;
        this.OnPropertyChanged(nameof (PageSize));
      }
    }

    public ObservableCollection<StructureNodeDTO> StructureNodeCollection
    {
      get => this._structureNodeCollection;
      set
      {
        this._structureNodeCollection = value;
        this.OnPropertyChanged(nameof (StructureNodeCollection));
      }
    }

    public bool IsRemoveVisible { get; set; }

    public ICommand RemoveSelectedPhysicalStructureCommandDeleteSelectedPhysicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          IKernel configurator = DIConfigurator.GetConfigurator();
          if (radTreeListView == null)
            return;
          StructureNodeDTO currentItem = (StructureNodeDTO) radTreeListView.CurrentItem;
          ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>()
          {
            currentItem
          };
          IEnumerable<StructureNodeDTO> logicalStructure = this.GetStructuresManagerInstance().GetAffectedLogicalStructure(currentItem, StructureTypeEnum.Physical);
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<DeleteStructureViewModel>((IParameter) new ConstructorArgument("structureToDelete", (object) observableCollection), (IParameter) new ConstructorArgument("otherAffectedStructures", (object) logicalStructure)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.GetStructuresManagerInstance().RemoveStructure(currentItem, StructureTypeEnum.Physical);
            this.MessageUserControlPhysical = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Remove_Structure.GetStringValue());
          }
          else
            this.MessageUserControlPhysical = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
          this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
        }));
      }
    }

    private void ShowMessage(MVVM.ViewModel.ViewModelBase control)
    {
      if (this.IsFixedTabSelected)
        this.MessageUserControlFixed = control;
      if (this.IsLogicalTabSelected)
        this.MessageUserControlLogical = control;
      if (!this.IsPhysicalTabSelected)
        return;
      this.MessageUserControlPhysical = control;
    }

    public ICommand ImportFromFileCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          try
          {
            MVVM.ViewModel.ViewModelBase viewModelBase = (MVVM.ViewModel.ViewModelBase) null;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            DIConfigurator.GetConfigurator();
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
              Filter = "CSV Document|*.csv|XML Document|*.xml|Xcel Document|*.xlsx",
              Title = Resources.MSS_Client_ImportStructureFromFile,
              RestoreDirectory = true
            };
            bool? nullable = openFileDialog.ShowDialog();
            List<string[]> nodesList = new List<string[]>();
            if (nullable.HasValue && nullable.Value)
              viewModelBase = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
            else
              this.ShowMessage(MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue()));
            if (openFileDialog.FileName == string.Empty)
              return;
            switch (openFileDialog.FilterIndex)
            {
              case 1:
                nodesList = new CSVManager().ReadStructureFromFile(openFileDialog.FileName);
                break;
              case 2:
                nodesList = new XMLManager<StructureNodeDTO>().ReadStructureFromFile(openFileDialog.FileName);
                break;
              case 3:
                nodesList = new XCellManager().ReadStructureFromFile(openFileDialog.FileName);
                break;
            }
            this.ImportNodeList(nodesList);
            this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
            this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
            this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
          }
          catch
          {
            this.ShowMessage(MessageHandlingManager.ShowWarningMessage(MessageCodes.Error_Incorrect_Format.GetStringValue()));
          }
        }));
      }
    }

    public ICommand ImportTranslationRulesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          MVVM.ViewModel.ViewModelBase control = (MVVM.ViewModel.ViewModelBase) null;
          Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = "Translation Rules (*.xml)|*.xml|All files (*.*)|*.*",
            Title = "Import translation rules",
            RestoreDirectory = true
          };
          bool? nullable = openFileDialog.ShowDialog();
          if (nullable.HasValue && nullable.Value && !string.IsNullOrEmpty(openFileDialog.FileName))
          {
            try
            {
              TranslationRulesManager.ImportRulesIntoDatabase(openFileDialog.FileName);
              control = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
            }
            catch (Exception ex)
            {
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Warning_Title), (IParameter) new ConstructorArgument("message", (object) ex.Message), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
            }
          }
          else
            control = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          this.ShowMessage(control);
        }));
      }
    }

    private void ImportNodeList(List<string[]> nodesList)
    {
      if (this.GetReportingManagerInstance().ExistingRootNode(nodesList))
      {
        List<string[]> strArrayList = this.GetReportingManagerInstance().ExistingMeters(nodesList);
        if (strArrayList.Any<string[]>())
        {
          this.SaveImportedStructureWithRootAndExistingMeters(nodesList, strArrayList);
        }
        else
        {
          bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_IMPORT_STRUCTURE_EXISTS_WARNING, Resources.MSS_IMPORT_STRUCTURE_EXISTS, true);
          if (nullable.HasValue && nullable.Value)
          {
            this.GetReportingManagerInstance().SaveImportedStructure(nodesList);
            this.ShowMessage(MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue()));
          }
          else
            this.ShowMessage(MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue()));
        }
      }
      else
      {
        List<string[]> strArrayList = this.GetReportingManagerInstance().ExistingMeters(nodesList);
        if (strArrayList.Any<string[]>())
          this.SaveImportedStructureWithExistingMeters(nodesList, strArrayList);
        else
          this.GetReportingManagerInstance().SaveImportedStructure(nodesList);
      }
    }

    private List<string> GetExistingSerialNumbers(List<string[]> existingMeterList)
    {
      List<string> existingSerialNumbers = new List<string>();
      foreach (string[] existingMeter in existingMeterList)
      {
        if (existingMeter[0] == typeof (MeterDTO).Name)
          existingSerialNumbers.Add(existingMeter[1]);
      }
      return existingSerialNumbers;
    }

    private void SaveImportedStructureWithRootAndExistingMeters(
      List<string[]> nodesList,
      List<string[]> existingMeters)
    {
      string rootAndMetersExists = Resources.MSS_IMPORT_ROOT_AND_METERS_EXISTS;
      List<string> existingSerialNumbers = this.GetExistingSerialNumbers(existingMeters);
      WarningWithListBoxViewModel listBoxViewModel = DIConfigurator.GetConfigurator().Get<WarningWithListBoxViewModel>((IParameter) new ConstructorArgument("existingItems", (object) existingSerialNumbers), (IParameter) new ConstructorArgument("warningMessage", (object) rootAndMetersExists));
      MVVM.ViewModel.ViewModelBase control = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
      bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) listBoxViewModel);
      if (newModalDialog.HasValue && newModalDialog.Value)
      {
        this.GetReportingManagerInstance().SaveImportedStructure(nodesList);
        this.ShowMessage(control);
      }
      else
        this.ShowMessage(MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue()));
    }

    private void SaveImportedStructureWithExistingMeters(
      List<string[]> nodesList,
      List<string[]> existingMeters)
    {
      string importMetersExists = Resources.MSS_IMPORT_METERS_EXISTS;
      List<string> existingSerialNumbers = this.GetExistingSerialNumbers(existingMeters);
      WarningWithListBoxViewModel listBoxViewModel = DIConfigurator.GetConfigurator().Get<WarningWithListBoxViewModel>((IParameter) new ConstructorArgument("existingItems", (object) existingSerialNumbers), (IParameter) new ConstructorArgument("warningMessage", (object) importMetersExists));
      MVVM.ViewModel.ViewModelBase control = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
      bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) listBoxViewModel);
      if (newModalDialog.HasValue && newModalDialog.Value)
      {
        this.GetReportingManagerInstance().SaveImportedStructure(nodesList);
        this.ShowMessage(control);
      }
      else
      {
        foreach (string[] existingMeter in existingMeters)
          nodesList.Remove(existingMeter);
        this.GetReportingManagerInstance().SaveImportedStructure(nodesList);
        this.ShowMessage(control);
      }
    }

    public ICommand ExportToFileCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CultureInfo cultureInfo = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
          Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          ReportingManager reportingManager = new ReportingManager(this._repositoryFactory);
          if (radTreeListView == null)
            return;
          List<StructureNodeDTO> list = radTreeListView.SelectedItems.Cast<StructureNodeDTO>().ToList<StructureNodeDTO>();
          SaveFileDialog saveFileDialog = new SaveFileDialog()
          {
            Filter = "CSV Document|*.csv|XML Document|*.xml|Xcel Document|*.xlsx",
            Title = Resources.MSS_Client_SaveStructureToFile
          };
          bool? nullable = saveFileDialog.ShowDialog();
          List<StructureNodeDTO> nodesToBeIgnored = new List<StructureNodeDTO>();
          bool isNotRoot = false;
          list.ForEach((Action<StructureNodeDTO>) (x =>
          {
            StructureNodeDTO rootNode = x;
            this.LoadSubNodesForRootNode(rootNode);
            if (rootNode.RootNode == rootNode)
              return;
            nodesToBeIgnored.Add(rootNode);
            isNotRoot = true;
          }));
          list.RemoveAll(new Predicate<StructureNodeDTO>(nodesToBeIgnored.Contains));
          if (isNotRoot)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_EXPORT_FIXED_STRUCTURE_TITLE, Resources.MSS_EXPORT_FIXED_STRUCTURE_NONROOT_NODES, false);
          List<string[]> nodeList1 = reportingManager.CreateNodeList(list);
          this.ShowMessage(!nullable.HasValue || !nullable.Value ? MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue()) : MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue()));
          if (saveFileDialog.FileName == string.Empty)
            return;
          switch (saveFileDialog.FilterIndex)
          {
            case 1:
              List<string[]> nodeList2 = CSVManager.AddQuatForCSV(nodeList1);
              new CSVManager().WriteToFile(saveFileDialog.FileName, nodeList2);
              break;
            case 2:
              new XMLManager<StructureNodeDTO>().WriteToFile(saveFileDialog.FileName, nodeList1);
              break;
            case 3:
              new XCellManager().WriteToFile(saveFileDialog.FileName, nodeList1);
              break;
          }
          Thread.CurrentThread.CurrentCulture = cultureInfo;
        }));
      }
    }

    public ICommand ExportStructureToFileCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          CultureInfo cultureInfo = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
          Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          ReportingManager reportingManager = new ReportingManager(this._repositoryFactory);
          if (radTreeListView == null)
            return;
          List<StructureNodeDTO> list = radTreeListView.SelectedItems.Cast<StructureNodeDTO>().ToList<StructureNodeDTO>();
          SaveFileDialog saveFileDialog = new SaveFileDialog()
          {
            Filter = "Excel Document|*.xlsx",
            Title = Resources.MSS_Client_SaveStructureToFile
          };
          bool? nullable = saveFileDialog.ShowDialog();
          List<StructureNodeDTO> nodesToBeIgnored = new List<StructureNodeDTO>();
          bool isNotRoot = false;
          list.ForEach((Action<StructureNodeDTO>) (x =>
          {
            StructureNodeDTO rootNode = x;
            this.LoadSubNodesForRootNode(rootNode);
            if (rootNode.RootNode == rootNode)
              return;
            nodesToBeIgnored.Add(rootNode);
            isNotRoot = true;
          }));
          list.RemoveAll(new Predicate<StructureNodeDTO>(nodesToBeIgnored.Contains));
          if (isNotRoot)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_EXPORT_FIXED_STRUCTURE_TITLE, Resources.MSS_EXPORT_FIXED_STRUCTURE_NONROOT_NODES, false);
          List<string[]> deviceList = reportingManager.CreateDeviceList(list);
          this.ShowMessage(!nullable.HasValue || !nullable.Value ? MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue()) : MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue()));
          if (saveFileDialog.FileName == string.Empty)
            return;
          if (saveFileDialog.FilterIndex == 1)
            new XCellManager().WriteToFile(saveFileDialog.FileName, deviceList);
          Thread.CurrentThread.CurrentCulture = cultureInfo;
        }));
      }
    }

    public ICommand ViewReadingValuesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          IKernel configurator = DIConfigurator.GetConfigurator();
          if (radTreeListView == null)
            return;
          StructureNodeDTO currentItem = (StructureNodeDTO) radTreeListView.CurrentItem;
          this.LoadSubNodesForRootNode(currentItem);
          object obj = (object) null;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<MeterReadingValuesViewModel>((IParameter) new ConstructorArgument("structureNode", (object) currentItem), (IParameter) new ConstructorArgument("selectedOrder", obj)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.ShowMessage(MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue()));
          else
            this.ShowMessage(MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue()));
        }));
      }
    }

    public async void InitializeStructures()
    {
      await Task.Run((Action) (() =>
      {
        StructuresManager structuresManager = new StructuresManager(this._repositoryFactory);
        this.StructureNodeCollection = structuresManager.GetStructureNodesCollection(StructureTypeEnum.Physical, true);
        this.LogicalStructureNodeCollection = structuresManager.GetStructureNodesCollection(StructureTypeEnum.Logical, true);
        this.FixedStructureNodeCollection = structuresManager.GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
        if (!File.Exists(CustomerConfiguration.GetPropertyValue("GMMMigrationDatabasePath")) || this.StructureNodeCollection.Count != 0)
          return;
        this.MigrateDataFromGMM();
      }));
      this.IsBusy = false;
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

    public string FolderPath
    {
      get => this._folderPath;
      set
      {
        this._folderPath = value;
        this.OnPropertyChanged(nameof (FolderPath));
      }
    }

    public ICommand ImportFromSasXmlCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = "XML Files (*.xml)|*.xml"
          };
          bool? nullable = openFileDialog.ShowDialog();
          bool flag = true;
          if (nullable.GetValueOrDefault() != flag || !nullable.HasValue)
            return;
          this.FolderPath = openFileDialog.FileName;
          GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.SAS_IMPORT), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.SAS_IMPORT_TEXT));
          BackgroundWorker backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            Mapper.CreateMap<ClassBwF, Heaters>();
            foreach (DictionaryEntry dictionaryEntry in (Hashtable) this.SoapToFromFile(this.FolderPath))
            {
              Heaters heater = Mapper.Map<ClassBwF, Heaters>((ClassBwF) dictionaryEntry.Value);
              if (this._repositoryFactory.GetRepository<Heaters>().SearchFor((Expression<Func<Heaters, bool>>) (x => x.Description == heater.Description && x.GroupName == heater.GroupName && x.Name == heater.Name)).Count == 0)
                this._repositoryFactory.GetRepository<Heaters>().Insert(heater);
              if (this._repositoryFactory.GetRepository<Heaters>().SearchFor((Expression<Func<Heaters, bool>>) (x => x.Description == heater.Description && x.GroupName == heater.GroupName && x.Name == heater.Name)).Count == 1)
              {
                Heaters entity = this._repositoryFactory.GetRepository<Heaters>().FirstOrDefault((Expression<Func<Heaters, bool>>) (x => x.Description == heater.Description && x.GroupName == heater.GroupName && x.Name == heater.Name));
                entity.EvaluationFactor = heater.EvaluationFactor;
                this._repositoryFactory.GetRepository<Heaters>().Update(entity);
              }
            }
          });
          backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            pd.OnRequestClose(false);
            if (args.Cancelled)
              this.ShowMessage(MessageHandlingManager.ShowWarningMessage(Resources.MSS_SAS_IMPORT_CANCEL));
            else if (args.Error != null)
            {
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              if (args.Error.GetType() == typeof (BaseApplicationException))
                MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Error_SAS_Import.GetStringValue(), args.Error.Message, true);
              else
                MessageHandlingManager.ShowExceptionMessageDialog(CultureResources.GetValue(Resources.ERR_FAILED_TO_SAS_IMPORT) + Environment.NewLine + "Message:" + args.Error.Message + Environment.NewLine + "Inner Exception:" + (args.Error.InnerException != null ? args.Error.InnerException.Message : string.Empty) + Environment.NewLine + "Stack Trace:" + args.Error.StackTrace, this._windowFactory);
            }
            else
              this.ShowMessage(MessageHandlingManager.ShowSuccessMessage(Resources.MSS_SAS_IMPORT_SUCCEDDED));
          });
          backgroundWorker.RunWorkerAsync();
          this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
        }));
      }
    }

    public ICommand SetEvaluationFactorCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO structureNodeDto = parameter as StructureNodeDTO;
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<SetEvaluationFactorViewModel>((IParameter) new ConstructorArgument("selectedNode", (object) structureNodeDto)));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControlFixed = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
          else
            this.MessageUserControlFixed = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
        }));
      }
    }

    private ObservableCollection<StructureNodeDTO> RemoveNonRootNodes(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      List<StructureNodeDTO> nodesToBeIgnored = new List<StructureNodeDTO>();
      bool isNotRoot = false;
      TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) nodeCollection, (Action<StructureNodeDTO>) (x =>
      {
        StructureNodeDTO structureNodeDto = x;
        if (structureNodeDto.RootNode == structureNodeDto)
          return;
        nodesToBeIgnored.Add(structureNodeDto);
        isNotRoot = true;
      }));
      foreach (StructureNodeDTO structureNodeDto in nodesToBeIgnored)
        nodeCollection.Remove(structureNodeDto);
      if (isNotRoot)
        MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DELETE_FIXED_STRUCTURE_TITLE, Resources.MSS_DELETE_FIXED_STRUCTURE_NONROOT_NODES, false);
      return nodeCollection;
    }

    public ICommand DeleteSelectedPhysicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          IKernel configurator = DIConfigurator.GetConfigurator();
          ObservableCollection<object> observableCollection = (ObservableCollection<object>) parameter;
          ObservableCollection<StructureNodeDTO> physicalStructureAffected = new ObservableCollection<StructureNodeDTO>();
          TypeHelperExtensionMethods.ForEach<object>((IEnumerable<object>) observableCollection, (Action<object>) (x =>
          {
            StructureNodeDTO rootNode = (StructureNodeDTO) x;
            this.LoadSubNodesForRootNode(rootNode);
            physicalStructureAffected.Add(rootNode);
          }));
          physicalStructureAffected = this.RemoveNonRootNodes(physicalStructureAffected);
          if (physicalStructureAffected.Count == 0)
          {
            MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_TITLE, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_EMPTY_LIST, false);
          }
          else
          {
            List<StructureNodeDTO> logicalStructure = new List<StructureNodeDTO>();
            TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) physicalStructureAffected, (Action<StructureNodeDTO>) (selectedNode => logicalStructure.AddRange(this.GetStructuresManagerInstance().GetAffectedLogicalStructure(selectedNode, StructureTypeEnum.Physical))));
            this.ShowDeleteWindow(logicalStructure, configurator, physicalStructureAffected);
          }
        }));
      }
    }

    private void ShowDeleteWindow(
      List<StructureNodeDTO> logicalStructure,
      IKernel diConfig,
      ObservableCollection<StructureNodeDTO> physicalStructureAffected)
    {
      ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>(logicalStructure);
      bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) diConfig.Get<DeleteStructureViewModel>((IParameter) new ConstructorArgument("structureToDelete", (object) physicalStructureAffected), (IParameter) new ConstructorArgument("otherAffectedStructures", (object) observableCollection)));
      if (newModalDialog.HasValue && newModalDialog.Value)
      {
        TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) physicalStructureAffected, (Action<StructureNodeDTO>) (selectedNode => this.GetStructuresManagerInstance().DeleteStructure(selectedNode, StructureTypeEnum.Physical)));
        this.MessageUserControlPhysical = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Delete_Structure.GetStringValue());
      }
      else
        this.MessageUserControlPhysical = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
      this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
      this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
    }

    public ICommand CreatePhysicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreatePhysicalStructureViewModel>());
          this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
          StructureTreeStateHelper.MaintainExpandedState(parameter as RadTreeListView, this._structureNodeCollection);
        }));
      }
    }

    public ICommand EditSelectedPhysicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!this.EditPhysicalStructuresVisibility || !(parameter is RadTreeListView radTreeListView2) || radTreeListView2.CurrentItem == null)
            return;
          StructureNodeDTO selectedNode = (StructureNodeDTO) radTreeListView2.CurrentItem;
          this.IsBusy = true;
          Task<StructureNodeDTO> task = new Task<StructureNodeDTO>((Func<StructureNodeDTO>) (() =>
          {
            if (selectedNode.ParentNode == null)
              this.LoadSubNodesForRootNode(selectedNode);
            return selectedNode;
          }));
          task.ContinueWith((Action<Task<StructureNodeDTO>>) (previousTask =>
          {
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditPhysicalStructureViewModel>((IParameter) new ConstructorArgument("selectedNode", (object) selectedNode), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) false), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false)));
            this.IsBusy = false;
          }), System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
          task.Start();
        }));
      }
    }

    public StructureNodeDTO SelectedItem
    {
      get => this._selectedItem;
      set
      {
        this._selectedItem = value;
        if (this._selectedItem != null)
          this.IsRootItemSelected = this._selectedItem.RootNode == this._selectedItem;
        else
          this.IsRootItemSelected = false;
      }
    }

    public bool IsRootItemSelected
    {
      get => this._isRootItemSelected;
      set
      {
        this._isRootItemSelected = value;
        this.OnPropertyChanged(nameof (IsRootItemSelected));
      }
    }

    public bool IsPhysicalTabSelected
    {
      get => this._isPhysicalTabSelected;
      set
      {
        this._isPhysicalTabSelected = value;
        if (!this._isPhysicalTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.StructuresPhysical
        }, (IViewModel) this);
      }
    }

    public ICommand UnlockPhysicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is StructureNodeDTO structureNodeDto2))
            return;
          this.GetStructuresManagerInstance().UnlockStructure(structureNodeDto2.RootNode.Id);
          this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
        }));
      }
    }

    public ICommand ExpandStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is RadTreeListView tree2))
            return;
          if (this.IsPhysicalTabSelected && this.StructureNodeCollection.Any<StructureNodeDTO>())
            this.ExpandNodesInStructure(this.StructureNodeCollection, tree2);
          else if (this.IsLogicalTabSelected && this.LogicalStructureNodeCollection.Any<StructureNodeDTO>())
            this.ExpandNodesInStructure(this.LogicalStructureNodeCollection, tree2);
          else if (this.IsFixedTabSelected && this.FixedStructureNodeCollection.Any<StructureNodeDTO>())
            this.ExpandNodesInStructure(this.FixedStructureNodeCollection, tree2);
        }));
      }
    }

    private void ExpandNodesInStructure(
      ObservableCollection<StructureNodeDTO> rootNodesCollection,
      RadTreeListView tree)
    {
      List<StructureNodeDTO> structureNodeDtoList = new List<StructureNodeDTO>();
      foreach (StructureNodeDTO rootNodes in (Collection<StructureNodeDTO>) rootNodesCollection)
      {
        if ((rootNodes.SubNodes == null || rootNodes.SubNodes.Count == 0) && rootNodes.Id == rootNodes.RootNode.Id)
          this.LoadSubNodesForRootNode(rootNodes, this._canViewMeter);
        structureNodeDtoList.Add(rootNodes);
      }
      rootNodesCollection.Clear();
      structureNodeDtoList.Reverse();
      foreach (StructureNodeDTO structureNodeDto in structureNodeDtoList)
      {
        rootNodesCollection.Insert(0, structureNodeDto);
        tree.ExpandAllHierarchyItems();
      }
    }

    public ICommand CollapseStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is RadTreeListView))
            return;
          if (this.IsPhysicalTabSelected && this.StructureNodeCollection.Any<StructureNodeDTO>())
            this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
          else if (this.IsLogicalTabSelected && this.LogicalStructureNodeCollection.Any<StructureNodeDTO>())
            this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
          else if (this.IsFixedTabSelected && this.FixedStructureNodeCollection.Any<StructureNodeDTO>())
            this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed);
        }));
      }
    }

    public ObservableCollection<StructureNodeDTO> LogicalStructureNodeCollection
    {
      get => this._logicalStructureNodeCollection;
      set
      {
        this._logicalStructureNodeCollection = value;
        this.OnPropertyChanged(nameof (LogicalStructureNodeCollection));
      }
    }

    public ICommand RemoveSelectedLogicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          IKernel configurator = DIConfigurator.GetConfigurator();
          if (radTreeListView == null)
            return;
          StructureNodeDTO currentItem = (StructureNodeDTO) radTreeListView.CurrentItem;
          ObservableCollection<StructureNodeDTO> observableCollection1 = new ObservableCollection<StructureNodeDTO>()
          {
            currentItem
          };
          ObservableCollection<StructureNodeDTO> observableCollection2 = new ObservableCollection<StructureNodeDTO>();
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<DeleteStructureViewModel>((IParameter) new ConstructorArgument("structureToDelete", (object) observableCollection1), (IParameter) new ConstructorArgument("otherAffectedStructures", (object) observableCollection2)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.GetStructuresManagerInstance().RemoveStructure(currentItem, StructureTypeEnum.Logical);
            this.MessageUserControlLogical = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Remove_Structure.GetStringValue());
          }
          else
            this.MessageUserControlLogical = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
          this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
        }));
      }
    }

    public ICommand DeleteSelectedLogicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          IKernel configurator = DIConfigurator.GetConfigurator();
          bool flag = false;
          ObservableCollection<object> observableCollection = (ObservableCollection<object>) parameter;
          ObservableCollection<StructureNodeDTO> logicalStructureAffected = new ObservableCollection<StructureNodeDTO>();
          ObservableCollection<StructureNodeDTO> otherAffectedStructures = new ObservableCollection<StructureNodeDTO>();
          TypeHelperExtensionMethods.ForEach<object>((IEnumerable<object>) observableCollection, (Action<object>) (x =>
          {
            StructureNodeDTO rootNode = (StructureNodeDTO) x;
            this.LoadSubNodesForRootNode(rootNode);
            logicalStructureAffected.Add(rootNode);
          }));
          logicalStructureAffected = this.RemoveNonRootNodes(logicalStructureAffected);
          if (logicalStructureAffected.Count == 0)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_TITLE, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_EMPTY_LIST, false);
          else if (flag)
          {
            bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_TITLE, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_MESSAGE, true);
            if (nullable.HasValue && nullable.Value)
              this.DeleteLogicalStructures(configurator, logicalStructureAffected, otherAffectedStructures);
            else
              this.MessageUserControlLogical = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          }
          else
            this.DeleteLogicalStructures(configurator, logicalStructureAffected, otherAffectedStructures);
        }));
      }
    }

    private void DeleteLogicalStructures(
      IKernel diConfig,
      ObservableCollection<StructureNodeDTO> logicalStructureAffected,
      ObservableCollection<StructureNodeDTO> otherAffectedStructures)
    {
      bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) diConfig.Get<DeleteStructureViewModel>((IParameter) new ConstructorArgument("structureToDelete", (object) logicalStructureAffected), (IParameter) new ConstructorArgument(nameof (otherAffectedStructures), (object) otherAffectedStructures)));
      if (newModalDialog.HasValue && newModalDialog.Value)
      {
        TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) logicalStructureAffected, (Action<StructureNodeDTO>) (selectedNode => this.GetStructuresManagerInstance().DeleteStructure(selectedNode, StructureTypeEnum.Logical)));
        this.MessageUserControlLogical = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Delete_Structure.GetStringValue());
      }
      else
        this.MessageUserControlLogical = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
      this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
      this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
    }

    private void DeleteFixedStructures(
      IKernel diConfig,
      ObservableCollection<StructureNodeDTO> fixedStructureAffected,
      ObservableCollection<StructureNodeDTO> otherAffectedStructures)
    {
      bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) diConfig.Get<DeleteStructureViewModel>((IParameter) new ConstructorArgument("structureToDelete", (object) fixedStructureAffected), (IParameter) new ConstructorArgument(nameof (otherAffectedStructures), (object) otherAffectedStructures)));
      if (newModalDialog.HasValue && newModalDialog.Value)
      {
        TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) fixedStructureAffected, (Action<StructureNodeDTO>) (selectedNode => this.GetStructuresManagerInstance().DeleteStructure(selectedNode, StructureTypeEnum.Fixed)));
        this.MessageUserControlFixed = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Delete_Structure.GetStringValue());
      }
      else
        this.MessageUserControlFixed = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
      this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
    }

    public ICommand CreateLogicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateLogicalStructureViewModel>());
          this.OnPropertyChanged("LogicalStructureNodeCollection");
          this.StructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
          this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
          StructureTreeStateHelper.MaintainExpandedState(parameter as RadTreeListView, this._logicalStructureNodeCollection);
        }));
      }
    }

    public ICommand EditSelectedLogicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!this.EditLogicalStructuresVisibility || !(parameter is RadTreeListView radTreeListView2) || radTreeListView2.CurrentItem == null)
            return;
          StructureNodeDTO selectedNode = (StructureNodeDTO) radTreeListView2.CurrentItem;
          this.IsBusy = true;
          Task<StructureNodeDTO> task = new Task<StructureNodeDTO>((Func<StructureNodeDTO>) (() =>
          {
            if (selectedNode.ParentNode == null)
              this.LoadSubNodesForRootNode(selectedNode);
            return selectedNode;
          }));
          task.ContinueWith((Action<Task<StructureNodeDTO>>) (previousTask =>
          {
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditLogicalStructureViewModel>((IParameter) new ConstructorArgument("selectedNode", (object) selectedNode), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false)));
            this.IsBusy = false;
          }), System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
          task.Start();
        }));
      }
    }

    public StructureNodeDTO SelectedLogicalItem
    {
      get => this._selectedLogicalItem;
      set
      {
        this._selectedLogicalItem = value;
        if (this._selectedLogicalItem != null)
          this.IsRootLogicalItemSelected = this._selectedLogicalItem.RootNode == this._selectedLogicalItem;
        else
          this.IsRootLogicalItemSelected = false;
      }
    }

    public bool IsRootLogicalItemSelected
    {
      get => this._isRootLogicalItemSelected;
      set
      {
        this._isRootLogicalItemSelected = value;
        this.OnPropertyChanged(nameof (IsRootLogicalItemSelected));
      }
    }

    public bool IsLogicalTabSelected
    {
      get => this._isLogicalTabSelected;
      set
      {
        this._isLogicalTabSelected = value;
        if (!this._isLogicalTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.StructuresLogical
        }, (IViewModel) this);
      }
    }

    public ICommand UnlockLogicalStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is StructureNodeDTO structureNodeDto2))
            return;
          this.GetStructuresManagerInstance().UnlockStructure(structureNodeDto2.RootNode.Id);
          this.LogicalStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Logical);
        }));
      }
    }

    public ObservableCollection<StructureNodeDTO> FixedStructureNodeCollection
    {
      get => this._fixedStructureNodeCollection;
      set
      {
        this._fixedStructureNodeCollection = value;
        this.OnPropertyChanged(nameof (FixedStructureNodeCollection));
      }
    }

    public StructureNodeDTO SelectedFixedItem
    {
      get => this._selectedFixedItem;
      set
      {
        this._selectedFixedItem = value;
        if (this._selectedFixedItem != null)
          this.IsRootFixedItemSelected = this._selectedFixedItem.RootNode == this._selectedFixedItem;
        else
          this.IsRootFixedItemSelected = false;
      }
    }

    public bool IsRootFixedItemSelected
    {
      get => this._isRootFixedItemSelected;
      set
      {
        this._isRootFixedItemSelected = value;
        this.OnPropertyChanged(nameof (IsRootFixedItemSelected));
      }
    }

    public bool IsFixedTabSelected
    {
      get => this._isFixedTabSelected;
      set
      {
        this._isFixedTabSelected = value;
        if (!this._isFixedTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.StructuresFixed
        }, (IViewModel) this);
      }
    }

    public bool EvaluationFactorVisibility { get; set; }

    public bool InstallationOrderAttachTestVisibility
    {
      get => this._installationOrderAttachTestVisibility;
      set
      {
        this._installationOrderAttachTestVisibility = value;
        this.OnPropertyChanged(nameof (InstallationOrderAttachTestVisibility));
      }
    }

    public ICommand RadioTestRunDialog
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          IKernel configurator = DIConfigurator.GetConfigurator();
          if (radTreeListView == null)
            return;
          StructureNodeDTO currentItem = (StructureNodeDTO) radTreeListView.CurrentItem;
          this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<RadioTestViewModel>((IParameter) new ConstructorArgument("structureNode", (object) currentItem)));
        }));
      }
    }

    public ICommand AssignRadioTestRunDialog
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          IKernel configurator = DIConfigurator.GetConfigurator();
          if (radTreeListView == null)
            return;
          StructureNodeDTO currentItem = (StructureNodeDTO) radTreeListView.CurrentItem;
          this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<AssignTestRunViewModel>((IParameter) new ConstructorArgument("structureNode", (object) currentItem)));
        }));
      }
    }

    public ICommand RemoveSelectedFixedStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          IKernel configurator = DIConfigurator.GetConfigurator();
          if (radTreeListView == null)
            return;
          StructureNodeDTO currentItem = (StructureNodeDTO) radTreeListView.CurrentItem;
          ObservableCollection<StructureNodeDTO> observableCollection1 = new ObservableCollection<StructureNodeDTO>()
          {
            currentItem
          };
          ObservableCollection<StructureNodeDTO> observableCollection2 = new ObservableCollection<StructureNodeDTO>();
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<DeleteStructureViewModel>((IParameter) new ConstructorArgument("structureToDelete", (object) observableCollection1), (IParameter) new ConstructorArgument("otherAffectedStructures", (object) observableCollection2)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.GetStructuresManagerInstance().RemoveStructure(currentItem, StructureTypeEnum.Fixed);
            this.MessageUserControlFixed = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Remove_Structure.GetStringValue());
          }
          else
            this.MessageUserControlFixed = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
        }));
      }
    }

    public ICommand DeleteSelectedFixedStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          IKernel configurator = DIConfigurator.GetConfigurator();
          bool flag = false;
          ObservableCollection<object> observableCollection = (ObservableCollection<object>) parameter;
          ObservableCollection<StructureNodeDTO> fixedStructureaffected = new ObservableCollection<StructureNodeDTO>();
          ObservableCollection<StructureNodeDTO> nodeCollection = new ObservableCollection<StructureNodeDTO>();
          TypeHelperExtensionMethods.ForEach<object>((IEnumerable<object>) observableCollection, (Action<object>) (x =>
          {
            StructureNodeDTO rootNode = (StructureNodeDTO) x;
            this.LoadSubNodesForRootNode(rootNode);
            fixedStructureaffected.Add(rootNode);
          }));
          fixedStructureaffected = this.RemoveNonRootNodes(fixedStructureaffected);
          ObservableCollection<StructureNodeDTO> otherAffectedStructures = this.RemoveNonRootNodes(nodeCollection);
          if (fixedStructureaffected.Count == 0)
            MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_TITLE, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_EMPTY_LIST, false);
          else if (flag)
          {
            bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_TITLE, Resources.MSS_DELETE_PHYSICAL_STRUCTURE_MESSAGE, true);
            if (nullable.HasValue && nullable.Value)
              this.DeleteFixedStructures(configurator, fixedStructureaffected, otherAffectedStructures);
            else
              this.MessageUserControlFixed = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
          }
          else
            this.DeleteFixedStructures(configurator, fixedStructureaffected, otherAffectedStructures);
        }));
      }
    }

    public ICommand CreateFixedStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateFixedStructureViewModel>());
          this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
          StructureTreeStateHelper.MaintainExpandedState(parameter as RadTreeListView, this._fixedStructureNodeCollection);
        }));
      }
    }

    public ICommand EditSelectedFixedStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (async parameter =>
        {
          if (!this.EditFixedStructuresVisibility)
            return;
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          if (radTreeListView != null && radTreeListView.CurrentItem != null)
          {
            StructureNodeDTO selectedNode = (StructureNodeDTO) radTreeListView.CurrentItem;
            StructureNodeDTO rootNode = selectedNode.RootNode ?? selectedNode;
            this.IsBusy = true;
            await Task.Run((Action) (() =>
            {
              if (rootNode.ParentNode != null)
                return;
              this.LoadSubNodesForRootNode(rootNode);
            }));
            EditFixedStructureViewModel vm = DIConfigurator.GetConfigurator().Get<EditFixedStructureViewModel>("EditFixedStructureForStructureViewModel", (IParameter) new ConstructorArgument("selectedNode", (object) rootNode), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) false));
            this._windowFactory.CreateNewModalDialog((IViewModel) vm);
            this.IsBusy = false;
            selectedNode = (StructureNodeDTO) null;
            vm = (EditFixedStructureViewModel) null;
          }
          this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
          StructureTreeStateHelper.MaintainExpandedState(radTreeListView, this._fixedStructureNodeCollection);
          radTreeListView = (RadTreeListView) null;
        }));
      }
    }

    public ICommand UnlockFixedStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is StructureNodeDTO structureNodeDto2))
            return;
          this.GetStructuresManagerInstance().UnlockStructure(structureNodeDto2.RootNode.Id);
          this.FixedStructureNodeCollection = this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
        }));
      }
    }

    public ICommand GmmToMssMigration
    {
      get => (ICommand) new RelayCommand((Action<object>) (_ => this.MigrateDataFromGMM()));
    }

    private void MigrateDataFromGMM()
    {
      MigrationManager migrationManager = new MigrationManager(this._repositoryFactory);
      SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
      System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext();
      bool? isOkButton = new bool?();
      if (new MSS.Business.Modules.AppParametersManagement.AppParametersManagement(this._repositoryFactory).GetAppParam("DoNotShowGmmImportScreenAtStartup").Value.ToLower() == "false")
        Application.Current.Dispatcher.Invoke((Action) (() => isOkButton = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ImportGmmDataViewModel>())));
      if (!isOkButton.HasValue || !isOkButton.Value)
        return;
      new Task((Action) (() =>
      {
        this.IsBusy = true;
        try
        {
          string validationMessages = "";
          List<StructureNodeDTO> validatedStructures;
          migrationManager.ValidateStructureMigration(out validatedStructures, out validationMessages);
          if (!string.IsNullOrEmpty(validationMessages))
          {
            Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DeleteStructure_Warning_Title, validationMessages, false)));
          }
          else
          {
            migrationManager.MigrateStructuresAndMeters(validatedStructures);
            migrationManager.MigrateReadingValues();
            StructuresManager structuresManager = new StructuresManager(this._repositoryFactory);
            this.StructureNodeCollection = structuresManager.GetStructureNodesCollection(StructureTypeEnum.Physical, true);
            this.LogicalStructureNodeCollection = structuresManager.GetStructureNodesCollection(StructureTypeEnum.Logical, true);
            this.FixedStructureNodeCollection = structuresManager.GetStructureNodesCollection(StructureTypeEnum.Fixed, true);
            Application.Current.Dispatcher.Invoke((Action) (() => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_Client_MigrationSuccessful_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_MigrationSuccessful), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)))));
          }
        }
        catch (Exception ex)
        {
          MSS.Business.Errors.MessageHandler.LogException(ex, MessageCodes.Error_MigrationFailed);
          Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DeleteStructure_Warning_Title, Resources.MSS_Client_MigrationFailed, false)));
        }
        finally
        {
          this.IsBusy = false;
        }
      })).Start();
    }

    public bool CreatePhysicalStructuresVisibility
    {
      get => this._createPhysicalStructuresVisibility;
      set
      {
        this._createPhysicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (CreatePhysicalStructuresVisibility));
      }
    }

    public bool EditPhysicalStructuresVisibility
    {
      get => this._editPhysicalStructuresVisibility;
      set
      {
        this._editPhysicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (EditPhysicalStructuresVisibility));
      }
    }

    public bool RemovePhysicalStructuresVisibility
    {
      get => this._removePhysicalStructuresVisibility;
      set
      {
        this._removePhysicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (RemovePhysicalStructuresVisibility));
      }
    }

    public bool DeletePhysicalStructuresVisibility
    {
      get => this._deletePhysicalStructuresVisibility;
      set
      {
        this._deletePhysicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (DeletePhysicalStructuresVisibility));
      }
    }

    public bool ImportPhysicalStructuresVisibility
    {
      get => this._importPhysicalStructuresVisibility;
      set
      {
        this._importPhysicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (ImportPhysicalStructuresVisibility));
      }
    }

    public bool ExportPhysicalStructuresVisibility
    {
      get => this._exportPhysicalStructuresVisibility;
      set
      {
        this._exportPhysicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (ExportPhysicalStructuresVisibility));
      }
    }

    public bool ReadingValuesVisibility
    {
      get => this._readingValuesVisibility;
      set
      {
        this._readingValuesVisibility = value;
        this.OnPropertyChanged(nameof (ReadingValuesVisibility));
      }
    }

    public bool CreateLogicalStructuresVisibility
    {
      get => this._createLogicalStructuresVisibility;
      set
      {
        this._createLogicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (CreateLogicalStructuresVisibility));
      }
    }

    public bool EditLogicalStructuresVisibility
    {
      get => this._editLogicalStructuresVisibility;
      set
      {
        this._editLogicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (EditLogicalStructuresVisibility));
      }
    }

    public bool RemoveLogicalStructuresVisibility
    {
      get => this._removeLogicalStructuresVisibility;
      set
      {
        this._removeLogicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (RemoveLogicalStructuresVisibility));
      }
    }

    public bool DeleteLogicalStructuresVisibility
    {
      get => this._deleteLogicalStructuresVisibility;
      set
      {
        this._deleteLogicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (DeleteLogicalStructuresVisibility));
      }
    }

    public bool ImportLogicalStructuresVisibility
    {
      get => this._importLogicalStructuresVisibility;
      set
      {
        this._importLogicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (ImportLogicalStructuresVisibility));
      }
    }

    public bool ExportLogicalStructuresVisibility
    {
      get => this._exportLogicalStructuresVisibility;
      set
      {
        this._exportLogicalStructuresVisibility = value;
        this.OnPropertyChanged(nameof (ExportLogicalStructuresVisibility));
      }
    }

    public bool CreateFixedStructuresVisibility
    {
      get => this._createFixedStructuresVisibility;
      set
      {
        this._createFixedStructuresVisibility = value;
        this.OnPropertyChanged(nameof (CreateFixedStructuresVisibility));
      }
    }

    public bool EditFixedStructuresVisibility
    {
      get => this._editFixedStructuresVisibility;
      set
      {
        this._editFixedStructuresVisibility = value;
        this.OnPropertyChanged(nameof (EditFixedStructuresVisibility));
      }
    }

    public bool RemoveFixedStructuresVisibility
    {
      get => this._removeFixedStructuresVisibility;
      set
      {
        this._removeFixedStructuresVisibility = value;
        this.OnPropertyChanged(nameof (RemoveFixedStructuresVisibility));
      }
    }

    public bool DeleteFixedStructuresVisibility
    {
      get => this._deleteFixedStructuresVisibility;
      set
      {
        this._deleteFixedStructuresVisibility = value;
        this.OnPropertyChanged(nameof (DeleteFixedStructuresVisibility));
      }
    }

    public bool ImportFixedStructuresVisibility
    {
      get => this._importFixedStructuresVisibility;
      set
      {
        this._importFixedStructuresVisibility = value;
        this.OnPropertyChanged(nameof (ImportFixedStructuresVisibility));
      }
    }

    public bool ExportFixedStructuresVisibility
    {
      get => this._exportFixedStructuresVisibility;
      set
      {
        this._exportFixedStructuresVisibility = value;
        this.OnPropertyChanged(nameof (ExportFixedStructuresVisibility));
      }
    }

    public bool ExportFixedStructureDevicesVisibility
    {
      get => this._exportFixedStructuresVisibility;
      set
      {
        this._exportFixedStructuresVisibility = value;
        this.OnPropertyChanged("ExportFixedStructuresVisibility");
      }
    }

    public bool IsPhysicalTabVisible { get; set; }

    public bool IsLogicalTabVisible { get; set; }

    public bool IsFixedTabVisible { get; set; }
  }
}
