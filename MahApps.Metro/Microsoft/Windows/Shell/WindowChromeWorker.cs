// Decompiled with JetBrains decompiler
// Type: Microsoft.Windows.Shell.WindowChromeWorker
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Standard;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Microsoft.Windows.Shell
{
  internal class WindowChromeWorker : DependencyObject
  {
    private static readonly Version _presentationFrameworkVersion = Assembly.GetAssembly(typeof (Window)).GetName().Version;
    private const SWP _SwpFlags = SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER;
    private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;
    private Window _window;
    [SecurityCritical]
    private IntPtr _hwnd;
    [SecurityCritical]
    private HwndSource _hwndSource;
    private bool _isHooked;
    private bool _isFixedUp;
    private bool _isUserResizing;
    private bool _hasUserMovedWindow;
    private Point _windowPosAtStartOfUserMove;
    private WindowChrome _chromeInfo;
    private WindowState _lastRoundingState;
    private WindowState _lastMenuState;
    private bool _isGlassEnabled;
    private WINDOWPOS _previousWP;
    public static readonly DependencyProperty WindowChromeWorkerProperty = DependencyProperty.RegisterAttached(nameof (WindowChromeWorker), typeof (WindowChromeWorker), typeof (WindowChromeWorker), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowChromeWorker._OnChromeWorkerChanged)));
    private static readonly HT[,] _HitTestBorders = new HT[3, 3]
    {
      {
        HT.TOPLEFT,
        HT.TOP,
        HT.TOPRIGHT
      },
      {
        HT.LEFT,
        HT.CLIENT,
        HT.RIGHT
      },
      {
        HT.BOTTOMLEFT,
        HT.BOTTOM,
        HT.BOTTOMRIGHT
      }
    };

    private static bool IsPresentationFrameworkVersionLessThan4
    {
      get => WindowChromeWorker._presentationFrameworkVersion < new Version(4, 0);
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public WindowChromeWorker()
    {
      this._messageTable = new List<KeyValuePair<WM, MessageHandler>>()
      {
        new KeyValuePair<WM, MessageHandler>(WM.NCUAHDRAWCAPTION, new MessageHandler(this._HandleNCUAHDrawCaption)),
        new KeyValuePair<WM, MessageHandler>(WM.SETTEXT, new MessageHandler(this._HandleSetTextOrIcon)),
        new KeyValuePair<WM, MessageHandler>(WM.SETICON, new MessageHandler(this._HandleSetTextOrIcon)),
        new KeyValuePair<WM, MessageHandler>(WM.SYSCOMMAND, new MessageHandler(this._HandleRestoreWindow)),
        new KeyValuePair<WM, MessageHandler>(WM.NCACTIVATE, new MessageHandler(this._HandleNCActivate)),
        new KeyValuePair<WM, MessageHandler>(WM.NCCALCSIZE, new MessageHandler(this._HandleNCCalcSize)),
        new KeyValuePair<WM, MessageHandler>(WM.NCHITTEST, new MessageHandler(this._HandleNCHitTest)),
        new KeyValuePair<WM, MessageHandler>(WM.NCRBUTTONUP, new MessageHandler(this._HandleNCRButtonUp)),
        new KeyValuePair<WM, MessageHandler>(WM.SIZE, new MessageHandler(this._HandleSize)),
        new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGING, new MessageHandler(this._HandleWindowPosChanging)),
        new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGED, new MessageHandler(this._HandleWindowPosChanged)),
        new KeyValuePair<WM, MessageHandler>(WM.GETMINMAXINFO, new MessageHandler(this._HandleGetMinMaxInfo)),
        new KeyValuePair<WM, MessageHandler>(WM.DWMCOMPOSITIONCHANGED, new MessageHandler(this._HandleDwmCompositionChanged)),
        new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMoveForAnimation)),
        new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMoveForRealSize)),
        new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMoveForAnimation))
      };
      if (!WindowChromeWorker.IsPresentationFrameworkVersionLessThan4)
        return;
      this._messageTable.AddRange((IEnumerable<KeyValuePair<WM, MessageHandler>>) new KeyValuePair<WM, MessageHandler>[4]
      {
        new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, new MessageHandler(this._HandleSettingChange)),
        new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMove)),
        new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMove)),
        new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMove))
      });
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public void SetWindowChrome(WindowChrome newChrome)
    {
      this.VerifyAccess();
      if (newChrome == this._chromeInfo)
        return;
      if (this._chromeInfo != null)
        this._chromeInfo.PropertyChangedThatRequiresRepaint -= new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
      this._chromeInfo = newChrome;
      if (this._chromeInfo != null)
        this._chromeInfo.PropertyChangedThatRequiresRepaint += new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
      this._ApplyNewCustomChrome();
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
    {
      this._UpdateFrameState(true);
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private static void _OnChromeWorkerChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Window window = (Window) d;
      ((WindowChromeWorker) e.NewValue)._SetWindow(window);
    }

    [SecurityCritical]
    private void _SetWindow(Window window)
    {
      this.UnsubscribeWindowEvents();
      this._window = window;
      this._hwnd = new WindowInteropHelper(this._window).Handle;
      Utility.AddDependencyPropertyChangeListener((object) this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
      Utility.AddDependencyPropertyChangeListener((object) this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
      this._window.Closed += new EventHandler(this._UnsetWindow);
      if (IntPtr.Zero != this._hwnd)
      {
        this._hwndSource = HwndSource.FromHwnd(this._hwnd);
        this._window.ApplyTemplate();
        if (this._chromeInfo == null)
          return;
        this._ApplyNewCustomChrome();
      }
      else
        this._window.SourceInitialized += new EventHandler(this._WindowSourceInitialized);
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private void _WindowSourceInitialized(object sender, EventArgs e)
    {
      this._hwnd = new WindowInteropHelper(this._window).Handle;
      this._hwndSource = HwndSource.FromHwnd(this._hwnd);
      if (this._chromeInfo == null)
        return;
      this._ApplyNewCustomChrome();
    }

    [SecurityCritical]
    private void UnsubscribeWindowEvents()
    {
      if (this._window == null)
        return;
      Utility.RemoveDependencyPropertyChangeListener((object) this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
      Utility.RemoveDependencyPropertyChangeListener((object) this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
      this._window.SourceInitialized -= new EventHandler(this._WindowSourceInitialized);
      this._window.StateChanged -= new EventHandler(this._FixupRestoreBounds);
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private void _UnsetWindow(object sender, EventArgs e)
    {
      this.UnsubscribeWindowEvents();
      if (this._chromeInfo != null)
        this._chromeInfo.PropertyChangedThatRequiresRepaint -= new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
      this._RestoreStandardChromeState(true);
    }

    public static WindowChromeWorker GetWindowChromeWorker(Window window)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      return (WindowChromeWorker) window.GetValue(WindowChromeWorker.WindowChromeWorkerProperty);
    }

    public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      window.SetValue(WindowChromeWorker.WindowChromeWorkerProperty, (object) chrome);
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
    {
      if (this._chromeInfo == null || !(this._hwnd != IntPtr.Zero))
        return;
      this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new WindowChromeWorker._Action(this._FixupTemplateIssues));
    }

    [SecurityCritical]
    private void _ApplyNewCustomChrome()
    {
      if (this._hwnd == IntPtr.Zero || this._hwndSource.IsDisposed)
        return;
      if (this._chromeInfo == null)
      {
        this._RestoreStandardChromeState(false);
      }
      else
      {
        if (!this._isHooked)
        {
          this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
          this._isHooked = true;
        }
        this._ModifyStyle(WS.OVERLAPPED, WS.CAPTION);
        this._FixupTemplateIssues();
        this._UpdateSystemMenu(new WindowState?(this._window.WindowState));
        this._UpdateFrameState(true);
        Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
      }
    }

    [SecurityCritical]
    private void _FixupTemplateIssues()
    {
      if (this._window.Template == null)
        return;
      if (VisualTreeHelper.GetChildrenCount((DependencyObject) this._window) == 0)
      {
        this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new WindowChromeWorker._Action(this._FixupTemplateIssues));
      }
      else
      {
        Thickness thickness1 = new Thickness();
        FrameworkElement child = (FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) this._window, 0);
        if (this._chromeInfo.SacrificialEdge != SacrificialEdge.None)
        {
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 2))
            thickness1.Top -= SystemParameters.WindowResizeBorderThickness.Top;
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 1))
            thickness1.Left -= SystemParameters.WindowResizeBorderThickness.Left;
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 8))
            thickness1.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 4))
            thickness1.Right -= SystemParameters.WindowResizeBorderThickness.Right;
        }
        if (WindowChromeWorker.IsPresentationFrameworkVersionLessThan4)
        {
          RECT windowRect = Standard.NativeMethods.GetWindowRect(this._hwnd);
          RECT adjustedWindowRect = this._GetAdjustedWindowRect(windowRect);
          Rect logical1 = DpiHelper.DeviceRectToLogical(new Rect((double) windowRect.Left, (double) windowRect.Top, (double) windowRect.Width, (double) windowRect.Height));
          Rect logical2 = DpiHelper.DeviceRectToLogical(new Rect((double) adjustedWindowRect.Left, (double) adjustedWindowRect.Top, (double) adjustedWindowRect.Width, (double) adjustedWindowRect.Height));
          if (!Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 1))
            thickness1.Right -= SystemParameters.WindowResizeBorderThickness.Left;
          if (!Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 4))
            thickness1.Right -= SystemParameters.WindowResizeBorderThickness.Right;
          if (!Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 2))
            thickness1.Bottom -= SystemParameters.WindowResizeBorderThickness.Top;
          if (!Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 8))
            thickness1.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
          thickness1.Bottom -= SystemParameters.WindowCaptionHeight;
          Transform transform;
          if (this._window.FlowDirection == FlowDirection.RightToLeft)
          {
            Thickness thickness2 = new Thickness(logical1.Left - logical2.Left, logical1.Top - logical2.Top, logical2.Right - logical1.Right, logical2.Bottom - logical1.Bottom);
            transform = (Transform) new MatrixTransform(1.0, 0.0, 0.0, 1.0, -(thickness2.Left + thickness2.Right), 0.0);
          }
          else
            transform = (Transform) null;
          child.RenderTransform = transform;
        }
        child.Margin = thickness1;
        if (!WindowChromeWorker.IsPresentationFrameworkVersionLessThan4 || this._isFixedUp)
          return;
        this._hasUserMovedWindow = false;
        this._window.StateChanged += new EventHandler(this._FixupRestoreBounds);
        this._isFixedUp = true;
      }
    }

    [SecuritySafeCritical]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private void _FixupRestoreBounds(object sender, EventArgs e)
    {
      if (this._window.WindowState != WindowState.Maximized && this._window.WindowState != WindowState.Minimized || !this._hasUserMovedWindow)
        return;
      this._hasUserMovedWindow = false;
      WINDOWPLACEMENT windowPlacement = Standard.NativeMethods.GetWindowPlacement(this._hwnd);
      RECT adjustedWindowRect = this._GetAdjustedWindowRect(new RECT()
      {
        Bottom = 100,
        Right = 100
      });
      Point logical = DpiHelper.DevicePixelsToLogical(new Point((double) (windowPlacement.rcNormalPosition.Left - adjustedWindowRect.Left), (double) (windowPlacement.rcNormalPosition.Top - adjustedWindowRect.Top)));
      this._window.Top = logical.Y;
      this._window.Left = logical.X;
    }

    [SecurityCritical]
    private RECT _GetAdjustedWindowRect(RECT rcWindow)
    {
      WS windowLongPtr1 = (WS) (int) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE);
      WS_EX windowLongPtr2 = (WS_EX) (int) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.EXSTYLE);
      return Standard.NativeMethods.AdjustWindowRectEx(rcWindow, windowLongPtr1, false, windowLongPtr2);
    }

    private bool _IsWindowDocked
    {
      [SecurityCritical] get
      {
        if (this._window.WindowState != WindowState.Normal)
          return false;
        RECT adjustedWindowRect = this._GetAdjustedWindowRect(new RECT()
        {
          Bottom = 100,
          Right = 100
        });
        return this._window.RestoreBounds.Location != new Point(this._window.Left, this._window.Top) - (Vector) DpiHelper.DevicePixelsToLogical(new Point((double) adjustedWindowRect.Left, (double) adjustedWindowRect.Top));
      }
    }

    private bool _MinimizeAnimation
    {
      get => SystemParameters.MinimizeAnimation && !this._chromeInfo.IgnoreTaskbarOnMaximize;
    }

    [SecurityCritical]
    private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      WM uMsg = (WM) msg;
      foreach (KeyValuePair<WM, MessageHandler> keyValuePair in this._messageTable)
      {
        if (keyValuePair.Key == uMsg)
          return keyValuePair.Value(uMsg, wParam, lParam, out handled);
      }
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleNCUAHDrawCaption(
      WM uMsg,
      IntPtr wParam,
      IntPtr lParam,
      out bool handled)
    {
      if (!this._window.ShowInTaskbar && this._GetHwndState() == WindowState.Minimized)
      {
        bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
        IntPtr num = Standard.NativeMethods.DefWindowProc(this._hwnd, uMsg, wParam, lParam);
        if (flag)
          this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
        handled = true;
        return num;
      }
      handled = false;
      return IntPtr.Zero;
    }

    private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
      IntPtr num = Standard.NativeMethods.DefWindowProc(this._hwnd, uMsg, wParam, lParam);
      if (flag)
        this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
      handled = true;
      return num;
    }

    [SecurityCritical]
    private IntPtr _HandleRestoreWindow(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      WINDOWPLACEMENT windowPlacement = Standard.NativeMethods.GetWindowPlacement(this._hwnd);
      if (61728 == (Environment.Is64BitProcess ? (int) wParam.ToInt64() : wParam.ToInt32()) && windowPlacement.showCmd == SW.SHOWMAXIMIZED && this._MinimizeAnimation)
      {
        bool flag = this._ModifyStyle(WS.SYSMENU, WS.OVERLAPPED);
        IntPtr num = Standard.NativeMethods.DefWindowProc(this._hwnd, uMsg, wParam, lParam);
        if (flag)
          this._ModifyStyle(WS.OVERLAPPED, WS.SYSMENU);
        handled = true;
        return num;
      }
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      IntPtr num = Standard.NativeMethods.DefWindowProc(this._hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
      handled = true;
      return num;
    }

    [SecurityCritical]
    private static RECT AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, RECT area)
    {
      IntPtr window = Standard.NativeMethods.FindWindow("Shell_TrayWnd", (string) null);
      IntPtr objB = Standard.NativeMethods.MonitorFromWindow(window, 2U);
      APPBARDATA pData = new APPBARDATA();
      pData.cbSize = Marshal.SizeOf((object) pData);
      pData.hWnd = window;
      int num = (int) Standard.NativeMethods.SHAppBarMessage(5, ref pData);
      if (!Convert.ToBoolean(Standard.NativeMethods.SHAppBarMessage(4, ref pData)) || !object.Equals((object) monitorContainingApplication, (object) objB))
        return area;
      switch (pData.uEdge)
      {
        case 0:
          area.Left += 2;
          break;
        case 1:
          area.Top += 2;
          break;
        case 2:
          area.Right -= 2;
          break;
        case 3:
          area.Bottom -= 2;
          break;
        default:
          return area;
      }
      return area;
    }

    [SecurityCritical]
    private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      if (Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd == SW.SHOWMAXIMIZED && this._MinimizeAnimation)
      {
        IntPtr num = Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2U);
        MONITORINFO monitorInfo = Standard.NativeMethods.GetMonitorInfo(num);
        RECT structure = (RECT) Marshal.PtrToStructure(lParam, typeof (RECT));
        Standard.NativeMethods.DefWindowProc(this._hwnd, WM.NCCALCSIZE, wParam, lParam);
        RECT rect = (RECT) Marshal.PtrToStructure(lParam, typeof (RECT)) with
        {
          Top = (int) ((long) structure.Top + (long) Standard.NativeMethods.GetWindowInfo(this._hwnd).cyWindowBorders)
        };
        if (monitorInfo.rcMonitor.Height == monitorInfo.rcWork.Height && monitorInfo.rcMonitor.Width == monitorInfo.rcWork.Width)
          rect = WindowChromeWorker.AdjustWorkingAreaForAutoHide(num, rect);
        Marshal.StructureToPtr((object) rect, lParam, true);
      }
      if (this._chromeInfo.SacrificialEdge != SacrificialEdge.None)
      {
        Thickness device = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness);
        RECT structure = (RECT) Marshal.PtrToStructure(lParam, typeof (RECT));
        if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 2))
          structure.Top += (int) device.Top;
        if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 1))
          structure.Left += (int) device.Left;
        if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 8))
          structure.Bottom -= (int) device.Bottom;
        if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 4))
          structure.Right -= (int) device.Right;
        Marshal.StructureToPtr((object) structure, lParam, false);
      }
      handled = true;
      return new IntPtr(1792);
    }

    private HT _GetHTFromResizeGripDirection(ResizeGripDirection direction)
    {
      bool flag = this._window.FlowDirection == FlowDirection.RightToLeft;
      switch (direction)
      {
        case ResizeGripDirection.TopLeft:
          return !flag ? HT.TOPLEFT : HT.TOPRIGHT;
        case ResizeGripDirection.Top:
          return HT.TOP;
        case ResizeGripDirection.TopRight:
          return !flag ? HT.TOPRIGHT : HT.TOPLEFT;
        case ResizeGripDirection.Right:
          return !flag ? HT.RIGHT : HT.LEFT;
        case ResizeGripDirection.BottomRight:
          return !flag ? HT.BOTTOMRIGHT : HT.BOTTOMLEFT;
        case ResizeGripDirection.Bottom:
          return HT.BOTTOM;
        case ResizeGripDirection.BottomLeft:
          return !flag ? HT.BOTTOMLEFT : HT.BOTTOMRIGHT;
        case ResizeGripDirection.Left:
          return !flag ? HT.LEFT : HT.RIGHT;
        case ResizeGripDirection.Caption:
          return HT.CAPTION;
        default:
          return HT.NOWHERE;
      }
    }

    [SecurityCritical]
    private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      Point devicePoint1 = new Point((double) Utility.GET_X_LPARAM(lParam), (double) Utility.GET_Y_LPARAM(lParam));
      Rect windowRect = this._GetWindowRect();
      Point devicePoint2 = devicePoint1;
      devicePoint2.Offset(-windowRect.X, -windowRect.Y);
      IInputElement inputElement = this._window.InputHitTest(DpiHelper.DevicePixelsToLogical(devicePoint2));
      if (inputElement != null)
      {
        if (WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
        {
          handled = true;
          return new IntPtr(1);
        }
        ResizeGripDirection resizeGripDirection = WindowChrome.GetResizeGripDirection(inputElement);
        if (resizeGripDirection != ResizeGripDirection.None)
        {
          handled = true;
          return new IntPtr((int) this._GetHTFromResizeGripDirection(resizeGripDirection));
        }
      }
      if (this._chromeInfo.UseAeroCaptionButtons && Utility.IsOSVistaOrNewer && this._chromeInfo.GlassFrameThickness != new Thickness() && this._isGlassEnabled)
      {
        IntPtr plResult;
        handled = Standard.NativeMethods.DwmDefWindowProc(this._hwnd, uMsg, wParam, lParam, out plResult);
        if (IntPtr.Zero != plResult)
          return plResult;
      }
      HT ht = this._HitTestNca(DpiHelper.DeviceRectToLogical(windowRect), DpiHelper.DevicePixelsToLogical(devicePoint1));
      handled = true;
      return new IntPtr((int) ht);
    }

    [SecurityCritical]
    private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      if (2 == (Environment.Is64BitProcess ? (int) wParam.ToInt64() : wParam.ToInt32()))
        SystemCommands.ShowSystemMenuPhysicalCoordinates(this._window, new Point((double) Utility.GET_X_LPARAM(lParam), (double) Utility.GET_Y_LPARAM(lParam)));
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      WindowState? assumeState = new WindowState?();
      if ((Environment.Is64BitProcess ? wParam.ToInt64() : (long) wParam.ToInt32()) == 2L)
        assumeState = new WindowState?(WindowState.Maximized);
      this._UpdateSystemMenu(assumeState);
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleWindowPosChanging(
      WM uMsg,
      IntPtr wParam,
      IntPtr lParam,
      out bool handled)
    {
      if (!this._isGlassEnabled)
      {
        WINDOWPOS structure = (WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (WINDOWPOS));
        if (this._chromeInfo.IgnoreTaskbarOnMaximize && this._GetHwndState() == WindowState.Maximized && structure.flags == 32)
        {
          structure.flags = 0;
          Marshal.StructureToPtr((object) structure, lParam, true);
        }
      }
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleWindowPosChanged(
      WM uMsg,
      IntPtr wParam,
      IntPtr lParam,
      out bool handled)
    {
      this._UpdateSystemMenu(new WindowState?());
      if (!this._isGlassEnabled)
      {
        WINDOWPOS structure = (WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (WINDOWPOS));
        if (!structure.Equals((object) this._previousWP))
        {
          this._previousWP = structure;
          this._SetRoundingRegion(new WINDOWPOS?(structure));
        }
        this._previousWP = structure;
      }
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleGetMinMaxInfo(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      if (this._chromeInfo.IgnoreTaskbarOnMaximize && Standard.NativeMethods.IsZoomed(this._hwnd))
      {
        MINMAXINFO structure = (MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (MINMAXINFO));
        IntPtr hMonitor = Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2U);
        if (hMonitor != IntPtr.Zero)
        {
          MONITORINFO monitorInfoW = Standard.NativeMethods.GetMonitorInfoW(hMonitor);
          RECT rcWork = monitorInfoW.rcWork;
          RECT rcMonitor = monitorInfoW.rcMonitor;
          structure.ptMaxPosition.x = Math.Abs(rcWork.Left - rcMonitor.Left);
          structure.ptMaxPosition.y = Math.Abs(rcWork.Top - rcMonitor.Top);
          structure.ptMaxSize.x = Math.Abs(monitorInfoW.rcMonitor.Width);
          structure.ptMaxSize.y = Math.Abs(monitorInfoW.rcMonitor.Height);
          structure.ptMaxTrackSize.x = structure.ptMaxSize.x;
          structure.ptMaxTrackSize.y = structure.ptMaxSize.y;
        }
        Marshal.StructureToPtr((object) structure, lParam, true);
      }
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleDwmCompositionChanged(
      WM uMsg,
      IntPtr wParam,
      IntPtr lParam,
      out bool handled)
    {
      this._UpdateFrameState(false);
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      this._FixupTemplateIssues();
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      this._isUserResizing = true;
      if (this._window.WindowState != WindowState.Maximized && !this._IsWindowDocked)
        this._windowPosAtStartOfUserMove = new Point(this._window.Left, this._window.Top);
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleEnterSizeMoveForAnimation(
      WM uMsg,
      IntPtr wParam,
      IntPtr lParam,
      out bool handled)
    {
      if (this._MinimizeAnimation && this._GetHwndState() == WindowState.Maximized)
        this._ModifyStyle(WS.DLGFRAME, WS.OVERLAPPED);
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleMoveForRealSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      if (this._GetHwndState() == WindowState.Maximized)
      {
        IntPtr hMonitor = Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2U);
        if (hMonitor != IntPtr.Zero)
        {
          bool taskbarOnMaximize = this._chromeInfo.IgnoreTaskbarOnMaximize;
          MONITORINFO monitorInfoW = Standard.NativeMethods.GetMonitorInfoW(hMonitor);
          RECT rect = taskbarOnMaximize ? monitorInfoW.rcMonitor : monitorInfoW.rcWork;
          Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, rect.Left, rect.Top, rect.Width, rect.Height, SWP.ASYNCWINDOWPOS | SWP.DRAWFRAME | SWP.NOCOPYBITS);
        }
      }
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private IntPtr _HandleExitSizeMoveForAnimation(
      WM uMsg,
      IntPtr wParam,
      IntPtr lParam,
      out bool handled)
    {
      if (this._MinimizeAnimation && this._ModifyStyle(WS.OVERLAPPED, WS.DLGFRAME))
        this._UpdateFrameState(true);
      handled = false;
      return IntPtr.Zero;
    }

    private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      this._isUserResizing = false;
      if (this._window.WindowState == WindowState.Maximized)
      {
        this._window.Top = this._windowPosAtStartOfUserMove.Y;
        this._window.Left = this._windowPosAtStartOfUserMove.X;
      }
      handled = false;
      return IntPtr.Zero;
    }

    private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
    {
      if (this._isUserResizing)
        this._hasUserMovedWindow = true;
      handled = false;
      return IntPtr.Zero;
    }

    [SecurityCritical]
    private bool _ModifyStyle(WS removeStyle, WS addStyle)
    {
      IntPtr windowLongPtr = Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE);
      WS ws1 = Environment.Is64BitProcess ? (WS) windowLongPtr.ToInt64() : (WS) windowLongPtr.ToInt32();
      WS ws2 = ws1 & ~removeStyle | addStyle;
      if (ws1 == ws2)
        return false;
      Standard.NativeMethods.SetWindowLongPtr(this._hwnd, GWL.STYLE, new IntPtr((int) ws2));
      return true;
    }

    [SecurityCritical]
    private WindowState _GetHwndState()
    {
      switch (Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd)
      {
        case SW.SHOWMINIMIZED:
          return WindowState.Minimized;
        case SW.SHOWMAXIMIZED:
          return WindowState.Maximized;
        default:
          return WindowState.Normal;
      }
    }

    [SecurityCritical]
    private Rect _GetWindowRect()
    {
      RECT windowRect = Standard.NativeMethods.GetWindowRect(this._hwnd);
      return new Rect((double) windowRect.Left, (double) windowRect.Top, (double) windowRect.Width, (double) windowRect.Height);
    }

    [SecurityCritical]
    private void _UpdateSystemMenu(WindowState? assumeState)
    {
      WindowState windowState = (WindowState) ((int) assumeState ?? (int) this._GetHwndState());
      if (!assumeState.HasValue && this._lastMenuState == windowState)
        return;
      this._lastMenuState = windowState;
      IntPtr systemMenu = Standard.NativeMethods.GetSystemMenu(this._hwnd, false);
      if (!(IntPtr.Zero != systemMenu))
        return;
      IntPtr windowLongPtr = Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE);
      WS ws = Environment.Is64BitProcess ? (WS) windowLongPtr.ToInt64() : (WS) windowLongPtr.ToInt32();
      bool flag1 = Utility.IsFlagSet((int) ws, 131072);
      bool flag2 = Utility.IsFlagSet((int) ws, 65536);
      bool flag3 = Utility.IsFlagSet((int) ws, 262144);
      switch (windowState)
      {
        case WindowState.Minimized:
          int num1 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
          int num2 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
          int num3 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
          int num4 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.GRAYED | MF.DISABLED);
          int num5 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag2 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          break;
        case WindowState.Maximized:
          int num6 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
          int num7 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
          int num8 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
          int num9 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag1 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          int num10 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.GRAYED | MF.DISABLED);
          break;
        default:
          int num11 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.GRAYED | MF.DISABLED);
          int num12 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.ENABLED);
          int num13 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, flag3 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          int num14 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag1 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          int num15 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag2 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          break;
      }
    }

    [SecurityCritical]
    private void _UpdateFrameState(bool force)
    {
      if (IntPtr.Zero == this._hwnd || this._hwndSource.IsDisposed)
        return;
      bool flag = Standard.NativeMethods.DwmIsCompositionEnabled();
      if (!force && flag == this._isGlassEnabled)
        return;
      this._isGlassEnabled = flag && this._chromeInfo.GlassFrameThickness != new Thickness();
      if (!this._isGlassEnabled)
      {
        this._SetRoundingRegion(new WINDOWPOS?());
      }
      else
      {
        this._ClearRoundingRegion();
        this._ExtendGlassFrame();
      }
      if (this._MinimizeAnimation)
        this._ModifyStyle(WS.OVERLAPPED, WS.CAPTION);
      else
        this._ModifyStyle(WS.CAPTION, WS.OVERLAPPED);
      Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
    }

    [SecurityCritical]
    private void _ClearRoundingRegion()
    {
      Standard.NativeMethods.SetWindowRgn(this._hwnd, IntPtr.Zero, Standard.NativeMethods.IsWindowVisible(this._hwnd));
    }

    [SecurityCritical]
    private RECT _GetClientRectRelativeToWindowRect(IntPtr hWnd)
    {
      RECT windowRect = Standard.NativeMethods.GetWindowRect(hWnd);
      RECT clientRect = Standard.NativeMethods.GetClientRect(hWnd);
      POINT point = new POINT() { x = 0, y = 0 };
      Standard.NativeMethods.ClientToScreen(hWnd, ref point);
      if (this._window.FlowDirection == FlowDirection.RightToLeft)
        clientRect.Offset(windowRect.Right - point.x, point.y - windowRect.Top);
      else
        clientRect.Offset(point.x - windowRect.Left, point.y - windowRect.Top);
      return clientRect;
    }

    [SecurityCritical]
    private void _SetRoundingRegion(WINDOWPOS? wp)
    {
      if (Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd == SW.SHOWMAXIMIZED)
      {
        RECT lprc;
        if (this._MinimizeAnimation)
        {
          lprc = this._GetClientRectRelativeToWindowRect(this._hwnd);
        }
        else
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
            Rect windowRect = this._GetWindowRect();
            num1 = (int) windowRect.Left;
            num2 = (int) windowRect.Top;
          }
          MONITORINFO monitorInfo = Standard.NativeMethods.GetMonitorInfo(Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2U));
          lprc = this._chromeInfo.IgnoreTaskbarOnMaximize ? monitorInfo.rcMonitor : monitorInfo.rcWork;
          lprc.Offset(-num1, -num2);
        }
        IntPtr gdiObject = IntPtr.Zero;
        try
        {
          gdiObject = Standard.NativeMethods.CreateRectRgnIndirect(lprc);
          Standard.NativeMethods.SetWindowRgn(this._hwnd, gdiObject, Standard.NativeMethods.IsWindowVisible(this._hwnd));
          gdiObject = IntPtr.Zero;
        }
        finally
        {
          Utility.SafeDeleteObject(ref gdiObject);
        }
      }
      else
      {
        Size size;
        if (wp.HasValue && !Utility.IsFlagSet(wp.Value.flags, 1))
        {
          size = new Size((double) wp.Value.cx, (double) wp.Value.cy);
        }
        else
        {
          if (wp.HasValue && this._lastRoundingState == this._window.WindowState)
            return;
          size = this._GetWindowRect().Size;
        }
        this._lastRoundingState = this._window.WindowState;
        IntPtr gdiObject = IntPtr.Zero;
        try
        {
          double num = Math.Min(size.Width, size.Height);
          CornerRadius cornerRadius = this._chromeInfo.CornerRadius;
          Point device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.TopLeft, 0.0));
          double radius1 = Math.Min(device.X, num / 2.0);
          if (WindowChromeWorker._IsUniform(this._chromeInfo.CornerRadius))
          {
            gdiObject = WindowChromeWorker._CreateRoundRectRgn(new Rect(size), radius1);
          }
          else
          {
            gdiObject = WindowChromeWorker._CreateRoundRectRgn(new Rect(0.0, 0.0, size.Width / 2.0 + radius1, size.Height / 2.0 + radius1), radius1);
            cornerRadius = this._chromeInfo.CornerRadius;
            device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.TopRight, 0.0));
            double radius2 = Math.Min(device.X, num / 2.0);
            Rect region1 = new Rect(0.0, 0.0, size.Width / 2.0 + radius2, size.Height / 2.0 + radius2);
            region1.Offset(size.Width / 2.0 - radius2, 0.0);
            WindowChromeWorker._CreateAndCombineRoundRectRgn(gdiObject, region1, radius2);
            cornerRadius = this._chromeInfo.CornerRadius;
            device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.BottomLeft, 0.0));
            double radius3 = Math.Min(device.X, num / 2.0);
            Rect region2 = new Rect(0.0, 0.0, size.Width / 2.0 + radius3, size.Height / 2.0 + radius3);
            region2.Offset(0.0, size.Height / 2.0 - radius3);
            WindowChromeWorker._CreateAndCombineRoundRectRgn(gdiObject, region2, radius3);
            cornerRadius = this._chromeInfo.CornerRadius;
            device = DpiHelper.LogicalPixelsToDevice(new Point(cornerRadius.BottomRight, 0.0));
            double radius4 = Math.Min(device.X, num / 2.0);
            Rect region3 = new Rect(0.0, 0.0, size.Width / 2.0 + radius4, size.Height / 2.0 + radius4);
            region3.Offset(size.Width / 2.0 - radius4, size.Height / 2.0 - radius4);
            WindowChromeWorker._CreateAndCombineRoundRectRgn(gdiObject, region3, radius4);
          }
          Standard.NativeMethods.SetWindowRgn(this._hwnd, gdiObject, Standard.NativeMethods.IsWindowVisible(this._hwnd));
          gdiObject = IntPtr.Zero;
        }
        finally
        {
          Utility.SafeDeleteObject(ref gdiObject);
        }
      }
    }

    [SecurityCritical]
    private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
    {
      return DoubleUtilities.AreClose(0.0, radius) ? Standard.NativeMethods.CreateRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top), (int) Math.Ceiling(region.Right), (int) Math.Ceiling(region.Bottom)) : Standard.NativeMethods.CreateRoundRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top), (int) Math.Ceiling(region.Right) + 1, (int) Math.Ceiling(region.Bottom) + 1, (int) Math.Ceiling(radius), (int) Math.Ceiling(radius));
    }

    [SecurityCritical]
    private static void _CreateAndCombineRoundRectRgn(
      IntPtr hrgnSource,
      Rect region,
      double radius)
    {
      IntPtr gdiObject = IntPtr.Zero;
      try
      {
        gdiObject = WindowChromeWorker._CreateRoundRectRgn(region, radius);
        if (Standard.NativeMethods.CombineRgn(hrgnSource, hrgnSource, gdiObject, RGN.OR) == CombineRgnResult.ERROR)
          throw new InvalidOperationException("Unable to combine two HRGNs.");
      }
      catch
      {
        Utility.SafeDeleteObject(ref gdiObject);
        throw;
      }
    }

    private static bool _IsUniform(CornerRadius cornerRadius)
    {
      return DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight) && DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight) && DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight);
    }

    [SecurityCritical]
    private void _ExtendGlassFrame()
    {
      if (!Utility.IsOSVistaOrNewer || IntPtr.Zero == this._hwnd)
        return;
      if (!Standard.NativeMethods.DwmIsCompositionEnabled())
      {
        if (this._window.AllowsTransparency)
          this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
        else
          this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
      }
      else
      {
        if (this._window.AllowsTransparency)
          this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
        Thickness device1 = DpiHelper.LogicalThicknessToDevice(this._chromeInfo.GlassFrameThickness);
        if (this._chromeInfo.SacrificialEdge != SacrificialEdge.None)
        {
          Thickness device2 = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness);
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 2))
          {
            device1.Top -= device2.Top;
            device1.Top = Math.Max(0.0, device1.Top);
          }
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 1))
          {
            device1.Left -= device2.Left;
            device1.Left = Math.Max(0.0, device1.Left);
          }
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 8))
          {
            device1.Bottom -= device2.Bottom;
            device1.Bottom = Math.Max(0.0, device1.Bottom);
          }
          if (Utility.IsFlagSet((int) this._chromeInfo.SacrificialEdge, 4))
          {
            device1.Right -= device2.Right;
            device1.Right = Math.Max(0.0, device1.Right);
          }
        }
        MARGINS pMarInset = new MARGINS()
        {
          cxLeftWidth = (int) Math.Ceiling(device1.Left),
          cxRightWidth = (int) Math.Ceiling(device1.Right),
          cyTopHeight = (int) Math.Ceiling(device1.Top),
          cyBottomHeight = (int) Math.Ceiling(device1.Bottom)
        };
        Standard.NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref pMarInset);
      }
    }

    private HT _HitTestNca(Rect windowPosition, Point mousePosition)
    {
      int index1 = 1;
      int index2 = 1;
      bool flag = false;
      if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top + this._chromeInfo.CaptionHeight)
      {
        flag = mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top;
        index1 = 0;
      }
      else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double) (int) this._chromeInfo.ResizeBorderThickness.Bottom)
        index1 = 2;
      if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double) (int) this._chromeInfo.ResizeBorderThickness.Left)
        index2 = 0;
      else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - this._chromeInfo.ResizeBorderThickness.Right)
        index2 = 2;
      if (index1 == 0 && index2 != 1 && !flag)
        index1 = 1;
      HT ht = WindowChromeWorker._HitTestBorders[index1, index2];
      if (ht == HT.TOP && !flag)
        ht = HT.CAPTION;
      return ht;
    }

    [SecurityCritical]
    private void _RestoreStandardChromeState(bool isClosing)
    {
      this.VerifyAccess();
      this._UnhookCustomChrome();
      if (isClosing || this._hwndSource.IsDisposed)
        return;
      this._RestoreFrameworkIssueFixups();
      this._RestoreGlassFrame();
      this._RestoreHrgn();
      this._window.InvalidateMeasure();
    }

    [SecurityCritical]
    private void _UnhookCustomChrome()
    {
      if (!this._isHooked)
        return;
      this._hwndSource.RemoveHook(new HwndSourceHook(this._WndProc));
      this._isHooked = false;
    }

    [SecurityCritical]
    private void _RestoreFrameworkIssueFixups()
    {
      ((FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) this._window, 0)).Margin = new Thickness();
      if (!WindowChromeWorker.IsPresentationFrameworkVersionLessThan4)
        return;
      this._window.StateChanged -= new EventHandler(this._FixupRestoreBounds);
      this._isFixedUp = false;
    }

    [SecurityCritical]
    private void _RestoreGlassFrame()
    {
      if (!Utility.IsOSVistaOrNewer || this._hwnd == IntPtr.Zero)
        return;
      this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
      if (!Standard.NativeMethods.DwmIsCompositionEnabled())
        return;
      MARGINS pMarInset = new MARGINS();
      Standard.NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref pMarInset);
    }

    [SecurityCritical]
    private void _RestoreHrgn()
    {
      this._ClearRoundingRegion();
      Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
    }

    private delegate void _Action();
  }
}
