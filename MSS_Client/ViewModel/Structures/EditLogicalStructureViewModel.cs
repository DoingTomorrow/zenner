// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.EditLogicalStructureViewModel
// Assembly: MSS_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04E68651-4483-4E77-961C-A6C12FC6E4D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS_Client.exe

using AutoMapper;
using MSS.Business.DTO;
using MSS.Business.Errors;
using MSS.Business.Events;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Meters;
using MSS.Core.Model.Structures;
using MSS.DTO.MessageHandler;
using MSS.DTO.Meters;
using MSS.DTO.Structures;
using MSS.Interfaces;
using MSS.Localisation;
using MSS.Utils.Utils;
using MSS_Client.Utils;
using MVVM.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeListView;

#nullable disable
namespace MSS_Client.ViewModel.Structures
{
  public class EditLogicalStructureViewModel : StructureViewModelBase
  {
    private readonly StructureNodeDTO _parentForSelectedNode;
    private readonly StructureNodeDTO _selectedNode;
    private bool _updatedForReadingOrder;
    private ObservableCollection<StructureNodeDTO> _nodesCollection = new ObservableCollection<StructureNodeDTO>();
    private List<StructureNodeLinks> _physicalStructureNodeLinksToDelete = new List<StructureNodeLinks>();
    private List<StructureNodeLinks> _logicalStructureNodeLinksToDelete = new List<StructureNodeLinks>();
    private List<StructureNode> _structureNodesToDelete = new List<StructureNode>();
    private ObservableCollection<StructureNodeDTO> _physicalNodesDTOToDelete = new ObservableCollection<StructureNodeDTO>();
    private ObservableCollection<StructureNodeDTO> _logicalNodesDTOToDelete = new ObservableCollection<StructureNodeDTO>();
    private bool _isPasteActive;
    private StructureNodeDTO _savedInClipBoardStructureNodeDto;

    public EditLogicalStructureViewModel(
      StructureNodeDTO selectedNode,
      bool updatedForReadingOrder,
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      this._selectedNode = selectedNode;
      this._updatedForReadingOrder = updatedForReadingOrder;
      this._nodesCollection.Add(this._selectedNode);
      this._parentForSelectedNode = selectedNode.ParentNode;
      Mapper.CreateMap<Meter, MeterDTO>();
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(((StructureViewModelBase) this).UpdateEntities));
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>().ForMember((Expression<Func<StructureNodeDTO, object>>) (x => x.SubNodes), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (y => y.Ignore()));
      this.Groups = new ObservableCollection<StructureNodeDTO>();
      StructureNodeType nodeType = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (s => s.Name == StructureNodeTypeEnum.Folder.GetStringValue()));
      if (nodeType != null)
      {
        StructureNodeDTO structureNodeDto1 = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_Available_Nodes_Folder"), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Logical);
        foreach (StructureNodeDTO logicalNodes in (Collection<StructureNodeDTO>) this.LogicalNodesCollection)
          structureNodeDto1.SubNodes.Add(logicalNodes);
        this.Groups.Add(structureNodeDto1);
        StructureNodeDTO structureNodeDto2 = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_Physical_Nodes_Folder"), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Physical);
        foreach (StructureNodeDTO physicalStructure in (Collection<StructureNodeDTO>) this.PhysicalStructureCollection)
          structureNodeDto2.SubNodes.Add(physicalStructure);
        this.Groups.Add(structureNodeDto2);
      }
      DragDropAttachedObject dropAttachedObject = new DragDropAttachedObject();
      dropAttachedObject.IsEnabled = true;
      dropAttachedObject.PhysicalLinks = this._structureNodeLinkRepository.SearchFor((Expression<Func<StructureNodeLinks, bool>>) (l => (int) l.StructureType == 0)).ToList<StructureNodeLinks>();
      this.DragDropAttachedProp = dropAttachedObject;
    }

    public ObservableCollection<StructureNodeDTO> Groups { get; private set; }

    public ObservableCollection<StructureNodeDTO> LogicalNodesCollection
    {
      get
      {
        List<StructureNodeDTO> list = new List<StructureNodeDTO>();
        IRepository<StructureNodeType> nodeTypeRepository = this._structureNodeTypeRepository;
        Expression<Func<StructureNodeType, bool>> predicate = (Expression<Func<StructureNodeType, bool>>) (s => s.IsLogical);
        foreach (StructureNodeType nodeType in (IEnumerable<StructureNodeType>) nodeTypeRepository.SearchFor(predicate))
        {
          StructureNodeDTO structureNodeDto = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_StructureNode_" + nodeType.Name), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Logical);
          list.Add(structureNodeDto);
        }
        return new ObservableCollection<StructureNodeDTO>(list);
      }
    }

    public ObservableCollection<StructureNodeDTO> PhysicalStructureCollection
    {
      get
      {
        return this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
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

    public DragDropAttachedObject DragDropAttachedProp { get; set; }

    public ICommand SaveStructureCommand
    {
      get
      {
        return (ICommand) new RelayCommand((Action<object>) (parameter =>
        {
          byte[] structureBytes = (byte[]) null;
          if (!(parameter is RadTreeListView radTreeListView2))
            return;
          MSS.DTO.Message.Message message = new MSS.DTO.Message.Message();
          radTreeListView2.ExpandAllHierarchyItems();
          ObservableCollection<StructureNodeDTO> observableCollection1 = new ObservableCollection<StructureNodeDTO>(radTreeListView2.Items.Cast<StructureNodeDTO>());
          observableCollection1.SetNodesOrderNumber(this._selectedNode, this._parentForSelectedNode);
          ObservableCollection<StructureNodeDTO> observableCollection2 = this.ReconstructNodeCollection(observableCollection1, this._parentForSelectedNode);
          if (this._logicalStructureNodeLinksToDelete.Count != 0 || this._structureNodesToDelete.Count != 0)
          {
            bool? deleteDialog = this.ShowWarningWithStructuresToDeleteDialog(this._logicalNodesDTOToDelete, new ObservableCollection<StructureNodeDTO>());
            if (deleteDialog.HasValue && deleteDialog.Value)
            {
              if (!this._updatedForReadingOrder)
                this.GetStructuresManagerInstance().TransactionalUpdateStructure((IList<StructureNodeDTO>) observableCollection2, StructureTypeEnum.Logical, (StructureNodeEquipmentSettings) null, (IList<StructureNodeLinks>) this._logicalStructureNodeLinksToDelete, (IList<StructureNode>) this._structureNodesToDelete);
              else
                this.CreateStructureBytes(observableCollection2, out structureBytes);
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
              this.GetStructuresManagerInstance().TransactionalUpdateStructure((IList<StructureNodeDTO>) observableCollection2, StructureTypeEnum.Logical, (StructureNodeEquipmentSettings) null);
            else
              this.CreateStructureBytes(observableCollection2, out structureBytes);
            message.MessageType = MessageTypeEnum.Success;
            message.MessageText = MessageCodes.Success_Save.GetStringValue();
          }
          if (!this._updatedForReadingOrder)
          {
            if (observableCollection1 != null && observableCollection1.Any<StructureNodeDTO>())
            {
              foreach (StructureNodeDTO structureNodeDto1 in (Collection<StructureNodeDTO>) observableCollection1)
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
              StructureNodeDTO structureNodeDto2 = observableCollection1.FirstOrDefault<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.ParentNode == null && n.RootNode == n));
              EventPublisher.Publish<StructureRootUpdated>(new StructureRootUpdated()
              {
                RootNodeId = structureNodeDto2 != null ? structureNodeDto2.Id : Guid.Empty
              }, (IViewModel) this);
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
          }
          else
            EventPublisher.Publish<StructureBytesUpdated>(new StructureBytesUpdated()
            {
              StructureBytes = structureBytes
            }, (IViewModel) this);
          this.OnRequestClose(true);
        }));
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
            this.SavedinClipStructureNodeDto.ParentNode.SubNodes.Remove(this.SavedinClipStructureNodeDto);
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
          if (!StructuresHelper.IsMeterWithMeterParent(structureNodeDto))
          {
            StructureNodeDTO destination = new StructureNodeDTO();
            StructureNodesValidator structureNodesValidator = new StructureNodesValidator();
            Mapper.Map<StructureNodeDTO, StructureNodeDTO>(structureNodeDto, destination);
            if (structureNodeDto != null)
              destination.SubNodes = structureNodeDto.SubNodes;
            if (this.SelectedStructureNode == null)
            {
              StructureTypeEnum? structureType = destination.StructureType;
              StructureTypeEnum structureTypeEnum = StructureTypeEnum.Logical;
              if (structureType.GetValueOrDefault() == structureTypeEnum && structureType.HasValue)
              {
                destination.RootNode = destination;
                this.LogicalNodesCollection.Add(destination);
              }
              else
                this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
            }
            else if (structureNodesValidator.IsValidNodesRelationship(structureNodeDto, this.SelectedStructureNode, false))
            {
              this.SelectedStructureNode.SubNodes.Add(destination);
              this.OnPropertyChanged("LogicalNodesCollection");
            }
            else
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_CannotMoveSubMeter);
        }));
      }
    }

    private void CreateStructureBytes(
      ObservableCollection<StructureNodeDTO> actualNodeCollection,
      out byte[] structureBytes)
    {
      this.GetStructuresManagerInstance().InsertEntitiesGuid(actualNodeCollection);
      this.GetStructuresManagerInstance().InsertStructureNodesGuid(actualNodeCollection);
      Structure structure = this.GetStructuresManagerInstance().GetStructure(actualNodeCollection);
      structureBytes = StructuresHelper.SerializeStructure(structure);
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
          EventPublisher.Publish<StructureUpdated>(new StructureUpdated()
          {
            Guid = Guid.Empty,
            Message = message
          }, (IViewModel) this);
          this.OnRequestClose(false);
        }));
      }
    }

    private void SetParentAndRootNode(StructureNodeDTO selectedItem)
    {
      if (selectedItem.ParentNode != null || selectedItem.RootNode != null || this._parentForSelectedNode == null)
        return;
      StructureNodeDTO structureNodeDto = this._parentForSelectedNode.RootNode != this._parentForSelectedNode ? this._parentForSelectedNode.RootNode : this._parentForSelectedNode;
      selectedItem.ParentNode = this._parentForSelectedNode;
      selectedItem.RootNode = structureNodeDto;
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
          this.SetParentAndRootNode(selectedItem);
          if (selectedItem == null)
            return;
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
          List<StructureNodeDTO> nodesToDelete = this.GetNodesToDelete(parameter as RadTreeListView);
          List<StructureNodeDTO> descendants = new List<StructureNodeDTO>();
          nodesToDelete.ForEach((Action<StructureNodeDTO>) (item => descendants.AddRange(StructuresHelper.Descendants(item))));
          if (nodesToDelete.All<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (item => item.Id == Guid.Empty)) && descendants.All<StructureNodeDTO>((Func<StructureNodeDTO, bool>) (n => n.Id == Guid.Empty)))
          {
            nodesToDelete.ForEach((Action<StructureNodeDTO>) (nodeToDelete =>
            {
              this.RemoveSelectedNodeFromStructure(nodeToDelete, this._nodesCollection);
              this.RemoveSerialNumberFromUniquenessList(nodeToDelete);
            }));
          }
          else
          {
            bool? nullable = MSSUIHelper.ShowWarningDialog(this._windowFactory, MessageCodes.Warning_DeleteStructure_Title.GetStringValue(), MessageCodes.Warning_DeleteLogicalStructure.GetStringValue(), true);
            if (nullable.HasValue && nullable.Value)
              nodesToDelete.ForEach((Action<StructureNodeDTO>) (nodeToDelete =>
              {
                this._logicalNodesDTOToDelete.Add(nodeToDelete);
                this.GetAffectedStructureNodesToDelete(nodeToDelete);
                this.RemoveSelectedNodeFromStructure(nodeToDelete, this._nodesCollection);
                this.RemoveSerialNumberFromUniquenessList(nodeToDelete);
              }));
          }
          this.OnPropertyChanged("StructureForSelectedNode");
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

    private void GetAffectedStructureNodesToDelete(StructureNodeDTO selectedNode)
    {
      List<StructureNodeLinks> structureNodeLinks;
      List<StructureNode> structureNodes;
      this.GetStructuresManagerInstance().GetLogicalStructureNodes(selectedNode, out structureNodeLinks, out structureNodes);
      structureNodeLinks.ForEach(new Action<StructureNodeLinks>(this._logicalStructureNodeLinksToDelete.Add));
      structureNodes.ForEach(new Action<StructureNode>(this._structureNodesToDelete.Add));
    }
  }
}
