// Decompiled with JetBrains decompiler
// Type: S4_Handler.ScenarioConfigurations
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using GmmDbLib;
using GmmDbLib.DataSets;
using HandlerLib;
using HandlerLib.NFC;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler
{
  public static class ScenarioConfigurations
  {
    internal static Logger ScenarioConfigurationsLogger = LogManager.GetLogger(nameof (ScenarioConfigurations));
    public const string KeyScenarioInfo = "ScenarioInfo";
    public const string KeyGroupInfo = "GroupInfo";
    public const string KeyModuleCode = "ModuleStructureCode";
    public const string KeyParameterVersion = "ParameterVersion";
    public const string KeyNumberOfCompatibleModuleCodes = "NumberOfCompatibleModuleCodes";
    public const string KeyCompatibleModuleCode = "CompatibleModuleCode";
    public const string KeyNumberOfModuleSetupBytes = "NumberOfModuleSetupBytes";
    public const string KeyNumberOfGroups = "NumberOfGroups";
    public const string KeyNumberOfDifVifBytes = "NumberOfDifVifBytes";
    public const string KeyNumberOfGroupParameters = "NumberOfGroupParameters";
    public const string KeyDifVif = "DifVif";
    public const string KeyDevEUI = "DevEUI";
    public const string KeyAppEUI = "AppEUI";
    public const string KeyAppKey = "AppKey";
    public const string KeyMinolDeviceType = "MinolDeviceType";
    public const string KeyModeOfOperation = "ModeOfOperation";
    public const string KeyFrameFormat = "FrameFormat";
    public const string KeySecurityMode = "SecurityMode";
    public const string KeyHeaderFormat = "HeaderFormat";
    public const string KeyFrequency = "Frequency";
    public const string KeyFrequencyBand = "FrequencyBand";
    public const string KeyAES_Key = "AES_Key";
    public const string KeyTransmitionCycleSeconds = "TransmitionCycleSeconds";
    public const string KeyNfcCycleSeconds = "NfcCycleSeconds";
    public const string KeyRadioOnSelection = "RadioOnSelection";
    private static Dictionary<ushort, List<KeyValuePair<string, object>>> AllConfigurations;
    private static SortedList<ushort, ScenarioEnergy> ScenarioEnergyInfos = new SortedList<ushort, ScenarioEnergy>();

    static ScenarioConfigurations()
    {
      ushort key1 = 201;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key1, new ScenarioEnergy("LoRa monthly")
      {
        PacketBuilding_ms = 10.0,
        SX1276_Setup_ms = 2.0,
        Tx_Duration_ms = 1400.0,
        Rx_Duration_ms = 1400.0,
        PacketBuilding_mA = 0.55,
        SX1276_Setup_mA = 2.4,
        Tx_Duration_mA = 30.5,
        Rx_Duration_mA = 13.0,
        PacketsPerDay = 16.0 / 31.0
      });
      ushort key2 = 202;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key2, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 201].Clone("LoRa daily"));
      ScenarioConfigurations.ScenarioEnergyInfos[key2].PacketsPerDay = 69.0 / 31.0;
      ushort key3 = 203;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key3, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 201].Clone("LoRa 3 hourly"));
      ScenarioConfigurations.ScenarioEnergyInfos[key3].PacketsPerDay = (double) byte.MaxValue / 31.0;
      ushort key4 = 204;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key4, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 201].Clone("LoRa hourly"));
      ScenarioConfigurations.ScenarioEnergyInfos[key4].PacketsPerDay = 751.0 / 31.0;
      ushort key5 = 205;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key5, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 201].Clone("LoRa monthly async alarm"));
      ushort key6 = 206;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key6, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 202].Clone("LoRa daily async alarm"));
      ushort key7 = 312;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key7, new ScenarioEnergy("wMBus walkby logger")
      {
        PacketBuilding_ms = 8.0,
        SX1276_Setup_ms = 6.0,
        Tx_Duration_ms = 11.0,
        PacketBuilding_mA = 1.2,
        SX1276_Setup_mA = 2.4,
        Tx_Duration_mA = 30.0,
        CycleSeconds = 120.0
      });
      ushort key8 = 313;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key8, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 312].Clone("wMBus walkby 20s C1_A_5"));
      ScenarioConfigurations.ScenarioEnergyInfos[key8].CycleSeconds = 20.0;
      ScenarioConfigurations.ScenarioEnergyInfos[key8].Tx_Duration_ms = 5.3;
      ushort key9 = 314;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key9, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 313].Clone("wMBus walkby 20s C1_A_7"));
      ScenarioConfigurations.ScenarioEnergyInfos[key9].Tx_Duration_ms = 7.1;
      ushort key10 = 315;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key10, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 313].Clone("wMBus hourly"));
      ScenarioConfigurations.ScenarioEnergyInfos[key10].PacketsPerDay = 24.0;
      ScenarioConfigurations.ScenarioEnergyInfos[key10].Tx_Duration_ms = 7.4;
      ushort key11 = 316;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key11, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 315].Clone("wMBus hourly obsolete"));
      ushort key12 = 317;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key12, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 315].Clone("wMBus 15 min"));
      ScenarioConfigurations.ScenarioEnergyInfos[key12].CycleSeconds = 900.0;
      ushort key13 = 318;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key13, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 314].Clone("wMBus 300s"));
      ScenarioConfigurations.ScenarioEnergyInfos[key13].CycleSeconds = 300.0;
      ScenarioConfigurations.ScenarioEnergyInfos[key13].Tx_Duration_ms = 6.5;
      ushort key14 = 319;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key14, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 318].Clone("wMBus 432s"));
      ScenarioConfigurations.ScenarioEnergyInfos[key14].CycleSeconds = 432.0;
      ushort key15 = 320;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key15, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 312].Clone("wMBus 20s month values"));
      ScenarioConfigurations.ScenarioEnergyInfos[key15].CycleSeconds = 20.0;
      ushort key16 = 321;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key16, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 313].Clone("wMBus 20s current values C1_A_5"));
      ScenarioConfigurations.ScenarioEnergyInfos[key16].Tx_Duration_ms = 7.4;
      ushort key17 = 333;
      ScenarioConfigurations.ScenarioEnergyInfos.Add(key17, ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 321].Clone("wMBus based on SC321 but 180s current values C1_A_5"));
      ScenarioConfigurations.ScenarioEnergyInfos[key17].CycleSeconds = 180.0;
      ScenarioConfigurations.AllConfigurations = new Dictionary<ushort, List<KeyValuePair<string, object>>>();
      try
      {
        ScenarioConfigurations.LoadAllScenariosFromDatabase();
      }
      catch
      {
      }
    }

    private static List<KeyValuePair<string, object>> NewScenario(ushort scenarioNumber)
    {
      List<KeyValuePair<string, object>> keyValuePairList = new List<KeyValuePair<string, object>>();
      ScenarioConfigurations.AllConfigurations.Add(scenarioNumber, keyValuePairList);
      return keyValuePairList;
    }

    private static List<S4_DifVif_Parameter> NewParameterGroup(
      List<List<S4_DifVif_Parameter>> parameterGroups)
    {
      List<S4_DifVif_Parameter> s4DifVifParameterList = new List<S4_DifVif_Parameter>();
      parameterGroups.Add(s4DifVifParameterList);
      return s4DifVifParameterList;
    }

    private static void UpdateNumberOfModuleSetupBytes(List<KeyValuePair<string, object>> theConfig)
    {
      int index;
      for (index = theConfig.Count - 1; theConfig[index].Key != "NumberOfModuleSetupBytes"; --index)
      {
        if (index < 0)
          throw new Exception("KeyNumberOfModuleSetupBytes not found");
      }
      byte[] configurationBytes = ScenarioConfigurations.GetConfigurationBytes(theConfig, index + 1);
      theConfig.RemoveAt(index);
      if (configurationBytes.Length == 0)
        return;
      theConfig.Insert(index, new KeyValuePair<string, object>("NumberOfModuleSetupBytes", (object) (byte) configurationBytes.Length));
    }

    private static void AddGroups(
      List<KeyValuePair<string, object>> theConfig,
      List<List<S4_DifVif_Parameter>> parameterGroups)
    {
      theConfig.Add(new KeyValuePair<string, object>("NumberOfGroups", (object) (byte) parameterGroups.Count));
      foreach (List<S4_DifVif_Parameter> parameterGroup in parameterGroups)
      {
        int num = 0;
        foreach (S4_DifVif_Parameter s4DifVifParameter in parameterGroup)
          num += s4DifVifParameter.GetDifVifBytes().Count;
        theConfig.Add(new KeyValuePair<string, object>("NumberOfDifVifBytes", (object) (byte) num));
        theConfig.Add(new KeyValuePair<string, object>("NumberOfGroupParameters", (object) (byte) parameterGroup.Count));
        foreach (S4_DifVif_Parameter s4DifVifParameter in parameterGroup)
          theConfig.Add(new KeyValuePair<string, object>("DifVif", (object) s4DifVifParameter));
      }
    }

    public static CommunicationScenarioRange GetCommunicationScenarioRange(ushort scenario)
    {
      if (scenario == (ushort) 0)
        return CommunicationScenarioRange.Non;
      if (scenario <= (ushort) 99)
        return CommunicationScenarioRange.unknown;
      if (scenario <= (ushort) 199)
        return CommunicationScenarioRange.radio3;
      if (scenario <= (ushort) 299)
        return CommunicationScenarioRange.LoRa;
      return scenario <= (ushort) 399 ? CommunicationScenarioRange.wMBus : CommunicationScenarioRange.unknown;
    }

    public static ScenarioEnergy GetScenarioEnergyValues(ushort scenario)
    {
      ScenarioEnergy scenarioEnergyValues;
      if (ScenarioConfigurations.ScenarioEnergyInfos.ContainsKey(scenario))
        scenarioEnergyValues = ScenarioConfigurations.ScenarioEnergyInfos[scenario];
      else if ((int) scenario % 100 == 0)
      {
        scenarioEnergyValues = new ScenarioEnergy("Radio off");
      }
      else
      {
        switch (ScenarioConfigurations.GetCommunicationScenarioRange(scenario))
        {
          case CommunicationScenarioRange.LoRa:
            ScenarioConfigurations.ScenarioConfigurationsLogger.Warn("Scenario " + scenario.ToString() + "LoRa default ScenarioEnergy used");
            scenarioEnergyValues = ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 204];
            break;
          case CommunicationScenarioRange.wMBus:
            ScenarioConfigurations.ScenarioConfigurationsLogger.Warn("Scenario " + scenario.ToString() + "wMBus default ScenarioEnergy used");
            scenarioEnergyValues = ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 320];
            break;
          default:
            ScenarioConfigurations.ScenarioConfigurationsLogger.Warn("Scenario " + scenario.ToString() + "ScenarioEnergy estimation without data");
            scenarioEnergyValues = ScenarioConfigurations.ScenarioEnergyInfos[(ushort) 320];
            break;
        }
      }
      object scenarioParameterValue = ScenarioConfigurations.GetScenarioParameterValue(scenario, "TransmitionCycleSeconds");
      if (scenarioParameterValue != null)
      {
        double num = (double) (uint) scenarioParameterValue;
        if (num != scenarioEnergyValues.CycleSeconds)
        {
          ScenarioConfigurations.ScenarioConfigurationsLogger.Warn("CycleSeconds changed to: " + num.ToString());
          scenarioEnergyValues.CycleSeconds = num;
        }
      }
      return scenarioEnergyValues;
    }

    public static void DeleteAllPreparedScenarios()
    {
      ScenarioConfigurations.AllConfigurations.Clear();
    }

    public static void DeletePreparedScenario(ushort scenario)
    {
      if (!ScenarioConfigurations.AllConfigurations.ContainsKey(scenario))
        return;
      ScenarioConfigurations.AllConfigurations.Remove(scenario);
    }

    public static List<ushort> GetPreparedScenarios()
    {
      return ScenarioConfigurations.AllConfigurations.Keys.ToList<ushort>();
    }

    public static object GetScenarioParameterValue(ushort scenario, string parameterName)
    {
      if (!ScenarioConfigurations.AllConfigurations.ContainsKey(scenario))
        return (object) null;
      foreach (KeyValuePair<string, object> keyValuePair in ScenarioConfigurations.AllConfigurations[scenario])
      {
        if (keyValuePair.Key == parameterName)
          return keyValuePair.Value;
      }
      return (object) null;
    }

    public static List<KeyValuePair<string, object>> GetConfigurationList(
      ushort communicationScenario)
    {
      return ScenarioConfigurations.AllConfigurations.ContainsKey(communicationScenario) ? ScenarioConfigurations.AllConfigurations[communicationScenario] : throw new Exception("Scenario not supported: " + communicationScenario.ToString());
    }

    public static List<KeyValuePair<string, object>> GetAdaptedConfigurationList(
      ushort communicationScenario,
      List<KeyValuePair<string, object>> adaptParams)
    {
      List<KeyValuePair<string, object>> configurationList1 = ScenarioConfigurations.GetConfigurationList(communicationScenario);
      List<KeyValuePair<string, object>> configurationList2 = new List<KeyValuePair<string, object>>();
      KeyValuePair<string, object> keyValuePair1 = new KeyValuePair<string, object>();
      foreach (KeyValuePair<string, object> keyValuePair2 in configurationList1)
      {
        KeyValuePair<string, object> param = keyValuePair2;
        KeyValuePair<string, object> keyValuePair3 = adaptParams.FirstOrDefault<KeyValuePair<string, object>>((System.Func<KeyValuePair<string, object>, bool>) (x => x.Key == param.Key));
        if (keyValuePair3.Key == keyValuePair1.Key)
          configurationList2.Add(param);
        else
          configurationList2.Add(keyValuePair3);
      }
      return configurationList2;
    }

    public static byte[] GetConfiguration(ushort communicationScenario)
    {
      return ScenarioConfigurations.GetConfigurationBytes(ScenarioConfigurations.GetConfigurationList(communicationScenario));
    }

    public static byte[] GetConfigurationBytes(
      List<KeyValuePair<string, object>> theConfig,
      int startIndex = 0,
      uint firmware = 0)
    {
      List<byte> byteList = new List<byte>();
      for (int index = startIndex; index < theConfig.Count; ++index)
      {
        KeyValuePair<string, object> keyValuePair = theConfig[index];
        if (!(keyValuePair.Key == "ScenarioInfo") && !(keyValuePair.Key == "GroupInfo"))
        {
          if (keyValuePair.Value is byte[])
            byteList.AddRange((IEnumerable<byte>) (byte[]) keyValuePair.Value);
          else if (keyValuePair.Value is S4_DifVif_Parameter)
          {
            S4_DifVif_Parameter s4DifVifParameter = (S4_DifVif_Parameter) keyValuePair.Value;
            List<byte> difVifBytes = s4DifVifParameter.GetDifVifBytes();
            if (firmware > 0U)
            {
              FirmwareVersion firmwareVersion = new FirmwareVersion(firmware);
              if (firmwareVersion <= (object) "1.5.2 IUW" && (s4DifVifParameter.StorageNumber == 30 || s4DifVifParameter.StorageNumber == 31) && s4DifVifParameter.SpacingControl > (byte) 0)
                return new byte[0];
              if (difVifBytes.Count == 8 && firmwareVersion == (object) "1.5.1 IUW" && difVifBytes[6] == (byte) 52)
                difVifBytes[6] = (byte) 50;
              if (s4DifVifParameter.Name == "Volume, logger, last month" && firmwareVersion < (object) "1.7.1 IUW")
                difVifBytes = S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "Volume, last month")).GetDifVifBytes();
            }
            byteList.AddRange((IEnumerable<byte>) difVifBytes);
          }
          else if (keyValuePair.Value is RadioOffTimeManagement)
            byteList.AddRange((IEnumerable<byte>) ((RadioOffTimeManagement) keyValuePair.Value).GetConfigBytes());
          else if (keyValuePair.Value is byte)
            byteList.Add((byte) keyValuePair.Value);
          else if (keyValuePair.Value is ushort)
            byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) keyValuePair.Value));
          else if (keyValuePair.Value is uint)
          {
            byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes((uint) keyValuePair.Value));
          }
          else
          {
            if (!(keyValuePair.Value is ulong))
              throw new Exception("Unknown config data");
            byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ulong) keyValuePair.Value));
          }
        }
      }
      return byteList.ToArray();
    }

    public static byte[] AddScenarioAsDataHeader(ushort communicationScenario, byte[] data)
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(communicationScenario));
      byteList.AddRange((IEnumerable<byte>) data);
      return byteList.ToArray();
    }

    public static string GetPreparedConfigurationsAsText()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Configurations for following scenarios are prepared:");
      stringBuilder.AppendLine();
      foreach (KeyValuePair<ushort, List<KeyValuePair<string, object>>> allConfiguration in ScenarioConfigurations.AllConfigurations)
      {
        stringBuilder.Append(allConfiguration.Key.ToString());
        if (allConfiguration.Value.Count > 0 && allConfiguration.Value[0].Key == "ScenarioInfo")
          stringBuilder.Append("; -> " + (string) allConfiguration.Value[0].Value);
        stringBuilder.AppendLine();
      }
      return stringBuilder.ToString();
    }

    public static string GetConfigurationText(ushort communicationScenario)
    {
      List<KeyValuePair<string, object>> configurationList = ScenarioConfigurations.GetConfigurationList(communicationScenario);
      return ScenarioConfigurations.GetConfigurationText(communicationScenario, configurationList);
    }

    public static string GetConfigurationText(
      ushort communicationScenario,
      List<KeyValuePair<string, object>> theConfig)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        stringBuilder.AppendLine("Communication scenario: " + communicationScenario.ToString());
        stringBuilder.AppendLine();
        foreach (KeyValuePair<string, object> keyValuePair in theConfig)
        {
          if (keyValuePair.Key == "ScenarioInfo")
          {
            stringBuilder.AppendLine("ScenarioInfo: " + (string) keyValuePair.Value);
            stringBuilder.AppendLine();
          }
          else
          {
            if (keyValuePair.Key == "NumberOfGroups" || keyValuePair.Key == "NumberOfModuleSetupBytes")
              stringBuilder.AppendLine();
            stringBuilder.Append(keyValuePair.Key + ": ");
            if (keyValuePair.Key == "ModuleStructureCode")
            {
              if (Enum.IsDefined(typeof (NDC_ModuleConfiguration), keyValuePair.Value))
                stringBuilder.Append(((byte) keyValuePair.Value).ToString() + " = " + ((NDC_ModuleConfiguration) keyValuePair.Value).ToString());
              else
                stringBuilder.Append(((byte) keyValuePair.Value).ToString() + " = unknown ModuleConfiguration");
            }
            else if (keyValuePair.Key == "NumberOfModuleSetupBytes" || keyValuePair.Key == "NumberOfGroups" || keyValuePair.Key == "NumberOfDifVifBytes")
              stringBuilder.Append(((byte) keyValuePair.Value).ToString());
            else if (keyValuePair.Key == "ModeOfOperation")
              stringBuilder.Append(((char) (byte) (ushort) keyValuePair.Value).ToString() + (object) (char) (byte) ((uint) (ushort) keyValuePair.Value >> 8));
            else if (keyValuePair.Key == "FrameFormat" || keyValuePair.Key == "SecurityMode" || keyValuePair.Key == "HeaderFormat" || keyValuePair.Key == "FrequencyBand")
              stringBuilder.Append((char) (byte) keyValuePair.Value);
            else if (keyValuePair.Value is string)
              stringBuilder.Append((string) keyValuePair.Value);
            else if (keyValuePair.Value is byte)
              stringBuilder.Append(((byte) keyValuePair.Value).ToString() + " = 0x" + ((byte) keyValuePair.Value).ToString("x02"));
            else if (keyValuePair.Value is byte[])
              stringBuilder.Append(Util.ByteArrayToHexString((byte[]) keyValuePair.Value));
            else if (keyValuePair.Value is S4_DifVif_Parameter)
              stringBuilder.Append(" -> " + ((S4_DifVif_Parameter) keyValuePair.Value).ToString());
            else if (keyValuePair.Value is RadioOffTimeManagement)
              stringBuilder.Append(" -> " + ((RadioOffTimeManagement) keyValuePair.Value).ToString());
            else if (keyValuePair.Value is byte)
              stringBuilder.Append(((byte) keyValuePair.Value).ToString() + " = 0x" + ((byte) keyValuePair.Value).ToString("x02"));
            else if (keyValuePair.Value is ushort)
              stringBuilder.Append(((ushort) keyValuePair.Value).ToString() + " = 0x" + ((ushort) keyValuePair.Value).ToString("x04"));
            else if (keyValuePair.Value is uint)
            {
              stringBuilder.Append(((uint) keyValuePair.Value).ToString() + " = 0x" + ((uint) keyValuePair.Value).ToString("x08"));
            }
            else
            {
              if (!(keyValuePair.Value is ulong))
                throw new Exception("Unsupported type for: " + keyValuePair.Key);
              stringBuilder.Append(((ulong) keyValuePair.Value).ToString() + " = 0x" + ((ulong) keyValuePair.Value).ToString("x016"));
            }
            stringBuilder.AppendLine();
          }
        }
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("******************************************");
        stringBuilder.AppendLine("*** Configuration bytes:");
        byte[] configurationBytes = ScenarioConfigurations.GetConfigurationBytes(theConfig);
        if (configurationBytes.Length != 0)
        {
          byte[] numArray = ScenarioConfigurations.AddScenarioAsDataHeader(communicationScenario, configurationBytes);
          stringBuilder.AppendLine(Util.ByteArrayToHexString(numArray));
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("******************************************");
          stringBuilder.AppendLine("*** Configuration text:");
          stringBuilder.AppendLine(ScenarioConfigurations.GetConfigurationText(communicationScenario, numArray));
        }
        else
          stringBuilder.AppendLine("Not supported for this firmware");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      return stringBuilder.ToString();
    }

    public static string GetConfigurationText(
      ushort communicationScenario,
      byte[] configData,
      bool? onlySetup = null)
    {
      string str = "   ";
      string indent = str + str;
      if (configData == null || configData.Length < 5)
        throw new Exception("Config data to short");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("CommunicationScenario: " + communicationScenario.ToString());
      stringBuilder.AppendLine();
      int num1 = 0;
      if ((int) ByteArrayScanner.ScanUInt16(configData, ref num1) != (int) communicationScenario)
        throw new Exception("Received scenario not equal requested scenario");
      NDC_ModuleConfiguration moduleConfiguration = (NDC_ModuleConfiguration) ByteArrayScanner.ScanByte(configData, ref num1);
      stringBuilder.AppendLine(str + "ModuleStructureCode: " + ((byte) moduleConfiguration).ToString() + " = " + moduleConfiguration.ToString());
      byte parameterStructVersion = ByteArrayScanner.ScanByte(configData, ref num1);
      stringBuilder.AppendLine(str + "ParameterVersion:" + parameterStructVersion.ToString());
      if (!onlySetup.HasValue || !onlySetup.Value)
      {
        byte num2 = ByteArrayScanner.ScanByte(configData, ref num1);
        stringBuilder.AppendLine(str + "NumberOfCompatibleModuleCodes:" + num2.ToString());
        for (byte index = 0; (int) index < (int) num2; ++index)
          stringBuilder.AppendLine(indent + "CompatibleModuleCode:" + ByteArrayScanner.ScanByte(configData, ref num1).ToString());
      }
      byte num3 = ByteArrayScanner.ScanByte(configData, ref num1);
      stringBuilder.AppendLine(str + "NumberOfModuleSetupBytes:" + num3.ToString());
      if (num3 > (byte) 0)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(str + "Module setup:");
        if (moduleConfiguration == NDC_ModuleConfiguration.NDC_wMBus_Configuration)
        {
          if (parameterStructVersion < (byte) 2)
          {
            stringBuilder.Append("Old, not supported wMBus configuration");
            return stringBuilder.ToString();
          }
          stringBuilder.Append(new MBusScenarioConfig(configData, parameterStructVersion, ref num1).ToTextBlock(indent));
        }
        else if (moduleConfiguration == NDC_ModuleConfiguration.NDC_LoRa_Configuration)
          stringBuilder.Append(new LoRaScenarioConfig(configData, parameterStructVersion, ref num1).ToTextBlock(indent));
        else if (moduleConfiguration == NDC_ModuleConfiguration.NDC_LoRa_Configuration)
        {
          stringBuilder.AppendLine("First and redefined illegal wMBus configuration.");
        }
        else
        {
          stringBuilder.Append(indent);
          for (int index = 0; index < (int) num3; ++index)
          {
            if (num1 >= configData.Length)
              throw new Exception("Illegal byte length");
            byte num4 = ByteArrayScanner.ScanByte(configData, ref num1);
            stringBuilder.Append(num4.ToString("x02"));
          }
          stringBuilder.AppendLine();
        }
      }
      if (num1 < configData.Length)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(str + "Group definitions DifVif's:");
        byte num5 = ByteArrayScanner.ScanByte(configData, ref num1);
        stringBuilder.AppendLine(str + "NumberOfGroups: " + num5.ToString());
        for (int index1 = 0; index1 < (int) num5; ++index1)
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendLine(str + "Group: " + index1.ToString());
          if (num1 >= configData.Length)
            throw new Exception("NumberOfDifVifBytes not available for group: " + index1.ToString());
          byte num6 = ByteArrayScanner.ScanByte(configData, ref num1);
          stringBuilder.AppendLine(str + "NumberOfDifVifBytes : " + num6.ToString());
          byte num7 = ByteArrayScanner.ScanByte(configData, ref num1);
          stringBuilder.AppendLine(str + "NumberOfDifVifParameters : " + num7.ToString());
          int offset = num1;
          stringBuilder.Append(indent + "DifVif: ");
          for (int index2 = 0; index2 < (int) num6; ++index2)
          {
            if (num1 >= configData.Length)
              throw new Exception("Illegal DifVif byte length: " + index1.ToString());
            byte num8 = ByteArrayScanner.ScanByte(configData, ref num1);
            stringBuilder.Append(num8.ToString("x02"));
          }
          stringBuilder.AppendLine();
          if (num7 > (byte) 0)
          {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(str + "Interpreted DifVif parameters:");
            for (int index3 = 0; index3 < (int) num7; ++index3)
            {
              S4_DifVif_Parameter parametersFromDifVifBytes = S4_DifVif_Parameter.GetParametersFromDifVifBytes(configData, ref offset);
              stringBuilder.AppendLine(indent + parametersFromDifVifBytes.ToString());
            }
          }
        }
      }
      else
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("*** No group definitions available ***");
      }
      if (num1 != configData.Length)
        stringBuilder.AppendLine("!!! Remaining bytes after scanning complete configuration. !!");
      return stringBuilder.ToString();
    }

    public static string GetShortConfigurationText(
      ushort communicationScenario,
      byte[] configData,
      bool? onlySetup = null)
    {
      string str1 = "   ";
      if (configData == null || configData.Length < 5)
        throw new Exception("Config data to short");
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("CommunicationScenario: " + communicationScenario.ToString());
      int offset = 0;
      if ((int) ByteArrayScanner.ScanUInt16(configData, ref offset) != (int) communicationScenario)
        throw new Exception("Scenario not equal requested scenario");
      NDC_ModuleTypes ndcModuleTypes = (NDC_ModuleTypes) ByteArrayScanner.ScanByte(configData, ref offset);
      StringBuilder stringBuilder2 = stringBuilder1;
      string[] strArray = new string[5]
      {
        str1,
        "ModuleStructureCode: ",
        null,
        null,
        null
      };
      byte num1 = (byte) ndcModuleTypes;
      strArray[2] = num1.ToString();
      strArray[3] = " = ";
      strArray[4] = ndcModuleTypes.ToString();
      string str2 = string.Concat(strArray);
      stringBuilder2.AppendLine(str2);
      byte num2 = ByteArrayScanner.ScanByte(configData, ref offset);
      stringBuilder1.AppendLine(str1 + "ParameterVersion:" + num2.ToString());
      if (!onlySetup.HasValue || !onlySetup.Value)
      {
        byte num3 = ByteArrayScanner.ScanByte(configData, ref offset);
        stringBuilder1.Append(str1 + "NumberOfCompatibleModuleCodes:" + num3.ToString() + " ;");
        for (byte index = 0; (int) index < (int) num3; ++index)
        {
          StringBuilder stringBuilder3 = stringBuilder1;
          num1 = ByteArrayScanner.ScanByte(configData, ref offset);
          string str3 = " " + num1.ToString();
          stringBuilder3.Append(str3);
        }
      }
      byte num4 = ByteArrayScanner.ScanByte(configData, ref offset);
      stringBuilder1.AppendLine(str1 + "NumberOfModuleSetupBytes:" + num4.ToString());
      return stringBuilder1.ToString();
    }

    public static List<KeyValuePair<string, string>> GetShortConfigurationList(
      ushort communicationScenario,
      byte[] configData,
      bool? onlySetup = null)
    {
      string str1 = "-> ";
      if (configData == null || configData.Length < 5)
        throw new Exception("Config data to short");
      List<KeyValuePair<string, string>> configurationList = new List<KeyValuePair<string, string>>();
      configurationList.Add(new KeyValuePair<string, string>("Scenario", communicationScenario.ToString()));
      int offset = 0;
      if ((int) ByteArrayScanner.ScanUInt16(configData, ref offset) != (int) communicationScenario)
        throw new Exception("Scenario not equal requested scenario");
      NDC_ModuleTypes ndcModuleTypes = (NDC_ModuleTypes) ByteArrayScanner.ScanByte(configData, ref offset);
      configurationList.Add(new KeyValuePair<string, string>(str1 + "ModuleStructureCode", ndcModuleTypes.ToString()));
      byte num1 = ByteArrayScanner.ScanByte(configData, ref offset);
      configurationList.Add(new KeyValuePair<string, string>(str1 + "ParameterVersion", num1.ToString()));
      if (!onlySetup.HasValue || !onlySetup.Value)
      {
        byte num2 = ByteArrayScanner.ScanByte(configData, ref offset);
        string str2 = "";
        for (byte index = 0; (int) index < (int) num2; ++index)
          str2 = str2 + " " + ByteArrayScanner.ScanByte(configData, ref offset).ToString();
        configurationList.Add(new KeyValuePair<string, string>(str1 + "CompatibleModuleCodes", str2));
      }
      byte num3 = ByteArrayScanner.ScanByte(configData, ref offset);
      configurationList.Add(new KeyValuePair<string, string>(str1 + "NumberOfModuleSetupBytes", num3.ToString()));
      return configurationList;
    }

    public static ushort AddScenarioFromFile(string fileContent)
    {
      return ScenarioConfigurations.AddScenarioFromFileContent(ScenarioConfigurations.AllConfigurations, fileContent);
    }

    public static ushort AddScenarioFromFileContent(
      Dictionary<ushort, List<KeyValuePair<string, object>>> AllConfigurations,
      string fileContent)
    {
      ushort key = 0;
      List<KeyValuePair<string, object>> theConfig = new List<KeyValuePair<string, object>>();
      List<List<S4_DifVif_Parameter>> parameterGroups = new List<List<S4_DifVif_Parameter>>();
      List<S4_DifVif_Parameter> s4DifVifParameterList = (List<S4_DifVif_Parameter>) null;
      List<byte> byteList = (List<byte>) null;
      string str1 = fileContent;
      string[] separator = new string[1]
      {
        Environment.NewLine
      };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        int length = str2.IndexOf("//");
        string str3 = (length < 0 ? str2 : str2.Substring(0, length)).Trim();
        try
        {
          string[] lineParts1 = str3.Split('>');
          if (lineParts1.Length != 0)
          {
            string[] strArray = lineParts1[0].Split(new char[1]
            {
              '<'
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length != 0)
            {
              if (strArray.Length == 1)
              {
                if (strArray[0] == "ModuleSetup")
                {
                  if (byteList == null || byteList.Count < 1)
                    throw new Exception("CompatibleModuleCodes not defined");
                  theConfig.Add(new KeyValuePair<string, object>("NumberOfCompatibleModuleCodes", (object) (byte) byteList.Count));
                  foreach (byte num in byteList)
                    theConfig.Add(new KeyValuePair<string, object>("CompatibleModuleCode", (object) num));
                  theConfig.Add(new KeyValuePair<string, object>("NumberOfModuleSetupBytes", (object) (byte) 0));
                  continue;
                }
                if (strArray[0] == "EndModuleSetup")
                {
                  ScenarioConfigurations.UpdateNumberOfModuleSetupBytes(theConfig);
                  continue;
                }
                if (strArray[0] == "ParameterGroup")
                {
                  s4DifVifParameterList = new List<S4_DifVif_Parameter>();
                  parameterGroups.Add(s4DifVifParameterList);
                  continue;
                }
                if (!(strArray[0] == "DifVif"))
                  throw new Exception("Illegal format key: " + str3);
                if (lineParts1.Length != 2)
                  throw new Exception("No DifVif data found" + str3);
                s4DifVifParameterList.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == lineParts1[1].Trim())) ?? throw new Exception("Pre defined DifVif not found" + str3));
                continue;
              }
              if (strArray.Length != 2)
                throw new Exception("No key found" + str3);
              string data = lineParts1.Length == 2 ? lineParts1[1].Trim() : throw new Exception("No data found" + str3);
              string str4 = strArray[1];
              if (str4 != null)
              {
                object obj;
                switch (str4.Length)
                {
                  case 4:
                    switch (str4[0])
                    {
                      case 'b':
                        if (str4 == "byte")
                        {
                          obj = (object) byte.Parse(data);
                          if (byteList == null && strArray[0] == "ParameterVersion")
                          {
                            byteList = new List<byte>();
                            break;
                          }
                          if (strArray[0] == "CompatibleModuleCode")
                          {
                            if (byteList == null)
                              throw new Exception("Illegal CompatibleModuleCode definition");
                            byteList.Add((byte) obj);
                            continue;
                          }
                          break;
                        }
                        goto label_61;
                      case 'u':
                        if (str4 == "uint")
                        {
                          obj = (object) uint.Parse(data);
                          break;
                        }
                        goto label_61;
                      default:
                        goto label_61;
                    }
                    break;
                  case 5:
                    switch (str4[0])
                    {
                      case 'A':
                        if (str4 == "ASCII")
                        {
                          if (data.Length == 1)
                          {
                            obj = (object) (byte) data[0];
                            break;
                          }
                          if (data.Length != 2)
                            throw new Exception("Not supported ASCII length" + str3);
                          obj = (object) (ushort) ((uint) (byte) data[0] + ((uint) (byte) data[1] << 8));
                          break;
                        }
                        goto label_61;
                      case 'u':
                        if (str4 == "ulong")
                        {
                          obj = (object) ulong.Parse(data);
                          break;
                        }
                        goto label_61;
                      default:
                        goto label_61;
                    }
                    break;
                  case 6:
                    switch (str4[0])
                    {
                      case 'b':
                        if (str4 == "byte[]")
                        {
                          obj = (object) Util.HexStringToByteArray(data);
                          break;
                        }
                        goto label_61;
                      case 's':
                        if (str4 == "string")
                        {
                          obj = (object) data;
                          break;
                        }
                        goto label_61;
                      case 'u':
                        if (str4 == "ushort")
                        {
                          obj = (object) ushort.Parse(data);
                          break;
                        }
                        goto label_61;
                      default:
                        goto label_61;
                    }
                    break;
                  case 19:
                    if (str4 == "S4_DifVif_Parameter")
                    {
                      obj = (object) S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == data));
                      break;
                    }
                    goto label_61;
                  case 22:
                    if (str4 == "RadioOffTimeManagement")
                    {
                      obj = (object) new RadioOffTimeManagement(data);
                      break;
                    }
                    goto label_61;
                  default:
                    goto label_61;
                }
                if (strArray[0] == "Scenario")
                {
                  key = (ushort) obj;
                  goto label_66;
                }
                else
                {
                  theConfig.Add(new KeyValuePair<string, object>(strArray[0], obj));
                  goto label_66;
                }
              }
label_61:
              throw new Exception("Illegal type" + str3);
            }
            continue;
          }
          continue;
        }
        catch (Exception ex)
        {
          throw new Exception("Illegal line: " + str3, ex);
        }
label_66:;
      }
      ScenarioConfigurations.AddGroups(theConfig, parameterGroups);
      if (AllConfigurations.ContainsKey(key))
        AllConfigurations.Remove(key);
      AllConfigurations.Add(key, theConfig);
      return key;
    }

    public static void WriteScenarioToDatabase(ushort scenario, string scenarioSource)
    {
      using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" SELECT * ");
        stringBuilder.Append(" FROM ScenarioDefinition");
        stringBuilder.Append(" WHERE Scenario = " + scenario.ToString());
        DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection, out DbCommandBuilder _);
        HandlerTables.ScenarioDefinitionDataTable definitionDataTable = new HandlerTables.ScenarioDefinitionDataTable();
        dataAdapter.Fill((DataTable) definitionDataTable);
        HandlerTables.ScenarioDefinitionRow row;
        if (definitionDataTable.Count == 0)
        {
          row = definitionDataTable.NewScenarioDefinitionRow();
          row.Scenario = (int) scenario;
          definitionDataTable.AddScenarioDefinitionRow(row);
        }
        else
          row = definitionDataTable[0];
        row.ScenarioSource = scenarioSource;
        row.GroupNames = ";IUW;";
        dataAdapter.Update((DataTable) definitionDataTable);
      }
    }

    public static string LoadAllScenariosFromDatabase()
    {
      return ScenarioConfigurations.LoadSelectedScenariosFromDatabase("IUW", "IUWO");
    }

    public static string LoadSelectedScenariosFromDatabase(string groupName, string loadOrderKey)
    {
      ScenarioConfigurations.AllConfigurations.Clear();
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("Loaded scenarios:");
      stringBuilder1.AppendLine();
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.Append(" SELECT * ");
          stringBuilder2.Append(" FROM ScenarioDefinition");
          stringBuilder2.Append(" WHERE GroupNames LIKE '%;" + groupName + ";%'");
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder2.ToString(), newConnection);
          HandlerTables.ScenarioDefinitionDataTable definitionDataTable = new HandlerTables.ScenarioDefinitionDataTable();
          dataAdapter.Fill((DataTable) definitionDataTable);
          SortedList<int, HandlerTables.ScenarioDefinitionRow> sortedList = new SortedList<int, HandlerTables.ScenarioDefinitionRow>();
          loadOrderKey = ";" + loadOrderKey + "_";
          foreach (HandlerTables.ScenarioDefinitionRow scenarioDefinitionRow in (TypedTableBase<HandlerTables.ScenarioDefinitionRow>) definitionDataTable)
          {
            if (!scenarioDefinitionRow.IsLoadDefinesNull())
            {
              int num1 = scenarioDefinitionRow.LoadDefines.IndexOf(loadOrderKey);
              if (num1 >= 0)
              {
                int num2 = scenarioDefinitionRow.LoadDefines.IndexOf(';', num1 + 1);
                if (num2 >= 0)
                {
                  int key = int.Parse(scenarioDefinitionRow.LoadDefines.Substring(num1 + loadOrderKey.Length, num2 - num1 - loadOrderKey.Length));
                  sortedList.Add(key, scenarioDefinitionRow);
                }
              }
            }
          }
          foreach (HandlerTables.ScenarioDefinitionRow scenarioDefinitionRow in (IEnumerable<HandlerTables.ScenarioDefinitionRow>) sortedList.Values)
          {
            int num = (int) ScenarioConfigurations.AddScenarioFromFileContent(ScenarioConfigurations.AllConfigurations, scenarioDefinitionRow.ScenarioSource);
            stringBuilder1.AppendLine(scenarioDefinitionRow.Scenario.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(nameof (LoadSelectedScenariosFromDatabase), ex);
      }
      return stringBuilder1.ToString();
    }

    public static string CopyChangedScenariosToSecondDatabase(bool onlyCheck)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      try
      {
        PlugInLoader.InitSecundaryDatabase();
        using (DbConnection newConnection1 = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.Append(" SELECT * ");
          stringBuilder2.Append(" FROM ScenarioDefinition");
          stringBuilder2.Append(" WHERE GroupNames LIKE '%;IUW;%'");
          stringBuilder2.Append(" ORDER BY Scenario");
          DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder2.ToString(), newConnection1);
          HandlerTables.ScenarioDefinitionDataTable definitionDataTable1 = new HandlerTables.ScenarioDefinitionDataTable();
          dataAdapter1.Fill((DataTable) definitionDataTable1);
          using (DbConnection newConnection2 = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
          {
            newConnection2.Open();
            DbTransaction transaction = newConnection2.BeginTransaction();
            DbDataAdapter dataAdapter2 = DbBasis.SecondaryDB.BaseDbConnection.GetDataAdapter(stringBuilder2.ToString(), newConnection2, transaction, out DbCommandBuilder _);
            HandlerTables.ScenarioDefinitionDataTable definitionDataTable2 = new HandlerTables.ScenarioDefinitionDataTable();
            dataAdapter2.Fill((DataTable) definitionDataTable2);
            int index1 = 0;
            int index2 = 0;
            int count = definitionDataTable2.Count;
            while (index1 < definitionDataTable1.Count || index2 < count)
            {
              bool flag1 = false;
              bool flag2 = false;
              bool flag3 = false;
              if (index1 >= definitionDataTable1.Count)
              {
                stringBuilder1.AppendLine(definitionDataTable2[index2].Scenario.ToString() + " No additional data in prime DB -> delete");
                flag2 = true;
              }
              else if (index2 >= count)
              {
                if (!definitionDataTable1[index1].IsLoadDefinesNull())
                {
                  stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " No additional data in sec DB -> copy");
                  flag1 = true;
                }
                else
                  ++index1;
              }
              else if (definitionDataTable1[index1].Scenario > definitionDataTable2[index2].Scenario)
              {
                stringBuilder1.AppendLine(definitionDataTable2[index2].Scenario.ToString() + " Not available in prime DB -> delete");
                flag2 = true;
              }
              else if (definitionDataTable1[index1].Scenario < definitionDataTable2[index2].Scenario)
              {
                if (!definitionDataTable1[index1].IsLoadDefinesNull())
                {
                  stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " Not available in second DB -> copy");
                  flag1 = true;
                }
                else
                  ++index1;
              }
              else
              {
                HandlerTables.ScenarioDefinitionRow scenarioDefinitionRow1 = definitionDataTable1[index1];
                HandlerTables.ScenarioDefinitionRow scenarioDefinitionRow2 = definitionDataTable2[index2];
                if (scenarioDefinitionRow1.ScenarioSource != scenarioDefinitionRow2.ScenarioSource)
                {
                  stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " ScenarioSource changed -> overwrite");
                  stringBuilder1.AppendLine("*** prime ***");
                  stringBuilder1.AppendLine(scenarioDefinitionRow1.ScenarioSource);
                  stringBuilder1.AppendLine("*** sec ***");
                  stringBuilder1.AppendLine(scenarioDefinitionRow2.ScenarioSource);
                  flag3 = true;
                }
                else if (scenarioDefinitionRow1.GroupNames != scenarioDefinitionRow2.GroupNames)
                {
                  stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " GroupNames changed -> overwrite");
                  stringBuilder1.AppendLine("   prime: " + scenarioDefinitionRow1.GroupNames);
                  stringBuilder1.AppendLine("     sec: " + scenarioDefinitionRow2.GroupNames);
                  flag3 = true;
                }
                else if (scenarioDefinitionRow1.IsLoadDefinesNull() != scenarioDefinitionRow2.IsLoadDefinesNull())
                {
                  if (scenarioDefinitionRow1.IsLoadDefinesNull())
                  {
                    stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " LoadDefines changed to null -> delete");
                    flag2 = true;
                    ++index1;
                  }
                  else
                  {
                    stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " LoadDefines changed to " + scenarioDefinitionRow1.LoadDefines + " -> overwrite");
                    flag3 = true;
                  }
                }
                else if (!scenarioDefinitionRow1.IsLoadDefinesNull() && scenarioDefinitionRow1.LoadDefines != scenarioDefinitionRow2.LoadDefines)
                {
                  stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " LoadDefines changed -> overwrite");
                  stringBuilder1.AppendLine("   prime: " + scenarioDefinitionRow1.LoadDefines);
                  stringBuilder1.AppendLine("     sec: " + scenarioDefinitionRow2.LoadDefines);
                  flag3 = true;
                }
                else if (scenarioDefinitionRow1.IsLoadDefinesNull())
                {
                  stringBuilder1.AppendLine(definitionDataTable1[index1].Scenario.ToString() + " Primary LoadDefines null -> delete");
                  flag2 = true;
                  ++index1;
                }
                else
                {
                  ++index1;
                  ++index2;
                }
              }
              if (flag2)
              {
                definitionDataTable2[index2].Delete();
                ++index2;
              }
              if (flag1)
              {
                HandlerTables.ScenarioDefinitionRow row = definitionDataTable2.NewScenarioDefinitionRow();
                row.Scenario = definitionDataTable1[index1].Scenario;
                row.ScenarioSource = definitionDataTable1[index1].ScenarioSource;
                row.GroupNames = definitionDataTable1[index1].GroupNames;
                if (!definitionDataTable1[index1].IsLoadDefinesNull())
                  row.LoadDefines = definitionDataTable1[index1].LoadDefines;
                definitionDataTable2.AddScenarioDefinitionRow(row);
                ++index1;
              }
              if (flag3)
              {
                HandlerTables.ScenarioDefinitionRow scenarioDefinitionRow = definitionDataTable2[index2];
                scenarioDefinitionRow.Scenario = definitionDataTable1[index1].Scenario;
                scenarioDefinitionRow.ScenarioSource = definitionDataTable1[index1].ScenarioSource;
                scenarioDefinitionRow.GroupNames = definitionDataTable1[index1].GroupNames;
                if (definitionDataTable1[index1].IsLoadDefinesNull())
                  scenarioDefinitionRow.SetLoadDefinesNull();
                else
                  scenarioDefinitionRow.LoadDefines = definitionDataTable1[index1].LoadDefines;
                ++index1;
                ++index2;
              }
            }
            if (stringBuilder1.Length == 0)
            {
              stringBuilder1.AppendLine("No changed scenarios");
            }
            else
            {
              stringBuilder1.Insert(0, "Not equal scenarios:" + Environment.NewLine + Environment.NewLine);
              if (!onlyCheck)
              {
                dataAdapter2.Update((DataTable) definitionDataTable2);
                transaction.Commit();
                stringBuilder1.AppendLine();
                stringBuilder1.AppendLine("Changes copied to secondary database");
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("LoadSelectedScenariosFromDatabase", ex);
      }
      return stringBuilder1.ToString();
    }

    public static bool IsScenarioEqualInSecondaryDB(ushort scenario, StringBuilder compareInfo)
    {
      bool flag = false;
      try
      {
        PlugInLoader.InitSecundaryDatabase();
        HandlerTables.ScenarioDefinitionRow scenarioDefinitionRow;
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(" SELECT * ");
          stringBuilder.Append(" FROM ScenarioDefinition");
          stringBuilder.Append(" WHERE Scenario = " + scenario.ToString());
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HandlerTables.ScenarioDefinitionDataTable definitionDataTable = new HandlerTables.ScenarioDefinitionDataTable();
          dataAdapter.Fill((DataTable) definitionDataTable);
          if (definitionDataTable.Count != 1)
          {
            compareInfo.AppendLine("Scenario in primary DB not found.");
            return flag;
          }
          scenarioDefinitionRow = definitionDataTable[0];
        }
        using (DbConnection newConnection = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(" SELECT * ");
          stringBuilder.Append(" FROM ScenarioDefinition");
          stringBuilder.Append(" WHERE Scenario = " + scenario.ToString());
          DbDataAdapter dataAdapter = DbBasis.SecondaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HandlerTables.ScenarioDefinitionDataTable definitionDataTable = new HandlerTables.ScenarioDefinitionDataTable();
          dataAdapter.Fill((DataTable) definitionDataTable);
          if (definitionDataTable.Count != 1)
          {
            compareInfo.AppendLine("Scenario in secondary DB not available.");
            return flag;
          }
          flag = true;
          if (scenarioDefinitionRow.ScenarioSource != definitionDataTable[0].ScenarioSource)
          {
            compareInfo.AppendLine("ScenarioSource different");
            compareInfo.AppendLine("PrimeDB:");
            compareInfo.AppendLine(scenarioDefinitionRow.ScenarioSource);
            compareInfo.AppendLine("SecondDB:");
            compareInfo.AppendLine(definitionDataTable[0].ScenarioSource);
            flag = false;
          }
          if (scenarioDefinitionRow.GroupNames != definitionDataTable[0].GroupNames)
          {
            compareInfo.AppendLine("GroupNames different");
            compareInfo.AppendLine("PrimeDB:");
            compareInfo.AppendLine(scenarioDefinitionRow.GroupNames);
            compareInfo.AppendLine("SecondDB:");
            compareInfo.AppendLine(definitionDataTable[0].GroupNames);
            flag = false;
          }
          if (scenarioDefinitionRow.LoadDefines != definitionDataTable[0].LoadDefines)
          {
            compareInfo.AppendLine("LoadDefines different");
            compareInfo.AppendLine("PrimeDB:");
            compareInfo.AppendLine(scenarioDefinitionRow.LoadDefines);
            compareInfo.AppendLine("SecondDB:");
            compareInfo.AppendLine(definitionDataTable[0].LoadDefines);
            flag = false;
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Compare scenario to sencond database", ex);
      }
      return flag;
    }

    public static void CopyScenarioToSecondaryDB(ushort scenario)
    {
      try
      {
        PlugInLoader.InitSecundaryDatabase();
        HandlerTables.ScenarioDefinitionRow scenarioDefinitionRow;
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(" SELECT * ");
          stringBuilder.Append(" FROM ScenarioDefinition");
          stringBuilder.Append(" WHERE Scenario = " + scenario.ToString());
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HandlerTables.ScenarioDefinitionDataTable definitionDataTable = new HandlerTables.ScenarioDefinitionDataTable();
          dataAdapter.Fill((DataTable) definitionDataTable);
          scenarioDefinitionRow = definitionDataTable.Count == 1 ? definitionDataTable[0] : throw new Exception("Data in primary database not found");
        }
        using (DbConnection newConnection = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(" SELECT * ");
          stringBuilder.Append(" FROM ScenarioDefinition");
          stringBuilder.Append(" WHERE Scenario = " + scenario.ToString());
          DbDataAdapter dataAdapter = DbBasis.SecondaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection, out DbCommandBuilder _);
          HandlerTables.ScenarioDefinitionDataTable definitionDataTable = new HandlerTables.ScenarioDefinitionDataTable();
          dataAdapter.Fill((DataTable) definitionDataTable);
          HandlerTables.ScenarioDefinitionRow row;
          if (definitionDataTable.Count < 1)
          {
            row = definitionDataTable.NewScenarioDefinitionRow();
            row.Scenario = (int) scenario;
          }
          else
            row = definitionDataTable[0];
          row.ScenarioSource = scenarioDefinitionRow.ScenarioSource;
          row.GroupNames = scenarioDefinitionRow.GroupNames;
          row.LoadDefines = scenarioDefinitionRow.LoadDefines;
          if (definitionDataTable.Count < 1)
            definitionDataTable.AddScenarioDefinitionRow(row);
          dataAdapter.Update((DataTable) definitionDataTable);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Copy scenario to sencond database", ex);
      }
    }
  }
}
