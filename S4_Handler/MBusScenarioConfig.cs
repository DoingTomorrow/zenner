// Decompiled with JetBrains decompiler
// Type: S4_Handler.MBusScenarioConfig
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler
{
  public class MBusScenarioConfig
  {
    private string ModeOfOperation;
    private char FrameFormat;
    private char SecurityMode;
    private uint Frequency;
    private char FrequencyBand;
    private byte[] AES_Key;
    private uint TransmitionCycleSeconds;
    private uint NfcCycleSeconds;
    private char HeaderFormat;
    private RadioOffTimeManagement RadioOnSelection;
    private byte ParameterStructVersion;

    public MBusScenarioConfig(byte[] configData, byte parameterStructVersion, ref int scanOffset)
    {
      this.ParameterStructVersion = parameterStructVersion;
      this.ModeOfOperation = ((char) ByteArrayScanner.ScanByte(configData, ref scanOffset)).ToString() + (object) (char) ByteArrayScanner.ScanByte(configData, ref scanOffset);
      this.FrameFormat = (char) ByteArrayScanner.ScanByte(configData, ref scanOffset);
      this.SecurityMode = (char) ByteArrayScanner.ScanByte(configData, ref scanOffset);
      this.Frequency = ByteArrayScanner.ScanUInt32(configData, ref scanOffset);
      this.FrequencyBand = (char) ByteArrayScanner.ScanByte(configData, ref scanOffset);
      this.AES_Key = new byte[16];
      Buffer.BlockCopy((Array) configData, scanOffset, (Array) this.AES_Key, 0, this.AES_Key.Length);
      scanOffset += this.AES_Key.Length;
      this.TransmitionCycleSeconds = ByteArrayScanner.ScanUInt32(configData, ref scanOffset);
      this.HeaderFormat = (char) ByteArrayScanner.ScanByte(configData, ref scanOffset);
      this.RadioOnSelection = new RadioOffTimeManagement(configData, scanOffset);
      scanOffset += 7;
      if (parameterStructVersion <= (byte) 2)
        return;
      this.NfcCycleSeconds = ByteArrayScanner.ScanUInt32(configData, ref scanOffset);
    }

    public string ToTextBlock(string indent)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(indent + "ModeOfOperation: " + this.ModeOfOperation);
      stringBuilder.AppendLine(indent + "FrameFormat: " + this.FrameFormat.ToString());
      stringBuilder.AppendLine(indent + "SecurityMode: " + this.SecurityMode.ToString());
      stringBuilder.AppendLine(indent + "Frequency: " + this.Frequency.ToString());
      stringBuilder.AppendLine(indent + "FrequencyBand: " + this.FrequencyBand.ToString());
      stringBuilder.AppendLine(indent + "AES_Key: " + Util.ByteArrayToHexString(this.AES_Key));
      stringBuilder.AppendLine(indent + "TransmitionCycleSeconds: " + this.TransmitionCycleSeconds.ToString());
      stringBuilder.AppendLine(indent + "HeaderFormat: " + this.HeaderFormat.ToString());
      stringBuilder.AppendLine(indent + "RadioOnSelection: " + this.RadioOnSelection.ToString());
      if (this.ParameterStructVersion > (byte) 0)
        stringBuilder.AppendLine(indent + "NfcCycleSeconds: " + this.NfcCycleSeconds.ToString());
      return stringBuilder.ToString();
    }
  }
}
