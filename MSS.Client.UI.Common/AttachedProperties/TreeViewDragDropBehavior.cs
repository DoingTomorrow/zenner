// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.AttachedProperties.TreeViewDragDropBehavior
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MSS.Business.DTO;
using MSS.Business.Modules.StructuresManagement;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;

#nullable disable
namespace MSS.Client.UI.Common.AttachedProperties
{
  public class TreeViewDragDropBehavior
  {
    private static readonly Dictionary<RadTreeView, TreeViewDragDropBehavior> instances;
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (TreeViewDragDropBehavior), new PropertyMetadata(new PropertyChangedCallback(TreeViewDragDropBehavior.OnIsEnabledPropertyChanged)));

    static TreeViewDragDropBehavior()
    {
      TreeViewDragDropBehavior.instances = new Dictionary<RadTreeView, TreeViewDragDropBehavior>();
    }

    public double TreeViewItemHeight { get; set; }

    public RadTreeView AssociatedObject { get; set; }

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool) obj.GetValue(TreeViewDragDropBehavior.IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
      TreeViewDragDropBehavior attachedBehavior = TreeViewDragDropBehavior.GetAttachedBehavior(obj as RadTreeView);
      attachedBehavior.AssociatedObject = obj as RadTreeView;
      if (value)
        attachedBehavior.Initialize();
      else
        attachedBehavior.CleanUp();
      obj.SetValue(TreeViewDragDropBehavior.IsEnabledProperty, (object) value);
    }

    public static void OnIsEnabledPropertyChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      TreeViewDragDropBehavior.SetIsEnabled(dependencyObject, (bool) e.NewValue);
    }

    private static TreeViewDragDropBehavior GetAttachedBehavior(RadTreeView gridview)
    {
      if (!TreeViewDragDropBehavior.instances.ContainsKey(gridview))
      {
        TreeViewDragDropBehavior.instances[gridview] = new TreeViewDragDropBehavior();
        TreeViewDragDropBehavior.instances[gridview].AssociatedObject = gridview;
      }
      return TreeViewDragDropBehavior.instances[gridview];
    }

    protected virtual void Initialize()
    {
      DragDropManager.AddDragInitializeHandler((DependencyObject) this.AssociatedObject, new DragInitializeEventHandler(this.OnDragInitialize), true);
      DragDropManager.AddDragOverHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnItemDragOver), true);
      this.TreeViewItemHeight = 24.0;
    }

    protected virtual void CleanUp()
    {
      DragDropManager.RemoveDragInitializeHandler((DependencyObject) this.AssociatedObject, new DragInitializeEventHandler(this.OnDragInitialize));
      DragDropManager.RemoveDragOverHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnItemDragOver));
    }

    private void OnDragInitialize(object sender, DragInitializeEventArgs e)
    {
      if (!(e.OriginalSource is RadTreeViewItem radTreeViewItem1))
        radTreeViewItem1 = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
      RadTreeViewItem radTreeViewItem2 = radTreeViewItem1;
      object node = radTreeViewItem2 != null ? radTreeViewItem2.Item : (sender as RadTreeView).SelectedItem;
      if (StructuresHelper.IsMeterWithMeterParent(node as StructureNodeDTO))
        node = (object) null;
      DropIndicationDetails indicationDetails = new DropIndicationDetails();
      if (node is StructureNodeDTO structureNodeDto)
        indicationDetails.CurrentDraggedItem = (object) structureNodeDto.Name;
      DragVisual dragVisual1 = new DragVisual();
      dragVisual1.Content = (object) indicationDetails;
      dragVisual1.ContentTemplate = this.AssociatedObject.Resources[(object) "DraggedItemTemplate"] as DataTemplate;
      DragVisual dragVisual2 = dragVisual1;
      e.DragVisual = (object) dragVisual2;
      e.DragVisualOffset = e.RelativeStartPoint;
      IDragPayload payload = DragDropPayloadManager.GeneratePayload((DataConverter) null);
      DragDropPayloadManager.SetData((object) payload, "DraggedData", (object) new Collection<object>()
      {
        node
      });
      DragDropPayloadManager.SetData((object) payload, "DropDetails", (object) new Collection<object>()
      {
        (object) indicationDetails
      });
      e.Data = (object) payload;
    }

    private void OnItemDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      e.Effects = DragDropEffects.None;
      e.Handled = true;
    }
  }
}
