// Decompiled with JetBrains decompiler
// Type: GMM_Handler.DataChecker
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class DataChecker
  {
    private ZR_HandlerFunctions MyHandler;
    private static string[] OldParameterToUse = new string[13]
    {
      "EEP_Header.EEP_HEADER_MBusMedium",
      "DefaultFunction.Emergency_Frame0",
      "DefaultFunction.Emergency_Frame1",
      "DefaultFunction.Emergency_Frame2",
      "DefaultFunction.Emergency_Frame3",
      "DefaultFunction.Emergency_Frame4",
      "DefaultFunction.Emergency_Frame5",
      "DefaultFunction.Emergency_Frame_1",
      "DefaultFunction.Emergency_Frame_2",
      "DefaultFunction.Emergency_Frame_3",
      "DefaultFunction.Emergency_Frame_4",
      "DefaultFunction.Emergency_Frame_5",
      "DefaultFunction.Emergency_Frame_6"
    };

    public DataChecker(ZR_HandlerFunctions MyHandlerIn) => this.MyHandler = MyHandlerIn;

    public static void GetEpromDiffs(
      byte[] Eprom1,
      byte[] Eprom2,
      bool OnlyDiffs,
      StringBuilder TheText)
    {
      int length = Eprom1.Length;
      if (length > Eprom2.Length)
        length = Eprom2.Length;
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < length; ++index)
      {
        if ((int) Eprom1[index] != (int) Eprom2[index])
          ++num2;
        if (Eprom1[index] != (byte) 0 || Eprom2[index] > (byte) 0)
          num1 = index + 1;
      }
      TheText.AppendLine("Number of differences:   " + num2.ToString());
      TheText.AppendLine("Number of no zero bytes: " + num1.ToString());
      TheText.AppendLine();
      TheText.AppendLine("   Addr.  .. Diag Comp");
      for (int index = 0; index < num1; ++index)
      {
        if ((int) Eprom1[index] == (int) Eprom2[index])
        {
          if (!OnlyDiffs)
            TheText.Append("   ");
          else
            continue;
        }
        else
          TheText.Append("** ");
        TheText.Append("0x" + index.ToString("x04"));
        TheText.Append(" .. ");
        TheText.Append("0x" + Eprom1[index].ToString("x02"));
        TheText.Append(" 0x" + Eprom2[index].ToString("x02"));
        TheText.Append(" .. ");
        TheText.Append(Eprom1[index].ToString("d03"));
        TheText.Append(" " + Eprom2[index].ToString("d03"));
        TheText.AppendLine();
      }
    }

    public static void GetParameterDiffs(SortedList List1, SortedList List2, StringBuilder TheText)
    {
      bool[] flagArray1 = new bool[List1.Count];
      bool[] flagArray2 = new bool[List2.Count];
      int num = 0;
      for (int index = 0; index < List1.Count; ++index)
      {
        string key = (string) List1.GetKey(index);
        Parameter parameter = (Parameter) List2[(object) key];
        if (parameter != null)
        {
          flagArray1[index] = true;
          flagArray2[List2.IndexOfKey((object) key)] = true;
          Parameter byIndex = (Parameter) List1.GetByIndex(index);
          bool flag = false;
          if (byIndex.ValueEprom != parameter.ValueEprom || byIndex.ValueCPU != parameter.ValueCPU || byIndex.MBusOn != parameter.MBusOn || byIndex.MBusShortOn != parameter.MBusShortOn)
            flag = true;
          if (byIndex.DifVifs != parameter.DifVifs || (int) byIndex.DifVifSize != (int) parameter.DifVifSize)
            flag = true;
          if (flag)
          {
            ++num;
            TheText.Append(key);
            if (byIndex.ValueEprom != parameter.ValueEprom)
              TheText.Append("  ValEpromDiag: " + byIndex.ValueEprom.ToString() + " .. ValEpromComp: " + parameter.ValueEprom.ToString());
            if (byIndex.ValueCPU != parameter.ValueCPU)
              TheText.Append("  ValCpuDiag: " + byIndex.ValueCPU.ToString() + " .. ValCpuComp: " + parameter.ValueCPU.ToString());
          }
        }
      }
      TheText.AppendLine();
      TheText.AppendLine("Difference count: " + num.ToString());
    }

    public static void GetEpromParameterByAddress(SortedList ParameterList, StringBuilder TheText)
    {
      TheText.AppendLine();
      TheText.AppendLine("Number of eprom variables: " + ParameterList.Count.ToString());
      TheText.AppendLine();
      for (int index = 0; index < ParameterList.Count; ++index)
      {
        Parameter byIndex = (Parameter) ParameterList.GetByIndex(index);
        TheText.Append(byIndex.Address.ToString("x04"));
        TheText.Append(" ");
        TheText.Append(byIndex.Name);
        TheText.Append(" ");
        TheText.Append(byIndex.FunctionNumber.ToString());
        TheText.AppendLine();
      }
    }

    public static void GetRamParameterByAddress(Meter TheMeter, StringBuilder TheText)
    {
      SortedList parametersByAddress = TheMeter.AllRamParametersByAddress;
      TheText.AppendLine();
      TheText.AppendLine("Number of ram variables: " + parametersByAddress.Count.ToString());
      TheText.AppendLine();
      for (int index = 0; index < parametersByAddress.Count; ++index)
      {
        Parameter byIndex = (Parameter) parametersByAddress.GetByIndex(index);
        TheText.Append(TheMeter.GetRamWriteProtectionChar(byIndex.Size, byIndex.AddressCPU));
        TheText.Append(byIndex.AddressCPU.ToString("x04"));
        TheText.Append(" ");
        TheText.Append(byIndex.Name);
        TheText.Append(" ");
        TheText.Append(byIndex.FunctionNumber.ToString());
        TheText.AppendLine();
      }
    }

    public static string GetStaticDiffToExternalConnectedMeter(Meter TheMeter)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!TheMeter.MyHandler.MyMeters.ConnectMeter())
      {
        stringBuilder.AppendLine("Can not connect external meter");
        return stringBuilder.ToString();
      }
      Meter connectedMeter = TheMeter.MyHandler.MyMeters.ConnectedMeter;
      int address1 = ((LinkObj) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_SerialNr"]).Address;
      Parameter allParameter1 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_HeaderChecksum"];
      Parameter allParameter2 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_StaticChecksum"];
      Parameter allParameter3 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupChecksum"];
      Parameter allParameter4 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RamParamBlockAdr"];
      Parameter allParameter5 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_BackupBlockAdr"];
      Parameter allParameter6 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FixedParamAdr"];
      Parameter allParameter7 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RuntimeVarsAdr"];
      Parameter allParameter8 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_RuntimeCodeAdr"];
      Parameter allParameter9 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_EpromVarsAdr"];
      Parameter allParameter10 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_ParamBlockAdr"];
      Parameter allParameter11 = (Parameter) TheMeter.AllParameters[(object) "EEP_Header.EEP_HEADER_FunctionTableAdr"];
      connectedMeter.Eprom = new byte[TheMeter.Eprom.Length];
      int NumberOfBytes1 = (int) (allParameter11.ValueEprom - 6L);
      ByteField MemoryData;
      if (!connectedMeter.MyCommunication.MyBus.ReadMemory(MemoryLocation.EEPROM, 6, NumberOfBytes1, out MemoryData))
      {
        stringBuilder.AppendLine("Read error");
        return stringBuilder.ToString();
      }
      for (int index = 0; index < MemoryData.Count; ++index)
        connectedMeter.Eprom[index + 6] = MemoryData.Data[index];
      int address2 = allParameter1.Address;
      if ((int) TheMeter.MyEpromHeader.GenerateChecksum(connectedMeter.Eprom, address1, address2 - address1, (ushort) 0) != (int) ParameterService.GetFromByteArray_ushort(connectedMeter.Eprom, ref address2))
        stringBuilder.AppendLine("Exteral header checksum is wrong");
      else
        stringBuilder.AppendLine("Exteral header checksum is ok");
      int valueEprom1 = (int) allParameter4.ValueEprom;
      int NumberOfBytes2 = (int) allParameter5.ValueEprom - valueEprom1;
      ushort checksum1 = TheMeter.MyEpromHeader.GenerateChecksum(connectedMeter.Eprom, valueEprom1, NumberOfBytes2, (ushort) 0);
      int valueEprom2 = (int) allParameter6.ValueEprom;
      int NumberOfBytes3 = (int) allParameter7.ValueEprom - valueEprom2;
      ushort checksum2 = TheMeter.MyEpromHeader.GenerateChecksum(connectedMeter.Eprom, valueEprom2, NumberOfBytes3, checksum1);
      int valueEprom3 = (int) allParameter8.ValueEprom;
      int NumberOfBytes4 = (int) allParameter9.ValueEprom - valueEprom3;
      ushort checksum3 = TheMeter.MyEpromHeader.GenerateChecksum(connectedMeter.Eprom, valueEprom3, NumberOfBytes4, checksum2);
      int valueEprom4 = (int) allParameter10.ValueEprom;
      int NumberOfBytes5 = (int) allParameter11.ValueEprom - valueEprom4;
      ushort checksum4 = TheMeter.MyEpromHeader.GenerateChecksum(connectedMeter.Eprom, valueEprom4, NumberOfBytes5, checksum3);
      if (allParameter2.ValueEprom != (long) checksum4)
        stringBuilder.AppendLine("Exteral static checksum is wrong");
      else
        stringBuilder.AppendLine("Exteral static checksum is ok");
      stringBuilder.AppendLine();
      int index1 = address1;
      for (int index2 = address2; index1 < index2; ++index1)
      {
        if ((int) connectedMeter.Eprom[index1] != (int) TheMeter.Eprom[index1])
          stringBuilder.AppendLine(index1.ToString("x04") + ": " + TheMeter.Eprom[index1].ToString("x02") + " -> " + connectedMeter.Eprom[index1].ToString("x02"));
      }
      int valueEprom5 = (int) allParameter4.ValueEprom;
      for (int valueEprom6 = (int) allParameter5.ValueEprom; valueEprom5 < valueEprom6; ++valueEprom5)
      {
        if ((int) connectedMeter.Eprom[valueEprom5] != (int) TheMeter.Eprom[valueEprom5])
          stringBuilder.AppendLine(valueEprom5.ToString("x04") + ": " + TheMeter.Eprom[valueEprom5].ToString("x02") + " -> " + connectedMeter.Eprom[valueEprom5].ToString("x02"));
      }
      int valueEprom7 = (int) allParameter6.ValueEprom;
      for (int valueEprom8 = (int) allParameter7.ValueEprom; valueEprom7 < valueEprom8; ++valueEprom7)
      {
        if ((int) connectedMeter.Eprom[valueEprom7] != (int) TheMeter.Eprom[valueEprom7])
          stringBuilder.AppendLine(valueEprom7.ToString("x04") + ": " + TheMeter.Eprom[valueEprom7].ToString("x02") + " -> " + connectedMeter.Eprom[valueEprom7].ToString("x02"));
      }
      int valueEprom9 = (int) allParameter8.ValueEprom;
      for (int valueEprom10 = (int) allParameter9.ValueEprom; valueEprom9 < valueEprom10; ++valueEprom9)
      {
        if ((int) connectedMeter.Eprom[valueEprom9] != (int) TheMeter.Eprom[valueEprom9])
          stringBuilder.AppendLine(valueEprom9.ToString("x04") + ": " + TheMeter.Eprom[valueEprom9].ToString("x02") + " -> " + connectedMeter.Eprom[valueEprom9].ToString("x02"));
      }
      int valueEprom11 = (int) allParameter10.ValueEprom;
      for (int valueEprom12 = (int) allParameter11.ValueEprom; valueEprom11 < valueEprom12; ++valueEprom11)
      {
        if ((int) connectedMeter.Eprom[valueEprom11] != (int) TheMeter.Eprom[valueEprom11])
          stringBuilder.AppendLine(valueEprom11.ToString("x04") + ": " + TheMeter.Eprom[valueEprom11].ToString("x02") + " -> " + connectedMeter.Eprom[valueEprom11].ToString("x02"));
      }
      return stringBuilder.ToString();
    }

    public static bool SetAllMaxValuesCritical(Meter TheMeter)
    {
      if (TheMeter.MyCommunication == null)
      {
        int num = (int) MessageBox.Show("Diagnostic object has no communication ability.");
        return false;
      }
      int index1 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.LastHourEnergy");
      if (index1 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index1);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      int index2 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.LastHourVolume");
      if (index2 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index2);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      int index3 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.MaxFlow");
      if (index3 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index3);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      int index4 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.MaxFlowAbs");
      if (index4 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index4);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      int index5 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.MaxFlowTimePoint");
      if (index5 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index5);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      int index6 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.MaxPower");
      if (index6 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index6);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      int index7 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.MaxPowerAbs");
      if (index7 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index7);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      int index8 = TheMeter.AllParameters.IndexOfKey((object) "MaxFlowAndPower.MaxPowerTimePoint");
      if (index8 >= 0)
      {
        Parameter byIndex = (Parameter) TheMeter.AllParameters.GetByIndex(index8);
        byIndex.ValueEprom = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.EEPROM);
        byIndex.ValueCPU = 0L;
        TheMeter.MyCommunication.WriteParameterValue(byIndex, MemoryLocation.RAM);
      }
      return true;
    }

    public static string GetAllEventTimes(Meter TheMeter)
    {
      StringBuilder stringBuilder = new StringBuilder(1000);
      if (TheMeter.MyCommunication != null)
      {
        Parameter allParameter = (Parameter) TheMeter.AllParameters[(object) "DefaultFunction.Sta_Secounds"];
        TheMeter.MyCommunication.ReadParameterValue(allParameter, MemoryLocation.RAM);
        DateTime dateTime = ZR_Calendar.Cal_GetDateTime((uint) allParameter.ValueCPU);
        stringBuilder.AppendLine(dateTime.ToString("dd.MM.yyyy HH.mm.ss") + " -> " + allParameter.FullName);
        stringBuilder.AppendLine();
      }
      foreach (Parameter TheParameter in (IEnumerable) TheMeter.AllParameters.Values)
      {
        if (TheParameter.Name.EndsWith("_0T") && TheParameter.ExistOnEprom)
        {
          if (TheMeter.MyCommunication != null)
            TheMeter.MyCommunication.ReadParameterValue(TheParameter, MemoryLocation.EEPROM);
          DateTime dateTime = ZR_Calendar.Cal_GetDateTime((uint) TheParameter.ValueEprom);
          stringBuilder.AppendLine(dateTime.ToString("dd.MM.yyyy HH.mm.ss") + " -> " + TheParameter.FullName);
        }
      }
      ByteField MemoryData;
      if (TheMeter.MyCommunication != null && TheMeter.MyCommunication.MyBus.ReadMemory(MemoryLocation.RAM, 512, 96, out MemoryData))
      {
        int num1 = 68;
        int index1 = num1;
        for (int index2 = 0; index2 < 10 && (MemoryData.Data[index1] != (byte) 212 || MemoryData.Data[index1 + 1] != (byte) 207); ++index2)
          index1 += 2;
        int index3 = num1;
        for (int index4 = 0; index4 < 10 && (MemoryData.Data[index3] != (byte) 104 || MemoryData.Data[index3 + 1] != (byte) 185); ++index4)
          index3 += 2;
        int num2 = num1 + 20;
        if (index1 < num2 && index3 < num2)
        {
          int num3 = 28;
          int num4 = num3 + index1 - num1;
          byte[] data1 = MemoryData.Data;
          int index5 = num4;
          int index6 = index5 + 1;
          int num5 = (int) data1[index5] + ((int) MemoryData.Data[index6] << 8);
          int num6 = num3 + index3 - num1;
          byte[] data2 = MemoryData.Data;
          int index7 = num6;
          int index8 = index7 + 1;
          int num7 = (int) data2[index7] + ((int) MemoryData.Data[index8] << 8) - num5;
          if (num7 < 0)
            num7 *= -1;
          if (num7 >= 32768)
            num7 = 65536 - num7;
          int num8 = num7 * 8;
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("Zeit/Zyklus Differenz [ms]: " + num8.ToString());
          int num9 = num8 % 512;
          stringBuilder.AppendLine("Zeit/Zyklus Verschiebung [ms]: " + num9.ToString());
        }
      }
      return stringBuilder.ToString();
    }

    public static string[] GetMBusParameterList(Meter TheMeter)
    {
      ArrayList arrayList = new ArrayList();
      foreach (string listParameterName in TheMeter.MyMBusList.ShortListParameterNames)
        arrayList.Add((object) listParameterName);
      int count = arrayList.Count;
      foreach (string listParameterName in TheMeter.MyMBusList.FullListParameterNames)
        arrayList.Add((object) listParameterName);
      string[] mbusParameterList = new string[arrayList.Count + 1];
      for (int index = 0; index < mbusParameterList.Length; ++index)
        mbusParameterList[index] = index >= count ? (index != count ? (string) arrayList[index - 1] : "LongList:") : (string) arrayList[index];
      return mbusParameterList;
    }

    public static string[] GetMBusParameterListWithListInfo(Meter TheMeter)
    {
      ArrayList arrayList = new ArrayList();
      foreach (string listParameterName in TheMeter.MyMBusList.ShortListParameterNames)
        arrayList.Add((object) ("Short." + listParameterName));
      foreach (string listParameterName in TheMeter.MyMBusList.FullListParameterNames)
        arrayList.Add((object) ("Full." + listParameterName));
      if (TheMeter.MyMBusList.ActiveLoggerFunctionNumber >= (short) 0)
      {
        Function function = (Function) TheMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) TheMeter.MyMBusList.ActiveLoggerFunctionNumber];
        arrayList.Add((object) ("Logger." + function.Name + "(" + function.Number.ToString() + ")"));
        foreach (MBusLoggerInfo mbusLoggerInfo in (IEnumerable<MBusLoggerInfo>) TheMeter.MyMBusList.MBusLoggerInfos.Values)
        {
          if ((int) mbusLoggerInfo.FunctionNumber == (int) TheMeter.MyMBusList.ActiveLoggerFunctionNumber)
          {
            arrayList.Add((object) ("LoggerEvents " + mbusLoggerInfo.LoggerNumberOfEntrys.ToString()));
            break;
          }
        }
      }
      string[] listWithListInfo = new string[arrayList.Count];
      for (int index = 0; index < listWithListInfo.Length; ++index)
        listWithListInfo[index] = (string) arrayList[index];
      return listWithListInfo;
    }

    public static string[] GetFunctionNumbersList(Meter TheMeter)
    {
      string[] functionNumbersList = new string[TheMeter.MyFunctionTable.FunctionListByNumber.Count];
      for (int index = 0; index < functionNumbersList.Length; ++index)
        functionNumbersList[index] = ((ushort) TheMeter.MyFunctionTable.FunctionListByNumber.GetKey(index)).ToString();
      return functionNumbersList;
    }

    public static string[] GetFunctionList(Meter TheMeter)
    {
      ArrayList arrayList = new ArrayList();
      for (int index1 = 0; index1 < TheMeter.MyFunctionTable.FunctionList.Count; ++index1)
      {
        for (int index2 = 0; index2 < TheMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList.Count; ++index2)
        {
          if (index1 == (int) (short) TheMeter.MyFunctionTable.FunctionStartIndexOfMenuColumnList[index2])
          {
            int num = index2 + 1;
            arrayList.Add((object) ("M:" + num.ToString()));
          }
        }
        arrayList.Add((object) (((Function) TheMeter.MyFunctionTable.FunctionList[index1]).Name + "(" + ((Function) TheMeter.MyFunctionTable.FunctionList[index1]).Number.ToString() + ")"));
      }
      string[] functionList = new string[arrayList.Count];
      for (int index = 0; index < functionList.Length; ++index)
        functionList[index] = arrayList[index].ToString();
      return functionList;
    }

    public static bool IsEqualMap(Meter OriginalMeter, Meter CompareMeter)
    {
      if (OriginalMeter.MyLinker.LinkBlockList.Count != CompareMeter.MyLinker.LinkBlockList.Count)
        return false;
      for (int index = 0; index < OriginalMeter.MyLinker.LinkBlockList.Count; ++index)
      {
        if (((LinkBlock) OriginalMeter.MyLinker.LinkBlockList[index]).BlockStartAddress != ((LinkBlock) CompareMeter.MyLinker.LinkBlockList[index]).BlockStartAddress)
          return false;
      }
      return true;
    }

    public static bool IsEqualAllPointers(Meter OriginalMeter, Meter CompareMeter)
    {
      ArrayList linkPointerList = OriginalMeter.MyLinker.LinkPointerList;
      if (OriginalMeter.MyLinker.LinkPointerList.Count < CompareMeter.MyLinker.LinkPointerList.Count)
        linkPointerList = CompareMeter.MyLinker.LinkPointerList;
      for (int index1 = 0; index1 < linkPointerList.Count; ++index1)
      {
        CodeObject pointerObject = ((LinkPointer) OriginalMeter.MyLinker.LinkPointerList[index1]).PointerObject;
        for (int index2 = 0; index2 < pointerObject.Size; ++index2)
        {
          if ((int) OriginalMeter.Eprom[pointerObject.Address + index2] != (int) CompareMeter.Eprom[pointerObject.Address + index2])
            return false;
        }
      }
      return true;
    }

    public static bool IsEqualProtectedArea(Meter OriginalMeter, Meter CompareMeter)
    {
      OriginalMeter.GenerateWriteEnableLists(true);
      CompareMeter.GenerateWriteEnableLists(true);
      if (OriginalMeter.EpromWriteEnable.Length != CompareMeter.EpromWriteEnable.Length)
        return false;
      for (int index = 0; index < OriginalMeter.EpromWriteEnable.Length; ++index)
      {
        if (OriginalMeter.EpromWriteEnable[index] != CompareMeter.EpromWriteEnable[index])
          return false;
      }
      return true;
    }

    public static bool IsProtectedDataDiff(Meter OriginalMeter, Meter CompareMeter)
    {
      OriginalMeter.GenerateWriteEnableLists(true);
      CompareMeter.GenerateWriteEnableLists(true);
      for (int index1 = 6; index1 < OriginalMeter.MyLoggerStore.BlockStartAddress; ++index1)
      {
        if ((int) OriginalMeter.Eprom[index1] != (int) CompareMeter.Eprom[index1] && !OriginalMeter.EpromWriteEnable[index1])
        {
          bool flag = false;
          for (int index2 = 0; index2 < DataChecker.OldParameterToUse.Length; ++index2)
          {
            Parameter allParameter = (Parameter) CompareMeter.AllParameters[(object) DataChecker.OldParameterToUse[index2]];
            if (allParameter != null && index1 >= allParameter.Address && index1 < allParameter.Address + allParameter.Size)
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            return false;
        }
      }
      return true;
    }

    public static bool IsLoggerEqualToTable(Meter TheMeter)
    {
      foreach (IntervalAndLogger allIntervallCode in TheMeter.MyLinker.AllIntervallCodes)
      {
        DataBaseAccess.LoggerEntryData loggerEntriesInfos = TheMeter.MyHandler.MyDataBaseAccess.GetLoggerEntriesInfos(allIntervallCode.FunctionNumber);
        if (loggerEntriesInfos.Entries != allIntervallCode.MaxEntries || loggerEntriesInfos.EntrySize != allIntervallCode.EntrySize || (long) loggerEntriesInfos.Interval != (long) allIntervallCode.Interval)
          return false;
      }
      return true;
    }

    public static bool IsEndTimeOk(Meter TheMeter, out string Info)
    {
      Info = "End time not found";
      int num = (int) ushort.MaxValue;
      if (TheMeter.DatabaseTime == DateTime.MinValue)
      {
        Info = "No database storage time found";
        return true;
      }
      int index1 = TheMeter.AllParameters.IndexOfKey((object) "EndOfBatterie.BattEndDate");
      if (index1 >= 0)
      {
        num = ZR_Calendar.Cal_GetDateTime((uint) ((Parameter) TheMeter.AllParameters.GetByIndex(index1)).ValueEprom).Year - TheMeter.DatabaseTime.Year;
        Info = "Battery end time years = " + num.ToString();
      }
      int index2 = TheMeter.AllParameters.IndexOfKey((object) "EichgültigkeitsdatumStatus.pEichgültigkeitsdatum");
      if (index2 >= 0)
      {
        num = ZR_Calendar.Cal_GetDateTime((uint) ((Parameter) TheMeter.AllParameters.GetByIndex(index2)).ValueEprom).Year - TheMeter.DatabaseTime.Year;
        Info = "Approval end time years = " + num.ToString();
      }
      return num == (int) ushort.MaxValue || num >= 5;
    }

    public static bool AreOverridesEqualToDatabase(Meter TheMeter)
    {
      ArrayList Overrides;
      if (!TheMeter.MyHandler.MyDataBaseAccess.GetOverrides(TheMeter.MyIdent.MeterInfoID, out Overrides))
        return false;
      foreach (OverrideParameter overrideParameter in Overrides)
      {
        if (overrideParameter.ParameterID == OverrideID.ReadingDate)
        {
          string stringValueWin = overrideParameter.GetStringValueWin();
          if (stringValueWin.Length > 2)
          {
            DateTime dateTime1 = DateTime.Parse(stringValueWin + ".2000");
            if (TheMeter.IsMeterResourceAvailable(MeterResources.DueDate))
            {
              DateTime dateTime2 = ZR_Calendar.Cal_GetDateTime((uint) ((Parameter) TheMeter.AllParametersByResource[(object) MeterResources.DueDate.ToString()]).ValueEprom);
              if (dateTime1.Day != dateTime2.Day || dateTime1.Month != dateTime2.Month)
                return false;
              break;
            }
            return TheMeter.MyHandler.MyDataBaseAccess.IsMeterInfoProperty(TheMeter.MyIdent.MeterInfoID, DataBaseAccess.MeterInfoProperties.OverridesChanged, (string) null);
          }
          break;
        }
      }
      return true;
    }

    public static bool GetMeterResourcesList(Meter TheMeter, StringBuilder TheText)
    {
      if (TheMeter == null || TheMeter.AvailableMeterResouces == null)
        return false;
      TheText.AppendLine("Resource Name ; Number of references");
      for (int index1 = 0; index1 < TheMeter.AvailableMeterResouces.Count; ++index1)
      {
        MeterResource byIndex = (MeterResource) TheMeter.AvailableMeterResouces.GetByIndex(index1);
        TheText.Append(byIndex.Name.PadRight(20) + ": ");
        TheText.Append(byIndex.SuppliedFromFunction.ToString("d3") + "  ");
        for (int index2 = 0; index2 < byIndex.UsedFromFunctions.Count; ++index2)
          TheText.Append(((ushort) byIndex.UsedFromFunctions[index2]).ToString("d3") + ";");
        TheText.AppendLine();
      }
      return true;
    }

    internal static void TryReloadWrongOldData(Meter ReadMeter, Meter WorkMeter)
    {
      for (int index = 0; index < DataChecker.OldParameterToUse.Length; ++index)
      {
        Parameter allParameter1 = (Parameter) ReadMeter.AllParameters[(object) DataChecker.OldParameterToUse[index]];
        Parameter allParameter2 = (Parameter) WorkMeter.AllParameters[(object) DataChecker.OldParameterToUse[index]];
        if (allParameter1 != null && allParameter2 != null)
        {
          allParameter2.ValueEprom = allParameter1.ValueEprom;
          allParameter2.UpdateByteList();
        }
      }
      WorkMeter.GenerateEprom();
    }
  }
}
