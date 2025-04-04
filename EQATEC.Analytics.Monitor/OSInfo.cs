// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.OSInfo
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using Microsoft.Win32;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

#nullable disable
namespace EQATEC.Analytics.Monitor
{
  internal class OSInfo
  {
    private static void GetPlatformAndOSInformation(OSInfoObject osInfo, ILogAnalyticsMonitor log)
    {
      try
      {
        osInfo.OSVersion = Environment.OSVersion.Version;
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive OS version information (Environment.OSVersion.Version). Error encountered is " + ex.Message);
      }
      try
      {
        osInfo.Platform = Environment.OSVersion.Platform;
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive OS version information (Environment.OSVersion.Platform). Error encountered is " + ex.Message);
      }
      try
      {
        NativeMethods.OSVERSIONINFOEX osVersionInfo = new NativeMethods.OSVERSIONINFOEX()
        {
          dwOSVersionInfoSize = Marshal.SizeOf(typeof (NativeMethods.OSVERSIONINFOEX))
        };
        if (NativeMethods.GetVersionEx(ref osVersionInfo))
        {
          osInfo.SPVersion = new Version((int) osVersionInfo.wServicePackMajor, (int) osVersionInfo.wServicePackMinor);
          osInfo.SuiteMask = osVersionInfo.wSuiteMask;
          osInfo.ProductType = (int) osVersionInfo.wProductType;
        }
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive OS version information. Error encountered is " + ex.Message);
      }
      try
      {
        if (!(osInfo.OSVersion != (Version) null) || osInfo.OSVersion.Major < 6)
          return;
        int edition;
        NativeMethods.GetProductInfo(osInfo.OSVersion.Major, osInfo.OSVersion.Minor, osInfo.SPVersion.Major, osInfo.SPVersion.Minor, out edition);
        osInfo.ProductInfo = edition;
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive OS product information. Error encountered is " + ex.Message);
      }
    }

    private static void GetCultureInformation(OSInfoObject osInfo, ILogAnalyticsMonitor log)
    {
      try
      {
        osInfo.LocaleUi = CultureInfo.CurrentUICulture.Name.ToLower();
        osInfo.LocaleThread = CultureInfo.CurrentCulture.Name.ToLower();
        if (!string.IsNullOrEmpty(osInfo.LocaleUi))
          return;
        osInfo.LocaleUi = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive culture information. Error encountered is " + ex.Message);
      }
    }

    private static void GetBitnessAndArchitecture(OSInfoObject osInfo, ILogAnalyticsMonitor log)
    {
      try
      {
        int num = IntPtr.Size * 8;
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432")))
          num = 64;
        osInfo.OSBit = num;
        osInfo.Architecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine);
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive architecture information. Error encountered is " + ex.Message);
      }
    }

    private static void GetPhysicalMemoryAndProcessors(
      OSInfoObject osInfo,
      ILogAnalyticsMonitor log)
    {
      try
      {
        osInfo.NumberOfProcessors = Environment.ProcessorCount;
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive processor count information. Error encountered is " + ex.Message);
      }
      try
      {
        NativeMethods.MEMORYSTATUSEX lpBuffer = new NativeMethods.MEMORYSTATUSEX();
        if (NativeMethods.GlobalMemoryStatusEx(lpBuffer))
        {
          osInfo.TotalPhysMemory = lpBuffer.ullTotalPhys;
        }
        else
        {
          NativeMethods.MEMORYSTATUS buf = new NativeMethods.MEMORYSTATUS();
          if (!NativeMethods.GlobalMemoryStatus(ref buf))
            return;
          osInfo.TotalPhysMemory = (ulong) buf.dwTotalPhys;
        }
      }
      catch (Exception ex)
      {
        log.LogError("Could not correctly retreive memory information. Error encountered is " + ex.Message);
      }
    }

    private static void GetScreenInformation(OSInfoObject osInfo, ILogAnalyticsMonitor log)
    {
      try
      {
        osInfo.ScreenResWidth = NativeMethods.GetSystemMetrics(0);
        osInfo.ScreenResHeight = NativeMethods.GetSystemMetrics(1);
        osInfo.NumberOfScreens = NativeMethods.GetSystemMetrics(80);
        NativeMethods.DisplayInfo displayInfo = NativeMethods.GetDisplays().Find((Predicate<NativeMethods.DisplayInfo>) (x => x.IsPrimary));
        if (displayInfo == null)
          return;
        osInfo.ScreenDPI = displayInfo.DPI;
      }
      catch (Exception ex)
      {
        log.LogError("Failed to obtain scrren information. Error message is " + ex.Message);
      }
    }

    private static void GetModelAndManufacturer(OSInfoObject osInfo, ILogAnalyticsMonitor log)
    {
      osInfo.Model = (string) null;
      osInfo.Manufacturer = (string) null;
    }

    private static void GetFrameworkVersion(OSInfoObject osInfo, ILogAnalyticsMonitor log)
    {
      try
      {
        Version version = new Version(0, 0);
        string newestFrameworkPath = (string) null;
        RegistryKey regKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\NET Framework Setup\\NDP");
        if (regKey != null)
          version = OSInfo.GetNewestFramework(regKey, regKey.GetSubKeyNames(), ref newestFrameworkPath);
        if (version == new Version(0, 0))
          version = Environment.Version;
        if (string.IsNullOrEmpty(newestFrameworkPath))
          return;
        RegistryKey registryKey = regKey.OpenSubKey(newestFrameworkPath);
        osInfo.FrameworkVersion = version.ToString();
        if (registryKey == null)
          return;
        osInfo.FrameworkSPVersion = registryKey.GetValue("SP", (object) 0).ToString();
      }
      catch (Exception ex)
      {
        log.LogError("Could not retreive NET Framework information. Error encountered is " + ex.Message);
      }
    }

    private static Version GetNewestFramework(
      RegistryKey regKey,
      string[] names,
      ref string newestFrameworkPath)
    {
      Version newestFramework = new Version(Environment.Version.ToString(2));
      foreach (string name in names)
      {
        try
        {
          Version versionFromString = OSInfo.GetVersionFromString(name);
          try
          {
            using (RegistryKey registryKey = regKey.OpenSubKey(name + "\\Client", false))
            {
              if (registryKey != null)
              {
                string str = registryKey.GetValue("Version") as string;
                if (!string.IsNullOrEmpty(str))
                  versionFromString = OSInfo.GetVersionFromString(str);
              }
            }
          }
          catch
          {
          }
          if ((Version) null != versionFromString)
          {
            if (newestFramework < versionFromString)
            {
              newestFramework = versionFromString;
              newestFrameworkPath = name;
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      return newestFramework;
    }

    private static Version GetVersionFromString(string str)
    {
      Version versionFromString = (Version) null;
      if (str.Length == 0)
        return versionFromString;
      if (str[0] == 'v')
      {
        string str1 = str.Substring(1);
        versionFromString = str1.Length != 1 ? new Version(str1) : new Version(int.Parse(str1), 0);
      }
      else if (char.IsNumber(str[0]))
        versionFromString = new Version(str);
      return versionFromString;
    }

    internal static OSInfoObject GetOSInfo(ILogAnalyticsMonitor log)
    {
      OSInfoObject osInfo = new OSInfoObject();
      try
      {
        OSInfo.GetPlatformAndOSInformation(osInfo, log);
        OSInfo.GetFrameworkVersion(osInfo, log);
        OSInfo.GetCultureInformation(osInfo, log);
        OSInfo.GetBitnessAndArchitecture(osInfo, log);
        OSInfo.GetPhysicalMemoryAndProcessors(osInfo, log);
        OSInfo.GetScreenInformation(osInfo, log);
        OSInfo.GetModelAndManufacturer(osInfo, log);
      }
      catch (Exception ex)
      {
        log.LogError("Failed to obtain environment information. Error message is: " + ex.Message);
      }
      return osInfo;
    }
  }
}
