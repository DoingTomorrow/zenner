// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonWindow
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  [SuppressMessage("Microsoft.Design", "CA1049")]
  public class RibbonWindow : Window
  {
    private const double Epsilon = 1.53E-06;
    private const int SwpFlags = 567;
    [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
    private static readonly int[,] HitTestBorders = new int[3, 3]
    {
      {
        13,
        12,
        14
      },
      {
        10,
        1,
        11
      },
      {
        16,
        15,
        17
      }
    };
    private IntPtr handle;
    private HwndSource hwndSource;
    private bool isHooked;
    private IntPtr mouseHook;
    private NativeMethods.HookProc mouseProc;
    private bool isFixedUp;
    private bool isUserResizing;
    private bool hasUserMovedWindow;
    private Point windowPosAtStartOfUserMove = new Point();
    private int blackGlassFixupAttemptCount;
    private WindowState lastRoundingState;
    private WindowState lastMenuState;
    private Grid mainGrid;
    public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register(nameof (ResizeBorderThickness), typeof (Thickness), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) new Thickness(9.0)));
    public static readonly DependencyProperty GlassBorderThicknessProperty = DependencyProperty.Register(nameof (GlassBorderThickness), typeof (Thickness), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) new Thickness(9.0, 29.0, 9.0, 9.0), new PropertyChangedCallback(RibbonWindow.OnGlassBorderThicknessChanged)));
    public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register(nameof (CaptionHeight), typeof (double), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) 20.0));
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) new CornerRadius(9.0, 9.0, 9.0, 9.0)));
    [SuppressMessage("Microsoft.Naming", "CA1704")]
    private static readonly DependencyPropertyKey IsDwmEnabledPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsDwmEnabled), typeof (bool), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) false));
    [SuppressMessage("Microsoft.Naming", "CA1704")]
    public static readonly DependencyProperty IsDwmEnabledProperty = RibbonWindow.IsDwmEnabledPropertyKey.DependencyProperty;
    public static readonly DependencyProperty IsIconVisibleProperty = DependencyProperty.Register(nameof (IsIconVisible), typeof (bool), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.Register(nameof (IsCollapsed), typeof (bool), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) false));
    private static readonly DependencyPropertyKey IsNonClientAreaActivePropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsNonClientAreaActive), typeof (bool), typeof (RibbonWindow), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsNonClientAreaActiveProperty = RibbonWindow.IsNonClientAreaActivePropertyKey.DependencyProperty;
    [SuppressMessage("Microsoft.Usage", "CA2211")]
    public static RoutedCommand MinimizeCommand = new RoutedCommand();
    [SuppressMessage("Microsoft.Usage", "CA2211")]
    public static RoutedCommand MaximizeCommand = new RoutedCommand();
    [SuppressMessage("Microsoft.Usage", "CA2211")]
    public static RoutedCommand NormalizeCommand = new RoutedCommand();
    [SuppressMessage("Microsoft.Usage", "CA2211")]
    public static RoutedCommand CloseCommand = new RoutedCommand();

    public Thickness ResizeBorderThickness
    {
      get => (Thickness) this.GetValue(RibbonWindow.ResizeBorderThicknessProperty);
      set => this.SetValue(RibbonWindow.ResizeBorderThicknessProperty, (object) value);
    }

    public Thickness GlassBorderThickness
    {
      get => (Thickness) this.GetValue(RibbonWindow.GlassBorderThicknessProperty);
      set => this.SetValue(RibbonWindow.GlassBorderThicknessProperty, (object) value);
    }

    private static void OnGlassBorderThicknessChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as RibbonWindow).UpdateFrameState(false);
    }

    public double CaptionHeight
    {
      get => (double) this.GetValue(RibbonWindow.CaptionHeightProperty);
      set => this.SetValue(RibbonWindow.CaptionHeightProperty, (object) value);
    }

    public CornerRadius CornerRadius
    {
      get => (CornerRadius) this.GetValue(RibbonWindow.CornerRadiusProperty);
      set => this.SetValue(RibbonWindow.CornerRadiusProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Naming", "CA1704")]
    public bool IsDwmEnabled
    {
      get => (bool) this.GetValue(RibbonWindow.IsDwmEnabledProperty);
      private set => this.SetValue(RibbonWindow.IsDwmEnabledPropertyKey, (object) value);
    }

    public bool IsIconVisible
    {
      get => (bool) this.GetValue(RibbonWindow.IsIconVisibleProperty);
      set => this.SetValue(RibbonWindow.IsIconVisibleProperty, (object) value);
    }

    public bool IsCollapsed
    {
      get => (bool) this.GetValue(RibbonWindow.IsCollapsedProperty);
      set => this.SetValue(RibbonWindow.IsCollapsedProperty, (object) value);
    }

    public bool IsNonClientAreaActive
    {
      get => (bool) this.GetValue(RibbonWindow.IsNonClientAreaActiveProperty);
      private set => this.SetValue(RibbonWindow.IsNonClientAreaActivePropertyKey, (object) value);
    }

    private bool IsWindowDocked
    {
      get
      {
        if (this.WindowState != WindowState.Normal)
          return false;
        NativeMethods.Rect adjustedWindowRect = this.GetAdjustedWindowRect(new NativeMethods.Rect()
        {
          Bottom = 100,
          Right = 100
        });
        return this.RestoreBounds.Location != new Point(this.Left, this.Top) - (Vector) DpiHelper.DevicePixelsToLogical(new Point((double) adjustedWindowRect.Left, (double) adjustedWindowRect.Top));
      }
    }

    static RibbonWindow()
    {
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (RibbonWindow), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RibbonWindow.OnCoerceStyle)));
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (RibbonWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (RibbonWindow)));
      if (FrameworkHelper.PresentationFrameworkVersion < new Version("4.0"))
      {
        Control.TemplateProperty.AddOwner(typeof (RibbonWindow), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(RibbonWindow.OnWindowPropertyChangedThatRequiresTemplateFixup)));
        FrameworkElement.FlowDirectionProperty.AddOwner(typeof (RibbonWindow), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(RibbonWindow.OnWindowPropertyChangedThatRequiresTemplateFixup)));
      }
      CommandManager.RegisterClassCommandBinding(typeof (RibbonWindow), new CommandBinding((ICommand) RibbonWindow.CloseCommand, new ExecutedRoutedEventHandler(RibbonWindow.OnCloseCommandExecuted)));
      CommandManager.RegisterClassCommandBinding(typeof (RibbonWindow), new CommandBinding((ICommand) RibbonWindow.MinimizeCommand, new ExecutedRoutedEventHandler(RibbonWindow.OnMinimizeCommandExecuted)));
      CommandManager.RegisterClassCommandBinding(typeof (RibbonWindow), new CommandBinding((ICommand) RibbonWindow.MaximizeCommand, new ExecutedRoutedEventHandler(RibbonWindow.OnMaximizeCommandExecuted)));
      CommandManager.RegisterClassCommandBinding(typeof (RibbonWindow), new CommandBinding((ICommand) RibbonWindow.NormalizeCommand, new ExecutedRoutedEventHandler(RibbonWindow.OnNormalizeCommandExecuted)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (RibbonWindow));
      return basevalue;
    }

    private static void OnWindowPropertyChangedThatRequiresTemplateFixup(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      RibbonWindow ribbonWindow = d as RibbonWindow;
      IntPtr handle = ribbonWindow.handle;
      ribbonWindow.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new ThreadStart(ribbonWindow.FixFrameworkIssues));
    }

    public RibbonWindow()
    {
      this.SizeChanged += new SizeChangedEventHandler(this.OnSizeChanged);
      this.Closed += new EventHandler(this.OnWindowClosed);
    }

    private void OnWindowClosed(object sender, EventArgs e)
    {
      NativeMethods.UnhookWindowsHookEx(this.mouseHook);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize.Width < 300.0 || e.NewSize.Height < 250.0)
        this.IsCollapsed = true;
      else
        this.IsCollapsed = false;
    }

    private static void OnCloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (!(sender is RibbonWindow ribbonWindow))
        return;
      ribbonWindow.Close();
    }

    private static void OnMaximizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (!(sender is RibbonWindow ribbonWindow))
        return;
      ribbonWindow.WindowState = WindowState.Maximized;
    }

    private static void OnNormalizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (!(sender is RibbonWindow ribbonWindow))
        return;
      ribbonWindow.WindowState = WindowState.Normal;
    }

    private static void OnMinimizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      if (!(sender is RibbonWindow ribbonWindow))
        return;
      ribbonWindow.WindowState = WindowState.Minimized;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);
      this.handle = new WindowInteropHelper((Window) this).Handle;
      this.hwndSource = HwndSource.FromHwnd(this.handle);
      this.IsDwmEnabled = NativeMethods.IsDwmEnabled();
      this.ApplyCustomChrome();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.mainGrid = this.GetTemplateChild("PART_MainGrid") as Grid;
      FrameworkElement iconImage = this.GetTemplateChild("PART_IconImage") as FrameworkElement;
      if (iconImage == null)
        return;
      iconImage.MouseUp += (MouseButtonEventHandler) ((o, e) =>
      {
        if (e.ChangedButton == MouseButton.Left)
        {
          if (e.ClickCount != 1)
            return;
          Point pos = iconImage.PointToScreen(new Point(0.0, 0.0));
          Size size = new Size(iconImage.ActualWidth, iconImage.ActualHeight);
          size = DpiHelper.LogicalSizeToDevice(size);
          if (this.FlowDirection == FlowDirection.RightToLeft)
            pos.X += size.Width;
          if (this.FlowDirection == FlowDirection.LeftToRight)
          {
            this.ShowSystemMenu(new Point(pos.X, pos.Y + size.Height));
          }
          else
          {
            DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.1);
            dispatcherTimer.Tick += (EventHandler) ((s, ee) =>
            {
              this.ShowSystemMenu(new Point(pos.X, pos.Y + size.Height));
              (s as DispatcherTimer).Stop();
            });
            dispatcherTimer.Start();
          }
        }
        else
        {
          if (e.ChangedButton != MouseButton.Right)
            return;
          this.ShowSystemMenu(iconImage.PointToScreen(Mouse.GetPosition((IInputElement) iconImage)));
        }
      });
      iconImage.MouseDown += (MouseButtonEventHandler) ((o, e) =>
      {
        if (e.ChangedButton != MouseButton.Left || e.ClickCount != 2)
          return;
        this.Close();
      });
    }

    private void ApplyCustomChrome()
    {
      if (!this.isHooked)
      {
        this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));
        this.mouseProc = new NativeMethods.HookProc(this.MouseWndProc);
        this.mouseHook = NativeMethods.SetWindowsHookEx(NativeMethods.HookType.WH_MOUSE, this.mouseProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
        this.isHooked = true;
      }
      this.FixFrameworkIssues();
      this.UpdateSystemMenu(new WindowState?(this.WindowState));
      this.UpdateFrameState(true);
      NativeMethods.SetWindowPos(this.handle, IntPtr.Zero, 0, 0, 0, 0, 567);
    }

    private void FixFrameworkIssues()
    {
      if (FrameworkHelper.PresentationFrameworkVersion >= new Version("4.0") || this.Template == null)
        return;
      if (VisualTreeHelper.GetChildrenCount((DependencyObject) this) == 0)
      {
        this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new ThreadStart(this.FixFrameworkIssues));
      }
      else
      {
        FrameworkElement child = (FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) this, 0);
        NativeMethods.Rect lpRect = new NativeMethods.Rect();
        NativeMethods.GetWindowRect(this.handle, ref lpRect);
        NativeMethods.Rect adjustedWindowRect = this.GetAdjustedWindowRect(lpRect);
        System.Windows.Rect logical1 = DpiHelper.DeviceRectToLogical(new System.Windows.Rect((double) lpRect.Left, (double) lpRect.Top, (double) (lpRect.Right - lpRect.Left), (double) (lpRect.Bottom - lpRect.Top)));
        System.Windows.Rect logical2 = DpiHelper.DeviceRectToLogical(new System.Windows.Rect((double) adjustedWindowRect.Left, (double) adjustedWindowRect.Top, (double) (adjustedWindowRect.Right - adjustedWindowRect.Left), (double) (adjustedWindowRect.Bottom - adjustedWindowRect.Top)));
        Thickness thickness = new Thickness(logical1.Left - logical2.Left, logical1.Top - logical2.Top, logical2.Right - logical1.Right, logical2.Bottom - logical1.Bottom);
        child.Margin = new Thickness(0.0, 0.0, -(thickness.Left + thickness.Right), -(thickness.Top + thickness.Bottom));
        if (this.FlowDirection == FlowDirection.RightToLeft)
          child.RenderTransform = (Transform) new MatrixTransform(1.0, 0.0, 0.0, 1.0, -(thickness.Left + thickness.Right), 0.0);
        else
          child.RenderTransform = (Transform) null;
        if (this.isFixedUp)
          return;
        this.hasUserMovedWindow = false;
        this.StateChanged += new EventHandler(this.FixRestoreBounds);
        this.isFixedUp = true;
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
    private void FixWindows7Issues()
    {
      if (this.blackGlassFixupAttemptCount > 5 || !this.IsDwmEnabled)
        return;
      ++this.blackGlassFixupAttemptCount;
      bool flag = false;
      try
      {
        flag = NativeMethods.DwmGetCompositionTimingInfo(this.handle).HasValue;
      }
      catch (Exception ex)
      {
      }
      if (!flag)
        this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new ThreadStart(this.FixWindows7Issues));
      else
        this.blackGlassFixupAttemptCount = 0;
    }

    private void FixRestoreBounds(object sender, EventArgs e)
    {
      if (this.WindowState != WindowState.Maximized && this.WindowState != WindowState.Minimized || !this.hasUserMovedWindow)
        return;
      this.hasUserMovedWindow = false;
      NativeMethods.WINDOWPLACEMENT lpwndpl = new NativeMethods.WINDOWPLACEMENT();
      NativeMethods.GetWindowPlacement(this.handle, lpwndpl);
      NativeMethods.Rect adjustedWindowRect = this.GetAdjustedWindowRect(new NativeMethods.Rect()
      {
        Bottom = 100,
        Right = 100
      });
      Point logical = DpiHelper.DevicePixelsToLogical(new Point((double) (lpwndpl.rcNormalPosition.Left - adjustedWindowRect.Left), (double) (lpwndpl.rcNormalPosition.Top - adjustedWindowRect.Top)));
      this.Top = logical.Y;
      this.Left = logical.X;
    }

    private NativeMethods.Rect GetAdjustedWindowRect(NativeMethods.Rect rcWindow)
    {
      IntPtr windowLongPtr1 = NativeMethods.GetWindowLongPtr(this.handle, -16);
      IntPtr windowLongPtr2 = NativeMethods.GetWindowLongPtr(this.handle, -20);
      NativeMethods.AdjustWindowRectEx(ref rcWindow, (int) windowLongPtr1, false, (int) windowLongPtr2);
      return rcWindow;
    }

    private bool ModifyStyle(int removeStyle, int addStyle)
    {
      int int32 = NativeMethods.GetWindowLongPtr(this.handle, -16).ToInt32();
      int num = int32 & ~removeStyle | addStyle;
      if (int32 == num)
        return false;
      NativeMethods.SetWindowLongPtr(this.handle, -16, new IntPtr(num));
      return true;
    }

    private int HitTestNonClientArea(System.Windows.Rect windowPosition, Point mousePosition)
    {
      int index1 = 1;
      int index2 = 1;
      bool flag = false;
      if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + this.ResizeBorderThickness.Top + this.CaptionHeight)
      {
        flag = mousePosition.Y < windowPosition.Top + this.ResizeBorderThickness.Top;
        index1 = 0;
      }
      else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double) (int) this.ResizeBorderThickness.Bottom)
        index1 = 2;
      if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double) (int) this.ResizeBorderThickness.Left)
        index2 = 0;
      else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - this.ResizeBorderThickness.Right)
        index2 = 2;
      if (index1 == 0 && index2 != 1 && !flag)
        index1 = 1;
      int num = RibbonWindow.HitTestBorders[index1, index2];
      if (num == 12 && !flag)
        num = 2;
      return num;
    }

    public void ShowSystemMenu(Point screenLocation)
    {
      this.ShowSystemMenuPhysicalCoordinates(screenLocation);
    }

    private void ShowSystemMenuPhysicalCoordinates(Point physicalScreenLocation)
    {
      if (this.handle == IntPtr.Zero)
        return;
      uint num = NativeMethods.TrackPopupMenuEx(NativeMethods.GetSystemMenu(this.handle, false), 256U, (int) physicalScreenLocation.X, (int) physicalScreenLocation.Y, this.handle, IntPtr.Zero);
      if (num == 0U)
        return;
      NativeMethods.PostMessage(this.handle, 274, new IntPtr((long) num), IntPtr.Zero);
    }

    private WindowState GetHwndState()
    {
      NativeMethods.WINDOWPLACEMENT lpwndpl = new NativeMethods.WINDOWPLACEMENT();
      NativeMethods.GetWindowPlacement(this.handle, lpwndpl);
      switch (lpwndpl.showCmd)
      {
        case 2:
          return WindowState.Minimized;
        case 3:
          return WindowState.Maximized;
        default:
          return WindowState.Normal;
      }
    }

    private static bool IsFlagSet(int value, int mask) => 0 != (value & mask);

    private void UpdateSystemMenu(WindowState? assumeState)
    {
      WindowState windowState = (WindowState) ((int) assumeState ?? (int) this.GetHwndState());
      if (!assumeState.HasValue && this.lastMenuState == windowState)
        return;
      this.lastMenuState = windowState;
      bool flag1 = this.ModifyStyle(268435456, 0);
      IntPtr systemMenu = NativeMethods.GetSystemMenu(this.handle, false);
      if (IntPtr.Zero != systemMenu)
      {
        int int32 = NativeMethods.GetWindowLongPtr(this.handle, -16).ToInt32();
        bool flag2 = RibbonWindow.IsFlagSet(int32, 131072);
        bool flag3 = RibbonWindow.IsFlagSet(int32, 65536);
        bool flag4 = RibbonWindow.IsFlagSet(int32, 262144);
        switch (windowState)
        {
          case WindowState.Minimized:
            NativeMethods.EnableMenuItem(systemMenu, 61728, 0U);
            NativeMethods.EnableMenuItem(systemMenu, 61456, 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61440, 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61472, 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61488, flag3 ? 0U : 3U);
            break;
          case WindowState.Maximized:
            NativeMethods.EnableMenuItem(systemMenu, 61728, 0U);
            NativeMethods.EnableMenuItem(systemMenu, 61456, 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61440, 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61472, flag2 ? 0U : 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61488, 3U);
            break;
          default:
            NativeMethods.EnableMenuItem(systemMenu, 61728, 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61456, 0U);
            NativeMethods.EnableMenuItem(systemMenu, 61440, flag4 ? 0U : 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61472, flag2 ? 0U : 3U);
            NativeMethods.EnableMenuItem(systemMenu, 61488, flag3 ? 0U : 3U);
            break;
        }
      }
      if (!flag1)
        return;
      this.ModifyStyle(0, 268435456);
    }

    private void UpdateFrameState(bool force)
    {
      if (IntPtr.Zero == this.handle)
        return;
      if (!this.IsDwmEnabled)
      {
        this.SetRoundingRegion(new NativeMethods.WINDOWPOS?());
      }
      else
      {
        this.ClearRoundingRegion();
        this.ExtendGlassFrame();
        this.FixWindows7Issues();
      }
      NativeMethods.SetWindowPos(this.handle, IntPtr.Zero, 0, 0, 0, 0, 567);
      this.UpdateLayout();
    }

    private void ClearRoundingRegion()
    {
      NativeMethods.SetWindowRgn(this.handle, IntPtr.Zero, NativeMethods.IsWindowVisible(this.handle));
    }

    private void SetRoundingRegion(NativeMethods.WINDOWPOS? wp)
    {
      NativeMethods.WINDOWPLACEMENT lpwndpl = new NativeMethods.WINDOWPLACEMENT();
      NativeMethods.GetWindowPlacement(this.handle, lpwndpl);
      if (lpwndpl.showCmd == 3)
      {
        int num1;
        int num2;
        if (wp.HasValue)
        {
          num1 = wp.Value.x;
          num2 = wp.Value.y;
        }
        else
        {
          NativeMethods.Rect lpRect = new NativeMethods.Rect();
          NativeMethods.GetWindowRect(this.handle, ref lpRect);
          num1 = lpRect.Left;
          num2 = lpRect.Top;
        }
        IntPtr hMonitor = NativeMethods.MonitorFromWindow(this.handle, 2U);
        NativeMethods.MonitorInfo lpmi = new NativeMethods.MonitorInfo();
        NativeMethods.GetMonitorInfo(hMonitor, lpmi);
        NativeMethods.Rect work = lpmi.Work;
        work.Left -= num1;
        work.Right -= num1;
        work.Top -= num2;
        work.Bottom -= num2;
        IntPtr num3 = IntPtr.Zero;
        try
        {
          num3 = NativeMethods.CreateRectRgnIndirect(ref work);
          NativeMethods.SetWindowRgn(this.handle, num3, NativeMethods.IsWindowVisible(this.handle));
          num3 = IntPtr.Zero;
        }
        finally
        {
          NativeMethods.DeleteObject(num3);
        }
      }
      else
      {
        Size size;
        if (wp.HasValue && !RibbonWindow.IsFlagSet(wp.Value.flags, 1))
        {
          size = new Size((double) wp.Value.cx, (double) wp.Value.cy);
        }
        else
        {
          if (wp.HasValue && this.lastRoundingState == this.WindowState)
            return;
          NativeMethods.Rect lpRect = new NativeMethods.Rect();
          NativeMethods.GetWindowRect(this.handle, ref lpRect);
          size = new System.Windows.Rect((double) lpRect.Left, (double) lpRect.Top, (double) (lpRect.Right - lpRect.Left), (double) (lpRect.Bottom - lpRect.Top)).Size;
        }
        this.lastRoundingState = this.WindowState;
        IntPtr num4 = IntPtr.Zero;
        try
        {
          double num5 = Math.Min(size.Width, size.Height);
          double radius1 = Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(this.CornerRadius.TopLeft, 0.0)).X, num5 / 2.0);
          if (RibbonWindow.IsUniform(this.CornerRadius))
          {
            num4 = RibbonWindow.CreateRoundRectRgn(new System.Windows.Rect(size), radius1);
          }
          else
          {
            num4 = RibbonWindow.CreateRoundRectRgn(new System.Windows.Rect(0.0, 0.0, size.Width / 2.0 + radius1, size.Height / 2.0 + radius1), radius1);
            double radius2 = Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(this.CornerRadius.TopRight, 0.0)).X, num5 / 2.0);
            System.Windows.Rect region1 = new System.Windows.Rect(0.0, 0.0, size.Width / 2.0 + radius2, size.Height / 2.0 + radius2);
            region1.Offset(size.Width / 2.0 - radius2, 0.0);
            RibbonWindow.CreateAndCombineRoundRectRgn(num4, region1, radius2);
            double radius3 = Math.Min(new Point(this.CornerRadius.BottomLeft, 0.0).X, num5 / 2.0);
            System.Windows.Rect region2 = new System.Windows.Rect(0.0, 0.0, size.Width / 2.0 + radius3, size.Height / 2.0 + radius3);
            region2.Offset(0.0, size.Height / 2.0 - radius3);
            RibbonWindow.CreateAndCombineRoundRectRgn(num4, region2, radius3);
            double radius4 = Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(this.CornerRadius.BottomRight, 0.0)).X, num5 / 2.0);
            System.Windows.Rect region3 = new System.Windows.Rect(0.0, 0.0, size.Width / 2.0 + radius4, size.Height / 2.0 + radius4);
            region3.Offset(size.Width / 2.0 - radius4, size.Height / 2.0 - radius4);
            RibbonWindow.CreateAndCombineRoundRectRgn(num4, region3, radius4);
          }
          NativeMethods.SetWindowRgn(this.handle, num4, NativeMethods.IsWindowVisible(this.handle));
          num4 = IntPtr.Zero;
        }
        finally
        {
          NativeMethods.DeleteObject(num4);
        }
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static bool AreClose(double value1, double value2)
    {
      if (value1 == value2)
        return true;
      double num = value1 - value2;
      return num < 1.53E-06 && num > -1.53E-06;
    }

    private static IntPtr CreateRoundRectRgn(System.Windows.Rect region, double radius)
    {
      return RibbonWindow.AreClose(0.0, radius) ? NativeMethods.CreateRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top), (int) Math.Ceiling(region.Right), (int) Math.Ceiling(region.Bottom)) : NativeMethods.CreateRoundRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top), (int) Math.Ceiling(region.Right) + 1, (int) Math.Ceiling(region.Bottom) + 1, (int) Math.Ceiling(radius), (int) Math.Ceiling(radius));
    }

    [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "HRGNs")]
    private static void CreateAndCombineRoundRectRgn(IntPtr hrgnSource, System.Windows.Rect region, double radius)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        num = RibbonWindow.CreateRoundRectRgn(region, radius);
        if (NativeMethods.CombineRgn(hrgnSource, hrgnSource, num, 2) == 0)
          throw new InvalidOperationException("Unable to combine two HRGNs.");
      }
      catch
      {
        NativeMethods.DeleteObject(num);
        throw;
      }
    }

    private static bool IsUniform(CornerRadius cornerRadius)
    {
      return RibbonWindow.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight) && RibbonWindow.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight) && RibbonWindow.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight);
    }

    private void ExtendGlassFrame()
    {
      if (IntPtr.Zero == this.handle)
        return;
      if (!this.IsDwmEnabled)
      {
        this.hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
      }
      else
      {
        this.hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
        Point device1 = DpiHelper.LogicalPixelsToDevice(new Point(this.GlassBorderThickness.Left, this.GlassBorderThickness.Top));
        Point device2 = DpiHelper.LogicalPixelsToDevice(new Point(this.GlassBorderThickness.Right, this.GlassBorderThickness.Bottom));
        NativeMethods.DwmExtendFrameIntoClientArea(this.handle, new NativeMethods.MARGINS((int) Math.Ceiling(device1.X), (int) Math.Ceiling(device1.Y), (int) Math.Ceiling(device2.X), (int) Math.Ceiling(device2.Y)));
      }
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      switch (msg)
      {
        case 5:
          WindowState? assumeState = new WindowState?();
          if (wParam.ToInt32() == 2)
            assumeState = new WindowState?(WindowState.Maximized);
          this.UpdateSystemMenu(assumeState);
          handled = false;
          return IntPtr.Zero;
        case 12:
        case 128:
          bool flag = this.ModifyStyle(268435456, 0);
          IntPtr num1 = NativeMethods.DefWindowProc(this.handle, msg, wParam, lParam);
          if (flag)
            this.ModifyStyle(0, 268435456);
          handled = true;
          return num1;
        case 71:
          if (!this.IsDwmEnabled)
          {
            NativeMethods.WINDOWPOS structure = (NativeMethods.WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (NativeMethods.WINDOWPOS));
            this.ModifyStyle(268435456, 0);
            this.SetRoundingRegion(new NativeMethods.WINDOWPOS?(structure));
            this.ModifyStyle(0, 268435456);
          }
          handled = false;
          return IntPtr.Zero;
        case 131:
          handled = true;
          return new IntPtr(768);
        case 132:
          return this.DoHitTest(msg, wParam, lParam, out handled);
        case 134:
          IntPtr num2 = NativeMethods.DefWindowProc(this.handle, 134, wParam, new IntPtr(-1));
          this.IsNonClientAreaActive = wParam != IntPtr.Zero;
          handled = true;
          return num2;
        case 165:
          if (2 == wParam.ToInt32())
            this.ShowSystemMenuPhysicalCoordinates(new Point((double) NativeMethods.LowWord(lParam), (double) NativeMethods.HiWord(lParam)));
          handled = false;
          return IntPtr.Zero;
        case 798:
          this.IsDwmEnabled = NativeMethods.IsDwmEnabled();
          this.UpdateFrameState(false);
          handled = false;
          return IntPtr.Zero;
        default:
          if (FrameworkHelper.PresentationFrameworkVersion < new Version("4.0"))
          {
            switch (msg)
            {
              case 3:
                if (this.isUserResizing)
                  this.hasUserMovedWindow = true;
                handled = false;
                return IntPtr.Zero;
              case 26:
                this.FixFrameworkIssues();
                handled = false;
                return IntPtr.Zero;
              case 561:
                this.isUserResizing = true;
                if (this.WindowState != WindowState.Maximized && !this.IsWindowDocked)
                  this.windowPosAtStartOfUserMove = new Point(this.Left, this.Top);
                handled = false;
                return IntPtr.Zero;
              case 562:
                this.isUserResizing = false;
                if (this.WindowState == WindowState.Maximized)
                {
                  this.Top = this.windowPosAtStartOfUserMove.Y;
                  this.Left = this.windowPosAtStartOfUserMove.X;
                }
                handled = false;
                return IntPtr.Zero;
            }
          }
          return IntPtr.Zero;
      }
    }

    private IntPtr DoHitTest(int msg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      IntPtr plResult = IntPtr.Zero;
      handled = false;
      if (this.IsDwmEnabled && Mouse.Captured == null)
        handled = NativeMethods.DwmDefWindowProc(this.handle, msg, wParam, lParam, ref plResult);
      if (IntPtr.Zero == plResult)
      {
        Point point = new Point((double) NativeMethods.LowWord(lParam), (double) NativeMethods.HiWord(lParam));
        NativeMethods.Rect lpRect = new NativeMethods.Rect();
        NativeMethods.GetWindowRect(this.handle, ref lpRect);
        int num = this.HitTestNonClientArea(DpiHelper.DeviceRectToLogical(new System.Windows.Rect((double) lpRect.Left, (double) lpRect.Top, (double) (lpRect.Right - lpRect.Left), (double) (lpRect.Bottom - lpRect.Top))), DpiHelper.DevicePixelsToLogical(point));
        if (num != 1 && this.mainGrid != null && this.mainGrid.IsLoaded)
        {
          int int32 = lParam.ToInt32();
          if (!this.mainGrid.IsVisible)
            return IntPtr.Zero;
          IInputElement inputElement = this.mainGrid.InputHitTest(DpiHelper.DevicePixelsToLogical(this.mainGrid.PointFromScreen(new Point((double) (short) (int32 & (int) ushort.MaxValue), (double) (short) (int32 >> 16 & (int) ushort.MaxValue)))));
          if (inputElement != null)
          {
            if ((inputElement as FrameworkElement).Name == "PART_TitleBar")
              num = 2;
            else if (inputElement != this.mainGrid)
              num = 1;
          }
        }
        if (this.GetTemplateChild("PART_ResizeGrip") is ResizeGrip templateChild && templateChild.IsLoaded && templateChild.InputHitTest(templateChild.PointFromScreen(point)) != null)
          num = this.FlowDirection != FlowDirection.LeftToRight ? 16 : 17;
        handled = true;
        plResult = new IntPtr(num);
      }
      return plResult;
    }

    private IntPtr MouseWndProc(int code, IntPtr wParam, IntPtr lParam)
    {
      if (code >= 0 && Mouse.Captured != null)
      {
        int int32_1 = wParam.ToInt32();
        NativeMethods.MOUSEHOOKSTRUCT structure = (NativeMethods.MOUSEHOOKSTRUCT) Marshal.PtrToStructure(lParam, typeof (NativeMethods.MOUSEHOOKSTRUCT));
        switch (int32_1)
        {
          case 513:
          case 515:
          case 516:
          case 518:
            if (this.IsInRootPopup(Mouse.Captured as DependencyObject))
              return IntPtr.Zero;
            IntPtr pp = Marshal.AllocHGlobal(Marshal.SizeOf((object) structure.pt));
            Marshal.StructureToPtr((object) structure.pt, pp, false);
            bool handled;
            IntPtr htResult = this.DoHitTest(132, IntPtr.Zero, NativeMethods.MakeDWord(structure.pt.x, structure.pt.y), out handled);
            if (!handled)
              htResult = NativeMethods.DefWindowProc(this.handle, 132, IntPtr.Zero, NativeMethods.MakeDWord(structure.pt.x, structure.pt.y));
            int int32_2 = htResult.ToInt32();
            int ncMessage = 161;
            switch (int32_1)
            {
              case 515:
                ncMessage = 163;
                break;
              case 516:
                if (int32_2 == 2 || int32_2 == 12)
                {
                  NativeMethods.ReleaseCapture();
                  if (int32_2 == 2)
                    this.ShowSystemMenu(new Point((double) structure.pt.x, (double) structure.pt.y));
                  ncMessage = 164;
                  break;
                }
                break;
            }
            if (int32_2 == 2 || int32_2 == 12)
            {
              NativeMethods.ReleaseCapture();
              htResult = this.DoHitTest(132, IntPtr.Zero, NativeMethods.MakeDWord(structure.pt.x, structure.pt.y), out handled);
              if (!handled)
                htResult = NativeMethods.DefWindowProc(this.handle, 132, IntPtr.Zero, NativeMethods.MakeDWord(structure.pt.x, structure.pt.y));
              int32_2 = htResult.ToInt32();
            }
            if (int32_2 == 3 || int32_2 == 8 || int32_2 == 9 || int32_2 == 20 || int32_2 == 21)
            {
              NativeMethods.ReleaseCapture();
              NativeMethods.SendMessage(this.handle, ncMessage, htResult, pp);
            }
            else if (int32_2 != 1)
            {
              NativeMethods.ReleaseCapture();
              this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (() => NativeMethods.SendMessage(this.handle, ncMessage, htResult, pp)));
            }
            return IntPtr.Zero;
        }
      }
      return NativeMethods.CallNextHookEx(this.mouseHook, code, wParam, lParam);
    }

    private bool IsInRootPopup(DependencyObject element)
    {
      if (PopupService.IsMousePhysicallyOver(element as UIElement) || element is IDropDownControl dropDownControl && dropDownControl.IsDropDownOpen && dropDownControl.DropDownPopup != null && dropDownControl.DropDownPopup.Child != null && PopupService.IsMousePhysicallyOver(dropDownControl.DropDownPopup.Child) || element is ContextMenu element1 && element1.IsOpen && PopupService.IsMousePhysicallyOver((UIElement) element1) || element is MenuItem menuItem && menuItem.IsDropDownOpen && PopupService.IsMousePhysicallyOver(menuItem.DropDownPopup.Child) || element is Popup popup && popup.IsOpen && PopupService.IsMousePhysicallyOver(popup.Child))
        return true;
      foreach (object child in LogicalTreeHelper.GetChildren(element))
      {
        if (child is DependencyObject element2 && (element2 as FrameworkElement).IsVisible && this.IsInRootPopup(element2))
          return true;
      }
      return false;
    }
  }
}
