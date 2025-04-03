// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.OrdersViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMMWrapper;
using MSS.Business.Modules.OrdersManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Meters;
using MSS_Client.ViewModel.Structures;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using ZENNER;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class OrdersViewModel : ViewModelBase
  {
    private readonly IRepository<Order> _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<StructureNodeType> _structureNodeTypeRepository;
    private readonly IRepository<MeterReplacementHistory> _meterReplacementHistoryRepository;
    private readonly ISession _nhSession;
    private readonly IWindowFactory _windowFactory;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly UsersManager _userManager;
    private ApplicationTabsEnum _selectedTab;
    private bool _isInstallationTabSelected;
    private int _selectedIndex;
    private bool _isReadingTabSelected;
    private IEnumerable<OrderDTO> _getInstallationOrders;
    private IEnumerable<OrderDTO> _getReadingOrders;
    private OrderDTO _selectedOrder;
    private ObservableCollection<OrderDTO> _selectedOrders = new ObservableCollection<OrderDTO>();
    private OrderDTO _selectedReadingOrder;
    private string _pageSize = string.Empty;
    private ViewModelBase _messageUserControl;
    private bool _isBusy;
    private bool _installationOrderVisibility;
    private bool _createInstallationOrderVisibility;
    private bool _deleteInstallationOrderVisibility;
    private bool _editInstallationOrderVisibility;
    private bool _executeInstallationOrderVisibility;
    private bool _attachTestInstallationOrderVisibility;
    private bool _readingOrderVisibility;
    private bool _createReadingOrderVisibility;
    private bool _deleteReadingOrderVisibility;
    private bool _editReadingOrderVisibility;
    private bool _executeReadingOrderVisibility;
    private ViewModelBase _messageUserControlInstallationOrder;
    private ViewModelBase _messageUserControlReadingOrder;
    private string _importExport;

    public OrdersViewModel()
    {
    }

    public OrdersViewModel(IRepositoryFactory repositoryFactory, IWindowFactory windowFactory)
    {
      this.IsBusy = true;
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._orderRepository = repositoryFactory.GetRepository<Order>();
      this._userRepository = repositoryFactory.GetUserRepository();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._nhSession = repositoryFactory.GetSession();
      this._meterReplacementHistoryRepository = repositoryFactory.GetRepository<MeterReplacementHistory>();
      this._userManager = new UsersManager(this._repositoryFactory);
      this.InstallationOrderVisibility = this._userManager.HasRight(OperationEnum.InstallationOrderView.ToString());
      this.CreateInstallationOrderVisibility = this._userManager.HasRight(OperationEnum.InstallationOrderCreate.ToString());
      this.DeleteInstallationOrderVisibility = this._userManager.HasRight(OperationEnum.InstallationOrderDelete.ToString());
      this.EditInstallationOrderVisibility = this._userManager.HasRight(OperationEnum.InstallationOrderEdit.ToString());
      this.ExecuteInstallationOrderVisibility = this._userManager.HasRight(OperationEnum.InstallationOrderExecute.ToString()) || this._userManager.HasRight(OperationEnum.ExecuteAnyOrder.ToString());
      UsersManager userManager1 = this._userManager;
      OperationEnum operationEnum = OperationEnum.InstallationOrderAttachTest;
      string operation1 = operationEnum.ToString();
      this.AttachTestInstallationOrderVisibility = userManager1.HasRight(operation1);
      UsersManager userManager2 = this._userManager;
      operationEnum = OperationEnum.UnlockOrder;
      string operation2 = operationEnum.ToString();
      this.UnlockOrderVisibility = userManager2.HasRight(operation2);
      UsersManager userManager3 = this._userManager;
      operationEnum = OperationEnum.ReadingOrderView;
      string operation3 = operationEnum.ToString();
      this.ReadingOrderVisibility = userManager3.HasRight(operation3);
      UsersManager userManager4 = this._userManager;
      operationEnum = OperationEnum.ReadingOrderCreate;
      string operation4 = operationEnum.ToString();
      this.CreateReadingOrderVisibility = userManager4.HasRight(operation4);
      UsersManager userManager5 = this._userManager;
      operationEnum = OperationEnum.ReadingOrderEdit;
      string operation5 = operationEnum.ToString();
      this.EditReadingOrderVisibility = userManager5.HasRight(operation5);
      UsersManager userManager6 = this._userManager;
      operationEnum = OperationEnum.ReadingOrderDelete;
      string operation6 = operationEnum.ToString();
      this.DeleteReadingOrderVisibility = userManager6.HasRight(operation6);
      UsersManager userManager7 = this._userManager;
      operationEnum = OperationEnum.ReadingOrderExecute;
      string operation7 = operationEnum.ToString();
      int num;
      if (!userManager7.HasRight(operation7))
      {
        UsersManager userManager8 = this._userManager;
        operationEnum = OperationEnum.ExecuteAnyOrder;
        string operation8 = operationEnum.ToString();
        num = userManager8.HasRight(operation8) ? 1 : 0;
      }
      else
        num = 1;
      this.ExecuteReadingOrderVisibility = num != 0;
      this.PageSize = MSS.Business.Utils.AppContext.Current.GetParameterValue<string>(nameof (PageSize));
      EventPublisher.Register<ActionSearch<OrderDTO>>(new Action<ActionSearch<OrderDTO>>(this.RefreshOrdersAfterSearch));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(this.CreateMessage));
      EventPublisher.Register<SelectedTabChanged>((Action<SelectedTabChanged>) (changed => this.SelectedTab = changed.SelectedTab));
      EventPublisher.Register<StructureUpdated>(new Action<StructureUpdated>(this.RefreshStructure));
      EventPublisher.Register<SelectedTabValue>(new Action<SelectedTabValue>(this.SetTab));
      GmmInterface.DeviceManager.SelectedFilter = (string) null;
      if (this.InstallationOrderVisibility)
      {
        this.IsInstallationTabSelected = true;
        this.SelectedIndex = 0;
      }
      else
      {
        this.IsReadingTabSelected = true;
        this.SelectedIndex = 1;
      }
      this.InitializeOrders();
    }

    private void SetTab(SelectedTabValue selectedTabValue)
    {
      switch (selectedTabValue.Tab)
      {
        case ApplicationTabsEnum.ReadingOrders:
          this.SelectedIndex = 1;
          break;
        case ApplicationTabsEnum.InstallationOrders:
          this.SelectedIndex = 0;
          break;
      }
    }

    private void RefreshStructure(StructureUpdated update)
    {
      if (!(update.Guid != Guid.Empty))
        return;
      this._repositoryFactory.GetRepository<StructureNode>().Refresh((object) update.Guid);
      if (update.EntityId != Guid.Empty)
      {
        switch (update.EntityType)
        {
          case StructureNodeTypeEnum.Location:
            this._repositoryFactory.GetRepository<Location>().Refresh((object) update.EntityId);
            break;
          case StructureNodeTypeEnum.Tenant:
            this._repositoryFactory.GetRepository<Tenant>().Refresh((object) update.EntityId);
            break;
          case StructureNodeTypeEnum.Meter:
          case StructureNodeTypeEnum.RadioMeter:
            this._repositoryFactory.GetRepository<Meter>().Refresh((object) update.EntityId);
            break;
        }
      }
    }

    public async void InitializeOrders()
    {
      await Task.Run((Action) (() =>
      {
        Mapper.CreateMap<Order, OrderDTO>();
        Mapper.CreateMap<OrderDTO, Order>();
        OrdersManager ordersManager = new OrdersManager(this._repositoryFactory);
        this.GetReadingOrders = ordersManager.GetReadingOrdersDTO();
        this.GetInstallationOrders = ordersManager.GetInstallationOrdersDTO();
      }));
      this.IsBusy = false;
    }

    private void SetSelectedTab()
    {
      switch (this.SelectedTab)
      {
        case ApplicationTabsEnum.ReadingOrders:
          this._isReadingTabSelected = true;
          break;
        case ApplicationTabsEnum.InstallationOrders:
          this._isInstallationTabSelected = true;
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
      if (this.IsInstallationTabSelected)
        this.MessageUserControlInstallationOrder = viewModelBase;
      if (!this.IsReadingTabSelected)
        return;
      this.MessageUserControlReadingOrder = viewModelBase;
    }

    private OrdersManager GetOrdersManagerInstance() => new OrdersManager(this._repositoryFactory);

    private StructuresManager GetStructureManagerInsance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    public bool IsInstallationTabSelected
    {
      get => this._isInstallationTabSelected;
      set
      {
        this._isInstallationTabSelected = value;
        if (!this._isInstallationTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.InstallationOrders
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

    public bool IsReadingTabSelected
    {
      get => this._isReadingTabSelected;
      set
      {
        this._isReadingTabSelected = value;
        if (!this._isReadingTabSelected)
          return;
        EventPublisher.Publish<SelectedTabChanged>(new SelectedTabChanged()
        {
          SelectedTab = ApplicationTabsEnum.ReadingOrders
        }, (IViewModel) this);
      }
    }

    public IEnumerable<OrderDTO> GetInstallationOrders
    {
      get => this._getInstallationOrders;
      set
      {
        this._getInstallationOrders = value;
        this.OnPropertyChanged(nameof (GetInstallationOrders));
      }
    }

    public IEnumerable<OrderDTO> GetReadingOrders
    {
      get => this._getReadingOrders;
      set
      {
        this._getReadingOrders = value;
        this.OnPropertyChanged(nameof (GetReadingOrders));
      }
    }

    public OrderDTO SelectedOrder
    {
      get => this._selectedOrder;
      set
      {
        this._selectedOrder = value;
        this.OnPropertyChanged("ExecutableOrder");
        this.OnPropertyChanged("ExecutableInstallationOrder");
        this.OnPropertyChanged("EditableInstallationOrder");
        this.OnPropertyChanged("EditableReadingOrder");
        this.OnPropertyChanged("RemovableInstallationOrder");
        this.OnPropertyChanged("RemovableReadingOrder");
        if (this.SelectedTab == ApplicationTabsEnum.InstallationOrders)
          this.OnPropertyChanged("UnlockableInstallationOrder");
        else
          this.OnPropertyChanged("UnlockableReadingOrder");
      }
    }

    public ObservableCollection<OrderDTO> SelectedOrders
    {
      get => this._selectedOrders;
      set
      {
        this._selectedOrders = value;
        this.OnPropertyChanged(nameof (SelectedOrders));
      }
    }

    public OrderDTO SelectedReadingOrder
    {
      get => this._selectedReadingOrder;
      set
      {
        this._selectedReadingOrder = value;
        this.OnPropertyChanged("ExecutableOrder");
        this.OnPropertyChanged("ExecutableInstallationOrder");
        this.OnPropertyChanged("EditableInstallationOrder");
        this.OnPropertyChanged("EditableReadingOrder");
        this.OnPropertyChanged("RemovableInstallationOrder");
        this.OnPropertyChanged("RemovableReadingOrder");
        if (this.SelectedTab == ApplicationTabsEnum.InstallationOrders)
          this.OnPropertyChanged("UnlockableInstallationOrder");
        else
          this.OnPropertyChanged("UnlockableReadingOrder");
      }
    }

    public bool ExecutableInstallationOrder
    {
      get
      {
        if (this.SelectedOrder != null && !this.SelectedOrder.Exported && this.SelectedOrder.UserIds != null)
        {
          List<User> list = this.SelectedOrder.UserIds.Select<Guid, User>((Func<Guid, User>) (userID => this._userRepository.GetById((object) userID))).ToList<User>();
          StructureTypeEnum? structureType = this.SelectedOrder.StructureType;
          StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Fixed;
          if ((structureType.GetValueOrDefault() == structureTypeEnum1 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
          {
            structureType = this.SelectedOrder.StructureType;
            StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Physical;
            if ((structureType.GetValueOrDefault() == structureTypeEnum2 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
              goto label_5;
          }
          int num;
          if (this.SelectedOrder.Status == StatusOrderEnum.Dated || this.SelectedOrder.Status == StatusOrderEnum.InProgress)
          {
            num = list.Select<User, Guid>((Func<User, Guid>) (l => l.Id)).ToList<Guid>().Contains(MSS.Business.Utils.AppContext.Current.LoggedUser.Id) ? 1 : (this._userManager.HasRight(OperationEnum.ExecuteAnyOrder.ToString()) ? 1 : 0);
            goto label_6;
          }
label_5:
          num = 0;
label_6:
          if (num != 0)
            return true;
        }
        return false;
      }
    }

    public bool ExecutableOrder
    {
      get
      {
        if (this.SelectedReadingOrder != null && !this.SelectedReadingOrder.Exported && this.SelectedReadingOrder.UserIds != null)
        {
          List<User> list = this.SelectedReadingOrder.UserIds.Select<Guid, User>((Func<Guid, User>) (userID => this._userRepository.GetById((object) userID))).ToList<User>();
          StructureTypeEnum? structureType = this.SelectedReadingOrder.StructureType;
          StructureTypeEnum structureTypeEnum1 = StructureTypeEnum.Fixed;
          if ((structureType.GetValueOrDefault() == structureTypeEnum1 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
          {
            structureType = this.SelectedReadingOrder.StructureType;
            StructureTypeEnum structureTypeEnum2 = StructureTypeEnum.Physical;
            if ((structureType.GetValueOrDefault() == structureTypeEnum2 ? (structureType.HasValue ? 1 : 0) : 0) == 0)
              goto label_5;
          }
          int num;
          if (this.SelectedReadingOrder.Status == StatusOrderEnum.Dated || this.SelectedReadingOrder.Status == StatusOrderEnum.InProgress)
          {
            num = list.Select<User, Guid>((Func<User, Guid>) (l => l.Id)).ToList<Guid>().Contains(MSS.Business.Utils.AppContext.Current.LoggedUser.Id) ? 1 : (this._userManager.HasRight(OperationEnum.ExecuteAnyOrder.ToString()) ? 1 : 0);
            goto label_6;
          }
label_5:
          num = 0;
label_6:
          if (num != 0)
            return true;
        }
        return false;
      }
    }

    public bool EditableInstallationOrder
    {
      get
      {
        if (this.SelectedOrder != null)
        {
          Guid? lockedBy = this.SelectedOrder.LockedBy;
          Guid empty = Guid.Empty;
          int num;
          if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
          {
            lockedBy = this.SelectedOrder.LockedBy;
            if (lockedBy.HasValue)
            {
              num = 0;
              goto label_5;
            }
          }
          num = this.SelectedOrder.Status != StatusOrderEnum.Closed ? 1 : 0;
label_5:
          if (num != 0)
            return true;
        }
        return false;
      }
    }

    public bool EditableReadingOrder
    {
      get
      {
        if (this.SelectedReadingOrder != null)
        {
          Guid? lockedBy = this.SelectedReadingOrder.LockedBy;
          Guid empty = Guid.Empty;
          int num;
          if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
          {
            lockedBy = this.SelectedReadingOrder.LockedBy;
            if (lockedBy.HasValue)
            {
              num = 0;
              goto label_5;
            }
          }
          num = this.SelectedReadingOrder.Status != StatusOrderEnum.Closed ? 1 : 0;
label_5:
          if (num != 0)
            return true;
        }
        return false;
      }
    }

    public bool RemovableInstallationOrder
    {
      get
      {
        if (this.SelectedOrder != null)
        {
          Guid? lockedBy = this.SelectedOrder.LockedBy;
          Guid empty = Guid.Empty;
          int num;
          if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
          {
            lockedBy = this.SelectedOrder.LockedBy;
            num = !lockedBy.HasValue ? 1 : 0;
          }
          else
            num = 1;
          if (num != 0)
            return true;
        }
        return false;
      }
    }

    public bool RemovableReadingOrder
    {
      get
      {
        if (this.SelectedReadingOrder != null)
        {
          Guid? lockedBy = this.SelectedReadingOrder.LockedBy;
          Guid empty = Guid.Empty;
          int num;
          if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
          {
            lockedBy = this.SelectedReadingOrder.LockedBy;
            num = !lockedBy.HasValue ? 1 : 0;
          }
          else
            num = 1;
          if (num != 0)
            return true;
        }
        return false;
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

    public bool UnlockableInstallationOrder
    {
      get
      {
        if (this.SelectedOrder == null)
          return false;
        Guid? lockedBy = this.SelectedOrder.LockedBy;
        Guid empty = Guid.Empty;
        int num;
        if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
        {
          lockedBy = this.SelectedOrder.LockedBy;
          num = !lockedBy.HasValue ? 1 : 0;
        }
        else
          num = 1;
        return num == 0;
      }
    }

    public bool UnlockableReadingOrder
    {
      get
      {
        if (this.SelectedReadingOrder == null)
          return false;
        Guid? lockedBy = this.SelectedReadingOrder.LockedBy;
        Guid empty = Guid.Empty;
        int num;
        if ((lockedBy.HasValue ? (lockedBy.HasValue ? (lockedBy.GetValueOrDefault() == empty ? 1 : 0) : 1) : 0) == 0)
        {
          lockedBy = this.SelectedReadingOrder.LockedBy;
          num = !lockedBy.HasValue ? 1 : 0;
        }
        else
          num = 1;
        return num == 0;
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

    public bool InstallationOrderVisibility
    {
      get => this._installationOrderVisibility;
      set
      {
        this._installationOrderVisibility = value;
        this.OnPropertyChanged(nameof (InstallationOrderVisibility));
      }
    }

    public bool CreateInstallationOrderVisibility
    {
      get => this._createInstallationOrderVisibility;
      set
      {
        this._createInstallationOrderVisibility = value;
        this.OnPropertyChanged(nameof (CreateInstallationOrderVisibility));
      }
    }

    public bool DeleteInstallationOrderVisibility
    {
      get => this._deleteInstallationOrderVisibility;
      set
      {
        this._deleteInstallationOrderVisibility = value;
        this.OnPropertyChanged(nameof (DeleteInstallationOrderVisibility));
      }
    }

    public bool EditInstallationOrderVisibility
    {
      get => this._editInstallationOrderVisibility;
      set
      {
        this._editInstallationOrderVisibility = value;
        this.OnPropertyChanged(nameof (EditInstallationOrderVisibility));
      }
    }

    public bool ExecuteInstallationOrderVisibility
    {
      get => this._executeInstallationOrderVisibility;
      set
      {
        this._executeInstallationOrderVisibility = value;
        this.OnPropertyChanged(nameof (ExecuteInstallationOrderVisibility));
      }
    }

    public bool AttachTestInstallationOrderVisibility
    {
      get => this._attachTestInstallationOrderVisibility;
      set
      {
        this._attachTestInstallationOrderVisibility = value;
        this.OnPropertyChanged(nameof (AttachTestInstallationOrderVisibility));
      }
    }

    public bool UnlockOrderVisibility { get; set; }

    public bool ReadingOrderVisibility
    {
      get => this._readingOrderVisibility;
      set
      {
        this._readingOrderVisibility = value;
        this.OnPropertyChanged(nameof (ReadingOrderVisibility));
      }
    }

    public bool CreateReadingOrderVisibility
    {
      get => this._createReadingOrderVisibility;
      set
      {
        this._createReadingOrderVisibility = value;
        this.OnPropertyChanged(nameof (CreateReadingOrderVisibility));
      }
    }

    public bool DeleteReadingOrderVisibility
    {
      get => this._deleteReadingOrderVisibility;
      set
      {
        this._deleteReadingOrderVisibility = value;
        this.OnPropertyChanged(nameof (DeleteReadingOrderVisibility));
      }
    }

    public bool EditReadingOrderVisibility
    {
      get => this._editReadingOrderVisibility;
      set
      {
        this._editReadingOrderVisibility = value;
        this.OnPropertyChanged(nameof (EditReadingOrderVisibility));
      }
    }

    public bool ExecuteReadingOrderVisibility
    {
      get => this._executeReadingOrderVisibility;
      set
      {
        this._executeReadingOrderVisibility = value;
        this.OnPropertyChanged(nameof (ExecuteReadingOrderVisibility));
      }
    }

    public System.Windows.Input.ICommand CreateInstallationOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditOrderViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) null), (IParameter) new ConstructorArgument("orderType", (object) OrderTypeEnum.InstallationOrder)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.GetInstallationOrders = this.GetOrdersManagerInstance().GetInstallationOrdersDTO();
            this.MessageUserControlInstallationOrder = MessageHandlingManager.ShowSuccessMessage(CultureResources.GetValue("MSS_Client_SuccessMessage"));
          }
          else
            this.MessageUserControlInstallationOrder = MessageHandlingManager.ShowWarningMessage(CultureResources.GetValue("MSS_Client_OperationCancelledMessage"));
        }));
      }
    }

    public System.Windows.Input.ICommand EditInstallationOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!this.EditInstallationOrderVisibility)
            return;
          OrderDTO orderDto = parameter as OrderDTO;
          if (this.EditableInstallationOrder)
          {
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditOrderViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) orderDto), (IParameter) new ConstructorArgument("orderType", (object) OrderTypeEnum.InstallationOrder)));
            if (newModalDialog.HasValue && newModalDialog.Value)
            {
              this._orderRepository.Refresh((object) orderDto.Id);
              this.MessageUserControlInstallationOrder = MessageHandlingManager.ShowSuccessMessage(CultureResources.GetValue("MSS_Client_SuccessMessage"));
            }
            else
              this.MessageUserControlInstallationOrder = MessageHandlingManager.ShowWarningMessage(CultureResources.GetValue("MSS_Client_OperationCancelledMessage"));
            this.GetInstallationOrders = this.GetOrdersManagerInstance().GetInstallationOrdersDTO();
          }
          else
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Structure_Warning_Order_Not_Editable, false);
        }));
      }
    }

    public System.Windows.Input.ICommand DeleteInstallationOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          List<OrderDTO> list = (parameter as ObservableCollection<object>).Cast<OrderDTO>().ToList<OrderDTO>();
          string str = CultureResources.GetValue("MSS_Client_Orders_DeleteInstallationOrder_Title");
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DeleteSingleOrderViewModel>((IParameter) new ConstructorArgument("selectedOrders", (object) list), (IParameter) new ConstructorArgument("deleteOrderTitle", (object) str)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            list.ForEach((Action<OrderDTO>) (x => this._orderRepository.Refresh((object) x.Id)));
            this.MessageUserControlInstallationOrder = MessageHandlingManager.ShowSuccessMessage(CultureResources.GetValue("MSS_Client_SuccessMessage"));
          }
          else
            this.MessageUserControlInstallationOrder = MessageHandlingManager.ShowWarningMessage(CultureResources.GetValue("MSS_Client_OperationCancelledMessage"));
          this.GetInstallationOrders = this.GetOrdersManagerInstance().GetInstallationOrdersDTO();
        }));
      }
    }

    public System.Windows.Input.ICommand ExecuteInstallationOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new AsyncCommand<object>((Func<object, Task<object>>) (async t =>
        {
          this.IsBusy = true;
          await Task.Run((Action) (() => this.IsFixedNetworkScenario().If<bool>((Func<bool, bool>) (_ => _), (Action<bool>) (_ => this.ExecuteFixedStructure(t)), (Action<bool>) (_ => this.ExecutePsychicalStructure(t)))));
          this.IsBusy = false;
          if (this._selectedOrder != null)
          {
            this._orderRepository.Refresh((object) this._selectedOrder.Id);
            this.GetInstallationOrders = this.GetOrdersManagerInstance().GetInstallationOrdersDTO();
          }
          return (object) null;
        }));
      }
    }

    public System.Windows.Input.ICommand UnlockInstallationOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is OrderDTO orderDto2))
            return;
          orderDto2.LockedBy = new Guid?();
          this.GetOrdersManagerInstance().EditOrder(orderDto2, new bool?(false));
          this.UnlockStructureNodes(Mapper.Map<OrderDTO, Order>(orderDto2));
          this.GetInstallationOrders = this.GetOrdersManagerInstance().GetInstallationOrdersDTO();
        }));
      }
    }

    private void ExecuteFixedStructure(object parameter)
    {
      OrderDTO selectedOrder = parameter.SafeCast<OrderDTO>();
      Guid rootStructureNodeId = selectedOrder.RootStructureNodeId;
      IList<StructureNodeType> all1 = this._repositoryFactory.GetRepository<StructureNodeType>().GetAll();
      IList<MeterReplacementHistory> all2 = this._repositoryFactory.GetRepository<MeterReplacementHistory>().GetAll();
      ObservableCollection<StructureNodeDTO> structureNodeDTO = new StructuresManager(this._repositoryFactory).GetNodeCollectionWithChildren(this._repositoryFactory.GetStructureNodeRepository(), new StructureTypeEnum?(StructureTypeEnum.Fixed), all1, all2, rootStructureNodeId);
      Application.Current.Dispatcher.Invoke((Action) (() => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ExecuteInstallationOrderViewModel>("ExecuteInstallationOrderViewModel", (IParameter) new ConstructorArgument("orderNumber", (object) selectedOrder.InstallationNumber), (IParameter) new ConstructorArgument("selectedNode", (object) structureNodeDTO.First<StructureNodeDTO>())))));
    }

    private void ExecutePsychicalStructure(object parameter)
    {
      if (!(parameter is OrderDTO orderDto))
        return;
      IEnumerable<StructureNodeDTO> nodeDtoForRootNode = OrdersHelper.GetStructureNodeDTOForRootNode(orderDto.RootStructureNodeId, this._structureNodeLinksRepository, this._structureNodeRepository, this._structureNodeTypeRepository, this._nhSession, (IList<MeterReplacementHistorySerializableDTO>) StructuresHelper.GetMeterReplacementHistorySerializableDTO(this._meterReplacementHistoryRepository.GetAll()));
      bool? isOkButton = new bool?(false);
      StructureTypeEnum? structureType = orderDto.StructureType;
      if (structureType.HasValue)
      {
        switch (structureType.GetValueOrDefault())
        {
          case StructureTypeEnum.Physical:
            StructureNodeDTO selectedNode = nodeDtoForRootNode.FirstOrDefault<StructureNodeDTO>();
            Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              isOkButton = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditPhysicalStructureViewModel>((IParameter) new ConstructorArgument("selectedNode", (object) selectedNode), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) true), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false)));
              if (isOkButton.HasValue && isOkButton.Value)
                this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
              else
                this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            }));
            break;
          case StructureTypeEnum.Fixed:
            StructureNodeDTO selectedFixedNode = nodeDtoForRootNode.First<StructureNodeDTO>();
            Application.Current.Dispatcher.Invoke((Action) (() =>
            {
              isOkButton = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditFixedStructureViewModel>("EditFixedStructureForOrderViewModel", (IParameter) new ConstructorArgument("selectedNode", (object) selectedFixedNode), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) true), (IParameter) new ConstructorArgument("orderDTO", (object) this._selectedOrder)));
              if (isOkButton.HasValue && isOkButton.Value)
                this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
              else
                this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
            }));
            break;
        }
      }
    }

    private Location GetLocationByOrder()
    {
      StructureNode byId = this._structureNodeRepository.GetById((object) this.SelectedOrder.RootStructureNodeId);
      return byId != null ? this._repositoryFactory.GetRepository<Location>().GetById((object) byId.EntityId) : (Location) null;
    }

    private bool IsFixedNetworkScenario()
    {
      StructureTypeEnum? structureType = this.SelectedOrder.StructureType;
      StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
      int num;
      if ((structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0)
      {
        Location locationByOrder1 = this.GetLocationByOrder();
        if ((locationByOrder1 != null ? (locationByOrder1.Generation == GenerationEnum.Radio3 ? 1 : 0) : 0) != 0)
        {
          Location locationByOrder2 = this.GetLocationByOrder();
          if ((locationByOrder2 != null ? (locationByOrder2.Scenario.Code == 0 ? 1 : 0) : 0) == 0)
          {
            Location locationByOrder3 = this.GetLocationByOrder();
            if ((locationByOrder3 != null ? (locationByOrder3.Scenario.Code == 1 ? 1 : 0) : 0) == 0)
            {
              Location locationByOrder4 = this.GetLocationByOrder();
              num = locationByOrder4 != null ? (locationByOrder4.Scenario.Code == 4 ? 1 : 0) : 0;
              goto label_7;
            }
          }
          num = 1;
          goto label_7;
        }
      }
      num = 0;
label_7:
      return num != 0;
    }

    private bool ValidateOrder(OrderDTO selectedOrder)
    {
      if (selectedOrder.RootStructureNodeId != Guid.Empty)
      {
        Guid structureRootNodeId = selectedOrder.RootStructureNodeId;
        List<Guid> nodeIDs = StructuresHelper.GetNodeIdList(StructuresHelper.GetStructureNodesForRootNode(this._structureNodeLinksRepository, this._structureNodeLinksRepository.FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (s => s.Node.Id == structureRootNodeId)).Node.Id).Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (s => !s.EndDate.HasValue)));
        IRepository<StructureNode> structureNodeRepository = this._structureNodeRepository;
        Expression<Func<StructureNode, bool>> predicate = (Expression<Func<StructureNode, bool>>) (s => nodeIDs.Contains(s.Id) && s.EndDate == new DateTime?());
        foreach (StructureNode structureNode in (IEnumerable<StructureNode>) structureNodeRepository.SearchFor(predicate))
        {
          if (!(structureNode.EntityId != Guid.Empty))
            return false;
          switch ((StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNode.EntityName, true))
          {
            case StructureNodeTypeEnum.Location:
              object entity1 = (object) StructuresHelper.GetEntity<Location>(structureNode.EntityId, this._nhSession);
              if (!((Location) entity1).Validate<object>(entity1))
                return false;
              break;
            case StructureNodeTypeEnum.Tenant:
              object entity2 = (object) StructuresHelper.GetEntity<Tenant>(structureNode.EntityId, this._nhSession);
              if (!(entity2 as Tenant).Validate<object>(entity2))
                return false;
              break;
            case StructureNodeTypeEnum.Meter:
            case StructureNodeTypeEnum.RadioMeter:
              object entity3 = (object) StructuresHelper.GetEntity<Meter>(structureNode.EntityId, this._nhSession);
              if (!((Meter) entity3).Validate<object>(entity3))
                return false;
              break;
          }
        }
      }
      return true;
    }

    public System.Windows.Input.ICommand CreateReadingOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditOrderViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) null), (IParameter) new ConstructorArgument("orderType", (object) OrderTypeEnum.ReadingOrder)));
          if (newModalDialog.HasValue && newModalDialog.Value)
          {
            this.GetReadingOrders = this.GetOrdersManagerInstance().GetReadingOrdersDTO();
            this.MessageUserControlReadingOrder = MessageHandlingManager.ShowSuccessMessage(CultureResources.GetValue("MSS_Client_SuccessMessage"));
          }
          else
            this.MessageUserControlReadingOrder = MessageHandlingManager.ShowWarningMessage(CultureResources.GetValue("MSS_Client_OperationCancelledMessage"));
        }));
      }
    }

    public System.Windows.Input.ICommand EditReadingOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (this.EditableReadingOrder)
          {
            OrderDTO orderDto = parameter as OrderDTO;
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<CreateEditOrderViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) orderDto), (IParameter) new ConstructorArgument("orderType", (object) OrderTypeEnum.ReadingOrder)));
            if (newModalDialog.HasValue && newModalDialog.Value)
            {
              this._orderRepository.Refresh((object) orderDto.Id);
              this.MessageUserControlReadingOrder = MessageHandlingManager.ShowSuccessMessage(CultureResources.GetValue("MSS_Client_SuccessMessage"));
            }
            else
              this.MessageUserControlReadingOrder = MessageHandlingManager.ShowWarningMessage(CultureResources.GetValue("MSS_Client_OperationCancelledMessage"));
            this.GetReadingOrders = this.GetOrdersManagerInstance().GetReadingOrdersDTO();
          }
          else
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Structure_Warning_Order_Not_Editable, false);
        }));
      }
    }

    public System.Windows.Input.ICommand DeleteReadingOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (parameter is ObservableCollection<object> source2)
          {
            List<OrderDTO> list = source2.Cast<OrderDTO>().ToList<OrderDTO>();
            string str = CultureResources.GetValue("MSS_Client_Orders_DeleteReadingOrder_Title");
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<DeleteSingleOrderViewModel>((IParameter) new ConstructorArgument("selectedOrders", (object) list), (IParameter) new ConstructorArgument("deleteOrderTitle", (object) str)));
            if (newModalDialog.HasValue && newModalDialog.Value)
            {
              list.ForEach((Action<OrderDTO>) (x => this._orderRepository.Refresh((object) x.Id)));
              this.MessageUserControlReadingOrder = MessageHandlingManager.ShowSuccessMessage(CultureResources.GetValue("MSS_Client_SuccessMessage"));
            }
            else
              this.MessageUserControlReadingOrder = MessageHandlingManager.ShowWarningMessage(CultureResources.GetValue("MSS_Client_OperationCancelledMessage"));
          }
          this.GetReadingOrders = this.GetOrdersManagerInstance().GetReadingOrdersDTO();
        }));
      }
    }

    public System.Windows.Input.ICommand ViewStructureCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          ViewModelBase viewModelBase = (ViewModelBase) null;
          if (parameter is OrderDTO orderDto2)
          {
            bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<StructureOrdersViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) orderDto2), (IParameter) new ConstructorArgument("orderType", (object) orderDto2.OrderType), (IParameter) new ConstructorArgument("viewMode", (object) true), (IParameter) new ConstructorArgument("selectedRootStructureNodeId", (object) Guid.Empty), (IParameter) new ConstructorArgument("orderDueDate", (object) orderDto2.DueDate)));
            viewModelBase = !newModalDialog.HasValue || !newModalDialog.Value ? MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue()) : MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
          }
          if (this.IsInstallationTabSelected)
            this.MessageUserControlInstallationOrder = viewModelBase;
          if (!this.IsReadingTabSelected)
            return;
          this.MessageUserControlReadingOrder = viewModelBase;
        }));
      }
    }

    public System.Windows.Input.ICommand ExecuteReadingOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          OrderDTO orderDto = parameter as OrderDTO;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<ExecuteReadingOrderViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) orderDto), (IParameter) new ConstructorArgument("deviceManager", (object) new DeviceManagerWrapper())));
          GmmInterface.DeviceManager.SelectedFilter = (string) null;
          if (orderDto != null)
            this._orderRepository.Refresh((object) orderDto.Id);
          this.GetReadingOrders = this.GetOrdersManagerInstance().GetReadingOrdersDTO();
        }));
      }
    }

    public System.Windows.Input.ICommand UnlockReadingOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is OrderDTO orderDTO2))
            return;
          orderDTO2.LockedBy = new Guid?();
          this.GetOrdersManagerInstance().EditOrder(orderDTO2, new bool?(false));
          this.GetReadingOrders = this.GetOrdersManagerInstance().GetReadingOrdersDTO();
        }));
      }
    }

    private void UpdateLockedStructureNodes(
      IEnumerable<StructureNode> structureNodes,
      IEnumerable<StructureNodeLinks> structureNodeLinks)
    {
      this._nhSession.FlushMode = FlushMode.Commit;
      ITransaction transaction = this._nhSession.BeginTransaction();
      foreach (StructureNode structureNode in structureNodes)
        this.GetStructureManagerInsance().UpdateStructureNode(structureNode);
      foreach (StructureNodeLinks structureNodeLink in structureNodeLinks)
        this.GetStructureManagerInsance().UpdateStructureNodeLink(structureNodeLink);
      transaction.Commit();
    }

    private void UnlockStructureNodes(Order order)
    {
      Guid rootStructureNodeId = order.RootStructureNodeId;
      this.GetStructureManagerInsance().UnlockStructure(rootStructureNodeId);
    }

    public System.Windows.Input.ICommand ViewReadingValuesCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is OrderDTO orderDto2))
            return;
          this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MeterReadingValuesViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) orderDto2)));
        }));
      }
    }

    public System.Windows.Input.ICommand PrintInstallationOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<PrintPreviewViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) this.SelectedOrder)))));
      }
    }

    public System.Windows.Input.ICommand PrintReadingOrderCommand
    {
      get
      {
        return (System.Windows.Input.ICommand) new RelayCommand((Action<object>) (parameter => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<PrintPreviewViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) this.SelectedReadingOrder)))));
      }
    }

    public ViewModelBase MessageUserControlInstallationOrder
    {
      get => this._messageUserControlInstallationOrder;
      set
      {
        this._messageUserControlInstallationOrder = value;
        this.OnPropertyChanged(nameof (MessageUserControlInstallationOrder));
      }
    }

    public ViewModelBase MessageUserControlReadingOrder
    {
      get => this._messageUserControlReadingOrder;
      set
      {
        this._messageUserControlReadingOrder = value;
        this.OnPropertyChanged(nameof (MessageUserControlReadingOrder));
      }
    }

    public string ImportExport
    {
      get => this._importExport;
      set
      {
        this._importExport = value;
        this.OnPropertyChanged(nameof (ImportExport));
      }
    }

    private void RefreshOrdersAfterSearch(ActionSearch<OrderDTO> update)
    {
      bool flag = update.Message == null;
      switch (update.SelectedTab)
      {
        case ApplicationTabsEnum.ReadingOrders:
          this._getReadingOrders = update.ObservableCollection.Count == 0 ? this.GetOrdersManagerInstance().GetReadingOrdersDTO() : (IEnumerable<OrderDTO>) update.ObservableCollection;
          this.OnPropertyChanged("GetReadingOrders");
          break;
        case ApplicationTabsEnum.InstallationOrders:
          this._getInstallationOrders = update.ObservableCollection.Count == 0 ? this.GetOrdersManagerInstance().GetInstallationOrdersDTO() : (IEnumerable<OrderDTO>) update.ObservableCollection;
          this.OnPropertyChanged("GetInstallationOrders");
          break;
      }
      if (flag)
        return;
      if (this.IsInstallationTabSelected)
        this.MessageUserControlInstallationOrder = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
      if (!this.IsReadingTabSelected)
        return;
      this.MessageUserControlReadingOrder = MessageHandlingManager.ShowWarningMessage(update.Message.MessageText);
    }
  }
}
