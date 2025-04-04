// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.LoRa_AlarmEntry
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using SmartFunctionCompiler;
using System;
using System.Text;

#nullable disable
namespace S4_Handler.Functions
{
  internal class LoRa_AlarmEntry
  {
    internal uint alarm;
    internal DateTime alarm_time;
    internal uint alarm_value;
    internal bool is_value_used;
    internal bool is_extern_send;
    internal bool is_intern_send;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.GetAlarmIdent());
      if (this.is_value_used)
        stringBuilder.Append("; Value: 0x" + this.alarm_value.ToString("x08"));
      else
        stringBuilder.Append("; No value");
      stringBuilder.Append("; Extern_send: " + this.is_extern_send.ToString());
      stringBuilder.Append("; Internal_send: " + this.is_intern_send.ToString());
      return stringBuilder.ToString();
    }

    internal string GetAlarmIdent()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Time:" + this.alarm_time.ToString("dd.MM.yy HH:mm:ss"));
      stringBuilder.Append(": Alarm: 0x" + this.alarm.ToString("x08") + "=" + ((LoRaAlarm) this.alarm).ToString());
      return stringBuilder.ToString();
    }
  }
}
