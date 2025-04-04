// Decompiled with JetBrains decompiler
// Type: Microsoft.Windows.Shell.SystemCommands
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Standard;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

#nullable disable
namespace Microsoft.Windows.Shell
{
  internal static class SystemCommands
  {
    public static RoutedCommand CloseWindowCommand { get; private set; }

    public static RoutedCommand MaximizeWindowCommand { get; private set; }

    public static RoutedCommand MinimizeWindowCommand { get; private set; }

    public static RoutedCommand RestoreWindowCommand { get; private set; }

    public static RoutedCommand ShowSystemMenuCommand { get; private set; }

    static SystemCommands()
    {
      SystemCommands.CloseWindowCommand = new RoutedCommand("CloseWindow", typeof (SystemCommands));
      SystemCommands.MaximizeWindowCommand = new RoutedCommand("MaximizeWindow", typeof (SystemCommands));
      SystemCommands.MinimizeWindowCommand = new RoutedCommand("MinimizeWindow", typeof (SystemCommands));
      SystemCommands.RestoreWindowCommand = new RoutedCommand("RestoreWindow", typeof (SystemCommands));
      SystemCommands.ShowSystemMenuCommand = new RoutedCommand("ShowSystemMenu", typeof (SystemCommands));
    }

    private static void _PostSystemCommand(Window window, SC command)
    {
      IntPtr handle = new WindowInteropHelper(window).Handle;
      if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
        return;
      NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((int) command), IntPtr.Zero);
    }

    public static void CloseWindow(Window window)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      SystemCommands._PostSystemCommand(window, SC.CLOSE);
    }

    public static void MaximizeWindow(Window window)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      SystemCommands._PostSystemCommand(window, SC.MAXIMIZE);
    }

    public static void MinimizeWindow(Window window)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      SystemCommands._PostSystemCommand(window, SC.MINIMIZE);
    }

    public static void RestoreWindow(Window window)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      SystemCommands._PostSystemCommand(window, SC.RESTORE);
    }

    public static void ShowSystemMenu(Window window, Point screenLocation)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      SystemCommands.ShowSystemMenuPhysicalCoordinates(window, DpiHelper.LogicalPixelsToDevice(screenLocation));
    }

    internal static void ShowSystemMenuPhysicalCoordinates(
      Window window,
      Point physicalScreenLocation)
    {
      Verify.IsNotNull<Window>(window, nameof (window));
      IntPtr handle = new WindowInteropHelper(window).Handle;
      if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
        return;
      uint num = NativeMethods.TrackPopupMenuEx(NativeMethods.GetSystemMenu(handle, false), 256U, (int) physicalScreenLocation.X, (int) physicalScreenLocation.Y, handle, IntPtr.Zero);
      if (num == 0U)
        return;
      NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((long) num), IntPtr.Zero);
    }
  }
}
