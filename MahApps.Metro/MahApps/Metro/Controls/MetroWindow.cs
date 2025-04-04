// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MetroWindow
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Native;
using Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_Icon", Type = typeof (UIElement))]
  [TemplatePart(Name = "PART_TitleBar", Type = typeof (UIElement))]
  [TemplatePart(Name = "PART_WindowTitleBackground", Type = typeof (UIElement))]
  [TemplatePart(Name = "PART_WindowTitleThumb", Type = typeof (Thumb))]
  [TemplatePart(Name = "PART_LeftWindowCommands", Type = typeof (WindowCommands))]
  [TemplatePart(Name = "PART_RightWindowCommands", Type = typeof (WindowCommands))]
  [TemplatePart(Name = "PART_WindowButtonCommands", Type = typeof (WindowButtonCommands))]
  [TemplatePart(Name = "PART_OverlayBox", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_MetroActiveDialogContainer", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_MetroInactiveDialogsContainer", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_FlyoutModal", Type = typeof (Rectangle))]
  public class MetroWindow : Window
  {
    private const string PART_Icon = "PART_Icon";
    private const string PART_TitleBar = "PART_TitleBar";
    private const string PART_WindowTitleBackground = "PART_WindowTitleBackground";
    private const string PART_WindowTitleThumb = "PART_WindowTitleThumb";
    private const string PART_LeftWindowCommands = "PART_LeftWindowCommands";
    private const string PART_RightWindowCommands = "PART_RightWindowCommands";
    private const string PART_WindowButtonCommands = "PART_WindowButtonCommands";
    private const string PART_OverlayBox = "PART_OverlayBox";
    private const string PART_MetroActiveDialogContainer = "PART_MetroActiveDialogContainer";
    private const string PART_MetroInactiveDialogsContainer = "PART_MetroInactiveDialogsContainer";
    private const string PART_FlyoutModal = "PART_FlyoutModal";
    public static readonly DependencyProperty ShowIconOnTitleBarProperty = DependencyProperty.Register(nameof (ShowIconOnTitleBar), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(MetroWindow.OnShowIconOnTitleBarPropertyChangedCallback)));
    public static readonly DependencyProperty IconEdgeModeProperty = DependencyProperty.Register(nameof (IconEdgeMode), typeof (EdgeMode), typeof (MetroWindow), new PropertyMetadata((object) EdgeMode.Aliased));
    public static readonly DependencyProperty IconBitmapScalingModeProperty = DependencyProperty.Register(nameof (IconBitmapScalingMode), typeof (BitmapScalingMode), typeof (MetroWindow), new PropertyMetadata((object) BitmapScalingMode.HighQuality));
    public static readonly DependencyProperty ShowTitleBarProperty = DependencyProperty.Register(nameof (ShowTitleBar), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(MetroWindow.OnShowTitleBarPropertyChangedCallback), new CoerceValueCallback(MetroWindow.OnShowTitleBarCoerceValueCallback)));
    public static readonly DependencyProperty ShowMinButtonProperty = DependencyProperty.Register(nameof (ShowMinButton), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty ShowMaxRestoreButtonProperty = DependencyProperty.Register(nameof (ShowMaxRestoreButton), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(nameof (ShowCloseButton), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty IsMinButtonEnabledProperty = DependencyProperty.Register(nameof (IsMinButtonEnabled), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty IsMaxRestoreButtonEnabledProperty = DependencyProperty.Register(nameof (IsMaxRestoreButtonEnabled), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty IsCloseButtonEnabledProperty = DependencyProperty.Register(nameof (IsCloseButtonEnabled), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty ShowSystemMenuOnRightClickProperty = DependencyProperty.Register(nameof (ShowSystemMenuOnRightClick), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty TitlebarHeightProperty = DependencyProperty.Register(nameof (TitlebarHeight), typeof (int), typeof (MetroWindow), new PropertyMetadata((object) 30, new PropertyChangedCallback(MetroWindow.TitlebarHeightPropertyChangedCallback)));
    public static readonly DependencyProperty TitleCapsProperty = DependencyProperty.Register(nameof (TitleCaps), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.Register(nameof (TitleAlignment), typeof (HorizontalAlignment), typeof (MetroWindow), new PropertyMetadata((object) HorizontalAlignment.Stretch));
    public static readonly DependencyProperty SaveWindowPositionProperty = DependencyProperty.Register(nameof (SaveWindowPosition), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) false));
    public static readonly DependencyProperty WindowPlacementSettingsProperty = DependencyProperty.Register(nameof (WindowPlacementSettings), typeof (IWindowPlacementSettings), typeof (MetroWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register(nameof (TitleForeground), typeof (Brush), typeof (MetroWindow));
    public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register(nameof (IgnoreTaskbarOnMaximize), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) false));
    public static readonly DependencyProperty FlyoutsProperty = DependencyProperty.Register(nameof (Flyouts), typeof (FlyoutsControl), typeof (MetroWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty WindowTransitionsEnabledProperty = DependencyProperty.Register(nameof (WindowTransitionsEnabled), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    public static readonly DependencyProperty MetroDialogOptionsProperty = DependencyProperty.Register(nameof (MetroDialogOptions), typeof (MetroDialogSettings), typeof (MetroWindow), new PropertyMetadata((object) new MetroDialogSettings()));
    public static readonly DependencyProperty WindowTitleBrushProperty = DependencyProperty.Register(nameof (WindowTitleBrush), typeof (Brush), typeof (MetroWindow), new PropertyMetadata((object) Brushes.Transparent));
    public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register(nameof (GlowBrush), typeof (Brush), typeof (MetroWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty NonActiveGlowBrushProperty = DependencyProperty.Register(nameof (NonActiveGlowBrush), typeof (Brush), typeof (MetroWindow), new PropertyMetadata((object) new SolidColorBrush(Color.FromRgb((byte) 153, (byte) 153, (byte) 153))));
    public static readonly DependencyProperty NonActiveBorderBrushProperty = DependencyProperty.Register(nameof (NonActiveBorderBrush), typeof (Brush), typeof (MetroWindow), new PropertyMetadata((object) Brushes.Gray));
    public static readonly DependencyProperty NonActiveWindowTitleBrushProperty = DependencyProperty.Register(nameof (NonActiveWindowTitleBrush), typeof (Brush), typeof (MetroWindow), new PropertyMetadata((object) Brushes.Gray));
    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(nameof (IconTemplate), typeof (DataTemplate), typeof (MetroWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof (TitleTemplate), typeof (DataTemplate), typeof (MetroWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LeftWindowCommandsProperty = DependencyProperty.Register(nameof (LeftWindowCommands), typeof (WindowCommands), typeof (MetroWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty RightWindowCommandsProperty = DependencyProperty.Register(nameof (RightWindowCommands), typeof (WindowCommands), typeof (MetroWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty LeftWindowCommandsOverlayBehaviorProperty = DependencyProperty.Register(nameof (LeftWindowCommandsOverlayBehavior), typeof (WindowCommandsOverlayBehavior), typeof (MetroWindow), new PropertyMetadata((object) WindowCommandsOverlayBehavior.Always));
    public static readonly DependencyProperty RightWindowCommandsOverlayBehaviorProperty = DependencyProperty.Register(nameof (RightWindowCommandsOverlayBehavior), typeof (WindowCommandsOverlayBehavior), typeof (MetroWindow), new PropertyMetadata((object) WindowCommandsOverlayBehavior.Always));
    public static readonly DependencyProperty WindowButtonCommandsOverlayBehaviorProperty = DependencyProperty.Register(nameof (WindowButtonCommandsOverlayBehavior), typeof (WindowCommandsOverlayBehavior), typeof (MetroWindow), new PropertyMetadata((object) WindowCommandsOverlayBehavior.Always));
    public static readonly DependencyProperty IconOverlayBehaviorProperty = DependencyProperty.Register(nameof (IconOverlayBehavior), typeof (WindowCommandsOverlayBehavior), typeof (MetroWindow), new PropertyMetadata((object) WindowCommandsOverlayBehavior.Never));
    [Obsolete("This property will be deleted in the next release. You should use LightMinButtonStyle or DarkMinButtonStyle in WindowButtonCommands to override the style.")]
    public static readonly DependencyProperty WindowMinButtonStyleProperty = DependencyProperty.Register(nameof (WindowMinButtonStyle), typeof (Style), typeof (MetroWindow), new PropertyMetadata((object) null, new PropertyChangedCallback(MetroWindow.OnWindowButtonStyleChanged)));
    [Obsolete("This property will be deleted in the next release. You should use LightMaxButtonStyle or DarkMaxButtonStyle in WindowButtonCommands to override the style.")]
    public static readonly DependencyProperty WindowMaxButtonStyleProperty = DependencyProperty.Register(nameof (WindowMaxButtonStyle), typeof (Style), typeof (MetroWindow), new PropertyMetadata((object) null, new PropertyChangedCallback(MetroWindow.OnWindowButtonStyleChanged)));
    [Obsolete("This property will be deleted in the next release. You should use LightCloseButtonStyle or DarkCloseButtonStyle in WindowButtonCommands to override the style.")]
    public static readonly DependencyProperty WindowCloseButtonStyleProperty = DependencyProperty.Register(nameof (WindowCloseButtonStyle), typeof (Style), typeof (MetroWindow), new PropertyMetadata((object) null, new PropertyChangedCallback(MetroWindow.OnWindowButtonStyleChanged)));
    public static readonly DependencyProperty UseNoneWindowStyleProperty = DependencyProperty.Register(nameof (UseNoneWindowStyle), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) false, new PropertyChangedCallback(MetroWindow.OnUseNoneWindowStylePropertyChangedCallback)));
    public static readonly DependencyProperty OverrideDefaultWindowCommandsBrushProperty = DependencyProperty.Register(nameof (OverrideDefaultWindowCommandsBrush), typeof (SolidColorBrush), typeof (MetroWindow));
    [Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" to get a drop shadow around the Window.")]
    public static readonly DependencyProperty EnableDWMDropShadowProperty = DependencyProperty.Register(nameof (EnableDWMDropShadow), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) false, new PropertyChangedCallback(MetroWindow.OnEnableDWMDropShadowPropertyChangedCallback)));
    public static readonly DependencyProperty IsWindowDraggableProperty = DependencyProperty.Register(nameof (IsWindowDraggable), typeof (bool), typeof (MetroWindow), new PropertyMetadata((object) true));
    private UIElement icon;
    private UIElement titleBar;
    private UIElement titleBarBackground;
    private Thumb windowTitleThumb;
    internal ContentPresenter LeftWindowCommandsPresenter;
    internal ContentPresenter RightWindowCommandsPresenter;
    internal WindowButtonCommands WindowButtonCommands;
    internal Grid overlayBox;
    internal Grid metroActiveDialogContainer;
    internal Grid metroInactiveDialogContainer;
    private Storyboard overlayStoryboard;
    private Rectangle flyoutModal;
    public static readonly RoutedEvent FlyoutsStatusChangedEvent = EventManager.RegisterRoutedEvent("FlyoutsStatusChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (MetroWindow));

    public event RoutedEventHandler FlyoutsStatusChanged
    {
      add => this.AddHandler(MetroWindow.FlyoutsStatusChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(MetroWindow.FlyoutsStatusChangedEvent, (Delegate) value);
    }

    public SolidColorBrush OverrideDefaultWindowCommandsBrush
    {
      get
      {
        return (SolidColorBrush) this.GetValue(MetroWindow.OverrideDefaultWindowCommandsBrushProperty);
      }
      set => this.SetValue(MetroWindow.OverrideDefaultWindowCommandsBrushProperty, (object) value);
    }

    public MetroDialogSettings MetroDialogOptions
    {
      get => (MetroDialogSettings) this.GetValue(MetroWindow.MetroDialogOptionsProperty);
      set => this.SetValue(MetroWindow.MetroDialogOptionsProperty, (object) value);
    }

    [Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" to get a drop shadow around the Window.")]
    public bool EnableDWMDropShadow
    {
      get => (bool) this.GetValue(MetroWindow.EnableDWMDropShadowProperty);
      set => this.SetValue(MetroWindow.EnableDWMDropShadowProperty, (object) value);
    }

    private static void OnEnableDWMDropShadowPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue || !(bool) e.NewValue)
        return;
      ((MetroWindow) d).UseDropShadow();
    }

    private void UseDropShadow()
    {
      this.BorderThickness = new Thickness(0.0);
      this.BorderBrush = (Brush) null;
      this.GlowBrush = (Brush) Brushes.Black;
    }

    public bool IsWindowDraggable
    {
      get => (bool) this.GetValue(MetroWindow.IsWindowDraggableProperty);
      set => this.SetValue(MetroWindow.IsWindowDraggableProperty, (object) value);
    }

    public WindowCommandsOverlayBehavior LeftWindowCommandsOverlayBehavior
    {
      get
      {
        return (WindowCommandsOverlayBehavior) this.GetValue(MetroWindow.LeftWindowCommandsOverlayBehaviorProperty);
      }
      set => this.SetValue(MetroWindow.LeftWindowCommandsOverlayBehaviorProperty, (object) value);
    }

    public WindowCommandsOverlayBehavior RightWindowCommandsOverlayBehavior
    {
      get
      {
        return (WindowCommandsOverlayBehavior) this.GetValue(MetroWindow.RightWindowCommandsOverlayBehaviorProperty);
      }
      set => this.SetValue(MetroWindow.RightWindowCommandsOverlayBehaviorProperty, (object) value);
    }

    public WindowCommandsOverlayBehavior WindowButtonCommandsOverlayBehavior
    {
      get
      {
        return (WindowCommandsOverlayBehavior) this.GetValue(MetroWindow.WindowButtonCommandsOverlayBehaviorProperty);
      }
      set => this.SetValue(MetroWindow.WindowButtonCommandsOverlayBehaviorProperty, (object) value);
    }

    public WindowCommandsOverlayBehavior IconOverlayBehavior
    {
      get => (WindowCommandsOverlayBehavior) this.GetValue(MetroWindow.IconOverlayBehaviorProperty);
      set => this.SetValue(MetroWindow.IconOverlayBehaviorProperty, (object) value);
    }

    [Obsolete("This property will be deleted in the next release. You should use LightMinButtonStyle or DarkMinButtonStyle in WindowButtonCommands to override the style.")]
    public Style WindowMinButtonStyle
    {
      get => (Style) this.GetValue(MetroWindow.WindowMinButtonStyleProperty);
      set => this.SetValue(MetroWindow.WindowMinButtonStyleProperty, (object) value);
    }

    [Obsolete("This property will be deleted in the next release. You should use LightMaxButtonStyle or DarkMaxButtonStyle in WindowButtonCommands to override the style.")]
    public Style WindowMaxButtonStyle
    {
      get => (Style) this.GetValue(MetroWindow.WindowMaxButtonStyleProperty);
      set => this.SetValue(MetroWindow.WindowMaxButtonStyleProperty, (object) value);
    }

    [Obsolete("This property will be deleted in the next release. You should use LightCloseButtonStyle or DarkCloseButtonStyle in WindowButtonCommands to override the style.")]
    public Style WindowCloseButtonStyle
    {
      get => (Style) this.GetValue(MetroWindow.WindowCloseButtonStyleProperty);
      set => this.SetValue(MetroWindow.WindowCloseButtonStyleProperty, (object) value);
    }

    public static void OnWindowButtonStyleChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      MetroWindow metroWindow = (MetroWindow) d;
      if (metroWindow.WindowButtonCommands == null)
        return;
      metroWindow.WindowButtonCommands.ApplyTheme();
    }

    public bool WindowTransitionsEnabled
    {
      get => (bool) this.GetValue(MetroWindow.WindowTransitionsEnabledProperty);
      set => this.SetValue(MetroWindow.WindowTransitionsEnabledProperty, (object) value);
    }

    public FlyoutsControl Flyouts
    {
      get => (FlyoutsControl) this.GetValue(MetroWindow.FlyoutsProperty);
      set => this.SetValue(MetroWindow.FlyoutsProperty, (object) value);
    }

    public DataTemplate IconTemplate
    {
      get => (DataTemplate) this.GetValue(MetroWindow.IconTemplateProperty);
      set => this.SetValue(MetroWindow.IconTemplateProperty, (object) value);
    }

    public DataTemplate TitleTemplate
    {
      get => (DataTemplate) this.GetValue(MetroWindow.TitleTemplateProperty);
      set => this.SetValue(MetroWindow.TitleTemplateProperty, (object) value);
    }

    public WindowCommands LeftWindowCommands
    {
      get => (WindowCommands) this.GetValue(MetroWindow.LeftWindowCommandsProperty);
      set => this.SetValue(MetroWindow.LeftWindowCommandsProperty, (object) value);
    }

    public WindowCommands RightWindowCommands
    {
      get => (WindowCommands) this.GetValue(MetroWindow.RightWindowCommandsProperty);
      set => this.SetValue(MetroWindow.RightWindowCommandsProperty, (object) value);
    }

    private static void WindowCommandsPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue || e.NewValue == null)
        return;
      ((MetroWindow) dependencyObject).RightWindowCommands = (WindowCommands) e.NewValue;
    }

    public bool IgnoreTaskbarOnMaximize
    {
      get => (bool) this.GetValue(MetroWindow.IgnoreTaskbarOnMaximizeProperty);
      set => this.SetValue(MetroWindow.IgnoreTaskbarOnMaximizeProperty, (object) value);
    }

    public Brush TitleForeground
    {
      get => (Brush) this.GetValue(MetroWindow.TitleForegroundProperty);
      set => this.SetValue(MetroWindow.TitleForegroundProperty, (object) value);
    }

    public bool SaveWindowPosition
    {
      get => (bool) this.GetValue(MetroWindow.SaveWindowPositionProperty);
      set => this.SetValue(MetroWindow.SaveWindowPositionProperty, (object) value);
    }

    public IWindowPlacementSettings WindowPlacementSettings
    {
      get => (IWindowPlacementSettings) this.GetValue(MetroWindow.WindowPlacementSettingsProperty);
      set => this.SetValue(MetroWindow.WindowPlacementSettingsProperty, (object) value);
    }

    public virtual IWindowPlacementSettings GetWindowPlacementSettings()
    {
      return this.WindowPlacementSettings ?? (IWindowPlacementSettings) new WindowApplicationSettings((Window) this);
    }

    public bool ShowIconOnTitleBar
    {
      get => (bool) this.GetValue(MetroWindow.ShowIconOnTitleBarProperty);
      set => this.SetValue(MetroWindow.ShowIconOnTitleBarProperty, (object) value);
    }

    private static void OnShowIconOnTitleBarPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      MetroWindow metroWindow = (MetroWindow) d;
      if (e.NewValue == e.OldValue)
        return;
      metroWindow.SetVisibiltyForIcon();
    }

    public EdgeMode IconEdgeMode
    {
      get => (EdgeMode) this.GetValue(MetroWindow.IconEdgeModeProperty);
      set => this.SetValue(MetroWindow.IconEdgeModeProperty, (object) value);
    }

    public BitmapScalingMode IconBitmapScalingMode
    {
      get => (BitmapScalingMode) this.GetValue(MetroWindow.IconBitmapScalingModeProperty);
      set => this.SetValue(MetroWindow.IconBitmapScalingModeProperty, (object) value);
    }

    public bool ShowTitleBar
    {
      get => (bool) this.GetValue(MetroWindow.ShowTitleBarProperty);
      set => this.SetValue(MetroWindow.ShowTitleBarProperty, (object) value);
    }

    private static void OnShowTitleBarPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      MetroWindow metroWindow = (MetroWindow) d;
      if (e.NewValue == e.OldValue)
        return;
      metroWindow.SetVisibiltyForAllTitleElements((bool) e.NewValue);
    }

    private static object OnShowTitleBarCoerceValueCallback(DependencyObject d, object value)
    {
      return ((MetroWindow) d).UseNoneWindowStyle ? (object) false : value;
    }

    public bool UseNoneWindowStyle
    {
      get => (bool) this.GetValue(MetroWindow.UseNoneWindowStyleProperty);
      set => this.SetValue(MetroWindow.UseNoneWindowStyleProperty, (object) value);
    }

    private static void OnUseNoneWindowStylePropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      bool newValue = (bool) e.NewValue;
      ((MetroWindow) d).ToggleNoneWindowStyle(newValue);
    }

    private void ToggleNoneWindowStyle(bool useNoneWindowStyle)
    {
      if (useNoneWindowStyle)
        this.ShowTitleBar = false;
      if (this.LeftWindowCommandsPresenter != null)
        this.LeftWindowCommandsPresenter.Visibility = useNoneWindowStyle ? Visibility.Collapsed : Visibility.Visible;
      if (this.RightWindowCommandsPresenter == null)
        return;
      this.RightWindowCommandsPresenter.Visibility = useNoneWindowStyle ? Visibility.Collapsed : Visibility.Visible;
    }

    public bool ShowMinButton
    {
      get => (bool) this.GetValue(MetroWindow.ShowMinButtonProperty);
      set => this.SetValue(MetroWindow.ShowMinButtonProperty, (object) value);
    }

    public bool ShowMaxRestoreButton
    {
      get => (bool) this.GetValue(MetroWindow.ShowMaxRestoreButtonProperty);
      set => this.SetValue(MetroWindow.ShowMaxRestoreButtonProperty, (object) value);
    }

    public bool ShowCloseButton
    {
      get => (bool) this.GetValue(MetroWindow.ShowCloseButtonProperty);
      set => this.SetValue(MetroWindow.ShowCloseButtonProperty, (object) value);
    }

    public bool IsMinButtonEnabled
    {
      get => (bool) this.GetValue(MetroWindow.IsMinButtonEnabledProperty);
      set => this.SetValue(MetroWindow.IsMinButtonEnabledProperty, (object) value);
    }

    public bool IsMaxRestoreButtonEnabled
    {
      get => (bool) this.GetValue(MetroWindow.IsMaxRestoreButtonEnabledProperty);
      set => this.SetValue(MetroWindow.IsMaxRestoreButtonEnabledProperty, (object) value);
    }

    public bool IsCloseButtonEnabled
    {
      get => (bool) this.GetValue(MetroWindow.IsCloseButtonEnabledProperty);
      set => this.SetValue(MetroWindow.IsCloseButtonEnabledProperty, (object) value);
    }

    public bool ShowSystemMenuOnRightClick
    {
      get => (bool) this.GetValue(MetroWindow.ShowSystemMenuOnRightClickProperty);
      set => this.SetValue(MetroWindow.ShowSystemMenuOnRightClickProperty, (object) value);
    }

    public int TitlebarHeight
    {
      get => (int) this.GetValue(MetroWindow.TitlebarHeightProperty);
      set => this.SetValue(MetroWindow.TitlebarHeightProperty, (object) value);
    }

    private static void TitlebarHeightPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      MetroWindow metroWindow = (MetroWindow) dependencyObject;
      if (e.NewValue == e.OldValue)
        return;
      metroWindow.SetVisibiltyForAllTitleElements((int) e.NewValue > 0);
    }

    private void SetVisibiltyForIcon()
    {
      if (this.icon == null)
        return;
      this.icon.Visibility = this.IconOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.HiddenTitleBar) && !this.ShowTitleBar || this.ShowIconOnTitleBar && this.ShowTitleBar ? Visibility.Visible : Visibility.Collapsed;
    }

    private void SetVisibiltyForAllTitleElements(bool visible)
    {
      this.SetVisibiltyForIcon();
      Visibility visibility = !visible || !this.ShowTitleBar ? Visibility.Collapsed : Visibility.Visible;
      if (this.titleBar != null)
        this.titleBar.Visibility = visibility;
      if (this.titleBarBackground != null)
        this.titleBarBackground.Visibility = visibility;
      if (this.LeftWindowCommandsPresenter != null)
        this.LeftWindowCommandsPresenter.Visibility = this.LeftWindowCommandsOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.HiddenTitleBar) ? Visibility.Visible : visibility;
      if (this.RightWindowCommandsPresenter != null)
        this.RightWindowCommandsPresenter.Visibility = this.RightWindowCommandsOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.HiddenTitleBar) ? Visibility.Visible : visibility;
      if (this.WindowButtonCommands != null)
        this.WindowButtonCommands.Visibility = this.WindowButtonCommandsOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.HiddenTitleBar) ? Visibility.Visible : visibility;
      this.SetWindowEvents();
    }

    public bool TitleCaps
    {
      get => (bool) this.GetValue(MetroWindow.TitleCapsProperty);
      set => this.SetValue(MetroWindow.TitleCapsProperty, (object) value);
    }

    public HorizontalAlignment TitleAlignment
    {
      get => (HorizontalAlignment) this.GetValue(MetroWindow.TitleAlignmentProperty);
      set => this.SetValue(MetroWindow.TitleAlignmentProperty, (object) value);
    }

    public Brush WindowTitleBrush
    {
      get => (Brush) this.GetValue(MetroWindow.WindowTitleBrushProperty);
      set => this.SetValue(MetroWindow.WindowTitleBrushProperty, (object) value);
    }

    public Brush GlowBrush
    {
      get => (Brush) this.GetValue(MetroWindow.GlowBrushProperty);
      set => this.SetValue(MetroWindow.GlowBrushProperty, (object) value);
    }

    public Brush NonActiveGlowBrush
    {
      get => (Brush) this.GetValue(MetroWindow.NonActiveGlowBrushProperty);
      set => this.SetValue(MetroWindow.NonActiveGlowBrushProperty, (object) value);
    }

    public Brush NonActiveBorderBrush
    {
      get => (Brush) this.GetValue(MetroWindow.NonActiveBorderBrushProperty);
      set => this.SetValue(MetroWindow.NonActiveBorderBrushProperty, (object) value);
    }

    public Brush NonActiveWindowTitleBrush
    {
      get => (Brush) this.GetValue(MetroWindow.NonActiveWindowTitleBrushProperty);
      set => this.SetValue(MetroWindow.NonActiveWindowTitleBrushProperty, (object) value);
    }

    public string WindowTitle => !this.TitleCaps ? this.Title : this.Title.ToUpper();

    public Task ShowOverlayAsync()
    {
      if (this.overlayBox == null)
        throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      if (this.IsOverlayVisible() && this.overlayStoryboard == null)
      {
        tcs.SetResult((object) null);
        return (Task) tcs.Task;
      }
      this.Dispatcher.VerifyAccess();
      this.overlayBox.Visibility = Visibility.Visible;
      Storyboard sb = (Storyboard) this.Template.Resources[(object) "OverlayFastSemiFadeIn"];
      sb = sb.Clone();
      EventHandler completionHandler = (EventHandler) null;
      completionHandler = (EventHandler) ((sender, args) =>
      {
        sb.Completed -= completionHandler;
        if (this.overlayStoryboard == sb)
          this.overlayStoryboard = (Storyboard) null;
        tcs.TrySetResult((object) null);
      });
      sb.Completed += completionHandler;
      this.overlayBox.BeginStoryboard(sb);
      this.overlayStoryboard = sb;
      return (Task) tcs.Task;
    }

    public Task HideOverlayAsync()
    {
      if (this.overlayBox == null)
        throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      if (this.overlayBox.Visibility == Visibility.Visible && this.overlayBox.Opacity == 0.0)
      {
        tcs.SetResult((object) null);
        return (Task) tcs.Task;
      }
      this.Dispatcher.VerifyAccess();
      Storyboard sb = (Storyboard) this.Template.Resources[(object) "OverlayFastSemiFadeOut"];
      sb = sb.Clone();
      EventHandler completionHandler = (EventHandler) null;
      completionHandler = (EventHandler) ((sender, args) =>
      {
        sb.Completed -= completionHandler;
        if (this.overlayStoryboard == sb)
        {
          this.overlayBox.Visibility = Visibility.Hidden;
          this.overlayStoryboard = (Storyboard) null;
        }
        tcs.TrySetResult((object) null);
      });
      sb.Completed += completionHandler;
      this.overlayBox.BeginStoryboard(sb);
      this.overlayStoryboard = sb;
      return (Task) tcs.Task;
    }

    public bool IsOverlayVisible()
    {
      if (this.overlayBox == null)
        throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
      return this.overlayBox.Visibility == Visibility.Visible && this.overlayBox.Opacity >= 0.7;
    }

    public void ShowOverlay()
    {
      this.overlayBox.Visibility = Visibility.Visible;
      this.overlayBox.SetCurrentValue(UIElement.OpacityProperty, (object) 0.7);
    }

    public void HideOverlay()
    {
      this.overlayBox.SetCurrentValue(UIElement.OpacityProperty, (object) 0.0);
      this.overlayBox.Visibility = Visibility.Hidden;
    }

    public MetroWindow() => this.Loaded += new RoutedEventHandler(this.MetroWindow_Loaded);

    private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.EnableDWMDropShadow)
        this.UseDropShadow();
      if (this.WindowTransitionsEnabled)
        VisualStateManager.GoToState((FrameworkElement) this, "AfterLoaded", true);
      this.ToggleNoneWindowStyle(this.UseNoneWindowStyle);
      if (this.Flyouts == null)
        this.Flyouts = new FlyoutsControl();
      this.ResetAllWindowCommandsBrush();
      ThemeManager.IsThemeChanged += new EventHandler<OnThemeChangedEventArgs>(this.ThemeManagerOnIsThemeChanged);
      this.Unloaded += (RoutedEventHandler) ((o, args) => ThemeManager.IsThemeChanged -= new EventHandler<OnThemeChangedEventArgs>(this.ThemeManagerOnIsThemeChanged));
    }

    private void MetroWindow_SizeChanged(object sender, RoutedEventArgs e)
    {
      Grid titleBar = (Grid) this.titleBar;
      ContentControl content = (ContentControl) ((ContentControl) titleBar.Children[0]).Content;
      ContentControl icon = (ContentControl) this.icon;
      double num1 = this.Width / 2.0;
      double num2 = content.ActualWidth / 2.0;
      double num3 = icon.ActualWidth + this.LeftWindowCommands.ActualWidth;
      double num4 = this.WindowButtonCommands.ActualWidth + this.RightWindowCommands.ActualWidth;
      if (num3 + num2 + 5.0 < num1 && num4 + num2 + 5.0 < num1)
      {
        Grid.SetColumn((UIElement) titleBar, 0);
        Grid.SetColumnSpan((UIElement) titleBar, 5);
      }
      else
      {
        Grid.SetColumn((UIElement) titleBar, 2);
        Grid.SetColumnSpan((UIElement) titleBar, 1);
      }
    }

    private void ThemeManagerOnIsThemeChanged(object sender, OnThemeChangedEventArgs e)
    {
      if (e.Accent == null)
        return;
      List<Flyout> list1 = this.Flyouts.GetFlyouts().ToList<Flyout>();
      List<FlyoutsControl> list2 = (this.Content as DependencyObject).FindChildren<FlyoutsControl>(true).ToList<FlyoutsControl>();
      if (list2.Any<FlyoutsControl>())
        list1.AddRange(list2.SelectMany<FlyoutsControl, Flyout>((Func<FlyoutsControl, IEnumerable<Flyout>>) (flyoutsControl => flyoutsControl.GetFlyouts())));
      if (!list1.Any<Flyout>())
      {
        this.ResetAllWindowCommandsBrush();
      }
      else
      {
        foreach (Flyout flyout in list1)
          flyout.ChangeFlyoutTheme(e.Accent, e.AppTheme);
        this.HandleWindowCommandsForFlyouts((IEnumerable<Flyout>) list1);
      }
    }

    private void FlyoutsPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.OriginalSource is DependencyObject originalSource && (originalSource.TryFindParent<Flyout>() != null || object.Equals((object) originalSource.TryFindParent<ContentControl>(), (object) this.icon) || originalSource.TryFindParent<WindowCommands>() != null || originalSource.TryFindParent<WindowButtonCommands>() != null))
        return;
      MouseButton? externalCloseButton = this.Flyouts.OverrideExternalCloseButton;
      if (!externalCloseButton.HasValue)
      {
        foreach (Flyout flyout in this.Flyouts.GetFlyouts().Where<Flyout>((Func<Flyout, bool>) (x =>
        {
          if (!x.IsOpen || x.ExternalCloseButton != e.ChangedButton)
            return false;
          return !x.IsPinned || this.Flyouts.OverrideIsPinned;
        })))
        {
          flyout.IsOpen = false;
          e.Handled = true;
        }
      }
      else
      {
        externalCloseButton = this.Flyouts.OverrideExternalCloseButton;
        MouseButton changedButton = e.ChangedButton;
        if ((externalCloseButton.GetValueOrDefault() == changedButton ? (externalCloseButton.HasValue ? 1 : 0) : 0) == 0)
          return;
        foreach (Flyout flyout in this.Flyouts.GetFlyouts().Where<Flyout>((Func<Flyout, bool>) (x =>
        {
          if (!x.IsOpen)
            return false;
          return !x.IsPinned || this.Flyouts.OverrideIsPinned;
        })))
        {
          flyout.IsOpen = false;
          e.Handled = true;
        }
      }
    }

    static MetroWindow()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (MetroWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (MetroWindow)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (this.LeftWindowCommands == null)
        this.LeftWindowCommands = new WindowCommands();
      if (this.RightWindowCommands == null)
        this.RightWindowCommands = new WindowCommands();
      this.LeftWindowCommands.ParentWindow = (Window) this;
      this.RightWindowCommands.ParentWindow = (Window) this;
      this.LeftWindowCommandsPresenter = this.GetTemplateChild("PART_LeftWindowCommands") as ContentPresenter;
      this.RightWindowCommandsPresenter = this.GetTemplateChild("PART_RightWindowCommands") as ContentPresenter;
      this.WindowButtonCommands = this.GetTemplateChild("PART_WindowButtonCommands") as WindowButtonCommands;
      if (this.WindowButtonCommands != null)
        this.WindowButtonCommands.ParentWindow = this;
      this.overlayBox = this.GetTemplateChild("PART_OverlayBox") as Grid;
      this.metroActiveDialogContainer = this.GetTemplateChild("PART_MetroActiveDialogContainer") as Grid;
      this.metroInactiveDialogContainer = this.GetTemplateChild("PART_MetroInactiveDialogsContainer") as Grid;
      this.flyoutModal = (Rectangle) this.GetTemplateChild("PART_FlyoutModal");
      this.flyoutModal.PreviewMouseDown += new MouseButtonEventHandler(this.FlyoutsPreviewMouseDown);
      this.PreviewMouseDown += new MouseButtonEventHandler(this.FlyoutsPreviewMouseDown);
      this.icon = this.GetTemplateChild("PART_Icon") as UIElement;
      this.titleBar = this.GetTemplateChild("PART_TitleBar") as UIElement;
      this.titleBarBackground = this.GetTemplateChild("PART_WindowTitleBackground") as UIElement;
      this.windowTitleThumb = this.GetTemplateChild("PART_WindowTitleThumb") as Thumb;
      this.SetVisibiltyForAllTitleElements(this.TitlebarHeight > 0);
    }

    protected new IntPtr CriticalHandle
    {
      get
      {
        return (IntPtr) typeof (Window).GetProperty(nameof (CriticalHandle), BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) this, new object[0]);
      }
    }

    private void ClearWindowEvents()
    {
      if (this.windowTitleThumb != null)
      {
        this.windowTitleThumb.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
        this.windowTitleThumb.DragDelta -= new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
        this.windowTitleThumb.MouseDoubleClick -= new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
        this.windowTitleThumb.MouseRightButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
      }
      if (this.icon != null)
        this.icon.MouseDown -= new MouseButtonEventHandler(this.IconMouseDown);
      this.SizeChanged -= new SizeChangedEventHandler(this.MetroWindow_SizeChanged);
    }

    private void SetWindowEvents()
    {
      this.ClearWindowEvents();
      if (this.icon != null && this.icon.Visibility == Visibility.Visible)
        this.icon.MouseDown += new MouseButtonEventHandler(this.IconMouseDown);
      if (this.windowTitleThumb != null)
      {
        this.windowTitleThumb.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
        this.windowTitleThumb.DragDelta += new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
        this.windowTitleThumb.MouseDoubleClick += new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
        this.windowTitleThumb.MouseRightButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
      }
      if (this.titleBar == null || !(this.titleBar.GetType() == typeof (Grid)))
        return;
      this.SizeChanged += new SizeChangedEventHandler(this.MetroWindow_SizeChanged);
    }

    private void IconMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      if (e.ClickCount == 2)
        this.Close();
      else
        MetroWindow.ShowSystemMenuPhysicalCoordinates((Window) this, this.PointToScreen(new Point(0.0, (double) this.TitlebarHeight)));
    }

    private void WindowTitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      MetroWindow.DoWindowTitleThumbOnPreviewMouseLeftButtonUp(this, e);
    }

    private void WindowTitleThumbMoveOnDragDelta(
      object sender,
      DragDeltaEventArgs dragDeltaEventArgs)
    {
      MetroWindow.DoWindowTitleThumbMoveOnDragDelta(this, dragDeltaEventArgs);
    }

    private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick(
      object sender,
      MouseButtonEventArgs mouseButtonEventArgs)
    {
      MetroWindow.DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(this, mouseButtonEventArgs);
    }

    private void WindowTitleThumbSystemMenuOnMouseRightButtonUp(
      object sender,
      MouseButtonEventArgs e)
    {
      MetroWindow.DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(this, e);
    }

    internal static void DoWindowTitleThumbOnPreviewMouseLeftButtonUp(
      MetroWindow window,
      MouseButtonEventArgs mouseButtonEventArgs)
    {
      Mouse.Capture((IInputElement) null);
    }

    internal static void DoWindowTitleThumbMoveOnDragDelta(
      MetroWindow window,
      DragDeltaEventArgs dragDeltaEventArgs)
    {
      if (!window.IsWindowDraggable || Math.Abs(dragDeltaEventArgs.HorizontalChange) <= 2.0 && Math.Abs(dragDeltaEventArgs.VerticalChange) <= 2.0)
        return;
      window.VerifyAccess();
      Standard.POINT cursorPos = NativeMethods.GetCursorPos();
      bool flag = window.WindowState == WindowState.Maximized;
      if ((Mouse.GetPosition((IInputElement) window.windowTitleThumb).Y > (double) window.TitlebarHeight || window.TitlebarHeight <= 0) & flag)
        return;
      UnsafeNativeMethods.ReleaseCapture();
      if (flag)
      {
        window.Top = 2.0;
        window.Left = Math.Max((double) cursorPos.x - window.RestoreBounds.Width / 2.0, 0.0);
        EventHandler windowOnStateChanged = (EventHandler) null;
        windowOnStateChanged = (EventHandler) ((sender, args) =>
        {
          window.StateChanged -= windowOnStateChanged;
          if (window.WindowState != WindowState.Normal)
            return;
          Mouse.Capture((IInputElement) window.windowTitleThumb, CaptureMode.Element);
        });
        window.StateChanged += windowOnStateChanged;
      }
      IntPtr criticalHandle = window.CriticalHandle;
      NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr) 61458, IntPtr.Zero);
      NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
    }

    internal static void DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(
      MetroWindow window,
      MouseButtonEventArgs mouseButtonEventArgs)
    {
      if (mouseButtonEventArgs.ChangedButton != MouseButton.Left || !((window.ResizeMode == ResizeMode.CanResizeWithGrip || window.ResizeMode == ResizeMode.CanResize) & (Mouse.GetPosition((IInputElement) window).Y <= (double) window.TitlebarHeight && window.TitlebarHeight > 0)))
        return;
      if (window.WindowState == WindowState.Maximized)
        Microsoft.Windows.Shell.SystemCommands.RestoreWindow((Window) window);
      else
        Microsoft.Windows.Shell.SystemCommands.MaximizeWindow((Window) window);
      mouseButtonEventArgs.Handled = true;
    }

    internal static void DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(
      MetroWindow window,
      MouseButtonEventArgs e)
    {
      if (!window.ShowSystemMenuOnRightClick)
        return;
      Point position = e.GetPosition((IInputElement) window);
      if ((position.Y > (double) window.TitlebarHeight || window.TitlebarHeight <= 0) && (!window.UseNoneWindowStyle || window.TitlebarHeight > 0))
        return;
      MetroWindow.ShowSystemMenuPhysicalCoordinates((Window) window, window.PointToScreen(position));
    }

    internal T GetPart<T>(string name) where T : DependencyObject
    {
      return this.GetTemplateChild(name) as T;
    }

    internal DependencyObject GetPart(string name) => this.GetTemplateChild(name);

    private static void ShowSystemMenuPhysicalCoordinates(
      Window window,
      Point physicalScreenLocation)
    {
      if (window == null)
        return;
      IntPtr handle = new WindowInteropHelper(window).Handle;
      if (handle == IntPtr.Zero || !UnsafeNativeMethods.IsWindow(handle))
        return;
      uint num = UnsafeNativeMethods.TrackPopupMenuEx(UnsafeNativeMethods.GetSystemMenu(handle, false), 256U, (int) physicalScreenLocation.X, (int) physicalScreenLocation.Y, handle, IntPtr.Zero);
      if (num == 0U)
        return;
      UnsafeNativeMethods.PostMessage(handle, 274U, new IntPtr((long) num), IntPtr.Zero);
    }

    internal void HandleFlyoutStatusChange(Flyout flyout, IEnumerable<Flyout> visibleFlyouts)
    {
      if (flyout.Position == Position.Left || flyout.Position == Position.Right || flyout.Position == Position.Top)
      {
        int num = flyout.IsOpen ? Panel.GetZIndex((UIElement) flyout) + 3 : visibleFlyouts.Count<Flyout>() + 2;
        if (this.icon != null)
          this.icon.SetValue(Panel.ZIndexProperty, (object) (this.IconOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
        if (this.LeftWindowCommandsPresenter != null)
          this.LeftWindowCommandsPresenter.SetValue(Panel.ZIndexProperty, (object) (this.LeftWindowCommandsOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
        if (this.RightWindowCommandsPresenter != null)
          this.RightWindowCommandsPresenter.SetValue(Panel.ZIndexProperty, (object) (this.RightWindowCommandsOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
        if (this.WindowButtonCommands != null)
          this.WindowButtonCommands.SetValue(Panel.ZIndexProperty, (object) (this.WindowButtonCommandsOverlayBehavior.HasFlag((Enum) WindowCommandsOverlayBehavior.Flyouts) ? num : 1));
        this.HandleWindowCommandsForFlyouts(visibleFlyouts);
      }
      this.flyoutModal.Visibility = visibleFlyouts.Any<Flyout>((Func<Flyout, bool>) (x => x.IsModal)) ? Visibility.Visible : Visibility.Hidden;
      this.RaiseEvent((RoutedEventArgs) new MetroWindow.FlyoutStatusChangedRoutedEventArgs(MetroWindow.FlyoutsStatusChangedEvent, (object) this)
      {
        ChangedFlyout = flyout
      });
    }

    public class FlyoutStatusChangedRoutedEventArgs : RoutedEventArgs
    {
      internal FlyoutStatusChangedRoutedEventArgs(RoutedEvent rEvent, object source)
        : base(rEvent, source)
      {
      }

      public Flyout ChangedFlyout { get; internal set; }
    }
  }
}
