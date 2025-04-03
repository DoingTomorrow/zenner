// Decompiled with JetBrains decompiler
// Type: MSS_Client.ViewModel.Structures.CreateLogicalStructureViewModel
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
  public class CreateLogicalStructureViewModel : StructureViewModelBase
  {
    private ObservableCollection<StructureNodeDTO> _nodesCollection = new ObservableCollection<StructureNodeDTO>();
    private bool _isPasteActive;
    private StructureNodeDTO _savedInClipBoardStructureNodeDto;

    public CreateLogicalStructureViewModel(
      IRepositoryFactory repositoryFactory,
      IWindowFactory windowFactory)
      : base(repositoryFactory, windowFactory)
    {
      Mapper.CreateMap<Meter, MeterDTO>();
      Mapper.CreateMap<StructureNodeDTO, StructureNodeDTO>().ForMember((Expression<Func<StructureNodeDTO, object>>) (x => x.SubNodes), (Action<IMemberConfigurationExpression<StructureNodeDTO>>) (y => y.Ignore()));
      EventPublisher.Register<ActionStructureAndEntitiesUpdate>(new Action<ActionStructureAndEntitiesUpdate>(((StructureViewModelBase) this).UpdateEntities));
      this.Groups = new ObservableCollection<StructureNodeDTO>();
      StructureNodeType nodeType = this._structureNodeTypeRepository.FirstOrDefault((Expression<Func<StructureNodeType, bool>>) (s => s.Name == StructureNodeTypeEnum.Folder.GetStringValue()));
      if (nodeType != null)
      {
        StructureNodeDTO structureNodeDto1 = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_Available_Nodes_Folder"), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Logical);
        foreach (StructureNodeDTO logicalNode in (Collection<StructureNodeDTO>) this.LogicalNodeCollection)
          structureNodeDto1.SubNodes.Add(logicalNode);
        this.Groups.Add(structureNodeDto1);
        StructureNodeDTO structureNodeDto2 = new StructureNodeDTO(Guid.Empty, CultureResources.GetValue("MSS_Physical_Nodes_Folder"), new BitmapImage(new Uri(nodeType.IconPath)), nodeType, "", (object) null, true, StructureTypeEnum.Physical);
        foreach (StructureNodeDTO physicalStructureNode in (Collection<StructureNodeDTO>) this.PhysicalStructureNodeCollection)
          structureNodeDto2.SubNodes.Add(physicalStructureNode);
        this.Groups.Add(structureNodeDto2);
      }
      this.DragDropAttachedProp = new DragDropAttachedObject()
      {
        IsEnabled = true,
        PhysicalLinks = new List<StructureNodeLinks>()
      };
    }

    public DragDropAttachedObject DragDropAttachedProp { get; set; }

    public ObservableCollection<StructureNodeDTO> Groups { get; private set; }

    public ObservableCollection<StructureNodeDTO> LogicalNodeCollection
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
                this.LogicalStructureNodeCollection.Add(destination);
              }
              else
                this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
            }
            else if (structureNodesValidator.IsValidNodesRelationship(structureNodeDto, this.SelectedStructureNode, false))
            {
              this.SelectedStructureNode.SubNodes.Add(destination);
              this.OnPropertyChanged("LogicalStructureNodeCollection");
            }
            else
              this.MessageUserControl = MessageHandlingManager.ShowValidationMessage(Resources.MSS_Client_Structure_Validation_Message);
          }
          else
            this.MessageUserControl = MessageHandlingManager.ShowWarningMessage(Resources.MSS_Client_CannotMoveSubMeter);
        }));
      }
    }

    public ObservableCollection<StructureNodeDTO> PhysicalStructureNodeCollection
    {
      get
      {
        return this.GetStructuresManagerInstance().GetStructureNodesCollection(StructureTypeEnum.Physical);
      }
    }

    public ObservableCollection<StructureNodeDTO> LogicalStructureNodeCollection
    {
      get
      {
        StructureImageHelper.SetImageIconPath(this._nodesCollection);
        return this._nodesCollection;
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
          this.GetStructuresManagerInstance().TransactionalSaveNewLogicalStructure((IList<StructureNodeDTO>) nodeCollection);
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
          this.OnRequestClose(false);
        }));
      }
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
          ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (object obj in radTreeListView.Items)
            observableCollection.Add((StructureNodeDTO) obj);
          this._nodesCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) observableCollection)
          {
            if (structureNodeDto.ParentNode == null)
              this._nodesCollection.Add(structureNodeDto);
          }
          this.OnPropertyChanged("LogicalStructureNodeCollection");
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
          ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (object obj in radTreeListView.Items)
            observableCollection.Add((StructureNodeDTO) obj);
          this._nodesCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) observableCollection)
          {
            if (structureNodeDto.ParentNode == null || structureNodeDto.ParentNode != null && structureNodeDto.ParentNode.Name == CultureResources.GetValue("MSS_Available_Nodes_Folder"))
              this._nodesCollection.Add(structureNodeDto);
          }
          this.OnPropertyChanged("LogicalStructureNodeCollection");
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
          ObservableCollection<StructureNodeDTO> observableCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (object obj in radTreeListView.Items)
            observableCollection.Add((StructureNodeDTO) obj);
          this._nodesCollection = new ObservableCollection<StructureNodeDTO>();
          foreach (StructureNodeDTO structureNodeDto in (Collection<StructureNodeDTO>) observableCollection)
          {
            if (structureNodeDto.ParentNode == null)
              this._nodesCollection.Add(structureNodeDto);
          }
          foreach (StructureNodeDTO selectedNode in this.GetNodesToDelete(radTreeListView))
          {
            this.RemoveSelectedNodeFromStructure(selectedNode, this._nodesCollection);
            this.RemoveSerialNumberFromUniquenessList(selectedNode);
          }
          this.OnPropertyChanged("LogicalStructureNodeCollection");
        }));
      }
    }

    private List<StructureNodeDTO> GetNodesToDelete(RadTreeListView radTreeListView)
    {
      List<StructureNodeDTO> nodesToDelete = new List<StructureNodeDTO>();
      foreach (object selectedItem in (Collection<object>) radTreeListView.SelectedItems)
      {
        StructureNodeDTO foundNode = (StructureNodeDTO) null;
        this.GetNodeToDeleteFromStructure((StructureNodeDTO) selectedItem, this.LogicalStructureNodeCollection, ref foundNode);
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
  }
}
