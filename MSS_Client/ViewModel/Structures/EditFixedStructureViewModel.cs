// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.EditFixedStructureViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.GMMWrapper;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.DataCollectors;
using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Orders;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Settings;
using MVVM.Commands;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Data;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class EditFixedStructureViewModel : StructureViewModelBase
  {
    private readonly StructureNodeDTO _parentForSelectedNode;
    private readonly StructureNodeDTO _selectedNode;
    private readonly bool _updatedForReadingOrder;
    private readonly ObservableCollection<StructureNodeDTO> _nodesCollection = new ObservableCollection<StructureNodeDTO>();
    private readonly List<StructureNodeLinks> _fixedStructureNodeLinksToDelete = new List<StructureNodeLinks>();
    private readonly List<StructureNode> _structureNodesToDelete = new List<StructureNode>();
    private readonly ObservableCollection<StructureNodeDTO> _fixedNodesDTOToDelete = new ObservableCollection<StructureNodeDTO>();
    private List<StructureNodeDTO> _newNodes = new List<StructureNodeDTO>();
    private byte[] _structureBytes;
    private ScanMinoConnectManager _scannerMinoConnectManager;
    private bool _isEditStructureNode;
    private int _numberOfReadMeters;
    private int _numberOfMetersInStructure;
    private List<StructureNodeDTO> _metersInStructure = new List<StructureNodeDTO>();
    private WalkByTestManager walkByTestManager;
    private readonly IDeviceManager _deviceManager;
    private string _progressDialogMessage;
    private int _progressBarValue;
    private bool _isTenantSelected;
    private StructureNodeDTO _selectedStructureNode;
    private StructureNodeDTO _selectedTenantStructureNode;
    private StructureNodeDTO _selectedMeterStructureNode;
    private bool _isPasteActive;
    private StructureNodeDTO _savedInClipBoardStructureNodeDto;
    private bool _isBusy;

    [Inject]
    public EditFixedStructureViewModel(
      StructureNodeDTO selectedNode,
      bool updatedForReadingOrder,
      bool isExecuteInstallation,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      this._selectedNode = selectedNode;
      this._updatedForReadingOrder = updatedForReadingOrder;
      this._nodesCollection.Add(this._selectedNode);
      this._parentForSelectedNode = selectedNode.ParentNode;
      this._deviceManager = (IDeviceManager) new DeviceManagerWrapper("DefaultScanner");
      this.Title = isExecuteInstallation ? Resources.MSS_Client_Execute_Installation_Order : Resources.MSS_Client_Structures_Edit_Fixed_Structure;
      this.UpdateSerialNumberListForReadingOrder(updatedForReadingOrder);
      this.AvailableNodesGroups = new ObservableCollection<MSS.Business.Modules.StructuresManagement.Group>();
      MSS.Business.Modules.StructuresManagement.Group group = new MSS.Business.Modules.StructuresManagement.Group()
      {
        Name = CultureResources.GetValue("MSS_Available_Nodes_Folder")
      };
      StructuresHelper.LoadItemsInGroup(group, this.AvailableNodesSettingsGroup);
      this.AvailableNodesGroups.Add(group);
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>();
      Mapper.CreateMap<MSS.Core.Model.Meters.Meter, MeterDTO>();
      Mapper.CreateMap<MSS.Core.Model.Structures.Location, LocationDTO>();
      Mapper.CreateMap<Tenant, TenantDTO>();
      Mapper.CreateMap<Minomat, MinomatSerializableDTO>().ForMember((Expression<Func<MinomatSerializableDTO, object>>) (x => (object) x.ProviderId), (Action<IMemberConfigurationExpression<Minomat>>) (x => x.ResolveUsing((Func<Minomat, object>) (y => y.Provider != null ? (object) y.Provider.Id : (object) Guid.Empty))));
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(((StructureViewModelBase) this).UpdateEntities));
      EventPublisher.Register<ReplaceDeviceEvent>(new Action<ReplaceDeviceEvent>(((StructureViewModelBase) this).ReplaceDevice));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(((StructureViewModelBase) this).ShowActionSyncFinished));
      this.StructureEquipmentSettings = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().FirstOrDefault((Expression<Func<StructureNodeEquipmentSettings, bool>>) (e => e.StructureNode.Id == selectedNode.RootNode.Id)) ?? new StructureNodeEquipmentSettings();
      this.DragDropAttachedProp = new DragDropAttachedObject()
      {
        IsEnabled = true,
        PhysicalLinks = new List<StructureNodeLinks>()
      };
      EventPublisher.Register<ItemDropped>(new Action<ItemDropped>(this.OnItemDropped));
      foreach (StructureNodeDTO root in (Collection<StructureNodeDTO>) this.StructureForSelectedNode)
        this.CalculateNoOfDevicesForAllTenants(root);
      this.ResetStartStopButtons();
      LocationDTO entity = this.StructureForSelectedNode[0].Entity as LocationDTO;
      if (!CustomerConfiguration.GetPropertyValue<bool>("IsTabletMode") || entity == null || entity.Generation != GenerationEnum.Radio3)
        return;
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(this.UpdateStructure));
    }

    [Inject]
    public EditFixedStructureViewModel(
      StructureNodeDTO selectedNode,
      bool updatedForReadingOrder,
      bool isExecuteInstallation,
      OrderDTO orderDTO,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      this._selectedNode = selectedNode;
      this._structureBytes = orderDTO?.StructureBytes;
      this._updatedForReadingOrder = updatedForReadingOrder;
      this._nodesCollection.Add(this._selectedNode);
      this._parentForSelectedNode = selectedNode.ParentNode;
      this.Title = isExecuteInstallation ? Resources.MSS_Client_Execute_Installation_Order : Resources.MSS_Client_Structures_Edit_Fixed_Structure;
      this.UpdateSerialNumberListForReadingOrder(updatedForReadingOrder);
      this.AvailableNodesGroups = new ObservableCollection<MSS.Business.Modules.StructuresManagement.Group>();
      MSS.Business.Modules.StructuresManagement.Group group = new MSS.Business.Modules.StructuresManagement.Group()
      {
        Name = CultureResources.GetValue("MSS_Available_Nodes_Folder")
      };
      StructuresHelper.LoadItemsInGroup(group, this.AvailableNodesSettingsGroup);
      this.AvailableNodesGroups.Add(group);
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>();
      Mapper.CreateMap<MSS.Core.Model.Meters.Meter, MeterDTO>();
      Mapper.CreateMap<MSS.Core.Model.Structures.Location, LocationDTO>();
      Mapper.CreateMap<Tenant, TenantDTO>();
      Mapper.CreateMap<Minomat, MinomatSerializableDTO>().ForMember((Expression<Func<MinomatSerializableDTO, object>>) (x => (object) x.ProviderId), (Action<IMemberConfigurationExpression<Minomat>>) (x => x.ResolveUsing((Func<Minomat, object>) (y => y.Provider != null ? (object) y.Provider.Id : (object) Guid.Empty))));
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(((StructureViewModelBase) this).UpdateEntities));
      EventPublisher.Register<ReplaceDeviceEvent>(new Action<ReplaceDeviceEvent>(((StructureViewModelBase) this).ReplaceDevice));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(((StructureViewModelBase) this).ShowActionSyncFinished));
      this.DragDropAttachedProp = new DragDropAttachedObject()
      {
        IsEnabled = true,
        PhysicalLinks = new List<StructureNodeLinks>()
      };
      EventPublisher.Register<ItemDropped>(new Action<ItemDropped>(this.OnItemDropped));
      foreach (StructureNodeDTO root in (Collection<StructureNodeDTO>) this.StructureForSelectedNode)
        this.CalculateNoOfDevicesForAllTenants(root);
      this.ResetStartStopButtons();
    }

    public DragDropAttachedObject DragDropAttachedProp { get; set; }

    public ObservableCollection<MSS.Business.Modules.StructuresManagement.Group> AvailableNodesGroups { get; }

    public ObservableCollection<StructureNodeDTO> AvailableNodesSettingsGroup
    {
      get
      {
        List<StructureNodeDTO> source = new List<StructureNodeDTO>();
        IRepository<StructureNodeType> nodeTypeRepository = this._structureNodeTypeRepository;
        Expression<Func<StructureNodeType, bool>> predicate = (Expression<Func<StructureNodeType, bool>>) (s => s.IsFixed);
        foreach (StructureNodeType nodeType in (IEnumerable<StructureNodeType>) nodeTypeRepository.SearchFor(predicate).OrderBy<StructureNodeType, string>((Func<StructureNodeType, string>) (n => n.Name)))
        {
          if (!nodeType.Name.Contains("Minomat"))
          {
            StructureNodeDTO structureNodeDto = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_StructureNode_" + nodeType.Name), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Fixed);
            source.Add(structureNodeDto);
          }
        }
        return new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) source.OrderBy<StructureNodeDTO, string>((Func<StructureNodeDTO, string>) (n => n.Name)));
      }
    }

    public ObservableCollection<StructureNodeDTO> StructureForSelectedNode
    {
      get
      {
        StructureImageHelper.SetImageIconPath(this._nodesCollection);
        return this._nodesCollection;
      }
    }

    public string Title { get; set; }

    public string ProgressDialogMessage
    {
      get => this._progressDialogMessage;
      set
      {
        this._progressDialogMessage = value;
        this.OnPropertyChanged(nameof (ProgressDialogMessage));
      }
    }

    public int ProgressBarValue
    {
      get => this._progressBarValue;
      set
      {
        this._progressBarValue = value;
        this.OnPropertyChanged(nameof (ProgressBarValue));
      }
    }

    public bool IsTenantSelected
    {
      get => this._isTenantSelected;
      set
      {
        this._isTenantSelected = value;
        this.OnPropertyChanged(nameof (IsTenantSelected));
      }
    }

    public StructureNodeDTO SelectedStructureNode
    {
      get => this._selectedStructureNode;
      set
      {
        this._selectedStructureNode = value;
        this.IsTenantSelected = value != null && value.NodeType.Name == "Tenant";
        this.OnPropertyChanged(nameof (SelectedStructureNode));
      }
    }

    public StructureNodeDTO SelectedTenantStructureNode
    {
      get => this._selectedTenantStructureNode;
      set
      {
        if (this.SelectedMeterStructureNode != null)
          this.SelectedMeterStructureNode = (StructureNodeDTO) null;
        this._selectedTenantStructureNode = value;
        this.SelectedStructureNode = value;
        this.OnPropertyChanged(nameof (SelectedTenantStructureNode));
      }
    }

    public StructureNodeDTO SelectedMeterStructureNode
    {
      get => this._selectedMeterStructureNode;
      set
      {
        if (this._selectedMeterStructureNode != null)
        {
          this._selectedMeterStructureNode = (StructureNodeDTO) null;
          this.OnPropertyChanged(nameof (SelectedMeterStructureNode));
        }
        if (this._selectedTenantStructureNode != null)
          this.SelectedTenantStructureNode = (StructureNodeDTO) null;
        this._selectedMeterStructureNode = value;
        this.SelectedStructureNode = value;
        this.OnPropertyChanged(nameof (SelectedMeterStructureNode));
      }
    }

    public bool IsPasteActive
    {
      get => this._isPasteActive;
      set
      {
        this._isPasteActive = value;
        this.OnPropertyChanged(nameof (IsPasteActive));
      }
    }

    public StructureNodeDTO SavedinClipStructureNodeDto
    {
      get => this._savedInClipBoardStructureNodeDto;
      set
      {
        this._savedInClipBoardStructureNodeDto = value;
        this.IsPasteActive = value != null;
      }
    }

    public ICommand AddTenantCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          StructureNodeType nodeType = this._repositoryFactory.GetRepository<StructureNodeType>().FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (x => x.Name == StructureNodeTypeEnum.Tenant.ToString()));
          StructureNodeDTO node = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_StructureNode_" + nodeType.Name), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Fixed)
          {
            ParentNode = this._nodesCollection[0],
            RootNode = this._nodesCollection[0]
          };
          this._nodesCollection[0].SubNodes.Add(node);
          bool? nullable = this.EditSelectedStructureNode(node);
          if (!nullable.HasValue || !nullable.Value)
            this._nodesCollection[0].SubNodes.Remove(node);
          this.OnPropertyChanged("StructureForSelectedNode");
        }));
      }
    }

    public ICommand AddMeterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.SelectedStructureNode == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_ADD_METER);
          }
          else
          {
            StructureNodeType nodeType = this._repositoryFactory.GetRepository<StructureNodeType>().FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (x => x.Name == StructureNodeTypeEnum.Meter.ToString()));
            StructureNodeDTO node = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_StructureNode_" + nodeType.Name), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Fixed)
            {
              ParentNode = this.SelectedStructureNode,
              RootNode = this.StructureForSelectedNode.ElementAt<StructureNodeDTO>(0)
            };
            this.SelectedStructureNode.SubNodes.Insert(0, node);
            this.EditSelectedStructureNode(node);
            StructureNodeDTO tenantParent = this.GetTenantParent(node);
            StructureNodeDTO structureNodeDto = tenantParent.SubNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter" && item.Id == Guid.Empty));
            if (structureNodeDto != null)
              tenantParent.SubNodes.Remove(structureNodeDto);
            if (tenantParent == null)
              return;
            (tenantParent.Entity as TenantDTO).NoOfDevices = new int?(tenantParent.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
          }
        }));
      }
    }

    public ICommand EditTenantOrMeterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          this._isEditStructureNode = true;
          if (this.SelectedStructureNode == null)
          {
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_EXECUTE_INSTALLATION_ORDER_INVALID_OPERATION);
          }
          else
          {
            this.EditSelectedStructureNode(this.SelectedStructureNode);
            this._isEditStructureNode = false;
          }
        }));
      }
    }

    public ICommand DeleteTenantOrMeterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO structureNodeDto = (StructureNodeDTO) null;
          if (this.SelectedStructureNode.NodeType.Name == "Meter")
            structureNodeDto = this.GetTenantParent(this.SelectedStructureNode);
          this.RemoveSerialNumberFromUniquenessList(this.SelectedStructureNode);
          this.RemoveSelectedNodeFromStructure(this.SelectedStructureNode, this._nodesCollection);
          if (structureNodeDto != null)
            (structureNodeDto.Entity as TenantDTO).NoOfDevices = new int?(structureNodeDto.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
          this.OnPropertyChanged("StructureForSelectedNode");
        }));
      }
    }

    public ICommand EditLocationCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this._isEditStructureNode = true;
          IKernel configurator = DIConfigurator.GetConfigurator();
          IParameter[] parameterArray = new IParameter[3];
          Guid? id = this._nodesCollection[0]?.Id;
          Guid empty = Guid.Empty;
          parameterArray[0] = (IParameter) new ConstructorArgument("isExistingEntity", (object) (bool) (id.HasValue ? (id.HasValue ? (id.GetValueOrDefault() != empty ? 1 : 0) : 0) : 1));
          parameterArray[1] = (IParameter) new ConstructorArgument("node", (object) this._nodesCollection[0]);
          parameterArray[2] = (IParameter) new ConstructorArgument("locationNumberList", (object) this.locationNumberList);
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<CreateEditLocationViewModel>(parameterArray));
          if (newModalDialog.HasValue && newModalDialog.Value)
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_OperationCancelledMessage);
          this._isEditStructureNode = false;
        }));
      }
    }

    public ICommand CutToolbarSelectedItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          this.SavedinClipStructureNodeDto = (StructureNodeDTO) radTreeListView.SelectedItem;
          if (this.SavedinClipStructureNodeDto.ParentNode == null || this.SavedinClipStructureNodeDto.ParentNode != null && this.SavedinClipStructureNodeDto.ParentNode.Name == CultureResources.GetValue("MSS_Available_Nodes_Folder"))
          {
            ((Collection<StructureNodeDTO>) radTreeListView.ItemsSource).Remove(this.SavedinClipStructureNodeDto);
            if (((Collection<StructureNodeDTO>) radTreeListView.ItemsSource).Count != 0)
              return;
            ((Collection<StructureNodeDTO>) radTreeListView.ItemsSource).Add(this.SavedinClipStructureNodeDto);
            this.SavedinClipStructureNodeDto = (StructureNodeDTO) null;
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_ROOTNODE_CANNOT_BE_CUTTED);
          }
          else
          {
            StructureNodeDTO meterNode = (StructureNodeDTO) null;
            if (this.SavedinClipStructureNodeDto.NodeType.Name == "Meter")
              meterNode = this.GetTenantParent(this.SavedinClipStructureNodeDto);
            this.SavedinClipStructureNodeDto.ParentNode.SubNodes.Remove(this.SavedinClipStructureNodeDto);
            this.CalculateNoOfDevicesForTenantParent(meterNode);
          }
        }));
      }
    }

    public ICommand PasteToolbarSelectedItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          StructureNodeDTO selectedStructureNode = (StructureNodeDTO) radTreeListView.SelectedItem;
          radTreeListView.SelectedItem = (object) null;
          FixedStructureNodesValidator structureNodesValidator = new FixedStructureNodesValidator();
          if (this.SavedinClipStructureNodeDto == null)
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_NOTHING_TO_PASTE);
          else if (structureNodesValidator.IsValidNodesRelationship(this.SavedinClipStructureNodeDto, selectedStructureNode, false))
          {
            if (selectedStructureNode != null && selectedStructureNode.SubNodes != null)
            {
              this.SavedinClipStructureNodeDto.ParentNode = selectedStructureNode;
              StructuresHelper.Descendants(this.SavedinClipStructureNodeDto).ToList<StructureNodeDTO>().ForEach((Action<StructureNodeDTO>) (x => x.RootNode = selectedStructureNode.RootNode));
              selectedStructureNode.SubNodes.Add(this.SavedinClipStructureNodeDto);
            }
            else if (selectedStructureNode == null)
            {
              this.SavedinClipStructureNodeDto.ParentNode = this.SavedinClipStructureNodeDto;
              StructuresHelper.Descendants(this.SavedinClipStructureNodeDto).ToList<StructureNodeDTO>().ForEach((Action<StructureNodeDTO>) (x => x.RootNode = this.SavedinClipStructureNodeDto));
              ((Collection<StructureNodeDTO>) radTreeListView.ItemsSource).Add(this.SavedinClipStructureNodeDto);
            }
            this.SavedinClipStructureNodeDto = (StructureNodeDTO) null;
            if (selectedStructureNode.NodeType.Name == "Tenant")
              (selectedStructureNode.Entity as TenantDTO).NoOfDevices = new int?(selectedStructureNode.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CUT_PASTE_ERROR);
        }));
      }
    }

    public ICommand PasteAfterToolbarSelectedItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          StructureNodeDTO selectedItem = (StructureNodeDTO) radTreeListView.SelectedItem;
          StructureNodeDTO parentNode = selectedItem.ParentNode;
          radTreeListView.SelectedItem = (object) null;
          FixedStructureNodesValidator structureNodesValidator = new FixedStructureNodesValidator();
          if (this.SavedinClipStructureNodeDto == null)
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_NOTHING_TO_PASTE);
          else if (structureNodesValidator.IsValidNodesRelationship(this.SavedinClipStructureNodeDto, parentNode, false))
          {
            if (parentNode != null && parentNode.SubNodes != null)
            {
              this.SavedinClipStructureNodeDto.ParentNode = parentNode;
              StructuresHelper.Descendants(this.SavedinClipStructureNodeDto).ToList<StructureNodeDTO>().ForEach((Action<StructureNodeDTO>) (x => x.RootNode = parentNode.RootNode));
              int num = parentNode.SubNodes.IndexOf(selectedItem);
              this.SavedinClipStructureNodeDto.ParentNode = parentNode;
              parentNode.SubNodes.Insert(num + 1, this.SavedinClipStructureNodeDto);
            }
            else if (parentNode == null)
            {
              this.SavedinClipStructureNodeDto.ParentNode = this.SavedinClipStructureNodeDto;
              StructuresHelper.Descendants(this.SavedinClipStructureNodeDto).ToList<StructureNodeDTO>().ForEach((Action<StructureNodeDTO>) (x => x.RootNode = this.SavedinClipStructureNodeDto));
              this.SavedinClipStructureNodeDto.RootNode = this.SavedinClipStructureNodeDto;
              this.SavedinClipStructureNodeDto.ParentNode = (StructureNodeDTO) null;
              ((Collection<StructureNodeDTO>) radTreeListView.ItemsSource).Add(this.SavedinClipStructureNodeDto);
            }
            this.SavedinClipStructureNodeDto = (StructureNodeDTO) null;
            this.CalculateNoOfDevicesForAllTenants(selectedItem.RootNode);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CUT_PASTE_ERROR);
        }));
      }
    }

    public ICommand AddNodeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO structureNodeDto1 = parameter as StructureNodeDTO;
          if (new FixedStructureNodesValidator().IsValidNodesRelationship(structureNodeDto1, this.SelectedItem, false))
          {
            StructureNodeDTO structureNodeDto2 = new StructureNodeDTO();
            Mapper.Map<StructureNodeDTO, StructureNodeDTO>(structureNodeDto1, structureNodeDto2);
            if (structureNodeDto2.NodeType.Name == "Tenant")
              structureNodeDto2.Entity = (object) new TenantDTO();
            this.SelectedItem.SubNodes.Add(structureNodeDto2);
            if (structureNodeDto2.NodeType.Name == "Meter")
              this.CalculateNoOfDevicesForTenantParent(structureNodeDto2);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
          this.OnPropertyChanged("StructureForSelectedNode");
        }));
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

    public ICommand SaveFixedStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this._nodesCollection.SetNodesOrderNumber();
          List<StructureNodeDTO> nodesList = new List<StructureNodeDTO>();
          this.GetNodesList(this._nodesCollection, ref nodesList);
          this.GetStructuresManagerInstance().TransactionalUpdateStructure((IList<StructureNodeDTO>) nodesList, StructureTypeEnum.Fixed, (StructureNodeEquipmentSettings) null);
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
          message.MessageType = MessageTypeEnum.Success;
          message.MessageText = MessageCodes.Success_Save.GetStringValue();
          foreach (StructureNodeDTO structureNodeDto in nodesList)
          {
            Guid entityId;
            StructureNodeTypeEnum entityType;
            StructuresHelper.GetEntityIdAndEntityType(structureNodeDto, out entityId, out entityType);
            EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
            {
              Guid = structureNodeDto.Id,
              EntityId = entityId,
              EntityType = entityType,
              Message = message
            }, (IViewModel) this);
          }
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand CancelWindowAndDeleteNewNodesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this._newNodes.Any<StructureNodeDTO>())
          {
            List<Guid> structureNodeIds = this._newNodes.Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (item => item.Id)).ToList<Guid>();
            List<StructureNode> list1 = this._structureNodeRepository.Where((Expression<Func<StructureNode, bool>>) (item => structureNodeIds.Contains(item.Id))).ToList<StructureNode>();
            List<StructureNodeLinks> list2 = this._structureNodeLinkRepository.Where((Expression<Func<StructureNodeLinks, bool>>) (item => structureNodeIds.Contains(item.Node.Id))).ToList<StructureNodeLinks>();
            ISession session = this._repositoryFactory.GetSession();
            session.BeginTransaction();
            this.GetStructuresManagerInstance().TransactionalDeleteAffectedStructureNodes((IList<StructureNodeLinks>) list2, (IList<StructureNode>) list1);
            session.Transaction.Commit();
          }
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.OperationCancelled.GetStringValue()
          };
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand SaveStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (async parameter =>
        {
          if (!(parameter is RadTreeListView radTreeListView2))
            return;
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
          bool isDeleted = true;
          radTreeListView2.ExpandAllHierarchyItems();
          DataItemCollection nodes = radTreeListView2.Items;
          ObservableCollection<StructureNodeDTO> nodeCollection = new ObservableCollection<StructureNodeDTO>(nodes.Cast<StructureNodeDTO>());
          bool continueSave = this.ReconstructCollectionWithoutInvalidMBusScannerMeters(nodeCollection);
          if (this.ContinueActionIfMBusIsStarted(this._scannerMinoConnectManager) && continueSave)
          {
            this.IsBusy = true;
            isDeleted = false;
            await Task.Run((Func<Task>) (async () =>
            {
              nodeCollection.SetNodesOrderNumber(this._selectedNode, this._parentForSelectedNode);
              ObservableCollection<StructureNodeDTO> actualNodeCollection = this.ReconstructNodeCollection(nodeCollection, this._parentForSelectedNode);
              if (this._fixedStructureNodeLinksToDelete.Count != 0 || this._structureNodesToDelete.Count != 0)
              {
                bool? isOkButton;
                await Task.Run((Func<Task>) (async () => await Application.Current.Dispatcher.InvokeAsync((Action) (() =>
                {
                  isOkButton = this.ShowWarningWithStructuresToDeleteDialog(this._fixedNodesDTOToDelete, new ObservableCollection<StructureNodeDTO>());
                  if (isOkButton.HasValue && isOkButton.Value)
                  {
                    if (!this._updatedForReadingOrder)
                      this.GetStructuresManagerInstance().TransactionalUpdateStructure((IList<StructureNodeDTO>) actualNodeCollection, StructureTypeEnum.Fixed, this.StructureEquipmentSettings, (IList<StructureNodeLinks>) this._fixedStructureNodeLinksToDelete, (IList<StructureNode>) this._structureNodesToDelete);
                    else
                      this._structureBytes = this.GetStructuresManagerInstance().CreateStructureBytes(actualNodeCollection, this._structureBytes);
                  }
                  else
                  {
                    message.MessageType = MessageTypeEnum.Warning;
                    message.MessageText = MessageCodes.OperationCancelled.GetStringValue();
                    isDeleted = true;
                  }
                }))));
              }
              else
              {
                if (!this._updatedForReadingOrder)
                  this.GetStructuresManagerInstance().TransactionalUpdateStructure((IList<StructureNodeDTO>) actualNodeCollection, StructureTypeEnum.Fixed, this.StructureEquipmentSettings);
                else
                  this._structureBytes = this.GetStructuresManagerInstance().CreateStructureBytes(actualNodeCollection, this._structureBytes);
                message.MessageType = MessageTypeEnum.Success;
                message.MessageText = MessageCodes.Success_Save.GetStringValue();
              }
              if (!this._updatedForReadingOrder)
              {
                StructureNodeDTO location = nodeCollection.Count > 0 ? nodeCollection.First<StructureNodeDTO>().RootNode : this._fixedNodesDTOToDelete.First<StructureNodeDTO>().RootNode;
                StructureNodeLinks locationLink = this._repositoryFactory.GetRepository<StructureNodeLinks>().FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (x => x.ParentNodeId == location.Id));
                IList<StructureNodeLinks> allLinksInTheStructure = (IList<StructureNodeLinks>) null;
                if (locationLink != null)
                {
                  Guid locationLinkParentNodeId = locationLink.ParentNodeId;
                  allLinksInTheStructure = this._repositoryFactory.GetRepository<StructureNodeLinks>().SearchFor((Expression<Func<StructureNodeLinks, bool>>) (x => x.ParentNodeId == locationLinkParentNodeId));
                }
                TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) nodeCollection, (Action<StructureNodeDTO>) (structureNodeDto =>
                {
                  Guid entityId;
                  StructureNodeTypeEnum entityType;
                  StructuresHelper.GetEntityIdAndEntityType(structureNodeDto, out entityId, out entityType);
                  StructureNodeLinks structureNodeLinks = allLinksInTheStructure != null ? allLinksInTheStructure.FirstOrDefault<StructureNodeLinks>((Func<StructureNodeLinks, bool>) (l => l.Node != null && l.Node.Id == structureNodeDto.Id && l.ParentNodeId == (structureNodeDto.ParentNode != null ? structureNodeDto.ParentNode.Id : Guid.Empty) && !l.EndDate.HasValue)) : (StructureNodeLinks) null;
                  message.MessageType = MessageTypeEnum.Success;
                  message.MessageText = MessageCodes.Success_Save.GetStringValue();
                  EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
                  {
                    Guid = structureNodeDto.Id,
                    LinkGuid = structureNodeLinks != null ? structureNodeLinks.Id : Guid.Empty,
                    EntityId = entityId,
                    EntityType = entityType,
                    Message = message
                  }, (IViewModel) this);
                }));
                StructureNodeDTO rootNode = nodeCollection.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.ParentNode == null && n.RootNode == n));
                EventPublisher.Publish<StructureRootUpdated>(new StructureRootUpdated()
                {
                  RootNodeId = rootNode != null ? rootNode.Id : Guid.Empty
                }, (IViewModel) this);
                foreach (StructureNodeDTO replacedMeter in this._replacedMeterList)
                {
                  this.GetStructuresManagerInstance().DeleteStructure(replacedMeter, StructureTypeEnum.Fixed, false);
                  this.GetStructuresManagerInstance().UpdateReplacedMeter(replacedMeter);
                }
                locationLink = (StructureNodeLinks) null;
                rootNode = (StructureNodeDTO) null;
              }
              else
              {
                this._structureBytes = this.GetStructuresManagerInstance().UpdateReplacedMeter(this._replacedMeterList, this._structureBytes);
                EventPublisher.Publish<StructureBytesUpdated>(new StructureBytesUpdated()
                {
                  StructureBytes = this._structureBytes
                }, (IViewModel) this);
              }
            }));
            this.IsBusy = false;
            if (!isDeleted)
              this.OnRequestClose(true);
          }
          nodes = (DataItemCollection) null;
        }));
      }
    }

    public ICommand EditEntityCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO selectedItem = (StructureNodeDTO) (parameter as RadTreeListView).SelectedItem;
          if (selectedItem != null)
          {
            this.SetParentAndRootNode(selectedItem);
            this.EditSelectedStructureNode(selectedItem);
            if (selectedItem.NodeType.Name == "Tenant" && selectedItem.SubNodes.Any<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")))
              this.CalculateNoOfDevicesForTenantParent(selectedItem.SubNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
            else if (selectedItem.NodeType.Name == "Meter")
              this.CalculateNoOfDevicesForTenantParent(selectedItem);
          }
          this.OnPropertyChanged("StructureForSelectedNode");
        }));
      }
    }

    public ICommand DeleteToolbarSelectedItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO selectedItem = (StructureNodeDTO) (parameter as RadTreeListView).SelectedItem;
          StructureNodeDTO structureNodeDto = (StructureNodeDTO) null;
          if (selectedItem.NodeType.Name == "Meter")
            structureNodeDto = this.GetTenantParent(selectedItem);
          this.DeleteSelectedNode(selectedItem);
          if (structureNodeDto != null)
            (structureNodeDto.Entity as TenantDTO).NoOfDevices = new int?(structureNodeDto.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
          this.OnPropertyChanged("StructureForSelectedNode");
        }));
      }
    }

    private void DeleteSelectedNode(StructureNodeDTO selectedNode)
    {
      IEnumerable<StructureNodeDTO> source = StructuresHelper.Descendants(selectedNode);
      if (selectedNode.Id == Guid.Empty && source.All<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.Id == Guid.Empty)))
      {
        this.RemoveSelectedNodeFromStructure(selectedNode, this._nodesCollection);
        this.RemoveSerialNumberFromUniquenessList(selectedNode);
      }
      else
      {
        bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning_DeleteStructure_Title.GetStringValue(), MessageCodes.Warning_DeleteFixedStructure.GetStringValue(), true);
        if (nullable.HasValue && nullable.Value)
        {
          this._fixedNodesDTOToDelete.Add(selectedNode);
          this.GetAffectedStructureNodesToDelete(selectedNode);
          this.RemoveSelectedNodeFromStructure(selectedNode, this._nodesCollection);
          this.RemoveSerialNumberFromUniquenessList(selectedNode);
        }
      }
    }

    public override ICommand CancelWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.OperationCancelled.GetStringValue()
          };
          if (!this.ContinueActionIfMBusIsStarted(this._scannerMinoConnectManager))
            return;
          EventPublisher.Publish<ShowMessage>(new ShowMessage()
          {
            Message = message
          }, (IViewModel) this);
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand ReplaceMeterCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (param =>
        {
          if (param is StructureNodeDTO node2)
            this.EditSelectedStructureNode(node2, true);
          this.OnPropertyChanged("StructureForSelectedNode");
        }));
      }
    }

    public ICommand ReplaceMeterContextMenuCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (param =>
        {
          RadContextMenu parent = param is RadMenuItem radMenuItem2 ? (RadContextMenu) radMenuItem2.Parent : (RadContextMenu) null;
          if (parent != null)
          {
            StructureNodeDTO node = (StructureNodeDTO) parent.GetClickedElement<TreeListViewRow>().Item;
            if (node != null)
              this.EditSelectedStructureNode(node, true);
          }
          this.OnPropertyChanged("StructureForSelectedNode");
        }));
      }
    }

    public ICommand ScanSettingsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<StructureScanSettingsViewModel>((IParameter) new ConstructorArgument("equipmentSettings", (object) this.StructureEquipmentSettings)));
          if (!newModalDialog.HasValue || !newModalDialog.Value)
            return;
          this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(Resources.MSS_Client_ScanSettings_Update_Message);
        }));
      }
    }

    public ICommand StartScanCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.MBusSelectedItem = this.SelectedItem;
          this._scannerMinoConnectManager = new ScanMinoConnectManager(this._repositoryFactory, this.StructureEquipmentSettings, this._deviceManager);
          this._scannerMinoConnectManager.OnProgressChanged += new EventHandler<int>(this.ScannerMinoConnectManagerOnProgressChanged);
          this._scannerMinoConnectManager.OnProgressMessage += new EventHandler<string>(this.ScannerMinoConnectManagerOnProgressMessage);
          this._scannerMinoConnectManager.OnMeterFound += new EventHandler<ZENNER.CommonLibrary.Entities.Meter>(this.ScannerMinoConnectManagerOnMeterFound);
          bool flag = this._scannerMinoConnectManager.StartScan();
          this.IsStartMBusScanButtonEnabled = false;
          this.IsStopMBusScanButtonEnabled = true;
          this.IsWalkByTestButtonEnabled = false;
          this.IsStopWalkByTestButtonEnabled = false;
          ScanMinoConnectManager.IsScanningStarted = true;
          if (flag)
            return;
          this._scannerMinoConnectManager.StopScan();
          this.ResetStartStopButtons();
          MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_ScanSettingsNotSet, false);
        }));
      }
    }

    public ICommand StopScanCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this._scannerMinoConnectManager.StopScan();
          this.ResetStartStopButtons();
        }));
      }
    }

    public ICommand WalkByTestCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO structureNodeDto = CustomerConfiguration.GetPropertyValue<bool>("IsTabletMode") ? this.SelectedStructureNode : this.SelectedItem;
          this.walkByTestManager = new WalkByTestManager(this._repositoryFactory, Guid.Empty, (ProfileType) null);
          this.walkByTestManager.OnMeterValuesReceivedHandler += new EventHandler<MSS.Core.Model.Meters.Meter>(this.OnMeterValuesReceivedHandler);
          this.walkByTestManager.OnErrorReceivedHandler += new EventHandler(this.OnErrorReceivedHandler);
          this.walkByTestManager.StartReadingValues(structureNodeDto, new ProfileType());
          this.IsStartMBusScanButtonEnabled = false;
          this.IsStopMBusScanButtonEnabled = false;
          this.IsWalkByTestButtonEnabled = false;
          this.IsStopWalkByTestButtonEnabled = true;
          WalkByTestManager.IsWalkByTestStarted = true;
          this.GetMetersInStructure(structureNodeDto);
          this._numberOfReadMeters = 0;
          this._numberOfMetersInStructure = this._metersInStructure.Count<StructureNodeDTO>();
        }));
      }
    }

    public ICommand StopWalkByTestCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          this.walkByTestManager.OnMeterValuesReceivedHandler -= new EventHandler<MSS.Core.Model.Meters.Meter>(this.OnMeterValuesReceivedHandler);
          this.walkByTestManager.OnErrorReceivedHandler -= new EventHandler(this.OnErrorReceivedHandler);
          this.walkByTestManager.StopReadingValues();
          this.ResetStartStopButtons();
        }));
      }
    }

    private void OnMeterValuesReceivedHandler(object sender, MSS.Core.Model.Meters.Meter meter)
    {
      MeterDTO savedMeterDTO = Mapper.Map<MSS.Core.Model.Meters.Meter, MeterDTO>(meter);
      foreach (StructureNodeDTO structureNodeDto in this._metersInStructure)
      {
        StructureNodeDTO node = structureNodeDto;
        if (node.NodeType.Name == "Meter" && ((MeterDTO) node.Entity).Id == meter.Id && ((MeterDTO) node.Entity).SerialNumber == meter.SerialNumber)
        {
          ++this._numberOfReadMeters;
          Application.Current.Dispatcher.Invoke((Action) (() =>
          {
            node.Entity = (object) savedMeterDTO;
            node.Image = node.SetImageNode();
          }));
        }
      }
      if (this._numberOfReadMeters != this._numberOfMetersInStructure)
        return;
      this.walkByTestManager.StopReadingValues();
      this.ResetStartStopButtons();
    }

    private void OnErrorReceivedHandler(object sender, object e) => this.ResetStartStopButtons();

    private void ScannerMinoConnectManagerOnMeterFound(object sender, ZENNER.CommonLibrary.Entities.Meter e)
    {
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        try
        {
          this.AddDevicesToFixedStructure(e);
        }
        catch (Exception ex)
        {
          MSS.Business.Errors.MessageHandler.LogException(ex);
          throw;
        }
      }));
    }

    private void ScannerMinoConnectManagerOnProgressMessage(object sender, string e)
    {
      this.ProgressDialogMessage = e;
    }

    private void ScannerMinoConnectManagerOnProgressChanged(object sender, int e)
    {
      this.ProgressBarValue = e;
      if (e != 100 || string.IsNullOrEmpty(this._scannerMinoConnectManager.SystemName) || !(this._scannerMinoConnectManager.SystemName == "M-Bus"))
        return;
      this._scannerMinoConnectManager.StopScan();
      this.ResetStartStopButtons();
    }

    private void GetAffectedStructureNodesToDelete(StructureNodeDTO selectedNode)
    {
      List<StructureNodeLinks> structureNodeLinks;
      List<StructureNode> structureNodes;
      this.GetStructuresManagerInstance().GetFixedStructureNodes(selectedNode, out structureNodeLinks, out structureNodes);
      structureNodeLinks.ForEach(new Action<StructureNodeLinks>(this._fixedStructureNodeLinksToDelete.Add));
      structureNodes.ForEach(new Action<StructureNode>(this._structureNodesToDelete.Add));
    }

    private void SetParentAndRootNode(StructureNodeDTO node)
    {
      if (node.ParentNode != null || node.RootNode != null || this._parentForSelectedNode == null)
        return;
      StructureNodeDTO structureNodeDto = this._parentForSelectedNode.RootNode != this._parentForSelectedNode ? this._parentForSelectedNode.RootNode : this._parentForSelectedNode;
      node.ParentNode = this._parentForSelectedNode;
      node.RootNode = structureNodeDto;
    }

    public override ObservableCollection<StructureNodeDTO> GetStructureCollection()
    {
      return this._nodesCollection;
    }

    private void GetMetersInStructure(StructureNodeDTO selectedNode)
    {
      this._metersInStructure.Clear();
      if (selectedNode.NodeType.Name == "Meter")
        this._metersInStructure.Add(selectedNode);
      this.WalkStructure(selectedNode);
    }

    private void WalkStructure(StructureNodeDTO selectedNode)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) selectedNode.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter")
          this._metersInStructure.Add(subNode);
        this.WalkStructure(subNode);
      }
    }

    private void ResetStartStopButtons()
    {
      ScanMinoConnectManager.IsScanningStarted = false;
      WalkByTestManager.IsWalkByTestStarted = false;
      this.IsStartMBusScanButtonEnabled = true;
      this.IsStopMBusScanButtonEnabled = false;
      this.IsWalkByTestButtonEnabled = true;
      this.IsStopWalkByTestButtonEnabled = false;
    }

    private StructureNodeDTO GetTenantParent(StructureNodeDTO node)
    {
      StructureNodeDTO structureNodeDto = node;
      while (structureNodeDto.ParentNode != null && structureNodeDto != structureNodeDto.RootNode && structureNodeDto.NodeType.Name != "Tenant")
        structureNodeDto = structureNodeDto.ParentNode;
      return structureNodeDto.NodeType.Name == "Tenant" ? structureNodeDto : (StructureNodeDTO) null;
    }

    protected void OnItemDropped(ItemDropped treeChange)
    {
      foreach (StructureNodeDTO root in (Collection<StructureNodeDTO>) this.StructureForSelectedNode)
        this.CalculateNoOfDevicesForAllTenants(root);
    }

    private void GetNodesList(
      ObservableCollection<StructureNodeDTO> nodeCollection,
      ref List<StructureNodeDTO> nodesList)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        nodesList.Add(node);
        this.GetNodesList(node.SubNodes, ref nodesList);
      }
    }

    protected void UpdateStructure(ActionStructureAndEntitiesUpdate update)
    {
      this._node = update.Node;
      this._updatedLocation = update.Location;
      this._updateMeterDTO = update.MeterDTO;
      this._updatedTenant = update.Tenant;
      this._name = update.Name;
      this._description = update.Description;
      if (update.Location != null)
        this._nodesCollection[0] = update.Node;
      if (this._updateMeterDTO != null)
        this._node.Entity = (object) this._updateMeterDTO;
      if (this._updatedTenant != null)
        this._node.Entity = (object) Mapper.Map<Tenant, TenantDTO>(this._updatedTenant);
      if (this._node != null)
      {
        this._node.Name = this._name;
        this._node.Description = this._description;
      }
      if (this._isEditStructureNode)
        return;
      new StructuresManager(this._repositoryFactory).TransactionalUpdateStructure((IList<StructureNodeDTO>) new StructureNodeDTO[1]
      {
        this._node
      }, StructureTypeEnum.Fixed, (StructureNodeEquipmentSettings) null);
      if (this._node.NodeType.Name == "Meter")
        this._newNodes.Insert(0, this._node);
      else if (this._node.NodeType.Name == "Tenant")
        this._newNodes.Insert(this._newNodes.Count, this._node);
    }
  }
}
