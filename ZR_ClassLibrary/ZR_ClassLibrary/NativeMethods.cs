// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.NativeMethods
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Runtime.InteropServices;

#nullable disable
namespace ZR_ClassLibrary
{
  public static class NativeMethods
  {
    public static void KeepWindowsCEDeviceAwake()
    {
      try
      {
        NativeMethods.SystemIdleTimerReset();
      }
      catch
      {
      }
    }

    [DllImport("coredll.dll")]
    private static extern void SystemIdleTimerReset();
  }
}
