// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.AppParametersManagement.EquipmentHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using InTheHand.Net.Sockets;
using Microsoft.Win32;
using MSS.Business.Errors;
using MSS.Business.Modules.GMM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Business.Modules.AppParametersManagement
{
  public class EquipmentHelper
  {
    public static List<ConfigurationPropertyDTO> PrepareDataSourceListForParam(
      ChangeableParameter item,
      EquipmentModel equipment)
    {
      bool flag1 = item.Key == "COMserver";
      bool flag2 = equipment.Name.Contains("MinoConnect");
      if (flag1)
        return EquipmentHelper.PrepareDataSourceListForParamComServer(item);
      return flag2 ? EquipmentHelper.PreparePortsForBluetoothMinoConnect(item) : EquipmentHelper.PrepareDataSourceListForParam(item);
    }

    private static List<ConfigurationPropertyDTO> PrepareDataSourceListForParamComServer(
      ChangeableParameter item)
    {
      MessageHandler.LogDebug("Prepare ComServer list for equipment");
      List<ConfigurationPropertyDTO> listItem = new List<ConfigurationPropertyDTO>();
      GMMHelper.UpdateAvailableValuesForParam(item);
      MessageHandler.LogDebug("Number of found com servers: " + (object) item == null || item.AvailableValues == null ? "0" : item.AvailableValues.Count.ToString());
      item?.AvailableValues?.ForEach((Action<ValueItem>) (availableValue => listItem.Add(EquipmentHelper.GetFormattedCOMServerItemForDropdown(availableValue))));
      listItem = listItem.OrderByDescending<ConfigurationPropertyDTO, bool>((Func<ConfigurationPropertyDTO, bool>) (t => t.IsOnline)).ThenBy<ConfigurationPropertyDTO, string>((Func<ConfigurationPropertyDTO, string>) (t => t.DisplayName)).ToList<ConfigurationPropertyDTO>();
      return listItem;
    }

    public static ConfigurationPropertyDTO GetFormattedCOMServerItemForDropdown(
      ValueItem availableValue)
    {
      if (availableValue.AdditionalInfo == null)
        return new ConfigurationPropertyDTO()
        {
          DisplayName = string.Empty,
          Value = string.Empty,
          IsOnline = false
        };
      ConfigurationPropertyDTO serverItemForDropdown = new ConfigurationPropertyDTO();
      foreach (AdditionalInfoKey key in Enum.GetValues(typeof (AdditionalInfoKey)).Cast<AdditionalInfoKey>())
      {
        string str;
        if (availableValue.AdditionalInfo != null && availableValue.AdditionalInfo.TryGetValue(key, out str))
        {
          if (key == AdditionalInfoKey.Name)
          {
            serverItemForDropdown.DisplayName = string.Format("{0} {1}", (object) str, (object) availableValue.Value);
            serverItemForDropdown.Value = availableValue.Value;
            MessageHandler.LogDebug("Add Com server to list:" + serverItemForDropdown.DisplayName);
          }
          else if (key == AdditionalInfoKey.IsOnline)
            serverItemForDropdown.IsOnline = bool.Parse(str);
        }
      }
      return serverItemForDropdown;
    }

    private static List<ConfigurationPropertyDTO> PreparePortsForBluetoothMinoConnect(
      ChangeableParameter item)
    {
      List<ConfigurationPropertyDTO> listItem = new List<ConfigurationPropertyDTO>();
      GMMHelper.UpdateAvailableValuesForParam(item);
      if (item?.AvailableValues == null || item.AvailableValues.Count <= 0)
        return listItem;
      Dictionary<string, string> comPortsDictionary = EquipmentHelper.GetConnectedMiconsForPorts();
      if (item.AvailableValues.Any<ValueItem>((Func<ValueItem, bool>) (c => c.AdditionalInfo[AdditionalInfoKey.HardwareType] == "Bluetooth" && !comPortsDictionary.ContainsKey(c.Value))))
      {
        MSS.Business.Utils.AppContext.Current.MinoConnectDeviceNames = EquipmentHelper.DiscoverMiCons();
        comPortsDictionary = EquipmentHelper.GetConnectedMiconsForPorts();
      }
      item?.AvailableValues?.ForEach((Action<ValueItem>) (_ => listItem.Add(new ConfigurationPropertyDTO()
      {
        IsOnline = true,
        Value = _.Value,
        DisplayName = comPortsDictionary.ContainsKey(_.Value) ? string.Format("{0} {1}", (object) comPortsDictionary[_.Value], (object) _.ToString()) : string.Format("{0}", (object) _.ToString())
      })));
      return listItem;
    }

    private static List<ConfigurationPropertyDTO> PrepareDataSourceListForParam(
      ChangeableParameter item)
    {
      List<ConfigurationPropertyDTO> listItem = new List<ConfigurationPropertyDTO>();
      GMMHelper.UpdateAvailableValuesForParam(item);
      item?.AvailableValues?.ForEach((Action<ValueItem>) (_ => listItem.Add(new ConfigurationPropertyDTO()
      {
        IsOnline = true,
        DisplayName = _.ToString(),
        Value = _.Value
      })));
      return listItem;
    }

    public static Dictionary<string, string> GetConnectedMiconsForPorts()
    {
      string[] portNames = SerialPort.GetPortNames();
      List<BluetoothDeviceInfo> bluetoothDeviceInfoList = MSS.Business.Utils.AppContext.Current.MinoConnectDeviceNames ?? EquipmentHelper.DiscoverMiCons();
      Dictionary<string, string> connectedMiconsForPorts = new Dictionary<string, string>();
      if (bluetoothDeviceInfoList != null)
      {
        foreach (BluetoothDeviceInfo pairedDevice in bluetoothDeviceInfoList)
        {
          Hashtable hashtable = EquipmentHelper.BuildPortNameHash(portNames, pairedDevice);
          if (hashtable != null && hashtable.Count > 0)
          {
            foreach (DictionaryEntry dictionaryEntry in hashtable)
              connectedMiconsForPorts.Add((string) dictionaryEntry.Value, pairedDevice.DeviceName);
          }
        }
      }
      return connectedMiconsForPorts;
    }

    public static List<BluetoothDeviceInfo> DiscoverMiCons()
    {
      BluetoothClient bluetoothClient = (BluetoothClient) null;
      List<BluetoothDeviceInfo> bluetoothDeviceInfoList = (List<BluetoothDeviceInfo>) null;
      try
      {
        bluetoothClient = new BluetoothClient();
        bluetoothDeviceInfoList = ((IEnumerable<BluetoothDeviceInfo>) bluetoothClient.DiscoverDevices()).Where<BluetoothDeviceInfo>((Func<BluetoothDeviceInfo, bool>) (device => device.DeviceName.StartsWith("MiCon"))).ToList<BluetoothDeviceInfo>();
      }
      catch (Exception ex)
      {
        MessageHandler.LogException(ex);
      }
      finally
      {
        bluetoothClient?.Dispose();
      }
      return bluetoothDeviceInfoList;
    }

    private static Hashtable BuildPortNameHash(
      string[] oPortsToMap,
      BluetoothDeviceInfo pairedDevice)
    {
      Hashtable oTargetMap = new Hashtable();
      EquipmentHelper.MineRegistryForPortName("SYSTEM\\CurrentControlSet\\Enum", oTargetMap, oPortsToMap, pairedDevice.DeviceName.Substring(pairedDevice.DeviceName.LastIndexOf('_') + 1) + "_C00000000");
      return oTargetMap;
    }

    private static void MineRegistryForPortName(
      string strStartKey,
      Hashtable oTargetMap,
      string[] oPortNamesToMatch,
      string toMatchMicon)
    {
      if (oTargetMap.Count >= oPortNamesToMatch.Length)
        return;
      RegistryKey localMachine = Registry.LocalMachine;
      try
      {
        string[] subKeyNames = localMachine.OpenSubKey(strStartKey).GetSubKeyNames();
        if (((ICollection<string>) subKeyNames).Contains("Device Parameters") && strStartKey != "SYSTEM\\CurrentControlSet\\Enum")
        {
          object obj1 = Registry.GetValue("HKEY_LOCAL_MACHINE\\" + strStartKey + "\\Device Parameters", "PortName", (object) null);
          if (obj1 == null || !((ICollection<string>) oPortNamesToMatch).Contains(obj1.ToString()))
            return;
          object obj2 = Registry.GetValue("HKEY_LOCAL_MACHINE\\" + strStartKey, "FriendlyName", (object) null);
          if (!strStartKey.EndsWith(toMatchMicon))
            return;
          string key = "N/A";
          if (obj2 != null)
            key = obj2.ToString();
          if (!key.Contains(obj1.ToString()))
            key = string.Format("{0} ({1})", (object) key, obj1);
          oTargetMap[(object) key] = obj1;
        }
        else
        {
          foreach (string str in subKeyNames)
            EquipmentHelper.MineRegistryForPortName(strStartKey + "\\" + str, oTargetMap, oPortNamesToMatch, toMatchMicon);
        }
      }
      catch
      {
      }
    }
  }
}
