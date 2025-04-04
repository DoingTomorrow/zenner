// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.AttachedProperties.ListBoxDragDropBehavior
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;

#nullable disable
namespace MSS.Client.UI.Common.AttachedProperties
{
  public class ListBoxDragDropBehavior
  {
    private ListBox _associatedObject;
    private static Dictionary<ListBox, ListBoxDragDropBehavior> instances;
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (ListBoxDragDropBehavior), new PropertyMetadata(new PropertyChangedCallback(ListBoxDragDropBehavior.OnIsEnabledPropertyChanged)));

    public ListBox AssociatedObject
    {
      get => this._associatedObject;
      set => this._associatedObject = value;
    }

    static ListBoxDragDropBehavior()
    {
      ListBoxDragDropBehavior.instances = new Dictionary<ListBox, ListBoxDragDropBehavior>();
    }

    public static bool GetIsEnabled(DependencyObject obj)
    {
      return (bool) obj.GetValue(ListBoxDragDropBehavior.IsEnabledProperty);
    }

    public static void SetIsEnabled(DependencyObject obj, bool value)
    {
      ListBoxDragDropBehavior attachedBehavior = ListBoxDragDropBehavior.GetAttachedBehavior(obj as ListBox);
      attachedBehavior.AssociatedObject = obj as ListBox;
      if (value)
        attachedBehavior.Initialize();
      else
        attachedBehavior.CleanUp();
      obj.SetValue(ListBoxDragDropBehavior.IsEnabledProperty, (object) value);
    }

    public static void OnIsEnabledPropertyChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      ListBoxDragDropBehavior.SetIsEnabled(dependencyObject, (bool) e.NewValue);
    }

    private static ListBoxDragDropBehavior GetAttachedBehavior(ListBox listBox)
    {
      if (!ListBoxDragDropBehavior.instances.ContainsKey(listBox))
      {
        ListBoxDragDropBehavior.instances[listBox] = new ListBoxDragDropBehavior();
        ListBoxDragDropBehavior.instances[listBox].AssociatedObject = listBox;
      }
      return ListBoxDragDropBehavior.instances[listBox];
    }

    protected virtual void Initialize()
    {
      this.UnsubscribeFromDragDropEvents();
      this.SubscribeToDragDropEvents();
    }

    protected virtual void CleanUp() => this.UnsubscribeFromDragDropEvents();

    private void SubscribeToDragDropEvents()
    {
      DragDropManager.AddDragInitializeHandler((DependencyObject) this.AssociatedObject, new DragInitializeEventHandler(this.OnDragInitialize));
      DragDropManager.AddGiveFeedbackHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.GiveFeedbackEventHandler(this.OnGiveFeedback));
      DragDropManager.AddDropHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDrop));
      DragDropManager.AddDragDropCompletedHandler((DependencyObject) this.AssociatedObject, new DragDropCompletedEventHandler(this.OnDragDropCompleted));
      DragDropManager.AddDragOverHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDragOver));
    }

    private void UnsubscribeFromDragDropEvents()
    {
      DragDropManager.RemoveDragInitializeHandler((DependencyObject) this.AssociatedObject, new DragInitializeEventHandler(this.OnDragInitialize));
      DragDropManager.RemoveGiveFeedbackHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.GiveFeedbackEventHandler(this.OnGiveFeedback));
      DragDropManager.RemoveDropHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDrop));
      DragDropManager.RemoveDragDropCompletedHandler((DependencyObject) this.AssociatedObject, new DragDropCompletedEventHandler(this.OnDragDropCompleted));
      DragDropManager.RemoveDragOverHandler((DependencyObject) this.AssociatedObject, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDragOver));
    }

    private void OnDragInitialize(object sender, DragInitializeEventArgs e)
    {
      DropIndicationDetails data1 = new DropIndicationDetails();
      if (!(e.OriginalSource is ListBoxItem listBoxItem1))
        listBoxItem1 = (e.OriginalSource as FrameworkElement).ParentOfType<ListBoxItem>();
      ListBoxItem listBoxItem2 = listBoxItem1;
      object data2 = listBoxItem2 != null ? listBoxItem2.DataContext : (sender as ListBox).SelectedItem;
      data1.CurrentDraggedItem = data2;
      IDragPayload payload = DragDropPayloadManager.GeneratePayload((DataConverter) null);
      payload.SetData("DraggedData", data2);
      payload.SetData("DropDetails", (object) data1);
      e.Data = (object) payload;
      DragInitializeEventArgs initializeEventArgs = e;
      DragVisual dragVisual1 = new DragVisual();
      dragVisual1.Content = (object) data1;
      dragVisual1.ContentTemplate = this.AssociatedObject.Resources[(object) "DraggedItemTemplate"] as DataTemplate;
      DragVisual dragVisual2 = dragVisual1;
      initializeEventArgs.DragVisual = (object) dragVisual2;
      e.DragVisualOffset = e.RelativeStartPoint;
      e.AllowedEffects = DragDropEffects.All;
    }

    private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs e)
    {
      e.SetCursor(Cursors.Arrow);
      e.Handled = true;
    }

    private void OnDragDropCompleted(object sender, DragDropCompletedEventArgs e)
    {
      object dataFromObject = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData");
      if (e.Effects == 0)
        return;
      ((sender as ListBox).ItemsSource as IList).Remove(dataFromObject);
    }

    private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      if (!(DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) is TreeViewDragDropOptions dataFromObject))
        return;
      object obj = dataFromObject.DraggedItems.FirstOrDefault<object>();
      if (obj == null)
        return;
      if (e.Effects != 0)
        ((sender as ListBox).ItemsSource as IList).Add(obj);
      e.Handled = true;
    }

    private void OnDragOver(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      if (!(DragDropPayloadManager.GetDataFromObject(e.Data, TreeViewDragDropOptions.Key) is TreeViewDragDropOptions dataFromObject))
      {
        e.Effects = DragDropEffects.None;
        e.Handled = true;
      }
      else
      {
        object obj = dataFromObject.DraggedItems.FirstOrDefault<object>();
        Type elementType = Queryable.AsQueryable(this.AssociatedObject.ItemsSource as IList).ElementType;
        if (obj.GetType() != elementType)
        {
          e.Effects = DragDropEffects.None;
        }
        else
        {
          (dataFromObject.DragVisual as TreeViewDragVisual).IsDropPossible = true;
          dataFromObject.DropAction = DropAction.Move;
          dataFromObject.UpdateDragVisual();
        }
        e.Handled = true;
      }
    }
  }
}
