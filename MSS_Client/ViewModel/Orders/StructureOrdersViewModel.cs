// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.StructureOrdersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Events;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS_Client.Utils;
using MSS_Client.ViewModel.GenericProgressDialog;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class StructureOrdersViewModel : ViewModelBase
  {
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<StructureNodeType> _structureNodeTypeRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IStructureNodeRepository _structureRepository;
    private readonly OrderDTO _selectedOrder;
    private readonly OrderTypeEnum _orderType;
    private readonly Guid _selectedRootStructureNodeId;
    private readonly DateTime _orderDueDate;
    private readonly DateTime _orderDueDateStart;
    private readonly DateTime _orderDueDateEnd;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWindowFactory _windowFactory;
    private bool _isBusy;
    private string _structureTypeFirstRadio;
    private bool _isExpanded;
    private string _structureTypeSecondRadio;
    private bool _isFirstRadioChecked;
    private bool _isSecondRadioChecked;
    private bool _radioButtonsVisibility;
    private bool _firstRadioButtonVisibility;
    private bool _secondRadioButtonVisibility;
    private StructureTypeEnum _selectedStructureType;
    private IEnumerable<StructureNodeDTO> _structureNodeCollection;
    private bool _isOKButtonVisible;
    private StructureNodeDTO _selectedStructureNodeItem;
    private bool _isRootNodeSelected;
    private bool _isShortDeviceNoVisible;
    private bool _isOkButtonEnabled;
    private bool _isOnlyDevicesWithNoReadingsChecked;
    private bool _isOnlyDevicesWithNoReadingsRangeChecked;
    private bool _isOnlyDevicesWithNoReadingsCheckBoxVisible;

    [Inject]
    public StructureOrdersViewModel(
      OrderDTO selectedOrder,
      OrderTypeEnum orderType,
      bool viewMode,
      Guid selectedRootStructureNodeId,
      DateTime orderDueDate,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      this._structureRepository = repositoryFactory.GetStructureNodeRepository();
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._selectedOrder = selectedOrder;
      this._orderType = orderType;
      this._selectedRootStructureNodeId = selectedRootStructureNodeId;
      this._orderDueDate = orderDueDate;
      this._orderDueDateStart = orderDueDate.AddDays(-3.0);
      this._orderDueDateEnd = orderDueDate;
      if (viewMode)
      {
        this.StructureNodeCollection = this.GetStructureNodeForOrder();
        foreach (StructureNodeDTO structureNode in this.StructureNodeCollection)
          structureNode.SubNodes = new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) structureNode.SubNodes.OrderBy<StructureNodeDTO, int>((Func<StructureNodeDTO, int>) (structure => !(structure.Entity is TenantDTO entity) ? structure.OrderNr : entity.TenantNr)));
        this.RadioButtonsVisibility = false;
        this.IsExpanded = true;
        StructureTypeEnum? structureType = this._selectedOrder.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        this.IsShortDeviceNoVisible = structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue;
      }
      else
      {
        this.IsExpanded = false;
        this.RadioButtonsVisibility = true;
        bool flag1 = LicenseHelper.HasRightOnLicense(LicenseHelper.GetValidHardwareKey(), OperationEnum.PhysicalStructureView);
        bool flag2 = LicenseHelper.HasRightOnLicense(LicenseHelper.GetValidHardwareKey(), OperationEnum.FixedStructureView);
        switch (this._orderType)
        {
          case OrderTypeEnum.ReadingOrder:
            this.StructureTypeFirstRadio = CultureResources.GetValue("MSS_Client_Orders_PhysicalStructure");
            this.StructureTypeSecondRadio = CultureResources.GetValue("MSS_Client_Orders_FixedStructure");
            this.FirstRadioButtonVisibility = flag1;
            this.SecondRadioButtonVisibility = flag2;
            if (this.FirstRadioButtonVisibility)
              this.IsFirstRadioChecked = true;
            else if (this.SecondRadioButtonVisibility)
              this.IsSecondRadioChecked = true;
            this.IsOnlyDevicesWithNoReadingsCheckBoxVisible = true;
            this.SelectionMode = true;
            break;
          case OrderTypeEnum.InstallationOrder:
            this.StructureTypeFirstRadio = CultureResources.GetValue("MSS_Client_Orders_PhysicalStructure");
            this.StructureTypeSecondRadio = CultureResources.GetValue("MSS_Client_Orders_FixedStructure");
            this.FirstRadioButtonVisibility = flag1;
            this.SecondRadioButtonVisibility = flag2;
            if (this.FirstRadioButtonVisibility)
              this.IsFirstRadioChecked = true;
            if (this.SecondRadioButtonVisibility)
              this.IsSecondRadioChecked = true;
            this.IsOnlyDevicesWithNoReadingsCheckBoxVisible = false;
            this.SelectionMode = false;
            break;
        }
      }
      this.IsOKButtonVisible = !viewMode;
      EventPublisher.Register<LoadSubNodesForRoot>(new Action<LoadSubNodesForRoot>(this.LoadSubNodesForRootNode));
    }

    private StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    private UsersManager GetUsersManagerInstance() => new UsersManager(this._repositoryFactory);

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

    public string StructureTypeFirstRadio
    {
      get => this._structureTypeFirstRadio;
      set
      {
        this._structureTypeFirstRadio = value;
        this.OnPropertyChanged(nameof (StructureTypeFirstRadio));
      }
    }

    public bool IsExpanded
    {
      get => this._isExpanded;
      set
      {
        this._isExpanded = value;
        this.OnPropertyChanged(nameof (IsExpanded));
      }
    }

    public string StructureTypeSecondRadio
    {
      get => this._structureTypeSecondRadio;
      set
      {
        this._structureTypeSecondRadio = value;
        this.OnPropertyChanged(nameof (StructureTypeSecondRadio));
      }
    }

    public bool IsFirstRadioChecked
    {
      get => this._isFirstRadioChecked;
      set
      {
        this._isFirstRadioChecked = value;
        if (this._isFirstRadioChecked)
        {
          switch (this._orderType)
          {
            case OrderTypeEnum.ReadingOrder:
              this.SelectedStructureType = StructureTypeEnum.Physical;
              break;
            case OrderTypeEnum.InstallationOrder:
              this.SelectedStructureType = StructureTypeEnum.Physical;
              break;
          }
        }
        this.OnPropertyChanged(nameof (IsFirstRadioChecked));
      }
    }

    public bool IsSecondRadioChecked
    {
      get => this._isSecondRadioChecked;
      set
      {
        this._isSecondRadioChecked = value;
        if (this._isSecondRadioChecked)
          this.SelectedStructureType = StructureTypeEnum.Fixed;
        this.OnPropertyChanged(nameof (IsSecondRadioChecked));
      }
    }

    public bool RadioButtonsVisibility
    {
      get => this._radioButtonsVisibility;
      set
      {
        this._radioButtonsVisibility = value;
        this.OnPropertyChanged(nameof (RadioButtonsVisibility));
      }
    }

    public bool FirstRadioButtonVisibility
    {
      get => this._firstRadioButtonVisibility;
      set
      {
        this._firstRadioButtonVisibility = value;
        this.OnPropertyChanged(nameof (FirstRadioButtonVisibility));
      }
    }

    public bool SecondRadioButtonVisibility
    {
      get => this._secondRadioButtonVisibility;
      set
      {
        this._secondRadioButtonVisibility = value;
        this.OnPropertyChanged(nameof (SecondRadioButtonVisibility));
      }
    }

    public StructureTypeEnum SelectedStructureType
    {
      get => this._selectedStructureType;
      set
      {
        this._selectedStructureType = value;
        this.OnPropertyChanged(nameof (SelectedStructureType));
        this.IsShortDeviceNoVisible = this.SelectedStructureType == StructureTypeEnum.Fixed;
        this.OnPropertyChanged("IsShortDeviceNoVisible");
        this.StructureNodeCollection = (IEnumerable<StructureNodeDTO>) this.GetStructuresManagerInstance().GetStructureNodesCollection(this._selectedStructureType, true);
        this.IsExpanded = false;
        this.SetSelectedRootStructureNode();
      }
    }

    public IEnumerable<StructureNodeDTO> StructureNodeCollection
    {
      get => this._structureNodeCollection;
      set
      {
        this._structureNodeCollection = value;
        this.OnPropertyChanged(nameof (StructureNodeCollection));
      }
    }

    public bool IsOKButtonVisible
    {
      get => this._isOKButtonVisible;
      set
      {
        this._isOKButtonVisible = value;
        this.OnPropertyChanged(nameof (IsOKButtonVisible));
      }
    }

    public StructureNodeDTO SelectedStructureNodeItem
    {
      get => this._selectedStructureNodeItem;
      set
      {
        this._selectedStructureNodeItem = value;
        this.IsRootNodeSelected = this._selectedStructureNodeItem != null && this._selectedStructureNodeItem != null && this._selectedStructureNodeItem.RootNode == this._selectedStructureNodeItem;
        switch (this._orderType)
        {
          case OrderTypeEnum.ReadingOrder:
            this.IsOkButtonEnabled = true;
            break;
          case OrderTypeEnum.InstallationOrder:
            this.IsOkButtonEnabled = this.IsRootNodeSelected;
            break;
        }
        this.OnPropertyChanged(nameof (SelectedStructureNodeItem));
      }
    }

    public bool IsRootNodeSelected
    {
      get => this._isRootNodeSelected;
      set
      {
        this._isRootNodeSelected = value;
        this.OnPropertyChanged(nameof (IsRootNodeSelected));
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

    public bool IsOkButtonEnabled
    {
      get => this._isOkButtonEnabled;
      set
      {
        this._isOkButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsOkButtonEnabled));
      }
    }

    public bool IsOnlyDevicesWithNoReadingsChecked
    {
      get => this._isOnlyDevicesWithNoReadingsChecked;
      set
      {
        if (this._isOnlyDevicesWithNoReadingsRangeChecked)
          this.IsOnlyDevicesWithNoReadingsRangeChecked = false;
        this._isOnlyDevicesWithNoReadingsChecked = value;
        this.OnPropertyChanged(nameof (IsOnlyDevicesWithNoReadingsChecked));
      }
    }

    public bool IsOnlyDevicesWithNoReadingsRangeChecked
    {
      get => this._isOnlyDevicesWithNoReadingsRangeChecked;
      set
      {
        if (this._isOnlyDevicesWithNoReadingsChecked)
          this.IsOnlyDevicesWithNoReadingsChecked = false;
        this._isOnlyDevicesWithNoReadingsRangeChecked = value;
        this.OnPropertyChanged(nameof (IsOnlyDevicesWithNoReadingsRangeChecked));
      }
    }

    public bool IsOnlyDevicesWithNoReadingsCheckBoxVisible
    {
      get => this._isOnlyDevicesWithNoReadingsCheckBoxVisible;
      set
      {
        this._isOnlyDevicesWithNoReadingsCheckBoxVisible = value;
        this.OnlyDevicesWithNoReadingsMessage = Resources.MSS_Client_OrderControl_OnlyDevicesWithNoReadings_Message;
        this.OnlyDevicesWithNoReadingsMessageRange = Resources.MSS_Client_OrderControl_OnlyDevicesWithNoReadingsRange_Message;
        this.DueDateValue = this._orderDueDate;
        this.DueDateStartValue = this._orderDueDateStart;
        this.DueDateEndValue = this._orderDueDateEnd;
      }
    }

    public string OnlyDevicesWithNoReadingsMessage { get; set; }

    public string OnlyDevicesWithNoReadingsMessageRange { get; set; }

    public DateTime DueDateValue { get; set; }

    public DateTime DueDateStartValue { get; set; }

    public DateTime DueDateEndValue { get; set; }

    public bool SelectionMode { get; set; }

    public ICommand OkStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter => this.CheckStructure(parameter)));
      }
    }

    private void CheckStructure(object parameter)
    {
      ObservableCollection<StructureNodeDTO> selectedNodeCollection = this.GetSelectedItems(parameter);
      bool isStructureOk = true;
      StructureNodeDTO selectedStructure = this.SelectedStructureNodeItem;
      GenericProgressDialogViewModel pd = DIConfigurator.GetConfigurator().Get<GenericProgressDialogViewModel>((IParameter) new ConstructorArgument("progressDialogTitle", (object) Resources.MSS_CLIENT_STRUCTURE_ASSIGN_TITLE), (IParameter) new ConstructorArgument("progressDialogMessage", (object) Resources.MSS_CLIENT_STRUCTURE_ASSIGN_MESSAGE));
      BackgroundWorker backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
      {
        this.LoadSubNodesForRootNode(this.SelectedStructureNodeItem);
        StructureTypeEnum? structureType = this.SelectedStructureNodeItem.StructureType;
        StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Fixed;
        int num;
        if ((structureType.GetValueOrDefault() == structureTypeEnum1 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
        {
          structureType = this.SelectedStructureNodeItem.StructureType;
          StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Physical;
          num = structureType.GetValueOrDefault() == structureTypeEnum2 ? (structureType.HasValue ? 1 : 0) : 0;
        }
        else
          num = 1;
        if (num != 0)
        {
          StructureNodeDTO structureNodeDto = StructuresHelper.GetPartialStructureNodeDTO(selectedNodeCollection);
          isStructureOk = this.HandleFixedOrPhisicalStructure(ref structureNodeDto);
        }
        else
          EventPublisher.Publish<OrderUpdated>(new OrderUpdated()
          {
            Guid = this.SelectedStructureNodeItem.Id,
            SelectedNodeDTO = selectedStructure,
            StructureType = this.SelectedStructureNodeItem.StructureType
          }, (IViewModel) this);
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
        if (message != null)
          EventPublisher.Publish<ActionFinished>(new ActionFinished()
          {
            Message = message
          }, (IViewModel) this);
        if (!isStructureOk)
          return;
        this.OnRequestClose(true);
      });
      backgroundWorker.RunWorkerAsync();
      this._windowFactory.CreateNewProgressDialog((IViewModel) pd, backgroundWorker);
    }

    private bool HandleFixedOrPhisicalStructure(ref StructureNodeDTO selectedStructure)
    {
      List<string> phisicalStructure = this.GetErrorListForFixedOrPhisicalStructure();
      if (phisicalStructure.Any<string>())
      {
        this.ShowErrorListInWarningDialog((IEnumerable<string>) phisicalStructure, Resources.VALIDATION_ORDER_NOT_ASSIGNABLE);
        return false;
      }
      bool flag = this.AdjustFixedOrPhisicalStucture(ref selectedStructure);
      if (flag)
        EventPublisher.Publish<OrderUpdated>(new OrderUpdated()
        {
          Guid = this.SelectedStructureNodeItem.Id,
          SelectedNodeDTO = selectedStructure,
          StructureType = this.SelectedStructureNodeItem.StructureType
        }, (IViewModel) this);
      return flag;
    }

    private bool AdjustFixedOrPhisicalStucture(ref StructureNodeDTO selectedStructure)
    {
      bool flag = true;
      if (this._orderType == OrderTypeEnum.ReadingOrder && (this.IsOnlyDevicesWithNoReadingsChecked || this.IsOnlyDevicesWithNoReadingsRangeChecked))
      {
        flag = this.RemoveMetersWithReadingValues(selectedStructure, this.IsOnlyDevicesWithNoReadingsRangeChecked);
        StructureTypeEnum? structureType = selectedStructure.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
        {
          List<StructureNodeDTO> structureNodeDtoList = new List<StructureNodeDTO>();
          structureNodeDtoList.AddRange(selectedStructure.SubNodes.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.NodeType.Name == StructureNodeTypeEnum.Tenant.ToString() && n.SubNodes.Count == 0)));
          foreach (StructureNodeDTO selectedNode in structureNodeDtoList)
          {
            ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
            observableCollection.Add(selectedStructure);
            ObservableCollection<StructureNodeDTO> nodeCollection = observableCollection;
            StructuresHelper.RemoveSelectedNodeFromStructure(selectedNode, nodeCollection);
          }
        }
      }
      return flag;
    }

    private List<string> GetErrorListForFixedOrPhisicalStructure()
    {
      List<string> source = new List<string>();
      List<TenantDTO> duplicates;
      StructuresHelper.ValidateTenantUniqueness(this.SelectedStructureNodeItem, out duplicates);
      if (duplicates.Any<TenantDTO>())
      {
        string error = string.Empty;
        duplicates.ForEach((Action<TenantDTO>) (t => error = error + string.Format(Resources.VALIDATION_TENANT_NOT_UNIQUE, (object) t.TenantNr) + Environment.NewLine));
        source.Add(error);
        return (List<string>) source.Distinct<string>();
      }
      if (this._orderType == OrderTypeEnum.ReadingOrder)
      {
        string str = new ReadingOrderHasMeterChecker(Resources.VALIDATION_NO_METERS).CheckForErrors(this.SelectedStructureNodeItem);
        if (string.Empty != str)
          source.Add(str);
        return source;
      }
      List<MeterDTO> notUniqueReadingMeters;
      List<MeterDTO> notUniqueInstallationMeters;
      if (StructuresHelper.ValidationMeterUniqueness(this.SelectedStructureNodeItem, this._orderType, out notUniqueReadingMeters, out notUniqueInstallationMeters))
        return source;
      if (notUniqueInstallationMeters.Any<MeterDTO>())
      {
        string error = string.Empty;
        notUniqueInstallationMeters.ForEach((Action<MeterDTO>) (x => error += string.Format("{0} {1}; Serial Number {2} - {3}", (object) Resources.METER_NOT_UNIQUE_TENANT_DECRIPTION, (object) x.TenantNo, (object) x.SerialNumber, (object) Environment.NewLine)));
        source.Add(error);
      }
      if (notUniqueReadingMeters.Any<MeterDTO>())
      {
        string error = string.Empty;
        notUniqueReadingMeters.ForEach((Action<MeterDTO>) (x => error += string.Format("{0} {1}; Serial Number {2} - {3}", (object) Resources.METER_NOT_UNIQUE_TENANT_DECRIPTION, (object) x.TenantNo, (object) x.SerialNumber, (object) Environment.NewLine)));
        source.Add(error);
      }
      return source;
    }

    private void ShowErrorListInWarningDialog(IEnumerable<string> errorList, string title)
    {
      string uniqueError = string.Empty;
      TypeHelperExtensionMethods.ForEach<string>(errorList, (Action<string>) (e => uniqueError = uniqueError + Environment.NewLine + e + Environment.NewLine));
      Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, title, uniqueError, false)));
    }

    private ObservableCollection<StructureNodeDTO> GetSelectedItems(object parameter)
    {
      ObservableCollection<StructureNodeDTO> selectedItems = new ObservableCollection<StructureNodeDTO>();
      foreach (object obj in (Collection<object>) (parameter as ObservableCollection<object>))
      {
        if (obj is StructureNodeDTO structureNodeDto)
          selectedItems.Add(structureNodeDto);
      }
      return selectedItems;
    }

    private bool RemoveMetersWithReadingValues(StructureNodeDTO selectedStructure, bool isRange)
    {
      List<StructureNodeDTO> metersToRemove = new List<StructureNodeDTO>();
      ObservableCollection<StructureNodeDTO> nodeCollection1 = new ObservableCollection<StructureNodeDTO>();
      nodeCollection1.Add(selectedStructure);
      List<StructureNodeDTO> meters = StructuresHelper.GetMeters(nodeCollection1);
      List<StructureNodeDTO> structureNodeDtoList = meters;
      ObservableCollection<StructureNodeDTO> nodeCollection2 = new ObservableCollection<StructureNodeDTO>();
      nodeCollection2.Add(selectedStructure);
      List<StructureNodeDTO> radioMeters = StructuresHelper.GetRadioMeters(nodeCollection2);
      structureNodeDtoList.AddRange((IEnumerable<StructureNodeDTO>) radioMeters);
      if (meters.Count == 0)
      {
        Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Warning_Title, Resources.VALIDATION_NO_METERS, false)));
        return false;
      }
      List<Guid> meterIds = meters.Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (m => ((MeterDTO) m.Entity).Id)).ToList<Guid>();
      IList<MeterReadingValue> readingValues;
      if (isRange)
        readingValues = (IList<MeterReadingValue>) this._repositoryFactory.GetRepository<MeterReadingValue>().Where((Expression<Func<MeterReadingValue, bool>>) (rv => meterIds.Contains(rv.MeterId) && rv.Date.Date >= this.DueDateStartValue.Date && rv.Date.Date <= this.DueDateEndValue)).ToList<MeterReadingValue>();
      else
        readingValues = (IList<MeterReadingValue>) this._repositoryFactory.GetRepository<MeterReadingValue>().Where((Expression<Func<MeterReadingValue, bool>>) (rv => meterIds.Contains(rv.MeterId) && rv.Date.Date == this.DueDateValue.Date)).ToList<MeterReadingValue>();
      meters.ForEach((Action<StructureNodeDTO>) (meter =>
      {
        if (meter.Entity == null)
          return;
        MeterDTO meterDTO = meter.Entity as MeterDTO;
        if (readingValues.Any<MeterReadingValue>((Func<MeterReadingValue, bool>) (rv => rv.MeterId == meterDTO.Id)))
          metersToRemove.Add(meter);
      }));
      if (metersToRemove.Count == meters.Count)
      {
        Application.Current.Dispatcher.Invoke((Action) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Warning_Title, string.Format(Resources.MSS_Client_OrderControl_OnlyDevicesWithReadings_Message, (object) this.DueDateValue), false)));
        return false;
      }
      foreach (StructureNodeDTO structureNodeDto in metersToRemove)
      {
        StructureNodeDTO selectedNode = structureNodeDto;
        ObservableCollection<StructureNodeDTO> observableCollection1 = new ObservableCollection<StructureNodeDTO>();
        observableCollection1.Add(selectedStructure);
        ObservableCollection<StructureNodeDTO> nodeCollection3 = observableCollection1;
        StructuresHelper.RemoveSelectedNodeFromStructure(selectedNode, nodeCollection3);
        if ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNodeDto.ParentNode.NodeType.Name, true) == StructureNodeTypeEnum.Tenant && structureNodeDto.ParentNode.SubNodes.Count == 0)
        {
          StructureNodeDTO parentNode = structureNodeDto.ParentNode;
          ObservableCollection<StructureNodeDTO> observableCollection2 = new ObservableCollection<StructureNodeDTO>();
          observableCollection2.Add(selectedStructure);
          ObservableCollection<StructureNodeDTO> nodeCollection4 = observableCollection2;
          StructuresHelper.RemoveSelectedNodeFromStructure(parentNode, nodeCollection4);
        }
      }
      return true;
    }

    public ICommand SearchCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          string searchText = parameter as string;
          this.IsBusy = true;
          BackgroundWorker backgroundWorker = new BackgroundWorker()
          {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
          };
          backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) =>
          {
            ObservableCollection<StructureNodeDTO> observableCollection = !(searchText != string.Empty) ? this.GetStructuresManagerInstance().GetStructureNodesCollection(this._selectedStructureType, true) : new ObservableCollection<StructureNodeDTO>(this.StructureNodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.Name.Contains(searchText) || n.Description.Contains(searchText))));
            args.Result = (object) observableCollection;
          });
          backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
          {
            if (!args.Cancelled && args.Error != null)
            {
              MSS.Business.Errors.MessageHandler.LogException(args.Error);
              MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
            }
            Dispatcher.CurrentDispatcher.Invoke((Action) (() =>
            {
              this.IsBusy = false;
              this.StructureNodeCollection = (IEnumerable<StructureNodeDTO>) (args.Result as ObservableCollection<StructureNodeDTO>);
            }));
          });
          backgroundWorker.RunWorkerAsync((object) this._repositoryFactory);
        }));
      }
    }

    private void LoadSubNodesForRootNode(LoadSubNodesForRoot rootNode)
    {
      this.IsBusy = true;
      BackgroundWorker backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, args) => this.LoadSubNodesForRootNode(rootNode.RootNode));
      backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, args) =>
      {
        rootNode.RootNode.SubNodes = new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) rootNode.RootNode.SubNodes.OrderBy<StructureNodeDTO, int>((Func<StructureNodeDTO, int>) (structure => !(structure.Entity is TenantDTO entity2) ? structure.OrderNr : entity2.TenantNr)));
        if (!args.Cancelled && args.Error != null)
        {
          MSS.Business.Errors.MessageHandler.LogException(args.Error);
          MessageHandlingManager.ShowExceptionMessageDialog(MSSHelper.GetErrorMessage(args.Error), this._windowFactory);
        }
        Dispatcher.CurrentDispatcher.Invoke((Action) (() => this.IsBusy = false));
      });
      backgroundWorker.RunWorkerAsync((object) this._repositoryFactory);
    }

    private void LoadSubNodesForRootNode(StructureNodeDTO rootNode)
    {
      if (rootNode.SubNodes != null && rootNode.SubNodes.Count != 0 || rootNode.ParentNode != null)
        return;
      StructuresHelper.LoadSubNodesForRootNode(this._repositoryFactory, rootNode, this.GetStructuresManagerInstance());
    }

    private IEnumerable<StructureNodeDTO> GetStructureNodeForOrder()
    {
      ObservableCollection<StructureNodeDTO> nodesCollection = new ObservableCollection<StructureNodeDTO>();
      switch (this._orderType)
      {
        case OrderTypeEnum.ReadingOrder:
          byte[] structureBytes = this._selectedOrder.StructureBytes;
          if (structureBytes != null)
          {
            OrderSerializableStructure orderserializablestructure = StructuresHelper.DeserializeStructure(structureBytes);
            Structure structure = this.GetStructuresManagerInstance().GetStructure(orderserializablestructure);
            Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
            structure.Locations.ForEach((Action<Location>) (l => entitiesDictionary.Add(l.Id, (object) l)));
            structure.Tenants.ForEach((Action<Tenant>) (t => entitiesDictionary.Add(t.Id, (object) t)));
            structure.Meters.ForEach((Action<Meter>) (m => entitiesDictionary.Add(m.Id, (object) m)));
            structure.Minomats.ForEach((Action<Minomat>) (m => entitiesDictionary.Add(m.Id, (object) m)));
            nodesCollection = StructuresHelper.GetTreeFromList(this._structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) structure.Links, entitiesDictionary);
            StructureImageHelper.SetImageIconPath(nodesCollection);
            break;
          }
          break;
        case OrderTypeEnum.InstallationOrder:
          Guid structureRootNodeId = this._selectedOrder.RootStructureNodeId;
          if (structureRootNodeId != Guid.Empty)
          {
            List<StructureNodeLinks> list = StructuresHelper.GetStructureNodesForRootNode(this._structureNodeLinksRepository, this._structureNodeLinksRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == structureRootNodeId)).Node.Id).Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => !s.EndDate.HasValue)).ToList<StructureNodeLinks>();
            List<Guid> nodeIDs = this.GetNodeIdList((IEnumerable<StructureNodeLinks>) list);
            Dictionary<Guid, object> entitiesDictionary = this.GetEntitiesDictionary(this._structureNodeRepository.SearchFor((Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id) && s.EndDate == new DateTime?())));
            nodesCollection = StructuresHelper.GetTreeFromList(this._structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) list, entitiesDictionary);
            break;
          }
          break;
      }
      return (IEnumerable<StructureNodeDTO>) nodesCollection;
    }

    private Dictionary<Guid, object> GetEntitiesDictionary(IList<StructureNode> structureNodeList)
    {
      Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
      foreach (StructureNode structureNode in (IEnumerable<StructureNode>) structureNodeList)
      {
        if (structureNode.EntityId != Guid.Empty)
        {
          StructureNodeTypeEnum structureNodeTypeName = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNode.EntityName, true);
          object entity = this.GetStructuresManagerInstance().GetEntity(structureNodeTypeName, structureNode);
          entitiesDictionary.Add(structureNode.EntityId, entity);
        }
      }
      return entitiesDictionary;
    }

    private List<Guid> GetNodeIdList(IEnumerable<StructureNodeLinks> structureNodeLinks)
    {
      List<Guid> nodeIDs = new List<Guid>();
      foreach (StructureNodeLinks structureNodeLinks1 in structureNodeLinks.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (structureNode => !nodeIDs.Contains(structureNode.Node.Id))))
        nodeIDs.Add(structureNodeLinks1.Node.Id);
      return nodeIDs;
    }

    private void SetSelectedRootStructureNode()
    {
      if (this._selectedOrder != null)
      {
        Guid selectedRootNodeId = this._selectedOrder.RootStructureNodeId;
        using (IEnumerator<StructureNodeDTO> enumerator = this.StructureNodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (s => s.Id == selectedRootNodeId)).GetEnumerator())
        {
          if (!enumerator.MoveNext())
            return;
          this.SelectedStructureNodeItem = enumerator.Current;
        }
      }
      else if (this._selectedRootStructureNodeId != Guid.Empty)
      {
        using (IEnumerator<StructureNodeDTO> enumerator = this.StructureNodeCollection.Where<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (s => s.Id == this._selectedRootStructureNodeId)).GetEnumerator())
        {
          if (enumerator.MoveNext())
            this.SelectedStructureNodeItem = enumerator.Current;
        }
      }
    }
  }
}
