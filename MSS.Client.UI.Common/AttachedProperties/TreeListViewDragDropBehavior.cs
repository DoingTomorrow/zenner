// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.AttachedProperties.TreeListViewDragDropBehavior
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Business.DTO;
using MSS.Business.Events;
using MSS.Business.Modules.StructuresManagement;
using MSS.Business.Utils;
using MSS.Core.Model.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;

#nullable disable
namespace MSS.Client.UI.Common.AttachedProperties
{
  public class TreeListViewDragDropBehavior
  {
    private static List<StructureNodeLinks> _physicalLinks = new List<StructureNodeLinks>();
    private static readonly Dictionary<RadTreeListView, TreeListViewDragDropBehavior> Instances;
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (DragDropAttachedObject), typeof (TreeListViewDragDropBehavior), new PropertyMetadata(new PropertyChangedCallback(TreeListViewDragDropBehavior.OnIsEnabledPropertyChanged)));
    private IList _destinationCollection;
    private bool _draggedFromTreelist;
    private object _originalSource;
    private StructureNodesValidator _physicalStructuresValidator = new StructureNodesValidator();
    private FixedStructureNodesValidator _fixedStructuresValidator = new FixedStructureNodesValidator();

    static TreeListViewDragDropBehavior()
    {
      TreeListViewDragDropBehavior.Instances = new Dictionary<RadTreeListView, TreeListViewDragDropBehavior>();
    }

    public TreeListViewDragDropBehavior() => this.SourceCollection = (IList) null;

    public IList SourceCollection { get; set; }

    public RadTreeListView AssociatedObject { get; set; }

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool) obj.GetValue(TreeListViewDragDropBehavior.IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, DragDropAttachedObject value)
    {
      TreeListViewDragDropBehavior attachedBehavior = TreeListViewDragDropBehavior.GetAttachedBehavior(obj as RadTreeListView);
      attachedBehavior.AssociatedObject = obj as RadTreeListView;
      if (value.IsEnabled)
        attachedBehavior.Initialize();
      else
        attachedBehavior.CleanUp();
      obj.SetValue(TreeListViewDragDropBehavior.IsEnabledProperty, (object) value);
    }

    public static void OnIsEnabledPropertyChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      TreeListViewDragDropBehavior._physicalLinks = ((DragDropAttachedObject) e.NewValue).PhysicalLinks;
      TreeListViewDragDropBehavior.SetIsEnabled(dependencyObject, (DragDropAttachedObject) e.NewValue);
    }

    public static TreeListViewDragDropBehavior GetAttachedBehavior(RadTreeListView treeListView)
    {
      if (TreeListViewDragDropBehavior.Instances.ContainsKey(treeListView))
        return TreeListViewDragDropBehavior.Instances[treeListView];
      TreeListViewDragDropBehavior.Instances[treeListView] = new TreeListViewDragDropBehavior()
      {
        AssociatedObject = treeListView
      };
      return TreeListViewDragDropBehavior.Instances[treeListView];
    }

    protected virtual void Initialize()
    {
      DragDropManager.AddDragInitializeHandler((DependencyObject) this.AssociatedObject, new DragInitializeEventHandler(this.OnDragInitialize), true);
      DragDropManager.AddDropHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDrop), true);
      DragDropManager.AddDragOverHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDragOver), true);
      this.AssociatedObject.DataLoaded += new EventHandler<EventArgs>(this.DataLoaded);
    }

    protected virtual void CleanUp()
    {
      DragDropManager.RemoveDragInitializeHandler((DependencyObject) this.AssociatedObject, new DragInitializeEventHandler(this.OnDragInitialize));
      DragDropManager.RemoveDropHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDrop));
      DragDropManager.RemoveDragOverHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDragOver));
      this.AssociatedObject.DataLoaded -= new EventHandler<EventArgs>(this.DataLoaded);
    }

    private void OnDragInitialize(object sender, DragInitializeEventArgs e)
    {
      if (!(e.OriginalSource is TreeListViewRow treeListViewRow1))
        treeListViewRow1 = (e.OriginalSource as FrameworkElement).ParentOfType<TreeListViewRow>();
      TreeListViewRow treeListViewRow2 = treeListViewRow1;
      if (treeListViewRow2 == null)
        return;
      this._draggedFromTreelist = true;
      object data = e.Data;
      DropIndicationDetails indicationDetails = new DropIndicationDetails();
      List<StructureNodeDTO> structureNodeDtoList = data as List<StructureNodeDTO>;
      indicationDetails.CurrentDraggedItem = structureNodeDtoList != null ? (object) structureNodeDtoList[0].Name : (object) string.Empty;
      DragInitializeEventArgs initializeEventArgs1 = e;
      DragInitializeEventArgs initializeEventArgs2 = e;
      DragVisual dragVisual1 = new DragVisual();
      dragVisual1.Content = (object) indicationDetails;
      dragVisual1.ContentTemplate = this.AssociatedObject.Resources[(object) "DraggedItemTemplate"] as DataTemplate;
      DragVisual dragVisual2;
      object obj1 = (object) (dragVisual2 = dragVisual1);
      initializeEventArgs2.DragVisual = (object) dragVisual2;
      object obj2 = obj1;
      initializeEventArgs1.DragVisual = obj2;
      e.DragVisualOffset = new Point(0.0, 0.0);
      IDragPayload payload = DragDropPayloadManager.GeneratePayload((DataConverter) null);
      DragDropPayloadManager.SetData((object) payload, "DraggedData", (object) new Collection<object>()
      {
        data
      });
      DragDropPayloadManager.SetData((object) payload, "DropDetails", (object) new Collection<object>()
      {
        (object) indicationDetails
      });
      e.Data = (object) payload;
      this._originalSource = treeListViewRow2.Item;
      this.SourceCollection = treeListViewRow2.ParentRow != null ? (IList) treeListViewRow2.ParentRow.Items.SourceCollection : (IList) treeListViewRow2.GridViewDataControl.Items;
    }

    private void OnDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      if (!(e.OriginalSource is TreeListViewRow treeListViewRow))
        treeListViewRow = (e.OriginalSource as FrameworkElement).ParentOfType<TreeListViewRow>();
      TreeListViewRow dropTarget = treeListViewRow;
      Collection<object> dataFromObject1 = DragDropPayloadManager.GetDataFromObject(e.Data, "DropDetails") as Collection<object>;
      Collection<object> dataFromObject2 = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData") as Collection<object>;
      if (dataFromObject1 == null)
        return;
      DropIndicationDetails indicationDetails = (DropIndicationDetails) dataFromObject1[0];
      if (dataFromObject2[0] is Group)
      {
        e.Effects = DragDropEffects.None;
      }
      else
      {
        List<StructureNodeDTO> structureNodeDtoList1 = new List<StructureNodeDTO>();
        if (dataFromObject2[0] is StructureNodeDTO)
          structureNodeDtoList1.Add(dataFromObject2[0] as StructureNodeDTO);
        else if (dataFromObject2[0] != null)
        {
          foreach (object obj in dataFromObject2[0] as Collection<object>)
            structureNodeDtoList1.Add(obj as StructureNodeDTO);
        }
        List<StructureNodeDTO> structureNodeDtoList2 = new List<StructureNodeDTO>();
        foreach (StructureNodeDTO structureNodeDto in structureNodeDtoList1)
          structureNodeDtoList2.Add(structureNodeDto);
        if ((structureNodeDtoList2.Any<StructureNodeDTO>() ? TreeListViewDragDropBehavior.GetRootNode(structureNodeDtoList2[0]) : (StructureNodeDTO) null) == null)
          this._draggedFromTreelist = false;
        StructureNodeDTO parentItem = (StructureNodeDTO) null;
        RadTreeListView radTreeListView = sender as RadTreeListView;
        if (dropTarget == null && radTreeListView == null || indicationDetails == null)
          return;
        if (dropTarget != null)
        {
          TreeListViewDropPosition viewDropPosition = (TreeListViewDropPosition) dropTarget.GetValue(RadTreeListView.DropPositionProperty);
          StructureNodeDTO dataContext = dropTarget.DataContext as StructureNodeDTO;
          indicationDetails.CurrentDraggedOverItem = dataContext != null ? (object) dataContext.Name : (object) string.Empty;
          indicationDetails.CurrentDropPosition = (DropPosition) dropTarget.GetValue(RadTreeListView.DropPositionProperty);
          indicationDetails.DropIndex = 1;
          parentItem = viewDropPosition != TreeListViewDropPosition.After && viewDropPosition != TreeListViewDropPosition.Before ? dataContext : dataContext?.ParentNode;
        }
        if (TreeListViewDragDropBehavior._physicalLinks.Any<StructureNodeLinks>())
        {
          if (TreeListViewDragDropBehavior._physicalLinks[0].StructureType == StructureTypeEnum.Fixed)
            this._fixedStructuresValidator.PhysicalLinks = TreeListViewDragDropBehavior._physicalLinks;
          else
            this._physicalStructuresValidator.PhysicalLinks = TreeListViewDragDropBehavior._physicalLinks;
        }
        int num;
        if (!TreeListViewDragDropBehavior.IsChildOf(dropTarget, this._originalSource) || !this._draggedFromTreelist)
        {
          StructureTypeEnum? structureType = structureNodeDtoList2[0].StructureType;
          StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
          num = !this.AreNodesValid((structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0 ? (StructureNodesValidator) this._fixedStructuresValidator : this._physicalStructuresValidator, structureNodeDtoList2, parentItem, this._draggedFromTreelist) ? 1 : 0;
        }
        else
          num = 1;
        if (num != 0)
          e.Effects = DragDropEffects.None;
      }
      e.Handled = true;
    }

    private bool AreNodesValid(
      StructureNodesValidator validator,
      List<StructureNodeDTO> draggedItems,
      StructureNodeDTO parentItem,
      bool draggedFromTreeList)
    {
      foreach (StructureNodeDTO draggedItem in draggedItems)
      {
        if (!validator.IsValidNodesRelationship(draggedItem, parentItem, draggedFromTreeList))
          return false;
      }
      return true;
    }

    private static StructureNodeDTO GetRootNode(StructureNodeDTO draggedItem)
    {
      if (draggedItem.RootNode == null || draggedItem.RootNode == draggedItem)
        return draggedItem.RootNode;
      TreeListViewDragDropBehavior.GetRootNode(draggedItem.RootNode);
      return draggedItem.RootNode;
    }

    private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      if (e.Data == null || e.AllowedEffects == DragDropEffects.None || !(DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData") is Collection<object> dataFromObject))
        return;
      List<StructureNodeDTO> structureNodeDtoList = new List<StructureNodeDTO>();
      if (dataFromObject[0] is StructureNodeDTO)
        structureNodeDtoList.Add(dataFromObject[0] as StructureNodeDTO);
      else if (dataFromObject[0] != null)
      {
        foreach (object obj in dataFromObject[0] as Collection<object>)
          structureNodeDtoList.Add(obj as StructureNodeDTO);
      }
      List<StructureNodeDTO> source = new List<StructureNodeDTO>();
      if (structureNodeDtoList.Any<StructureNodeDTO>())
        source = this.CopyTreeList(structureNodeDtoList);
      if (!(e.OriginalSource is TreeListViewRow treeListViewRow1))
        treeListViewRow1 = (e.OriginalSource as FrameworkElement).ParentOfType<TreeListViewRow>();
      TreeListViewRow treeListViewRow2 = treeListViewRow1;
      if (source.Any<StructureNodeDTO>())
      {
        if (treeListViewRow2 == null)
        {
          this._destinationCollection = (IList) (sender as RadTreeListView).ItemsSource;
          if (this._draggedFromTreelist)
          {
            foreach (StructureNodeDTO droppedItem in source)
              this.MoveItemToRoot(droppedItem);
          }
          else
          {
            this._destinationCollection.Add((object) source[0]);
            source[0].ParentNode = (StructureNodeDTO) null;
            source[0].RootNode = source[0];
          }
        }
        else
        {
          StructureNodeDTO dataContext = treeListViewRow2.DataContext as StructureNodeDTO;
          TreeListViewDropPosition relativeDropPosition = (TreeListViewDropPosition) treeListViewRow2.GetValue(RadTreeListView.DropPositionProperty);
          this._destinationCollection = relativeDropPosition == TreeListViewDropPosition.Inside ? (IList) treeListViewRow2.Items.SourceCollection : (treeListViewRow2.ParentRow != null ? (IList) treeListViewRow2.ParentRow.Items.SourceCollection : (((IList) treeListViewRow2.GridViewDataControl.ItemsSource).Contains((object) dataContext) ? (IList) treeListViewRow2.GridViewDataControl.ItemsSource : (IList) dataContext?.ParentNode.SubNodes));
          if (this._draggedFromTreelist)
          {
            int num = 0;
            foreach (StructureNodeDTO droppedItem in source)
              this.MoveItem(droppedItem, structureNodeDtoList[num++], dataContext, relativeDropPosition);
          }
          else
            this.InsertInDestinationCollection(source[0], dataContext, relativeDropPosition);
        }
      }
      this._draggedFromTreelist = false;
      this._originalSource = (object) null;
      EventPublisher.Publish<TreeDragDropChange>(new TreeDragDropChange(), (object) this);
      EventPublisher.Publish<ItemDropped>(new ItemDropped(), (object) this);
    }

    private static bool IsChildOf(TreeListViewRow dropTarget, object originalSource)
    {
      for (TreeListViewRow treeListViewRow = dropTarget; treeListViewRow != null; treeListViewRow = treeListViewRow.ParentRow)
      {
        if (treeListViewRow.Item == originalSource)
          return true;
      }
      return false;
    }

    private void DataLoaded(object sender, EventArgs e)
    {
      this.AssociatedObject.DataLoaded -= new EventHandler<EventArgs>(this.DataLoaded);
      this.AssociatedObject.ExpandAllHierarchyItems();
    }

    private void MoveItemToRoot(StructureNodeDTO droppedItem)
    {
      this.SourceCollection.Remove((object) droppedItem);
      droppedItem.ParentNode = (StructureNodeDTO) null;
      droppedItem.RootNode = droppedItem;
      TreeListViewDragDropBehavior.SetRootNodeForDescendants(TreeListViewDragDropBehavior.FindChildren(droppedItem), droppedItem);
      this._destinationCollection.Add((object) droppedItem);
      this.AssociatedObject.ExpandAllHierarchyItems();
      EventPublisher.Publish<TreeDragDropChange>(new TreeDragDropChange(), (object) this);
    }

    private void MoveItem(
      StructureNodeDTO droppedItem,
      StructureNodeDTO itemToRemove,
      StructureNodeDTO targetItem,
      TreeListViewDropPosition relativeDropPosition)
    {
      if (droppedItem == targetItem)
        return;
      int num1 = this._destinationCollection.IndexOf((object) targetItem);
      int num2;
      if (relativeDropPosition == TreeListViewDropPosition.After)
      {
        if (relativeDropPosition == TreeListViewDropPosition.After)
        {
          if (num1 <= -1)
          {
            if (num1 == -1)
            {
              StructureTypeEnum? structureType = droppedItem.StructureType;
              StructureTypeEnum structureTypeEnum = StructureTypeEnum.Fixed;
              num2 = this.AreNodesValid((structureType.GetValueOrDefault() == structureTypeEnum ? (structureType.HasValue ? 1 : 0) : 0) != 0 ? (StructureNodesValidator) this._fixedStructuresValidator : this._physicalStructuresValidator, new List<StructureNodeDTO>()
              {
                droppedItem
              }, (StructureNodeDTO) null, (this._draggedFromTreelist ? 1 : 0) != 0) ? 1 : 0;
            }
            else
              num2 = 0;
          }
          else
            num2 = 1;
        }
        else
          num2 = 0;
      }
      else
        num2 = 1;
      if (num2 == 0)
        return;
      this.SourceCollection.Remove((object) itemToRemove);
      this.InsertInDestinationCollection(droppedItem, targetItem, relativeDropPosition);
      this.AssociatedObject.ExpandAllHierarchyItems();
      EventPublisher.Publish<TreeDragDropChange>(new TreeDragDropChange(), (object) this);
    }

    private void InsertInDestinationCollection(
      StructureNodeDTO droppedItem,
      StructureNodeDTO targetItem,
      TreeListViewDropPosition relativeDropPosition)
    {
      switch (relativeDropPosition)
      {
        case TreeListViewDropPosition.Before:
          if (targetItem.ParentNode == null && targetItem.RootNode == targetItem)
          {
            droppedItem.ParentNode = (StructureNodeDTO) null;
            droppedItem.RootNode = droppedItem;
            TreeListViewDragDropBehavior.SetRootNodeForDescendants(TreeListViewDragDropBehavior.FindChildren(droppedItem), droppedItem);
          }
          this._destinationCollection.Insert(this._destinationCollection.IndexOf((object) targetItem), (object) droppedItem);
          break;
        case TreeListViewDropPosition.Inside:
          this._destinationCollection.Add((object) droppedItem);
          TreeListViewDragDropBehavior.SetRootNodeForDescendants(StructuresHelper.Descendants(droppedItem), targetItem);
          break;
        case TreeListViewDropPosition.After:
          this._destinationCollection.Insert(this._destinationCollection.IndexOf((object) targetItem) + 1, (object) droppedItem);
          break;
      }
      EventPublisher.Publish<TreeDragDropChange>(new TreeDragDropChange(), (object) this);
    }

    private static IEnumerable<StructureNodeDTO> FindChildren(StructureNodeDTO droppedItem)
    {
      IEnumerable<StructureNodeDTO> structureNodeDtos = StructuresHelper.Descendants(droppedItem);
      ObservableCollection<StructureNodeDTO> children = new ObservableCollection<StructureNodeDTO>();
      foreach (StructureNodeDTO structureNodeDto in structureNodeDtos)
      {
        if (!structureNodeDto.Equals((object) droppedItem))
          children.Add(structureNodeDto);
      }
      return (IEnumerable<StructureNodeDTO>) children;
    }

    private static void SetRootNodeForDescendants(
      IEnumerable<StructureNodeDTO> descendants,
      StructureNodeDTO targetItem)
    {
      foreach (StructureNodeDTO descendant in descendants)
        descendant.RootNode = targetItem.ParentNode != null ? targetItem.RootNode : targetItem;
    }

    private List<StructureNodeDTO> CopyTreeList(List<StructureNodeDTO> treesToCopy)
    {
      List<StructureNodeDTO> structureNodeDtoList = new List<StructureNodeDTO>();
      foreach (StructureNodeDTO structureNodeDto1 in treesToCopy)
      {
        StructureNodeDTO structureNodeDto2 = this.CopyNode(structureNodeDto1);
        structureNodeDto2.ParentNode = (StructureNodeDTO) null;
        structureNodeDto2.RootNode = structureNodeDto2;
        structureNodeDtoList.Add(structureNodeDto2);
        this.AddSubnodes(structureNodeDto1, structureNodeDto2, structureNodeDto2);
      }
      return structureNodeDtoList;
    }

    private void AddSubnodes(
      StructureNodeDTO oldNode,
      StructureNodeDTO newNode,
      StructureNodeDTO parentForCurrentSubnodes)
    {
      foreach (StructureNodeDTO subNode in (Collection<StructureNodeDTO>) oldNode.SubNodes)
      {
        StructureNodeDTO parentForCurrentSubnodes1 = this.CopyNode(subNode);
        parentForCurrentSubnodes1.ParentNode = parentForCurrentSubnodes;
        parentForCurrentSubnodes1.RootNode = newNode;
        if (parentForCurrentSubnodes.SubNodes == null)
          parentForCurrentSubnodes.SubNodes = new ObservableCollection<StructureNodeDTO>();
        parentForCurrentSubnodes.SubNodes.Add(parentForCurrentSubnodes1);
        this.AddSubnodes(subNode, newNode, parentForCurrentSubnodes1);
      }
    }

    private StructureNodeDTO CopyNode(StructureNodeDTO nodeToCopy)
    {
      return new StructureNodeDTO()
      {
        Id = nodeToCopy.Id,
        Name = nodeToCopy.Name,
        NodeType = nodeToCopy.NodeType,
        Image = nodeToCopy.Image,
        Description = nodeToCopy.Description,
        Entity = nodeToCopy.Entity,
        IsNewNode = nodeToCopy.IsNewNode,
        StructureType = nodeToCopy.StructureType,
        BackgroundColor = nodeToCopy.BackgroundColor,
        OrderNr = nodeToCopy.OrderNr,
        StartDate = nodeToCopy.StartDate,
        EndDate = nodeToCopy.EndDate,
        IsEmpty = nodeToCopy.IsEmpty,
        IsExpanded = nodeToCopy.IsExpanded,
        IsDuplicate = nodeToCopy.IsDuplicate,
        AssignedPicture = nodeToCopy.AssignedPicture,
        AssignedNotes = nodeToCopy.AssignedNotes,
        IsMeterNonEditable = nodeToCopy.IsMeterNonEditable
      };
    }
  }
}
