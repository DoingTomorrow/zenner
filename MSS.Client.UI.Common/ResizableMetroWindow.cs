// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Common.ResizableMetroWindow
// Assembly: MSS.Client.UI.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15ED3F62-7ABB-4067-AE48-CE636F8F9754
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Common.dll

using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

#nullable disable
namespace MSS.Client.UI.Common
{
  public class ResizableMetroWindow : MetroWindow
  {
    private const int MONITOR_DEFAULTTONEAREST = 2;
    private bool restoreWindow = false;
    private static Dictionary<Window, WindowState> windowsWithWindowStates;

    private static int GetEdge(ResizableMetroWindow.RECT rc)
    {
      return rc.top != rc.left || rc.bottom <= rc.right ? (rc.top != rc.left || rc.bottom >= rc.right ? (rc.top <= rc.left ? 2 : 3) : 1) : 0;
    }

    public void Drag_Window(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.DragMove();
    }

    public void Minimize_Window(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.WindowState = WindowState.Minimized;
    }

    public void Maximize_Window(object sender, MouseButtonEventArgs e)
    {
      if (this.WindowState == WindowState.Normal)
        this.WindowState = WindowState.Maximized;
      else
        this.WindowState = WindowState.Normal;
    }

    public void Close_Window(object sender, MouseButtonEventArgs e) => this.Close();

    private static ResizableMetroWindow.MINMAXINFO AdjustWorkingAreaForAutoHide(
      IntPtr monitorContainingApplication,
      ResizableMetroWindow.MINMAXINFO mmi)
    {
      IntPtr window = ResizableMetroWindow.FindWindow("Shell_TrayWnd", (string) null);
      IntPtr num = ResizableMetroWindow.MonitorFromWindow(window, 2);
      if (!monitorContainingApplication.Equals((object) num))
        return mmi;
      ResizableMetroWindow.APPBARDATA pData = new ResizableMetroWindow.APPBARDATA();
      pData.cbSize = Marshal.SizeOf<ResizableMetroWindow.APPBARDATA>(pData);
      pData.hWnd = window;
      ResizableMetroWindow.SHAppBarMessage(5, ref pData);
      int edge = ResizableMetroWindow.GetEdge(pData.rc);
      if (!Convert.ToBoolean(ResizableMetroWindow.SHAppBarMessage(4, ref pData)))
        return mmi;
      switch (edge)
      {
        case 0:
          mmi.ptMaxPosition.x += 13;
          mmi.ptMaxTrackSize.x -= 13;
          mmi.ptMaxSize.x -= 13;
          break;
        case 1:
          mmi.ptMaxPosition.y += 13;
          mmi.ptMaxTrackSize.y -= 13;
          mmi.ptMaxSize.y -= 13;
          break;
        case 2:
          mmi.ptMaxSize.x -= 13;
          mmi.ptMaxTrackSize.x -= 13;
          break;
        case 3:
          mmi.ptMaxSize.y -= 13;
          mmi.ptMaxTrackSize.y -= 13;
          break;
        default:
          return mmi;
      }
      return mmi;
    }

    private IntPtr WindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      if (msg == 36)
      {
        ResizableMetroWindow.WmGetMinMaxInfo(hwnd, lParam);
        handled = true;
      }
      return (IntPtr) 0;
    }

    protected void win_SourceInitialized(object sender, EventArgs e)
    {
      HwndSource.FromHwnd(new WindowInteropHelper((Window) this).Handle).AddHook(new HwndSourceHook(this.WindowProc));
    }

    private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
    {
      ResizableMetroWindow.MINMAXINFO minmaxinfo = (ResizableMetroWindow.MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (ResizableMetroWindow.MINMAXINFO));
      int flags = 2;
      IntPtr num = ResizableMetroWindow.MonitorFromWindow(hwnd, flags);
      if (num != IntPtr.Zero)
      {
        ResizableMetroWindow.MONITORINFO lpmi = new ResizableMetroWindow.MONITORINFO();
        ResizableMetroWindow.GetMonitorInfo(num, lpmi);
        ResizableMetroWindow.RECT rcWork = lpmi.rcWork;
        ResizableMetroWindow.RECT rcMonitor = lpmi.rcMonitor;
        minmaxinfo.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left);
        minmaxinfo.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
        minmaxinfo.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
        minmaxinfo.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top);
        minmaxinfo = ResizableMetroWindow.AdjustWorkingAreaForAutoHide(num, minmaxinfo);
      }
      Marshal.StructureToPtr<ResizableMetroWindow.MINMAXINFO>(minmaxinfo, lParam, true);
    }

    [DllImport("user32")]
    internal static extern bool GetMonitorInfo(
      IntPtr hMonitor,
      ResizableMetroWindow.MONITORINFO lpmi);

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(ref Point lpPoint);

    [DllImport("User32")]
    internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

    [DllImport("user32", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
    public static extern int SHAppBarMessage(
      int dwMessage,
      ref ResizableMetroWindow.APPBARDATA pData);

    private void RestoreMinimizedWindow()
    {
      if (this.WindowState != WindowState.Minimized)
        return;
      this.WindowState = WindowState.Normal;
    }

    protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.RestoreMinimizedWindow();
    }

    protected void OnStateChanged(object sender, EventArgs eventArgs)
    {
      if (this.WindowState == WindowState.Minimized && this.Owner != null)
      {
        ResizableMetroWindow.windowsWithWindowStates = new Dictionary<Window, WindowState>();
        this.Owner.WindowState = this.WindowState;
        foreach (Window ownedWindow in this.Owner.OwnedWindows)
        {
          if (ownedWindow.WindowState == WindowState.Maximized && ownedWindow.GetType() != typeof (GlowWindow))
            ResizableMetroWindow.windowsWithWindowStates.Add(ownedWindow, ownedWindow.WindowState);
        }
        this.restoreWindow = true;
      }
      else if (this.restoreWindow)
      {
        this.Topmost = true;
        this.Show();
        this.Activate();
        this.restoreWindow = false;
        ResizableMetroWindow.windowsWithWindowStates = this.RestorePreviousWindowState(ResizableMetroWindow.windowsWithWindowStates, sender, false);
      }
      else
        ResizableMetroWindow.windowsWithWindowStates = this.RestorePreviousWindowState(ResizableMetroWindow.windowsWithWindowStates, sender, true);
    }

    private Dictionary<Window, WindowState> RestorePreviousWindowState(
      Dictionary<Window, WindowState> windowsWithWindowStates,
      object sender,
      bool setStateToCurrentWindow)
    {
      Window key = (Window) sender;
      if (windowsWithWindowStates != null && windowsWithWindowStates.ContainsKey(key))
      {
        WindowState windowsWithWindowState = windowsWithWindowStates[key];
        windowsWithWindowStates.Remove(key);
        if (setStateToCurrentWindow)
          this.WindowState = windowsWithWindowState;
        if (windowsWithWindowStates.Count == 0)
          windowsWithWindowStates = (Dictionary<Window, WindowState>) null;
      }
      return windowsWithWindowStates;
    }

    protected void Window_Deactivated(object sender, EventArgs e)
    {
      ((Window) sender).Topmost = false;
    }

    internal struct WINDOWPOS
    {
      public IntPtr hwnd;
      public IntPtr hwndInsertAfter;
      public int x;
      public int y;
      public int cx;
      public int cy;
      public int flags;
    }

    public enum SWP : uint
    {
      NOSIZE = 1,
      NOMOVE = 2,
      NOZORDER = 4,
      NOREDRAW = 8,
      NOACTIVATE = 16, // 0x00000010
      FRAMECHANGED = 32, // 0x00000020
      SHOWWINDOW = 64, // 0x00000040
      HIDEWINDOW = 128, // 0x00000080
      NOCOPYBITS = 256, // 0x00000100
      NOOWNERZORDER = 512, // 0x00000200
      NOSENDCHANGING = 1024, // 0x00000400
    }

    public struct POINT(int x, int y)
    {
      public int x = x;
      public int y = y;
    }

    public struct MINMAXINFO
    {
      public ResizableMetroWindow.POINT ptReserved;
      public ResizableMetroWindow.POINT ptMaxSize;
      public ResizableMetroWindow.POINT ptMaxPosition;
      public ResizableMetroWindow.POINT ptMinTrackSize;
      public ResizableMetroWindow.POINT ptMaxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
      public int cbSize = Marshal.SizeOf(typeof (ResizableMetroWindow.MONITORINFO));
      public ResizableMetroWindow.RECT rcMonitor = new ResizableMetroWindow.RECT();
      public ResizableMetroWindow.RECT rcWork = new ResizableMetroWindow.RECT();
      public int dwFlags = 0;
    }

    public struct RECT
    {
      public int left;
      public int top;
      public int right;
      public int bottom;
      public static readonly ResizableMetroWindow.RECT Empty = new ResizableMetroWindow.RECT();

      public int Width => Math.Abs(this.right - this.left);

      public int Height => this.bottom - this.top;

      public RECT(int left, int top, int right, int bottom)
      {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
      }

      public RECT(ResizableMetroWindow.RECT rcSrc)
      {
        this.left = rcSrc.left;
        this.top = rcSrc.top;
        this.right = rcSrc.right;
        this.bottom = rcSrc.bottom;
      }

      public bool IsEmpty => this.left >= this.right || this.top >= this.bottom;

      public override string ToString()
      {
        if (this == ResizableMetroWindow.RECT.Empty)
          return "RECT {Empty}";
        return "RECT { left : " + (object) this.left + " / top : " + (object) this.top + " / right : " + (object) this.right + " / bottom : " + (object) this.bottom + " }";
      }

      public override bool Equals(object obj)
      {
        return obj is Rect && this == (ResizableMetroWindow.RECT) obj;
      }

      public override int GetHashCode()
      {
        return this.left.GetHashCode() + this.top.GetHashCode() + this.right.GetHashCode() + this.bottom.GetHashCode();
      }

      public static bool operator ==(
        ResizableMetroWindow.RECT rect1,
        ResizableMetroWindow.RECT rect2)
      {
        return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
      }

      public static bool operator !=(
        ResizableMetroWindow.RECT rect1,
        ResizableMetroWindow.RECT rect2)
      {
        return !(rect1 == rect2);
      }
    }

    public struct APPBARDATA
    {
      public int cbSize;
      public IntPtr hWnd;
      public int uCallbackMessage;
      public int uEdge;
      public ResizableMetroWindow.RECT rc;
      public bool lParam;
    }

    public enum ABMsg
    {
      ABM_NEW,
      ABM_REMOVE,
      ABM_QUERYPOS,
      ABM_SETPOS,
      ABM_GETSTATE,
      ABM_GETTASKBARPOS,
      ABM_ACTIVATE,
      ABM_GETAUTOHIDEBAR,
      ABM_SETAUTOHIDEBAR,
      ABM_WINDOWPOSCHANGED,
      ABM_SETSTATE,
    }

    public enum ABEdge
    {
      ABE_LEFT,
      ABE_TOP,
      ABE_RIGHT,
      ABE_BOTTOM,
    }
  }
}
