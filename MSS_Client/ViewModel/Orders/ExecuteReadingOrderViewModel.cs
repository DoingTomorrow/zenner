// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.ExecuteReadingOrderViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using Common.Library.NHibernate.Data;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.GMMWrapper;
using MSS.Business.Modules.OrdersManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.ApplicationParamenters;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Configuration;
using MSS_Client.ViewModel.Meters;
using MSS_Client.ViewModel.Orders.Ctrls;
using MSS_Client.ViewModel.Settings;
using MVVM.Commands;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Data;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class ExecuteReadingOrderViewModel : MVVM.ViewModel.ViewModelBase
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly OrderDTO _selectedOrder;
    protected readonly IRepository<MeterReadingValue> _meterReadingValueRepository;
    protected readonly IRepository<OrderReadingValues> _orderReadingValuesRepository;
    private readonly ISession nhSession;
    private BackgroundWorker _backgroundWorker;
    private readonly IWindowFactory _windowFactory;
    private ReaderMinoConnectManager _readerMinoConnect;
    private ReceiverMinoConnectManager _receiverMinoConnect;
    private IDeviceManager _deviceManager;
    private readonly List<Guid> _blueMeters;
    private readonly StructureNodeEquipmentSettings _structureNodeEquipmentSettings;
    private bool isIRReadingStarted = false;
    private readonly int registerCollectionCount;
    private Dictionary<Guid, string> _metersToBeRead = new Dictionary<Guid, string>();
    private const string REMOTE_VPN = "VPN";
    private const string IR_MINOMAT_V4 = "IR Minomat V4";
    private EquipmentModel _selectedEquipmentModel;
    private Dictionary<Guid, StructureNodeEquipmentSettings> _equipmentSettingsForMeters;
    private Guid _currentlySelectedItem = Guid.Empty;
    private IStatelessSession _session;
    private IList<MeasureUnit> _measureUnitList;
    private List<string> _importableDeviceModelNameList;
    private byte[] _structureBytes;
    private ObservableCollection<ExecuteOrderStructureNode> _nodesList = new ObservableCollection<ExecuteOrderStructureNode>();
    private ObservableCollection<ExecuteOrderStructureNode> _selectedMeterList;
    private IEnumerable<ProfileType> _connectionProfileCollection;
    private ProfileType _selectedConnectionProfile;
    private Visibility _isChangeProfileTypeVisible = Visibility.Visible;
    private ExecuteOrderStructureNode _selectedItem;
    private ObservableCollection<object> _selectedNodes;
    private bool _isDeviceSelected;
    private bool _isTenantOrDeviceSelected;
    private RadObservableCollection<ReadingValuesDTO> _readingValuesCollection;
    private ObservableCollection<MeterReadingValueDTO> _genericMbusReadingValuesCollection;
    private IEnumerable<ValueIdent.ValueIdPart_MeterType> _registerCollection;
    private IEnumerable<ValueIdentString> _physicalQuantitiesEnumerable;
    private IEnumerable<ValueIdentString> _meterTypeEnumerable;
    private IEnumerable<ValueIdentString> _calculationEnumerable;
    private IEnumerable<ValueIdentString> _calculationStartEnumerable;
    private IEnumerable<ValueIdentString> _storageIntervalEnumerable;
    private IEnumerable<ValueIdentString> _creationEnumerable;
    private bool _isReadingValuesGridVisible;
    private bool _isSaveButtonVisible;
    private bool _isGenericMbusReadingValuesGridVisible;
    private string _newRowPosition;
    private bool _canUserInsertRows;
    private MVVM.ViewModel.ViewModelBase _messageUserControl;
    private string _numberOfReadMetersLabel;
    private bool _isPhysicalStructure;
    private bool _isStartButtonEnabled;
    private bool _isChangeDeviceModelParametersEnabled;
    private ObservableCollection<ReadingValuesDTO> _readingValuesToDeleteCollection = new ObservableCollection<ReadingValuesDTO>();
    private ObservableCollection<MeterReadingValueDTO> _meterReadingValuesToDeleteCollection = new ObservableCollection<MeterReadingValueDTO>();
    private string _selectedEquipmentName;
    private bool _isStopButtonEnabled;
    private TransceiverType _transceiverType;
    private bool _isReadingDeviceViaCBEnabled;
    private bool _isReadingStarted;
    private bool _isShortDeviceNoVisible;
    private ProgressBarCtrl _progressBar;

    [Inject]
    public ExecuteReadingOrderViewModel(
      OrderDTO selectedOrder,
      IDeviceManager deviceManager,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._deviceManager = deviceManager;
      this.nhSession = this._repositoryFactory.GetSession();
      this._meterReadingValueRepository = this._repositoryFactory.GetRepository<MeterReadingValue>();
      this._orderReadingValuesRepository = this._repositoryFactory.GetRepository<OrderReadingValues>();
      this._repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>();
      this._selectedOrder = selectedOrder;
      StructureTypeEnum? structureType = this._selectedOrder.StructureType;
      StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Physical;
      this.IsPhysicalStructure = structureType.GetValueOrDefault() == structureTypeEnum1 && structureType.HasValue;
      this._blueMeters = new List<Guid>();
      this.StructureBytes = selectedOrder.StructureBytes;
      EventPublisher.Register<StructureBytesUpdated>(new Action<StructureBytesUpdated>(this.RefreshStructureBytes));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      EventPublisher.Register<ErrorDuringReading>(new Action<ErrorDuringReading>(this.HandleErrorDuringReading));
      EventPublisher.Register<OrderReadingValuesSavedEvent>(new Action<OrderReadingValuesSavedEvent>(this.OrderReadingValuesSaved));
      EventPublisher.Register<ProgressFinished>(new Action<ProgressFinished>(this.OnReadingProgressBarFinish));
      this.CloseCommand = (ICommand) new MSS.Client.UI.Common.Utils.DelegateCommand((Action) (() =>
      {
        this.CleanupBeforeClosing();
        this.Dispose();
      }), (Func<bool>) (() => true));
      this._importableDeviceModelNameList = GMMHelper.GetDeviceModelNameList(this._selectedOrder.StructureType);
      this.StructureForSelectedOrder = this.GetOrderManagerInstance().GetNodes(this._structureBytes, this._selectedOrder.Id, this._blueMeters, this._importableDeviceModelNameList).Item1;
      this.IsChangeDeviceModelParametersEnabled = false;
      this._structureNodeEquipmentSettings = repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().FirstOrDefault((Expression<Func<StructureNodeEquipmentSettings, bool>>) (x => x.StructureNode.Id == this._selectedOrder.RootStructureNodeId));
      if (this._structureNodeEquipmentSettings != null)
      {
        this.SelectedEquipmentName = this._structureNodeEquipmentSettings?.EquipmentName;
        this._selectedEquipmentModel = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateEquipmentWithSavedParams(this._deviceManager.GetEquipmentModels().FirstOrDefault<EquipmentModel>((Func<EquipmentModel, bool>) (e => e.Name == this.SelectedEquipmentName)), this._structureNodeEquipmentSettings.EquipmentParams);
        this._selectedItem = this.StructureForSelectedOrder.FirstOrDefault<ExecuteOrderStructureNode>();
        this.ConnectionProfileCollection = this.InitConnectionProfileCollection();
        if (this._structureNodeEquipmentSettings != null)
        {
          this.SelectedConnectionProfile = this._connectionProfileCollection.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (_ => _.Name == this._structureNodeEquipmentSettings.ReadingProfileName)) ?? this._connectionProfileCollection.FirstOrDefault<ProfileType>();
          if (!string.IsNullOrEmpty(this._structureNodeEquipmentSettings.ReadingProfileParams) && this.SelectedConnectionProfile != null)
            MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateProfileTypeWithSavedParams(this.SelectedConnectionProfile, this._structureNodeEquipmentSettings.ReadingProfileParams);
          else if (string.IsNullOrEmpty(this._structureNodeEquipmentSettings.ReadingProfileParams) && this._structureNodeEquipmentSettings?.SystemName == "M-Bus" && this._structureNodeEquipmentSettings.ScanParams != null && this.SelectedConnectionProfile != null)
            MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateProfileTypeWithSavedParams(this.SelectedConnectionProfile, this._structureNodeEquipmentSettings.ScanParams);
        }
        else
          this.SelectedConnectionProfile = this._connectionProfileCollection.FirstOrDefault<ProfileType>();
      }
      this.IsStartButtonEnabled = this.SelectedItem != null && this.SelectedConnectionProfile != null;
      this.IsStopButtonEnabled = false;
      this.IsSaveButtonVisible = false;
      this.IsReadingDeviceViaCBEnabled = true;
      this.registerCollectionCount = this.GetRegisterColection.Count<ValueIdent.ValueIdPart_MeterType>();
      this.IsReadingStarted = false;
      structureType = this._selectedOrder.StructureType;
      StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Fixed;
      this.IsShortDeviceNoVisible = structureType.GetValueOrDefault() == structureTypeEnum2 && structureType.HasValue;
      this.UpdateLabel();
      this._physicalQuantitiesEnumerable = ValueIdentHelper.GetPhysicalQuantitiesEnumerable().Select<string, ValueIdentString>((Func<string, ValueIdentString>) (item => new ValueIdentString()
      {
        Value = item
      }));
      this._meterTypeEnumerable = ValueIdentHelper.GetMeterTypeEnumerable().Select<string, ValueIdentString>((Func<string, ValueIdentString>) (item => new ValueIdentString()
      {
        Value = item
      }));
      this._calculationEnumerable = ValueIdentHelper.GetCalculationEnumerable().Select<string, ValueIdentString>((Func<string, ValueIdentString>) (item => new ValueIdentString()
      {
        Value = item
      }));
      this._calculationStartEnumerable = ValueIdentHelper.GetCalculationStartEnumerable().Select<string, ValueIdentString>((Func<string, ValueIdentString>) (item => new ValueIdentString()
      {
        Value = item
      }));
      this._storageIntervalEnumerable = ValueIdentHelper.GetStorageIntervalEnumerable().Select<string, ValueIdentString>((Func<string, ValueIdentString>) (item => new ValueIdentString()
      {
        Value = item
      }));
      this._creationEnumerable = ValueIdentHelper.GetCreationEnumerable().Select<string, ValueIdentString>((Func<string, ValueIdentString>) (item => new ValueIdentString()
      {
        Value = item
      }));
      this._measureUnitList = this._repositoryFactory.GetRepository<MeasureUnit>().GetAll();
      Mapper.CreateMap<MeterReadingValue, MeterReadingValueDTO>();
      this._equipmentSettingsForMeters = new Dictionary<Guid, StructureNodeEquipmentSettings>();
      structureType = this._selectedOrder.StructureType;
      StructureTypeEnum structureTypeEnum3 = StructureTypeEnum.Fixed;
      if (structureType.GetValueOrDefault() != structureTypeEnum3 || !structureType.HasValue)
      {
        List<Guid> meterGuids = this.GetMeterGuids();
        List<StructureNode> list1 = this._repositoryFactory.GetRepository<StructureNode>().Where((Expression<Func<StructureNode, bool>>) (item => meterGuids.Contains(item.EntityId))).ToList<StructureNode>();
        List<Guid> structureNodeIds = list1.Select<StructureNode, Guid>((Func<StructureNode, Guid>) (item => item.Id)).ToList<Guid>();
        List<StructureNodeEquipmentSettings> list2 = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().Where((Expression<Func<StructureNodeEquipmentSettings, bool>>) (item => structureNodeIds.Contains(item.StructureNode.Id))).ToList<StructureNodeEquipmentSettings>();
        foreach (Guid guid in meterGuids)
        {
          Guid meterId = guid;
          Guid? structureNodeIdForCurrentMeter = list1.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (item => item.EntityId == meterId))?.Id;
          StructureNodeEquipmentSettings equipmentSettings = (StructureNodeEquipmentSettings) null;
          if (structureNodeIdForCurrentMeter.HasValue)
            equipmentSettings = list2.FirstOrDefault<StructureNodeEquipmentSettings>((Func<StructureNodeEquipmentSettings, bool>) (item =>
            {
              Guid id = item.StructureNode.Id;
              Guid? nullable = structureNodeIdForCurrentMeter;
              return nullable.HasValue && id == nullable.GetValueOrDefault();
            }));
          this._equipmentSettingsForMeters.Add(meterId, equipmentSettings);
        }
      }
      string appSetting = ConfigurationManager.AppSettings["DatabaseEngine"];
      HibernateMultipleDatabasesManager.Initialize(appSetting);
      this._session = HibernateMultipleDatabasesManager.DataSessionFactory(appSetting).OpenStatelessSession();
      this.ProgressBar = new ProgressBarCtrl();
    }

    private void UpdateLabel()
    {
      this.NumberOfReadMetersLabel = Resources.ExecuteReadingOrder_ReadMeters + " " + (object) this.GetNumberOfReadMetersFromStructure() + " " + Resources.ExecuteReadingOrder_of + " " + (object) this._selectedOrder.DevicesCount;
    }

    private void CreateMessage(ActionSyncFinished messageFinished)
    {
      switch (messageFinished.Message.MessageType)
      {
        case MessageTypeEnum.Success:
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(messageFinished.Message.MessageText);
          break;
        case MessageTypeEnum.Warning:
          if (!messageFinished.ContinueScanning)
            this.StopReading();
          MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Warning_Title, messageFinished.Message.MessageText, false);
          break;
      }
    }

    private void RefreshStructureBytes(StructureBytesUpdated update)
    {
      if (update.StructureBytes != null)
      {
        this.StructureBytes = update.StructureBytes;
        this._selectedOrder.StructureBytes = update.StructureBytes;
      }
      if (update.MeterReadByWalkBy != Guid.Empty)
        this._blueMeters.Add(update.MeterReadByWalkBy);
      if (update.SerialNumberRead != null)
      {
        Guid key = this._metersToBeRead.FirstOrDefault<KeyValuePair<Guid, string>>((Func<KeyValuePair<Guid, string>, bool>) (item => item.Value == update.SerialNumberRead)).Key;
        ExecuteOrderStructureNode foundNode = (ExecuteOrderStructureNode) null;
        this.FindItemInOrderByGuid(key, this.StructureForSelectedOrder, ref foundNode);
        ReadingValueStatusEnum? status1;
        int num;
        if (foundNode != null)
        {
          status1 = foundNode.Status;
          ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.ok;
          num = status1.GetValueOrDefault() == readingValueStatusEnum ? (!status1.HasValue ? 1 : 0) : 1;
        }
        else
          num = 0;
        if (num != 0)
        {
          foundNode.Status = new ReadingValueStatusEnum?(ReadingValueStatusEnum.ok);
          OrderSerializableStructure serializableStructure = StructuresHelper.DeserializeStructure(this.StructureBytes);
          OrderSerializableStructure orderSerializableStructure = serializableStructure;
          Guid meterId = foundNode.MeterId;
          status1 = foundNode.Status;
          ReadingValueStatusEnum? status2 = new ReadingValueStatusEnum?(status1.Value);
          this.SetMeterSerializableDTOStatus(orderSerializableStructure, meterId, status2);
          this.SetMeterSerializableDTOIsConfigured(serializableStructure, foundNode.MeterId, update.IsConfigured);
          this.StructureBytes = StructuresHelper.SerializeStructure(serializableStructure);
          this._selectedOrder.StructureBytes = this.StructureBytes;
          OrdersManager.SetImageAndColor(foundNode, Brushes.LightGreen, ImageHelper.Instance.GetBitmapImageFromFiles(new string[1]
          {
            "pack://application:,,,/Styles;component/Images/StructureIcons/meter.png"
          }));
          this.OnPropertyChanged("StructureForSelectedOrder");
          EventPublisher.Publish<ProgressBarItemDone>(new ProgressBarItemDone()
          {
            Item = update.SerialNumberRead
          }, (IViewModel) this);
        }
      }
      if (!update.AnyReadingValuesRead)
        return;
      this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(string.Format(Resources.MSS_Client_Reading_Values_Received_Message, (object) update.SerialNumberRead));
    }

    private void HandleErrorDuringReading(ErrorDuringReading error)
    {
      if (error.SerialNumber == null)
        return;
      Guid key = this._metersToBeRead.FirstOrDefault<KeyValuePair<Guid, string>>((Func<KeyValuePair<Guid, string>, bool>) (item => item.Value == error.SerialNumber)).Key;
      ExecuteOrderStructureNode foundNode = (ExecuteOrderStructureNode) null;
      this.FindItemInOrderByGuid(key, this.StructureForSelectedOrder, ref foundNode);
      if (foundNode != null)
      {
        if (error.ErrorMessage == ReadingValueStatusEnum.MissingTranslationRules.ToString())
          this.SetMeterImageStatusAndDisplayWarningMessage(foundNode, Brushes.Yellow, ReadingValueStatusEnum.MissingTranslationRules, Resources.MSS_ReaderManager_MissingTranslationRule, error.SerialNumber);
        else
          this.SetMeterImageStatusAndDisplayWarningMessage(foundNode, Brushes.OrangeRed, ReadingValueStatusEnum.nok, Resources.MSS_NoValidReadingValuesReceived, error.SerialNumber);
        if (foundNode.Status.HasValue)
          this.SetMeterSerializableDTOStatusInStructureBytes(foundNode.MeterId, new ReadingValueStatusEnum?(foundNode.Status.Value));
        this._selectedOrder.StructureBytes = this.StructureBytes;
        this._selectedOrder.Status = StatusOrderEnum.InProgress;
        this.GetOrderManagerInstance().EditOrder(this._selectedOrder, new bool?(false));
        this.OnPropertyChanged("StructureForSelectedOrder");
        EventPublisher.Publish<ProgressBarItemDone>(new ProgressBarItemDone()
        {
          Item = error.SerialNumber
        }, (IViewModel) this);
      }
    }

    private void FindItemInOrderByGuid(
      Guid id,
      ObservableCollection<ExecuteOrderStructureNode> structure,
      ref ExecuteOrderStructureNode foundNode)
    {
      foreach (ExecuteOrderStructureNode orderStructureNode in (Collection<ExecuteOrderStructureNode>) structure)
      {
        if (orderStructureNode.Id == id)
        {
          foundNode = orderStructureNode;
          break;
        }
        this.FindItemInOrderByGuid(id, orderStructureNode.Children, ref foundNode);
      }
    }

    protected StructuresManager GetStructureManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    protected GMMManager GetGMMManagerInstance() => new GMMManager(this._repositoryFactory);

    protected OrdersManager GetOrderManagerInstance() => new OrdersManager(this._repositoryFactory);

    protected ReadingValuesManager GetReadingValuesManagerInstance()
    {
      return new ReadingValuesManager(this._repositoryFactory);
    }

    public byte[] StructureBytes
    {
      get => this._structureBytes;
      set
      {
        this._structureBytes = value;
        this.OnPropertyChanged(nameof (StructureBytes));
      }
    }

    public ObservableCollection<ExecuteOrderStructureNode> StructureForSelectedOrder
    {
      get => this._nodesList;
      set
      {
        this._nodesList = value;
        this.OnPropertyChanged(nameof (StructureForSelectedOrder));
      }
    }

    public ObservableCollection<ExecuteOrderStructureNode> SelectedMeterList
    {
      get
      {
        this._selectedMeterList = new ObservableCollection<ExecuteOrderStructureNode>();
        if (this.SelectedNodes != null && this.SelectedNodes.Count > 1)
        {
          foreach (object selectedNode in (Collection<object>) this.SelectedNodes)
          {
            if (selectedNode is ExecuteOrderStructureNode orderStructureNode && orderStructureNode.IsMeter())
              this._selectedMeterList.Add(orderStructureNode);
          }
          ObservableCollection<ExecuteOrderStructureNode> unselectedSubnodes = new ObservableCollection<ExecuteOrderStructureNode>();
          foreach (ExecuteOrderStructureNode selectedMeter in (Collection<ExecuteOrderStructureNode>) this._selectedMeterList)
            TypeHelperExtensionMethods.ForEach<ExecuteOrderStructureNode>(OrdersHelper.Descendants(selectedMeter), (Action<ExecuteOrderStructureNode>) (item =>
            {
              if (!item.IsMeter() || item.SerialNumber == null || !GMMHelper.IsDeviceIncludedInLicense(item.DeviceType, this._importableDeviceModelNameList) || this.SelectedNodes.Any<object>((Func<object, bool>) (x => x is ExecuteOrderStructureNode && (x as ExecuteOrderStructureNode).Id == item.Id)))
                return;
              unselectedSubnodes.Add(item);
            }));
          TypeHelperExtensionMethods.ForEach<ExecuteOrderStructureNode>((IEnumerable<ExecuteOrderStructureNode>) unselectedSubnodes, (Action<ExecuteOrderStructureNode>) (item => this._selectedMeterList.Add(item)));
        }
        else if (this.SelectedItem != null)
        {
          if (this.SelectedItem.IsMeter())
            this._selectedMeterList.Add(this.SelectedItem);
          else
            TypeHelperExtensionMethods.ForEach<ExecuteOrderStructureNode>(OrdersHelper.Descendants(this.SelectedItem), (Action<ExecuteOrderStructureNode>) (n =>
            {
              if (!n.IsMeter() || n.SerialNumber == null || !GMMHelper.IsDeviceIncludedInLicense(n.DeviceType, this._importableDeviceModelNameList))
                return;
              this._selectedMeterList.Add(n);
            }));
        }
        if (this._selectedMeterList.Count <= 0)
          return this._selectedMeterList;
        List<ExecuteOrderStructureNode> list = this._selectedMeterList.Reverse<ExecuteOrderStructureNode>().ToList<ExecuteOrderStructureNode>();
        ObservableCollection<ExecuteOrderStructureNode> selectedMeterList = new ObservableCollection<ExecuteOrderStructureNode>();
        foreach (ExecuteOrderStructureNode orderStructureNode in list)
          selectedMeterList.Add(orderStructureNode);
        return selectedMeterList;
      }
    }

    public IEnumerable<ProfileType> ConnectionProfileCollection
    {
      get => this._connectionProfileCollection;
      set
      {
        this._connectionProfileCollection = value;
        this.OnPropertyChanged(nameof (ConnectionProfileCollection));
        this.IsChangeProfileTypeVisible = value == null || !value.Any<ProfileType>() ? Visibility.Collapsed : Visibility.Visible;
      }
    }

    private IEnumerable<ProfileType> InitConnectionProfileCollection()
    {
      List<long> filterListForOrder = this.GetGMMManagerInstance().GetFilterListForOrder(this._selectedOrder.Id);
      return (IEnumerable<ProfileType>) GMMHelper.GetProfileTypes(this._deviceManager, this.GetReadableMeters(), filterListForOrder, this._selectedEquipmentModel, this._selectedOrder.StructureType);
    }

    public ProfileType SelectedConnectionProfile
    {
      get => this._selectedConnectionProfile;
      set
      {
        if (value != null)
          this._selectedConnectionProfile = this._connectionProfileCollection.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (_ => _.Name == value.Name));
        this.IsStartButtonEnabled = this.SelectedItem != null && this._selectedConnectionProfile != null;
        if (this._selectedConnectionProfile != null)
          this._transceiverType = this.GetTransceiverType();
        this.CloseGMMConnection();
        this.OnPropertyChanged(nameof (SelectedConnectionProfile));
      }
    }

    public Visibility IsChangeProfileTypeVisible
    {
      get => this._isChangeProfileTypeVisible;
      set
      {
        this._isChangeProfileTypeVisible = value;
        this.OnPropertyChanged(nameof (IsChangeProfileTypeVisible));
      }
    }

    public ExecuteOrderStructureNode SelectedItem
    {
      get => this._selectedItem;
      set
      {
        this._selectedItem = value;
        if (this._selectedItem != null)
        {
          this.IsTenantSelected = this._selectedItem.NodeType == StructureNodeTypeEnum.Tenant;
          this.IsDeviceSelected = this._selectedItem.IsMeter();
          this.IsTenantOrDeviceSelected = this.IsTenantSelected || this.IsDeviceSelected;
          if (this.IsDeviceSelected && this.IsDeviceSelected && this.SelectedItem.MeterId != Guid.Empty)
          {
            this.NewRowPosition = "Bottom";
            DeviceTypeEnum? deviceType = this.SelectedItem.DeviceType;
            DeviceTypeEnum deviceTypeEnum1 = DeviceTypeEnum.MinomessMicroRadio3;
            int num;
            if ((deviceType.GetValueOrDefault() == deviceTypeEnum1 ? (deviceType.HasValue ? 1 : 0) : 0) == 0)
            {
              deviceType = this.SelectedItem.DeviceType;
              DeviceTypeEnum deviceTypeEnum2 = DeviceTypeEnum.M7;
              if ((deviceType.GetValueOrDefault() == deviceTypeEnum2 ? (deviceType.HasValue ? 1 : 0) : 0) == 0)
              {
                deviceType = this.SelectedItem.DeviceType;
                DeviceTypeEnum deviceTypeEnum3 = DeviceTypeEnum.M6;
                num = deviceType.GetValueOrDefault() == deviceTypeEnum3 ? (deviceType.HasValue ? 1 : 0) : 0;
                goto label_6;
              }
            }
            num = 1;
label_6:
            this.CanUserInsertRows = num == 0 ? this.ReadingValuesCollection == null || this.ReadingValuesCollection.Count < this.registerCollectionCount : this.ReadingValuesCollection.Count < 1;
          }
        }
        else
          this.IsDeviceSelected = false;
        this.IsStartButtonEnabled = this._selectedItem != null && this.SelectedConnectionProfile != null && this.IsReadingDeviceViaCBEnabled && !this.IsStopButtonEnabled;
        this.OnPropertyChanged("IsRegisterVisible");
        this.OnPropertyChanged("IsActualValueVisible");
        this.OnPropertyChanged("IsDueDateValueVisible");
        this.OnPropertyChanged("IsUnitVisible");
        this.OnPropertyChanged("GetUnitCollection");
        if (!this.IsStopButtonEnabled)
        {
          if (this._structureNodeEquipmentSettings != null)
          {
            this.ConnectionProfileCollection = this.InitConnectionProfileCollection();
            this._selectedConnectionProfile = this._connectionProfileCollection.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (item => item.Name == this._selectedConnectionProfile?.Name));
            if (!string.IsNullOrEmpty(this._structureNodeEquipmentSettings.ReadingProfileParams) && this.SelectedConnectionProfile != null)
              MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateProfileTypeWithSavedParams(this.SelectedConnectionProfile, this._structureNodeEquipmentSettings.ReadingProfileParams);
            else if (string.IsNullOrEmpty(this._structureNodeEquipmentSettings.ReadingProfileParams) && this._structureNodeEquipmentSettings?.SystemName == "M-Bus" && this._structureNodeEquipmentSettings.ScanParams != null && this.SelectedConnectionProfile != null)
              MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateProfileTypeWithSavedParams(this.SelectedConnectionProfile, this._structureNodeEquipmentSettings.ScanParams);
          }
          this.OnPropertyChanged("SelectedConnectionProfile");
        }
        this.OnPropertyChanged(nameof (SelectedItem));
        if (this._selectedItem == null)
          return;
        this._currentlySelectedItem = this._selectedItem.Id;
      }
    }

    public ObservableCollection<object> SelectedNodes
    {
      get => this._selectedNodes;
      set
      {
        this._selectedNodes = value;
        if (this._selectedNodes.Count > 1)
        {
          this.ConnectionProfileCollection = this.InitConnectionProfileCollection();
          this._selectedConnectionProfile = this._connectionProfileCollection.FirstOrDefault<ProfileType>((Func<ProfileType, bool>) (item => item.Name == this._selectedConnectionProfile?.Name));
          this.OnPropertyChanged("SelectedConnectionProfile");
        }
        this.IsChangeDeviceModelParametersEnabled = this._selectedNodes != null && this._selectedNodes.Count == 1 && this._selectedNodes.First<object>() is ExecuteOrderStructureNode && (this._selectedNodes.First<object>() as ExecuteOrderStructureNode).NodeType == StructureNodeTypeEnum.Meter;
      }
    }

    public bool IsDeviceSelected
    {
      get => this._isDeviceSelected;
      set
      {
        this._isDeviceSelected = value;
        this.OnPropertyChanged(nameof (IsDeviceSelected));
        if (this._isDeviceSelected)
        {
          if (this.SelectedItem.IsMeter() && this.SelectedItem.DeviceType.HasValue && GMMHelper.GetDeviceModel(this.SelectedItem.DeviceType.Value, DeviceModelTags.MBus | DeviceModelTags.wMBus) != null)
          {
            this.IsReadingValuesGridVisible = false;
            this.IsGenericMbusReadingValuesGridVisible = true;
          }
          else
          {
            this.IsReadingValuesGridVisible = this.IsRegisterVisible || this.IsActualValueVisible || this.IsDueDateValueVisible || this.IsUnitVisible;
            this.IsGenericMbusReadingValuesGridVisible = false;
          }
          if (this.IsReadingValuesGridVisible)
            this.ReadingValuesCollection = this.GetReadingValuesCollection();
          if (this.IsGenericMbusReadingValuesGridVisible)
            this.GenericMbusReadingValuesCollection = this.GetGenericMbusReadingValuesCollection();
        }
        else
        {
          this.IsReadingValuesGridVisible = false;
          this.IsGenericMbusReadingValuesGridVisible = false;
        }
        this.IsSaveButtonVisible = this.IsReadingValuesGridVisible || this.IsGenericMbusReadingValuesGridVisible;
      }
    }

    public bool IsTenantSelected { get; set; }

    public bool IsTenantOrDeviceSelected
    {
      get => this._isTenantOrDeviceSelected;
      set
      {
        this._isTenantOrDeviceSelected = value;
        this.OnPropertyChanged(nameof (IsTenantOrDeviceSelected));
      }
    }

    public RadObservableCollection<ReadingValuesDTO> ReadingValuesCollection
    {
      get => this._readingValuesCollection;
      set
      {
        this._readingValuesCollection = value;
        this.OnPropertyChanged(nameof (ReadingValuesCollection));
      }
    }

    public ObservableCollection<MeterReadingValueDTO> GenericMbusReadingValuesCollection
    {
      get => this._genericMbusReadingValuesCollection;
      set
      {
        this._genericMbusReadingValuesCollection = value;
        this.OnPropertyChanged(nameof (GenericMbusReadingValuesCollection));
      }
    }

    public IEnumerable<ValueIdent.ValueIdPart_MeterType> GetRegisterColection
    {
      get
      {
        this._registerCollection = (IEnumerable<ValueIdent.ValueIdPart_MeterType>) ValueIdentHelper.GetMeterTypeEnumerableAsValueIdPart().ToList<ValueIdent.ValueIdPart_MeterType>();
        return this._registerCollection;
      }
      set
      {
        this._registerCollection = value;
        this.OnPropertyChanged(nameof (GetRegisterColection));
      }
    }

    public IEnumerable<MeasureUnitDTO> GetUnitCollection
    {
      get
      {
        return this.SelectedItem != null ? MeasureUnitsHelper.GetMeasureUnits(this._repositoryFactory.GetRepository<MeasureUnit>()) : (IEnumerable<MeasureUnitDTO>) null;
      }
    }

    public IEnumerable<ValueIdentString> PhysicalQuantitiesEnumerable
    {
      get => this._physicalQuantitiesEnumerable;
    }

    public IEnumerable<ValueIdentString> MeterTypeEnumerable => this._meterTypeEnumerable;

    public IEnumerable<ValueIdentString> CalculationEnumerable => this._calculationEnumerable;

    public IEnumerable<ValueIdentString> CalculationStartEnumerable
    {
      get => this._calculationStartEnumerable;
    }

    public IEnumerable<ValueIdentString> StorageIntervalEnumerable
    {
      get => this._storageIntervalEnumerable;
    }

    public IEnumerable<ValueIdentString> CreationEnumerable => this._creationEnumerable;

    public bool IsRegisterVisible
    {
      get
      {
        return this.GetVisibilityForProperty(DeviceTypeVisibilityHelper.GetPropertyName<ValueIdent.ValueIdPart_MeterType>((Expression<Func<ValueIdent.ValueIdPart_MeterType>>) (() => System.Linq.Expressions.Expression.New(typeof (ReadingValuesDTO)).Register)));
      }
    }

    public bool IsActualValueVisible
    {
      get
      {
        return this.GetVisibilityForProperty(DeviceTypeVisibilityHelper.GetPropertyName<double>((Expression<Func<double>>) (() => System.Linq.Expressions.Expression.New(typeof (ReadingValuesDTO)).ActualValue)));
      }
    }

    public bool IsDueDateValueVisible
    {
      get
      {
        return this.GetVisibilityForProperty(DeviceTypeVisibilityHelper.GetPropertyName<double>((Expression<Func<double>>) (() => System.Linq.Expressions.Expression.New(typeof (ReadingValuesDTO)).DueDateValue)));
      }
    }

    public bool IsUnitVisible
    {
      get
      {
        return this.GetVisibilityForProperty(DeviceTypeVisibilityHelper.GetPropertyName<Guid>((Expression<Func<Guid>>) (() => System.Linq.Expressions.Expression.New(typeof (ReadingValuesDTO)).UnitId)));
      }
    }

    public bool IsReadingValuesGridVisible
    {
      get => this._isReadingValuesGridVisible;
      set
      {
        this._isReadingValuesGridVisible = value;
        this.OnPropertyChanged(nameof (IsReadingValuesGridVisible));
      }
    }

    public bool IsSaveButtonVisible
    {
      get => this._isSaveButtonVisible;
      set
      {
        this._isSaveButtonVisible = value;
        this.OnPropertyChanged(nameof (IsSaveButtonVisible));
      }
    }

    public bool IsGenericMbusReadingValuesGridVisible
    {
      get => this._isGenericMbusReadingValuesGridVisible;
      set
      {
        this._isGenericMbusReadingValuesGridVisible = value;
        this.OnPropertyChanged(nameof (IsGenericMbusReadingValuesGridVisible));
      }
    }

    public string NewRowPosition
    {
      get => this._newRowPosition;
      set
      {
        this._newRowPosition = value;
        this.OnPropertyChanged(nameof (NewRowPosition));
      }
    }

    public bool CanUserInsertRows
    {
      get => this._canUserInsertRows;
      set
      {
        this._canUserInsertRows = value;
        this.OnPropertyChanged(nameof (CanUserInsertRows));
      }
    }

    public MVVM.ViewModel.ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public string NumberOfReadMetersLabel
    {
      get => this._numberOfReadMetersLabel;
      set
      {
        this._numberOfReadMetersLabel = value;
        this.OnPropertyChanged(nameof (NumberOfReadMetersLabel));
      }
    }

    public bool IsPhysicalStructure
    {
      get => this._isPhysicalStructure;
      set
      {
        this._isPhysicalStructure = value;
        this.OnPropertyChanged(nameof (IsPhysicalStructure));
      }
    }

    public ICommand CloseCommand { get; private set; }

    private int GetNumberOfReadMetersFromStructure()
    {
      int metersFromStructure = 0;
      foreach (ExecuteOrderStructureNode node in (Collection<ExecuteOrderStructureNode>) this.StructureForSelectedOrder)
      {
        int noOfReadMeters = this.IsMeterRead(node) ? 1 : 0;
        this.WalkStructure(node, ref noOfReadMeters);
        metersFromStructure += noOfReadMeters;
      }
      return metersFromStructure;
    }

    private void WalkStructure(ExecuteOrderStructureNode node, ref int noOfReadMeters)
    {
      foreach (ExecuteOrderStructureNode child in (Collection<ExecuteOrderStructureNode>) node.Children)
      {
        if (this.IsMeterRead(child))
          ++noOfReadMeters;
        if (child.Children.Count > 0)
          this.WalkStructure(child, ref noOfReadMeters);
      }
    }

    private bool IsMeterRead(ExecuteOrderStructureNode node)
    {
      int num;
      if (node.IsMeter())
      {
        ReadingValueStatusEnum? status = node.Status;
        ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.ok;
        if ((status.GetValueOrDefault() == readingValueStatusEnum ? (status.HasValue ? 1 : 0) : 0) != 0)
        {
          num = node.ReadingEnabled ? 1 : 0;
          goto label_4;
        }
      }
      num = 0;
label_4:
      return num != 0;
    }

    public bool IsStartButtonEnabled
    {
      get => this._isStartButtonEnabled;
      set
      {
        this._isStartButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsStartButtonEnabled));
      }
    }

    public bool IsChangeDeviceModelParametersEnabled
    {
      get => this._isChangeDeviceModelParametersEnabled;
      set
      {
        int num;
        if (value)
        {
          StructureTypeEnum? structureType = this._selectedOrder.StructureType;
          StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
          num = structureType.GetValueOrDefault() == structureTypeEnum ? (!structureType.HasValue ? 1 : 0) : 1;
        }
        else
          num = 0;
        this._isChangeDeviceModelParametersEnabled = num != 0;
        this.OnPropertyChanged(nameof (IsChangeDeviceModelParametersEnabled));
      }
    }

    public ObservableCollection<ReadingValuesDTO> ReadingValuesToDeleteCollection
    {
      get => this._readingValuesToDeleteCollection;
      set
      {
        this._readingValuesToDeleteCollection = value;
        this.OnPropertyChanged(nameof (ReadingValuesToDeleteCollection));
      }
    }

    public ObservableCollection<MeterReadingValueDTO> MeterReadingValuesToDeleteCollection
    {
      get => this._meterReadingValuesToDeleteCollection;
      set
      {
        this._meterReadingValuesToDeleteCollection = value;
        this.OnPropertyChanged(nameof (MeterReadingValuesToDeleteCollection));
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

    public bool IsStopButtonEnabled
    {
      get => this._isStopButtonEnabled;
      set
      {
        this._isStopButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsStopButtonEnabled));
      }
    }

    public TransceiverType GetTransceiverType()
    {
      this.GetDeviceModel();
      if (this._selectedEquipmentModel != null && this._selectedConnectionProfile != null)
      {
        Enum.TryParse<TransceiverType>(this._selectedConnectionProfile.Parameters[ConnectionProfileParameter.TransceiverType], true, out this._transceiverType);
        string newValue = (string) null;
        if (this._transceiverType == TransceiverType.Reader)
          newValue = "DefaultReader";
        if (this._transceiverType == TransceiverType.Receiver)
          newValue = "DefaultReceiver";
        this._deviceManager.SetFilter(newValue);
      }
      return this._transceiverType;
    }

    private DeviceModel GetDeviceModel()
    {
      ExecuteOrderStructureNode orderStructureNode;
      if (this.SelectedItem != null)
      {
        ExecuteOrderStructureNode selectedItem = this.SelectedItem;
        orderStructureNode = !selectedItem.IsMeter() ? this.GetFirstMeterInStructure() : selectedItem;
      }
      else
        orderStructureNode = this.GetFirstMeterInStructure();
      DeviceModel deviceModel;
      if (orderStructureNode != null && orderStructureNode.DeviceType.HasValue)
      {
        string deviceModelName = orderStructureNode.DeviceType.Value.GetGMMDeviceModelName();
        deviceModel = this._deviceManager.GetDeviceModels().FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.Name == deviceModelName));
      }
      else
        deviceModel = this._deviceManager.GetDeviceModels().FirstOrDefault<DeviceModel>((Func<DeviceModel, bool>) (x => x.Name == "M7"));
      return deviceModel;
    }

    private ExecuteOrderStructureNode GetFirstMeterInStructure()
    {
      IEnumerable<ExecuteOrderStructureNode> orderStructureNodes = OrdersHelper.Descendants(this.StructureForSelectedOrder.First<ExecuteOrderStructureNode>());
      List<ExecuteOrderStructureNode> meterList = new List<ExecuteOrderStructureNode>();
      List<string> importableDeviceModelNameList = GMMHelper.GetDeviceModelNameList(this._selectedOrder.StructureType);
      TypeHelperExtensionMethods.ForEach<ExecuteOrderStructureNode>(orderStructureNodes, (Action<ExecuteOrderStructureNode>) (n =>
      {
        if (!n.IsMeter() || !GMMHelper.IsDeviceIncludedInLicense(n.DeviceType, importableDeviceModelNameList))
          return;
        meterList.Add(n);
      }));
      return meterList.FirstOrDefault<ExecuteOrderStructureNode>();
    }

    public bool IsReadingDeviceViaCBEnabled
    {
      get => this._isReadingDeviceViaCBEnabled;
      set
      {
        this._isReadingDeviceViaCBEnabled = value;
        this.OnPropertyChanged(nameof (IsReadingDeviceViaCBEnabled));
      }
    }

    public bool IsReadingStarted
    {
      get => this._isReadingStarted;
      set
      {
        this._isReadingStarted = value;
        this.OnPropertyChanged(nameof (IsReadingStarted));
      }
    }

    public bool IsShortDeviceNoVisible
    {
      get => this._isShortDeviceNoVisible;
      set
      {
        this._isShortDeviceNoVisible = value;
        this.OnPropertyChanged(nameof (IsShortDeviceNoVisible));
      }
    }

    public ProgressBarCtrl ProgressBar
    {
      get => this._progressBar;
      set
      {
        this._progressBar = value;
        this.OnPropertyChanged(nameof (ProgressBar));
      }
    }

    public override ICommand CancelWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this.CleanupBeforeClosing();
          this.OnRequestClose(false);
        }));
      }
    }

    private void CleanupBeforeClosing()
    {
      this.CloseGMMConnection();
      this.SetOrderStatus(this.StructureForSelectedOrder, this._selectedOrder);
      this.SaveStructureBytesForOrder();
    }

    private void CloseGMMConnection()
    {
      if (this._readerMinoConnect != null)
      {
        this._readerMinoConnect.CloseConnection();
        this._readerMinoConnect.StopReadingValues();
      }
      if (this._receiverMinoConnect == null)
        return;
      this._receiverMinoConnect.StopReadingValues();
    }

    private void SaveStructureBytesForOrder()
    {
      MSS.Core.Model.Orders.Order byId = this._repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>().GetById((object) this._selectedOrder.Id);
      if (byId == null)
        return;
      byId.StructureBytes = this.StructureBytes;
      byId.RootStructureNodeId = this._selectedOrder.RootStructureNodeId;
      this._repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>().Update(byId);
    }

    public ICommand ViewMessagesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ExecuteOrderStructureNode orderStructureNode1 = parameter as ExecuteOrderStructureNode;
          List<ExecuteOrderStructureNode> metersList = new List<ExecuteOrderStructureNode>();
          if (orderStructureNode1 != null)
          {
            if (orderStructureNode1.IsMeter())
              metersList.Add(orderStructureNode1);
            foreach (ExecuteOrderStructureNode child in (Collection<ExecuteOrderStructureNode>) orderStructureNode1.Children)
            {
              if (child.IsMeter())
                metersList.Add(child);
              this.WalkStructure(child, ref metersList);
            }
          }
          List<OrderMessage> orderMessageList = new List<OrderMessage>();
          IRepository<OrderMessage> repository = this._repositoryFactory.GetRepository<OrderMessage>();
          foreach (ExecuteOrderStructureNode orderStructureNode2 in metersList)
          {
            ExecuteOrderStructureNode meter = orderStructureNode2;
            orderMessageList.AddRange((IEnumerable<OrderMessage>) repository.SearchFor((Expression<Func<OrderMessage, bool>>) (item => item.Order.Id == this._selectedOrder.Id && item.Meter.Id == meter.MeterId)));
          }
          orderMessageList.Sort((Comparison<OrderMessage>) ((item1, item2) => item2.Timepoint.CompareTo(item1.Timepoint)));
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<OrderMessagesViewModel>((IParameter) new ConstructorArgument("orderMessages", (object) orderMessageList)));
        }));
      }
    }

    private void WalkStructure(
      ExecuteOrderStructureNode currentNode,
      ref List<ExecuteOrderStructureNode> metersList)
    {
      foreach (ExecuteOrderStructureNode child in (Collection<ExecuteOrderStructureNode>) currentNode.Children)
      {
        if (child.IsMeter())
          metersList.Add(child);
        this.WalkStructure(child, ref metersList);
      }
    }

    public ICommand ChangeProfileTypeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.SelectedConnectionProfile != null)
          {
            if (this.SelectedConnectionProfile.ChangeableParameters != null)
            {
              ProfileTypeViewModel profileTypeViewModel = DIConfigurator.GetConfigurator().Get<ProfileTypeViewModel>((IParameter) new ConstructorArgument("profileTypeCollection", (object) this.ConnectionProfileCollection), (IParameter) new ConstructorArgument("selectedProfileType", (object) this.SelectedConnectionProfile));
              bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) profileTypeViewModel);
              if (!(newModalDialog.HasValue & newModalDialog.Value))
                return;
              this.SelectedConnectionProfile = profileTypeViewModel.SelectedProfileType;
              if (this.SelectedConnectionProfile?.ChangeableParameters != null)
              {
                this._structureNodeEquipmentSettings.ReadingProfileParams = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(this.SelectedConnectionProfile.ChangeableParameters), this.SelectedConnectionProfile.ChangeableParameters);
                this._structureNodeEquipmentSettings.ReadingProfileName = this.SelectedConnectionProfile.Name;
                this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().Update(this._structureNodeEquipmentSettings);
              }
            }
            else
              this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_CannotModifyChangeableParamsDueToLicense), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
          }
          else if (!this.ConnectionProfileCollection.Any<ProfileType>())
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_CannotModifyChangeableParamsDueToLicense), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
          else
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<GenericMessageViewModel>((IParameter) new ConstructorArgument("title", (object) Resources.MSS_DeleteStructure_Warning_Title), (IParameter) new ConstructorArgument("message", (object) Resources.MSS_Client_PleaseSelectValueFromReadingDevicesVia), (IParameter) new ConstructorArgument("isCancelButtonVisible", (object) false)));
        }));
      }
    }

    public ICommand ChangeDeviceModelParametersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          ExecuteOrderStructureNode selectedNode = this.SelectedNodes.First<object>() as ExecuteOrderStructureNode;
          if (selectedNode == null || selectedNode.NodeType != StructureNodeTypeEnum.Meter)
            return;
          StructureNodeEquipmentSettings entity1 = this._equipmentSettingsForMeters.FirstOrDefault<KeyValuePair<Guid, StructureNodeEquipmentSettings>>((Func<KeyValuePair<Guid, StructureNodeEquipmentSettings>, bool>) (item => item.Key == selectedNode.MeterId)).Value;
          MSS.Core.Model.Meters.Meter selectedMeter = this._repositoryFactory.GetRepository<MSS.Core.Model.Meters.Meter>().FirstOrDefault((Expression<Func<MSS.Core.Model.Meters.Meter, bool>>) (item => item.Id == selectedNode.MeterId && !item.IsDeactivated));
          if (selectedMeter != null)
          {
            DeviceModel deviceModel = this._deviceManager.GetDeviceModel(selectedMeter.DeviceType.GetGMMDeviceModelName());
            if (entity1 != null && !string.IsNullOrEmpty(entity1.DeviceModelReadingParams))
              deviceModel = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateDeviceModelWithSavedParams(deviceModel, entity1.DeviceModelReadingParams);
            if (deviceModel != null && deviceModel.ChangeableParameters != null && deviceModel.ChangeableParameters.Any<ChangeableParameter>())
            {
              DeviceModelChangeableParametersViewModel parametersViewModel = DIConfigurator.GetConfigurator().Get<DeviceModelChangeableParametersViewModel>((IParameter) new ConstructorArgument("selectedDeviceModel", (object) deviceModel));
              bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) parametersViewModel);
              if (newModalDialog.HasValue & newModalDialog.Value)
              {
                StructureNode structureNodeForMeter = this._repositoryFactory.GetRepository<StructureNode>().FirstOrDefault((Expression<Func<StructureNode, bool>>) (item => item.EntityId == selectedMeter.Id));
                if (structureNodeForMeter != null)
                {
                  List<Config> changeableParameters = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(parametersViewModel.SelectedDeviceModel.ChangeableParameters);
                  if (entity1 == null)
                  {
                    entity1 = new StructureNodeEquipmentSettings();
                    entity1.StructureNode = structureNodeForMeter;
                  }
                  entity1.DeviceModelReadingParams = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(changeableParameters, parametersViewModel.SelectedDeviceModel.ChangeableParameters);
                  IRepository<StructureNodeEquipmentSettings> repository = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>();
                  ISession session = this._repositoryFactory.GetSession();
                  session.BeginTransaction();
                  StructureNodeEquipmentSettings entity2 = repository.FirstOrDefault((Expression<Func<StructureNodeEquipmentSettings, bool>>) (item => item.StructureNode.Id == structureNodeForMeter.Id));
                  if (entity2 != null)
                  {
                    entity2.DeviceModelReadingParams = entity1.DeviceModelReadingParams;
                    repository.TransactionalUpdate(entity2);
                  }
                  else
                    repository.TransactionalInsert(entity1);
                  session.Transaction.Commit();
                  session.Clear();
                  this._equipmentSettingsForMeters[selectedNode.MeterId] = entity1;
                }
                else
                  MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_DeviceModelChangeableParams_CannotSaveToDB, false);
              }
            }
            else
              MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_DeviceModeChangeableParameters_ParamsAreNull, false);
          }
          else
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_DeviceModelChangeableParams_MeterHasBeenDeleted, false);
        }));
      }
    }

    private ObservableCollection<ExecuteOrderStructureNode> GetReadableMeters()
    {
      ObservableCollection<ExecuteOrderStructureNode> readableMeters = new ObservableCollection<ExecuteOrderStructureNode>();
      TypeHelperExtensionMethods.ForEach<ExecuteOrderStructureNode>((IEnumerable<ExecuteOrderStructureNode>) this.SelectedMeterList, (Action<ExecuteOrderStructureNode>) (item =>
      {
        if (!item.ReadingEnabled)
          return;
        readableMeters.Add(item);
      }));
      return readableMeters;
    }

    private ObservableCollection<ExecuteOrderStructureNode> ConstructOrderedListOfMetersToBeRead(
      ObservableCollection<ExecuteOrderStructureNode> structureForOrder,
      Dictionary<Guid, string> metersToBeRead,
      List<ExecuteOrderStructureNode> unreadMeters)
    {
      ObservableCollection<ExecuteOrderStructureNode> collection = new ObservableCollection<ExecuteOrderStructureNode>();
      foreach (ExecuteOrderStructureNode orderStructureNode1 in (Collection<ExecuteOrderStructureNode>) structureForOrder)
      {
        ExecuteOrderStructureNode node = orderStructureNode1;
        if (node.IsMeter())
        {
          ExecuteOrderStructureNode orderStructureNode2 = unreadMeters.FirstOrDefault<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (m => m.SerialNumber == node.SerialNumber));
          if (orderStructureNode2 != null)
          {
            metersToBeRead.Add(orderStructureNode2.Id, orderStructureNode2.SerialNumber);
            collection.Add(orderStructureNode2);
          }
        }
        if (node.Children != null && node.Children.Any<ExecuteOrderStructureNode>())
          collection.AddRange<ExecuteOrderStructureNode>((IEnumerable<ExecuteOrderStructureNode>) this.ConstructOrderedListOfMetersToBeRead(node.Children, metersToBeRead, unreadMeters));
      }
      return collection;
    }

    public ICommand StartCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this._backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = false,
            WorkerSupportsCancellation = true
          };
          StructureNodeEquipmentSettings equipmentSettings = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().FirstOrDefault((Expression<Func<StructureNodeEquipmentSettings, bool>>) (i => i.StructureNode.Id == this._selectedOrder.RootStructureNodeId));
          if (equipmentSettings != null)
            equipmentSettings.ReadingProfileName = this.SelectedConnectionProfile?.Name;
          this._backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            args.Cancel |= this._backgroundWorker.CancellationPending;
            this._metersToBeRead.Clear();
            ObservableCollection<ExecuteOrderStructureNode> beRead = this.ConstructOrderedListOfMetersToBeRead(this.StructureForSelectedOrder, this._metersToBeRead, this.SelectedMeterList.Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (item =>
            {
              ReadingValueStatusEnum? status3 = item.Status;
              ReadingValueStatusEnum readingValueStatusEnum3 = ReadingValueStatusEnum.ok;
              if ((status3.GetValueOrDefault() == readingValueStatusEnum3 ? (!status3.HasValue ? 1 : 0) : 1) != 0)
              {
                ReadingValueStatusEnum? status4 = item.Status;
                ReadingValueStatusEnum readingValueStatusEnum4 = ReadingValueStatusEnum.notavailable;
                if ((status4.GetValueOrDefault() == readingValueStatusEnum4 ? (!status4.HasValue ? 1 : 0) : 1) != 0)
                  return item.ReadingEnabled;
              }
              return false;
            })).ToList<ExecuteOrderStructureNode>());
            if (this._metersToBeRead.Count > 0)
              this.ProgressBar.Start(this._metersToBeRead.Count != 1 ? (IProgressBarUpdater) new ListProgressBarUpdater(this._metersToBeRead) : (IProgressBarUpdater) new PercentProgressBarUpdater());
            bool flag = false;
            switch (this._transceiverType)
            {
              case TransceiverType.None:
                Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_ReadErrorMessage, false)));
                break;
              case TransceiverType.Reader:
                flag = this.StartReadingValuesReader(beRead);
                break;
              case TransceiverType.Receiver:
                flag = this.StartReadingValuesReceiver(beRead);
                break;
            }
            if (flag)
              return;
            this.StopReading();
          });
          this._backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            if (args.Cancelled)
            {
              this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_ExecuteReadingOrder_Cancelled);
            }
            else
            {
              if (args.Error == null)
                return;
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
            }
          });
          this._backgroundWorker.RunWorkerAsync();
        }));
      }
    }

    private bool StartReadingValuesReader(
      ObservableCollection<ExecuteOrderStructureNode> listOfMetersToBeRead)
    {
      if (this._metersToBeRead.Count == 0)
        Application.Current.Dispatcher.Invoke<bool>((Func<bool>) (() =>
        {
          MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_SelectedMetersAlreadyRead, false);
          return false;
        }));
      if (!this.isIRReadingStarted)
      {
        this._readerMinoConnect = new ReaderMinoConnectManager(this._repositoryFactory, this._deviceManager);
        this._readerMinoConnect.OnError += (EventHandler<string>) ((o, message) => Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(message))));
        this._readerMinoConnect.OnMissingTranslationRule += (EventHandler<string>) ((o, message) => Application.Current.Dispatcher.Invoke((Action) (() => this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(message))));
        this._readerMinoConnect.OnReadingFinished += (System.EventHandler) ((o, eventArgs) => Application.Current.Dispatcher.Invoke(new Action(this.StopReading)));
        this.isIRReadingStarted = true;
      }
      StructureTypeEnum? structureType = this._selectedOrder.StructureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
      if (!(structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue ? this._readerMinoConnect.StartReadingValues(listOfMetersToBeRead, this._selectedOrder, this.SelectedConnectionProfile, this._selectedEquipmentModel, this._structureNodeEquipmentSettings.ScanParams) : this._readerMinoConnect.StartReadingValues(listOfMetersToBeRead, this._selectedOrder, this.SelectedConnectionProfile, this._selectedEquipmentModel, this._equipmentSettingsForMeters, this._structureNodeEquipmentSettings.ScanParams)))
      {
        Application.Current.Dispatcher.Invoke<bool>((Func<bool>) (() =>
        {
          MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_DefaultEquipmentNotSet, false);
          return false;
        }));
      }
      else
      {
        this.IsStartButtonEnabled = false;
        this.IsStopButtonEnabled = true;
        this.IsReadingDeviceViaCBEnabled = false;
        this.IsReadingStarted = true;
      }
      return true;
    }

    private bool StartReadingValuesReceiver(
      ObservableCollection<ExecuteOrderStructureNode> listOfMetersToBeRead)
    {
      if (this._metersToBeRead.Count == 0)
      {
        Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_SelectedMetersAlreadyRead, false)));
        return false;
      }
      this._receiverMinoConnect = new ReceiverMinoConnectManager(this._repositoryFactory, this._selectedOrder, this.SelectedItem.Id, this.SelectedConnectionProfile);
      this._receiverMinoConnect.OnJobFinished += (System.EventHandler) ((o, eventArgs) => Application.Current.Dispatcher.Invoke(new Action(this.StopReading)));
      if (!this._receiverMinoConnect.StartReadingValues(listOfMetersToBeRead, this._selectedOrder, this.SelectedConnectionProfile, this._selectedEquipmentModel))
      {
        Application.Current.Dispatcher.Invoke<bool>((Func<bool>) (() =>
        {
          MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_DefaultEquipmentNotSet, false);
          return false;
        }));
      }
      else
      {
        this.IsStartButtonEnabled = false;
        this.IsStopButtonEnabled = true;
        this.IsReadingDeviceViaCBEnabled = false;
        this.IsReadingStarted = true;
      }
      return true;
    }

    public ICommand StopCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          this._backgroundWorker?.CancelAsync();
          this.StopReading();
        }));
      }
    }

    public ICommand SaveReadingValuesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          ProfileType connectionProfile = this.SelectedConnectionProfile;
          if (this.IsReadingValuesGridVisible)
            this.SaveReadingValuesGrid();
          else if (this.IsGenericMbusReadingValuesGridVisible)
            this.SaveGenericMbusReadingValuesGrid();
          this.SelectedConnectionProfile = connectionProfile;
        }));
      }
    }

    private void SaveReadingValuesGrid()
    {
      this.nhSession.BeginTransaction();
      this.nhSession.FlushMode = FlushMode.Commit;
      this.TransactionalDeleteReadingValues(this.ReadingValuesToDeleteCollection);
      foreach (ReadingValuesDTO readingValues in (Collection<ReadingValuesDTO>) this.ReadingValuesCollection)
      {
        MeterReadingValue meterReadingValue1 = this.GetMeterReadingValue(ValueIdent.ValueIdPart_StorageInterval.None, readingValues);
        MeterReadingValue meterReadingValue2 = this.GetMeterReadingValue(ValueIdent.ValueIdPart_StorageInterval.DueDate, readingValues);
        if (meterReadingValue1 == null)
          this.TransactionalCreateActualReadingValue(readingValues);
        else
          this.TransactionalUpdateActualReadingValue(readingValues);
        if (meterReadingValue2 == null)
          this.TransactionalCreateDueDateReadingValue(readingValues);
        else
          this.TransactionalUpdateDueDateReadingValue(readingValues);
      }
      this.UpdateOrderStatus(this._selectedOrder, StatusOrderEnum.InProgress, true);
      ReadingValueStatusEnum? nullable;
      if (this.SelectedItem.IsMeter() && this.SelectedItem.MeterId != Guid.Empty)
      {
        nullable = this.SelectedItem.Status;
        ReadingValueStatusEnum readingValueStatusEnum = ReadingValueStatusEnum.notavailable;
        if (nullable.GetValueOrDefault() == readingValueStatusEnum && nullable.HasValue)
        {
          IRepository<MeterReadingValue> readingValueRepository1 = this._meterReadingValueRepository;
          IList<MeterReadingValue> meterReadingValueList1;
          if (readingValueRepository1 == null)
            meterReadingValueList1 = (IList<MeterReadingValue>) null;
          else
            meterReadingValueList1 = readingValueRepository1.SearchFor((Expression<Func<MeterReadingValue, bool>>) (m => m.MeterId == this.SelectedItem.MeterId && m.ValueId == 0L));
          IList<MeterReadingValue> meterReadingValueList2 = meterReadingValueList1;
          if (meterReadingValueList2 != null)
          {
            foreach (MeterReadingValue entity in (IEnumerable<MeterReadingValue>) meterReadingValueList2)
              this._meterReadingValueRepository?.TransactionalDelete(entity);
          }
          IRepository<MeterReadingValue> readingValueRepository2 = this._meterReadingValueRepository;
          IList<MeterReadingValue> meterReadingValueList3;
          if (readingValueRepository2 == null)
            meterReadingValueList3 = (IList<MeterReadingValue>) null;
          else
            meterReadingValueList3 = readingValueRepository2.SearchFor((Expression<Func<MeterReadingValue, bool>>) (m => m.MeterId == this.SelectedItem.MeterId && m.ValueId != 0L));
          foreach (MeterReadingValue entity in (IEnumerable<MeterReadingValue>) meterReadingValueList3)
            this._meterReadingValueRepository?.TransactionalUpdate(entity);
        }
      }
      this.nhSession.Transaction.Commit();
      this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
      if (this.ReadingValuesCollection.Count == 0)
      {
        Guid meterId = this.SelectedItem.MeterId;
        nullable = new ReadingValueStatusEnum?();
        ReadingValueStatusEnum? status = nullable;
        this.SetMeterSerializableDTOStatusInStructureBytes(meterId, status);
      }
      else
        this.SetMeterSerializableDTOStatusInStructureBytes(this.SelectedItem.MeterId, new ReadingValueStatusEnum?(ReadingValueStatusEnum.ok));
      Guid id = this.SelectedItem.Id;
      this.StructureForSelectedOrder = this.GetOrderManagerInstance().GetNodes(this.StructureBytes, this._selectedOrder.Id, this._blueMeters, this._importableDeviceModelNameList).Item1;
      this.SelectedItem = OrdersManager.FindNodeById(id, this.StructureForSelectedOrder);
    }

    private void SaveGenericMbusReadingValuesGrid()
    {
      this.nhSession.BeginTransaction();
      this.nhSession.FlushMode = FlushMode.Commit;
      this.TransactionalDeleteMeterReadingValues(this.MeterReadingValuesToDeleteCollection);
      this.MeterReadingValuesToDeleteCollection = (ObservableCollection<MeterReadingValueDTO>) null;
      foreach (MeterReadingValueDTO mbusReadingValues in (Collection<MeterReadingValueDTO>) this.GenericMbusReadingValuesCollection)
      {
        MeterReadingValueDTO meterReadingValue = mbusReadingValues;
        if (this._meterReadingValueRepository.FirstOrDefault((Expression<Func<MeterReadingValue, bool>>) (item => item.Id == meterReadingValue.Id)) == null)
          this.TransactionalCreateActualMeterReadingValue(meterReadingValue);
        else
          this.TransactionalUpdateActualMeterReadingValue(meterReadingValue);
      }
      this.UpdateOrderStatus(this._selectedOrder, StatusOrderEnum.InProgress, true);
      this.nhSession.Transaction.Commit();
      this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
      if (this.GenericMbusReadingValuesCollection.Count == 0)
        this.SetMeterSerializableDTOStatusInStructureBytes(this.SelectedItem.MeterId, new ReadingValueStatusEnum?());
      else if (!this.MeterHasOnlyWarningNumberOrSignalStrengthAsReadingValues(this.GenericMbusReadingValuesCollection.ToList<MeterReadingValueDTO>()))
        this.SetMeterSerializableDTOStatusInStructureBytes(this.SelectedItem.MeterId, new ReadingValueStatusEnum?(ReadingValueStatusEnum.ok));
      Guid id = this.SelectedItem.Id;
      this.StructureForSelectedOrder = this.GetOrderManagerInstance().GetNodes(this.StructureBytes, this._selectedOrder.Id, this._blueMeters, this._importableDeviceModelNameList).Item1;
      this.SelectedItem = OrdersManager.FindNodeById(id, this.StructureForSelectedOrder);
    }

    public ICommand ReadingNotPossibleCommand
    {
      get => (ICommand) new RelayCommand((Action<object>) (Delegate => this.NodeNotRead()));
    }

    private void NodeNotRead()
    {
      OrderSerializableStructure serializableStructure = StructuresHelper.DeserializeStructure(this.StructureBytes);
      ProfileType connectionProfile = this.SelectedConnectionProfile;
      switch (this.SelectedItem.NodeType)
      {
        case StructureNodeTypeEnum.Tenant:
          ObservableCollection<ExecuteOrderStructureNode> children = this.SelectedItem.Children;
          bool flag = false;
          foreach (ExecuteOrderStructureNode node in (Collection<ExecuteOrderStructureNode>) children)
          {
            if (node.IsMeter() && node.ReadingEnabled && node.MeterId != Guid.Empty)
            {
              flag = true;
              this.SetMeterSerializableDTOStatus(serializableStructure, node.MeterId, new ReadingValueStatusEnum?(ReadingValueStatusEnum.notavailable));
              OrdersManager.SetImageAndColor(node, Brushes.OrangeRed);
            }
          }
          if (flag)
            OrdersManager.SetImageAndColor(this.SelectedItem, Brushes.OrangeRed);
          this.StructureBytes = StructuresHelper.SerializeStructure(serializableStructure);
          this.StructureForSelectedOrder = this.GetOrderManagerInstance().GetNodes(this.StructureBytes, this._selectedOrder.Id, this._blueMeters, this._importableDeviceModelNameList).Item1;
          this._selectedOrder.StructureBytes = this.StructureBytes;
          this._selectedOrder.Status = StatusOrderEnum.InProgress;
          this.GetOrderManagerInstance().EditOrder(this._selectedOrder, new bool?(false));
          break;
        case StructureNodeTypeEnum.Meter:
        case StructureNodeTypeEnum.RadioMeter:
          if (this.SelectedItem.MeterId != Guid.Empty)
          {
            IRepository<MeterReadingValue> readingValueRepository = this._meterReadingValueRepository;
            Expression<Func<MeterReadingValue, bool>> predicate = (Expression<Func<MeterReadingValue, bool>>) (m => m.MeterId == this.SelectedItem.MeterId && m.ValueId != 0L);
            foreach (MeterReadingValue meterReadingValue1 in (IEnumerable<MeterReadingValue>) readingValueRepository.SearchFor(predicate))
            {
              MeterReadingValue meterReadingValue = meterReadingValue1;
              OrderReadingValues entity = this._orderReadingValuesRepository.FirstOrDefault((Expression<Func<OrderReadingValues, bool>>) (or => or.MeterReadingValue.Id == meterReadingValue.Id && or.OrderObj.Id == this._selectedOrder.Id));
              if (entity != null)
                this._orderReadingValuesRepository.Delete(entity);
              this._meterReadingValueRepository.Delete(meterReadingValue);
            }
            this._blueMeters.Remove(this.SelectedItem.MeterId);
            this.SetMeterSerializableDTOStatus(serializableStructure, this.SelectedItem.MeterId, new ReadingValueStatusEnum?(ReadingValueStatusEnum.notavailable));
            this.StructureBytes = StructuresHelper.SerializeStructure(serializableStructure);
            this.StructureForSelectedOrder = this.GetOrderManagerInstance().GetNodes(this.StructureBytes, this._selectedOrder.Id, this._blueMeters, this._importableDeviceModelNameList).Item1;
            this._selectedOrder.StructureBytes = this.StructureBytes;
            this._selectedOrder.Status = StatusOrderEnum.InProgress;
            this.GetOrderManagerInstance().EditOrder(this._selectedOrder, new bool?(false));
            break;
          }
          break;
      }
      this.SelectedConnectionProfile = connectionProfile;
    }

    private void SetMeterSerializableDTOStatus(
      OrderSerializableStructure orderSerializableStructure,
      Guid meterId,
      ReadingValueStatusEnum? status)
    {
      MeterSerializableDTO meterSerializableDto = orderSerializableStructure.meterList.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.Id == meterId));
      if (meterSerializableDto == null)
        return;
      meterSerializableDto.Status = status;
    }

    private void SetMeterSerializableDTOStatusInStructureBytes(
      Guid meterId,
      ReadingValueStatusEnum? status)
    {
      OrderSerializableStructure serializableStructure = StructuresHelper.DeserializeStructure(this.StructureBytes);
      this.SetMeterSerializableDTOStatus(serializableStructure, meterId, status);
      this.StructureBytes = StructuresHelper.SerializeStructure(serializableStructure);
    }

    private void SetMeterSerializableDTOIsConfigured(
      OrderSerializableStructure orderSerializableStructure,
      Guid meterId,
      bool IsConfigured)
    {
      if (!IsConfigured)
        return;
      MeterSerializableDTO meterSerializableDto = orderSerializableStructure.meterList.FirstOrDefault<MeterSerializableDTO>((Func<MeterSerializableDTO, bool>) (m => m.Id == meterId));
      if (meterSerializableDto != null)
        meterSerializableDto.IsConfigured = new bool?(true);
    }

    private bool MeterHasOnlyWarningNumberOrSignalStrengthAsReadingValues(
      List<MeterReadingValueDTO> readingValues)
    {
      return readingValues.All<MeterReadingValueDTO>((Func<MeterReadingValueDTO, bool>) (rv => rv.PhysicalQuantity == ValueIdent.ValueIdPart_PhysicalQuantity.WarningNumber || rv.PhysicalQuantity == ValueIdent.ValueIdPart_PhysicalQuantity.SignalStrength));
    }

    public ICommand ChangeDefaultEquipmentCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          ConfigChangeableParamsViewModel changeableParamsViewModel = DIConfigurator.GetConfigurator().Get<ConfigChangeableParamsViewModel>((IParameter) new ConstructorArgument("equipmentModel", (object) this._selectedEquipmentModel));
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) changeableParamsViewModel);
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            return;
          this.SelectedEquipmentName = changeableParamsViewModel.EquipmentSelectorProperty.SelectedEquipmentModel.Name;
          this._selectedEquipmentModel = changeableParamsViewModel.EquipmentSelectorProperty.SelectedEquipmentModel;
          this._transceiverType = this.GetTransceiverType();
          this.ConnectionProfileCollection = this.InitConnectionProfileCollection();
          this._selectedConnectionProfile = this._connectionProfileCollection.FirstOrDefault<ProfileType>();
          this.OnPropertyChanged("SelectedConnectionProfile");
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_DefaultEquipment_Update_Message);
        }));
      }
    }

    public ICommand ShowReadingValuesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (param =>
        {
          ExecuteOrderStructureNode device = param as ExecuteOrderStructureNode;
          if (device == null)
            return;
          IEnumerable<StructureNodeDTO> source = StructuresHelper.Descendants(new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) this.GetStructureManagerInstance().GetStructureNodeDTO(this.StructureBytes)).First<StructureNodeDTO>()).Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.Id == device.Id));
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MeterReadingValuesViewModel>((IParameter) new ConstructorArgument("structureNode", (object) source.First<StructureNodeDTO>()), (IParameter) new ConstructorArgument("selectedOrder", (object) this._selectedOrder)));
        }));
      }
    }

    private void RefreshEquipmentName()
    {
      ApplicationParameter applicationParameter = this._repositoryFactory.GetRepository<ApplicationParameter>().FirstOrDefault((Expression<Func<ApplicationParameter, bool>>) (x => x.Parameter == "DefaultEquipment"));
      if (applicationParameter == null)
        return;
      this._repositoryFactory.GetRepository<ApplicationParameter>().Refresh((object) applicationParameter.Id);
      this.SelectedEquipmentName = applicationParameter.Value;
    }

    private void TransactionalDeleteReadingValues(
      ObservableCollection<ReadingValuesDTO> readingValuesToDelete)
    {
      foreach (ReadingValuesDTO readingValuesDto in (Collection<ReadingValuesDTO>) readingValuesToDelete)
      {
        MeterReadingValue actualMeterReadingValue = this.GetMeterReadingValue(ValueIdent.ValueIdPart_StorageInterval.None, readingValuesDto);
        MeterReadingValue dueDateMeterReadingValue = this.GetMeterReadingValue(ValueIdent.ValueIdPart_StorageInterval.DueDate, readingValuesDto);
        if (actualMeterReadingValue != null)
        {
          OrderReadingValues entity = this._orderReadingValuesRepository.FirstOrDefault((Expression<Func<OrderReadingValues, bool>>) (orv => orv.MeterReadingValue.Id == actualMeterReadingValue.Id));
          if (entity != null)
            this._orderReadingValuesRepository.TransactionalDelete(entity);
          this._meterReadingValueRepository.TransactionalDelete(actualMeterReadingValue);
        }
        if (dueDateMeterReadingValue != null)
        {
          OrderReadingValues entity = this._orderReadingValuesRepository.FirstOrDefault((Expression<Func<OrderReadingValues, bool>>) (orv => orv.MeterReadingValue.Id == dueDateMeterReadingValue.Id));
          if (entity != null)
            this._orderReadingValuesRepository.TransactionalDelete(entity);
          this._meterReadingValueRepository.TransactionalDelete(dueDateMeterReadingValue);
        }
      }
    }

    private void TransactionalDeleteMeterReadingValues(
      ObservableCollection<MeterReadingValueDTO> meterReadingValuesToDelete)
    {
      if (meterReadingValuesToDelete == null)
        return;
      foreach (MeterReadingValueDTO meterReadingValueDto1 in (Collection<MeterReadingValueDTO>) meterReadingValuesToDelete)
      {
        MeterReadingValueDTO meterReadingValueDto = meterReadingValueDto1;
        if (meterReadingValueDto.Id != Guid.Empty)
        {
          OrderReadingValues entity = this._orderReadingValuesRepository.FirstOrDefault((Expression<Func<OrderReadingValues, bool>>) (item => item.OrderObj.Id == this._selectedOrder.Id && item.MeterReadingValue.Id == meterReadingValueDto.Id));
          if (entity != null)
            this._orderReadingValuesRepository.TransactionalDelete(entity);
          this._meterReadingValueRepository.TransactionalDelete(this._meterReadingValueRepository.FirstOrDefault((Expression<Func<MeterReadingValue, bool>>) (item => item.Id == meterReadingValueDto.Id)));
        }
      }
    }

    private void TransactionalCreateDueDateReadingValue(ReadingValuesDTO readingValuesDto)
    {
      MeterReadingValue meterReadingValue1 = new MeterReadingValue();
      meterReadingValue1.MeterId = this.SelectedItem.MeterId;
      meterReadingValue1.Date = DateTime.Now;
      meterReadingValue1.Value = readingValuesDto.DueDateValue;
      meterReadingValue1.MeterSerialNumber = this.SelectedItem.SerialNumber;
      meterReadingValue1.StorageInterval = 12582912L;
      meterReadingValue1.CreatedOn = DateTime.Now;
      meterReadingValue1.Unit = this._repositoryFactory.GetRepository<MeasureUnit>().FirstOrDefault((Expression<Func<MeasureUnit, bool>>) (u => u.Id == readingValuesDto.UnitId));
      meterReadingValue1.Creation = 1342177280L;
      meterReadingValue1.MeterType = (long) readingValuesDto.Register;
      meterReadingValue1.ValueId = long.Parse(ValueIdentHelper.GetValueId(ValueIdent.ValueIdPart_PhysicalQuantity.Any, readingValuesDto.Register, ValueIdent.ValueIdPart_Calculation.Any, ValueIdent.ValueIdPart_CalculationStart.Any, ValueIdent.ValueIdPart_StorageInterval.DueDate, ValueIdent.ValueIdPart_Creation.Manually, 0));
      MeterReadingValue meterReadingValue2 = meterReadingValue1;
      if (!this.GetReadingValuesManagerInstance().IsValidReadingValues(meterReadingValue2))
        return;
      this._meterReadingValueRepository.TransactionalInsert(meterReadingValue2);
      MSS.Core.Model.Orders.Order byId = this._repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>().GetById((object) this._selectedOrder.Id);
      this.GetReadingValuesManagerInstance().InsertOrderReadingValues(this.nhSession, byId, meterReadingValue2);
    }

    private void TransactionalUpdateDueDateReadingValue(ReadingValuesDTO readingValuesDto)
    {
      MeterReadingValue meterReadingValue = this.GetMeterReadingValue(ValueIdent.ValueIdPart_StorageInterval.DueDate, readingValuesDto);
      if (meterReadingValue == null)
        return;
      meterReadingValue.Value = readingValuesDto.DueDateValue;
      meterReadingValue.Unit = this._repositoryFactory.GetRepository<MeasureUnit>().FirstOrDefault((Expression<Func<MeasureUnit, bool>>) (u => u.Id == readingValuesDto.UnitId));
      meterReadingValue.Creation = 1342177280L;
      meterReadingValue.MeterType = (long) readingValuesDto.Register;
      meterReadingValue.ValueId = long.Parse(ValueIdentHelper.GetValueId(ValueIdent.ValueIdPart_PhysicalQuantity.Any, readingValuesDto.Register, ValueIdent.ValueIdPart_Calculation.Any, ValueIdent.ValueIdPart_CalculationStart.Any, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Manually, 0));
      meterReadingValue.CreatedOn = DateTime.Now;
      this._meterReadingValueRepository.TransactionalUpdate(meterReadingValue);
    }

    private void TransactionalCreateActualReadingValue(ReadingValuesDTO readingValuesDto)
    {
      MeterReadingValue meterReadingValue1 = new MeterReadingValue();
      meterReadingValue1.MeterId = this.SelectedItem.MeterId;
      meterReadingValue1.Date = DateTime.Now;
      meterReadingValue1.Value = readingValuesDto.ActualValue;
      meterReadingValue1.MeterSerialNumber = this.SelectedItem.SerialNumber;
      meterReadingValue1.StorageInterval = 4194304L;
      meterReadingValue1.CreatedOn = DateTime.Now;
      meterReadingValue1.Unit = this._repositoryFactory.GetRepository<MeasureUnit>().FirstOrDefault((Expression<Func<MeasureUnit, bool>>) (u => u.Id == readingValuesDto.UnitId));
      meterReadingValue1.Creation = 1342177280L;
      meterReadingValue1.MeterType = (long) readingValuesDto.Register;
      meterReadingValue1.ValueId = long.Parse(ValueIdentHelper.GetValueId(ValueIdent.ValueIdPart_PhysicalQuantity.Any, readingValuesDto.Register, ValueIdent.ValueIdPart_Calculation.Any, ValueIdent.ValueIdPart_CalculationStart.Any, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Manually, 0));
      MeterReadingValue meterReadingValue2 = meterReadingValue1;
      if (!this.GetReadingValuesManagerInstance().IsValidReadingValues(meterReadingValue2))
        return;
      this._meterReadingValueRepository.TransactionalInsert(meterReadingValue2);
      MSS.Core.Model.Orders.Order byId = this._repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>().GetById((object) this._selectedOrder.Id);
      this.GetReadingValuesManagerInstance().InsertOrderReadingValues(this.nhSession, byId, meterReadingValue2);
    }

    private void TransactionalCreateActualMeterReadingValue(
      MeterReadingValueDTO meterReadingValueDto)
    {
      MeterReadingValue meterReadingValue1 = new MeterReadingValue();
      meterReadingValue1.MeterId = this.SelectedItem.MeterId;
      meterReadingValue1.Date = meterReadingValueDto.Date;
      meterReadingValue1.Value = meterReadingValueDto.Value;
      meterReadingValue1.MeterSerialNumber = this.SelectedItem.SerialNumber;
      meterReadingValue1.StorageInterval = (long) meterReadingValueDto.StorageInterval;
      meterReadingValue1.CreatedOn = DateTime.Now;
      meterReadingValue1.Unit = this._repositoryFactory.GetRepository<MeasureUnit>().FirstOrDefault((Expression<Func<MeasureUnit, bool>>) (item => item.Id == meterReadingValueDto.UnitId));
      meterReadingValue1.PhysicalQuantity = (long) meterReadingValueDto.PhysicalQuantity;
      meterReadingValue1.Calculation = (long) meterReadingValueDto.Calculation;
      meterReadingValue1.CalculationStart = (long) meterReadingValueDto.CalculationStart;
      meterReadingValue1.Creation = (long) meterReadingValueDto.Creation;
      meterReadingValue1.MeterType = (long) meterReadingValueDto.MeterType;
      meterReadingValue1.ValueId = long.Parse(ValueIdentHelper.GetValueId(meterReadingValueDto.PhysicalQuantity, meterReadingValueDto.MeterType, meterReadingValueDto.Calculation, meterReadingValueDto.CalculationStart, meterReadingValueDto.StorageInterval, meterReadingValueDto.Creation, 0));
      MeterReadingValue meterReadingValue2 = meterReadingValue1;
      if (!this.GetReadingValuesManagerInstance().IsValidReadingValues(meterReadingValue2))
        return;
      this._meterReadingValueRepository.TransactionalInsert(meterReadingValue2);
      MSS.Core.Model.Orders.Order byId = this._repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>().GetById((object) this._selectedOrder.Id);
      this.GetReadingValuesManagerInstance().InsertOrderReadingValues(this.nhSession, byId, meterReadingValue2);
    }

    private void TransactionalUpdateActualReadingValue(ReadingValuesDTO readingValuesDto)
    {
      MeterReadingValue meterReadingValue = this.GetMeterReadingValue(ValueIdent.ValueIdPart_StorageInterval.None, readingValuesDto);
      if (meterReadingValue == null)
        return;
      meterReadingValue.Value = readingValuesDto.ActualValue;
      meterReadingValue.Unit = this._repositoryFactory.GetRepository<MeasureUnit>().FirstOrDefault((Expression<Func<MeasureUnit, bool>>) (u => u.Id == readingValuesDto.UnitId));
      meterReadingValue.Creation = 1342177280L;
      meterReadingValue.MeterType = (long) readingValuesDto.Register;
      meterReadingValue.ValueId = long.Parse(ValueIdentHelper.GetValueId(ValueIdent.ValueIdPart_PhysicalQuantity.Any, readingValuesDto.Register, ValueIdent.ValueIdPart_Calculation.Any, ValueIdent.ValueIdPart_CalculationStart.Any, ValueIdent.ValueIdPart_StorageInterval.None, ValueIdent.ValueIdPart_Creation.Manually, 0));
      meterReadingValue.CreatedOn = DateTime.Now;
      this._meterReadingValueRepository.TransactionalUpdate(meterReadingValue);
    }

    private void TransactionalUpdateActualMeterReadingValue(
      MeterReadingValueDTO meterReadingValueDto)
    {
      MeterReadingValue entity = this._meterReadingValueRepository.FirstOrDefault((Expression<Func<MeterReadingValue, bool>>) (item => item.Id == meterReadingValueDto.Id));
      if (entity == null)
        return;
      entity.Value = meterReadingValueDto.Value;
      entity.Unit = this._repositoryFactory.GetRepository<MeasureUnit>().FirstOrDefault((Expression<Func<MeasureUnit, bool>>) (item => item.Id == meterReadingValueDto.UnitId));
      entity.PhysicalQuantity = (long) meterReadingValueDto.PhysicalQuantity;
      entity.MeterType = (long) meterReadingValueDto.MeterType;
      entity.Calculation = (long) meterReadingValueDto.Calculation;
      entity.CalculationStart = (long) meterReadingValueDto.CalculationStart;
      entity.StorageInterval = (long) meterReadingValueDto.StorageInterval;
      entity.Creation = (long) meterReadingValueDto.Creation;
      entity.ValueId = long.Parse(ValueIdentHelper.GetValueId(meterReadingValueDto.PhysicalQuantity, meterReadingValueDto.MeterType, meterReadingValueDto.Calculation, meterReadingValueDto.CalculationStart, meterReadingValueDto.StorageInterval, meterReadingValueDto.Creation, 0));
      entity.CreatedOn = DateTime.Now;
      this._meterReadingValueRepository.TransactionalUpdate(entity);
    }

    private MeterReadingValue GetMeterReadingValue(
      ValueIdent.ValueIdPart_StorageInterval readingValueEnum,
      ReadingValuesDTO readingValuesDto)
    {
      Guid meterId = readingValuesDto.MeterId;
      Guid orderId = readingValuesDto.OrderId;
      long register = (long) readingValuesDto.Register;
      return this._repositoryFactory.GetSession().CreateCriteria<MeterReadingValue>("RV").CreateCriteria("RV.OrderReadingValues", "ORV", JoinType.InnerJoin).Add((ICriterion) Restrictions.Eq("ORV.OrderObj.Id", (object) orderId)).Add((ICriterion) Restrictions.Eq("RV.MeterId", (object) meterId)).Add((ICriterion) Restrictions.Eq("RV.MeterType", (object) register)).Add((ICriterion) Restrictions.Eq("RV.StorageInterval", (object) (long) readingValueEnum)).List<MeterReadingValue>().FirstOrDefault<MeterReadingValue>();
    }

    private bool GetVisibilityForProperty(string propertyName)
    {
      if (this.SelectedItem != null && this.SelectedItem.MeterId != Guid.Empty)
      {
        foreach (DeviceTypeVisibilityAttribute visibilityAttribute in (IEnumerable<DeviceTypeVisibilityAttribute>) DeviceTypeVisibilityHelper.GetDeviceTypeVisibilityAttributes(typeof (ReadingValuesDTO), propertyName))
        {
          if (visibilityAttribute.DeviceTypeEnum.Equals((object) this.SelectedItem.DeviceType))
            return true;
        }
      }
      return false;
    }

    private List<Guid> GetMeterGuids()
    {
      List<Guid> meterGuids = new List<Guid>();
      foreach (ExecuteOrderStructureNode node in (Collection<ExecuteOrderStructureNode>) this.StructureForSelectedOrder)
      {
        if (node.NodeType == StructureNodeTypeEnum.Meter)
          meterGuids.Add(node.MeterId);
        this.WalkStructureAndGetMeterGuids(node, ref meterGuids);
      }
      return meterGuids;
    }

    private void WalkStructureAndGetMeterGuids(
      ExecuteOrderStructureNode node,
      ref List<Guid> meterGuids)
    {
      foreach (ExecuteOrderStructureNode child in (Collection<ExecuteOrderStructureNode>) node.Children)
      {
        if (child.NodeType == StructureNodeTypeEnum.Meter)
          meterGuids.Add(child.MeterId);
        this.WalkStructureAndGetMeterGuids(child, ref meterGuids);
      }
    }

    private void SetOrderStatus(
      ObservableCollection<ExecuteOrderStructureNode> nodesList,
      OrderDTO selectedOrder)
    {
      if (!OrdersHelper.Descendants(nodesList[0]).Where<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (d => d.IsMeter())).All<ExecuteOrderStructureNode>((Func<ExecuteOrderStructureNode, bool>) (m => object.Equals((object) m.Status, (object) ReadingValueStatusEnum.ok) || object.Equals((object) m.Status, (object) ReadingValueStatusEnum.notavailable))) || selectedOrder.Status != StatusOrderEnum.InProgress && selectedOrder.Status != StatusOrderEnum.Dated || !this.UpdateOrderStatus(selectedOrder, StatusOrderEnum.Closed))
        return;
      MSSUIHelper.ShowWarningDialog(this._windowFactory, string.Empty, Resources.Warning_Order_Close, false);
    }

    private bool UpdateOrderStatus(
      OrderDTO selectedOrder,
      StatusOrderEnum status,
      bool isTransactional = false)
    {
      IRepository<MSS.Core.Model.Orders.Order> repository = this._repositoryFactory.GetRepository<MSS.Core.Model.Orders.Order>();
      MSS.Core.Model.Orders.Order entity = repository.FirstOrDefault((Expression<Func<MSS.Core.Model.Orders.Order, bool>>) (o => o.Id == selectedOrder.Id));
      if (entity == null)
        return false;
      selectedOrder.Status = status;
      entity.Status = status;
      if (!isTransactional)
        repository.Update(entity);
      else
        repository.TransactionalUpdate(entity);
      return true;
    }

    private RadObservableCollection<ReadingValuesDTO> GetReadingValuesCollection()
    {
      Guid orderId = this._selectedOrder.Id;
      Guid meterId = this.SelectedItem.MeterId;
      RadObservableCollection<ReadingValuesDTO> valuesCollection = new RadObservableCollection<ReadingValuesDTO>();
      List<Guid?> readingValuesOrderIds = this._orderReadingValuesRepository.SearchFor((Expression<Func<OrderReadingValues, bool>>) (or => or.OrderObj.Id == orderId)).Select<OrderReadingValues, Guid?>((Func<OrderReadingValues, Guid?>) (x => x?.MeterReadingValue?.Id)).ToList<Guid?>();
      IRepository<MeterReadingValue> readingValueRepository = this._meterReadingValueRepository;
      Expression<Func<MeterReadingValue, bool>> predicate = (Expression<Func<MeterReadingValue, bool>>) (r => r.MeterId == meterId && readingValuesOrderIds.Contains((Guid?) r.Id) && r.Creation == 1342177280L);
      foreach (IGrouping<long, MeterReadingValue> source in readingValueRepository.SearchFor(predicate).GroupBy<MeterReadingValue, long>((Func<MeterReadingValue, long>) (r => r.MeterType)))
      {
        List<MeterReadingValue> list = source.ToList<MeterReadingValue>();
        MeterReadingValue meterReadingValue1 = list.FirstOrDefault<MeterReadingValue>((Func<MeterReadingValue, bool>) (m => m.StorageInterval == 4194304L));
        MeterReadingValue meterReadingValue2 = list.FirstOrDefault<MeterReadingValue>((Func<MeterReadingValue, bool>) (m => m.StorageInterval == 12582912L));
        if (meterReadingValue1 != null)
        {
          RadObservableCollection<ReadingValuesDTO> observableCollection = valuesCollection;
          ReadingValuesDTO readingValuesDto1 = new ReadingValuesDTO();
          readingValuesDto1.ActualValue = meterReadingValue1.Value;
          readingValuesDto1.DueDateValue = meterReadingValue2 != null ? meterReadingValue2.Value : 0.0;
          readingValuesDto1.Register = (ValueIdent.ValueIdPart_MeterType) source.Key;
          ReadingValuesDTO readingValuesDto2 = readingValuesDto1;
          MeasureUnit unit = meterReadingValue1.Unit;
          Guid guid = unit != null ? unit.Id : Guid.Empty;
          readingValuesDto2.UnitId = guid;
          readingValuesDto1.MeterId = meterId;
          readingValuesDto1.OrderId = orderId;
          ReadingValuesDTO readingValuesDto3 = readingValuesDto1;
          observableCollection.Add(readingValuesDto3);
        }
      }
      return valuesCollection;
    }

    private ObservableCollection<MeterReadingValueDTO> GetGenericMbusReadingValuesCollection()
    {
      ISQLQuery sqlQuery = this._session.CreateSQLQuery(string.Format("SELECT rv.* FROM [t_ReadingValues] as rv \r\n                        INNER JOIN [t_OrderReadingValues] AS orv ON rv.[Id] = orv.[MeterReadingValueId] \r\n                        WHERE orv.OrderId = '{0}' AND rv.MeterSerialNumber = '{1}'", (object) this._selectedOrder.Id, (object) this.SelectedItem.SerialNumber));
      sqlQuery.AddEntity("rv", typeof (MeterReadingValue));
      IList<MeterReadingValue> meterReadingValueList = sqlQuery.List<MeterReadingValue>();
      ObservableCollection<MeterReadingValueDTO> valuesCollection = new ObservableCollection<MeterReadingValueDTO>();
      foreach (MeterReadingValue source in (IEnumerable<MeterReadingValue>) meterReadingValueList)
        valuesCollection.Add(Mapper.Map<MeterReadingValue, MeterReadingValueDTO>(source));
      return valuesCollection;
    }

    private void OrderReadingValuesSaved(OrderReadingValuesSavedEvent readingValuesEvent)
    {
      if (!(this.SelectedItem.SerialNumber == readingValuesEvent.SerialNumber))
        return;
      this.GenericMbusReadingValuesCollection = this.GetGenericMbusReadingValuesCollection();
    }

    private void OnReadingProgressBarFinish(ProgressFinished obj)
    {
      switch (this._transceiverType)
      {
        case TransceiverType.Reader:
          this.isIRReadingStarted = false;
          this._readerMinoConnect.StopReadingValues();
          break;
        case TransceiverType.Receiver:
          this._receiverMinoConnect.StopReadingValues();
          break;
      }
      this.IsStartButtonEnabled = true;
      this.IsStopButtonEnabled = false;
      this.IsReadingStarted = false;
      this.IsReadingDeviceViaCBEnabled = true;
      this.isIRReadingStarted = false;
      this.UpdateLabel();
    }

    public void StopReading() => this.ProgressBar.ResetProgressBar();

    private void SetMeterImageStatusAndDisplayWarningMessage(
      ExecuteOrderStructureNode meter,
      SolidColorBrush brush,
      ReadingValueStatusEnum status,
      string errorMessage,
      string serialNumber)
    {
      meter.Status = new ReadingValueStatusEnum?(status);
      this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(string.Format(errorMessage, (object) serialNumber));
      OrdersManager.SetImageAndColor(meter, brush);
    }
  }
}
