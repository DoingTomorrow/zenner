// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceStateCounter
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class DeviceStateCounter
  {
    private SortedList<DeviceStateCounterID, uint> StateCounters;

    private DeviceStateCounter()
    {
      this.StateCounters = new SortedList<DeviceStateCounterID, uint>();
    }

    public DeviceStateCounter(byte[] receivedData)
      : this()
    {
      int offset = 0;
      while (offset < receivedData.Length)
      {
        byte key = ByteArrayScanner.ScanByte(receivedData, ref offset);
        if (!Enum.IsDefined(typeof (DeviceStateCounterID), (object) key))
        {
          offset += 4;
        }
        else
        {
          uint num = ByteArrayScanner.ScanUInt32(receivedData, ref offset);
          this.StateCounters.Add((DeviceStateCounterID) key, num);
        }
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<DeviceStateCounterID, uint> stateCounter in this.StateCounters)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append("; ");
        stringBuilder.Append(stateCounter.Key.ToString() + ":" + stateCounter.Value.ToString());
      }
      return stringBuilder.ToString();
    }

    public string ToTextBlock()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      int num1 = 0;
      foreach (KeyValuePair<DeviceStateCounterID, uint> stateCounter in this.StateCounters)
      {
        if (num1 == 3)
        {
          stringBuilder1.AppendLine();
          num1 = 1;
        }
        else
          ++num1;
        string str1 = stateCounter.Key.ToString();
        string str2 = (str1 + ": ").PadRight(27, '.').PadRight(28);
        stringBuilder1.Append(str2);
        uint num2;
        double num3;
        if (str1.EndsWith("MsTime"))
        {
          num2 = stateCounter.Value;
          string str3 = num2.ToString() + " ms";
          stringBuilder1.Append(str3.PadRight(11));
          StringBuilder stringBuilder2 = stringBuilder1;
          num3 = (double) stateCounter.Value / 1000.0 / 3600.0;
          string str4 = " = " + num3.ToString("0.000") + " hours";
          stringBuilder2.Append(str4);
        }
        else if (str1.EndsWith("Time"))
        {
          num2 = stateCounter.Value;
          string str5 = num2.ToString() + " s";
          stringBuilder1.Append(str5.PadRight(11));
          StringBuilder stringBuilder3 = stringBuilder1;
          num3 = (double) stateCounter.Value / 86400.0;
          string str6 = " = " + num3.ToString("0.00") + " days";
          stringBuilder3.Append(str6);
        }
        else
        {
          StringBuilder stringBuilder4 = stringBuilder1;
          num2 = stateCounter.Value;
          string str7 = num2.ToString();
          stringBuilder4.Append(str7);
        }
        stringBuilder1.AppendLine();
      }
      return stringBuilder1.ToString();
    }

    public static void DeleteStateInMemory(
      DeviceMemory theMemory,
      SortedList<DeviceStateCounterID, AddressRange> memoryRanges)
    {
      foreach (KeyValuePair<DeviceStateCounterID, AddressRange> memoryRange in memoryRanges)
      {
        if (theMemory.AreDataAvailable(memoryRange.Value))
          theMemory.SetValue<uint>(0U, memoryRange.Value.StartAddress);
      }
    }

    public static DeviceStateCounter CreateObjectFromMemory(
      DeviceMemory theMemory,
      SortedList<DeviceStateCounterID, AddressRange> memoryRanges)
    {
      DeviceStateCounter objectFromMemory = new DeviceStateCounter();
      foreach (KeyValuePair<DeviceStateCounterID, AddressRange> memoryRange in memoryRanges)
      {
        if (theMemory.AreDataAvailable(memoryRange.Value))
        {
          uint num = theMemory.GetValue<uint>(memoryRange.Value.StartAddress);
          objectFromMemory.StateCounters.Add(memoryRange.Key, num);
        }
      }
      return objectFromMemory;
    }
  }
}
