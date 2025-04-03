// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.SystemValues
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class SystemValues
  {
    public static string AppPath = string.Empty;
    public static string DataPath;
    public static string ImportPath;
    public static string ExportPath;
    public static string DatabasePath;
    public static string CachePath;
    public static string ZDF_DataPath;
    public static string MobileDB_DataPath;
    public static string LoggDataPath;
    public static string BussesPath;
    public static string SettingsPath;
    public static string ExportStylesPath;
    public static string MappingPath;
    private static CultureInfo oldCI;
    private static CultureInfo DeCI = new CultureInfo("de");
    public static string ZRTimeFormatShort;
    public static string ZRTimeFormat;
    public static string ZRTimeFormatLong;
    public static string ZRDateFormat;
    public static string ZRDateTimeFormat;
    public static string ZR_FixedDateTimeFormat = "dd.MM.yyyy HH:mm:ss";
    public static string ZRDezimalSeparator;
    public static string ZRNumberGroupSeperator;
    public static NumberFormatInfo ZRNumberFormatInfo;
    public static string ZRCurrentUserName;
    public static string ZRCurrentMachineName;
    public static string RootPathInfo;
    public static string AppPathInfo;
    public static readonly DateTime GMM_Start = DateTime.Now;
    public static readonly int GMM_StartTicks = Environment.TickCount;

    public static DateTime DateTimeNow => DateTime.Now;

    public static void Dispose()
    {
      SystemValues.AppPathInfo = (string) null;
      SystemValues.RootPathInfo = (string) null;
      SystemValues.ZRCurrentMachineName = (string) null;
      SystemValues.ZRCurrentUserName = (string) null;
      SystemValues.ZRNumberFormatInfo = (NumberFormatInfo) null;
      SystemValues.ZRNumberGroupSeperator = (string) null;
      SystemValues.ZRDezimalSeparator = (string) null;
      SystemValues.ZRDateTimeFormat = (string) null;
      SystemValues.ZRTimeFormatShort = (string) null;
      SystemValues.ZRTimeFormat = (string) null;
      SystemValues.ZRTimeFormatLong = (string) null;
      SystemValues.ZRDateFormat = (string) null;
      SystemValues.oldCI = (CultureInfo) null;
      SystemValues.DataPath = (string) null;
      SystemValues.ImportPath = (string) null;
      SystemValues.ExportPath = (string) null;
      SystemValues.DatabasePath = (string) null;
      SystemValues.CachePath = (string) null;
      SystemValues.ZDF_DataPath = (string) null;
      SystemValues.MobileDB_DataPath = (string) null;
      SystemValues.LoggDataPath = (string) null;
      SystemValues.BussesPath = (string) null;
      SystemValues.SettingsPath = (string) null;
      SystemValues.ExportStylesPath = (string) null;
      SystemValues.MappingPath = (string) null;
      SystemValues.ZR_FixedDateTimeFormat = "dd.MM.yyyy HH:mm:ss";
      SystemValues.AppPath = string.Empty;
      SystemValues.DeCI = new CultureInfo("de");
    }

    public SystemValues()
    {
      if (SystemValues.AppPath.Length == 0)
      {
        string path2 = Path.Combine("ZENNER", "GMM");
        SystemValues.DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), path2);
        SystemValues.AppPath = Application.StartupPath;
        string path1 = Path.Combine(SystemValues.AppPath, "DataPathOverride.txt");
        if (File.Exists(path1))
        {
          try
          {
            using (StreamReader streamReader = new StreamReader(path1))
            {
              string path3 = streamReader.ReadLine();
              if (Directory.Exists(path3))
                SystemValues.DataPath = path3;
            }
          }
          catch
          {
          }
        }
        SystemValues.ImportPath = Path.Combine(SystemValues.DataPath, "Import");
        SystemValues.ExportPath = Path.Combine(SystemValues.DataPath, "Export");
        SystemValues.DatabasePath = Path.Combine(SystemValues.DataPath, "Database");
        SystemValues.CachePath = Path.Combine(SystemValues.DataPath, "Cache");
        SystemValues.ZDF_DataPath = Path.Combine(SystemValues.DataPath, "ZDF_Data");
        SystemValues.MobileDB_DataPath = Path.Combine(SystemValues.DataPath, "MobileDB_Data");
        SystemValues.LoggDataPath = Path.Combine(SystemValues.DataPath, "LoggData");
        SystemValues.BussesPath = Path.Combine(SystemValues.DataPath, "Busses");
        SystemValues.SettingsPath = Path.Combine(SystemValues.DataPath, "Settings");
        SystemValues.ExportStylesPath = Path.Combine(SystemValues.SettingsPath, "ExportStyles");
        SystemValues.MappingPath = Path.Combine(SystemValues.DataPath, "Mappings");
        Directory.CreateDirectory(SystemValues.DataPath);
        Directory.CreateDirectory(SystemValues.ImportPath);
        Directory.CreateDirectory(SystemValues.ExportPath);
        Directory.CreateDirectory(SystemValues.DatabasePath);
        Directory.CreateDirectory(SystemValues.CachePath);
        Directory.CreateDirectory(SystemValues.ZDF_DataPath);
        Directory.CreateDirectory(SystemValues.MobileDB_DataPath);
        Directory.CreateDirectory(SystemValues.LoggDataPath);
        Directory.CreateDirectory(SystemValues.BussesPath);
        Directory.CreateDirectory(SystemValues.SettingsPath);
        Directory.CreateDirectory(SystemValues.ExportStylesPath);
        Directory.CreateDirectory(SystemValues.MappingPath);
        if (!File.Exists(CsvStyle.GetPath(CsvStyle.DefaultStyle1)))
          CsvStyle.Save(CsvStyle.DefaultStyle1);
        if (!File.Exists(CsvStyle.GetPath(CsvStyle.DefaultStyle2)))
          CsvStyle.Save(CsvStyle.DefaultStyle2);
        if (!File.Exists(CsvStyle.GetPath(CsvStyle.DefaultStyle3)))
          CsvStyle.Save(CsvStyle.DefaultStyle3);
      }
      CultureInfo cultureInfo = new CultureInfo(CultureInfo.CurrentCulture.Name);
      SystemValues.ZRCurrentUserName = Environment.UserDomainName;
      SystemValues.ZRCurrentMachineName = Environment.MachineName;
      SystemValues.AppPathInfo = Application.ExecutablePath.ToLower();
      SystemValues.ZRDateTimeFormat = cultureInfo.DateTimeFormat.ShortDatePattern + " " + cultureInfo.DateTimeFormat.LongTimePattern;
      SystemValues.ZRDateFormat = cultureInfo.DateTimeFormat.ShortDatePattern;
      SystemValues.ZRTimeFormat = cultureInfo.DateTimeFormat.LongTimePattern;
      SystemValues.ZRNumberFormatInfo = cultureInfo.NumberFormat;
      SystemValues.ZRDezimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
      SystemValues.ZRNumberGroupSeperator = cultureInfo.NumberFormat.NumberGroupSeparator;
      ushort num1 = 0;
      for (int index = 0; index < SystemValues.AppPathInfo.Length; ++index)
        num1 += (ushort) SystemValues.AppPathInfo[index];
      SystemValues.RootPathInfo = Path.GetPathRoot(SystemValues.AppPathInfo);
      SystemValues.AppPathInfo = num1.ToString("d5") + ((ulong) File.GetCreationTime(SystemValues.RootPathInfo).ToUniversalTime().Ticks).ToString("d22");
      string str = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      try
      {
        ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
        managementObject.Get();
        str = managementObject["VolumeSerialNumber"].ToString();
      }
      catch
      {
      }
      if (str == string.Empty)
      {
        try
        {
          ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementClass("Win32_OperatingSystem").GetInstances().GetEnumerator();
          enumerator.MoveNext();
          empty1 = enumerator.Current["SerialNumber"].ToString();
        }
        catch
        {
        }
        str = empty1;
      }
      SystemValues.AppPathInfo = str + "sujkio" + empty2 + "1h78jki" + empty3;
      ushort num2 = 2345;
      for (int index = 0; index < SystemValues.AppPathInfo.Length; ++index)
        num2 += (ushort) SystemValues.AppPathInfo[index];
      SystemValues.AppPathInfo += num2.ToString();
      byte[] numArray = new byte[27];
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < 200; ++index)
      {
        if (num4 >= SystemValues.AppPathInfo.Length)
          num4 = 0;
        if (num3 >= numArray.Length)
          num3 = 0;
        numArray[num3++] += (byte) SystemValues.AppPathInfo[num4++];
      }
      StringBuilder stringBuilder = new StringBuilder(27);
      for (int index = 0; index < 27; ++index)
      {
        int num5 = (int) numArray[index] % 10;
        stringBuilder.Append(num5.ToString());
      }
      SystemValues.AppPathInfo = stringBuilder.ToString();
    }

    public static List<string> GetPortNames()
    {
      SortedList<int, SerialDeviceSystemData> TheDeviceList;
      if (!SystemValues.GetInstalledSerialDevicesListFromSystem(SystemValues.SerialDeviceType.All, SystemValues.SerialDeviceManufacturer.All, out TheDeviceList, out string _))
        return (List<string>) null;
      List<string> portNames = new List<string>();
      foreach (KeyValuePair<int, SerialDeviceSystemData> keyValuePair in TheDeviceList)
        portNames.Add("COM" + keyValuePair.Key.ToString());
      return portNames;
    }

    public static bool GetInstalledSerialDevicesListFromSystem(
      SystemValues.SerialDeviceType TheDeviceType,
      SystemValues.SerialDeviceManufacturer TheDeviceManufacturer,
      out SortedList<int, SerialDeviceSystemData> TheDeviceList,
      out string ErrorString)
    {
      ErrorString = string.Empty;
      TheDeviceList = new SortedList<int, SerialDeviceSystemData>();
      try
      {
        ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from win32_PnPEntity");
        if (managementObjectSearcher != null)
        {
          foreach (ManagementObject managementObject in managementObjectSearcher.Get())
          {
            try
            {
              string str1 = managementObject.GetPropertyValue("Name").ToString();
              string TheSerialDeviceName = str1.Replace(" ", string.Empty);
              if (str1.Contains("(COM"))
              {
                int comPortNumber = SystemValues.GetComPortNumber(TheSerialDeviceName);
                SerialDeviceSystemData deviceSystemData = new SerialDeviceSystemData();
                deviceSystemData.Name = str1;
                deviceSystemData.DeviceID = managementObject.GetPropertyValue("DeviceID").ToString();
                deviceSystemData.Manufacturer = managementObject.GetPropertyValue("Manufacturer").ToString();
                deviceSystemData.Service = managementObject.GetPropertyValue("Service").ToString();
                deviceSystemData.Status = managementObject.GetPropertyValue("Status").ToString();
                try
                {
                  if (deviceSystemData.DeviceID.StartsWith("BTHENUM"))
                  {
                    int num = deviceSystemData.DeviceID.IndexOf("\\8&27BEC276&0&");
                    if (num > 0)
                    {
                      string str2 = deviceSystemData.DeviceID.Substring(num + 22, 4);
                      if (str2.Length == 4 && str2 != "0000")
                        deviceSystemData.SpecialDeviceName = "MinoConnectBT: " + str2;
                    }
                  }
                  else if (deviceSystemData.DeviceID.StartsWith("FTDIBUS"))
                  {
                    int num = deviceSystemData.DeviceID.IndexOf("VID_0403+PID_6001+");
                    if (num > 0)
                    {
                      string str3 = deviceSystemData.DeviceID.Substring(num + 22, 5);
                      if (str3.Length == 5)
                        deviceSystemData.SpecialDeviceName = "MinoConnectUSB: " + str3;
                    }
                  }
                }
                catch
                {
                }
                bool flag = true;
                if (TheDeviceManufacturer == SystemValues.SerialDeviceManufacturer.FTDI && deviceSystemData.Manufacturer.ToUpper().Trim() != "FTDI")
                  flag = false;
                if (TheDeviceType == SystemValues.SerialDeviceType.USB && !deviceSystemData.Name.ToUpper().Trim().Contains("USB"))
                  flag = false;
                if (flag)
                  TheDeviceList[comPortNumber] = deviceSystemData;
              }
            }
            catch
            {
            }
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        ErrorString = ex.ToString();
        return false;
      }
    }

    private static int GetComPortNumber(string TheSerialDeviceName)
    {
      string str = "(COM";
      int startIndex = TheSerialDeviceName.IndexOf(str) + str.Length;
      string empty = string.Empty;
      while (SystemValues.IsNumber(TheSerialDeviceName.Substring(startIndex, 1)))
      {
        empty += TheSerialDeviceName.Substring(startIndex, 1);
        ++startIndex;
        if (startIndex >= TheSerialDeviceName.Length - 1)
          break;
      }
      return int.Parse(empty);
    }

    public static string GetFTDIAddressFromDeviceID(string TheDeviceID)
    {
      try
      {
        if (!(TheDeviceID.Trim().Substring(0, 7) == "FTDIBUS"))
          return string.Empty;
        string str = TheDeviceID.Substring(TheDeviceID.LastIndexOf("+") + 1);
        return str.Remove(str.LastIndexOf("\\"));
      }
      catch
      {
        return string.Empty;
      }
    }

    private static bool IsNumber(string TheCharacter)
    {
      char[] charArray = TheCharacter.ToCharArray(0, 1);
      return charArray[0] >= '0' && charArray[0] <= '9';
    }

    public static string FillRightWithSpaces(string TheString, int TheLengthAfterFilling)
    {
      if (TheString.Length < TheLengthAfterFilling)
      {
        do
        {
          TheString += " ";
        }
        while (TheString.Length < TheLengthAfterFilling);
      }
      return TheString;
    }

    public static string convertGermanNumberFormatToCurrent(string NumberAsString)
    {
      string current = NumberAsString;
      SystemValues.oldCI = Thread.CurrentThread.CurrentUICulture;
      try
      {
        string[] strArray = NumberAsString.Split('.');
        if (strArray.GetLength(0) == 2)
          current = strArray[0] + SystemValues.ZRDezimalSeparator + strArray[1];
      }
      catch
      {
        Thread.CurrentThread.CurrentUICulture = SystemValues.oldCI;
      }
      return current;
    }

    public enum SerialDeviceManufacturer
    {
      All,
      FTDI,
    }

    public enum SerialDeviceType
    {
      All,
      USB,
    }
  }
}
