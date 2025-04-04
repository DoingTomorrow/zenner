// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_ScenarioManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using HandlerLib.NFC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_ScenarioManager
  {
    internal S4_DeviceMemory myDeviceMemory;
    internal NfcDeviceCommands Nfc;
    internal List<KeyValuePair<ushort, byte[]>> ConfigsFromMap;

    internal S4_ScenarioManager(S4_DeviceCommandsNFC deviceCommands, S4_DeviceMemory deviceMemory = null)
    {
      if (deviceCommands != null)
        this.Nfc = deviceCommands.CommonNfcCommands;
      this.myDeviceMemory = deviceMemory;
    }

    internal async Task<ushort[]> ReadAvailableModuleConfigurations(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] result = await this.Nfc.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetAvailableModuleConfigurations, NfcDeviceCommands.FillData);
      int configCount = result.Length / 2;
      if (configCount * 2 != result.Length)
        throw new Exception("Illegal number of result bytes");
      ushort[] scenarios = new ushort[configCount];
      int scanOffset = 0;
      for (int i = 0; i < scenarios.Length; ++i)
        scenarios[i] = ByteArrayScanner.ScanUInt16(result, ref scanOffset);
      ushort[] numArray = scenarios;
      result = (byte[]) null;
      scenarios = (ushort[]) null;
      return numArray;
    }

    internal async Task WriteCommunicationScenario(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort scenario,
      byte? modulOption = null)
    {
      byte[] data;
      if (!modulOption.HasValue)
        data = new byte[2];
      else
        data = new byte[3]
        {
          (byte) 0,
          (byte) 0,
          modulOption.Value
        };
      data[0] = (byte) scenario;
      data[1] = (byte) ((uint) scenario >> 8);
      byte[] resultAsync = await this.Nfc.SendIrCommandAndGetResultAsync(progress, cancelToken, Manufacturer_FC.SetCommunicationScenario_0x92, data);
      data = (byte[]) null;
    }

    internal async Task<ushort> ReadCommunicationScenario(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] result = await this.Nfc.SendIrCommandAndGetResultAsync(progress, cancelToken, Manufacturer_FC.GetCommunicationScenario_0x91);
      ushort num = result.Length == 2 ? BitConverter.ToUInt16(result, 0) : throw new Exception("Illegal result length");
      result = (byte[]) null;
      return num;
    }

    internal async Task<ushort> ReadCommunicationScenarioByOptions(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte moduleCode,
      byte modulOption)
    {
      byte[] data = new byte[2]{ modulOption, moduleCode };
      byte[] result = await this.Nfc.SendIrCommandAndGetResultAsync(progress, cancelToken, Manufacturer_FC.GetCommunicationScenario_0x91, data);
      ushort num = result.Length == 2 ? BitConverter.ToUInt16(result, 0) : throw new Exception("Illegal result length");
      data = (byte[]) null;
      result = (byte[]) null;
      return num;
    }

    internal async Task SetGroupsForScenario(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort scenario)
    {
      byte[] data = new byte[2]
      {
        (byte) scenario,
        (byte) ((uint) scenario >> 8)
      };
      byte[] resultAsync = await this.Nfc.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SetGroupsForScenario, data);
      data = (byte[]) null;
    }

    internal async Task DeleteAllModuleConfigurations(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.Nfc.DeleteAllModuleConfigurations(progress, cancelToken);
    }

    internal async Task WriteModuleConfiguration(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort scenario,
      byte[] config)
    {
      byte[] data = new byte[config.Length + 2];
      data[0] = (byte) scenario;
      data[1] = (byte) ((uint) scenario >> 8);
      Buffer.BlockCopy((Array) config, 0, (Array) data, 2, config.Length);
      byte[] resultAsync = await this.Nfc.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SetModuleConfiguration, data);
      data = (byte[]) null;
    }

    internal async Task DeleteScenario(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort scenario)
    {
      byte[] data = new byte[2]
      {
        (byte) scenario,
        (byte) ((uint) scenario >> 8)
      };
      byte[] resultAsync = await this.Nfc.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.SetModuleConfiguration, data);
      data = (byte[]) null;
    }

    internal async Task DeleteScenarioRange(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ScenarioRanges scenarioRange)
    {
      ushort rangeBase = (ushort) scenarioRange;
      ushort[] scenarios = await this.ReadAvailableModuleConfigurations(progress, cancelToken);
      ushort[] numArray = scenarios;
      for (int index = 0; index < numArray.Length; ++index)
      {
        ushort scenario = numArray[index];
        if ((int) scenario / 100 * 100 == (int) rangeBase)
          await this.DeleteScenario(progress, cancelToken, scenario);
      }
      numArray = (ushort[]) null;
      scenarios = (ushort[]) null;
    }

    internal async Task<byte[]> ReadModuleConfiguration(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ushort scenario,
      bool complete = true)
    {
      byte[] data = new byte[3]
      {
        (byte) scenario,
        (byte) ((uint) scenario >> 8),
        !complete ? (byte) 0 : (byte) 1
      };
      byte[] result = await this.Nfc.SendCommandAndGetResultAsync(progress, cancelToken, NfcCommands.GetModuleConfiguration, data);
      byte[] numArray = result;
      data = (byte[]) null;
      result = (byte[]) null;
      return numArray;
    }

    internal string GetAdaptedConfigurationAsText(ushort communicationScenario)
    {
      List<KeyValuePair<string, object>> configurationList = this.GetAdaptedConfigurationList(communicationScenario);
      return ScenarioConfigurations.GetConfigurationText(communicationScenario, configurationList);
    }

    internal byte[] GetAdaptedConfiguration(ushort communicationScenario)
    {
      return ScenarioConfigurations.GetConfigurationBytes(this.GetAdaptedConfigurationList(communicationScenario), firmware: this.Nfc.ConnectedDeviceVersion.FirmwareVersion.Value);
    }

    internal List<KeyValuePair<string, object>> GetAdaptedConfigurationList(
      ushort communicationScenario)
    {
      List<KeyValuePair<string, object>> adaptParams = new List<KeyValuePair<string, object>>();
      if (this.myDeviceMemory != null)
      {
        if (this.myDeviceMemory.IsParameterAvailable(S4_Params.LoRa_DevEUI))
        {
          ulong parameterValue = this.myDeviceMemory.GetParameterValue<ulong>(S4_Params.LoRa_DevEUI);
          adaptParams.Add(new KeyValuePair<string, object>("DevEUI", (object) parameterValue));
        }
        if (this.myDeviceMemory.IsParameterAvailable(S4_Params.LoRa_AppEUI))
        {
          ulong parameterValue = this.myDeviceMemory.GetParameterValue<ulong>(S4_Params.LoRa_AppEUI);
          adaptParams.Add(new KeyValuePair<string, object>("AppEUI", (object) parameterValue));
        }
        if (this.myDeviceMemory.IsParameterAvailable(S4_Params.LoRa_AppKey))
        {
          byte[] data = this.myDeviceMemory.GetData(S4_Params.LoRa_AppKey);
          adaptParams.Add(new KeyValuePair<string, object>("AppKey", (object) data));
        }
        if (this.myDeviceMemory.IsParameterAvailable(S4_Params.WMBus_AesKey))
        {
          byte[] data = this.myDeviceMemory.GetData(S4_Params.WMBus_AesKey);
          adaptParams.Add(new KeyValuePair<string, object>("AES_Key", (object) data));
        }
      }
      return ScenarioConfigurations.GetAdaptedConfigurationList(communicationScenario, adaptParams);
    }

    internal List<ushort> GetPreparedScenarios() => ScenarioConfigurations.GetPreparedScenarios();

    internal async Task ReadConfigurationFromMap(
      ProgressHandler progress,
      CancellationToken cancelToken,
      List<AddressRangeInfo> ranges = null)
    {
      uint sc_StorageAdr = this.myDeviceMemory.ScenarioConfigurationFlashRange.StartAddress;
      ushort nextConfigOffset = 0;
      ushort nextReadOffset = 0;
      AddressRange readRange = new AddressRange(sc_StorageAdr, 500U);
      this.ConfigsFromMap = new List<KeyValuePair<ushort, byte[]>>();
      while ((int) nextReadOffset <= (int) nextConfigOffset)
      {
        ranges?.Add(new AddressRangeInfo("Scenario config", readRange));
        await this.Nfc.ReadMemoryAsync(readRange, (DeviceMemory) this.myDeviceMemory, progress, cancelToken);
        readRange.StartAddress += 500U;
        nextReadOffset += (ushort) 500;
        while ((int) nextConfigOffset < (int) nextReadOffset)
        {
          ushort offsetDisplacement = this.myDeviceMemory.GetValue<ushort>(sc_StorageAdr + (uint) nextConfigOffset);
          if (offsetDisplacement != (ushort) 0)
          {
            ushort dataStartOffset = (ushort) ((uint) nextConfigOffset + 6U);
            ushort configLength = this.myDeviceMemory.GetValue<ushort>((uint) ((int) sc_StorageAdr + (int) nextConfigOffset + 2));
            ushort scenario = this.myDeviceMemory.GetValue<ushort>((uint) ((int) sc_StorageAdr + (int) nextConfigOffset + 4));
            byte[] configBytes = this.myDeviceMemory.GetData((uint) ((int) sc_StorageAdr + (int) nextConfigOffset + 4), (uint) configLength);
            nextConfigOffset += offsetDisplacement;
            this.ConfigsFromMap.Add(new KeyValuePair<ushort, byte[]>(scenario, configBytes));
            configBytes = (byte[]) null;
          }
          else
            break;
        }
      }
      readRange = (AddressRange) null;
    }

    internal bool PrepareConfigurationFromMap()
    {
      uint address = 0;
      if (this.myDeviceMemory.ScenarioConfigurationFlashRange != null)
        address = this.myDeviceMemory.ScenarioConfigurationFlashRange.StartAddress;
      if (!this.myDeviceMemory.AreDataAvailable(address, 1U))
        return false;
      ushort num1 = 0;
      this.ConfigsFromMap = new List<KeyValuePair<ushort, byte[]>>();
      while (true)
      {
        ushort num2 = this.myDeviceMemory.GetValue<ushort>(address + (uint) num1);
        if (num2 != (ushort) 0)
        {
          ushort num3 = (ushort) ((uint) num1 + 6U);
          ushort byteSize = this.myDeviceMemory.GetValue<ushort>((uint) ((int) address + (int) num1 + 2));
          ushort key = this.myDeviceMemory.GetValue<ushort>((uint) ((int) address + (int) num1 + 4));
          byte[] data = this.myDeviceMemory.GetData((uint) ((int) address + (int) num1 + 4), (uint) byteSize);
          num1 += num2;
          this.ConfigsFromMap.Add(new KeyValuePair<ushort, byte[]>(key, data));
        }
        else
          break;
      }
      return true;
    }

    internal string GetScenariosListFromMap()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("############### Scenario list form Map ###############");
      if (this.ConfigsFromMap == null)
        stringBuilder.AppendLine("Config not available");
      else if (this.ConfigsFromMap.Count == 0)
      {
        stringBuilder.AppendLine("Scenario list is empty");
      }
      else
      {
        foreach (KeyValuePair<ushort, byte[]> configsFrom in this.ConfigsFromMap)
          stringBuilder.AppendLine(ScenarioConfigurations.GetConfigurationText(configsFrom.Key, configsFrom.Value));
      }
      return stringBuilder.ToString();
    }

    internal string GetShortScenariosTextBlockFromMap()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("############### Scenario list form Map ###############");
      if (this.ConfigsFromMap == null)
        stringBuilder.AppendLine("Config not available");
      else if (this.ConfigsFromMap.Count == 0)
      {
        stringBuilder.AppendLine("Scenario list is empty");
      }
      else
      {
        foreach (KeyValuePair<ushort, byte[]> configsFrom in this.ConfigsFromMap)
          stringBuilder.AppendLine(ScenarioConfigurations.GetShortConfigurationText(configsFrom.Key, configsFrom.Value));
      }
      return stringBuilder.ToString();
    }

    internal List<KeyValuePair<string, string>> GetShortScenariosListFromMap()
    {
      List<KeyValuePair<string, string>> scenariosListFromMap = new List<KeyValuePair<string, string>>();
      if (this.ConfigsFromMap == null)
        scenariosListFromMap.Add(new KeyValuePair<string, string>("Config not available", (string) null));
      else if (this.ConfigsFromMap.Count == 0)
      {
        scenariosListFromMap.Add(new KeyValuePair<string, string>("Scenario list is empty", (string) null));
      }
      else
      {
        foreach (KeyValuePair<ushort, byte[]> configsFrom in this.ConfigsFromMap)
          scenariosListFromMap.AddRange((IEnumerable<KeyValuePair<string, string>>) ScenarioConfigurations.GetShortConfigurationList(configsFrom.Key, configsFrom.Value));
      }
      return scenariosListFromMap;
    }
  }
}
