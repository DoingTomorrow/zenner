// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Native.UnsafeNativeMethods
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Native
{
  [SuppressUnmanagedCodeSecurity]
  internal static class UnsafeNativeMethods
  {
    internal const int GWL_STYLE = -16;
    internal const int WS_SYSMENU = 524288;

    [DllImport("dwmapi", PreserveSig = false)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DwmIsCompositionEnabled();

    [DllImport("dwmapi")]
    [return: MarshalAs(UnmanagedType.Error)]
    internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, [In] ref MARGINS pMarInset);

    [DllImport("dwmapi")]
    internal static extern int DwmSetWindowAttribute(
      [In] IntPtr hwnd,
      [In] int attr,
      [In] ref int attrValue,
      [In] int attrSize);

    [DllImport("user32")]
    internal static extern IntPtr DefWindowProc(
      [In] IntPtr hwnd,
      [In] int msg,
      [In] IntPtr wParam,
      [In] IntPtr lParam);

    [DllImport("user32", EntryPoint = "GetMonitorInfoW", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetMonitorInfo([In] IntPtr hMonitor, [Out] MONITORINFO lpmi);

    [DllImport("user32")]
    internal static extern IntPtr MonitorFromWindow([In] IntPtr handle, [In] int flags);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr MonitorFromPoint(POINT pt, MONITORINFO.MonitorOptions dwFlags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int X,
      int Y,
      int cx,
      int cy,
      uint uFlags);

    [DllImport("user32", EntryPoint = "LoadStringW", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int LoadString(
      [In, Optional] SafeLibraryHandle hInstance,
      [In] uint uID,
      [Out] StringBuilder lpBuffer,
      [In] int nBufferMax);

    [DllImport("user32", CharSet = CharSet.Auto)]
    internal static extern bool IsWindow([In, Optional] IntPtr hWnd);

    [DllImport("user32")]
    internal static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In] bool bRevert);

    [DllImport("user32")]
    internal static extern uint TrackPopupMenuEx(
      [In] IntPtr hmenu,
      [In] uint fuFlags,
      [In] int x,
      [In] int y,
      [In] IntPtr hwnd,
      [In, Optional] IntPtr lptpm);

    [DllImport("user32", EntryPoint = "PostMessage", SetLastError = true)]
    private static extern bool _PostMessage([In, Optional] IntPtr hWnd, [In] uint Msg, [In] IntPtr wParam, [In] IntPtr lParam);

    [DllImport("user32")]
    internal static extern bool GetCursorPos(out POINT pt);

    [DllImport("user32", CharSet = CharSet.Auto)]
    internal static extern int GetDoubleClickTime();

    [DllImport("kernel32", EntryPoint = "LoadLibraryW", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern SafeLibraryHandle LoadLibrary([MarshalAs(UnmanagedType.LPWStr), In] string lpFileName);

    [DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool FreeLibrary([In] IntPtr hModule);

    [DllImport("user32.dll", EntryPoint = "SetClassLong")]
    internal static extern uint SetClassLongPtr32(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetClassLongPtr")]
    internal static extern IntPtr SetClassLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "GetClassLong")]
    internal static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
    internal static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

    [DllImport("gdi32.dll")]
    internal static extern IntPtr CreateSolidBrush(int crColor);

    [DllImport("gdi32.dll")]
    internal static extern bool DeleteObject(IntPtr hObject);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll")]
    internal static extern uint EnableMenuItem(IntPtr hMenu, uint itemId, uint uEnable);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    internal static void PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
    {
      if (!UnsafeNativeMethods._PostMessage(hWnd, Msg, wParam, lParam))
        throw new Win32Exception();
    }

    internal static Point GetPoint(IntPtr ptr)
    {
      uint x = Environment.Is64BitProcess ? (uint) ptr.ToInt64() : (uint) ptr.ToInt32();
      return new Point((double) (short) x, (double) (short) (x >> 16));
    }

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [Obsolete("Use NativeMethods.FindWindow instead.")]
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [Obsolete("Use NativeMethods.SHAppBarMessage instead.")]
    [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
    public static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool RedrawWindow(
      IntPtr hWnd,
      [In] ref RECT lprcUpdate,
      IntPtr hrgnUpdate,
      Constants.RedrawWindowFlags flags);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool RedrawWindow(
      IntPtr hWnd,
      IntPtr lprcUpdate,
      IntPtr hrgnUpdate,
      Constants.RedrawWindowFlags flags);

    internal struct Win32Point
    {
      public readonly int X;
      public readonly int Y;
    }
  }
}
