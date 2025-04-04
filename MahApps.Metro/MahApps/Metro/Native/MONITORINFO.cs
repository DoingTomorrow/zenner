// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Native.MONITORINFO
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.InteropServices;

#nullable disable
namespace MahApps.Metro.Native
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal class MONITORINFO
  {
    public int cbSize = Marshal.SizeOf(typeof (MONITORINFO));
    public RECT rcMonitor;
    public RECT rcWork;
    public int dwFlags;

    public enum MonitorOptions : uint
    {
      MONITOR_DEFAULTTONULL,
      MONITOR_DEFAULTTOPRIMARY,
      MONITOR_DEFAULTTONEAREST,
    }
  }
}
