// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Flyout
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_BackButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_WindowTitleThumb", Type = typeof (Thumb))]
  [TemplatePart(Name = "PART_Header", Type = typeof (ContentPresenter))]
  [TemplatePart(Name = "PART_Content", Type = typeof (ContentPresenter))]
  public class Flyout : ContentControl
  {
    public static readonly RoutedEvent IsOpenChangedEvent = EventManager.RegisterRoutedEvent("IsOpenChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Flyout));
    public static readonly RoutedEvent ClosingFinishedEvent = EventManager.RegisterRoutedEvent("ClosingFinished", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Flyout));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (string), typeof (Flyout), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof (Position), typeof (Position), typeof (Flyout), new PropertyMetadata((object) Position.Left, new PropertyChangedCallback(Flyout.PositionChanged)));
    public static readonly DependencyProperty IsPinnedProperty = DependencyProperty.Register(nameof (IsPinned), typeof (bool), typeof (Flyout), new PropertyMetadata((object) true));
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof (IsOpen), typeof (bool), typeof (Flyout), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Flyout.IsOpenedChanged)));
    public static readonly DependencyProperty AnimateOnPositionChangeProperty = DependencyProperty.Register(nameof (AnimateOnPositionChange), typeof (bool), typeof (Flyout), new PropertyMetadata((object) true));
    public static readonly DependencyProperty AnimateOpacityProperty = DependencyProperty.Register(nameof (AnimateOpacity), typeof (bool), typeof (Flyout), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(Flyout.AnimateOpacityChanged)));
    public static readonly DependencyProperty IsModalProperty = DependencyProperty.Register(nameof (IsModal), typeof (bool), typeof (Flyout));
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (Flyout));
    public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.RegisterAttached(nameof (CloseCommand), typeof (ICommand), typeof (Flyout), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof (Theme), typeof (FlyoutTheme), typeof (Flyout), (PropertyMetadata) new FrameworkPropertyMetadata((object) FlyoutTheme.Dark, new PropertyChangedCallback(Flyout.ThemeChanged)));
    public static readonly DependencyProperty ExternalCloseButtonProperty = DependencyProperty.Register(nameof (ExternalCloseButton), typeof (MouseButton), typeof (Flyout), new PropertyMetadata((object) MouseButton.Left));
    public static readonly DependencyProperty CloseButtonVisibilityProperty = DependencyProperty.Register(nameof (CloseButtonVisibility), typeof (Visibility), typeof (Flyout), (PropertyMetadata) new FrameworkPropertyMetadata((object) Visibility.Visible));
    public static readonly DependencyProperty CloseButtonIsCancelProperty = DependencyProperty.Register(nameof (CloseButtonIsCancel), typeof (bool), typeof (Flyout), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    public static readonly DependencyProperty TitleVisibilityProperty = DependencyProperty.Register(nameof (TitleVisibility), typeof (Visibility), typeof (Flyout), (PropertyMetadata) new FrameworkPropertyMetadata((object) Visibility.Visible));
    public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register(nameof (AreAnimationsEnabled), typeof (bool), typeof (Flyout), new PropertyMetadata((object) true));
    public static readonly DependencyProperty FocusedElementProperty = DependencyProperty.Register(nameof (FocusedElement), typeof (FrameworkElement), typeof (Flyout), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty AllowFocusElementProperty = DependencyProperty.Register(nameof (AllowFocusElement), typeof (bool), typeof (Flyout), new PropertyMetadata((object) true));
    private MetroWindow parentWindow;
    private Grid root;
    private Storyboard hideStoryboard;
    private SplineDoubleKeyFrame hideFrame;
    private SplineDoubleKeyFrame hideFrameY;
    private SplineDoubleKeyFrame showFrame;
    private SplineDoubleKeyFrame showFrameY;
    private SplineDoubleKeyFrame fadeOutFrame;
    private ContentPresenter PART_Header;
    private ContentPresenter PART_Content;
    private Thumb windowTitleThumb;

    public event RoutedEventHandler IsOpenChanged
    {
      add => this.AddHandler(Flyout.IsOpenChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(Flyout.IsOpenChangedEvent, (Delegate) value);
    }

    public event RoutedEventHandler ClosingFinished
    {
      add => this.AddHandler(Flyout.ClosingFinishedEvent, (Delegate) value);
      remove => this.RemoveHandler(Flyout.ClosingFinishedEvent, (Delegate) value);
    }

    internal PropertyChangeNotifier IsOpenPropertyChangeNotifier { get; set; }

    internal PropertyChangeNotifier ThemePropertyChangeNotifier { get; set; }

    public bool AreAnimationsEnabled
    {
      get => (bool) this.GetValue(Flyout.AreAnimationsEnabledProperty);
      set => this.SetValue(Flyout.AreAnimationsEnabledProperty, (object) value);
    }

    public Visibility TitleVisibility
    {
      get => (Visibility) this.GetValue(Flyout.TitleVisibilityProperty);
      set => this.SetValue(Flyout.TitleVisibilityProperty, (object) value);
    }

    public Visibility CloseButtonVisibility
    {
      get => (Visibility) this.GetValue(Flyout.CloseButtonVisibilityProperty);
      set => this.SetValue(Flyout.CloseButtonVisibilityProperty, (object) value);
    }

    public bool CloseButtonIsCancel
    {
      get => (bool) this.GetValue(Flyout.CloseButtonIsCancelProperty);
      set => this.SetValue(Flyout.CloseButtonIsCancelProperty, (object) value);
    }

    public ICommand CloseCommand
    {
      get => (ICommand) this.GetValue(Flyout.CloseCommandProperty);
      set => this.SetValue(Flyout.CloseCommandProperty, (object) value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) this.GetValue(Flyout.HeaderTemplateProperty);
      set => this.SetValue(Flyout.HeaderTemplateProperty, (object) value);
    }

    public bool IsOpen
    {
      get => (bool) this.GetValue(Flyout.IsOpenProperty);
      set => this.SetValue(Flyout.IsOpenProperty, (object) value);
    }

    public bool AnimateOnPositionChange
    {
      get => (bool) this.GetValue(Flyout.AnimateOnPositionChangeProperty);
      set => this.SetValue(Flyout.AnimateOnPositionChangeProperty, (object) value);
    }

    public bool AnimateOpacity
    {
      get => (bool) this.GetValue(Flyout.AnimateOpacityProperty);
      set => this.SetValue(Flyout.AnimateOpacityProperty, (object) value);
    }

    public bool IsPinned
    {
      get => (bool) this.GetValue(Flyout.IsPinnedProperty);
      set => this.SetValue(Flyout.IsPinnedProperty, (object) value);
    }

    public MouseButton ExternalCloseButton
    {
      get => (MouseButton) this.GetValue(Flyout.ExternalCloseButtonProperty);
      set => this.SetValue(Flyout.ExternalCloseButtonProperty, (object) value);
    }

    public bool IsModal
    {
      get => (bool) this.GetValue(Flyout.IsModalProperty);
      set => this.SetValue(Flyout.IsModalProperty, (object) value);
    }

    public Position Position
    {
      get => (Position) this.GetValue(Flyout.PositionProperty);
      set => this.SetValue(Flyout.PositionProperty, (object) value);
    }

    public string Header
    {
      get => (string) this.GetValue(Flyout.HeaderProperty);
      set => this.SetValue(Flyout.HeaderProperty, (object) value);
    }

    public FlyoutTheme Theme
    {
      get => (FlyoutTheme) this.GetValue(Flyout.ThemeProperty);
      set => this.SetValue(Flyout.ThemeProperty, (object) value);
    }

    public FrameworkElement FocusedElement
    {
      get => (FrameworkElement) this.GetValue(Flyout.FocusedElementProperty);
      set => this.SetValue(Flyout.FocusedElementProperty, (object) value);
    }

    public bool AllowFocusElement
    {
      get => (bool) this.GetValue(Flyout.AllowFocusElementProperty);
      set => this.SetValue(Flyout.AllowFocusElementProperty, (object) value);
    }

    public Flyout()
    {
      this.Loaded += (RoutedEventHandler) ((sender, args) => this.UpdateFlyoutTheme());
    }

    private MetroWindow ParentWindow
    {
      get => this.parentWindow ?? (this.parentWindow = this.TryFindParent<MetroWindow>());
    }

    private void UpdateFlyoutTheme()
    {
      FlyoutsControl parent = this.TryFindParent<FlyoutsControl>();
      if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
        this.Visibility = parent != null ? Visibility.Collapsed : Visibility.Visible;
      MetroWindow parentWindow = this.ParentWindow;
      if (parentWindow == null)
        return;
      Tuple<AppTheme, Accent> tuple = Flyout.DetectTheme(this);
      if (tuple != null && tuple.Item2 != null)
        this.ChangeFlyoutTheme(tuple.Item2, tuple.Item1);
      if (parent == null || !this.IsOpen)
        return;
      parent.HandleFlyoutStatusChange(this, parentWindow);
    }

    internal void ChangeFlyoutTheme(Accent windowAccent, AppTheme windowTheme)
    {
      switch (this.Theme)
      {
        case FlyoutTheme.Adapt:
          ThemeManager.ChangeAppStyle(this.Resources, windowAccent, windowTheme);
          break;
        case FlyoutTheme.Inverse:
          ThemeManager.ChangeAppStyle(this.Resources, windowAccent, ThemeManager.GetInverseAppTheme(windowTheme) ?? throw new InvalidOperationException("The inverse flyout theme only works if the window theme abides the naming convention. See ThemeManager.GetInverseAppTheme for more infos"));
          break;
        case FlyoutTheme.Dark:
          ThemeManager.ChangeAppStyle(this.Resources, windowAccent, ThemeManager.GetAppTheme("BaseDark"));
          break;
        case FlyoutTheme.Light:
          ThemeManager.ChangeAppStyle(this.Resources, windowAccent, ThemeManager.GetAppTheme("BaseLight"));
          break;
        case FlyoutTheme.Accent:
          ThemeManager.ChangeAppStyle(this.Resources, windowAccent, windowTheme);
          this.SetResourceReference(Control.BackgroundProperty, (object) "HighlightBrush");
          this.SetResourceReference(Control.ForegroundProperty, (object) "IdealForegroundColorBrush");
          break;
      }
    }

    private static Tuple<AppTheme, Accent> DetectTheme(Flyout flyout)
    {
      if (flyout == null)
        return (Tuple<AppTheme, Accent>) null;
      MetroWindow parentWindow = flyout.ParentWindow;
      Tuple<AppTheme, Accent> tuple1 = parentWindow != null ? ThemeManager.DetectAppStyle((Window) parentWindow) : (Tuple<AppTheme, Accent>) null;
      if (tuple1 != null && tuple1.Item2 != null)
        return tuple1;
      if (Application.Current != null)
      {
        Tuple<AppTheme, Accent> tuple2 = Application.Current.MainWindow is MetroWindow mainWindow ? ThemeManager.DetectAppStyle((Window) mainWindow) : (Tuple<AppTheme, Accent>) null;
        if (tuple2 != null && tuple2.Item2 != null)
          return tuple2;
        Tuple<AppTheme, Accent> tuple3 = ThemeManager.DetectAppStyle(Application.Current);
        if (tuple3 != null && tuple3.Item2 != null)
          return tuple3;
      }
      return (Tuple<AppTheme, Accent>) null;
    }

    private void UpdateOpacityChange()
    {
      if (this.root == null || this.fadeOutFrame == null || DesignerProperties.GetIsInDesignMode((DependencyObject) this))
        return;
      if (!this.AnimateOpacity)
      {
        this.fadeOutFrame.Value = 1.0;
        this.root.Opacity = 1.0;
      }
      else
      {
        this.fadeOutFrame.Value = 0.0;
        if (this.IsOpen)
          return;
        this.root.Opacity = 0.0;
      }
    }

    private static void IsOpenedChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      Flyout flyout = (Flyout) dependencyObject;
      Action method = (Action) (() =>
      {
        if (e.NewValue != e.OldValue)
        {
          if (flyout.AreAnimationsEnabled)
          {
            if ((bool) e.NewValue)
            {
              if (flyout.hideStoryboard != null)
                flyout.hideStoryboard.Completed -= new EventHandler(flyout.HideStoryboard_Completed);
              flyout.Visibility = Visibility.Visible;
              flyout.ApplyAnimation(flyout.Position, flyout.AnimateOpacity);
              flyout.TryFocusElement();
            }
            else
            {
              flyout.Focus();
              if (flyout.hideStoryboard != null)
                flyout.hideStoryboard.Completed += new EventHandler(flyout.HideStoryboard_Completed);
              else
                flyout.Hide();
            }
            VisualStateManager.GoToState((FrameworkElement) flyout, !(bool) e.NewValue ? "Hide" : "Show", true);
          }
          else
          {
            if ((bool) e.NewValue)
            {
              flyout.Visibility = Visibility.Visible;
              flyout.TryFocusElement();
            }
            else
            {
              flyout.Focus();
              flyout.Hide();
            }
            VisualStateManager.GoToState((FrameworkElement) flyout, !(bool) e.NewValue ? "HideDirect" : "ShowDirect", true);
          }
        }
        flyout.RaiseEvent(new RoutedEventArgs(Flyout.IsOpenChangedEvent));
      });
      flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) method);
    }

    private void HideStoryboard_Completed(object sender, EventArgs e)
    {
      this.hideStoryboard.Completed -= new EventHandler(this.HideStoryboard_Completed);
      this.Hide();
    }

    private void Hide()
    {
      this.Visibility = Visibility.Hidden;
      this.RaiseEvent(new RoutedEventArgs(Flyout.ClosingFinishedEvent));
    }

    private void TryFocusElement()
    {
      if (!this.AllowFocusElement)
        return;
      this.Focus();
      if (this.FocusedElement != null)
      {
        this.FocusedElement.Focus();
      }
      else
      {
        if (this.PART_Content != null && this.PART_Content.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)) || this.PART_Header == null)
          return;
        this.PART_Header.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
      }
    }

    private static void ThemeChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      ((Flyout) dependencyObject).UpdateFlyoutTheme();
    }

    private static void AnimateOpacityChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      ((Flyout) dependencyObject).UpdateOpacityChange();
    }

    private static void PositionChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      Flyout control = (Flyout) dependencyObject;
      bool isOpen = control.IsOpen;
      if (isOpen && control.AnimateOnPositionChange)
      {
        control.ApplyAnimation((Position) e.NewValue, control.AnimateOpacity);
        VisualStateManager.GoToState((FrameworkElement) control, "Hide", true);
      }
      else
        control.ApplyAnimation((Position) e.NewValue, control.AnimateOpacity, false);
      if (!isOpen || !control.AnimateOnPositionChange)
        return;
      control.ApplyAnimation((Position) e.NewValue, control.AnimateOpacity);
      VisualStateManager.GoToState((FrameworkElement) control, "Show", true);
    }

    static Flyout()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Flyout), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Flyout)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.root = (Grid) this.GetTemplateChild("root");
      if (this.root == null)
        return;
      this.PART_Header = (ContentPresenter) this.GetTemplateChild("PART_Header");
      this.PART_Content = (ContentPresenter) this.GetTemplateChild("PART_Content");
      this.windowTitleThumb = this.GetTemplateChild("PART_WindowTitleThumb") as Thumb;
      if (this.windowTitleThumb != null)
      {
        this.windowTitleThumb.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
        this.windowTitleThumb.DragDelta -= new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
        this.windowTitleThumb.MouseDoubleClick -= new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
        this.windowTitleThumb.MouseRightButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
        this.windowTitleThumb.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
        this.windowTitleThumb.DragDelta += new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
        this.windowTitleThumb.MouseDoubleClick += new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
        this.windowTitleThumb.MouseRightButtonUp += new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
      }
      this.hideStoryboard = (Storyboard) this.GetTemplateChild("HideStoryboard");
      this.hideFrame = (SplineDoubleKeyFrame) this.GetTemplateChild("hideFrame");
      this.hideFrameY = (SplineDoubleKeyFrame) this.GetTemplateChild("hideFrameY");
      this.showFrame = (SplineDoubleKeyFrame) this.GetTemplateChild("showFrame");
      this.showFrameY = (SplineDoubleKeyFrame) this.GetTemplateChild("showFrameY");
      this.fadeOutFrame = (SplineDoubleKeyFrame) this.GetTemplateChild("fadeOutFrame");
      if (this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null || this.fadeOutFrame == null)
        return;
      this.ApplyAnimation(this.Position, this.AnimateOpacity);
    }

    protected internal void CleanUp(FlyoutsControl flyoutsControl)
    {
      if (this.windowTitleThumb != null)
      {
        this.windowTitleThumb.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbOnPreviewMouseLeftButtonUp);
        this.windowTitleThumb.DragDelta -= new DragDeltaEventHandler(this.WindowTitleThumbMoveOnDragDelta);
        this.windowTitleThumb.MouseDoubleClick -= new MouseButtonEventHandler(this.WindowTitleThumbChangeWindowStateOnMouseDoubleClick);
        this.windowTitleThumb.MouseRightButtonUp -= new MouseButtonEventHandler(this.WindowTitleThumbSystemMenuOnMouseRightButtonUp);
      }
      this.parentWindow = (MetroWindow) null;
    }

    private void WindowTitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      MetroWindow parentWindow = this.ParentWindow;
      if (parentWindow == null || this.Position == Position.Bottom)
        return;
      MetroWindow.DoWindowTitleThumbOnPreviewMouseLeftButtonUp(parentWindow, e);
    }

    private void WindowTitleThumbMoveOnDragDelta(
      object sender,
      DragDeltaEventArgs dragDeltaEventArgs)
    {
      MetroWindow parentWindow = this.ParentWindow;
      if (parentWindow == null || this.Position == Position.Bottom)
        return;
      MetroWindow.DoWindowTitleThumbMoveOnDragDelta(parentWindow, dragDeltaEventArgs);
    }

    private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick(
      object sender,
      MouseButtonEventArgs mouseButtonEventArgs)
    {
      MetroWindow parentWindow = this.ParentWindow;
      if (parentWindow == null || this.Position == Position.Bottom)
        return;
      MetroWindow.DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(parentWindow, mouseButtonEventArgs);
    }

    private void WindowTitleThumbSystemMenuOnMouseRightButtonUp(
      object sender,
      MouseButtonEventArgs e)
    {
      MetroWindow parentWindow = this.ParentWindow;
      if (parentWindow == null || this.Position == Position.Bottom)
        return;
      MetroWindow.DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(parentWindow, e);
    }

    internal void ApplyAnimation(Position position, bool animateOpacity, bool resetShowFrame = true)
    {
      if (this.root == null || this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null || this.fadeOutFrame == null)
        return;
      if (this.Position == Position.Left || this.Position == Position.Right)
        this.showFrame.Value = 0.0;
      if (this.Position == Position.Top || this.Position == Position.Bottom)
        this.showFrameY.Value = 0.0;
      if (!animateOpacity)
      {
        this.fadeOutFrame.Value = 1.0;
        this.root.Opacity = 1.0;
      }
      else
      {
        this.fadeOutFrame.Value = 0.0;
        if (!this.IsOpen)
          this.root.Opacity = 0.0;
      }
      switch (position)
      {
        case Position.Right:
          this.HorizontalAlignment = HorizontalAlignment.Right;
          this.VerticalAlignment = VerticalAlignment.Stretch;
          this.hideFrame.Value = this.root.ActualWidth;
          if (!resetShowFrame)
            break;
          this.root.RenderTransform = (Transform) new TranslateTransform(this.root.ActualWidth, 0.0);
          break;
        case Position.Top:
          this.HorizontalAlignment = HorizontalAlignment.Stretch;
          this.VerticalAlignment = VerticalAlignment.Top;
          this.hideFrameY.Value = -this.root.ActualHeight - 1.0;
          if (!resetShowFrame)
            break;
          this.root.RenderTransform = (Transform) new TranslateTransform(0.0, -this.root.ActualHeight - 1.0);
          break;
        case Position.Bottom:
          this.HorizontalAlignment = HorizontalAlignment.Stretch;
          this.VerticalAlignment = VerticalAlignment.Bottom;
          this.hideFrameY.Value = this.root.ActualHeight;
          if (!resetShowFrame)
            break;
          this.root.RenderTransform = (Transform) new TranslateTransform(0.0, this.root.ActualHeight);
          break;
        default:
          this.HorizontalAlignment = HorizontalAlignment.Left;
          this.VerticalAlignment = VerticalAlignment.Stretch;
          this.hideFrame.Value = -this.root.ActualWidth;
          if (!resetShowFrame)
            break;
          this.root.RenderTransform = (Transform) new TranslateTransform(-this.root.ActualWidth, 0.0);
          break;
      }
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
      base.OnRenderSizeChanged(sizeInfo);
      if (!this.IsOpen || !sizeInfo.WidthChanged && !sizeInfo.HeightChanged || this.root == null || this.hideFrame == null || this.showFrame == null || this.hideFrameY == null || this.showFrameY == null)
        return;
      if (this.Position == Position.Left || this.Position == Position.Right)
        this.showFrame.Value = 0.0;
      if (this.Position == Position.Top || this.Position == Position.Bottom)
        this.showFrameY.Value = 0.0;
      switch (this.Position)
      {
        case Position.Right:
          this.hideFrame.Value = this.root.ActualWidth;
          break;
        case Position.Top:
          this.hideFrameY.Value = -this.root.ActualHeight - 1.0;
          break;
        case Position.Bottom:
          this.hideFrameY.Value = this.root.ActualHeight;
          break;
        default:
          this.hideFrame.Value = -this.root.ActualWidth;
          break;
      }
    }
  }
}
