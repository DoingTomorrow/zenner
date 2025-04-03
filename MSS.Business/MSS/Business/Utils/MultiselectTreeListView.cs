// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.MultiselectTreeListView
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.DTO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Business.Utils
{
  public class MultiselectTreeListView : RadTreeListView
  {
    private RadTreeViewItem deselectionRealTarget;
    private bool _isSelectionRootNode;
    private StructureNodeDTO _currentSelectionParentNode;
    private int _noOfSelectedItems;
    private List<object> _itemsWithOtherParents;

    static MultiselectTreeListView()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (MultiselectTreeListView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (MultiselectTreeListView)));
    }

    public MultiselectTreeListView()
    {
      this.SelectionChanging += new EventHandler<SelectionChangingEventArgs>(this.rt_OnSelectionChanging);
      this.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(this.rt_OnSelectionChange);
      this._currentSelectionParentNode = (StructureNodeDTO) null;
      this._isSelectionRootNode = false;
      this._noOfSelectedItems = 0;
    }

    ~MultiselectTreeListView()
    {
      this.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.rt_PreviewMouseLeftButtonDown);
      this.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.rt_PreviewMouseLeftButtonUp);
      this.SelectionChanging -= new EventHandler<SelectionChangingEventArgs>(this.rt_OnSelectionChanging);
      this.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(this.rt_OnSelectionChange);
      this._currentSelectionParentNode = (StructureNodeDTO) null;
      this._isSelectionRootNode = false;
      this._noOfSelectedItems = 0;
    }

    private void rt_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      e.Handled = true;
    }

    private void rt_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      RadTreeViewItem radTreeViewItem = (e.OriginalSource as UIElement).ParentOfType<RadTreeViewItem>();
      if (radTreeViewItem == null || !radTreeViewItem.IsSelected)
        return;
      this.deselectionRealTarget = (e.OriginalSource as UIElement).ParentOfType<RadTreeViewItem>();
      this.Dispatcher.BeginInvoke((Delegate) (() =>
      {
        this.deselectionRealTarget.IsSelected = false;
        this.deselectionRealTarget = (RadTreeViewItem) null;
      }));
    }

    private void rt_OnSelectionChanging(object sender, SelectionChangingEventArgs e)
    {
      this._itemsWithOtherParents = new List<object>();
      this._noOfSelectedItems = (sender as MultiselectTreeListView).SelectedItems.Count;
      if (e.RemovedItems.Count > 0 && this._noOfSelectedItems == 0)
      {
        this._currentSelectionParentNode = (StructureNodeDTO) null;
        this._isSelectionRootNode = false;
      }
      if (e.AddedItems.Count <= 0)
        return;
      if (this._noOfSelectedItems == 0 || this._noOfSelectedItems == 1 && e.AddedItems.Count == 1 && e.RemovedItems.Count == 1)
      {
        this._currentSelectionParentNode = (e.AddedItems[0] as StructureNodeDTO).ParentNode;
        this._isSelectionRootNode = this._currentSelectionParentNode == null;
      }
      else
      {
        foreach (object addedItem in e.AddedItems)
        {
          if (this._isSelectionRootNode && (addedItem as StructureNodeDTO).ParentNode != null)
            this._itemsWithOtherParents.Add(addedItem);
          if (!this._isSelectionRootNode && (addedItem as StructureNodeDTO).ParentNode != this._currentSelectionParentNode)
            this._itemsWithOtherParents.Add(addedItem);
        }
      }
    }

    private void rt_OnSelectionChange(object sender, SelectionChangeEventArgs e)
    {
      foreach (object itemsWithOtherParent in this._itemsWithOtherParents)
        (sender as MultiselectTreeListView).SelectedItems.Remove(itemsWithOtherParent);
    }
  }
}
