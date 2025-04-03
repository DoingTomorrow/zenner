// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.CreateEditOrderViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.OrdersManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.DataFilters;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DIConfiguration;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.DataFilters;
using MSS_Client.ViewModel.Structures;
using MVVM.Commands;
using MVVM.ViewModel;
using NHibernate;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class CreateEditOrderViewModel : ValidationViewModelBase
  {
    private readonly IUserRepository _userRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<StructureNodeType> _structureNodeTypeRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<MeterReplacementHistory> _meterReplacementHistoryRepository;
    private readonly IRepository<MSS.Core.Model.DataFilters.Filter> _filterRepository;
    private readonly IWindowFactory _windowFactory;
    private readonly ISession _nhSession;
    private readonly OrderDTO _selectedOrder;
    private OrderTypeEnum _orderType;
    private bool _instNumberUniquenessChecked;
    private bool _instNoExists;
    private readonly IRepositoryFactory _repositoryFactory;
    private IEnumerable<MSS.Core.Model.DataFilters.Filter> _filterCollection;
    private Guid _selectedFilterId;
    private string _orderDialogTitle;
    private bool _isEditStructureButtonVisible;
    private bool _isReasonVisible;
    private string _installationNumberValue;
    private string _deviceNumberValue;
    private string _detailValue;
    private bool _exportedValue;
    private DateTime? _dueDateValue;
    private StatusOrderEnum _selectedStatus;
    private CloseOrderReasonsEnum _selectedReason;
    private Guid _rootStructureNodeId;
    private byte[] _structureBytes;
    private StructureTypeEnum? _structureType;
    private User _selectedUser;
    private ViewModelBase _messageUserControl;
    private ObservableCollection<User> _userList;
    private bool _printButtonEnabled;
    private bool _editStructureButtonVisibility;
    private string _structureInfo;
    private int _dueDateYear;

    [Inject]
    public CreateEditOrderViewModel(
      OrderDTO selectedOrder,
      OrderTypeEnum orderType,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
    {
      StructuresHelper.InitializeMappings();
      this._repositoryFactory = repositoryFactory;
      this._windowFactory = windowFactory;
      this._selectedOrder = selectedOrder;
      this._userRepository = repositoryFactory.GetUserRepository();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._orderRepository = repositoryFactory.GetRepository<Order>();
      this._meterReplacementHistoryRepository = repositoryFactory.GetRepository<MeterReplacementHistory>();
      this._filterRepository = repositoryFactory.GetRepository<MSS.Core.Model.DataFilters.Filter>();
      this._nhSession = repositoryFactory.GetSession();
      bool flag1 = false;
      this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._filterRepository.GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
      this.SelectedFilterId = selectedOrder != null ? selectedOrder.FilterId : Guid.Empty;
      int num;
      if (orderType == OrderTypeEnum.ReadingOrder)
      {
        Guid selectedFilterId = this.SelectedFilterId;
        num = this.SelectedFilterId == Guid.Empty ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        MSS.Core.Model.DataFilters.Filter filter = this.FilterCollection.FirstOrDefault<MSS.Core.Model.DataFilters.Filter>((Func<MSS.Core.Model.DataFilters.Filter, bool>) (item => item.Name == "Any"));
        this.SelectedFilterId = filter != null ? filter.Id : Guid.Empty;
      }
      this.PrintButtonEnabled = true;
      this._orderType = orderType;
      switch (this._orderType)
      {
        case OrderTypeEnum.ReadingOrder:
          this.OrderDialogTitle = this._selectedOrder != null ? CultureResources.GetValue("MSS_Client_Orders_EditReadingOrder_Title") : CultureResources.GetValue("MSS_Client_Orders_CreateReadingOrder_Title");
          this.IsUserListBoxVisible = true;
          this.IsUserComboBoxVisible = true;
          this.IsYearDropDownVisible = true;
          this.IsReadingOrderDueDateYearEnabled = true;
          this.NumberText = CultureResources.GetValue("MSS_Client_OrderControl_Header_ReadingNumber");
          this.IsUserDropdownEnabled = this._selectedOrder == null || !this._selectedOrder.Exported;
          if (selectedOrder != null)
          {
            flag1 = selectedOrder.StructureBytes == null;
            break;
          }
          break;
        case OrderTypeEnum.InstallationOrder:
          this.OrderDialogTitle = this._selectedOrder != null ? CultureResources.GetValue("MSS_Client_Orders_EditInstallationOrder_Title") : CultureResources.GetValue("MSS_Client_Orders_CreateInstallationOrder_Title");
          this.IsUserListBoxVisible = true;
          this.IsUserComboBoxVisible = false;
          this.IsYearDropDownVisible = false;
          this.IsUserListBoxEnabled = this._selectedOrder == null || !this._selectedOrder.Exported;
          this.NumberText = CultureResources.GetValue("MSS_Client_OrderControl_Header_InstallationNumber");
          if (selectedOrder != null)
          {
            Guid rootStructureNodeId = selectedOrder.RootStructureNodeId;
            this._repositoryFactory.GetSession().Clear();
            List<MeterReplacementHistorySerializableDTO> historySerializableDto = StructuresHelper.GetMeterReplacementHistorySerializableDTO(this._meterReplacementHistoryRepository.GetAll());
            flag1 = !OrdersHelper.GetStructureNodeDTOForRootNode(rootStructureNodeId, this._structureNodeLinksRepository, this._structureNodeRepository, this._structureNodeTypeRepository, this._nhSession, (IList<MeterReplacementHistorySerializableDTO>) historySerializableDto).Any<StructureNodeDTO>();
            break;
          }
          break;
      }
      bool flag2 = this._selectedOrder != null;
      this.IsReadingOrder = orderType == OrderTypeEnum.ReadingOrder;
      this.IsInstallationOrder = orderType == OrderTypeEnum.InstallationOrder;
      this.IsAddOrderButtonVisible = !flag2;
      this.IsEditOrderButtonVisible = flag2;
      this.IsEditStructureButtonVisible = !flag1 & flag2;
      this.IsAssignStructureButtonVisible = !flag2;
      this.IsPrintButtonVisible = flag2;
      if (flag2)
      {
        this.IsInstallationOrderEdit = true;
        this.InstallationNumberValue = selectedOrder.InstallationNumber;
        this.DetailValue = selectedOrder.Details;
        this.ExportedValue = selectedOrder.Exported;
        this.SelectedStatus = selectedOrder.Status;
        IEnumerable<User> source = selectedOrder.UserIds != null ? selectedOrder.UserIds.Select<Guid, User>((Func<Guid, User>) (userID => this._userRepository.GetById((object) userID))) : (IEnumerable<User>) new List<User>();
        this._dueDateValue = new DateTime?(selectedOrder.DueDate);
        switch (orderType)
        {
          case OrderTypeEnum.ReadingOrder:
            this.SelectedUser = source.FirstOrDefault<User>();
            if (selectedOrder.StructureBytes != null)
              this.StructureInfo = selectedOrder.RootNodeName + "\n" + selectedOrder.RootNodeDescription;
            this._dueDateYear = selectedOrder.DueDate.Year;
            this.IsReadingOrderDueDateYearEnabled = false;
            break;
          case OrderTypeEnum.InstallationOrder:
            this.SelectedUser = source.FirstOrDefault<User>();
            this.StructureInfo = selectedOrder.RootNodeName + "\n" + selectedOrder.RootNodeDescription;
            this.IsReadingOrderDueDateYearEnabled = false;
            break;
        }
        this.DeviceNumberValue = selectedOrder.DeviceNumber;
        this.SelectedReason = selectedOrder.CloseOrderReason;
        this.RootStructureNodeId = selectedOrder.RootStructureNodeId;
        this.StructureBytes = selectedOrder.StructureBytes;
        this.StructureType = selectedOrder.StructureType;
      }
      else
      {
        this._dueDateValue = new DateTime?(DateTime.Now);
        this._dueDateYear = DateTime.Now.Year;
        this.SelectedStatus = StatusOrderEnum.New;
        this.InstallationNumberValue = this.GetNextInstallationNumber();
        this.UserList = new ObservableCollection<User>();
      }
      EventPublisher.Register<OrderUpdated>(new Action<OrderUpdated>(this.RefreshOrder));
      EventPublisher.Register<StructureBytesUpdated>(new Action<StructureBytesUpdated>(this.RefreshStructureBytes));
      EventPublisher.Register<StructureRootUpdated>(new Action<StructureRootUpdated>(this.RefreshStructure));
      EventPublisher.Register<RefreshFilters>(new Action<RefreshFilters>(this.RefreshFilter));
      this.UsersCollection = this.UserCollection() as ObservableCollection<User>;
    }

    private string GetNextInstallationNumber()
    {
      bool flag = true;
      string installationNumber = (string) null;
      while (flag)
      {
        string number1 = MSSHelper.GenerateRandomNumber(12);
        flag = this._orderRepository.Exists((Expression<Func<Order, bool>>) (o => o.InstallationNumber == number1));
        if (!flag)
          installationNumber = number1;
      }
      return installationNumber;
    }

    private void RefreshFilter(RefreshFilters obj)
    {
      this._nhSession.Clear();
      this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._filterRepository.GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
      MSS.Core.Model.DataFilters.Filter filter = this.FilterCollection.FirstOrDefault<MSS.Core.Model.DataFilters.Filter>((Func<MSS.Core.Model.DataFilters.Filter, bool>) (item => item.Id == this.SelectedFilterId));
      this.SelectedFilterId = filter != null ? filter.Id : Guid.Empty;
    }

    private void RefreshOrder(OrderUpdated update)
    {
      if (!(update.Guid != Guid.Empty))
        return;
      this.RootStructureNodeId = update.SelectedNodeDTO.Id;
      this.StructureType = update.StructureType;
      if (this._orderType == OrderTypeEnum.ReadingOrder)
      {
        IOrderedEnumerable<StructureNodeDTO> collection = StructuresHelper.Descendants(update.SelectedNodeDTO).OrderBy<StructureNodeDTO, int>((Func<StructureNodeDTO, int>) (n => n.OrderNr));
        Structure structure = this.GetStructureManagerInstance().GetStructure(new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) collection));
        this.StructureBytes = StructuresHelper.SerializeStructure(structure);
        this.StructureInfo = this.GetStructureManagerInstance().GetNameAndDescriptionRootForStructureBytes(this.StructureBytes);
        StructureTypeEnum? structureType = update.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
        {
          DateTime? structureDueDate = this.GetStructureManagerInstance().GetStructureDueDate(structure);
          DateTime? nullable = structureDueDate;
          DateTime minValue = DateTime.MinValue;
          this.DueDateValue = (nullable.HasValue ? (nullable.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0 ? structureDueDate : this.DueDateValue;
          this.ComputeDueDateYear();
        }
      }
      else
      {
        this.StructureInfo = this.GetStructureManagerInstance().GetNameAndDescriptionForRootNodeID(this.RootStructureNodeId);
        StructureTypeEnum? structureType = update.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
        {
          DateTime? structureDueDate = this.GetStructureManagerInstance().GetStructureDueDate(this.RootStructureNodeId);
          DateTime minValue = DateTime.MinValue;
          this.DueDateValue = (structureDueDate.HasValue ? (structureDueDate.GetValueOrDefault() > minValue ? 1 : 0) : 0) != 0 ? this.GetStructureManagerInstance().GetStructureDueDate(this.RootStructureNodeId) : this.DueDateValue;
        }
      }
    }

    private void ComputeDueDateYear()
    {
      DateTime t1;
      ref DateTime local = ref t1;
      DateTime now = DateTime.Now;
      int year = now.Year;
      now = this.DueDateValue.Value;
      int month = now.Month;
      now = this.DueDateValue.Value;
      int day = now.Day;
      local = new DateTime(year, month, day);
      this.DueDateYear = DateTime.Compare(t1, DateTime.Now) > 0 ? DateTime.Now.Year - 1 : DateTime.Now.Year;
    }

    private void RefreshStructure(StructureRootUpdated update)
    {
      this._repositoryFactory.GetSession().Clear();
      if (!(update.RootNodeId != Guid.Empty) || this._orderType != OrderTypeEnum.InstallationOrder)
        return;
      Guid rootNodeId = update.RootNodeId;
      this.StructureInfo = this.GetStructureManagerInstance().GetNameAndDescriptionForRootNodeID(rootNodeId);
    }

    private void RefreshStructureBytes(StructureBytesUpdated update)
    {
      if (update.StructureBytes == null)
        return;
      this.StructureBytes = update.StructureBytes;
      if (this._selectedOrder != null)
        this._selectedOrder.StructureBytes = this.StructureBytes;
      this.StructureInfo = this.GetStructureManagerInstance().GetNameAndDescriptionRootForStructureBytes(this.StructureBytes);
      int num;
      if (this._orderType == OrderTypeEnum.ReadingOrder)
      {
        StructureTypeEnum? structureType = this.StructureType;
        StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
        num = structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0;
      }
      else
        num = 0;
      if (num != 0)
        this.DueDateValue = this.GetStructureManagerInstance().GetStructureDueDate(this.StructureBytes);
    }

    private OrdersManager GetOrderManagerInstance() => new OrdersManager(this._repositoryFactory);

    private StructuresManager GetStructureManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    private UsersManager GetUsersManagerInstance() => new UsersManager(this._repositoryFactory);

    public override List<string> ValidateProperty(string propertyName)
    {
      string propertyName1 = this.GetPropertyName<string>((Expression<Func<string>>) (() => this.InstallationNumberValue));
      if (!(propertyName == propertyName1))
        return new List<string>();
      ICollection<string> validationErrors;
      this.ValidateInstallationNr(this.InstallationNumberValue, this._selectedOrder, out validationErrors);
      this.IsValid &= validationErrors.Count <= 0;
      return validationErrors.ToList<string>();
    }

    public bool ValidateInstallationNr(
      string installationNumberValue,
      OrderDTO selectedOrder,
      out ICollection<string> validationErrors)
    {
      validationErrors = (ICollection<string>) new List<string>();
      if (!this._instNumberUniquenessChecked)
      {
        int num;
        if (selectedOrder == null)
          num = this._orderRepository.Exists((Expression<Func<Order, bool>>) (t => t.InstallationNumber == installationNumberValue)) ? 1 : 0;
        else
          num = this._orderRepository.Exists((Expression<Func<Order, bool>>) (t => t.Id != selectedOrder.Id && t.InstallationNumber == installationNumberValue)) ? 1 : 0;
        this._instNoExists = num != 0;
        this._instNumberUniquenessChecked = true;
      }
      if (this._instNoExists)
        validationErrors.Add(Resources.MSS_Client_Structures_InstallationNrExisting);
      return validationErrors.Count == 0;
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

    public IEnumerable<Rules> FilterRules { get; set; }

    public Guid SelectedFilterId
    {
      get => this._selectedFilterId;
      set
      {
        this._selectedFilterId = value;
        this.OnPropertyChanged(nameof (SelectedFilterId));
        this.FilterRules = (IEnumerable<Rules>) this._repositoryFactory.GetRepository<Rules>().SearchFor((Expression<Func<Rules, bool>>) (x => x.Filter.Id == this._selectedFilterId));
        this.OnPropertyChanged("FilterRules");
      }
    }

    public Guid FilterRuleId { get; set; }

    public string OrderDialogTitle
    {
      get => this._orderDialogTitle;
      set
      {
        this._orderDialogTitle = value;
        this.OnPropertyChanged(nameof (OrderDialogTitle));
      }
    }

    public bool IsAddOrderButtonVisible { get; set; }

    public bool IsEditOrderButtonVisible { get; set; }

    public bool IsEditStructureButtonVisible
    {
      get => this._isEditStructureButtonVisible;
      set
      {
        this._isEditStructureButtonVisible = value;
        this.OnPropertyChanged(nameof (IsEditStructureButtonVisible));
      }
    }

    public bool IsAssignStructureButtonVisible { get; set; }

    public bool IsReadingOrder { get; set; }

    public bool IsInstallationOrder { get; set; }

    public bool IsInstallationOrderEdit { get; set; }

    public bool IsPrintButtonVisible { get; set; }

    public string NumberText { get; set; }

    public bool IsReasonVisible
    {
      get => this._isReasonVisible;
      set
      {
        this._isReasonVisible = value;
        this.OnPropertyChanged(nameof (IsReasonVisible));
      }
    }

    [Required(ErrorMessage = "MSS_Client_Order_CreateOrder_InstallationNrValidation")]
    public string InstallationNumberValue
    {
      get => this._installationNumberValue;
      set
      {
        this._installationNumberValue = value;
        this.OnPropertyChanged(nameof (InstallationNumberValue));
      }
    }

    public string DeviceNumberValue
    {
      get => this._deviceNumberValue;
      set
      {
        this._deviceNumberValue = value;
        this.OnPropertyChanged(nameof (DeviceNumberValue));
      }
    }

    public string DetailValue
    {
      get => this._detailValue;
      set => this._detailValue = value;
    }

    public bool ExportedValue
    {
      get => this._exportedValue;
      set => this._exportedValue = value;
    }

    public DateTime? DueDateValue
    {
      get => this._dueDateValue;
      set
      {
        this._dueDateValue = value;
        this.OnPropertyChanged(nameof (DueDateValue));
      }
    }

    public IEnumerable<StatusOrderEnum> StatusCollection
    {
      get
      {
        return (IEnumerable<StatusOrderEnum>) Enum.GetValues(typeof (StatusOrderEnum)).Cast<StatusOrderEnum>().ToList<StatusOrderEnum>();
      }
    }

    public StatusOrderEnum SelectedStatus
    {
      get => this._selectedStatus;
      set
      {
        this._selectedStatus = value;
        this.IsReasonVisible = this._selectedStatus == StatusOrderEnum.Closed;
        this.OnPropertyChanged(nameof (SelectedStatus));
      }
    }

    public IEnumerable<CloseOrderReasonsEnum> ReasonsCollection
    {
      get
      {
        return (IEnumerable<CloseOrderReasonsEnum>) Enum.GetValues(typeof (CloseOrderReasonsEnum)).Cast<CloseOrderReasonsEnum>().ToList<CloseOrderReasonsEnum>();
      }
    }

    public CloseOrderReasonsEnum SelectedReason
    {
      get => this._selectedReason;
      set
      {
        this._selectedReason = value;
        this.OnPropertyChanged(nameof (SelectedReason));
      }
    }

    public Guid RootStructureNodeId
    {
      get => this._rootStructureNodeId;
      set
      {
        this._rootStructureNodeId = value;
        this.OnPropertyChanged(nameof (RootStructureNodeId));
      }
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

    public StructureTypeEnum? StructureType
    {
      get => this._structureType;
      set
      {
        this._structureType = value;
        if (this._orderType == OrderTypeEnum.InstallationOrder)
        {
          StructureTypeEnum? nullable = value;
          if (nullable.HasValue)
          {
            switch (nullable.GetValueOrDefault())
            {
              case StructureTypeEnum.Physical:
                this.EditStructureButtonVisibility = this.GetUsersManagerInstance().HasRight(OperationEnum.PhysicalStructureEdit.ToString());
                break;
              case StructureTypeEnum.Logical:
                this.EditStructureButtonVisibility = this.GetUsersManagerInstance().HasRight(OperationEnum.LogicalStructureEdit.ToString());
                break;
              case StructureTypeEnum.Fixed:
                this.EditStructureButtonVisibility = this.GetUsersManagerInstance().HasRight(OperationEnum.FixedStructureEdit.ToString());
                break;
            }
          }
        }
        else
          this.EditStructureButtonVisibility = true;
        this.OnPropertyChanged(nameof (StructureType));
      }
    }

    private IEnumerable<User> UserCollection()
    {
      IOrderedEnumerable<User> orderedEnumerable = this._userRepository.GetAllUsers().Where<User>((Func<User, bool>) (x => !x.IsDeactivated && !x.Username.StartsWith("default"))).OrderBy<User, string>((Func<User, string>) (u => u.LastName)).ThenBy<User, string>((Func<User, string>) (u => u.FirstName));
      return this.UserList != null ? (IEnumerable<User>) new ObservableCollection<User>(orderedEnumerable.Where<User>((Func<User, bool>) (x => !this.UserList.Select<User, Guid>((Func<User, Guid>) (y => y.Id)).ToList<Guid>().Contains(x.Id)))) : (IEnumerable<User>) new ObservableCollection<User>((IEnumerable<User>) orderedEnumerable);
    }

    public ObservableCollection<User> UsersCollection { get; set; }

    public ViewModelBase MessageUserControl
    {
      get => this._messageUserControl;
      set
      {
        this._messageUserControl = value;
        this.OnPropertyChanged(nameof (MessageUserControl));
      }
    }

    public User SelectedUser
    {
      get => this._selectedUser;
      set
      {
        this._selectedUser = value;
        if (this.SelectedStatus == StatusOrderEnum.Dated || this.SelectedStatus == StatusOrderEnum.New)
          this.SelectedStatus = value != null ? StatusOrderEnum.Dated : StatusOrderEnum.New;
        this.OnPropertyChanged(nameof (SelectedUser));
      }
    }

    public User SelectedUserInListBox
    {
      set
      {
        this.OnPropertyChanged("UserList");
        if (this.SelectedStatus != StatusOrderEnum.Dated && this.SelectedStatus != StatusOrderEnum.New)
          return;
        this.SelectedStatus = this._userList.ToList<User>().Count != 0 ? StatusOrderEnum.Dated : StatusOrderEnum.New;
      }
    }

    public ObservableCollection<User> UserList
    {
      get => this._userList;
      set
      {
        this._userList = value;
        this.OnPropertyChanged(nameof (UserList));
      }
    }

    public bool PrintButtonEnabled
    {
      get => this._printButtonEnabled;
      set
      {
        this._printButtonEnabled = value;
        this.OnPropertyChanged(nameof (PrintButtonEnabled));
      }
    }

    public bool EditStructureButtonVisibility
    {
      get => this._editStructureButtonVisibility;
      set
      {
        this._editStructureButtonVisibility = value;
        this.OnPropertyChanged(nameof (EditStructureButtonVisibility));
      }
    }

    public bool IsUserComboBoxVisible { get; set; }

    public bool IsUserListBoxVisible { get; set; }

    public bool IsUserListBoxEnabled { get; set; }

    public bool IsUserDropdownEnabled { get; set; }

    [Required(ErrorMessage = "MSS_STRUCTURE_REQUIRED")]
    public string StructureInfo
    {
      get => this._structureInfo;
      set
      {
        this._structureInfo = value;
        this.OnPropertyChanged(nameof (StructureInfo));
      }
    }

    public int DueDateYear
    {
      get => this._dueDateYear;
      set
      {
        this._dueDateYear = value;
        this.OnPropertyChanged(nameof (DueDateYear));
      }
    }

    public bool IsYearDropDownVisible { get; set; }

    public bool IsReadingOrderDueDateYearEnabled { get; set; }

    public ICommand AddOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (this.IsValid)
          {
            OrdersManager orderManagerInstance = this.GetOrderManagerInstance();
            List<Guid> guidList = new List<Guid>();
            DateTime dateTime1 = new DateTime();
            if (this._selectedUser != null)
              guidList.Add(this._selectedUser.Id);
            switch (this._orderType)
            {
              case OrderTypeEnum.ReadingOrder:
                DateTime? dueDateValue1 = this.DueDateValue;
                DateTime dateTime2;
                if (!dueDateValue1.HasValue)
                {
                  dateTime2 = DateTime.Now;
                }
                else
                {
                  int dueDateYear = this.DueDateYear;
                  dueDateValue1 = this.DueDateValue;
                  int month = dueDateValue1.Value.Month;
                  dueDateValue1 = this.DueDateValue;
                  int day = dueDateValue1.Value.Day;
                  dateTime2 = new DateTime(dueDateYear, month, day);
                }
                dateTime1 = dateTime2;
                break;
              case OrderTypeEnum.InstallationOrder:
                DateTime? dueDateValue2 = this.DueDateValue;
                DateTime now;
                if (!dueDateValue2.HasValue)
                {
                  now = DateTime.Now;
                }
                else
                {
                  dueDateValue2 = this.DueDateValue;
                  now = dueDateValue2.Value;
                }
                dateTime1 = now;
                break;
            }
            orderManagerInstance.CreateOrder(new OrderDTO()
            {
              InstallationNumber = this._installationNumberValue,
              Details = this._detailValue,
              Exported = this._exportedValue,
              Status = this._selectedStatus,
              DueDate = dateTime1,
              RootStructureNodeId = this._rootStructureNodeId,
              StructureType = this._structureType,
              UserIds = guidList,
              OrderType = this._orderType,
              CloseOrderReason = this._selectedReason,
              FilterId = this.SelectedFilterId,
              StructureBytes = this._structureBytes
            }, new bool?(false));
            this.OnRequestClose(true);
          }
          else
            Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DeleteStructure_Warning_Title, Resources.MSS_Client_FillInStructureField_Warning, false)));
        }));
      }
    }

    private bool? UpdateDueDateStructureValue(out bool isOKPressed)
    {
      isOKPressed = true;
      if (this.RootStructureNodeId != Guid.Empty)
      {
        Location basedOnRootNodeId = this.GetOrderManagerInstance().GetLocationBasedOnRootNodeId(this.RootStructureNodeId);
        DateTime? nullable1;
        int num;
        if (basedOnRootNodeId != null)
        {
          nullable1 = this.DueDateValue;
          int month1 = nullable1.Value.Month;
          nullable1 = basedOnRootNodeId.DueDate;
          int month2 = nullable1.Value.Month;
          if (month1 == month2)
          {
            nullable1 = this.DueDateValue;
            int day1 = nullable1.Value.Day;
            nullable1 = basedOnRootNodeId.DueDate;
            int day2 = nullable1.Value.Day;
            if (day1 == day2)
              goto label_5;
          }
          nullable1 = basedOnRootNodeId.DueDate;
          DateTime minValue = DateTime.MinValue;
          num = nullable1.HasValue ? (nullable1.GetValueOrDefault() > minValue ? 1 : 0) : 0;
          goto label_6;
        }
label_5:
        num = 0;
label_6:
        if (num != 0)
        {
          IRepository<StructureNodeLinks> nodeLinksRepository = this._structureNodeLinksRepository;
          ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof (StructureNodeLinks), "l");
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          BinaryExpression left1 = System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (StructureNodeLinks.get_ParentNodeId))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this, typeof (CreateEditOrderViewModel)), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (CreateEditOrderViewModel.get_RootStructureNodeId))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (StructureNodeLinks.get_RootNode))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (StructureNode.get_Id))), (System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Constant((object) this, typeof (CreateEditOrderViewModel)), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (CreateEditOrderViewModel.get_RootStructureNodeId))), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Guid.op_Equality))));
          // ISSUE: method reference
          MemberExpression left2 = System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (StructureNodeLinks.get_EndDate)));
          nullable1 = new DateTime?();
          ConstantExpression right1 = System.Linq.Expressions.Expression.Constant((object) nullable1, typeof (DateTime?));
          // ISSUE: method reference
          MethodInfo methodFromHandle = (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (DateTime.op_Equality));
          BinaryExpression right2 = System.Linq.Expressions.Expression.Equal((System.Linq.Expressions.Expression) left2, (System.Linq.Expressions.Expression) right1, false, methodFromHandle);
          Expression<Func<StructureNodeLinks, bool>> predicate = System.Linq.Expressions.Expression.Lambda<Func<StructureNodeLinks, bool>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.AndAlso((System.Linq.Expressions.Expression) left1, (System.Linq.Expressions.Expression) right2), parameterExpression);
          if (nodeLinksRepository.SearchFor(predicate).Count <= 0)
            return new bool?(true);
          bool? nullable2 = MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning_UpdateDueDateStructureValue_Title.GetStringValue(), MessageCodes.Warning_UpdateDueDateStructureValue_Message.GetStringValue(), true);
          if (nullable2.HasValue)
            isOKPressed = nullable2.Value;
          return new bool?(isOKPressed);
        }
      }
      return new bool?(false);
    }

    public ICommand EditOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          if (!this.IsValid)
            return;
          OrdersManager orderManagerInstance = this.GetOrderManagerInstance();
          List<Guid> guidList = new List<Guid>();
          DateTime dateTime1 = new DateTime();
          if (this._selectedUser != null)
            guidList.Add(this._selectedUser.Id);
          switch (this._orderType)
          {
            case OrderTypeEnum.ReadingOrder:
              DateTime dateTime2;
              if (!this.DueDateValue.HasValue)
              {
                dateTime2 = DateTime.Now;
              }
              else
              {
                int dueDateYear = this.DueDateYear;
                DateTime? dueDateValue = this.DueDateValue;
                DateTime dateTime3 = dueDateValue.Value;
                int month = dateTime3.Month;
                dueDateValue = this.DueDateValue;
                dateTime3 = dueDateValue.Value;
                int day = dateTime3.Day;
                dateTime2 = new DateTime(dueDateYear, month, day);
              }
              dateTime1 = dateTime2;
              break;
            case OrderTypeEnum.InstallationOrder:
              dateTime1 = this.DueDateValue.HasValue ? this.DueDateValue.Value : DateTime.Now;
              break;
          }
          orderManagerInstance.EditOrder(new OrderDTO()
          {
            Id = this._selectedOrder.Id,
            InstallationNumber = this._installationNumberValue,
            Details = this._detailValue,
            Exported = this._exportedValue,
            Status = this._selectedStatus,
            DueDate = dateTime1,
            RootStructureNodeId = this._rootStructureNodeId,
            StructureType = this._structureType,
            UserIds = guidList,
            OrderType = this._orderType,
            CloseOrderReason = this._selectedReason,
            FilterId = this.SelectedFilterId,
            StructureBytes = this._structureBytes
          }, new bool?(false));
          this.OnRequestClose(true);
        }));
      }
    }

    public ICommand ViewStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<StructureOrdersViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) this._selectedOrder), (IParameter) new ConstructorArgument("orderType", (object) this._selectedOrder.OrderType), (IParameter) new ConstructorArgument("viewMode", (object) true), (IParameter) new ConstructorArgument("selectedRootStructureNodeId", (object) Guid.Empty), (IParameter) new ConstructorArgument("orderDueDate", (object) (this.DueDateValue.HasValue ? new DateTime(this.DueDateYear, this.DueDateValue.Value.Month, this.DueDateValue.Value.Day) : DateTime.Now))))));
      }
    }

    public ICommand FilterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          FilterViewModel filterViewModel = DIConfigurator.GetConfigurator().Get<FilterViewModel>();
          this.FilterCollection = (IEnumerable<MSS.Core.Model.DataFilters.Filter>) this._filterRepository.GetAll().OrderBy<MSS.Core.Model.DataFilters.Filter, string>((Func<MSS.Core.Model.DataFilters.Filter, string>) (f => f.Name));
          this._windowFactory.CreateNewModalDialog((IViewModel) filterViewModel);
        }));
      }
    }

    public ICommand AssignUser
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          User user = parameter as User;
          this.UsersCollection.Remove(user);
          this.UserList.Add(user);
        }));
      }
    }

    public ICommand UnassignUser
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          User user = parameter as User;
          this.UserList.Remove(user);
          this.UsersCollection.Add(user);
        }));
      }
    }

    public ICommand EditStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          OrderDTO selectedOrder = this._selectedOrder;
          OrderTypeEnum orderTypeEnum = selectedOrder != null ? selectedOrder.OrderType : this._orderType;
          bool? nullable = new bool?();
          switch (orderTypeEnum)
          {
            case OrderTypeEnum.ReadingOrder:
              byte[] structureBytes = this.StructureBytes;
              if (structureBytes != null)
              {
                OrderSerializableStructure orderserializablestructure = StructuresHelper.DeserializeStructure(structureBytes);
                Structure structure = this.GetStructureManagerInstance().GetStructure(orderserializablestructure);
                Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
                structure.Locations.ForEach((Action<Location>) (l => entitiesDictionary.Add(l.Id, (object) l)));
                structure.Tenants.ForEach((Action<Tenant>) (t => entitiesDictionary.Add(t.Id, (object) t)));
                structure.Meters.ForEach((Action<Meter>) (m => entitiesDictionary.Add(m.Id, (object) m)));
                structure.Minomats.ForEach((Action<Minomat>) (m => entitiesDictionary.Add(m.Id, (object) m)));
                ObservableCollection<StructureNodeDTO> treeFromList = StructuresHelper.GetTreeFromList(this._structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) structure.Links, entitiesDictionary, meterReplacementHistoryList: (IList<MeterReplacementHistorySerializableDTO>) structure.MeterReplacementHistory);
                if (!this.StructureType.HasValue && structure.Links.Count > 0)
                {
                  StructureNodeLinks structureNodeLinks = structure.Links.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (l => l.ParentNodeId == Guid.Empty && l.RootNode.Id == l.Node.Id && !l.EndDate.HasValue));
                  if (structureNodeLinks != null)
                    this.StructureType = new StructureTypeEnum?(structureNodeLinks.StructureType);
                }
                StructureTypeEnum? structureType = this.StructureType;
                if (structureType.HasValue)
                {
                  switch (structureType.GetValueOrDefault())
                  {
                    case StructureTypeEnum.Physical:
                      StructureNodeDTO structureNodeDto1 = treeFromList.First<StructureNodeDTO>();
                      nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditPhysicalStructureViewModel>((IParameter) new ConstructorArgument("selectedNode", (object) structureNodeDto1), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) false), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) true)));
                      break;
                    case StructureTypeEnum.Logical:
                      StructureNodeDTO structureNodeDto2 = treeFromList.First<StructureNodeDTO>();
                      nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditLogicalStructureViewModel>((IParameter) new ConstructorArgument("selectedNode", (object) structureNodeDto2), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false)));
                      break;
                    case StructureTypeEnum.Fixed:
                      StructureNodeDTO structureNodeDto3 = treeFromList.First<StructureNodeDTO>();
                      nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditFixedStructureViewModel>("EditFixedStructureForOrderViewModel", (IParameter) new ConstructorArgument("selectedNode", (object) structureNodeDto3), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) true), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) false), (IParameter) new ConstructorArgument("orderDTO", (object) this._selectedOrder)));
                      break;
                  }
                }
                break;
              }
              break;
            case OrderTypeEnum.InstallationOrder:
              Guid rootStructureNodeId = this.RootStructureNodeId;
              this._repositoryFactory.GetSession().Clear();
              List<MeterReplacementHistorySerializableDTO> historySerializableDto = StructuresHelper.GetMeterReplacementHistorySerializableDTO(this._meterReplacementHistoryRepository.GetAll());
              IEnumerable<StructureNodeDTO> nodeDtoForRootNode = OrdersHelper.GetStructureNodeDTOForRootNode(rootStructureNodeId, this._structureNodeLinksRepository, this._structureNodeRepository, this._structureNodeTypeRepository, this._nhSession, (IList<MeterReplacementHistorySerializableDTO>) historySerializableDto);
              StructureTypeEnum? structureType1 = this.StructureType;
              if (structureType1.HasValue)
              {
                switch (structureType1.GetValueOrDefault())
                {
                  case StructureTypeEnum.Physical:
                    StructureNodeDTO structureNodeDto4 = nodeDtoForRootNode.First<StructureNodeDTO>();
                    nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditPhysicalStructureViewModel>((IParameter) new ConstructorArgument("selectedNode", (object) structureNodeDto4), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) false), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false)));
                    break;
                  case StructureTypeEnum.Fixed:
                    StructureNodeDTO structureNodeDto5 = nodeDtoForRootNode.First<StructureNodeDTO>();
                    nullable = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<EditFixedStructureViewModel>("EditFixedStructureForOrderViewModel", (IParameter) new ConstructorArgument("selectedNode", (object) structureNodeDto5), (IParameter) new ConstructorArgument("updatedForReadingOrder", (object) false), (IParameter) new ConstructorArgument("isExecuteInstallation", (object) false), (IParameter) new ConstructorArgument("orderDTO", (object) this._selectedOrder)));
                    break;
                }
              }
              else
                break;
              break;
          }
          if (nullable.HasValue && nullable.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_SuccessMessage);
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
        }));
      }
    }

    public ICommand AddEditStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<StructureOrdersViewModel>((IParameter) new ConstructorArgument("selectedOrder", (object) this._selectedOrder), (IParameter) new ConstructorArgument("orderType", (object) this._orderType), (IParameter) new ConstructorArgument("viewMode", (object) false), (IParameter) new ConstructorArgument("selectedRootStructureNodeId", (object) this._rootStructureNodeId), (IParameter) new ConstructorArgument("orderDueDate", (object) (this.DueDateValue.HasValue ? new DateTime(this.DueDateYear, this.DueDateValue.Value.Month, this.DueDateValue.Value.Day) : DateTime.Now))));
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            return;
          this.IsEditStructureButtonVisible = true;
        }));
      }
    }
  }
}
