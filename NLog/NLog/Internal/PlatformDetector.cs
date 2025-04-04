// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PlatformDetector
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Internal
{
  internal static class PlatformDetector
  {
    private static RuntimeOS currentOS = PlatformDetector.GetCurrentRuntimeOS();
    private static bool? _isMono;

    public static RuntimeOS CurrentOS => PlatformDetector.currentOS;

    public static bool IsDesktopWin32
    {
      get
      {
        return PlatformDetector.currentOS == RuntimeOS.Windows || PlatformDetector.currentOS == RuntimeOS.WindowsNT;
      }
    }

    public static bool IsWin32
    {
      get
      {
        return PlatformDetector.currentOS == RuntimeOS.Windows || PlatformDetector.currentOS == RuntimeOS.WindowsNT || PlatformDetector.currentOS == RuntimeOS.WindowsCE;
      }
    }

    public static bool IsUnix => PlatformDetector.currentOS == RuntimeOS.Unix;

    public static bool IsMono
    {
      get
      {
        return PlatformDetector._isMono ?? (PlatformDetector._isMono = new bool?(Type.GetType("Mono.Runtime") != (Type) null)).Value;
      }
    }

    public static bool SupportsSharableMutex
    {
      get => !PlatformDetector.IsMono || Environment.Version.Major >= 4;
    }

    private static RuntimeOS GetCurrentRuntimeOS()
    {
      switch (Environment.OSVersion.Platform)
      {
        case PlatformID.Win32Windows:
          return RuntimeOS.Windows;
        case PlatformID.Win32NT:
          return RuntimeOS.WindowsNT;
        case PlatformID.WinCE:
          return RuntimeOS.WindowsCE;
        case PlatformID.Unix:
        case (PlatformID) 128:
          return RuntimeOS.Unix;
        default:
          return RuntimeOS.Unknown;
      }
    }
  }
}
