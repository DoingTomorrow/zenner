// Decompiled with JetBrains decompiler
// Type: S4_Handler.LoRaScenarioConfig
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
  public class LoRaScenarioConfig
  {
    private ulong DevEUI;
    private ulong JoinEUI;
    private byte[] AppKey;
    private byte MinolDeviceType;
    private byte ParameterStructVersion;

    public LoRaScenarioConfig(byte[] configData, byte parameterStructVersion, ref int scanOffset)
    {
      this.ParameterStructVersion = parameterStructVersion <= (byte) 0 ? parameterStructVersion : throw new Exception("Not supported LoRa parameterStructVersion:" + parameterStructVersion.ToString());
      this.DevEUI = ByteArrayScanner.ScanUInt64(configData, ref scanOffset);
      this.JoinEUI = ByteArrayScanner.ScanUInt64(configData, ref scanOffset);
      this.AppKey = new byte[16];
      Buffer.BlockCopy((Array) configData, scanOffset, (Array) this.AppKey, 0, this.AppKey.Length);
      scanOffset += this.AppKey.Length;
      this.MinolDeviceType = ByteArrayScanner.ScanByte(configData, ref scanOffset);
    }

    public string ToTextBlock(string indent)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(indent + "DevEUI: 0x" + this.DevEUI.ToString("x016"));
      stringBuilder.AppendLine(indent + "JoinEUI: 0x" + this.JoinEUI.ToString("x016"));
      stringBuilder.AppendLine(indent + "AppKey: " + Util.ByteArrayToHexString(this.AppKey));
      stringBuilder.AppendLine(indent + "MinolDeviceType: " + this.MinolDeviceType.ToString());
      return stringBuilder.ToString();
    }
  }
}
