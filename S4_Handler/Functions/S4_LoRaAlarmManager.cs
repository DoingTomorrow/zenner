// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_LoRaAlarmManager
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_LoRaAlarmManager
  {
    private S4_DeviceMemory workMeterMemory;
    private S4_DeviceCommandsNFC commands;
    private List<LoRa_AlarmEntry> LoRa_Alarms;
    private List<LoRa_AlarmEntry> Last_LoRa_Alarms;

    internal S4_LoRaAlarmManager(S4_DeviceCommandsNFC commands, S4_DeviceMemory workMeterMemory)
    {
      this.workMeterMemory = workMeterMemory;
      this.commands = commands;
    }

    internal async Task<List<LoRa_AlarmEntry>> Read(
      ProgressHandler progress,
      CancellationToken token)
    {
      AddressRange sfunc_alarm_range = new AddressRange(this.workMeterMemory.GetParameterAddress(S4_Params.sfunc_alarm), 80U);
      await this.commands.ReadMemoryAsync(sfunc_alarm_range, (DeviceMemory) this.workMeterMemory, progress, token);
      byte alarm_counts = this.workMeterMemory.GetParameterValue<byte>(S4_Params.sfunc_lora_alarm_counts);
      this.Last_LoRa_Alarms = this.LoRa_Alarms;
      this.LoRa_Alarms = new List<LoRa_AlarmEntry>();
      uint readAddress = sfunc_alarm_range.StartAddress;
      for (int i = 0; i < (int) alarm_counts; ++i)
      {
        LoRa_AlarmEntry new_entry = new LoRa_AlarmEntry();
        new_entry.alarm = this.workMeterMemory.GetValue<uint>(readAddress);
        new_entry.alarm_time = this.GetDateTimeFromAddress(readAddress + 4U);
        new_entry.alarm_value = this.workMeterMemory.GetValue<uint>(readAddress + 8U);
        new_entry.is_value_used = this.workMeterMemory.GetValue<byte>(readAddress + 12U) > (byte) 0;
        new_entry.is_extern_send = this.workMeterMemory.GetValue<byte>(readAddress + 13U) > (byte) 0;
        new_entry.is_intern_send = this.workMeterMemory.GetValue<byte>(readAddress + 14U) > (byte) 0;
        this.LoRa_Alarms.Add(new_entry);
        new_entry = (LoRa_AlarmEntry) null;
      }
      List<LoRa_AlarmEntry> loRaAlarms = this.LoRa_Alarms;
      sfunc_alarm_range = (AddressRange) null;
      return loRaAlarms;
    }

    internal string GetAlarmState()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Number of prepared LoRa alarms: " + this.LoRa_Alarms.Count.ToString());
      foreach (LoRa_AlarmEntry loRaAlarm in this.LoRa_Alarms)
        stringBuilder.AppendLine(loRaAlarm.ToString());
      return stringBuilder.ToString();
    }

    internal string GetAlarmChanges()
    {
      if (this.LoRa_Alarms == null)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      List<LoRa_AlarmEntry> source = new List<LoRa_AlarmEntry>();
      foreach (LoRa_AlarmEntry lastLoRaAlarm in this.Last_LoRa_Alarms)
        source.Add(lastLoRaAlarm);
      List<LoRa_AlarmEntry> loRaAlarmEntryList = new List<LoRa_AlarmEntry>();
      foreach (LoRa_AlarmEntry loRaAlarm in this.LoRa_Alarms)
      {
        LoRa_AlarmEntry testAlarm = loRaAlarm;
        LoRa_AlarmEntry loRaAlarmEntry = (LoRa_AlarmEntry) null;
        if (this.Last_LoRa_Alarms != null)
          loRaAlarmEntry = source.FirstOrDefault<LoRa_AlarmEntry>((Func<LoRa_AlarmEntry, bool>) (x => (int) x.alarm == (int) testAlarm.alarm && x.alarm_time == testAlarm.alarm_time));
        if (loRaAlarmEntry != null)
        {
          string str = string.Empty;
          if (testAlarm.is_value_used != loRaAlarmEntry.is_value_used)
            str = str + "; is_value_used changed to: " + testAlarm.is_value_used.ToString();
          else if (testAlarm.is_value_used && (int) testAlarm.alarm_value != (int) loRaAlarmEntry.alarm_value)
            str = str + "; alarm_value change to: 0x" + testAlarm.alarm_value.ToString("x08");
          if (testAlarm.is_extern_send != loRaAlarmEntry.is_extern_send)
            str = str + "; is_extern_send change to: " + testAlarm.is_extern_send.ToString();
          if (testAlarm.is_intern_send != loRaAlarmEntry.is_intern_send)
            str = str + "; is_intern_send change to: " + testAlarm.is_intern_send.ToString();
          if (str.Length > 0)
            stringBuilder.AppendLine("Changed alarm: " + testAlarm.GetAlarmIdent() + str);
          loRaAlarmEntryList.Add(loRaAlarmEntry);
          source.Remove(loRaAlarmEntry);
        }
        else
          stringBuilder.AppendLine("New alarm: ... " + testAlarm.ToString());
      }
      foreach (LoRa_AlarmEntry loRaAlarmEntry in source)
        stringBuilder.AppendLine("Deleted alarm: " + loRaAlarmEntry.ToString());
      return stringBuilder.ToString();
    }

    private DateTime GetDateTimeFromAddress(uint readAddress)
    {
      return new DateTime(2000, 1, 1).AddSeconds((double) this.workMeterMemory.GetValue<uint>(readAddress));
    }
  }
}
