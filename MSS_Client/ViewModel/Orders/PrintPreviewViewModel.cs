// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Orders.PrintPreviewViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Orders;
using MSS.Core.Model.Structures;
using MSS.Core.Model.UsersManagement;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MVVM.Commands;
using MVVM.ViewModel;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MSS_Client.ViewModel.Orders
{
  public class PrintPreviewViewModel : ViewModelBase
  {
    private readonly IRepository<StructureNode> _structureNodeRepository;
    private readonly IRepository<StructureNodeType> _structureNodeTypeRepository;
    private readonly IRepository<StructureNodeLinks> _structureNodeLinksRepository;
    private readonly IUserRepository _userRepository;
    private readonly OrderDTO _selectedOrder;
    private readonly IRepositoryFactory _repositoryFactory;
    private bool _isReasonVisible;
    private string _installationNumberValue;
    private string _deviceNumberValue;
    private string _detailValue;
    private bool _exportedValue;
    private DateTime _dueDateValue;
    private StatusOrderEnum _selectedStatus;
    private CloseOrderReasonsEnum _selectedReason;
    private Guid _rootStructureNodeId;
    private User _selectedUser;
    private string _selectedUserName;
    private IEnumerable<StructureNodeDTO> _structureNodeCollection;

    [Inject]
    public PrintPreviewViewModel(OrderDTO selectedOrder, IRepositoryFactory repositoryFactory)
    {
      this._repositoryFactory = repositoryFactory;
      this._structureNodeRepository = repositoryFactory.GetRepository<StructureNode>();
      this._structureNodeTypeRepository = repositoryFactory.GetRepository<StructureNodeType>();
      this._structureNodeLinksRepository = repositoryFactory.GetRepository<StructureNodeLinks>();
      this._selectedOrder = selectedOrder;
      this._userRepository = repositoryFactory.GetUserRepository();
      this.StructureNodeCollection = this.GetStructureNodeForOrder(this._selectedOrder.OrderType);
      this.InstallationNumberValue = selectedOrder.InstallationNumber;
      this.DetailValue = selectedOrder.Details;
      this.ExportedValue = selectedOrder.Exported;
      this.SelectedStatus = selectedOrder.Status;
      this.DueDateValue = selectedOrder.DueDate;
      List<User> list = selectedOrder.UserIds.Select<Guid, User>((Func<Guid, User>) (userID => this._userRepository.GetById((object) userID))).ToList<User>();
      if (this._selectedOrder.OrderType == OrderTypeEnum.ReadingOrder)
      {
        this.SelectedUser = list.FirstOrDefault<User>();
      }
      else
      {
        foreach (User user in list)
          this.SelectedUserName = this.SelectedUserName + user.FirstName + " " + user.LastName + "\n";
      }
      this.DeviceNumberValue = selectedOrder.DeviceNumber;
      this.SelectedReason = selectedOrder.CloseOrderReason;
    }

    private StructuresManager GetStructuresManagerInstance()
    {
      return new StructuresManager(this._repositoryFactory);
    }

    private StructuresManager GetStructuresManagerInstance(bool isConstructorWithoutMappings)
    {
      return new StructuresManager(this._repositoryFactory, isConstructorWithoutMappings);
    }

    public bool IsReasonVisible
    {
      get => this._isReasonVisible;
      set
      {
        this._isReasonVisible = value;
        this.OnPropertyChanged(nameof (IsReasonVisible));
      }
    }

    public string InstallationNumberValue
    {
      get => this._installationNumberValue;
      set => this._installationNumberValue = value;
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

    public DateTime DueDateValue
    {
      get => this._dueDateValue;
      set => this._dueDateValue = value;
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

    public IEnumerable<User> UsersCollection
    {
      get => (IEnumerable<User>) this._userRepository.GetAllUsers();
    }

    public User SelectedUser
    {
      get => this._selectedUser;
      set
      {
        this._selectedUser = value;
        if (value != null)
        {
          this.SelectedStatus = StatusOrderEnum.Dated;
          this.SelectedUserName = this._selectedUser.FirstName + " " + this._selectedUser.LastName;
        }
        this.OnPropertyChanged(nameof (SelectedUser));
      }
    }

    public string SelectedUserName
    {
      get => this._selectedUserName;
      set
      {
        this._selectedUserName = value;
        this.OnPropertyChanged(nameof (SelectedUserName));
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

    public string OrderNumberLabel
    {
      get => this.GetOrderNumberLabel();
      set
      {
        this._selectedUserName = value;
        this.OnPropertyChanged(nameof (OrderNumberLabel));
      }
    }

    public ICommand PrintOrderCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          try
          {
            Grid grid = parameter as Grid;
            new UIPrinter()
            {
              Title = ("Order " + this.InstallationNumberValue),
              Content = grid
            }.Print();
            this.OnRequestClose(false);
          }
          catch (PrintAborted ex)
          {
            string message = ex.Message;
          }
        }));
      }
    }

    private IEnumerable<StructureNodeDTO> GetStructureNodeForOrder(OrderTypeEnum orderType)
    {
      ObservableCollection<StructureNodeDTO> structureNodeForOrder = new ObservableCollection<StructureNodeDTO>();
      switch (orderType)
      {
        case OrderTypeEnum.ReadingOrder:
          if (this._selectedOrder.StructureBytes != null)
          {
            OrderSerializableStructure orderserializablestructure = StructuresHelper.DeserializeStructure(this._selectedOrder.StructureBytes);
            Structure structure = this.GetStructuresManagerInstance().GetStructure(orderserializablestructure);
            Dictionary<Guid, object> entityDictionary = new Dictionary<Guid, object>();
            structure.Locations.ForEach((Action<Location>) (l => entityDictionary.Add(l.Id, (object) l)));
            structure.Tenants.ForEach((Action<Tenant>) (t => entityDictionary.Add(t.Id, (object) t)));
            structure.Meters.ForEach((Action<Meter>) (m => entityDictionary.Add(m.Id, (object) m)));
            structure.Minomats.ForEach((Action<Minomat>) (m => entityDictionary.Add(m.Id, (object) m)));
            structureNodeForOrder = StructuresHelper.GetTreeFromList(this._structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) structure.Links, entityDictionary);
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
            structureNodeForOrder = StructuresHelper.GetTreeFromList(this._structureNodeTypeRepository.GetAll(), (IList<StructureNodeLinks>) list, entitiesDictionary);
            break;
          }
          break;
      }
      return (IEnumerable<StructureNodeDTO>) structureNodeForOrder;
    }

    private List<Guid> GetNodeIdList(IEnumerable<StructureNodeLinks> structureNodeLinks)
    {
      List<Guid> nodeIDs = new List<Guid>();
      foreach (StructureNodeLinks structureNodeLinks1 in structureNodeLinks.Where<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (structureNode => !nodeIDs.Contains(structureNode.Node.Id))))
        nodeIDs.Add(structureNodeLinks1.Node.Id);
      return nodeIDs;
    }

    private Dictionary<Guid, object> GetEntitiesDictionary(IList<StructureNode> structureNodeList)
    {
      Dictionary<Guid, object> entitiesDictionary = new Dictionary<Guid, object>();
      foreach (StructureNode structureNode in (IEnumerable<StructureNode>) structureNodeList)
      {
        if (structureNode.EntityId != Guid.Empty)
        {
          StructureNodeTypeEnum structureNodeTypeName = (StructureNodeTypeEnum) Enum.Parse(typeof (StructureNodeTypeEnum), structureNode.EntityName, true);
          object entity = this.GetStructuresManagerInstance(true).GetEntity(structureNodeTypeName, structureNode);
          entitiesDictionary.Add(structureNode.EntityId, entity);
        }
      }
      return entitiesDictionary;
    }

    private string GetOrderNumberLabel()
    {
      if (this._selectedOrder.OrderType == OrderTypeEnum.ReadingOrder)
        return CultureResources.GetValue("MSS_Client_OrderControl_Header_ReadingNumber");
      return this._selectedOrder.OrderType == OrderTypeEnum.InstallationOrder ? CultureResources.GetValue("MSS_Client_OrderControl_Header_InstallationNumber") : "";
    }
  }
}
