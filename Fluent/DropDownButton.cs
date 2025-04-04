// Decompiled with JetBrains decompiler
// Type: Fluent.DropDownButton
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  [ContentProperty("Items")]
  [TemplatePart(Name = "PART_Popup", Type = typeof (Popup))]
  public class DropDownButton : 
    MenuBase,
    IQuickAccessItemProvider,
    IRibbonControl,
    IKeyTipedControl,
    IDropDownControl
  {
    private Thumb resizeBothThumb;
    private Thumb resizeVerticalThumb;
    private Popup popup;
    private Border buttonBorder;
    private IInputElement focusedElement;
    private MenuPanel menuPanel;
    private ScrollViewer scrollViewer;
    private bool isFirstTime;
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (DropDownButton));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.AttachSizeDefinition(typeof (DropDownButton));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (DropDownButton));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (DropDownButton), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(DropDownButton.OnIconChanged)));
    public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (object), typeof (DropDownButton), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty HasTriangleProperty = DependencyProperty.Register(nameof (HasTriangle), typeof (bool), typeof (DropDownButton), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (DropDownButton), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register(nameof (ResizeMode), typeof (ContextMenuResizeMode), typeof (DropDownButton), (PropertyMetadata) new UIPropertyMetadata((object) ContextMenuResizeMode.None));
    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof (MaxDropDownHeight), typeof (double), typeof (DropDownButton), (PropertyMetadata) new UIPropertyMetadata((object) 350.0));
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (DropDownButton), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (DropDownButton));

    public Popup DropDownPopup => this.popup;

    public bool IsContextMenuOpened { get; set; }

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(DropDownButton.SizeProperty);
      set => this.SetValue(DropDownButton.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(DropDownButton.SizeDefinitionProperty);
      set => this.SetValue(DropDownButton.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => (object) (string) this.GetValue(DropDownButton.HeaderProperty);
      set => this.SetValue(DropDownButton.HeaderProperty, value);
    }

    public object Icon
    {
      get => this.GetValue(DropDownButton.IconProperty);
      set => this.SetValue(DropDownButton.IconProperty, value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DropDownButton dropDownButton = d as DropDownButton;
      if (e.OldValue is FrameworkElement oldValue)
        dropDownButton.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      dropDownButton.AddLogicalChild((object) newValue);
    }

    public object LargeIcon
    {
      get => this.GetValue(DropDownButton.LargeIconProperty);
      set => this.SetValue(DropDownButton.LargeIconProperty, value);
    }

    public bool HasTriangle
    {
      get => (bool) this.GetValue(DropDownButton.HasTriangleProperty);
      set => this.SetValue(DropDownButton.HasTriangleProperty, (object) value);
    }

    public bool IsDropDownOpen
    {
      get => (bool) this.GetValue(DropDownButton.IsDropDownOpenProperty);
      set => this.SetValue(DropDownButton.IsDropDownOpenProperty, (object) value);
    }

    public ContextMenuResizeMode ResizeMode
    {
      get => (ContextMenuResizeMode) this.GetValue(DropDownButton.ResizeModeProperty);
      set => this.SetValue(DropDownButton.ResizeModeProperty, (object) value);
    }

    public double MaxDropDownHeight
    {
      get => (double) this.GetValue(DropDownButton.MaxDropDownHeightProperty);
      set => this.SetValue(DropDownButton.MaxDropDownHeightProperty, (object) value);
    }

    public event EventHandler DropDownOpened;

    public event EventHandler DropDownClosed;

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static DropDownButton()
    {
      Type type = typeof (DropDownButton);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      KeyboardNavigation.TabNavigationProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Cycle));
      KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Cycle));
      KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) KeyboardNavigationMode.Cycle));
      ToolTipService.Attach(type);
      PopupService.Attach(type);
      ContextMenuService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(DropDownButton.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (DropDownButton));
      return basevalue;
    }

    public DropDownButton()
    {
      KeyboardNavigation.SetControlTabNavigation((DependencyObject) this, KeyboardNavigationMode.Cycle);
      KeyboardNavigation.SetDirectionalNavigation((DependencyObject) this, KeyboardNavigationMode.Cycle);
      KeyboardNavigation.SetTabNavigation((DependencyObject) this, KeyboardNavigationMode.Cycle);
      ContextMenuService.Coerce((DependencyObject) this);
      this.Focusable = false;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new MenuItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is FrameworkElement;

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (!this.buttonBorder.IsMouseOver)
        return;
      if (!this.IsDropDownOpen)
      {
        if (this.isFirstTime)
          this.popup.Opacity = 0.0;
        if (this.menuPanel != null)
        {
          if (this.scrollViewer != null && this.ResizeMode != ContextMenuResizeMode.None)
            this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
          this.menuPanel.Width = double.NaN;
          this.menuPanel.Height = double.NaN;
          this.menuPanel.Loaded += new RoutedEventHandler(this.OnMenuPanelLoaded);
        }
        this.IsDropDownOpen = true;
      }
      else
      {
        PopupService.RaiseDismissPopupEvent((object) this, DismissPopupMode.MouseNotOver);
        this.IsDropDownOpen = false;
      }
      this.focusedElement = Keyboard.FocusedElement;
      if (this.focusedElement != null)
      {
        this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
        this.focusedElement.PreviewKeyDown += new KeyEventHandler(this.OnFocusedElementPreviewKeyDown);
      }
      e.Handled = true;
      if (!this.isFirstTime)
        return;
      this.isFirstTime = false;
      this.IsDropDownOpen = false;
      this.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
      {
        if (this.menuPanel != null)
        {
          if (this.scrollViewer != null && this.ResizeMode != ContextMenuResizeMode.None)
            this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
          this.menuPanel.Width = double.NaN;
          this.menuPanel.Height = double.NaN;
          this.menuPanel.Loaded += new RoutedEventHandler(this.OnMenuPanelLoaded);
        }
        this.IsDropDownOpen = true;
        this.OnMenuPanelLoaded((object) null, (RoutedEventArgs) null);
        this.popup.Opacity = 1.0;
      }));
    }

    private void OnMenuPanelLoaded(object sender, RoutedEventArgs e)
    {
      this.menuPanel.Loaded -= new RoutedEventHandler(this.OnMenuPanelLoaded);
      if (this.ResizeMode == ContextMenuResizeMode.None)
        return;
      this.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
      {
        if (double.IsNaN(this.menuPanel.Width))
          this.menuPanel.Width = this.menuPanel.ActualWidth;
        if (double.IsNaN(this.menuPanel.Height))
          this.menuPanel.Height = this.menuPanel.ActualHeight;
        this.menuPanel.Width = Math.Max(this.menuPanel.ResizeMinWidth, this.menuPanel.Width);
        this.menuPanel.Height = Math.Min(Math.Max(this.menuPanel.ResizeMinHeight, this.menuPanel.Height), this.MaxDropDownHeight);
        if (this.scrollViewer == null)
          return;
        this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      }));
    }

    private void OnFocusedElementPreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Down)
      {
        if (!this.Items.Contains((object) Keyboard.FocusedElement))
          Keyboard.Focus(this.Items[0] as IInputElement);
        e.Handled = true;
      }
      else if (e.Key == Key.Up)
      {
        if (!this.Items.Contains((object) Keyboard.FocusedElement))
          Keyboard.Focus(this.Items[this.Items.Count - 1] as IInputElement);
        e.Handled = true;
      }
      else
      {
        if (e.Key != Key.Escape)
          return;
        this.IsDropDownOpen = false;
      }
    }

    private void OnFocusedElementLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      this.focusedElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
      this.focusedElement.PreviewKeyDown -= new KeyEventHandler(this.OnFocusedElementPreviewKeyDown);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        this.IsDropDownOpen = false;
      base.OnKeyDown(e);
    }

    public override void OnApplyTemplate()
    {
      this.isFirstTime = true;
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
      this.buttonBorder = this.GetTemplateChild("PART_ButtonBorder") as Border;
      this.menuPanel = this.GetTemplateChild("PART_MenuPanel") as MenuPanel;
      this.scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
      base.OnApplyTemplate();
    }

    public virtual void OnKeyTipPressed()
    {
      this.IsDropDownOpen = true;
      this.focusedElement = Keyboard.FocusedElement;
      if (this.focusedElement == null)
        return;
      this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
      this.focusedElement.PreviewKeyDown += new KeyEventHandler(this.OnFocusedElementPreviewKeyDown);
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
      if (this.DropDownClosed != null)
        this.DropDownClosed((object) this, e);
      if (Mouse.Captured != this)
        return;
      Mouse.Capture((IInputElement) null);
    }

    private void OnDropDownOpened(object sender, EventArgs e)
    {
      if (this.DropDownOpened != null)
        this.DropDownOpened((object) this, e);
      Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
    }

    public virtual FrameworkElement CreateQuickAccessItem()
    {
      DropDownButton quickAccessItem = new DropDownButton();
      quickAccessItem.Size = RibbonControlSize.Small;
      this.BindQuickAccessItem((FrameworkElement) quickAccessItem);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "DisplayMemberPath", ItemsControl.DisplayMemberPathProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "GroupStyleSelector", ItemsControl.GroupStyleSelectorProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemContainerStyle", ItemsControl.ItemContainerStyleProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemsPanel", ItemsControl.ItemsPanelProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemStringFormat", ItemsControl.ItemStringFormatProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemTemplate", ItemsControl.ItemTemplateProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "MaxDropDownHeight", DropDownButton.MaxDropDownHeightProperty, BindingMode.OneWay);
      quickAccessItem.DropDownOpened += new EventHandler(this.OnQuickAccessOpened);
      return (FrameworkElement) quickAccessItem;
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
      DropDownButton button = (DropDownButton) sender;
      button.DropDownClosed -= new EventHandler(this.OnQuickAccessMenuClosed);
      this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
      {
        if (button.ItemsSource != null)
        {
          this.ItemsSource = button.ItemsSource;
          button.ItemsSource = (IEnumerable) null;
        }
        else
        {
          for (int index = 0; index < button.Items.Count; index = index - 1 + 1)
          {
            object obj = button.Items[0];
            button.Items.Remove(obj);
            this.Items.Add(obj);
          }
        }
      }));
    }

    protected virtual void BindQuickAccessItem(FrameworkElement element)
    {
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, element);
      RibbonControl.Bind((object) this, element, "ResizeMode", DropDownButton.ResizeModeProperty, BindingMode.Default);
      RibbonControl.Bind((object) this, element, "MaxDropDownHeight", DropDownButton.MaxDropDownHeightProperty, BindingMode.Default);
      RibbonControl.Bind((object) this, element, "HasTriangle", DropDownButton.HasTriangleProperty, BindingMode.Default);
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(DropDownButton.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(DropDownButton.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(DropDownButton.QuickAccessElementStyleProperty);
      set => this.SetValue(DropDownButton.QuickAccessElementStyleProperty, (object) value);
    }
  }
}
