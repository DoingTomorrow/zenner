// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Models.Win32.NativeMethods
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace MahApps.Metro.Models.Win32
{
  internal static class NativeMethods
  {
    public static WS GetWindowLong(this IntPtr hWnd) => (WS) NativeMethods.GetWindowLong(hWnd, -16);

    public static WSEX GetWindowLongEx(this IntPtr hWnd)
    {
      return (WSEX) NativeMethods.GetWindowLong(hWnd, -20);
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    public static WS SetWindowLong(this IntPtr hWnd, WS dwNewLong)
    {
      return (WS) NativeMethods.SetWindowLong(hWnd, -16, (int) dwNewLong);
    }

    public static WSEX SetWindowLongEx(this IntPtr hWnd, WSEX dwNewLong)
    {
      return (WSEX) NativeMethods.SetWindowLong(hWnd, -20, (int) dwNewLong);
    }

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int x,
      int y,
      int cx,
      int cy,
      SWP flags);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);
  }
}
