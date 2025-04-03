// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.CreatePhysicalStructureViewModel
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
  public class CreatePhysicalStructureViewModel : StructureViewModelBase
  {
    private ObservableCollection<StructureNodeDTO> _nodesCollection = new ObservableCollection<StructureNodeDTO>();
    private ScanMinoConnectManager _scannerMinoConnectManager;
    private int _metersInStructures;
    private string _systemName;
    private Dictionary<StructureNodeDTO, StructureNodeEquipmentSettings> _equipmentSettingsForMeters = new Dictionary<StructureNodeDTO, StructureNodeEquipmentSettings>();
    private IRepository<PhotoMeter> _photoMeterRepository;
    private IRepository<Note> _noteRepository;
    private bool _canViewMeter;
    private bool _canAddMeter;
    private bool _canEditMeter;
    private bool _canDeleteMeter;
    private readonly IDeviceManager _deviceManager;
    private string _progressDialogMessage;
    private int _progressBarValue;
    private bool _isPasteActive;
    private bool _isPhotosButtonActive;
    private StructureNodeDTO _savedInClipBoardStructureNodeDto;
    private bool _mBusScanNetworkVisibility;

    public CreatePhysicalStructureViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      this._photoMeterRepository = this._repositoryFactory.GetRepository<PhotoMeter>();
      this._noteRepository = this._repositoryFactory.GetRepository<Note>();
      this.AvailableNodesGroups = new ObservableCollection<MSS.Business.Modules.StructuresManagement.Group>();
      MSS.Business.Modules.StructuresManagement.Group group = new MSS.Business.Modules.StructuresManagement.Group()
      {
        Name = CultureResources.GetValue("MSS_Available_Nodes_Folder")
      };
      StructuresHelper.LoadItemsInGroup(group, this.AvailableNodesSettingsGroup);
      this.AvailableNodesGroups.Add(group);
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>();
      Mapper.CreateMap<MSS.Core.Model.Meters.Meter, MeterDTO>();
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(((StructureViewModelBase) this).UpdateEntities));
      EventPublisher.Register<ActionSyncFinished>(new Action<ActionSyncFinished>(((StructureViewModelBase) this).ShowActionSyncFinished));
      EventPublisher.Register<TreeDragDropChange>(new Action<TreeDragDropChange>(this.OnTreeChange));
      EventPublisher.Register<MeterNotesUpdated>(new Action<MeterNotesUpdated>(this.UpdateTreeWithMeterNotes));
      EventPublisher.Register<MeterPhotosUpdated>(new Action<MeterPhotosUpdated>(this.UpdateTreeWithMeterPhotos));
      this.DragDropAttachedProp = new DragDropAttachedObject()
      {
        IsEnabled = true,
        PhysicalLinks = new List<StructureNodeLinks>()
      };
      this._deviceManager = (IDeviceManager) new DeviceManagerWrapper("DefaultScanner");
      StructureViewModelBase.deviceModelsInLicense = LicenseHelper.GetDeviceTypes().ToList<string>();
      this.InitStructureNodesCollection();
      this.StructureEquipmentSettings = new StructureNodeEquipmentSettings();
      this.UpdateDevicesFoundLabel();
      UsersManager usersManager = new UsersManager(repositoryFactory);
      this.MBusScanNetworkVisibility = usersManager.HasRight(OperationEnum.MbuScanNetwork.ToString());
      this._canViewMeter = usersManager.HasRight(OperationEnum.MeterView.ToString());
      this._canAddMeter = usersManager.HasRight(OperationEnum.MeterAdd.ToString());
      this._canDeleteMeter = usersManager.HasRight(OperationEnum.MeterDelete.ToString());
      this._canEditMeter = usersManager.HasRight(OperationEnum.MeterEdit.ToString());
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

    public ObservableCollection<StructureNodeDTO> StructureNodeCollection
    {
      get => this._nodesCollection;
      set
      {
        this._nodesCollection = value;
        this.OnPropertyChanged(nameof (StructureNodeCollection));
      }
    }

    public StructureNodeDTO SelectedStructureNode { get; set; }

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

    public bool IsPasteActive
    {
      get => this._isPasteActive;
      set
      {
        this._isPasteActive = value;
        this.OnPropertyChanged(nameof (IsPasteActive));
      }
    }

    public bool IsPhotosButtonActive
    {
      get => this._isPhotosButtonActive;
      set
      {
        this._isPhotosButtonActive = value;
        this.OnPropertyChanged(nameof (IsPhotosButtonActive));
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
                if (this._equipmentSettingsForMeters.Any<KeyValuePair<StructureNodeDTO, StructureNodeEquipmentSettings>>() && this._equipmentSettingsForMeters[this.SelectedItem] != null)
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
          radTreeListView.SelectedItem = (object) null;
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
              this.StructureNodeCollection.Add(destination);
            }
            else
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
          }
          else if (structureNodesValidator.IsValidNodesRelationship(structureNodeDto, this.SelectedItem, false))
          {
            this.SelectedItem.SubNodes.Add(destination);
            this.OnPropertyChanged("StructureNodeCollection");
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
          radTreeListView2.ExpandAllHierarchyItems();
          DataItemCollection items = radTreeListView2.Items;
          if (items.Count > 0)
          {
            ObservableCollection<StructureNodeDTO> nodeCollection = new ObservableCollection<StructureNodeDTO>(items.Cast<StructureNodeDTO>());
            if (this.ContinueActionIfMBusIsStarted(this._scannerMinoConnectManager) && this.ReconstructCollectionWithoutInvalidMBusScannerMeters(nodeCollection))
            {
              if (this.RemoveDuplicateAndNonExistentSerialNumbers(ref nodeCollection))
                MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_DeleteStructure_Warning_Title, Resources.MSS_Client_DuplicateMetersWillNotBeSaved, false);
              nodeCollection.SetNodesOrderNumber();
              foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) nodeCollection)
              {
                if (structureNodeDto.Entity is MeterDTO && !((MeterDTO) structureNodeDto.Entity).ConfigDate.HasValue)
                  ((MeterDTO) structureNodeDto.Entity).ConfigDate = new DateTime?(DateTime.Now);
              }
              this.GetStructuresManagerInstance().TransactionalSaveNewPhysicalStructure((IList<StructureNodeDTO>) nodeCollection, this.StructureEquipmentSettings);
              MSS.DTO.Message.Message message = new MSS.DTO.Message.Message()
              {
                MessageType = MessageTypeEnum.Success,
                MessageText = MessageCodes.Success_Save.GetStringValue()
              };
              foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) nodeCollection)
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
                ISession session = this._repositoryFactory.GetSession();
                session.BeginTransaction();
                repository.TransactionalInsertMany((IEnumerable<StructureNodeEquipmentSettings>) this._equipmentSettingsForMeters.Values.ToList<StructureNodeEquipmentSettings>());
                session.Transaction.Commit();
                session.Clear();
              }
              this.OnRequestClose(true);
            }
          }
          else
            this.OnRequestClose(false);
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

    public ICommand EditSelectedItemCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          RadContextMenu parent = parameter is RadMenuItem radMenuItem2 ? (RadContextMenu) radMenuItem2.Parent : (RadContextMenu) null;
          if (parent == null)
            return;
          TreeListViewRow clickedElement = parent.GetClickedElement<TreeListViewRow>();
          this.EditSelectedStructureNode((StructureNodeDTO) clickedElement.Item);
          RadTreeListView radTreeListView = clickedElement.ParentOfType<RadTreeListView>();
          this._nodesCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (object obj in radTreeListView.Items)
          {
            if (((StructureNodeDTO) obj).ParentNode == null)
              this._nodesCollection.Add((StructureNodeDTO) obj);
          }
          this.OnPropertyChanged("StructureNodeCollection");
        }));
      }
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
            this.EditSelectedStructureNode(selectedItem);
          this._nodesCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (object obj in radTreeListView.Items)
          {
            if (((StructureNodeDTO) obj).ParentNode == null)
              this._nodesCollection.Add((StructureNodeDTO) obj);
          }
          this.OnPropertyChanged("StructureNodeCollection");
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
        this.GetNodeToDeleteFromStructure((StructureNodeDTO) selectedItem, this.StructureNodeCollection, ref foundNode);
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
      selectedNodesToDelete.ForEach((Action<StructureNodeDTO>) (nodeToDelete =>
      {
        this.RemoveSelectedNodeFromStructure(nodeToDelete, this._nodesCollection);
        this.RemoveSerialNumberFromUniquenessList(nodeToDelete);
      }));
      this.OnPropertyChanged("StructureNodeCollection");
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
        return (ICommand) new RelayCommand((Action<object>) (_ => this.ImportDeliveryNote(this.StructureNodeCollection)));
      }
    }

    public ICommand PhotosCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (_ =>
        {
          if (this.SelectedItem.Id == Guid.Empty)
            Application.Current.Dispatcher.Invoke<bool?>((Func<bool?>) (() => MSSUIHelper.ShowWarningDialog(this._windowFactory, Resources.MSS_Order_Warning_Title, Resources.MSS_Client_Photos_MeterMustBeSavedFirst, false)));
          else
            this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MeterPhotosViewModel>((IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("selectedStructureNode", (object) this.SelectedItem)));
        }));
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
          EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
          {
            Guid = Guid.Empty,
            Message = message
          }, (IViewModel) this);
          this.OnRequestClose(false);
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
          this._scannerMinoConnectManager.OnMissingTranslationRule += new EventHandler<string>(((StructureViewModelBase) this).OnMissingTranslationRule);
          this._systemName = this.StructureEquipmentSettings.SystemName;
          bool flag = this._scannerMinoConnectManager.StartScan();
          this.IsStartMBusScanButtonEnabled = false;
          ScanMinoConnectManager.IsScanningStarted = true;
          if (flag)
            return;
          this._scannerMinoConnectManager.StopScan();
          ScanMinoConnectManager.IsScanningStarted = false;
          this.IsStartMBusScanButtonEnabled = true;
          MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning.GetStringValue(), Resources.Warning_ScanSettingsNotSet, false);
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
          this.UpdateDevicesFoundLabel();
          this.RemoveSerialNumberOfMeterThatIsMissingTranslationRulesFromList(e.SerialNumber);
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

    public ICommand NotesCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (async _ =>
        {
          await Task.Delay(100);
          await Task.Run((Action) (() =>
          {
            if (this.SelectedItem.AssignedNotes == null)
              this.SelectedItem.AssignedNotes = new List<Note>();
            Application.Current.Dispatcher.InvokeAsync<bool?>((Func<bool?>) (() => this._windowFactory.CreateNewModalDialog((IViewModel) DIConfigurator.GetConfigurator().Get<MeterNotesViewModel>((IParameter) new ConstructorArgument("windowFactory", (object) this._windowFactory), (IParameter) new ConstructorArgument("selectedStructureNode", (object) this.SelectedItem), (IParameter) new ConstructorArgument("repositoryFactory", (object) this._repositoryFactory)))));
          }));
        }));
      }
    }

    protected override sealed void UpdateDevicesFoundLabel()
    {
      this._metersInStructures = 0;
      if (this.StructureNodeCollection != null && this.StructureNodeCollection.Count > 0)
      {
        foreach (StructureNodeDTO structureNode in (Collection<StructureNodeDTO>) this.StructureNodeCollection)
        {
          int meters;
          this.GetMetersInStructure(structureNode, out meters);
          this._metersInStructures += meters;
        }
      }
      this.DevicesFoundLabel = Resources.MSS_DevicesFound + " " + (object) this._metersInStructures;
    }

    private void InitStructureNodesCollection()
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
      List<Note> noteList = new List<Note>();
      foreach (Note assignedNote in update.UpdatedNode.AssignedNotes)
      {
        if (!update.NewNotesList.Contains(assignedNote))
          noteList.Add(assignedNote);
      }
      StructureNodeDTO structureNodeDto = update.UpdatedNode.NodeType.Name == "Meter" || update.UpdatedNode.NodeType.Name == "RadioMeter" ? update.UpdatedNode : this.GetMeterById(this._nodesCollection.First<StructureNodeDTO>(), update.UpdatedNode.Id);
      if (structureNodeDto != null)
      {
        noteList.ForEach((Action<Note>) (item => this._noteRepository.Delete(item)));
        structureNodeDto.AssignedNotes = update.NewNotesList;
        this.OnPropertyChanged("StructureNodeCollection");
      }
    }

    private void UpdateTreeWithMeterPhotos(MeterPhotosUpdated update)
    {
      if (update.UpdatedNode == null)
        return;
      StructureNodeDTO meterById = this.GetMeterById(this._nodesCollection.FirstOrDefault<StructureNodeDTO>(), update.UpdatedNode.Id);
      if (meterById != null)
      {
        meterById.AssignedPicture = update.NewPhotos;
        this.OnPropertyChanged("StructureNodeCollection");
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
  }
}
