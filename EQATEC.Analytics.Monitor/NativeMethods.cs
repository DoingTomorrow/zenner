// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.NativeMethods
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class NativeMethods
  {
    internal const int SM_CXSCREEN = 0;
    internal const int SM_CYSCREEN = 1;
    internal const int SM_CMONITORS = 80;
    private const int BITSPIXEL = 12;
    private const int PLANES = 14;
    private const int LOGPIXELSX = 88;
    private const int LOGPIXELSY = 90;
    private const int CCHDEVICENAME = 32;
    private const uint MONITORINFOF_PRIMARY = 1;

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetVersionEx(ref NativeMethods.OSVERSIONINFOEX osVersionInfo);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GlobalMemoryStatus(ref NativeMethods.MEMORYSTATUS buf);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GlobalMemoryStatusEx([In, Out] NativeMethods.MEMORYSTATUSEX lpBuffer);

    [DllImport("Kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetProductInfo(
      int osMajorVersion,
      int osMinorVersion,
      int spMajorVersion,
      int spMinorVersion,
      out int edition);

    [DllImport("gdi32.dll")]
    internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    internal static extern int GetSystemMetrics(int smIndex);

    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref NativeMethods.MonitorInfoEx lpmi);

    [DllImport("user32.dll")]
    private static extern bool EnumDisplayMonitors(
      IntPtr hdc,
      IntPtr lprcClip,
      NativeMethods.MonitorEnumDelegate lpfnEnum,
      IntPtr dwData);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateDC(
      string lpszDriver,
      string lpszDevice,
      string lpszOutput,
      IntPtr lpInitData);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hdc);

    internal static List<NativeMethods.DisplayInfo> GetDisplays()
    {
      List<NativeMethods.DisplayInfo> col = new List<NativeMethods.DisplayInfo>();
      NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (NativeMethods.MonitorEnumDelegate) ((IntPtr hMonitor, IntPtr hdcMonitor, ref NativeMethods.RectStruct lprcMonitor, IntPtr dwData) =>
      {
        NativeMethods.MonitorInfoEx lpmi = new NativeMethods.MonitorInfoEx();
        lpmi.Init();
        if (NativeMethods.GetMonitorInfo(hMonitor, ref lpmi))
        {
          NativeMethods.DisplayInfo displayInfo = new NativeMethods.DisplayInfo();
          displayInfo.ScreenWidth = (lpmi.Monitor.Right - lpmi.Monitor.Left).ToString();
          displayInfo.ScreenHeight = (lpmi.Monitor.Bottom - lpmi.Monitor.Top).ToString();
          displayInfo.MonitorArea = lpmi.Monitor;
          displayInfo.WorkArea = lpmi.WorkArea;
          displayInfo.Availability = lpmi.Flags.ToString();
          displayInfo.IsPrimary = ((int) lpmi.Flags & 1) != 0;
          string deviceName = lpmi.DeviceName;
          if (displayInfo.IsPrimary)
          {
            IntPtr hdc = hdcMonitor;
            if (hdcMonitor == IntPtr.Zero)
              hdc = NativeMethods.CreateDC(deviceName, (string) null, (string) null, IntPtr.Zero);
            displayInfo.BitDepth = NativeMethods.GetDeviceCaps(hdc, 12);
            displayInfo.BitDepth *= NativeMethods.GetDeviceCaps(hdc, 14);
            displayInfo.DPI = NativeMethods.GetDeviceCaps(hdc, 88);
            displayInfo.DPI = NativeMethods.GetDeviceCaps(hdc, 90);
            if (hdc != hdcMonitor)
              NativeMethods.DeleteDC(hdc);
          }
          col.Add(displayInfo);
        }
        return true;
      }), IntPtr.Zero);
      return col;
    }

    internal struct OSVERSIONINFOEX
    {
      public int dwOSVersionInfoSize;
      public int dwMajorVersion;
      public int dwMinorVersion;
      public int dwBuildNumber;
      public int dwPlatformId;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string szCSDVersion;
      public ushort wServicePackMajor;
      public ushort wServicePackMinor;
      public ushort wSuiteMask;
      public byte wProductType;
      public byte wReserved;
    }

    internal struct MEMORYSTATUS
    {
      public uint dwLength;
      public uint dwMemoryLoad;
      public uint dwTotalPhys;
      public uint dwAvailPhys;
      public uint dwTotalPageFile;
      public uint dwAvailPageFile;
      public uint dwTotalVirtual;
      public uint dwAvailVirtual;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MEMORYSTATUSEX
    {
      public uint dwLength;
      public uint dwMemoryLoad;
      public ulong ullTotalPhys;
      public ulong ullAvailPhys;
      public ulong ullTotalPageFile;
      public ulong ullAvailPageFile;
      public ulong ullTotalVirtual;
      public ulong ullAvailVirtual;
      public ulong ullAvailExtendedVirtual;

      public MEMORYSTATUSEX()
      {
        this.dwLength = (uint) Marshal.SizeOf(typeof (NativeMethods.MEMORYSTATUSEX));
      }
    }

    private delegate bool MonitorEnumDelegate(
      IntPtr hMonitor,
      IntPtr hdcMonitor,
      ref NativeMethods.RectStruct lprcMonitor,
      IntPtr dwData);

    internal struct MonitorInfoEx
    {
      public int Size;
      public NativeMethods.RectStruct Monitor;
      public NativeMethods.RectStruct WorkArea;
      public uint Flags;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string DeviceName;

      public void Init()
      {
        this.Size = Marshal.SizeOf((object) this);
        this.DeviceName = string.Empty;
      }
    }

    public struct RectStruct
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }

    public class DisplayInfo
    {
      public string Availability { get; set; }

      public string ScreenHeight { get; set; }

      public string ScreenWidth { get; set; }

      public NativeMethods.RectStruct MonitorArea { get; set; }

      public NativeMethods.RectStruct WorkArea { get; set; }

      public bool IsPrimary { get; set; }

      public int BitDepth { get; set; }

      public int DPI { get; set; }
    }
  }
}
