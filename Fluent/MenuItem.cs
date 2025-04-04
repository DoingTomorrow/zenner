// Decompiled with JetBrains decompiler
// Type: Fluent.MenuItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace Fluent
{
  [ContentProperty("Items")]
  public class MenuItem : System.Windows.Controls.MenuItem, IQuickAccessItemProvider, IRibbonControl, IKeyTipedControl
  {
    private Popup popup;
    private Thumb resizeBothThumb;
    private Thumb resizeVerticalThumb;
    private MenuPanel menuPanel;
    private ScrollViewer scrollViewer;
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof (Description), typeof (string), typeof (MenuItem), (PropertyMetadata) new UIPropertyMetadata((object) ""));
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (MenuItem));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.SizeDefinitionProperty.AddOwner(typeof (MenuItem));
    public static readonly DependencyProperty IsDefinitiveProperty = DependencyProperty.Register(nameof (IsDefinitive), typeof (bool), typeof (MenuItem), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register(nameof (ResizeMode), typeof (ContextMenuResizeMode), typeof (MenuItem), (PropertyMetadata) new UIPropertyMetadata((object) ContextMenuResizeMode.None));
    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof (MaxDropDownHeight), typeof (double), typeof (MenuItem), (PropertyMetadata) new UIPropertyMetadata((object) 100.0));
    public static readonly DependencyProperty IsSplitedProperty = DependencyProperty.Register(nameof (IsSplited), typeof (bool), typeof (MenuItem), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (MenuItem));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (MenuItem));

    public Popup DropDownPopup => this.popup;

    public bool IsContextMenuOpened { get; set; }

    public string Description
    {
      get => (string) this.GetValue(MenuItem.DescriptionProperty);
      set => this.SetValue(MenuItem.DescriptionProperty, (object) value);
    }

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(MenuItem.SizeProperty);
      set => this.SetValue(MenuItem.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(MenuItem.SizeDefinitionProperty);
      set => this.SetValue(MenuItem.SizeDefinitionProperty, (object) value);
    }

    public bool IsDropDownOpen
    {
      get => this.IsSubmenuOpen;
      set => this.IsSubmenuOpen = value;
    }

    public bool IsDefinitive
    {
      get => (bool) this.GetValue(MenuItem.IsDefinitiveProperty);
      set => this.SetValue(MenuItem.IsDefinitiveProperty, (object) value);
    }

    public ContextMenuResizeMode ResizeMode
    {
      get => (ContextMenuResizeMode) this.GetValue(MenuItem.ResizeModeProperty);
      set => this.SetValue(MenuItem.ResizeModeProperty, (object) value);
    }

    public double MaxDropDownHeight
    {
      get => (double) this.GetValue(MenuItem.MaxDropDownHeightProperty);
      set => this.SetValue(MenuItem.MaxDropDownHeightProperty, (object) value);
    }

    public bool IsSplited
    {
      get => (bool) this.GetValue(MenuItem.IsSplitedProperty);
      set => this.SetValue(MenuItem.IsSplitedProperty, (object) value);
    }

    public event EventHandler DropDownOpened;

    public event EventHandler DropDownClosed;

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static MenuItem()
    {
      Type type = typeof (MenuItem);
      ToolTipService.Attach(type);
      ContextMenuService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(MenuItem.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (MenuItem));
      return basevalue;
    }

    public MenuItem()
    {
      ContextMenuService.Coerce((DependencyObject) this);
      this.ToolTip = (object) new System.Windows.Controls.ToolTip();
      (this.ToolTip as System.Windows.Controls.ToolTip).Template = (ControlTemplate) null;
      FocusManager.SetIsFocusScope((DependencyObject) this, true);
    }

    public FrameworkElement CreateQuickAccessItem()
    {
      if (this.HasItems)
      {
        if (this.IsSplited)
        {
          SplitButton quickAccessItem = new SplitButton();
          RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) quickAccessItem);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ResizeMode", MenuItem.ResizeModeProperty, BindingMode.Default);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "MaxDropDownHeight", MenuItem.MaxDropDownHeightProperty, BindingMode.Default);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "DisplayMemberPath", ItemsControl.DisplayMemberPathProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "GroupStyleSelector", ItemsControl.GroupStyleSelectorProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemContainerStyle", ItemsControl.ItemContainerStyleProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemsPanel", ItemsControl.ItemsPanelProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemStringFormat", ItemsControl.ItemStringFormatProperty, BindingMode.OneWay);
          RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemTemplate", ItemsControl.ItemTemplateProperty, BindingMode.OneWay);
          quickAccessItem.DropDownOpened += new EventHandler(this.OnQuickAccessOpened);
          return (FrameworkElement) quickAccessItem;
        }
        DropDownButton quickAccessItem1 = new DropDownButton();
        RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) quickAccessItem1);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "ResizeMode", MenuItem.ResizeModeProperty, BindingMode.Default);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "MaxDropDownHeight", MenuItem.MaxDropDownHeightProperty, BindingMode.Default);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "DisplayMemberPath", ItemsControl.DisplayMemberPathProperty, BindingMode.OneWay);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "GroupStyleSelector", ItemsControl.GroupStyleSelectorProperty, BindingMode.OneWay);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "ItemContainerStyle", ItemsControl.ItemContainerStyleProperty, BindingMode.OneWay);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "ItemsPanel", ItemsControl.ItemsPanelProperty, BindingMode.OneWay);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "ItemStringFormat", ItemsControl.ItemStringFormatProperty, BindingMode.OneWay);
        RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem1, "ItemTemplate", ItemsControl.ItemTemplateProperty, BindingMode.OneWay);
        quickAccessItem1.DropDownOpened += new EventHandler(this.OnQuickAccessOpened);
        return (FrameworkElement) quickAccessItem1;
      }
      Button element = new Button();
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) element);
      return (FrameworkElement) element;
    }

    protected void OnQuickAccessOpened(object sender, EventArgs e)
    {
      DropDownButton dropDownButton = (DropDownButton) sender;
      if (this.ItemsSource != null)
      {
        dropDownButton.ItemsSource = this.ItemsSource;
        this.ItemsSource = (IEnumerable) null;
      }
      else
      {
        for (int index = 0; index < this.Items.Count; index = index - 1 + 1)
        {
          object obj = this.Items[0];
          this.Items.Remove(obj);
          dropDownButton.Items.Add(obj);
        }
      }
      dropDownButton.DropDownClosed += new EventHandler(this.OnQuickAccessMenuClosed);
    }

    protected void OnQuickAccessMenuClosed(object sender, EventArgs e)
    {
      DropDownButton dropDownButton = (DropDownButton) sender;
      dropDownButton.DropDownClosed -= new EventHandler(this.OnQuickAccessMenuClosed);
      if (dropDownButton.ItemsSource != null)
      {
        this.ItemsSource = dropDownButton.ItemsSource;
        dropDownButton.ItemsSource = (IEnumerable) null;
      }
      else
      {
        for (int index = 0; index < dropDownButton.Items.Count; index = index - 1 + 1)
        {
          object obj = dropDownButton.Items[0];
          dropDownButton.Items.Remove(obj);
          this.Items.Add(obj);
        }
      }
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(MenuItem.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(MenuItem.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(MenuItem.QuickAccessElementStyleProperty);
      set => this.SetValue(MenuItem.QuickAccessElementStyleProperty, (object) value);
    }

    public virtual void OnKeyTipPressed()
    {
      if (!this.HasItems)
      {
        this.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.MenuItem.ClickEvent));
      }
      else
      {
        Keyboard.Focus((IInputElement) this);
        this.IsDropDownOpen = true;
      }
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new MenuItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is FrameworkElement;

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if (e.ClickCount == 1 && this.IsSplited && this.GetTemplateChild("PART_ButtonBorder") is Border templateChild && PopupService.IsMousePhysicallyOver((UIElement) templateChild))
        this.OnClick();
      base.OnMouseLeftButtonUp(e);
    }

    protected override void OnClick()
    {
      if (this.IsDefinitive && (!this.HasItems || this.IsSplited))
        PopupService.RaiseDismissPopupEvent((object) this, DismissPopupMode.Always);
      base.OnClick();
    }

    public override void OnApplyTemplate()
    {
      if (this.popup != null)
      {
        this.popup.Opened -= new EventHandler(this.OnDropDownOpened);
        this.popup.Closed -= new EventHandler(this.OnDropDownClosed);
      }
      this.popup = this.GetTemplateChild("PART_Popup") as Popup;
      if (this.popup != null)
      {
        this.popup.Opened += new EventHandler(this.OnDropDownOpened);
        this.popup.Closed += new EventHandler(this.OnDropDownClosed);
        KeyboardNavigation.SetControlTabNavigation((DependencyObject) this.popup, KeyboardNavigationMode.Cycle);
        KeyboardNavigation.SetDirectionalNavigation((DependencyObject) this.popup, KeyboardNavigationMode.Cycle);
        KeyboardNavigation.SetTabNavigation((DependencyObject) this.popup, KeyboardNavigationMode.Cycle);
      }
      if (this.resizeVerticalThumb != null)
        this.resizeVerticalThumb.DragDelta -= new DragDeltaEventHandler(this.OnResizeVerticalDelta);
      this.resizeVerticalThumb = this.GetTemplateChild("PART_ResizeVerticalThumb") as Thumb;
      if (this.resizeVerticalThumb != null)
        this.resizeVerticalThumb.DragDelta += new DragDeltaEventHandler(this.OnResizeVerticalDelta);
      if (this.resizeBothThumb != null)
        this.resizeBothThumb.DragDelta -= new DragDeltaEventHandler(this.OnResizeBothDelta);
      this.resizeBothThumb = this.GetTemplateChild("PART_ResizeBothThumb") as Thumb;
      if (this.resizeBothThumb != null)
        this.resizeBothThumb.DragDelta += new DragDeltaEventHandler(this.OnResizeBothDelta);
      this.scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
      this.menuPanel = this.GetTemplateChild("PART_MenuPanel") as MenuPanel;
    }

    protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
    {
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        if (this.IsSubmenuOpen)
        {
          this.IsSubmenuOpen = false;
        }
        else
        {
          DependencyObject dropDownOrMenuItem = this.FindParentDropDownOrMenuItem();
          if (dropDownOrMenuItem != null)
          {
            if (dropDownOrMenuItem is IDropDownControl dropDownControl)
              dropDownControl.IsDropDownOpen = false;
            else
              (dropDownOrMenuItem as System.Windows.Controls.MenuItem).IsSubmenuOpen = false;
          }
        }
        e.Handled = true;
      }
      else
        base.OnKeyDown(e);
    }

    private DependencyObject FindParentDropDownOrMenuItem()
    {
      DependencyObject parent = this.Parent;
      while (true)
      {
        switch (parent)
        {
          case null:
            goto label_5;
          case IDropDownControl _:
            goto label_1;
          case System.Windows.Controls.MenuItem _:
            goto label_2;
          default:
            parent = LogicalTreeHelper.GetParent(parent);
            continue;
        }
      }
label_1:
      return parent;
label_2:
      return parent;
label_5:
      return (DependencyObject) null;
    }

    protected virtual void OnSizePropertyChanged(
      RibbonControlSize previous,
      RibbonControlSize current)
    {
    }

    private void OnResizeBothDelta(object sender, DragDeltaEventArgs e)
    {
      if (this.scrollViewer != null)
        this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      if (double.IsNaN(this.menuPanel.Width))
        this.menuPanel.Width = this.menuPanel.ActualWidth;
      if (double.IsNaN(this.menuPanel.Height))
        this.menuPanel.Height = this.menuPanel.ActualHeight;
      this.menuPanel.Width = Math.Max(this.menuPanel.ResizeMinWidth, this.menuPanel.Width + e.HorizontalChange);
      this.menuPanel.Height = Math.Min(Math.Max(this.menuPanel.ResizeMinHeight, this.menuPanel.Height + e.VerticalChange), this.MaxDropDownHeight);
    }

    private void OnResizeVerticalDelta(object sender, DragDeltaEventArgs e)
    {
      if (this.scrollViewer != null)
        this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      if (double.IsNaN(this.menuPanel.Height))
        this.menuPanel.Height = this.menuPanel.ActualHeight;
      this.menuPanel.Height = Math.Min(Math.Max(this.menuPanel.ResizeMinHeight, this.menuPanel.Height + e.VerticalChange), this.MaxDropDownHeight);
    }

    private void OnDropDownClosed(object sender, EventArgs e)
    {
      if (this.DropDownClosed == null)
        return;
      this.DropDownClosed((object) this, e);
    }

    private void OnDropDownOpened(object sender, EventArgs e)
    {
      if (this.scrollViewer != null && this.ResizeMode != ContextMenuResizeMode.None)
        this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
      this.menuPanel.Width = double.NaN;
      this.menuPanel.Height = double.NaN;
      if (this.DropDownOpened == null)
        return;
      this.DropDownOpened((object) this, e);
    }

    [SpecialName]
    object IRibbonControl.get_Header() => this.Header;

    [SpecialName]
    void IRibbonControl.set_Header([In] object obj0) => this.Header = obj0;

    [SpecialName]
    object IRibbonControl.get_Icon() => this.Icon;

    [SpecialName]
    void IRibbonControl.set_Icon([In] object obj0) => this.Icon = obj0;
  }
}
