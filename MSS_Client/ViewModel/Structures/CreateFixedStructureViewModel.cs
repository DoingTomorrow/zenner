// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.CreateFixedStructureViewModel
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
using MSS.Core.Model.Structures;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Settings;
using MVVM.Commands;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class CreateFixedStructureViewModel : StructureViewModelBase
  {
    private ObservableCollection<StructureNodeDTO> _nodesCollection;
    private ScanMinoConnectManager _scannerMinoConnectManager;
    private int _numberOfReadMeters;
    private int _numberOfMetersInStructure;
    private List<StructureNodeDTO> _metersInStructure = new List<StructureNodeDTO>();
    private WalkByTestManager walkByTestManager;
    private bool _isEditStructureNode;
    private readonly IDeviceManager _deviceManager;
    private string _progressDialogMessage;
    private int _progressBarValue;
    private bool _isTenantSelected;
    private StructureNodeDTO _selectedStructureNode;
    private StructureNodeDTO _selectedTenantStructureNode;
    private StructureNodeDTO _selectedMeterStructureNode;
    private bool _isPasteActive;
    private StructureNodeDTO _savedInClipBoardStructureNodeDto;

    public CreateFixedStructureViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      this.AvailableNodesGroups = new ObservableCollection<Group>();
      Group group = new Group()
      {
        Name = CultureResources.GetValue("MSS_Available_Nodes_Folder")
      };
      StructuresHelper.LoadItemsInGroup(group, this.AvailableFixedStructureNodes);
      this.AvailableNodesGroups.Add(group);
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>();
      this._nodesCollection = this.InitializeNewFixedStructure();
      Mapper.CreateMap<MSS.Core.Model.Meters.Meter, MeterDTO>();
      Mapper.CreateMap<MSS.Core.Model.Structures.Location, LocationDTO>();
      Mapper.CreateMap<Tenant, TenantDTO>();
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(((StructureViewModelBase) this).UpdateEntities));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(((StructureViewModelBase) this).ShowActionSyncFinished));
      this.DragDropAttachedProp = new DragDropAttachedObject()
      {
        IsEnabled = true,
        PhysicalLinks = new List<StructureNodeLinks>()
      };
      this._deviceManager = (IDeviceManager) new DeviceManagerWrapper("DefaultScanner");
      EventPublisher.Register<ItemDropped>(new Action<ItemDropped>(this.OnItemDropped));
      if (CustomerConfiguration.GetPropertyValue<bool>("IsTabletMode"))
      {
        bool? nullable = new bool?();
        while (!nullable.HasValue || !nullable.Value)
        {
          IKernel configurator = DIConfigurator.GetConfigurator();
          IParameter[] parameterArray = new IParameter[3];
          Guid? id = this._nodesCollection[0]?.Id;
          Guid empty = Guid.Empty;
          parameterArray[0] = (IParameter) new ConstructorArgument("isExistingEntity", (object) (bool) (id.HasValue ? (id.HasValue ? (id.GetValueOrDefault() != empty ? 1 : 0) : 0) : 1));
          parameterArray[1] = (IParameter) new ConstructorArgument("node", (object) this._nodesCollection[0]);
          parameterArray[2] = (IParameter) new ConstructorArgument("locationNumberList", (object) this.locationNumberList);
          nullable = this._windowFactory.CreateNewModalDialog((IViewModel) configurator.Get<CreateEditLocationViewModel>(parameterArray));
          if (nullable.HasValue && nullable.Value)
          {
            this._nodesCollection[0] = this._node;
            LocationDTO locationDto = new LocationDTO();
            this._nodesCollection[0].Entity = (object) Mapper.Map<LocationDTO>((object) this._updatedLocation);
            this.GetStructuresManagerInstance().TransactionalSaveNewFixedStructure((IList<StructureNodeDTO>) new List<StructureNodeDTO>()
            {
              this._nodesCollection[0]
            }, this.StructureEquipmentSettings);
          }
        }
        EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(this.UpdateStructure));
      }
      this.StructureEquipmentSettings = new StructureNodeEquipmentSettings();
      this.ResetStartStopButtons();
      this.IsTenantSelected = false;
    }

    public DragDropAttachedObject DragDropAttachedProp { get; set; }

    private ObservableCollection<StructureNodeDTO> InitializeNewFixedStructure()
    {
      ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
      StructureNodeType nodeType = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (n => n.Name == StructureNodeTypeEnum.Location.GetStringValue()));
      StructureNodeDTO structureNodeDto = new StructureNodeDTO(Guid.Empty, nodeType.Name, new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Fixed);
      structureNodeDto.RootNode = structureNodeDto;
      observableCollection.Add(structureNodeDto);
      this.StructureEquipmentSettings = new StructureNodeEquipmentSettings();
      return observableCollection;
    }

    public ObservableCollection<Group> AvailableNodesGroups { get; private set; }

    public ObservableCollection<StructureNodeDTO> AvailableFixedStructureNodes
    {
      get
      {
        List<StructureNodeDTO> source = new List<StructureNodeDTO>();
        IRepository<StructureNodeType> nodeTypeRepository = this._structureNodeTypeRepository;
        Expression<Func<StructureNodeType, bool>> predicate = (Expression<Func<StructureNodeType, bool>>) (n => n.IsFixed);
        foreach (StructureNodeType nodeType in (IEnumerable<StructureNodeType>) nodeTypeRepository.SearchFor(predicate))
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

    public ObservableCollection<StructureNodeDTO> FixedStructureNodeCollection
    {
      get
      {
        StructureImageHelper.SetImageIconPath(this._nodesCollection);
        return this._nodesCollection;
      }
    }

    public override ObservableCollection<StructureNodeDTO> GetStructureCollection()
    {
      return this._nodesCollection;
    }

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
          this.OnPropertyChanged("FixedStructureNodeCollection");
        }));
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
          this.OnPropertyChanged("FixedStructureNodeCollection");
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
              RootNode = this.FixedStructureNodeCollection.ElementAt<StructureNodeDTO>(0)
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
          this.OnPropertyChanged("FixedStructureNodeCollection");
        }));
      }
    }

    public ICommand SaveStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is RadTreeListView radTreeListView2))
            return;
          radTreeListView2.ExpandAllHierarchyItems();
          ObservableCollection<StructureNodeDTO> nodeCollection = new ObservableCollection<StructureNodeDTO>(radTreeListView2.Items.Cast<StructureNodeDTO>());
          nodeCollection.SetNodesOrderNumber();
          if (this.ContinueActionIfMBusIsStarted(this._scannerMinoConnectManager))
            this.SaveFixedStructure(nodeCollection);
        }));
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

    private void SaveFixedStructure(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      if (!this.ReconstructCollectionWithoutInvalidMBusScannerMeters(nodeCollection))
        return;
      this.GetStructuresManagerInstance().TransactionalSaveNewFixedStructure((IList<StructureNodeDTO>) nodeCollection, this.StructureEquipmentSettings);
      MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
      message.MessageType = MessageTypeEnum.Success;
      message.MessageText = MessageCodes.Success_Save.GetStringValue();
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        Guid entityId;
        StructureNodeTypeEnum entityType;
        StructuresHelper.GetEntityIdAndEntityType(node, out entityId, out entityType);
        EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
        {
          Guid = node.Id,
          EntityId = entityId,
          EntityType = entityType,
          Message = message
        }, (IViewModel) this);
      }
      this.OnRequestClose(false);
    }

    public ICommand EditEntityCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          StructureNodeDTO selectedItem = (StructureNodeDTO) radTreeListView.SelectedItem;
          if (selectedItem != null)
          {
            this.EditSelectedStructureNode(selectedItem);
            if (selectedItem.NodeType.Name == "Tenant" && selectedItem.SubNodes.Any<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")))
              this.CalculateNoOfDevicesForTenantParent(selectedItem.SubNodes.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
            else if (selectedItem.NodeType.Name == "Meter")
              this.CalculateNoOfDevicesForTenantParent(selectedItem);
          }
          this._nodesCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (object obj in radTreeListView.Items)
          {
            if (((StructureNodeDTO) obj).ParentNode == null)
              this._nodesCollection.Add((StructureNodeDTO) obj);
          }
          this.OnPropertyChanged("FixedStructureNodeCollection");
        }));
      }
    }

    public ICommand DeleteToolbarSelectedItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadTreeListView radTreeListView = parameter as RadTreeListView;
          StructureNodeDTO selectedItem = (StructureNodeDTO) radTreeListView.SelectedItem;
          this._nodesCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (object obj in radTreeListView.Items)
          {
            if (((StructureNodeDTO) obj).ParentNode == null)
              this._nodesCollection.Add((StructureNodeDTO) obj);
          }
          StructureNodeDTO structureNodeDto = (StructureNodeDTO) null;
          if (selectedItem.NodeType.Name == "Meter")
            structureNodeDto = this.GetTenantParent(selectedItem);
          this.RemoveSelectedNodeFromStructure(selectedItem, this._nodesCollection);
          this.RemoveSerialNumberFromUniquenessList(selectedItem);
          if (structureNodeDto != null)
            (structureNodeDto.Entity as TenantDTO).NoOfDevices = new int?(structureNodeDto.SubNodes.Count<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.NodeType.Name == "Meter")));
          this.OnPropertyChanged("FixedStructureNodeCollection");
        }));
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

    public override ICommand CancelWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
          message.MessageType = MessageTypeEnum.Warning;
          message.MessageText = MessageCodes.OperationCancelled.GetStringValue();
          if (!this.ContinueActionIfMBusIsStarted(this._scannerMinoConnectManager))
            return;
          EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
          {
            Guid = Guid.Empty,
            Message = message
          }, (IViewModel) this);
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand CancelWindowAndDeleteStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          StructureNodeDTO nodeDTO = StructuresHelper.LoadStructureFromRootNodeId(this._repositoryFactory, this._nodesCollection[0].Id);
          this.GetStructuresManagerInstance().DeleteStructureNodeAndDescendants(nodeDTO);
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
          {
            MessageType = MessageTypeEnum.Warning,
            MessageText = MessageCodes.OperationCancelled.GetStringValue()
          };
          this.OnRequestClose(false);
        }));
      }
    }

    public ICommand ScanSettingsCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
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
          WalkByTestManager.IsWalkByTestStarted = false;
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
          MSS.Business.Errors.MessageHandler.LogDebug("User stoped the scanning");
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
      foreach (StructureNodeDTO fixedStructureNode in (Collection<StructureNodeDTO>) this.FixedStructureNodeCollection)
        this.CalculateNoOfDevicesForAllTenants(fixedStructureNode);
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
  }
}
