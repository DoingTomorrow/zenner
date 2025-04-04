// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonGroupBox
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_UpPanel", Type = typeof (Panel))]
  [TemplatePart(Name = "PART_DialogLauncherButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_Popup", Type = typeof (Popup))]
  [TemplatePart(Name = "PART_DownGrid", Type = typeof (Grid))]
  public class RibbonGroupBox : ItemsControl, IQuickAccessItemProvider, IDropDownControl
  {
    private Popup popup;
    private Grid downGrid;
    private Panel upPanel;
    private Panel parentPanel;
    private Image snappedImage;
    private bool isSnapped;
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof (State), typeof (RibbonGroupBoxState), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) RibbonGroupBoxState.Large, new PropertyChangedCallback(RibbonGroupBox.StatePropertyChanged)));
    private int scale;
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (string), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) nameof (RibbonGroupBox)));
    public static readonly DependencyProperty IsLauncherVisibleProperty = DependencyProperty.Register(nameof (IsLauncherVisible), typeof (bool), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty DialogLauncherButtonKeyTipKeysProperty = DependencyProperty.Register(nameof (LauncherKeys), typeof (string), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonGroupBox.OnDialogLauncherButtonKeyTipKeysChanged)));
    public static readonly DependencyProperty LauncherIconProperty = DependencyProperty.Register(nameof (LauncherIcon), typeof (ImageSource), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LauncherTextProperty = DependencyProperty.Register(nameof (LauncherText), typeof (string), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LauncherCommandParameterProperty = DependencyProperty.Register(nameof (LauncherCommandParameter), typeof (object), typeof (RibbonGroupBox), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LauncherCommandProperty = DependencyProperty.Register(nameof (LauncherCommand), typeof (ICommand), typeof (RibbonGroupBox), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LauncherCommandTargetProperty = DependencyProperty.Register(nameof (LauncherCommandTarget), typeof (IInputElement), typeof (RibbonGroupBox), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LauncherToolTipProperty = DependencyProperty.Register(nameof (LauncherToolTip), typeof (object), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsLauncherEnabledProperty = DependencyProperty.Register(nameof (IsLauncherEnabled), typeof (bool), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) true));
    private static readonly DependencyPropertyKey LauncherButtonPropertyKey = DependencyProperty.RegisterReadOnly(nameof (LauncherButton), typeof (Button), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LauncherButtonProperty = RibbonGroupBox.LauncherButtonPropertyKey.DependencyProperty;
    public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(RibbonGroupBox.OnIsOpenChanged), new CoerceValueCallback(RibbonGroupBox.CoerceIsDropDownOpen)));
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RibbonGroupBox.OnIconChanged)));
    private readonly Dictionary<RibbonGroupBox.StateScale, Size> cachedMeasures = new Dictionary<RibbonGroupBox.StateScale, Size>();
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = DependencyProperty.Register(nameof (CanAddToQuickAccessToolBar), typeof (bool), typeof (RibbonGroupBox), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (RibbonGroupBox));

    public Popup DropDownPopup => this.popup;

    public bool IsContextMenuOpened { get; set; }

    public RibbonGroupBoxState State
    {
      get => (RibbonGroupBoxState) this.GetValue(RibbonGroupBox.StateProperty);
      set => this.SetValue(RibbonGroupBox.StateProperty, (object) value);
    }

    private static void StatePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonGroupBox ribbonGroupBox = (RibbonGroupBox) d;
      RibbonGroupBox.SetChildSizes((RibbonGroupBoxState) e.NewValue, ribbonGroupBox);
    }

    private static void SetChildSizes(
      RibbonGroupBoxState ribbonGroupBoxState,
      RibbonGroupBox ribbonGroupBox)
    {
      for (int index = 0; index < ribbonGroupBox.Items.Count; ++index)
        RibbonGroupBox.SetAppropriateSizeRecursive((UIElement) ribbonGroupBox.Items[index], ribbonGroupBoxState);
    }

    private static void SetAppropriateSizeRecursive(
      UIElement root,
      RibbonGroupBoxState ribbonGroupBoxState)
    {
      if (root == null)
        return;
      if (root is IRibbonControl)
      {
        RibbonControl.SetAppropriateSize(root, ribbonGroupBoxState);
      }
      else
      {
        int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject) root);
        for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
          RibbonGroupBox.SetAppropriateSizeRecursive(VisualTreeHelper.GetChild((DependencyObject) root, childIndex) as UIElement, ribbonGroupBoxState);
      }
    }

    internal int Scale
    {
      get => this.scale;
      set
      {
        int num = value - this.scale;
        this.scale = value;
        for (int index = 0; index < Math.Abs(num); ++index)
        {
          if (num > 0)
            this.IncreaseScalableElement();
          else
            this.DecreaseScalableElement();
        }
      }
    }

    private void IncreaseScalableElement()
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        if (obj is IScalableRibbonControl scalableRibbonControl)
          scalableRibbonControl.Enlarge();
      }
    }

    private void OnScalableControlScaled(object sender, EventArgs e)
    {
      if (this.SuppressCacheReseting)
        return;
      this.cachedMeasures.Clear();
    }

    internal bool SuppressCacheReseting { get; set; }

    private void DecreaseScalableElement()
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        if (obj is IScalableRibbonControl scalableRibbonControl)
          scalableRibbonControl.Reduce();
      }
    }

    private void UpdateScalableControlSubscribing()
    {
      foreach (object obj in (IEnumerable) this.Items)
      {
        if (obj is IScalableRibbonControl scalableRibbonControl)
        {
          scalableRibbonControl.Scaled -= new EventHandler(this.OnScalableControlScaled);
          scalableRibbonControl.Scaled += new EventHandler(this.OnScalableControlScaled);
        }
      }
    }

    public string Header
    {
      get => (string) this.GetValue(RibbonGroupBox.HeaderProperty);
      set => this.SetValue(RibbonGroupBox.HeaderProperty, (object) value);
    }

    public bool IsLauncherVisible
    {
      get => (bool) this.GetValue(RibbonGroupBox.IsLauncherVisibleProperty);
      set => this.SetValue(RibbonGroupBox.IsLauncherVisibleProperty, (object) value);
    }

    [Category("KeyTips")]
    [Description("Key tip keys for dialog launcher button")]
    [DisplayName("DialogLauncher Keys")]
    public string LauncherKeys
    {
      get => (string) this.GetValue(RibbonGroupBox.DialogLauncherButtonKeyTipKeysProperty);
      set => this.SetValue(RibbonGroupBox.DialogLauncherButtonKeyTipKeysProperty, (object) value);
    }

    private static void OnDialogLauncherButtonKeyTipKeysChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonGroupBox ribbonGroupBox = (RibbonGroupBox) d;
      if (ribbonGroupBox.LauncherButton == null)
        return;
      KeyTip.SetKeys((DependencyObject) ribbonGroupBox.LauncherButton, (string) e.NewValue);
    }

    public ImageSource LauncherIcon
    {
      get => (ImageSource) this.GetValue(RibbonGroupBox.LauncherIconProperty);
      set => this.SetValue(RibbonGroupBox.LauncherIconProperty, (object) value);
    }

    public string LauncherText
    {
      get => (string) this.GetValue(RibbonGroupBox.LauncherTextProperty);
      set => this.SetValue(RibbonGroupBox.LauncherTextProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Action")]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public ICommand LauncherCommand
    {
      get => (ICommand) this.GetValue(RibbonGroupBox.LauncherCommandProperty);
      set => this.SetValue(RibbonGroupBox.LauncherCommandProperty, (object) value);
    }

    [Category("Action")]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [Bindable(true)]
    public object LauncherCommandParameter
    {
      get => this.GetValue(RibbonGroupBox.LauncherCommandParameterProperty);
      set => this.SetValue(RibbonGroupBox.LauncherCommandParameterProperty, value);
    }

    [Category("Action")]
    [Bindable(true)]
    public IInputElement LauncherCommandTarget
    {
      get => (IInputElement) this.GetValue(RibbonGroupBox.LauncherCommandTargetProperty);
      set => this.SetValue(RibbonGroupBox.LauncherCommandTargetProperty, (object) value);
    }

    public object LauncherToolTip
    {
      get => this.GetValue(RibbonGroupBox.LauncherToolTipProperty);
      set => this.SetValue(RibbonGroupBox.LauncherToolTipProperty, value);
    }

    public bool IsLauncherEnabled
    {
      get => (bool) this.GetValue(RibbonGroupBox.IsLauncherEnabledProperty);
      set => this.SetValue(RibbonGroupBox.IsLauncherEnabledProperty, (object) value);
    }

    public Button LauncherButton
    {
      get => (Button) this.GetValue(RibbonGroupBox.LauncherButtonProperty);
      private set => this.SetValue(RibbonGroupBox.LauncherButtonPropertyKey, (object) value);
    }

    public bool IsDropDownOpen
    {
      get => (bool) this.GetValue(RibbonGroupBox.IsDropDownOpenProperty);
      set => this.SetValue(RibbonGroupBox.IsDropDownOpenProperty, (object) value);
    }

    private static object CoerceIsDropDownOpen(DependencyObject d, object basevalue)
    {
      return (d as RibbonGroupBox).State != RibbonGroupBoxState.Collapsed ? (object) false : basevalue;
    }

    protected override IEnumerator LogicalChildren
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        arrayList.AddRange((ICollection) this.Items);
        if (this.LauncherButton != null)
          arrayList.Add((object) this.LauncherButton);
        return arrayList.GetEnumerator();
      }
    }

    public ImageSource Icon
    {
      get => (ImageSource) this.GetValue(RibbonGroupBox.IconProperty);
      set => this.SetValue(RibbonGroupBox.IconProperty, (object) value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RibbonGroupBox ribbonGroupBox = d as RibbonGroupBox;
      if (e.OldValue is FrameworkElement oldValue)
        ribbonGroupBox.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      ribbonGroupBox.AddLogicalChild((object) newValue);
    }

    public event RoutedEventHandler LauncherClick;

    public event EventHandler DropDownOpened;

    public event EventHandler DropDownClosed;

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonGroupBox()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonGroupBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonGroupBox)));
      UIElement.VisibilityProperty.AddOwner(typeof (RibbonGroupBox), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(RibbonGroupBox.OnVisibilityChanged)));
      FrameworkElement.ContextMenuProperty.AddOwner(typeof (RibbonGroupBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(RibbonGroupBox.OnContextMenuChanged), new CoerceValueCallback(RibbonGroupBox.CoerceContextMenu)));
      PopupService.Attach(typeof (RibbonGroupBox));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonGroupBox), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonGroupBox.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonGroupBox));
      return basevalue;
    }

    private static void OnContextMenuChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(FrameworkElement.ContextMenuProperty);
    }

    private static object CoerceContextMenu(DependencyObject d, object basevalue)
    {
      return basevalue == null ? (object) Ribbon.RibbonContextMenu : basevalue;
    }

    private static void OnVisibilityChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as RibbonGroupBox).ClearCache();
    }

    public RibbonGroupBox()
    {
      this.ToolTip = (object) new System.Windows.Controls.ToolTip();
      (this.ToolTip as System.Windows.Controls.ToolTip).Template = (ControlTemplate) null;
      this.CoerceValue(FrameworkElement.ContextMenuProperty);
      this.Focusable = false;
      FocusManager.SetIsFocusScope((DependencyObject) this, false);
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
      if (this.State != RibbonGroupBoxState.Collapsed)
        return;
      this.IsDropDownOpen = true;
      e.Handled = true;
    }

    internal Panel GetPanel() => this.upPanel;

    internal Panel GetLayoutRoot() => this.parentPanel;

    public bool IsSnapped
    {
      get => this.isSnapped;
      set
      {
        if (value == this.isSnapped)
          return;
        if (value)
        {
          if (this.IsVisible)
          {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) this.ActualWidth, (int) this.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
            renderTargetBitmap.Render((Visual) VisualTreeHelper.GetChild((DependencyObject) this, 0));
            this.snappedImage.FlowDirection = this.FlowDirection;
            this.snappedImage.Source = (ImageSource) renderTargetBitmap;
            this.snappedImage.Width = this.ActualWidth;
            this.snappedImage.Height = this.ActualHeight;
            this.snappedImage.Visibility = Visibility.Visible;
            this.isSnapped = value;
          }
        }
        else if (this.snappedImage != null)
        {
          this.snappedImage.Visibility = Visibility.Collapsed;
          this.isSnapped = value;
        }
        this.InvalidateVisual();
      }
    }

    internal RibbonGroupBoxState StateIntermediate { get; set; }

    internal int ScaleIntermediate { get; set; }

    internal Size DesiredSizeIntermediate
    {
      get
      {
        RibbonGroupBox.StateScale key = new RibbonGroupBox.StateScale()
        {
          Scale = this.ScaleIntermediate,
          State = this.StateIntermediate
        };
        Size desiredSize;
        if (!this.cachedMeasures.TryGetValue(key, out desiredSize))
        {
          this.SuppressCacheReseting = true;
          this.UpdateScalableControlSubscribing();
          RibbonGroupBoxState state = this.State;
          int scale = this.Scale;
          this.State = this.StateIntermediate;
          this.Scale = this.ScaleIntermediate;
          this.InvalidateLayout();
          this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
          this.cachedMeasures.Add(key, this.DesiredSize);
          desiredSize = this.DesiredSize;
          this.State = state;
          this.Scale = scale;
          this.InvalidateLayout();
          this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
          this.SuppressCacheReseting = false;
        }
        return desiredSize;
      }
    }

    public void ClearCache() => this.cachedMeasures.Clear();

    internal void InvalidateLayout() => RibbonGroupBox.InvalidateMeasureRecursive((UIElement) this);

    private static void InvalidateMeasureRecursive(UIElement element)
    {
      if (element == null)
        return;
      element.InvalidateMeasure();
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) element); ++childIndex)
        RibbonGroupBox.InvalidateMeasureRecursive(VisualTreeHelper.GetChild((DependencyObject) element, childIndex) as UIElement);
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems != null)
      {
        foreach (UIElement newItem in (IEnumerable) e.NewItems)
          RibbonControl.SetAppropriateSize(newItem, this.State);
      }
      base.OnItemsChanged(e);
    }

    public override void OnApplyTemplate()
    {
      this.cachedMeasures.Clear();
      if (this.LauncherButton != null)
        this.LauncherButton.Click -= new RoutedEventHandler(this.OnDialogLauncherButtonClick);
      this.LauncherButton = this.GetTemplateChild("PART_DialogLauncherButton") as Button;
      if (this.LauncherButton != null)
      {
        this.LauncherButton.Click += new RoutedEventHandler(this.OnDialogLauncherButtonClick);
        if (this.LauncherKeys != null)
          KeyTip.SetKeys((DependencyObject) this.LauncherButton, this.LauncherKeys);
      }
      if (this.popup != null)
      {
        this.popup.Opened -= new EventHandler(this.OnPopupOpened);
        this.popup.Closed -= new EventHandler(this.OnPopupClosed);
      }
      this.popup = this.GetTemplateChild("PART_Popup") as Popup;
      if (this.popup != null)
      {
        this.popup.Opened += new EventHandler(this.OnPopupOpened);
        this.popup.Closed += new EventHandler(this.OnPopupClosed);
      }
      this.downGrid = this.GetTemplateChild("PART_DownGrid") as Grid;
      this.upPanel = this.GetTemplateChild("PART_UpPanel") as Panel;
      this.parentPanel = this.GetTemplateChild("PART_ParentPanel") as Panel;
      this.snappedImage = this.GetTemplateChild("PART_SnappedImage") as Image;
    }

    private void OnPopupOpened(object sender, EventArgs e)
    {
      if (this.DropDownOpened == null)
        return;
      this.DropDownOpened((object) this, e);
    }

    private void OnPopupClosed(object sender, EventArgs e)
    {
      if (this.DropDownClosed == null)
        return;
      this.DropDownClosed((object) this, e);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (this.State != RibbonGroupBoxState.Collapsed || this.popup == null)
        return;
      e.Handled = true;
      if (!this.IsDropDownOpen)
        this.IsDropDownOpen = true;
      else
        PopupService.RaiseDismissPopupEvent((object) this, DismissPopupMode.MouseNotOver);
    }

    private void OnDialogLauncherButtonClick(object sender, RoutedEventArgs e)
    {
      if (this.LauncherClick == null)
        return;
      this.LauncherClick((object) this, e);
    }

    private void OnRibbonGroupBoxPopupClosing()
    {
      if (Mouse.Captured != this)
        return;
      Mouse.Capture((IInputElement) null);
    }

    private void OnRibbonGroupBoxPopupOpening()
    {
      Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
    }

    private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RibbonGroupBox ribbonGroupBox = (RibbonGroupBox) d;
      if (ribbonGroupBox.IsDropDownOpen)
        ribbonGroupBox.OnRibbonGroupBoxPopupOpening();
      else
        ribbonGroupBox.OnRibbonGroupBoxPopupClosing();
    }

    public FrameworkElement CreateQuickAccessItem()
    {
      RibbonGroupBox target = new RibbonGroupBox();
      target.DropDownOpened += new EventHandler(this.OnQuickAccessOpened);
      target.DropDownClosed += new EventHandler(this.OnQuickAccessClosed);
      target.State = RibbonGroupBoxState.Collapsed;
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Icon", RibbonControl.IconProperty, BindingMode.OneWay);
      if (this.QuickAccessElementStyle != null)
        RibbonControl.Bind((object) this, (FrameworkElement) target, "QuickAccessElementStyle", FrameworkElement.StyleProperty, BindingMode.OneWay);
      return (FrameworkElement) target;
    }

    private void OnQuickAccessOpened(object sender, EventArgs e)
    {
      if (this.IsDropDownOpen || this.IsSnapped)
        return;
      RibbonGroupBox ribbonGroupBox = sender as RibbonGroupBox;
      this.IsSnapped = true;
      for (int index = 0; index < this.Items.Count; index = index - 1 + 1)
      {
        object obj = this.Items[0];
        this.Items.Remove(obj);
        ribbonGroupBox.Items.Add(obj);
      }
    }

    private void OnQuickAccessClosed(object sender, EventArgs e)
    {
      RibbonGroupBox ribbonGroupBox = sender as RibbonGroupBox;
      for (int index = 0; index < ribbonGroupBox.Items.Count; index = index - 1 + 1)
      {
        object obj = ribbonGroupBox.Items[0];
        ribbonGroupBox.Items.Remove(obj);
        this.Items.Add(obj);
      }
      this.IsSnapped = false;
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(RibbonGroupBox.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(RibbonGroupBox.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(RibbonGroupBox.QuickAccessElementStyleProperty);
      set => this.SetValue(RibbonGroupBox.QuickAccessElementStyleProperty, (object) value);
    }

    private struct StateScale
    {
      public RibbonGroupBoxState State;
      public int Scale;
    }
  }
}
