// Decompiled with JetBrains decompiler
// Type: S4_Handler.S4_FunctionalState
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using SmartFunctionCompiler;
using System;
using System.Text;

#nullable disable
namespace S4_Handler
{
  public class S4_FunctionalState
  {
    private ushort FunctionalState;

    public DateTime? DeviceTime { get; set; }

    public uint? AlarmValue { get; set; }

    public byte? ConfigChangedCounter { get; set; }

    public byte? StateNumber { get; set; }

    public bool BatteryOver => ((int) this.FunctionalState & 1) > 0;

    public bool BatteryWarning => ((int) this.FunctionalState & 2) > 0;

    public bool BatteryError => ((int) this.FunctionalState & 4) > 0;

    public bool AccuracyUnsafe => ((int) this.FunctionalState & 8) > 0;

    public bool HardwareError => ((int) this.FunctionalState & 16) > 0;

    public bool EmptyTube => ((int) this.FunctionalState & 32) > 0;

    public bool FlowOutOfRange => ((int) this.FunctionalState & 64) > 0;

    public bool Sleep => ((int) this.FunctionalState & 128) > 0;

    public bool NotProtected => ((int) this.FunctionalState & 256) > 0;

    public string Alarm
    {
      get
      {
        if (!this.AlarmValue.HasValue)
          return (string) null;
        return Enum.IsDefined(typeof (LoRaAlarm), (object) this.AlarmValue.Value) ? ((LoRaAlarm) this.AlarmValue.Value).ToString().Replace("LoRaAlarm_", "") : "0x" + this.AlarmValue.Value.ToString("x08");
      }
    }

    public S4_FunctionalState(byte[] receivedFrame)
    {
      int offset = 2;
      this.DeviceTime = new DateTime?(ByteArrayScanner.ScanDateTime(receivedFrame, ref offset));
      this.FunctionalState = ByteArrayScanner.ScanUInt16(receivedFrame, ref offset);
      if (receivedFrame.Length >= 13)
        this.ConfigChangedCounter = new byte?(ByteArrayScanner.ScanByte(receivedFrame, ref offset));
      if (receivedFrame.Length == 14 || receivedFrame.Length > 17)
        this.StateNumber = new byte?(ByteArrayScanner.ScanByte(receivedFrame, ref offset));
      if (receivedFrame.Length < 17)
        return;
      this.AlarmValue = new uint?(BitConverter.ToUInt32(receivedFrame, offset));
    }

    public S4_FunctionalState(ushort functionalState) => this.FunctionalState = functionalState;

    public override string ToString()
    {
      StringBuilder info = new StringBuilder();
      info.Append("0x" + this.FunctionalState.ToString("x04"));
      int length = info.Length;
      if (this.FunctionalState == (ushort) 32768)
      {
        this.appendFlagToList(info, "invalid");
      }
      else
      {
        if (this.BatteryOver)
          this.appendFlagToList(info, "BatOver");
        if (this.BatteryWarning)
          this.appendFlagToList(info, "BatWarning");
        if (this.BatteryError)
          this.appendFlagToList(info, "BatError");
        if (this.AccuracyUnsafe)
          this.appendFlagToList(info, "Accuracy");
        if (this.HardwareError)
          this.appendFlagToList(info, "HardErr");
        if (this.EmptyTube)
          this.appendFlagToList(info, "EmptyTube");
        if (this.FlowOutOfRange)
          this.appendFlagToList(info, "FlowOutOfRange");
        if (this.Sleep)
          this.appendFlagToList(info, "Sleep");
        if (info.Length == length)
          this.appendFlagToList(info, "Ok");
      }
      return info.ToString();
    }

    private void appendFlagToList(StringBuilder info, string flag)
    {
      if (info.Length > 0)
        info.Append(',');
      info.Append(flag);
    }

    public string ToTextBlock()
    {
      if (this.FunctionalState == (ushort) 32768)
        return "State not valid";
      StringBuilder stringBuilder1 = new StringBuilder();
      DateTime? deviceTime = this.DeviceTime;
      if (deviceTime.HasValue)
      {
        StringBuilder stringBuilder2 = stringBuilder1;
        deviceTime = this.DeviceTime;
        string str = "Device time: " + deviceTime.Value.ToString("dd.MM.yyyy HH:mm:ss");
        stringBuilder2.AppendLine(str);
      }
      byte? nullable;
      if (this.ConfigChangedCounter.HasValue)
      {
        StringBuilder stringBuilder3 = stringBuilder1;
        nullable = this.ConfigChangedCounter;
        string str = "Configuration counter: " + nullable.Value.ToString();
        stringBuilder3.AppendLine(str);
      }
      nullable = this.StateNumber;
      if (nullable.HasValue)
      {
        StringBuilder stringBuilder4 = stringBuilder1;
        nullable = this.StateNumber;
        string str = "State number: " + nullable.Value.ToString();
        stringBuilder4.AppendLine(str);
      }
      int length = stringBuilder1.Length;
      stringBuilder1.AppendLine("Status flags:");
      if (this.BatteryOver)
        stringBuilder1.AppendLine("  BatteryOver");
      if (this.BatteryWarning)
        stringBuilder1.AppendLine("  BatteryWarning");
      if (this.BatteryError)
        stringBuilder1.AppendLine("  BatteryError");
      if (this.AccuracyUnsafe)
        stringBuilder1.AppendLine("  AccuracyUnsafe");
      if (this.HardwareError)
        stringBuilder1.AppendLine("  HardwareError");
      if (this.EmptyTube)
        stringBuilder1.AppendLine("  EmptyTube");
      if (this.FlowOutOfRange)
        stringBuilder1.AppendLine("  FlowOutOfRange");
      if (this.Sleep)
        stringBuilder1.AppendLine("  Sleep");
      if (length == stringBuilder1.Length)
        stringBuilder1.AppendLine("  No state is set");
      if (this.Alarm != null)
        stringBuilder1.AppendLine("Alarm:" + this.Alarm);
      return stringBuilder1.ToString();
    }
  }
}
