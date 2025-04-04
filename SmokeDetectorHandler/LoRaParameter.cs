// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.LoRaParameter
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public sealed class LoRaParameter
  {
    public byte[] JoinEUI { get; set; }

    public byte[] DevEUI { get; set; }

    public byte[] DevKey { get; set; }

    public byte[] NwkSKey { get; set; }

    public byte[] AppSKey { get; set; }

    public OTAA_ABP Activation { get; set; }

    public byte TransmissionScenario { get; set; }

    public byte ADR { get; set; }

    public byte[] ArmUniqueID { get; set; }

    public byte[] MBusKey { get; set; }

    public MBusChannelIdentification MBusIdent { get; set; }

    public MBusChannelIdentification MBusIdentRadio3 { get; set; }

    public bool RadioEnabled { get; set; }

    public ushort? Mbus_interval { get; set; }

    public SmokeDetectorHandlerFunctions.WeekDay? Mbus_radio_suppression_days { get; set; }

    public byte? Mbus_nighttime_start { get; set; }

    public byte? Mbus_nighttime_stop { get; set; }

    public int? CommunicationScenario { get; set; }

    public LoRaWANVersion LoRaWanVersion { get; set; }

    public LoRaFcVersion LoRaVersion { get; set; }

    public LoRaParameter()
    {
      this.MBusKey = new byte[16];
      this.MBusIdent = new MBusChannelIdentification();
      this.MBusIdentRadio3 = new MBusChannelIdentification();
    }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      byte[] array1 = new List<byte>((IEnumerable<byte>) this.JoinEUI).ToArray();
      Array.Reverse((Array) array1);
      byte[] array2 = new List<byte>((IEnumerable<byte>) this.DevEUI).ToArray();
      Array.Reverse((Array) array2);
      stringBuilder1.Append("LoRaWanVersion: ".PadRight(spaces, ' ')).AppendLine(this.LoRaWanVersion.ToString());
      stringBuilder1.Append("LoRaVersion: ".PadRight(spaces, ' ')).AppendLine(this.LoRaVersion.ToString());
      stringBuilder1.Append("DevEUI: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(array2));
      stringBuilder1.Append("JoinEUI: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(array1));
      stringBuilder1.Append("DevEUI: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(array2));
      stringBuilder1.Append("DevKey: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(this.DevKey));
      stringBuilder1.Append("NwkSKey: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(this.NwkSKey));
      stringBuilder1.Append("AppSKey: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(this.AppSKey));
      stringBuilder1.Append("Activation: ".PadRight(spaces, ' ')).AppendLine(this.Activation.ToString());
      stringBuilder1.Append("TransmissionScenario: ".PadRight(spaces, ' ')).AppendLine(this.TransmissionScenario.ToString());
      if (this.CommunicationScenario.HasValue)
        stringBuilder1.Append("CommunicationScenario: ".PadRight(spaces, ' ')).AppendLine(this.CommunicationScenario.ToString());
      StringBuilder stringBuilder2 = stringBuilder1.Append("ADR: ".PadRight(spaces, ' '));
      byte num = this.ADR;
      string str1 = num.ToString();
      stringBuilder2.AppendLine(str1);
      stringBuilder1.Append("ArmUniqueId: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(this.ArmUniqueID));
      if (!Util.ArraysEqual(this.MBusKey, new byte[16]))
        stringBuilder1.Append("MBusKey: ".PadRight(spaces, ' ')).AppendLine(Util.ByteArrayToHexString(this.MBusKey));
      StringBuilder stringBuilder3 = stringBuilder1.Append("Mbus_ID: ".PadRight(spaces, ' '));
      long serialNumber = this.MBusIdent.SerialNumber;
      string str2 = serialNumber.ToString();
      stringBuilder3.AppendLine(str2);
      StringBuilder stringBuilder4 = stringBuilder1.Append("Radio3_ID: ".PadRight(spaces, ' '));
      serialNumber = this.MBusIdentRadio3.SerialNumber;
      string str3 = serialNumber.ToString();
      stringBuilder4.AppendLine(str3);
      stringBuilder1.Append("Mbus_Manufacturer: ".PadRight(spaces, ' ')).AppendLine(this.MBusIdent.Manufacturer);
      StringBuilder stringBuilder5 = stringBuilder1.Append("Mbus_Generation: ".PadRight(spaces, ' '));
      num = this.MBusIdent.Generation;
      string str4 = num.ToString();
      stringBuilder5.AppendLine(str4);
      stringBuilder1.Append("Mbus_Medium: ".PadRight(spaces, ' ')).AppendLine(this.MBusIdent.Medium);
      stringBuilder1.Append("Radio Enabled: ".PadRight(spaces, ' ')).AppendLine(this.RadioEnabled.ToString());
      if (this.Mbus_interval.HasValue)
        stringBuilder1.Append("Mbus_interval: ".PadRight(spaces, ' ')).Append(this.Mbus_interval.ToString()).AppendLine(" sec");
      if (this.Mbus_radio_suppression_days.HasValue)
        stringBuilder1.Append("Mbus_radio_suppression_days: ".PadRight(spaces, ' ')).AppendLine(this.Mbus_radio_suppression_days.ToString());
      byte? nullable;
      if (this.Mbus_nighttime_start.HasValue)
      {
        StringBuilder stringBuilder6 = stringBuilder1.Append("Mbus_nighttime_start: ".PadRight(spaces, ' '));
        nullable = this.Mbus_nighttime_start;
        string str5 = nullable.ToString();
        stringBuilder6.AppendLine(str5);
      }
      nullable = this.Mbus_nighttime_stop;
      if (nullable.HasValue)
      {
        StringBuilder stringBuilder7 = stringBuilder1.Append("Mbus_nighttime_stop: ".PadRight(spaces, ' '));
        nullable = this.Mbus_nighttime_stop;
        string str6 = nullable.ToString();
        stringBuilder7.AppendLine(str6);
      }
      return stringBuilder1.ToString();
    }

    internal LoRaParameter DeepCopy()
    {
      return new LoRaParameter()
      {
        JoinEUI = this.JoinEUI,
        DevEUI = this.DevEUI,
        DevKey = this.DevKey,
        NwkSKey = this.NwkSKey,
        AppSKey = this.AppSKey,
        Activation = this.Activation,
        TransmissionScenario = this.TransmissionScenario,
        ADR = this.ADR,
        ArmUniqueID = this.ArmUniqueID,
        MBusKey = this.MBusKey,
        MBusIdent = this.MBusIdent,
        MBusIdentRadio3 = this.MBusIdentRadio3,
        RadioEnabled = this.RadioEnabled,
        Mbus_interval = this.Mbus_interval,
        Mbus_radio_suppression_days = this.Mbus_radio_suppression_days,
        Mbus_nighttime_start = this.Mbus_nighttime_start,
        Mbus_nighttime_stop = this.Mbus_nighttime_stop,
        CommunicationScenario = this.CommunicationScenario,
        LoRaWanVersion = this.LoRaWanVersion,
        LoRaVersion = this.LoRaVersion
      };
    }
  }
}
