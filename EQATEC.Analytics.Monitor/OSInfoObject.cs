// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.OSInfoObject
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class OSInfoObject
  {
    public Version OSVersion { get; set; }

    public PlatformID Platform { get; set; }

    public Version SPVersion { get; set; }

    public ushort SuiteMask { get; set; }

    public int ProductType { get; set; }

    public int ProductInfo { get; set; }

    public string FrameworkVersion { get; set; }

    public string FrameworkSPVersion { get; set; }

    public string LocaleUi { get; set; }

    public string LocaleThread { get; set; }

    public int OSBit { get; set; }

    public string Architecture { get; set; }

    public int NumberOfProcessors { get; set; }

    public ulong TotalPhysMemory { get; set; }

    public int ScreenResWidth { get; set; }

    public int ScreenResHeight { get; set; }

    public int ScreenDPI { get; set; }

    public int NumberOfScreens { get; set; }

    public string Manufacturer { get; set; }

    public string Model { get; set; }
  }
}
