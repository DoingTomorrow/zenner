// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.BorderlessWindowBehavior
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls;
using MahApps.Metro.Native;
using Microsoft.Windows.Shell;
using Standard;
using System;
using System.ComponentModel;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Interop;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class BorderlessWindowBehavior : Behavior<Window>
  {
    private IntPtr handle;
    private HwndSource hwndSource;
    private WindowChrome windowChrome;
    private PropertyChangeNotifier borderThicknessChangeNotifier;
    private Thickness? savedBorderThickness;
    private PropertyChangeNotifier topMostChangeNotifier;
    private bool savedTopMost;
    private bool isCleanedUp;
    [Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" properties in your Window to get a drop shadow around it.")]
    public static readonly DependencyProperty EnableDWMDropShadowProperty = DependencyProperty.Register(nameof (EnableDWMDropShadow), typeof (bool), typeof (BorderlessWindowBehavior), new PropertyMetadata((object) false));

    protected override void OnAttached()
    {
      this.windowChrome = new WindowChrome()
      {
        ResizeBorderThickness = SystemParameters.WindowResizeBorderThickness,
        CaptionHeight = 0.0,
        CornerRadius = new CornerRadius(0.0),
        GlassFrameThickness = new Thickness(0.0),
        UseAeroCaptionButtons = false
      };
      if (this.AssociatedObject is MetroWindow associatedObject)
      {
        this.windowChrome.IgnoreTaskbarOnMaximize = associatedObject.IgnoreTaskbarOnMaximize;
        this.windowChrome.UseNoneWindowStyle = associatedObject.UseNoneWindowStyle;
        DependencyPropertyDescriptor.FromProperty(MetroWindow.IgnoreTaskbarOnMaximizeProperty, typeof (MetroWindow)).AddValueChanged((object) this.AssociatedObject, new EventHandler(this.IgnoreTaskbarOnMaximizePropertyChangedCallback));
        DependencyPropertyDescriptor.FromProperty(MetroWindow.UseNoneWindowStyleProperty, typeof (MetroWindow)).AddValueChanged((object) this.AssociatedObject, new EventHandler(this.UseNoneWindowStylePropertyChangedCallback));
      }
      this.AssociatedObject.SetValue(WindowChrome.WindowChromeProperty, (object) this.windowChrome);
      IntPtr handle = new WindowInteropHelper(this.AssociatedObject).Handle;
      if (!this.AssociatedObject.IsLoaded)
      {
        if (handle == IntPtr.Zero)
        {
          try
          {
            this.AssociatedObject.AllowsTransparency = false;
          }
          catch (Exception ex)
          {
          }
        }
      }
      this.AssociatedObject.WindowStyle = WindowStyle.None;
      this.savedBorderThickness = new Thickness?(this.AssociatedObject.BorderThickness);
      this.borderThicknessChangeNotifier = new PropertyChangeNotifier((DependencyObject) this.AssociatedObject, Control.BorderThicknessProperty);
      this.borderThicknessChangeNotifier.ValueChanged += new EventHandler(this.BorderThicknessChangeNotifierOnValueChanged);
      this.savedTopMost = this.AssociatedObject.Topmost;
      this.topMostChangeNotifier = new PropertyChangeNotifier((DependencyObject) this.AssociatedObject, Window.TopmostProperty);
      this.topMostChangeNotifier.ValueChanged += new EventHandler(this.TopMostChangeNotifierOnValueChanged);
      this.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObject_Loaded);
      this.AssociatedObject.Unloaded += new RoutedEventHandler(this.AssociatedObject_Unloaded);
      this.AssociatedObject.SourceInitialized += new EventHandler(this.AssociatedObject_SourceInitialized);
      this.AssociatedObject.StateChanged += new EventHandler(this.OnAssociatedObjectHandleMaximize);
      this.HandleMaximize();
      base.OnAttached();
    }

    private void BorderThicknessChangeNotifierOnValueChanged(object sender, EventArgs e)
    {
      this.savedBorderThickness = new Thickness?(this.AssociatedObject.BorderThickness);
    }

    private void TopMostChangeNotifierOnValueChanged(object sender, EventArgs e)
    {
      this.savedTopMost = this.AssociatedObject.Topmost;
    }

    private void UseNoneWindowStylePropertyChangedCallback(object sender, EventArgs e)
    {
      if (!(sender is MetroWindow metroWindow) || this.windowChrome == null || object.Equals((object) this.windowChrome.UseNoneWindowStyle, (object) metroWindow.UseNoneWindowStyle))
        return;
      this.windowChrome.UseNoneWindowStyle = metroWindow.UseNoneWindowStyle;
      this.ForceRedrawWindowFromPropertyChanged();
    }

    private void IgnoreTaskbarOnMaximizePropertyChangedCallback(object sender, EventArgs e)
    {
      if (!(sender is MetroWindow metroWindow) || this.windowChrome == null || object.Equals((object) this.windowChrome.IgnoreTaskbarOnMaximize, (object) metroWindow.IgnoreTaskbarOnMaximize))
        return;
      bool flag = this._ModifyStyle(WS.THICKFRAME | WS.GROUP | WS.TABSTOP, WS.OVERLAPPED);
      this.windowChrome.IgnoreTaskbarOnMaximize = metroWindow.IgnoreTaskbarOnMaximize;
      this.ForceRedrawWindowFromPropertyChanged();
      if (!flag)
        return;
      this._ModifyStyle(WS.OVERLAPPED, WS.THICKFRAME | WS.GROUP | WS.TABSTOP);
    }

    [SecurityCritical]
    private bool _ModifyStyle(WS removeStyle, WS addStyle)
    {
      if (this.handle == IntPtr.Zero)
        return false;
      IntPtr windowLongPtr = NativeMethods.GetWindowLongPtr(this.handle, GWL.STYLE);
      WS ws1 = Environment.Is64BitProcess ? (WS) windowLongPtr.ToInt64() : (WS) windowLongPtr.ToInt32();
      WS ws2 = ws1 & ~removeStyle | addStyle;
      if (ws1 == ws2)
        return false;
      NativeMethods.SetWindowLongPtr(this.handle, GWL.STYLE, new IntPtr((int) ws2));
      return true;
    }

    private void ForceRedrawWindowFromPropertyChanged()
    {
      this.HandleMaximize();
      if (!(this.handle != IntPtr.Zero))
        return;
      UnsafeNativeMethods.RedrawWindow(this.handle, IntPtr.Zero, IntPtr.Zero, Constants.RedrawWindowFlags.Invalidate | Constants.RedrawWindowFlags.Frame);
    }

    private void Cleanup()
    {
      if (this.isCleanedUp)
        return;
      this.isCleanedUp = true;
      if (this.AssociatedObject is MetroWindow)
      {
        DependencyPropertyDescriptor.FromProperty(MetroWindow.IgnoreTaskbarOnMaximizeProperty, typeof (MetroWindow)).RemoveValueChanged((object) this.AssociatedObject, new EventHandler(this.IgnoreTaskbarOnMaximizePropertyChangedCallback));
        DependencyPropertyDescriptor.FromProperty(MetroWindow.UseNoneWindowStyleProperty, typeof (MetroWindow)).RemoveValueChanged((object) this.AssociatedObject, new EventHandler(this.UseNoneWindowStylePropertyChangedCallback));
      }
      this.AssociatedObject.Loaded -= new RoutedEventHandler(this.AssociatedObject_Loaded);
      this.AssociatedObject.Unloaded -= new RoutedEventHandler(this.AssociatedObject_Unloaded);
      this.AssociatedObject.SourceInitialized -= new EventHandler(this.AssociatedObject_SourceInitialized);
      this.AssociatedObject.StateChanged -= new EventHandler(this.OnAssociatedObjectHandleMaximize);
      if (this.hwndSource != null)
        this.hwndSource.RemoveHook(new HwndSourceHook(this.WindowProc));
      this.windowChrome = (WindowChrome) null;
    }

    protected override void OnDetaching()
    {
      this.Cleanup();
      base.OnDetaching();
    }

    private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e) => this.Cleanup();

    private IntPtr WindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      IntPtr num = IntPtr.Zero;
      switch (msg)
      {
        case 133:
          handled = true;
          break;
        case 134:
          num = UnsafeNativeMethods.DefWindowProc(hwnd, msg, wParam, new IntPtr(-1));
          handled = true;
          break;
      }
      return num;
    }

    private void OnAssociatedObjectHandleMaximize(object sender, EventArgs e)
    {
      this.HandleMaximize();
    }

    private void HandleMaximize()
    {
      this.borderThicknessChangeNotifier.ValueChanged -= new EventHandler(this.BorderThicknessChangeNotifierOnValueChanged);
      this.topMostChangeNotifier.ValueChanged -= new EventHandler(this.TopMostChangeNotifierOnValueChanged);
      MetroWindow associatedObject = this.AssociatedObject as MetroWindow;
      bool flag1 = this.EnableDWMDropShadow;
      if (associatedObject != null)
        flag1 = associatedObject.GlowBrush == null && (associatedObject.EnableDWMDropShadow || this.EnableDWMDropShadow);
      if (this.AssociatedObject.WindowState == WindowState.Maximized)
      {
        this.windowChrome.ResizeBorderThickness = new Thickness(0.0);
        this.AssociatedObject.BorderThickness = new Thickness(0.0);
        bool flag2 = associatedObject != null && associatedObject.IgnoreTaskbarOnMaximize;
        if (flag2 && this.handle != IntPtr.Zero)
        {
          IntPtr hMonitor = UnsafeNativeMethods.MonitorFromWindow(this.handle, 2);
          if (hMonitor != IntPtr.Zero)
          {
            MahApps.Metro.Native.MONITORINFO lpmi = new MahApps.Metro.Native.MONITORINFO();
            UnsafeNativeMethods.GetMonitorInfo(hMonitor, lpmi);
            int X = flag2 ? lpmi.rcMonitor.left : lpmi.rcWork.left;
            int Y = flag2 ? lpmi.rcMonitor.top : lpmi.rcWork.top;
            int cx = flag2 ? Math.Abs(lpmi.rcMonitor.right - X) : Math.Abs(lpmi.rcWork.right - X);
            int cy = flag2 ? Math.Abs(lpmi.rcMonitor.bottom - Y) : Math.Abs(lpmi.rcWork.bottom - Y);
            UnsafeNativeMethods.SetWindowPos(this.handle, new IntPtr(-2), X, Y, cx, cy, 64U);
          }
        }
      }
      else
      {
        this.windowChrome.ResizeBorderThickness = SystemParameters.WindowResizeBorderThickness;
        if (!flag1)
          this.AssociatedObject.BorderThickness = this.savedBorderThickness.GetValueOrDefault(new Thickness(0.0));
      }
      this.AssociatedObject.Topmost = false;
      this.AssociatedObject.Topmost = this.AssociatedObject.WindowState == WindowState.Minimized || this.savedTopMost;
      this.borderThicknessChangeNotifier.ValueChanged += new EventHandler(this.BorderThicknessChangeNotifierOnValueChanged);
      this.topMostChangeNotifier.ValueChanged += new EventHandler(this.TopMostChangeNotifierOnValueChanged);
    }

    private void AssociatedObject_SourceInitialized(object sender, EventArgs e)
    {
      this.handle = new WindowInteropHelper(this.AssociatedObject).Handle;
      IntPtr handle = this.handle;
      this.hwndSource = HwndSource.FromHwnd(this.handle);
      if (this.hwndSource != null)
        this.hwndSource.AddHook(new HwndSourceHook(this.WindowProc));
      if (this.AssociatedObject.ResizeMode == ResizeMode.NoResize)
        return;
      SizeToContent sizeToContent = this.AssociatedObject.SizeToContent;
      bool snapsToDevicePixels = this.AssociatedObject.SnapsToDevicePixels;
      this.AssociatedObject.SnapsToDevicePixels = true;
      this.AssociatedObject.SizeToContent = sizeToContent == SizeToContent.WidthAndHeight ? SizeToContent.Height : SizeToContent.Manual;
      this.AssociatedObject.SizeToContent = sizeToContent;
      this.AssociatedObject.SnapsToDevicePixels = snapsToDevicePixels;
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is MetroWindow window))
        return;
      window.SetIsHitTestVisibleInChromeProperty<UIElement>("PART_Icon");
      window.SetIsHitTestVisibleInChromeProperty<UIElement>("PART_TitleBar");
      window.SetIsHitTestVisibleInChromeProperty<Thumb>("PART_WindowTitleThumb");
      window.SetIsHitTestVisibleInChromeProperty<ContentPresenter>("PART_LeftWindowCommands");
      window.SetIsHitTestVisibleInChromeProperty<ContentPresenter>("PART_RightWindowCommands");
      window.SetIsHitTestVisibleInChromeProperty<ContentControl>("PART_WindowButtonCommands");
      window.SetWindowChromeResizeGripDirection("WindowResizeGrip", ResizeGripDirection.BottomRight);
    }

    [Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" properties in your Window to get a drop shadow around it.")]
    public bool EnableDWMDropShadow
    {
      get => (bool) this.GetValue(BorderlessWindowBehavior.EnableDWMDropShadowProperty);
      set => this.SetValue(BorderlessWindowBehavior.EnableDWMDropShadowProperty, (object) value);
    }
  }
}
