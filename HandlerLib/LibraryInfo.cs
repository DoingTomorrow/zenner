// Decompiled with JetBrains decompiler
// Type: HandlerLib.LibraryInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;

#nullable disable
namespace HandlerLib
{
  public static class LibraryInfo
  {
    public static string MBusProtocolSpecification = "2.23";
    public static string MBusProtocolDateOfImplementation = "10/25/2017";
    public static string ImplementedFunctionCodes = "\n" + LibraryInfo.getAllImplementedFCsFromManufacturer();
    public static string ImplementedExtendedFunctionCodes = "\n Radio commands\n" + LibraryInfo.getAllImplementedRadioCMDs() + "\n MBus commands\n" + LibraryInfo.getAllImplementedMBusCMDs() + "\n LoRa commands\n" + LibraryInfo.getAllImplementedLoRaCMDs() + "\n Special commands\n" + LibraryInfo.getAllImplementedSpecialCMDs() + "\n";

    private static string getAllImplementedFCsFromManufacturer()
    {
      string str = string.Empty;
      foreach (byte num in Enum.GetValues(typeof (Manufacturer_FC)))
        str = str + ", 0x" + num.ToString("x2");
      return str.Substring(1);
    }

    private static string getAllImplementedLoRaCMDs()
    {
      string str = string.Empty;
      foreach (byte num in Enum.GetValues(typeof (CommonLoRaCommands_EFC)))
        str = str + ", 0x" + num.ToString("x2");
      return str.Substring(1);
    }

    private static string getAllImplementedMBusCMDs()
    {
      string str = string.Empty;
      foreach (byte num in Enum.GetValues(typeof (CommonMBusCommands_EFC)))
        str = str + ", 0x" + num.ToString("x2");
      return str.Substring(1);
    }

    private static string getAllImplementedRadioCMDs()
    {
      string str = string.Empty;
      foreach (byte num in Enum.GetValues(typeof (CommonRadioCommands_EFC)))
        str = str + ", 0x" + num.ToString("x2");
      return str.Substring(1);
    }

    private static string getAllImplementedSpecialCMDs()
    {
      string str = string.Empty;
      foreach (byte num in Enum.GetValues(typeof (SpecialCommands_EFC)))
        str = str + ", 0x" + num.ToString("x2");
      return str.Substring(1);
    }
  }
}
