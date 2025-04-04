// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonTabControl
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_ToolbarPanel", Type = typeof (Panel))]
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (RibbonTabItem))]
  [TemplatePart(Name = "PART_Popup", Type = typeof (Popup))]
  [TemplatePart(Name = "PART_TabsContainer", Type = typeof (IScrollInfo))]
  public class RibbonTabControl : Selector, IDropDownControl
  {
    private Popup popup;
    private object oldSelectedItem;
    private ObservableCollection<UIElement> toolBarItems;
    private Panel toolbarPanel;
    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof (Menu), typeof (UIElement), typeof (RibbonTabControl), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    private static readonly DependencyPropertyKey SelectedContentPropertyKey = DependencyProperty.RegisterReadOnly(nameof (SelectedContent), typeof (object), typeof (RibbonTabControl), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty SelectedContentProperty = RibbonTabControl.SelectedContentPropertyKey.DependencyProperty;
    public static readonly DependencyProperty IsMinimizedProperty = DependencyProperty.Register(nameof (IsMinimized), typeof (bool), typeof (RibbonTabControl), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(RibbonTabControl.OnMinimizedChanged)));
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (RibbonTabControl), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(RibbonTabControl.OnIsOpenChanged), new CoerceValueCallback(RibbonTabControl.CoerceIsDropDownOpen)));
    internal static readonly DependencyProperty SelectedTabItemProperty = DependencyProperty.Register(nameof (SelectedTabItem), typeof (RibbonTabItem), typeof (RibbonTabControl), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public UIElement Menu
    {
      get => (UIElement) this.GetValue(RibbonTabControl.MenuProperty);
      set => this.SetValue(RibbonTabControl.MenuProperty, (object) value);
    }

    public Popup DropDownPopup => this.popup;

    public bool IsContextMenuOpened { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object SelectedContent
    {
      get => this.GetValue(RibbonTabControl.SelectedContentProperty);
      internal set => this.SetValue(RibbonTabControl.SelectedContentPropertyKey, value);
    }

    public bool IsMinimized
    {
      get => (bool) this.GetValue(RibbonTabControl.IsMinimizedProperty);
      set => this.SetValue(RibbonTabControl.IsMinimizedProperty, (object) value);
    }

    public bool IsDropDownOpen
    {
      get => (bool) this.GetValue(RibbonTabControl.IsDropDownOpenProperty);
      set => this.SetValue(RibbonTabControl.IsDropDownOpenProperty, (object) value);
    }

    private static object CoerceIsDropDownOpen(DependencyObject d, object basevalue)
    {
      return !(d as RibbonTabControl).IsMinimized ? (object) false : basevalue;
    }

    internal bool CanScroll
    {
      get
      {
        return this.GetTemplateChild("PART_TabsContainer") is IScrollInfo templateChild && templateChild.ExtentWidth > templateChild.ViewportWidth;
      }
    }

    internal RibbonTabItem SelectedTabItem
    {
      get => (RibbonTabItem) this.GetValue(RibbonTabControl.SelectedTabItemProperty);
      set => this.SetValue(RibbonTabControl.SelectedTabItemProperty, (object) value);
    }

    public ObservableCollection<UIElement> ToolBarItems
    {
      get
      {
        if (this.toolBarItems == null)
        {
          this.toolBarItems = new ObservableCollection<UIElement>();
          this.toolBarItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnToolbarItemsCollectionChanged);
        }
        return this.toolBarItems;
      }
    }

    internal Panel ToolbarPanel => this.toolbarPanel;

    private void OnToolbarItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          for (int index = 0; index < e.NewItems.Count; ++index)
          {
            if (this.ToolbarPanel != null)
              this.ToolbarPanel.Children.Insert(e.NewStartingIndex + index, (UIElement) e.NewItems[index]);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          IEnumerator enumerator1 = e.OldItems.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              if (this.ToolbarPanel != null)
                this.ToolbarPanel.Children.Remove(current as UIElement);
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Replace:
          foreach (object oldItem in (IEnumerable) e.OldItems)
          {
            if (this.ToolbarPanel != null)
              this.ToolbarPanel.Children.Remove(oldItem as UIElement);
          }
          IEnumerator enumerator2 = e.NewItems.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              if (this.ToolbarPanel != null)
                this.ToolbarPanel.Children.Add(current as UIElement);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonTabControl()
    {
      Type type = typeof (RibbonTabControl);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonTabControl)));
      ContextMenuService.Attach(type);
      PopupService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonTabControl.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonTabControl));
      return basevalue;
    }

    public RibbonTabControl() => ContextMenuService.Coerce((DependencyObject) this);

    protected override void OnInitialized(EventArgs e)
    {
      base.OnInitialized(e);
      this.ItemContainerGenerator.StatusChanged += new EventHandler(this.OnGeneratorStatusChanged);
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new RibbonTabItem();
    }

    public override void OnApplyTemplate()
    {
      this.popup = this.GetTemplateChild("PART_Popup") as Popup;
      if (this.popup != null)
        this.popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.CustomPopupPlacementMethod);
      if (this.ToolbarPanel != null && this.toolBarItems != null)
      {
        for (int index = 0; index < this.toolBarItems.Count; ++index)
          this.ToolbarPanel.Children.Remove(this.toolBarItems[index]);
      }
      this.toolbarPanel = this.GetTemplateChild("PART_ToolbarPanel") as Panel;
      if (this.ToolbarPanel == null || this.toolBarItems == null)
        return;
      for (int index = 0; index < this.toolBarItems.Count; ++index)
        this.ToolbarPanel.Children.Add(this.toolBarItems[index]);
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is RibbonTabItem;

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      if (e.Action != NotifyCollectionChangedAction.Remove || this.SelectedIndex != -1)
        return;
      int startIndex = e.OldStartingIndex + 1;
      if (startIndex > this.Items.Count)
        startIndex = 0;
      RibbonTabItem nextTabItem = this.FindNextTabItem(startIndex, -1);
      if (nextTabItem != null)
        nextTabItem.IsSelected = true;
      else
        this.SelectedContent = (object) null;
    }

    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count > 0)
      {
        if (this.IsMinimized)
        {
          if (this.oldSelectedItem == e.AddedItems[0])
            this.IsDropDownOpen = !this.IsDropDownOpen;
          else
            this.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, (Delegate) (() => this.IsDropDownOpen = true));
          ((UIElement) e.AddedItems[0]).IsHitTestVisible = false;
        }
        this.UpdateSelectedContent();
      }
      if (e.RemovedItems.Count > 0)
      {
        this.oldSelectedItem = e.RemovedItems[0];
        ((UIElement) e.RemovedItems[0]).IsHitTestVisible = true;
      }
      base.OnSelectionChanged(e);
    }

    protected override void OnPreviewMouseWheel(MouseWheelEventArgs e) => this.ProcessMouseWheel(e);

    private Ribbon FindParentRibbon()
    {
      DependencyObject current = (DependencyObject) this;
      while (LogicalTreeHelper.GetParent(current) != null)
      {
        current = LogicalTreeHelper.GetParent(current);
        if (current is Ribbon parentRibbon)
          return parentRibbon;
      }
      return (Ribbon) null;
    }

    private bool IsRibbonAncestorOf(DependencyObject element)
    {
      for (; element != null; element = LogicalTreeHelper.GetParent(element) ?? VisualTreeHelper.GetParent(element))
      {
        if (element is Ribbon)
          return true;
      }
      return false;
    }

    internal void ProcessMouseWheel(MouseWheelEventArgs e)
    {
      if (this.IsMinimized || this.SelectedItem == null || Keyboard.FocusedElement is DependencyObject focusedElement && this.IsRibbonAncestorOf(focusedElement))
        return;
      List<RibbonTabItem> ribbonTabItemList = new List<RibbonTabItem>();
      int index1 = -1;
      for (int index2 = 0; index2 < this.Items.Count; ++index2)
      {
        if ((this.Items[index2] as RibbonTabItem).Visibility == Visibility.Visible)
        {
          ribbonTabItemList.Add(this.Items[index2] as RibbonTabItem);
          if ((this.Items[index2] as RibbonTabItem).IsSelected)
            index1 = ribbonTabItemList.Count - 1;
        }
      }
      if (e.Delta > 0 && index1 > 0)
      {
        ribbonTabItemList[index1].IsSelected = false;
        --index1;
        ribbonTabItemList[index1].IsSelected = true;
      }
      if (e.Delta < 0 && index1 < ribbonTabItemList.Count - 1)
      {
        ribbonTabItemList[index1].IsSelected = false;
        int index3 = index1 + 1;
        ribbonTabItemList[index3].IsSelected = true;
      }
      e.Handled = true;
    }

    private RibbonTabItem GetSelectedTabItem()
    {
      object selectedItem = this.SelectedItem;
      if (selectedItem == null)
        return (RibbonTabItem) null;
      if (!(selectedItem is RibbonTabItem selectedTabItem))
        selectedTabItem = this.ItemContainerGenerator.ContainerFromIndex(this.SelectedIndex) as RibbonTabItem;
      return selectedTabItem;
    }

    private RibbonTabItem FindNextTabItem(int startIndex, int direction)
    {
      if (direction != 0)
      {
        int index1 = startIndex;
        for (int index2 = 0; index2 < this.Items.Count; ++index2)
        {
          index1 += direction;
          if (index1 >= this.Items.Count)
            index1 = 0;
          else if (index1 < 0)
            index1 = this.Items.Count - 1;
          if (this.ItemContainerGenerator.ContainerFromIndex(index1) is RibbonTabItem nextTabItem && nextTabItem.IsEnabled && nextTabItem.Visibility == Visibility.Visible)
            return nextTabItem;
        }
      }
      return (RibbonTabItem) null;
    }

    private void UpdateSelectedContent()
    {
      if (this.SelectedIndex < 0)
      {
        this.SelectedContent = (object) null;
        this.SelectedTabItem = (RibbonTabItem) null;
      }
      else
      {
        RibbonTabItem selectedTabItem = this.GetSelectedTabItem();
        if (selectedTabItem == null)
          return;
        this.SelectedContent = (object) selectedTabItem.GroupsContainer;
        this.UpdateLayout();
        this.SelectedTabItem = selectedTabItem;
      }
    }

    private void OnGeneratorStatusChanged(object sender, EventArgs e)
    {
      if (this.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
        return;
      this.UpdateSelectedContent();
    }

    private static void OnMinimizedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RibbonTabControl ribbonTabControl = (RibbonTabControl) d;
      if (ribbonTabControl.IsMinimized)
        return;
      ribbonTabControl.IsDropDownOpen = false;
    }

    private void OnRibbonTabPopupClosing()
    {
      if (this.SelectedItem is RibbonTabItem)
        (this.SelectedItem as RibbonTabItem).IsHitTestVisible = true;
      if (Mouse.Captured != this)
        return;
      Mouse.Capture((IInputElement) null);
    }

    private void OnRibbonTabPopupOpening()
    {
      if (this.SelectedItem is RibbonTabItem)
        (this.SelectedItem as RibbonTabItem).IsHitTestVisible = false;
      Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
    }

    private CustomPopupPlacement[] CustomPopupPlacementMethod(
      Size popupsize,
      Size targetsize,
      Point offset)
    {
      if (this.popup != null && this.SelectedTabItem != null)
      {
        Point screen1 = this.SelectedTabItem.PointToScreen(new Point(0.0, 0.0));
        NativeMethods.Rect lprc = new NativeMethods.Rect();
        lprc.Left = (int) screen1.X;
        lprc.Top = (int) screen1.Y;
        lprc.Right = (int) screen1.X + (int) this.SelectedTabItem.ActualWidth;
        lprc.Bottom = (int) screen1.Y + (int) this.SelectedTabItem.ActualHeight;
        uint dwFlags = 2;
        IntPtr hMonitor = NativeMethods.MonitorFromRect(ref lprc, dwFlags);
        if (hMonitor != IntPtr.Zero)
        {
          NativeMethods.MonitorInfo monitorInfo = new NativeMethods.MonitorInfo();
          monitorInfo.Size = Marshal.SizeOf((object) monitorInfo);
          NativeMethods.GetMonitorInfo(hMonitor, monitorInfo);
          Point screen2 = this.PointToScreen(new Point(0.0, 0.0));
          if (this.FlowDirection == FlowDirection.RightToLeft)
            screen2.X -= this.ActualWidth;
          double val2 = (double) monitorInfo.Work.Right - Math.Max((double) monitorInfo.Work.Left, screen2.X);
          double actualWidth = this.ActualWidth;
          if (screen2.X < (double) monitorInfo.Work.Left)
          {
            actualWidth -= (double) monitorInfo.Work.Left - screen2.X;
            screen2.X = (double) monitorInfo.Work.Left;
          }
          this.popup.Width = Math.Min(actualWidth, val2);
          return new CustomPopupPlacement[2]
          {
            new CustomPopupPlacement(new Point(screen2.X - screen1.X, this.SelectedTabItem.ActualHeight - (this.popup.Child as FrameworkElement).Margin.Top), PopupPrimaryAxis.None),
            new CustomPopupPlacement(new Point(screen2.X - screen1.X, -(this.SelectedContent as ScrollViewer).ActualHeight - (this.popup.Child as FrameworkElement).Margin.Bottom), PopupPrimaryAxis.None)
          };
        }
      }
      return (CustomPopupPlacement[]) null;
    }

    private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RibbonTabControl ribbonTabControl = (RibbonTabControl) d;
      if (ribbonTabControl.IsDropDownOpen)
        ribbonTabControl.OnRibbonTabPopupOpening();
      else
        ribbonTabControl.OnRibbonTabPopupClosing();
    }
  }
}
