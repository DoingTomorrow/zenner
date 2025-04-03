// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Constants
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using ZENNER.CommonLibrary.Entities;
using ZENNER.CommonLibrary.MeterVPNServer;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public static class Constants
  {
    public static List<ValueItem> GetAvailableComPorts()
    {
      SortedList<int, ValueItem> sortedList = new SortedList<int, ValueItem>();
      using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption, Service, DeviceID FROM Win32_PnPEntity WHERE Caption LIKE '%(COM%' AND ConfigManagerErrorCode = 0"))
      {
        foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
        {
          string deviceID = managementBaseObject["DeviceID"].ToString();
          string service = managementBaseObject["Service"].ToString();
          string str1 = managementBaseObject["Caption"].ToString();
          int startIndex = str1.IndexOf("(") + 1;
          int num = str1.IndexOf(")");
          if (num >= 0)
          {
            string str2 = str1.Substring(startIndex, num - startIndex);
            int result;
            if (int.TryParse(str2.Replace("COM", ""), out result))
            {
              string hardwareId = Constants.TryParseHardwareID(service, deviceID);
              if (!(service == "BTHMODEM") || !(hardwareId == "000000000000"))
              {
                ValueItem valueItem = new ValueItem(str2);
                valueItem.AddAdditionalInfo(AdditionalInfoKey.HardwareType, Constants.Translate(service));
                valueItem.AddAdditionalInfo(AdditionalInfoKey.HardwareID, hardwareId);
                sortedList.Add(result, valueItem);
              }
            }
          }
        }
      }
      DeviceInformationCollection result1 = WindowsRuntimeSystemExtensions.GetAwaiter<DeviceInformationCollection>(DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelector())).GetResult();
      HashSet<string> MiConComPorts = new HashSet<string>();
      int num1 = 1000;
      foreach (DeviceInformation BLE_DeviceInfo in (IEnumerable<DeviceInformation>) result1)
      {
        BluetoothLEDevice bluetoothDevice;
        string miConBlePortName = Constants.GetMiConBLE_PortName(BLE_DeviceInfo, MiConComPorts, out bluetoothDevice);
        if (miConBlePortName != null)
        {
          ValueItem valueItem = new ValueItem(miConBlePortName);
          valueItem.AddAdditionalInfo(AdditionalInfoKey.HardwareType, "BluetoothLE");
          valueItem.AddAdditionalInfo(AdditionalInfoKey.BluetoothAddress, bluetoothDevice.BluetoothAddress.ToString("X012"));
          sortedList.Add(num1++, valueItem);
        }
      }
      return sortedList.Values.ToList<ValueItem>();
    }

    public static BluetoothLEDevice GetMiConBLE_DeviceFromPort(string port)
    {
      DeviceInformationCollection result = WindowsRuntimeSystemExtensions.GetAwaiter<DeviceInformationCollection>(DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelector())).GetResult();
      HashSet<string> MiConComPorts = new HashSet<string>();
      BluetoothLEDevice bluetoothDevice = (BluetoothLEDevice) null;
      foreach (DeviceInformation BLE_DeviceInfo in (IEnumerable<DeviceInformation>) result)
      {
        string miConBlePortName = Constants.GetMiConBLE_PortName(BLE_DeviceInfo, MiConComPorts, out bluetoothDevice);
        if (miConBlePortName != null && miConBlePortName == port)
          return bluetoothDevice;
      }
      throw new Exception("MinoConnect port not found");
    }

    private static string GetMiConBLE_PortName(
      DeviceInformation BLE_DeviceInfo,
      HashSet<string> MiConComPorts,
      out BluetoothLEDevice bluetoothDevice)
    {
      bluetoothDevice = (BluetoothLEDevice) null;
      string id = BLE_DeviceInfo.Id;
      string name = BLE_DeviceInfo.Name;
      if (!name.StartsWith("Mi"))
        return (string) null;
      string miConBlePortName;
      if (name.Contains("MinoConnectBLE"))
      {
        bluetoothDevice = WindowsRuntimeSystemExtensions.GetAwaiter<BluetoothLEDevice>(BluetoothLEDevice.FromIdAsync(id)).GetResult();
        if (bluetoothDevice == null)
          return (string) null;
        miConBlePortName = "Mi" + bluetoothDevice.BluetoothAddress.ToString("X012").Substring(9);
        if (miConBlePortName != name.Substring(0, 5))
          return (string) null;
      }
      else
      {
        if (name.Length == 6)
        {
          bluetoothDevice = WindowsRuntimeSystemExtensions.GetAwaiter<BluetoothLEDevice>(BluetoothLEDevice.FromIdAsync(id)).GetResult();
          if (bluetoothDevice == null)
            return (string) null;
          miConBlePortName = "Mi" + bluetoothDevice.BluetoothAddress.ToString("X012").Substring(8);
        }
        else
        {
          if (name.Length != 5)
            return (string) null;
          bluetoothDevice = WindowsRuntimeSystemExtensions.GetAwaiter<BluetoothLEDevice>(BluetoothLEDevice.FromIdAsync(id)).GetResult();
          if (bluetoothDevice == null)
            return (string) null;
          miConBlePortName = "Mi" + bluetoothDevice.BluetoothAddress.ToString("X012").Substring(9);
        }
        if (name != miConBlePortName)
          return (string) null;
      }
      int num = 1;
      string str = miConBlePortName;
      while (MiConComPorts.Contains(miConBlePortName))
      {
        miConBlePortName = str + "_" + num.ToString();
        ++num;
      }
      MiConComPorts.Add(miConBlePortName);
      return miConBlePortName;
    }

    private static string Translate(string service)
    {
      switch (service)
      {
        case "BTHMODEM":
          return "Bluetooth";
        case "FTSER2K":
        case "usbser":
          return "USB";
        default:
          return service;
      }
    }

    private static string TryParseHardwareID(string service, string deviceID)
    {
      try
      {
        switch (service)
        {
          case "Serial":
            int startIndex1 = deviceID.LastIndexOf("\\") + 1;
            return deviceID.Substring(startIndex1);
          case "BTHMODEM":
            int startIndex2 = deviceID.LastIndexOf("&") + 1;
            int num1 = deviceID.LastIndexOf("_");
            if (num1 >= 0)
              return deviceID.Substring(startIndex2, num1 - startIndex2);
            break;
          case "FTSER2K":
            int startIndex3 = deviceID.LastIndexOf("+") + 1;
            int num2 = deviceID.LastIndexOf("\\");
            if (num2 >= 0)
              return deviceID.Substring(startIndex3, num2 - startIndex3);
            break;
          case "usbser":
            int startIndex4 = deviceID.LastIndexOf("\\") + 1;
            return deviceID.Substring(startIndex4);
        }
      }
      catch
      {
      }
      return deviceID;
    }

    public static List<ValueItem> GetAvailableBaudrates()
    {
      return new List<ValueItem>()
      {
        new ValueItem("300"),
        new ValueItem("600"),
        new ValueItem("1200"),
        new ValueItem("2400"),
        new ValueItem("4800"),
        new ValueItem("9600"),
        new ValueItem("19200"),
        new ValueItem("38400"),
        new ValueItem("57600"),
        new ValueItem("115200")
      };
    }

    public static List<ValueItem> GetAvailableCOMserver()
    {
      List<ValueItem> availableCoMserver = new List<ValueItem>();
      availableCoMserver.Add(new ValueItem("-"));
      COMserver[] source = MeterVPN.ReadListOfCOMserver();
      if (source == null)
        return availableCoMserver;
      foreach (COMserver coMserver in (IEnumerable<COMserver>) ((IEnumerable<COMserver>) source).OrderBy<COMserver, int>((Func<COMserver, int>) (comServer => Convert.ToInt32(comServer.Cert.Replace("COMserver", string.Empty)))).OrderByDescending<COMserver, bool>((Func<COMserver, bool>) (comServer => comServer.Online)))
      {
        ValueItem valueItem = new ValueItem(coMserver.Cert)
        {
          AdditionalInfo = new Dictionary<AdditionalInfoKey, string>()
        };
        valueItem.AdditionalInfo.Add(AdditionalInfoKey.IpAddress, coMserver.IP);
        valueItem.AdditionalInfo.Add(AdditionalInfoKey.Name, coMserver.Name);
        valueItem.AdditionalInfo.Add(AdditionalInfoKey.IsOnline, coMserver.Online.ToString());
        valueItem.AdditionalInfo.Add(AdditionalInfoKey.LastSeen, coMserver.LastSeen);
        valueItem.AdditionalInfo.Add(AdditionalInfoKey.Traffic, coMserver.Traffic);
        availableCoMserver.Add(valueItem);
      }
      return availableCoMserver;
    }

    public static List<ValueItem> GetAvailableParity()
    {
      return new List<ValueItem>()
      {
        new ValueItem("no"),
        new ValueItem("odd"),
        new ValueItem("even")
      };
    }

    public static List<ValueItem> GetAvailableTestEcho()
    {
      return new List<ValueItem>()
      {
        new ValueItem(true.ToString()),
        new ValueItem(false.ToString())
      };
    }

    public static List<ValueItem> GetAvailableWakeup()
    {
      return Constants.GetEnumNames(typeof (WakeupSystem));
    }

    public static List<ValueItem> GetAvailableIrDaSelection()
    {
      return Constants.GetEnumNames(typeof (IrDaSelection));
    }

    public static List<ValueItem> GetAvailableCombiHeadSelection()
    {
      return Constants.GetEnumNames(typeof (CombiHeadSelection));
    }

    public static List<ValueItem> GetAvailableTransceiverDevice()
    {
      return Constants.GetEnumNames(typeof (TransceiverDevice));
    }

    public static List<ValueItem> GetAvailableAsyncComConnectionType()
    {
      return Constants.GetEnumNames(typeof (AsyncComConnectionType));
    }

    public static List<ValueItem> GetAvailableBusMode() => Constants.GetEnumNames(typeof (BusMode));

    public static List<ValueItem> GetAvailableMinoConnectBaseStates()
    {
      return Constants.GetEnumNames(typeof (MinoConnectBaseStates));
    }

    private static List<ValueItem> GetEnumNames(Type enumType)
    {
      List<ValueItem> enumNames = new List<ValueItem>();
      foreach (string name in Enum.GetNames(enumType))
        enumNames.Add(new ValueItem(name));
      return enumNames;
    }
  }
}
