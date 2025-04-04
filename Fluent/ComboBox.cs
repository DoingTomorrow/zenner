// Decompiled with JetBrains decompiler
// Type: Fluent.ComboBox
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_ResizeVerticalThumb", Type = typeof (Thumb))]
  [TemplatePart(Name = "PART_ResizeBothThumb", Type = typeof (Thumb))]
  public class ComboBox : 
    System.Windows.Controls.ComboBox,
    IQuickAccessItemProvider,
    IRibbonControl,
    IKeyTipedControl,
    IDropDownControl
  {
    private Thumb resizeBothThumb;
    private Thumb resizeVerticalThumb;
    private Popup popup;
    private IInputElement focusedElement;
    private System.Windows.Controls.TextBox editableTextBox;
    private Panel menuPanel;
    private ContentPresenter contentSite;
    private Image snappedImage;
    private bool isSnapped;
    private GalleryPanel galleryPanel;
    private ScrollViewer scrollViewer;
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (ComboBox));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.AttachSizeDefinition(typeof (ComboBox));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (ComboBox));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (ComboBox), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ComboBox.OnIconChanged)));
    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof (Menu), typeof (RibbonMenu), typeof (ComboBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty InputWidthProperty = DependencyProperty.Register(nameof (InputWidth), typeof (double), typeof (ComboBox), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), typeof (double), typeof (ComboBox), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty GroupByProperty = DependencyProperty.Register(nameof (GroupBy), typeof (string), typeof (ComboBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register(nameof (ResizeMode), typeof (ContextMenuResizeMode), typeof (ComboBox), (PropertyMetadata) new UIPropertyMetadata((object) ContextMenuResizeMode.None));
    private bool isQuickAccessFocused;
    private bool isQuickAccessOpened;
    private object selectedItem;
    private ComboBox quickAccessCombo;
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (ComboBox), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (ComboBox));

    public Popup DropDownPopup => this.popup;

    public bool IsContextMenuOpened { get; set; }

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(ComboBox.SizeProperty);
      set => this.SetValue(ComboBox.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(ComboBox.SizeDefinitionProperty);
      set => this.SetValue(ComboBox.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => (object) (string) this.GetValue(ComboBox.HeaderProperty);
      set => this.SetValue(ComboBox.HeaderProperty, value);
    }

    public object Icon
    {
      get => (object) (ImageSource) this.GetValue(ComboBox.IconProperty);
      set => this.SetValue(ComboBox.IconProperty, value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ComboBox comboBox = d as ComboBox;
      if (e.OldValue is FrameworkElement oldValue)
        comboBox.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      comboBox.AddLogicalChild((object) newValue);
    }

    public RibbonMenu Menu
    {
      get => (RibbonMenu) this.GetValue(ComboBox.MenuProperty);
      set => this.SetValue(ComboBox.MenuProperty, (object) value);
    }

    public double InputWidth
    {
      get => (double) this.GetValue(ComboBox.InputWidthProperty);
      set => this.SetValue(ComboBox.InputWidthProperty, (object) value);
    }

    public double ItemHeight
    {
      get => (double) this.GetValue(ComboBox.ItemHeightProperty);
      set => this.SetValue(ComboBox.ItemHeightProperty, (object) value);
    }

    public string GroupBy
    {
      get => (string) this.GetValue(ComboBox.GroupByProperty);
      set => this.SetValue(ComboBox.GroupByProperty, (object) value);
    }

    public ContextMenuResizeMode ResizeMode
    {
      get => (ContextMenuResizeMode) this.GetValue(ComboBox.ResizeModeProperty);
      set => this.SetValue(ComboBox.ResizeModeProperty, (object) value);
    }

    private bool IsSnapped
    {
      get => this.isSnapped;
      set
      {
        if (value == this.isSnapped || this.snappedImage == null)
          return;
        if (value && (int) this.contentSite.ActualWidth > 0 && (int) this.contentSite.ActualHeight > 0)
        {
          RenderOptions.SetBitmapScalingMode((DependencyObject) this.snappedImage, BitmapScalingMode.NearestNeighbor);
          RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) this.contentSite.ActualWidth + (int) this.contentSite.Margin.Left + (int) this.contentSite.Margin.Right, (int) this.contentSite.ActualHeight + (int) this.contentSite.Margin.Top + (int) this.contentSite.Margin.Bottom, 96.0, 96.0, PixelFormats.Pbgra32);
          renderTargetBitmap.Render((Visual) this.contentSite);
          this.snappedImage.Source = (ImageSource) renderTargetBitmap;
          this.snappedImage.FlowDirection = this.FlowDirection;
          this.snappedImage.Visibility = Visibility.Visible;
          this.contentSite.Visibility = Visibility.Hidden;
          this.isSnapped = value;
        }
        else
        {
          this.snappedImage.Visibility = Visibility.Collapsed;
          this.contentSite.Visibility = Visibility.Visible;
          this.isSnapped = value;
        }
        this.InvalidateVisual();
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static ComboBox()
    {
      Type type = typeof (ComboBox);
      ToolTipService.Attach(type);
      PopupService.Attach(type);
      ContextMenuService.Attach(type);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      Selector.SelectedItemProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(ComboBox.OnSelectionItemChanged), new CoerceValueCallback(ComboBox.CoerceSelectedItem)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (ComboBox), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(ComboBox.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (ComboBox));
      return basevalue;
    }

    private static void OnSelectionItemChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ComboBox comboBox = d as ComboBox;
      if (comboBox.isQuickAccessOpened || comboBox.isQuickAccessFocused || comboBox.quickAccessCombo == null)
        return;
      comboBox.UpdateQuickAccessCombo();
    }

    private static object CoerceSelectedItem(DependencyObject d, object basevalue)
    {
      ComboBox comboBox = d as ComboBox;
      return comboBox.isQuickAccessOpened || comboBox.isQuickAccessFocused ? comboBox.selectedItem : basevalue;
    }

    public ComboBox() => ContextMenuService.Coerce((DependencyObject) this);

    public virtual FrameworkElement CreateQuickAccessItem()
    {
      ComboBox quickAccessItem = new ComboBox();
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) quickAccessItem);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "GroupBy", ComboBox.GroupByProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ActualWidth", FrameworkElement.WidthProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "InputWidth", ComboBox.InputWidthProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemHeight", ComboBox.ItemHeightProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "IsEditable", System.Windows.Controls.ComboBox.IsEditableProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "IsReadOnly", System.Windows.Controls.ComboBox.IsReadOnlyProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ResizeMode", ComboBox.ResizeModeProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "Text", System.Windows.Controls.ComboBox.TextProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "DisplayMemberPath", ItemsControl.DisplayMemberPathProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "GroupStyleSelector", ItemsControl.GroupStyleSelectorProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemContainerStyle", ItemsControl.ItemContainerStyleProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemsPanel", ItemsControl.ItemsPanelProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemStringFormat", ItemsControl.ItemStringFormatProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "ItemTemplate", ItemsControl.ItemTemplateProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "SelectedValuePath", Selector.SelectedValuePathProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "MaxDropDownHeight", System.Windows.Controls.ComboBox.MaxDropDownHeightProperty, BindingMode.OneWay);
      quickAccessItem.DropDownOpened += new EventHandler(this.OnQuickAccessOpened);
      if (this.IsEditable)
        quickAccessItem.GotFocus += new RoutedEventHandler(this.OnQuickAccessTextBoxGetFocus);
      this.quickAccessCombo = quickAccessItem;
      this.UpdateQuickAccessCombo();
      return (FrameworkElement) quickAccessItem;
    }

    private void OnQuickAccessTextBoxGetFocus(object sender, RoutedEventArgs e)
    {
      this.isQuickAccessFocused = true;
      if (!this.isQuickAccessOpened)
        this.Freeze();
      this.quickAccessCombo.LostFocus += new RoutedEventHandler(this.OnQuickAccessTextBoxLostFocus);
    }

    private void OnQuickAccessTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
      this.quickAccessCombo.LostFocus -= new RoutedEventHandler(this.OnQuickAccessTextBoxLostFocus);
      if (!this.isQuickAccessOpened)
        this.Unfreeze();
      this.isQuickAccessFocused = false;
    }

    private void OnQuickAccessOpened(object sender, EventArgs e)
    {
      this.isQuickAccessOpened = true;
      this.quickAccessCombo.DropDownClosed += new EventHandler(this.OnQuickAccessMenuClosed);
      this.quickAccessCombo.UpdateLayout();
      if (this.isQuickAccessFocused)
        return;
      this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() =>
      {
        this.Freeze();
        this.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Delegate) (() =>
        {
          if (this.quickAccessCombo.SelectedItem == null)
            return;
          (this.quickAccessCombo.ItemContainerGenerator.ContainerFromItem(this.quickAccessCombo.SelectedItem) as ComboBoxItem).BringIntoView();
        }));
      }));
    }

    private void OnQuickAccessMenuClosed(object sender, EventArgs e)
    {
      this.quickAccessCombo.DropDownClosed -= new EventHandler(this.OnQuickAccessMenuClosed);
      if (!this.isQuickAccessFocused)
        this.Unfreeze();
      this.isQuickAccessOpened = false;
    }

    private void Freeze()
    {
      this.IsSnapped = true;
      this.selectedItem = this.SelectedItem;
      if (this.ItemsSource != null)
      {
        this.quickAccessCombo.ItemsSource = this.ItemsSource;
        this.ItemsSource = (IEnumerable) null;
      }
      else
      {
        for (int index = 0; index < this.Items.Count; index = index - 1 + 1)
        {
          object obj = this.Items[0];
          this.Items.Remove(obj);
          this.quickAccessCombo.Items.Add(obj);
        }
      }
      this.SelectedItem = (object) null;
      this.quickAccessCombo.SelectedItem = this.selectedItem;
      this.quickAccessCombo.Menu = this.Menu;
      this.Menu = (RibbonMenu) null;
      this.quickAccessCombo.IsSnapped = false;
    }

    private void Unfreeze()
    {
      string text = this.quickAccessCombo.Text;
      this.selectedItem = this.quickAccessCombo.SelectedItem;
      this.quickAccessCombo.IsSnapped = true;
      if (this.quickAccessCombo.ItemsSource != null)
      {
        this.ItemsSource = this.quickAccessCombo.ItemsSource;
        this.quickAccessCombo.ItemsSource = (IEnumerable) null;
      }
      else
      {
        for (int index = 0; index < this.quickAccessCombo.Items.Count; index = index - 1 + 1)
        {
          object obj = this.quickAccessCombo.Items[0];
          this.quickAccessCombo.Items.Remove(obj);
          this.Items.Add(obj);
        }
      }
      this.quickAccessCombo.SelectedItem = (object) null;
      this.SelectedItem = this.selectedItem;
      this.Menu = this.quickAccessCombo.Menu;
      this.quickAccessCombo.Menu = (RibbonMenu) null;
      this.IsSnapped = false;
      this.Text = text;
      this.UpdateLayout();
    }

    private void UpdateQuickAccessCombo()
    {
      if (!this.IsLoaded)
        this.Loaded += new RoutedEventHandler(this.OnFirstLoaded);
      if (this.IsEditable)
        return;
      this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
      {
        this.quickAccessCombo.IsSnapped = true;
        this.IsSnapped = true;
        if (this.snappedImage != null && this.quickAccessCombo.snappedImage != null)
        {
          this.quickAccessCombo.snappedImage.Source = this.snappedImage.Source;
          this.quickAccessCombo.snappedImage.Visibility = Visibility.Visible;
          if (!this.quickAccessCombo.IsSnapped)
            this.quickAccessCombo.isSnapped = true;
        }
        this.IsSnapped = false;
      }));
    }

    private void OnFirstLoaded(object sender, RoutedEventArgs e)
    {
      this.Loaded -= new RoutedEventHandler(this.OnFirstLoaded);
      this.UpdateQuickAccessCombo();
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(ComboBox.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(ComboBox.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(ComboBox.QuickAccessElementStyleProperty);
      set => this.SetValue(ComboBox.QuickAccessElementStyleProperty, (object) value);
    }

    public override void OnApplyTemplate()
    {
      this.popup = this.GetTemplateChild("PART_Popup") as Popup;
      this.editableTextBox = this.GetTemplateChild("PART_EditableTextBox") as System.Windows.Controls.TextBox;
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
      this.menuPanel = this.GetTemplateChild("PART_MenuPanel") as Panel;
      this.snappedImage = this.GetTemplateChild("PART_SelectedImage") as Image;
      this.contentSite = this.GetTemplateChild("PART_ContentSite") as ContentPresenter;
      this.galleryPanel = this.GetTemplateChild("PART_GalleryPanel") as GalleryPanel;
      this.scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
      base.OnApplyTemplate();
    }

    protected override void OnDropDownOpened(EventArgs e)
    {
      this.Focus();
      base.OnDropDownOpened(e);
      Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
      if (this.SelectedItem != null)
        Keyboard.Focus(this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as IInputElement);
      this.focusedElement = Keyboard.FocusedElement;
      this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
    }

    protected override void OnDropDownClosed(EventArgs e)
    {
      base.OnDropDownClosed(e);
      if (Mouse.Captured == this)
        Mouse.Capture((IInputElement) null);
      if (this.focusedElement != null)
        this.focusedElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
      this.focusedElement = (IInputElement) null;
    }

    private void OnFocusedElementLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      if (this.focusedElement != null)
        this.focusedElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
      this.focusedElement = Keyboard.FocusedElement;
      if (this.focusedElement == null)
        return;
      this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusedElementLostKeyboardFocus);
      if (!this.IsEditable || !this.Items.Contains(this.ItemContainerGenerator.ItemFromContainer(Keyboard.FocusedElement as DependencyObject)))
        return;
      this.SelectedItem = this.ItemContainerGenerator.ItemFromContainer(Keyboard.FocusedElement as DependencyObject);
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
      if (this.IsEditable && (e.Key == Key.Down || e.Key == Key.Up) && !this.IsDropDownOpen)
      {
        this.IsDropDownOpen = true;
        e.Handled = true;
      }
      else if (this.IsEditable && (e.Key == Key.Return || e.Key == Key.Escape) && !this.IsDropDownOpen)
      {
        this.editableTextBox.Focusable = false;
        this.Focus();
        this.editableTextBox.Focusable = true;
        e.Handled = true;
      }
      else
        base.OnPreviewKeyDown(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (e.Key == Key.Down)
      {
        if (this.Menu != null && this.Menu.Items.Contains(this.Menu.ItemContainerGenerator.ItemFromContainer(Keyboard.FocusedElement as DependencyObject)))
        {
          int num = this.Menu.ItemContainerGenerator.IndexFromContainer(Keyboard.FocusedElement as DependencyObject);
          if (num != this.Menu.Items.Count - 1)
            Keyboard.Focus(this.Menu.ItemContainerGenerator.ContainerFromIndex(num + 1) as IInputElement);
          else if (this.Items.Count > 0 && !this.IsEditable)
            Keyboard.Focus(this.ItemContainerGenerator.ContainerFromIndex(0) as IInputElement);
          else
            Keyboard.Focus(this.Menu.Items[0] as IInputElement);
        }
        else if (this.Items.Contains(this.ItemContainerGenerator.ItemFromContainer(Keyboard.FocusedElement as DependencyObject)))
        {
          int num = this.ItemContainerGenerator.IndexFromContainer(Keyboard.FocusedElement as DependencyObject);
          if (num != this.Items.Count - 1)
            Keyboard.Focus(this.ItemContainerGenerator.ContainerFromIndex(num + 1) as IInputElement);
          else if (this.Menu != null && this.Menu.Items.Count > 0 && !this.IsEditable)
            Keyboard.Focus(this.Menu.ItemContainerGenerator.ContainerFromIndex(0) as IInputElement);
          else
            Keyboard.Focus(this.ItemContainerGenerator.ContainerFromIndex(0) as IInputElement);
        }
        else if (this.SelectedItem != null)
          Keyboard.Focus(this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as IInputElement);
        e.Handled = true;
      }
      else if (e.Key == Key.Up)
      {
        if (this.Menu != null && this.Menu.Items.Contains(this.Menu.ItemContainerGenerator.ItemFromContainer(Keyboard.FocusedElement as DependencyObject)))
        {
          int num = this.Menu.ItemContainerGenerator.IndexFromContainer(Keyboard.FocusedElement as DependencyObject);
          if (num != 0)
            Keyboard.Focus(this.Menu.ItemContainerGenerator.ContainerFromIndex(num - 1) as IInputElement);
          else if (this.Items.Count > 0 && !this.IsEditable)
            Keyboard.Focus(this.ItemContainerGenerator.ContainerFromIndex(this.Items.Count - 1) as IInputElement);
          else
            Keyboard.Focus(this.Menu.Items[this.Menu.Items.Count - 1] as IInputElement);
        }
        else if (this.Items.Contains(this.ItemContainerGenerator.ItemFromContainer(Keyboard.FocusedElement as DependencyObject)))
        {
          int num = this.ItemContainerGenerator.IndexFromContainer(Keyboard.FocusedElement as DependencyObject);
          if (num != 0)
            Keyboard.Focus(this.ItemContainerGenerator.ContainerFromIndex(num - 1) as IInputElement);
          else if (this.Menu != null && this.Menu.Items.Count > 0 && !this.IsEditable)
            Keyboard.Focus(this.Menu.ItemContainerGenerator.ContainerFromIndex(this.Menu.Items.Count - 1) as IInputElement);
          else
            Keyboard.Focus(this.ItemContainerGenerator.ContainerFromIndex(this.Items.Count - 1) as IInputElement);
        }
        else if (this.SelectedItem != null)
          Keyboard.Focus(this.ItemContainerGenerator.ContainerFromItem(this.SelectedItem) as IInputElement);
        e.Handled = true;
      }
      else
      {
        if (e.Key == Key.Return && !this.IsEditable)
          this.SelectedIndex = this.ItemContainerGenerator.IndexFromContainer(Keyboard.FocusedElement as DependencyObject);
        base.OnKeyDown(e);
      }
    }

    public virtual void OnKeyTipPressed()
    {
      if (this.IsEditable)
        this.Focus();
      else
        this.IsDropDownOpen = true;
    }

    private void OnResizeBothDelta(object sender, DragDeltaEventArgs e)
    {
      if (double.IsNaN(this.scrollViewer.Height))
        this.scrollViewer.Height = this.scrollViewer.ActualHeight;
      this.scrollViewer.Height = Math.Min(Math.Max(this.galleryPanel.GetItemSize().Height, this.scrollViewer.Height + e.VerticalChange), this.MaxDropDownHeight);
      this.menuPanel.Width = double.NaN;
      if (double.IsNaN(this.galleryPanel.Width))
        this.galleryPanel.Width = 500.0;
      this.galleryPanel.Width = Math.Max(this.galleryPanel.Width + e.HorizontalChange, 0.0);
    }

    private void OnResizeVerticalDelta(object sender, DragDeltaEventArgs e)
    {
      if (double.IsNaN(this.scrollViewer.Height))
        this.scrollViewer.Height = this.scrollViewer.ActualHeight;
      this.scrollViewer.Height = Math.Min(Math.Max(this.galleryPanel.GetItemSize().Height, this.scrollViewer.Height + e.VerticalChange), this.MaxDropDownHeight);
    }

    [SpecialName]
    bool IDropDownControl.get_IsDropDownOpen() => this.IsDropDownOpen;

    [SpecialName]
    void IDropDownControl.set_IsDropDownOpen([In] bool obj0) => this.IsDropDownOpen = obj0;
  }
}
