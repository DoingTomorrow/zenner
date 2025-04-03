// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.EditPhysicalStructureViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using Microsoft.Win32;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Business.Modules.GMM;
using MSS.Business.Modules.GMMWrapper;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Modules.UsersManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.Core.Utils;
using MSS.DIConfiguration;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MSS_Client.ViewModel.Configuration;
using MSS_Client.ViewModel.Meters;
using MSS_Client.ViewModel.Settings;
using MVVM.Commands;
using NHibernate;
using NHibernate.Linq;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
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
  public class EditPhysicalStructureViewModel : StructureViewModelBase
  {
    private readonly StructureNodeDTO _parentForSelectedNode;
    private readonly StructureNodeDTO _selectedNode;
    private bool _updatedForReadingOrder;
    private string _systemName;
    private ObservableCollection<StructureNodeDTO> _nodesCollection = new ObservableCollection<StructureNodeDTO>();
    private List<StructureNodeLinks> _physicalStructureNodeLinksToDelete = new List<StructureNodeLinks>();
    private List<StructureNodeLinks> _logicalStructureNodeLinksToDelete = new List<StructureNodeLinks>();
    private List<StructureNode> _structureNodesToDelete = new List<StructureNode>();
    private Dictionary<StructureNodeDTO, StructureNodeEquipmentSettings> _equipmentSettingsForMeters;
    private ObservableCollection<StructureNodeDTO> _physicalNodesDTOToDelete = new ObservableCollection<StructureNodeDTO>();
    private ObservableCollection<StructureNodeDTO> _logicalNodesDTOToDelete = new ObservableCollection<StructureNodeDTO>();
    private byte[] _structureBytes;
    private ScanMinoConnectManager _scannerMinoConnectManager;
    private int _metersInStructure;
    private IRepository<PhotoMeter> _photoMeterRepository;
    private IRepository<Note> _noteRepository;
    private bool wasSaveExecuted = false;
    private readonly IDeviceManager _deviceManager;
    private string _progressDialogMessage;
    private bool _isBusy;
    private int _progressBarValue;
    private bool _isPasteActive;
    private bool _isPhotosButtonEnabled;
    private StructureNodeDTO _savedInClipBoardStructureNodeDto;
    private bool _mBusScanNetworkVisibility;

    public EditPhysicalStructureViewModel(
      StructureNodeDTO selectedNode,
      bool isExecuteInstallation,
      bool updatedForReadingOrder,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      this._selectedNode = selectedNode;
      this._updatedForReadingOrder = updatedForReadingOrder;
      this._nodesCollection.Add(this._selectedNode);
      this._parentForSelectedNode = selectedNode?.ParentNode;
      this.Title = isExecuteInstallation ? Resources.MSS_Client_Execute_Installation_Order : Resources.MSS_Client_Structures_Edit_Physical_Structure;
      this.UpdateSerialNumberListForReadingOrder(updatedForReadingOrder);
      this._photoMeterRepository = this._repositoryFactory.GetRepository<PhotoMeter>();
      this._noteRepository = this._repositoryFactory.GetRepository<Note>();
      this._deviceManager = (IDeviceManager) new DeviceManagerWrapper("DefaultScanner");
      this.AvailableNodesGroups = new ObservableCollection<MSS.Business.Modules.StructuresManagement.Group>();
      MSS.Business.Modules.StructuresManagement.Group group = new MSS.Business.Modules.StructuresManagement.Group();
      group.Name = CultureResources.GetValue("MSS_Available_Nodes_Folder");
      StructuresHelper.LoadItemsInGroup(group, this.AvailableNodesSettingsGroup);
      this.AvailableNodesGroups.Add(group);
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>();
      Mapper.CreateMap<MSS.Core.Model.Meters.Meter, MeterDTO>();
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(((StructureViewModelBase) this).UpdateEntities));
      EventPublisher.Register<ReplaceDeviceEvent>(new Action<ReplaceDeviceEvent>(((StructureViewModelBase) this).ReplaceDevice));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(((StructureViewModelBase) this).ShowActionSyncFinished));
      EventPublisher.Register<TreeDragDropChange>(new Action<TreeDragDropChange>(this.OnTreeChange));
      EventPublisher.Register<MeterNotesUpdated>(new Action<MeterNotesUpdated>(this.UpdateTreeWithMeterNotes));
      EventPublisher.Register<MeterPhotosUpdated>(new Action<MeterPhotosUpdated>(this.UpdateTreeWithMeterPhotos));
      this.InitStructureForSelectedNode();
      ScanMinoConnectManager.IsScanningStarted = false;
      this.DragDropAttachedProp = new DragDropAttachedObject()
      {
        IsEnabled = true,
        PhysicalLinks = new List<StructureNodeLinks>()
      };
      this.StructureEquipmentSettings = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().FirstOrDefault((Expression<Func<StructureNodeEquipmentSettings, bool>>) (e => e.StructureNode.Id == selectedNode.RootNode.Id)) ?? new StructureNodeEquipmentSettings();
      this.UpdateDevicesFoundLabel();
      this.IsChangeDeviceModelParametersEnabled = false;
      StructureViewModelBase.deviceModelsInLicense = LicenseHelper.GetDeviceTypes().ToList<string>();
      this._equipmentSettingsForMeters = new Dictionary<StructureNodeDTO, StructureNodeEquipmentSettings>();
      List<StructureNodeDTO> structureNodesForMeters = new List<StructureNodeDTO>();
      List<StructureNodeDTO> structureNodes = new List<StructureNodeDTO>();
      structureNodes.Add(this.StructureForSelectedNode[0]);
      this.GetMeterNodesAndStructureNodes(this.StructureForSelectedNode[0], ref structureNodesForMeters, ref structureNodes);
      List<Guid> structureNodeIds = structureNodesForMeters.Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (item => item.Id)).ToList<Guid>();
      List<StructureNodeEquipmentSettings> list = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>().Where((Expression<Func<StructureNodeEquipmentSettings, bool>>) (item => structureNodeIds.Contains(item.StructureNode.Id))).ToList<StructureNodeEquipmentSettings>();
      foreach (StructureNodeDTO structureNodeDto in structureNodesForMeters)
      {
        StructureNodeDTO currentStructureNode = structureNodeDto;
        StructureNodeEquipmentSettings equipmentSettings = list.FirstOrDefault<StructureNodeEquipmentSettings>((Func<StructureNodeEquipmentSettings, bool>) (item => item.StructureNode.Id == currentStructureNode.Id));
        if (equipmentSettings != null)
          this._equipmentSettingsForMeters.Add(currentStructureNode, equipmentSettings);
      }
      foreach (StructureNodeDTO node in structureNodes)
        node.IsMeterNonEditable = node.Entity is MeterDTO && this.IsItemNonEditable(node);
      this.MBusScanNetworkVisibility = new UsersManager(repositoryFactory).HasRight(OperationEnum.MbuScanNetwork.ToString());
    }

    public DragDropAttachedObject DragDropAttachedProp { get; set; }

    public ObservableCollection<MSS.Business.Modules.StructuresManagement.Group> AvailableNodesGroups { get; private set; }

    public ObservableCollection<StructureNodeDTO> AvailableNodesSettingsGroup
    {
      get
      {
        List<StructureNodeDTO> source = new List<StructureNodeDTO>();
        IRepository<StructureNodeType> nodeTypeRepository = this._structureNodeTypeRepository;
        Expression<Func<StructureNodeType, bool>> predicate = (Expression<Func<StructureNodeType, bool>>) (n => n.IsPhysical);
        foreach (StructureNodeType nodeType in (IEnumerable<StructureNodeType>) nodeTypeRepository.SearchFor(predicate))
        {
          StructureNodeDTO structureNodeDto = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_StructureNode_" + nodeType.Name), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Physical);
          source.Add(structureNodeDto);
        }
        return new ObservableCollection<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) source.OrderBy<StructureNodeDTO, string>((Func<StructureNodeDTO, string>) (n => n.Name)));
      }
    }

    public ObservableCollection<StructureNodeDTO> StructureForSelectedNode
    {
      get => this._nodesCollection;
      set
      {
        this._nodesCollection = value;
        this.OnPropertyChanged(nameof (StructureForSelectedNode));
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

    public bool IsBusy
    {
      get => this._isBusy;
      set
      {
        this._isBusy = value;
        this.OnPropertyChanged(nameof (IsBusy));
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

    public StructureNodeDTO SelectedStructureNode { get; set; }

    public bool IsPasteActive
    {
      get => this._isPasteActive;
      set
      {
        this._isPasteActive = value;
        this.OnPropertyChanged(nameof (IsPasteActive));
      }
    }

    public bool IsPhotosButtonEnabled
    {
      get => this._isPhotosButtonEnabled;
      set
      {
        this._isPhotosButtonEnabled = value;
        this.OnPropertyChanged(nameof (IsPhotosButtonEnabled));
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

    public bool MBusScanNetworkVisibility
    {
      get => this._mBusScanNetworkVisibility;
      set
      {
        this._mBusScanNetworkVisibility = value;
        this.OnPropertyChanged(nameof (MBusScanNetworkVisibility));
      }
    }

    public ICommand ChangeDeviceModelParametersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.SelectedItem == null || !(this.SelectedItem.NodeType.Name == "Meter"))
            return;
          if (this.SelectedItem.Entity is MeterDTO entity2)
          {
            DeviceModel deviceModel = this._deviceManager.GetDeviceModel(entity2.DeviceType.GetGMMDeviceModelName());
            StructureNodeEquipmentSettings equipmentSettings = this._equipmentSettingsForMeters.FirstOrDefault<KeyValuePair<StructureNodeDTO, StructureNodeEquipmentSettings>>((Func<KeyValuePair<StructureNodeDTO, StructureNodeEquipmentSettings>, bool>) (item => item.Key == this.SelectedItem)).Value;
            if (equipmentSettings != null && !string.IsNullOrEmpty(equipmentSettings.DeviceModelReadingParams))
              deviceModel = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.UpdateDeviceModelWithSavedParams(deviceModel, equipmentSettings.DeviceModelReadingParams);
            if (deviceModel != null && deviceModel.ChangeableParameters != null && deviceModel.ChangeableParameters.Any<ChangeableParameter>())
            {
              DeviceModelChangeableParametersViewModel parametersViewModel = DIConfigurator.GetConfigurator().Get<DeviceModelChangeableParametersViewModel>((IParameter) new ConstructorArgument("selectedDeviceModel", (object) deviceModel));
              bool? newModalDialog = this._windowFactory.CreateNewModalDialog((IViewModel) parametersViewModel);
              if (newModalDialog.HasValue & newModalDialog.Value)
              {
                List<Config> changeableParameters = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.GetConfigListFromChangeableParameters(parametersViewModel.SelectedDeviceModel.ChangeableParameters);
                if (equipmentSettings == null)
                  equipmentSettings = new StructureNodeEquipmentSettings();
                equipmentSettings.DeviceModelReadingParams = MSS.Business.Modules.AppParametersManagement.AppParametersManagement.SerializedConfigList(changeableParameters, parametersViewModel.SelectedDeviceModel.ChangeableParameters);
                if (this._equipmentSettingsForMeters.Any<KeyValuePair<StructureNodeDTO, StructureNodeEquipmentSettings>>() && this._equipmentSettingsForMeters.Keys.Contains<StructureNodeDTO>(this.SelectedItem) && this._equipmentSettingsForMeters[this.SelectedItem] != null)
                  this._equipmentSettingsForMeters[this.SelectedItem] = equipmentSettings;
                else
                  this._equipmentSettingsForMeters.Add(this.SelectedItem, equipmentSettings);
              }
            }
            else
              MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_Client_DeviceModeChangeableParameters_ParamsAreNull, false);
          }
          else
            MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.MSS_DeviceModelChangeableParams_MissingDeviceModel, false);
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
            if (((Collection<StructureNodeDTO>) radTreeListView.ItemsSource).Count == 0)
            {
              ((Collection<StructureNodeDTO>) radTreeListView.ItemsSource).Add(this.SavedinClipStructureNodeDto);
              this.SavedinClipStructureNodeDto = (StructureNodeDTO) null;
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_ROOTNODE_CANNOT_BE_CUTTED);
            }
          }
          else
            this.SavedinClipStructureNodeDto.ParentNode.SubNodes.Remove(this.SavedinClipStructureNodeDto);
          this.UpdateDevicesFoundLabel();
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
          StructureNodesValidator structureNodesValidator = new StructureNodesValidator();
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
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CUT_PASTE_ERROR);
          this.UpdateDevicesFoundLabel();
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
          StructureNodesValidator structureNodesValidator = new StructureNodesValidator();
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
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_CUT_PASTE_ERROR);
          this.OnPropertyChanged("StructureForSelectedNode");
          this.UpdateDevicesFoundLabel();
        }));
      }
    }

    public ICommand AddNodeCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          StructureNodeDTO structureNodeDto = parameter as StructureNodeDTO;
          StructureNodeDTO destination = new StructureNodeDTO();
          StructureNodesValidator structureNodesValidator = new StructureNodesValidator();
          Mapper.Map<StructureNodeDTO, StructureNodeDTO>(structureNodeDto, destination);
          if (this.SelectedItem == null)
          {
            if (!destination.NodeType.Name.Equals(typeof (MSS.Core.Model.Meters.Meter).Name))
            {
              destination.RootNode = destination;
              this.StructureForSelectedNode.Add(destination);
            }
            else
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
          }
          else if (structureNodesValidator.IsValidNodesRelationship(structureNodeDto, this.SelectedItem, false))
          {
            this.SelectedItem.SubNodes.Add(destination);
            this.OnPropertyChanged("StructureForSelectedNode");
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
          this.UpdateDevicesFoundLabel();
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
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
          radTreeListView2.ExpandAllHierarchyItems();
          DataItemCollection items = radTreeListView2.Items;
          ObservableCollection<StructureNodeDTO> nodeCollection = new ObservableCollection<StructureNodeDTO>(items.Cast<StructureNodeDTO>());
          bool flag = this.ReconstructCollectionWithoutInvalidMBusScannerMeters(nodeCollection);
          if (this.ContinueActionIfMBusIsStarted(this._scannerMinoConnectManager) && flag)
          {
            if (this.RemoveDuplicateAndNonExistentSerialNumbers(ref nodeCollection))
              MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DeleteStructure_Warning_Title, Resources.MSS_Client_DuplicateMetersWillNotBeSaved, false);
            nodeCollection.SetNodesOrderNumber(this._selectedNode, this._parentForSelectedNode);
            foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) nodeCollection)
            {
              if (structureNodeDto.Entity is MeterDTO && !((MeterDTO) structureNodeDto.Entity).ConfigDate.HasValue)
                ((MeterDTO) structureNodeDto.Entity).ConfigDate = new DateTime?(DateTime.Now);
            }
            ObservableCollection<StructureNodeDTO> observableCollection = this.ReconstructNodeCollection(nodeCollection, this._parentForSelectedNode);
            if (this._physicalStructureNodeLinksToDelete.Count != 0 || this._logicalStructureNodeLinksToDelete.Count != 0 || this._structureNodesToDelete.Count != 0)
            {
              foreach (StructureNodeDTO selectedNode in (Collection<StructureNodeDTO>) this._physicalNodesDTOToDelete)
                this.GetStructuresManagerInstance().GetAffectedLogicalStructure(selectedNode, StructureTypeEnum.Physical).ToList<StructureNodeDTO>().ForEach(new Action<StructureNodeDTO>(((Collection<StructureNodeDTO>) this._logicalNodesDTOToDelete).Add));
              bool? deleteDialog = this.ShowWarningWithStructuresToDeleteDialog(this._physicalNodesDTOToDelete, this._logicalNodesDTOToDelete);
              if (deleteDialog.HasValue && deleteDialog.Value)
              {
                List<StructureNodeLinks> structureNodeLinksToBeDeleted = new List<StructureNodeLinks>();
                this._physicalStructureNodeLinksToDelete.ForEach(new Action<StructureNodeLinks>(structureNodeLinksToBeDeleted.Add));
                this._logicalStructureNodeLinksToDelete.ForEach(new Action<StructureNodeLinks>(structureNodeLinksToBeDeleted.Add));
                if (!this._updatedForReadingOrder)
                  this.GetStructuresManagerInstance().TransactionalUpdateStructure((IList<StructureNodeDTO>) observableCollection, StructureTypeEnum.Physical, this.StructureEquipmentSettings, (IList<StructureNodeLinks>) structureNodeLinksToBeDeleted, (IList<StructureNode>) this._structureNodesToDelete);
                else
                  this._structureBytes = this.CreateStructureBytes(observableCollection, this._structureBytes);
              }
              else
              {
                message.MessageType = MessageTypeEnum.Warning;
                message.MessageText = MessageCodes.OperationCancelled.GetStringValue();
              }
            }
            else
            {
              if (!this._updatedForReadingOrder)
                this.GetStructuresManagerInstance().TransactionalUpdateStructure((IList<StructureNodeDTO>) observableCollection, StructureTypeEnum.Physical, this.StructureEquipmentSettings);
              else
                this._structureBytes = this.CreateStructureBytes(observableCollection, this._structureBytes);
              message.MessageType = MessageTypeEnum.Success;
              message.MessageText = MessageCodes.Success_Save.GetStringValue();
            }
            if (!this._updatedForReadingOrder)
            {
              if (nodeCollection != null && nodeCollection.Any<StructureNodeDTO>())
              {
                foreach (StructureNodeDTO structureNodeDto1 in (Collection<StructureNodeDTO>) nodeCollection)
                {
                  StructureNodeDTO structureNodeDto = structureNodeDto1;
                  Guid entityId;
                  StructureNodeTypeEnum entityType;
                  StructuresHelper.GetEntityIdAndEntityType(structureNodeDto, out entityId, out entityType);
                  StructureNodeLinks structureNodeLinks = this._repositoryFactory.GetRepository<StructureNodeLinks>().FirstOrDefault((Expression<Func<StructureNodeLinks, bool>>) (l => l.Node.Id == structureNodeDto.Id && l.ParentNodeId == (structureNodeDto.ParentNode != default (object) ? structureNodeDto.ParentNode.Id : Guid.Empty) && l.RootNode.Id == structureNodeDto.RootNode.Id && l.EndDate == new DateTime?()));
                  EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
                  {
                    Guid = structureNodeDto.Id,
                    LinkGuid = structureNodeLinks != null ? structureNodeLinks.Id : Guid.Empty,
                    EntityId = entityId,
                    EntityType = entityType,
                    Message = message
                  }, (IViewModel) this);
                }
                if (items.Count > observableCollection.Count)
                  EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
                  {
                    Guid = Guid.Empty,
                    Message = message
                  }, (IViewModel) this);
                StructureNodeDTO structureNodeDto2 = nodeCollection.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.ParentNode == null && n.RootNode == n));
                EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
                {
                  RootNode = structureNodeDto2,
                  Message = message
                }, (IViewModel) this);
              }
              else
                EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
                {
                  Message = message
                }, (IViewModel) this);
              foreach (StructureNodeDTO replacedMeter1 in this._replacedMeterList)
              {
                StructureNodeDTO replacedMeter = replacedMeter1;
                StructureNodeDTO structureNodeDto = replacedMeter;
                if (replacedMeter.Id == Guid.Empty)
                  structureNodeDto = Mapper.Map<StructureNode, StructureNodeDTO>(this._structureNodeRepository.FirstOrDefault((Expression<Func<StructureNode, bool>>) (item => item.EntityId == (replacedMeter.Entity as MeterDTO).Id && item.EndDate == new DateTime?())));
                if (structureNodeDto != null)
                {
                  this.GetStructuresManagerInstance().DeleteStructure(structureNodeDto, StructureTypeEnum.Physical, false);
                  this.GetStructuresManagerInstance().UpdateReplacedMeter(structureNodeDto);
                  this.GetStructuresManagerInstance().ReplacePhysicalMeterInLogicalStructure(structureNodeDto);
                }
              }
            }
            else
            {
              this._structureBytes = this.GetStructuresManagerInstance().UpdateReplacedMeter(this._replacedMeterList, this._structureBytes);
              EventPublisher.Publish<StructureBytesUpdated>(new StructureBytesUpdated()
              {
                StructureBytes = this._structureBytes
              }, (IViewModel) this);
            }
            if (this._equipmentSettingsForMeters.Any<KeyValuePair<StructureNodeDTO, StructureNodeEquipmentSettings>>())
            {
              List<Guid> structureNodeIds = this._equipmentSettingsForMeters.Keys.Select<StructureNodeDTO, Guid>((Func<StructureNodeDTO, Guid>) (item => item.Id)).ToList<Guid>();
              List<StructureNode> list = this._structureNodeRepository.Where((Expression<Func<StructureNode, bool>>) (item => structureNodeIds.Contains(item.Id))).ToList<StructureNode>();
              foreach (KeyValuePair<StructureNodeDTO, StructureNodeEquipmentSettings> settingsForMeter in this._equipmentSettingsForMeters)
              {
                KeyValuePair<StructureNodeDTO, StructureNodeEquipmentSettings> currentEquipmentSettings = settingsForMeter;
                StructureNode structureNode = list.FirstOrDefault<StructureNode>((Func<StructureNode, bool>) (item => item.Id == currentEquipmentSettings.Key.Id));
                currentEquipmentSettings.Value.StructureNode = structureNode;
              }
              IRepository<StructureNodeEquipmentSettings> repository = this._repositoryFactory.GetRepository<StructureNodeEquipmentSettings>();
              List<StructureNodeEquipmentSettings> equipmentSettingsToInsert = new List<StructureNodeEquipmentSettings>();
              List<StructureNodeEquipmentSettings> equipmentSettingsToUpdate = new List<StructureNodeEquipmentSettings>();
              TypeHelperExtensionMethods.ForEach<StructureNodeEquipmentSettings>((IEnumerable<StructureNodeEquipmentSettings>) this._equipmentSettingsForMeters.Values, (Action<StructureNodeEquipmentSettings>) (eqSettings =>
              {
                if (eqSettings.Id == Guid.Empty)
                  equipmentSettingsToInsert.Add(eqSettings);
                else
                  equipmentSettingsToUpdate.Add(eqSettings);
              }));
              ISession session = this._repositoryFactory.GetSession();
              session.BeginTransaction();
              repository.TransactionalInsertMany((IEnumerable<StructureNodeEquipmentSettings>) equipmentSettingsToInsert);
              repository.TransactionalInsertMany((IEnumerable<StructureNodeEquipmentSettings>) equipmentSettingsToUpdate);
              session.Transaction.Commit();
              session.Clear();
            }
            this.wasSaveExecuted = true;
            this.OnRequestClose(true);
          }
        }));
      }
    }

    private bool RemoveDuplicateAndNonExistentSerialNumbers(
      ref ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      List<string> distinctSerialNumbers = new List<string>();
      List<StructureNodeDTO> nodesToRemove = new List<StructureNodeDTO>();
      TypeHelperExtensionMethods.ForEach<StructureNodeDTO>((IEnumerable<StructureNodeDTO>) nodeCollection, (Action<StructureNodeDTO>) (item =>
      {
        if (!(item.NodeType.Name == "Meter") && !(item.NodeType.Name == "RadioMeter"))
          return;
        if (item.Entity is MeterDTO entity2 && !distinctSerialNumbers.Contains(entity2.SerialNumber) && !string.IsNullOrEmpty(entity2.SerialNumber))
          distinctSerialNumbers.Add(entity2.SerialNumber);
        else
          nodesToRemove.Add(item);
      }));
      foreach (StructureNodeDTO node in nodesToRemove)
      {
        if (nodeCollection.Contains(node))
        {
          this.RemoveNodeAndChildrenFromCollection(node, ref nodeCollection);
          node.ParentNode.SubNodes.Remove(node);
        }
      }
      return nodesToRemove.Any<StructureNodeDTO>();
    }

    private void RemoveNodeAndChildrenFromCollection(
      StructureNodeDTO node,
      ref ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      nodeCollection.Remove(node);
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
        this.RemoveNodeAndChildrenFromCollection(subNode, ref nodeCollection);
    }

    private byte[] CreateStructureBytes(
      ObservableCollection<StructureNodeDTO> actualNodeCollection,
      byte[] structureBytes)
    {
      this.GetStructuresManagerInstance().InsertEntitiesGuid(actualNodeCollection);
      this.GetStructuresManagerInstance().InsertStructureNodesGuid(actualNodeCollection);
      structureBytes = StructuresHelper.SerializeStructure(actualNodeCollection, structureBytes);
      return structureBytes;
    }

    public ICommand EditSelectedItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          if (!(parameter is RadMenuItem radMenuItem2))
            return;
          RadContextMenu parent = (RadContextMenu) radMenuItem2.Parent;
          if (parent != null)
          {
            StructureNodeDTO structureNodeDto = (StructureNodeDTO) parent.GetClickedElement<TreeListViewRow>().Item;
            this.SetParentAndRootNode(structureNodeDto);
            this.EditSelectedStructureNode(structureNodeDto);
            this.OnPropertyChanged("StructureForSelectedNode");
          }
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
          if (selectedItem == null)
            return;
          this.SetParentAndRootNode(selectedItem);
          this.EditSelectedStructureNode(selectedItem);
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
          this.DeleteSelectedNodes(this.GetNodesToDelete(parameter as RadTreeListView));
          this.UpdateDevicesFoundLabel();
        }));
      }
    }

    private List<StructureNodeDTO> GetNodesToDelete(RadTreeListView radTreeListView)
    {
      List<StructureNodeDTO> nodesToDelete = new List<StructureNodeDTO>();
      foreach (object selectedItem in (Collection<object>) radTreeListView.SelectedItems)
      {
        StructureNodeDTO foundNode = (StructureNodeDTO) null;
        this.GetNodeToDeleteFromStructure((StructureNodeDTO) selectedItem, this.StructureForSelectedNode, ref foundNode);
        if (foundNode != null)
          nodesToDelete.Add(foundNode);
      }
      return nodesToDelete;
    }

    private void GetNodeToDeleteFromStructure(
      StructureNodeDTO nodeToDelete,
      ObservableCollection<StructureNodeDTO> collectionToSearch,
      ref StructureNodeDTO foundNode)
    {
      foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) collectionToSearch)
      {
        if (nodeToDelete != structureNodeDto)
        {
          this.GetNodeToDeleteFromStructure(nodeToDelete, structureNodeDto.SubNodes, ref foundNode);
        }
        else
        {
          foundNode = structureNodeDto;
          break;
        }
      }
    }

    private void DeleteSelectedNodes(List<StructureNodeDTO> selectedNodesToDelete)
    {
      List<StructureNodeDTO> descendants = new List<StructureNodeDTO>();
      selectedNodesToDelete.ForEach((Action<StructureNodeDTO>) (item => descendants.AddRange(StructuresHelper.Descendants(item))));
      bool? shouldContinueWithDeletion = new bool?(true);
      if (!selectedNodesToDelete.All<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Id == Guid.Empty)) || !descendants.All<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.Id == Guid.Empty)))
        Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => shouldContinueWithDeletion = MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning_DeleteStructure_Title.GetStringValue(), MessageCodes.Warning_DeletePhysicalNodeInReadingOrder_Message.GetStringValue(), true)));
      if (!shouldContinueWithDeletion.HasValue || !shouldContinueWithDeletion.Value)
        return;
      selectedNodesToDelete.ForEach((Action<StructureNodeDTO>) (nodeToDelete =>
      {
        if (!this._updatedForReadingOrder)
          this.GetRelatedPhysicalAndLogicalNodes(nodeToDelete);
        else
          this.RemovePhysicalNodeInReadingOrder(nodeToDelete);
      }));
    }

    private void GetRelatedPhysicalAndLogicalNodes(StructureNodeDTO selectedNode)
    {
      this._physicalNodesDTOToDelete.Add(selectedNode);
      this.GetAffectedStructureNodesToDelete(selectedNode);
      this.RemoveSelectedNodeFromStructure(selectedNode, this._nodesCollection);
      this.RemoveSerialNumberFromUniquenessList(selectedNode);
    }

    private void RemovePhysicalNodeInReadingOrder(StructureNodeDTO selectedNode)
    {
      this._physicalNodesDTOToDelete.Add(selectedNode);
      this.RemoveSelectedNodeFromStructure(selectedNode, this._nodesCollection);
      this.RemoveSerialNumberFromUniquenessList(selectedNode);
    }

    public override ICommand CancelWindowCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (Delegate =>
        {
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
          if (!this.wasSaveExecuted)
          {
            message.MessageType = MessageTypeEnum.Warning;
            message.MessageText = MessageCodes.OperationCancelled.GetStringValue();
          }
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
          if (param is RadMenuItem radMenuItem2)
          {
            RadContextMenu parent = (RadContextMenu) radMenuItem2.Parent;
            if (parent != null)
            {
              StructureNodeDTO node = (StructureNodeDTO) parent.GetClickedElement<TreeListViewRow>().Item;
              if (node != null)
                this.EditSelectedStructureNode(node, true);
            }
          }
          this.OnPropertyChanged("StructureForSelectedNode");
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

    public ICommand ImportRadioMetersCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (!(this.SelectedItem.NodeType.Name == "Radio"))
            return;
          Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
          OpenFileDialog openFileDialog = new OpenFileDialog()
          {
            Filter = "CSV Document|*.csv|XML Document|*.xml|Xcel Document|*.xlsx",
            Title = Resources.MSS_Client_ImportStructureFromFile,
            RestoreDirectory = true
          };
          bool? nullable = openFileDialog.ShowDialog();
          if (nullable.HasValue && nullable.Value)
          {
            this.MessageUserControl = MessageHandlingManager.ShowSuccessMessage(MessageCodes.Success_Operation.GetStringValue());
            if (openFileDialog.FileName == string.Empty)
              return;
            this.ImportRadioMeters(openFileDialog.FileName);
            this.UpdateDevicesFoundLabel();
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(MessageCodes.OperationCancelled.GetStringValue());
        }));
      }
    }

    public ICommand ImportDeliveryNoteCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ => this.ImportDeliveryNote(this.StructureForSelectedNode)));
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
          this._systemName = this.StructureEquipmentSettings.SystemName;
          bool flag = this._scannerMinoConnectManager.StartScan();
          ScanMinoConnectManager.IsScanningStarted = true;
          this.IsStartMBusScanButtonEnabled = false;
          if (flag)
            return;
          this._scannerMinoConnectManager.StopScan();
          ScanMinoConnectManager.IsScanningStarted = false;
          this.IsStartMBusScanButtonEnabled = true;
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
          ScanMinoConnectManager.IsScanningStarted = false;
          this.IsStartMBusScanButtonEnabled = true;
          this.UpdateDevicesFoundLabel();
        }));
      }
    }

    public ICommand PhotosCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (async _ =>
        {
          this.IsBusy = true;
          await Task.Delay(100);
          int num = await Task.Run((Action) (() =>
          {
            if (this.SelectedItem.Id == Guid.Empty)
            {
              Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Order_Warning_Title, Resources.MSS_Client_Photos_MeterMustBeSavedFirst, false)));
            }
            else
            {
              if (this.SelectedItem.AssignedPicture == null)
                this.SelectedItem.AssignedPicture = this._repositoryFactory.GetRepository<PhotoMeter>().SearchFor((Expression<Func<PhotoMeter, bool>>) (p => p.StructureNode.Id == this.SelectedItem.Id)).Select<PhotoMeter, byte[]>((Func<PhotoMeter, byte[]>) (p => p.Payload)).ToList<byte[]>();
              Application.Current.Dispatcher.InvokeAsync<bool?>((Func<bool?>) (() => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MeterPhotosViewModel>((IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("selectedStructureNode", (object) this.SelectedItem)))));
            }
          })).ContinueWith<bool>((Func<Task, bool>) (v => this.IsBusy = false), System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext()) ? 1 : 0;
        }));
      }
    }

    public ICommand NotesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (async _ =>
        {
          this.IsBusy = true;
          await Task.Delay(100);
          await Task.Run((Action) (() =>
          {
            if (this.SelectedItem.AssignedNotes == null)
              this.SelectedItem.AssignedNotes = this._repositoryFactory.GetRepository<Note>().SearchFor((Expression<Func<Note, bool>>) (n => n.StructureNode.Id == this.SelectedItem.Id)).ToList<Note>();
            Application.Current.Dispatcher.InvokeAsync<bool?>((Func<bool?>) (() => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MeterNotesViewModel>((IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("selectedStructureNode", (object) this.SelectedItem), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory)))));
          }));
          this.IsBusy = false;
        }));
      }
    }

    private void ScannerMinoConnectManagerOnMeterFound(object sender, ZENNER.CommonLibrary.Entities.Meter e)
    {
      Application.Current.Dispatcher.Invoke((Action) (() =>
      {
        try
        {
          this.AddDevicesToPhysicalStructure(e, this._systemName);
          this.GetMetersInStructure(this._selectedNode, out this._metersInStructure);
          this.DevicesFoundLabel = Resources.MSS_DevicesFound + " " + (object) this._metersInStructure;
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
      ScanMinoConnectManager.IsScanningStarted = false;
      this.IsStartMBusScanButtonEnabled = true;
      this.UpdateDevicesFoundLabel();
    }

    private void SetParentAndRootNode(StructureNodeDTO selectedItem)
    {
      if (selectedItem.ParentNode != null || selectedItem.RootNode != null || this._parentForSelectedNode == null)
        return;
      StructureNodeDTO structureNodeDto = this._parentForSelectedNode.RootNode != this._parentForSelectedNode ? this._parentForSelectedNode.RootNode : this._parentForSelectedNode;
      selectedItem.ParentNode = this._parentForSelectedNode;
      selectedItem.RootNode = structureNodeDto;
    }

    private void GetAffectedStructureNodesToDelete(StructureNodeDTO selectedNode)
    {
      List<StructureNodeLinks> structureNodeLinks;
      List<StructureNodeLinks> logicalStructureNodeLinks;
      List<StructureNode> structureNodes;
      this.GetStructuresManagerInstance().GetAffectedPhysicalStructureNodes(selectedNode, StructureTypeEnum.Physical, out structureNodeLinks, out logicalStructureNodeLinks, out structureNodes);
      structureNodeLinks.ForEach(new Action<StructureNodeLinks>(this._physicalStructureNodeLinksToDelete.Add));
      logicalStructureNodeLinks.ForEach(new Action<StructureNodeLinks>(this._logicalStructureNodeLinksToDelete.Add));
      structureNodes.ForEach(new Action<StructureNode>(this._structureNodesToDelete.Add));
    }

    public override ObservableCollection<StructureNodeDTO> GetStructureCollection()
    {
      return this._nodesCollection;
    }

    protected override void UpdateDevicesFoundLabel()
    {
      this.GetMetersInStructure(this._selectedNode, out this._metersInStructure);
      this.DevicesFoundLabel = Resources.MSS_DevicesFound + " " + (object) this._metersInStructure;
    }

    private void InitStructureForSelectedNode()
    {
      StructureImageHelper.SetImageIconPath(this._nodesCollection);
      this.LoadPhotosAndNotes(this._nodesCollection);
    }

    public void OnTreeChange(TreeDragDropChange obj) => this.UpdateDevicesFoundLabel();

    private void LoadPhotosAndNotes(
      ObservableCollection<StructureNodeDTO> nodeCollection)
    {
      foreach (StructureNodeDTO node in (Collection<StructureNodeDTO>) nodeCollection)
      {
        this.LoadPhotosAndNotesForNode(node);
        this.WalkSubnodesAndLoadPhotosAndNotes(node);
      }
    }

    private void WalkSubnodesAndLoadPhotosAndNotes(StructureNodeDTO node)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) node.SubNodes)
      {
        this.LoadPhotosAndNotesForNode(subNode);
        this.WalkSubnodesAndLoadPhotosAndNotes(subNode);
      }
    }

    private void LoadPhotosAndNotesForNode(StructureNodeDTO node)
    {
      node.AssignedPicture = (List<byte[]>) null;
      node.AssignedNotes = new List<Note>();
      if (!(node.Id != Guid.Empty) || !(node.NodeType.Name == "Meter") && !(node.NodeType.Name == "RadioMeter"))
        return;
      node.AssignedPicture = this._photoMeterRepository.SearchFor((Expression<Func<PhotoMeter, bool>>) (item => item.StructureNode.Id == node.Id)).ToList<PhotoMeter>().Select<PhotoMeter, byte[]>((Func<PhotoMeter, byte[]>) (x => x.Payload)).ToList<byte[]>();
      node.AssignedNotes = this._noteRepository.SearchFor((Expression<Func<Note, bool>>) (item => item.StructureNode.Id == node.Id)).ToList<Note>();
      StructureNodeDTO structureNodeDto1 = node;
      StructureNodeDTO structureNodeDto2 = node;
      int num1;
      if (structureNodeDto2 == null)
      {
        num1 = 0;
      }
      else
      {
        int? count = structureNodeDto2.AssignedPicture?.Count;
        int num2 = 0;
        num1 = count.GetValueOrDefault() > num2 ? (count.HasValue ? 1 : 0) : 0;
      }
      List<byte[]> assignedPicture = num1 != 0 ? node.AssignedPicture : (List<byte[]>) null;
      structureNodeDto1.AssignedPicture = assignedPicture;
    }

    private void UpdateTreeWithMeterNotes(MeterNotesUpdated update)
    {
      if (update.UpdatedNode == null)
        return;
      StructureNodeDTO structureNodeDto = update.UpdatedNode.NodeType.Name == "Meter" || update.UpdatedNode.NodeType.Name == "RadioMeter" ? update.UpdatedNode : this.GetMeterById(this._nodesCollection.First<StructureNodeDTO>(), update.UpdatedNode.Id);
      if (structureNodeDto != null)
      {
        structureNodeDto.AssignedNotes = update.NewNotesList;
        this.OnPropertyChanged("StructureForSelectedNode");
      }
    }

    private void UpdateTreeWithMeterPhotos(MeterPhotosUpdated update)
    {
      if (update.UpdatedNode == null)
        return;
      StructureNodeDTO meterById = this.GetMeterById(this._nodesCollection.First<StructureNodeDTO>(), update.UpdatedNode.Id);
      if (meterById != null)
      {
        meterById.AssignedPicture = update.NewPhotos;
        this.OnPropertyChanged("StructureForSelectedNode");
      }
    }

    private StructureNodeDTO GetMeterById(StructureNodeDTO root, Guid idToFind)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) root.SubNodes)
      {
        if (subNode.Id != Guid.Empty && subNode.Id == idToFind)
          return subNode;
        this.GetMeterById(subNode, idToFind);
      }
      return (StructureNodeDTO) null;
    }

    private void GetMeterNodesAndStructureNodes(
      StructureNodeDTO rootNode,
      ref List<StructureNodeDTO> structureNodesForMeters,
      ref List<StructureNodeDTO> structureNodes)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) rootNode.SubNodes)
      {
        if (subNode.NodeType.Name == "Meter" && subNode.Entity is MeterDTO entity && entity.Id != Guid.Empty)
          structureNodesForMeters.Add(subNode);
        structureNodes.Add(subNode);
        this.GetMeterNodesAndStructureNodes(subNode, ref structureNodesForMeters, ref structureNodes);
      }
    }
  }
}
